using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum BackgroundTypeNumber
	{
		[Description("Default")]
		BackDefault = 0,
		[Description("Solid Purple")]
		SolidPurple = 1,
		[Description("Solid Green")]
		SolidGreen = 2,
		[Description("Solid Sky Blue")]
		SolidSkyBlue = 3,
		[Description("Alice HP")]
		AriceBack = 4,
		[Description("Genso Area")]
		GenNetBack = 5,
		[Description("Refrigerator")]
		RefrigeratorBack = 6,
		[Description("City Area")]
		SityNetBack = 7,
		[Description("Air Cleaner")]
		AirBack = 8,
		[Description("Mari HP")]
		MariBack = 9,
		[Description("Remilia HP")]
		RemiHPback = 10,
		[Description("Rika HP")]
		rikaHPBack = 11,
		[Description("Eien Area")]
		EienBack = 12,
		[Description("Undernet")]
		UraBack = 13,
		[Description("School")]
		SchoolBack = 14,
		[Description("Solid Black")]
		SolidBlack = 15,
		[Description("NetAgent")]
		NAback = 16,
		[Description("Tournament")]
		TournamentBack = 17,
		[Description("Shrine")]
		JinjaBack = 18,
		[Description("Eien School")]
		EienSchoolBack = 19,
		[Description("Tsubaki HP")]
		NeckBack = 20,
		[Description("Cruise")]
		SeirenNetBack = 21,
		[Description("Cruise (flooded)")]
		SeirenNetBack2 = 22,
		[Description("Engelles Area")]
		IngBack = 23,
		[Description("Hotel")]
		HotelBack = 24,
		[Description("Clocktower")]
		ClockTowerBack = 25,
		[Description("Undernet (Deep)")]
		UraBack2 = 26,
		[Description("Border Concern")]
		CompanyServerBack = 27,
		[Description("Tenshi HP")]
		TenshiBack = 28,
		[Description("Default (dupe)")]
		BackDefault2 = 29,
		[Description("Deep Sea")]
		LostShipBack = 30,
		[Description("ROM (Blue)")]
		ROMBlue = 31,
		[Description("ROM (Green)")]
		ROMGreen = 32,
		[Description("ROM (Yellow)")]
		ROMYellow = 33,
		[Description("ROM (Purple)")]
		ROMPurple = 34,
		[Description("Hospital")]
		HospitalBack = 35,
		[Description("Heaven")]
		HeavenBack = 36,
        [Description("Final Floor")]
        FinalFloorBack = 37,
        [Description("Central")]
        Central = 38,
        [Description("Heaven Water (cyan)")]
        HeavenWater = 39,
        [Description("Deep Undernet")]
        DeepUra = 40
    }
}
