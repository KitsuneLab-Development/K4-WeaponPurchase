using CounterStrikeSharp.API.Core;
using System.Collections.Immutable;

namespace K4WeaponPurchase
{
	public static class WeaponConstants
	{
		public const int INVALID_WEAPON_ID = -1;
		public const int DEFAULT_MAX_PLAYER_SPEED = 250;
		public const float DEFAULT_CYCLE_TIME = 0.15f;
	}

	public class WeaponInfo
	{
		public ushort Id { get; }
		public string ClassName { get; }
		public string DisplayName { get; }
		public gear_slot_t Slot { get; }
		public ImmutableList<string> Aliases { get; }
		public int Price { get; }

		public WeaponInfo(ushort id, string className, string displayName, gear_slot_t slot, IEnumerable<string> aliases, int price)
		{
			Id = id;
			ClassName = className;
			DisplayName = displayName;
			Slot = slot;
			Aliases = aliases.ToImmutableList();
			Price = price;
		}

		public bool IsPrimary => Slot == gear_slot_t.GEAR_SLOT_RIFLE;
		public bool IsSecondary => Slot == gear_slot_t.GEAR_SLOT_PISTOL;
		public bool IsGrenade => Slot == gear_slot_t.GEAR_SLOT_GRENADES;
		public bool IsUtility => Slot == gear_slot_t.GEAR_SLOT_UTILITY;
		public bool IsWeapon => IsPrimary || IsSecondary;
	}

	public static class WeaponDatabase
	{
		public static ImmutableList<WeaponInfo> Weapons { get; } =
		[
			new WeaponInfo(60, "weapon_m4a1_silencer", "M4A1-S", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "m4a1s", "m4s" }, 2900),
			new WeaponInfo(40, "weapon_ssg08", "SSG 08", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "ssg", "scout", "ssg08" }, 1700),
			new WeaponInfo(39, "weapon_sg556", "SG 553", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "sg556", "sg" }, 3000),
			new WeaponInfo(38, "weapon_scar20", "SCAR-20", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "scar20", "scar" }, 5000),
			new WeaponInfo(35, "weapon_nova", "Nova", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "nova" }, 1050),
			new WeaponInfo(34, "weapon_mp9", "MP9", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "mp9" }, 1250),
			new WeaponInfo(33, "weapon_mp7", "MP7", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "mp7" }, 1500),
			new WeaponInfo(29, "weapon_sawedoff", "Sawed-Off", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "sawedoff" }, 1100),
			new WeaponInfo(28, "weapon_negev", "Negev", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "negev" }, 1700),
			new WeaponInfo(27, "weapon_mag7", "MAG-7", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "mag7", "mag" }, 1300),
			new WeaponInfo(26, "weapon_bizon", "PP-Bizon", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "bizon" }, 1400),
			new WeaponInfo(25, "weapon_xm1014", "XM1014", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "xm1014", "xm" }, 2000),
			new WeaponInfo(24, "weapon_ump45", "UMP-45", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "ump45", "ump" }, 1200),
			new WeaponInfo(23, "weapon_mp5sd", "MP5-SD", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "mp5sd", "mp5" }, 1500),
			new WeaponInfo(19, "weapon_p90", "P90", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "p90" }, 2350),
			new WeaponInfo(17, "weapon_mac10", "MAC-10", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "mac10", "mac" }, 1050),
			new WeaponInfo(16, "weapon_m4a1", "M4A4", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "m4a1", "m4", "m4a4" }, 3100),
			new WeaponInfo(14, "weapon_m249", "M249", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "m249" }, 5200),
			new WeaponInfo(13, "weapon_galilar", "Galil AR", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "galilar", "galil" }, 1800),
			new WeaponInfo(11, "weapon_g3sg1", "G3SG1", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "g3sg1" }, 5000),
			new WeaponInfo(10, "weapon_famas", "FAMAS", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "famas" }, 2050),
			new WeaponInfo(9, "weapon_awp", "AWP", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "awp" }, 4750),
			new WeaponInfo(8, "weapon_aug", "AUG", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "aug" }, 3300),
			new WeaponInfo(7, "weapon_ak47", "AK-47", gear_slot_t.GEAR_SLOT_RIFLE, new[] { "ak47", "ak" }, 2700),
			new WeaponInfo(64, "weapon_revolver", "R8 Revolver", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "revolver", "r8revolver", "r8" }, 600),
			new WeaponInfo(63, "weapon_cz75a", "CZ75-Auto", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "cz75a", "cz", "cs75-auto" }, 500),
			new WeaponInfo(61, "weapon_usp_silencer", "USP-S", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "usp_silencer", "usp", "usp-s" }, 200),
			new WeaponInfo(36, "weapon_p250", "P250", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "p250" }, 300),
			new WeaponInfo(32, "weapon_hkp2000", "P2000", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "hkp2000", "hkp", "p2000" }, 200),
			new WeaponInfo(30, "weapon_tec9", "Tec-9", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "tec9", "tec" }, 500),
			new WeaponInfo(4, "weapon_glock", "Glock-18", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "glock", "glock18" }, 200),
			new WeaponInfo(3, "weapon_fiveseven", "Five-SeveN", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "fiveseven" }, 500),
			new WeaponInfo(2, "weapon_elite", "Dual Berettas", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "elite", "dualberettas" }, 300),
			new WeaponInfo(1, "weapon_deagle", "Desert Eagle", gear_slot_t.GEAR_SLOT_PISTOL, new[] { "deagle" }, 700),
			new WeaponInfo(62, "weapon_taser", "Taser", gear_slot_t.GEAR_SLOT_UTILITY, new[] { "taser" }, 200),
			new WeaponInfo(58, "weapon_shield", "Shield", gear_slot_t.GEAR_SLOT_UTILITY, new[] { "shield" }, 0),
			new WeaponInfo(57, "weapon_healthshot", "Healthshot", gear_slot_t.GEAR_SLOT_UTILITY, new[] { "healthshot" }, 0),
			new WeaponInfo(50, "item_kevlar", "Kevlar Vest", gear_slot_t.GEAR_SLOT_UTILITY, new[] { "kevlar" }, 650),
			new WeaponInfo(31, "weapon_flashbang", "Flashbang", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "flashbang", "flash" }, 200),
			new WeaponInfo(20, "weapon_smokegrenade", "Smoke Grenade", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "smokegrenade", "smoke" }, 300),
			new WeaponInfo(18, "weapon_molotov", "Molotov", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "molotov" }, 400),
			new WeaponInfo(15, "weapon_hegrenade", "HE Grenade", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "hegrenade", "grenade", "he" }, 300),
			new WeaponInfo(37, "weapon_incgrenade", "Incendiary Grenade", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "incgrenade" }, 600),
			new WeaponInfo(41, "weapon_decoy", "Decoy Grenade", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "decoy" }, 50),
			new WeaponInfo(42, "weapon_tagrenade", "Tactical Grenade", gear_slot_t.GEAR_SLOT_GRENADES, new[] { "tagrenade" }, 100)
,
		];

		public static WeaponInfo? GetWeaponByClassName(string className)
		{
			return Weapons.FirstOrDefault(w => w.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));
		}

		public static WeaponInfo? GetWeaponByAlias(string alias)
		{
			return Weapons.FirstOrDefault(w => w.Aliases.Contains(alias, StringComparer.OrdinalIgnoreCase));
		}
	}
}