using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace K4WeaponPurchase
{
	public sealed class PluginConfig : BasePluginConfig
	{
		[JsonPropertyName("AllowedWeapons")]
		public List<string> AllowedWeapons { get; set; } = new List<string>() { "weapon_ak47", "weapon_m4a1_silencer", "weapon_m4a1", "weapon_revolver", "weapon_healthshot" };

		[JsonPropertyName("CustomPrices")]
		public Dictionary<string, int> CustomPrices { get; set; } = new Dictionary<string, int>()
		{
			{ "weapon_healthshot", 1000 }
		};

		[JsonPropertyName("ConfigVersion")]
		public override int Version { get; set; } = 1;
	}

	[MinimumApiVersion(250)]
	public class Plugin : BasePlugin, IPluginConfig<PluginConfig>
	{
		public override string ModuleName => "Weapon Purchase";
		public override string ModuleAuthor => "K4ryuu";
		public override string ModuleDescription => "Purchase weapons, grenades and more through commands";
		public override string ModuleVersion => "1.0.1";

		public required PluginConfig Config { get; set; } = new PluginConfig();

		public void OnConfigParsed(PluginConfig config)
		{
			if (config.Version < Config.Version)
				base.Logger.LogWarning("Configuration version mismatch (Expected: {0} | Current: {1})", this.Config.Version, config.Version);
			this.Config = config;
		}

		public override void Load(bool hotReload)
		{
			WeaponDatabase.Weapons.Where(w => Config.AllowedWeapons.Any(cw => w.ClassName == cw)).ToList().ForEach(w =>
			{
				int price = Config.CustomPrices.TryGetValue(w.ClassName, out int customPrice) ? customPrice : w.Price;

				w.Aliases.ForEach(alias =>
				{
					AddCommand($"css_{alias}", $"Purchase {w.DisplayName}", (controller, info) =>
					{
						if (controller is null || controller.InGameMoneyServices is null)
							return;

						if (controller.PlayerPawn.Value?.Health <= 0 || controller.Team <= CsTeam.Spectator)
						{
							controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.dead"]}");
							return;
						}

						if (controller.InGameMoneyServices.Account < price)
						{
							controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.not_enough_money", price]}");
							return;
						}

						if (w.Slot == gear_slot_t.GEAR_SLOT_GRENADES)
						{
							int grenadeLimit = ConVar.Find("ammo_grenade_limit_total")!.GetPrimitiveValue<int>();
							var grenades = GetItems(controller, slot: gear_slot_t.GEAR_SLOT_GRENADES);
							if (grenades.Count >= grenadeLimit)
							{
								controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.grenade_limit", grenadeLimit]}");
								return;
							}
							if (w.ClassName == "weapon_flashbang")
							{
								int flashbangLimit = ConVar.Find("ammo_grenade_limit_flashbang")!.GetPrimitiveValue<int>();
								if (grenades.Count(g => g == "weapon_flashbang") >= flashbangLimit)
								{
									controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.flashbang_limit", flashbangLimit]}");
									return;
								}
							}
							else
							{
								int defaultLimit = ConVar.Find("ammo_grenade_limit_default")!.GetPrimitiveValue<int>();
								if (grenades.Count(g => g == w.ClassName) >= defaultLimit)
								{
									controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.default_limit", defaultLimit]}");
									return;
								}
							}
						}
						else if (w.ClassName == "weapon_healthshot")
						{
							int healthshotLimit = ConVar.Find("ammo_item_limit_healthshot")!.GetPrimitiveValue<int>();
							var healthshots = GetItems(controller, className: "weapon_healthshot");
							if (healthshots.Count >= healthshotLimit)
							{
								controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.healthshot_limit", healthshotLimit]}");
								return;
							}
						}
						else if (w.IsWeapon)
						{
							DropSlot(controller, w.Slot);
						}
						else
						{
							DropSlot(controller, className: w.ClassName);
						}

						controller.InGameMoneyServices!.Account -= price;
						Utilities.SetStateChanged(controller, "CCSPlayerController", "m_pInGameMoneyServices");
						controller.GiveNamedItem(w.ClassName);
						controller.PrintToChat($" {Localizer["k4.general.prefix"]} {Localizer["k4.weaponpurchase.purchase_success", w.DisplayName, price]}");
					});
				});
			});
		}

		private static void DropSlot(CCSPlayerController player, gear_slot_t? slot = null, string? className = null)
		{
			if (player.PlayerPawn.Value?.WeaponServices is null)
				return;

			List<CHandle<CBasePlayerWeapon>> weaponList = [.. player.PlayerPawn.Value.WeaponServices.MyWeapons];
			foreach (CHandle<CBasePlayerWeapon> weapon in weaponList)
			{
				if (weapon.IsValid && weapon.Value != null)
				{
					CCSWeaponBase ccsWeaponBase = weapon.Value.As<CCSWeaponBase>();
					if (ccsWeaponBase?.IsValid == true)
					{
						CCSWeaponBaseVData? weaponData = ccsWeaponBase.VData;
						if (weaponData == null)
							continue;

						if ((slot.HasValue && weaponData.GearSlot == slot) ||
							(className != null && ccsWeaponBase.DesignerName == className))
						{
							player.PlayerPawn.Value.WeaponServices.ActiveWeapon.Raw = weapon.Raw;
							player.DropActiveWeapon();
							break;
						}
					}
				}
			}
		}

		private List<string> GetItems(CCSPlayerController player, gear_slot_t? slot = null, string? className = null)
		{
			List<string> items = new();
			if (player.PlayerPawn.Value?.WeaponServices is null)
				return items;

			List<CHandle<CBasePlayerWeapon>> weaponList = [.. player.PlayerPawn.Value.WeaponServices.MyWeapons];
			foreach (CHandle<CBasePlayerWeapon> weapon in weaponList)
			{
				if (weapon.IsValid && weapon.Value != null)
				{
					CCSWeaponBase ccsWeaponBase = weapon.Value.As<CCSWeaponBase>();
					if (ccsWeaponBase?.IsValid == true)
					{
						CCSWeaponBaseVData? weaponData = ccsWeaponBase.VData;

						if (weaponData == null || (slot != null && weaponData.GearSlot != slot) || (className != null && !ccsWeaponBase.DesignerName.Contains(className)))
							continue;

						if (ccsWeaponBase.DesignerName == "weapon_healthshot")
						{
							for (int i = 0; i < player.PlayerPawn.Value.WeaponServices.Ammo[20]; i++)
								items.Add(ccsWeaponBase.DesignerName);
						}
						else
							items.Add(ccsWeaponBase.DesignerName);
					}
				}
			}

			return items;
		}
	}
}