using NSAddOn;
using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class AddOnBaseExtensionMethods
    {
        private static Dictionary<string, Type> AddOns;

        private static Dictionary<Type, string> Names;

        static AddOnBaseExtensionMethods()
        {
            AddOns = new Dictionary<string, Type>
            {
                { "AcidBody", typeof(AcidBody) },
                { "AngerMind", typeof(AngerMind) },
                { "AssaultBuster", typeof(AssaultBuster) },
                { "AutoADD", typeof(AutoADD) },
                { "AutoCharge", typeof(AutoCharge) },
                { "BaisokuRunner", typeof(BaisokuRunner) },
                { "BlackMind", typeof(BlackMind) },
                { "BlueBuster", typeof(BlueBuster) },
                { "Bullet", typeof(Bullet) },
                { "BulletBig", typeof(BulletBig) },
                { "BuraiStyle", typeof(BuraiStyle) },
                { "BustorCharge", typeof(BustorCharge) },
                { "BustorPower", typeof(BustorPower) },
                { "BustorRapid", typeof(BustorRapid) },
                { "BustorSet", typeof(BustorSet) },
                { "CAuraSword", typeof(CAuraSword) },
                { "CBlastCanon", typeof(CBlastCanon) },
                { "CDustBomb", typeof(CDustBomb) },
                { "CFallKnife", typeof(CFallKnife) },
                { "ChageBypass", typeof(ChageBypass) },
                { "ChipChanger", typeof(ChipChanger) },
                { "ChipSizeMinus", typeof(ChipSizeMinus) },
                { "ChipSizePlus", typeof(ChipSizePlus) },
                { "CLance", typeof(CLance) },
                { "CRepair", typeof(CRepair) },
                { "CShotGun", typeof(CShotGun) },
                { "CustomPain", typeof(CustomPain) },
                { "CVulcan", typeof(CVulcan) },
                { "DamageGhost", typeof(DamageGhost) },
                { "DarkPlus", typeof(DarkPlus) },
                { "DataSalvage", typeof(DataSalvage) },
                { "EirinCall", typeof(EirinCall) },
                { "EriaGuard", typeof(EriaGuard) },
                { "EscapeSoul", typeof(EscapeSoul) },
                { "FirstAdd", typeof(FirstAdd) },
                { "FirstBarrier", typeof(FirstBarrier) },
                { "FudouMyoou", typeof(FudouMyoou) },
                { "Grimoire", typeof(Grimoire) },
                { "Haisui", typeof(Haisui) },
                { "HardObject", typeof(HardObject) },
                { "HeavyFoot", typeof(HeavyFoot) },
                { "HideLife", typeof(HideLife) },
                { "HPDown100", typeof(HPDown100) },
                { "HPPlus100", typeof(HPPlus100) },
                { "HPPlus200", typeof(HPPlus200) },
                { "HPPlus300", typeof(HPPlus300) },
                { "HPPlus50", typeof(HPPlus50) },
                { "HPPlus500", typeof(HPPlus500) },
                { "HumorSense", typeof(HumorSense) },
                { "KishiKaisei", typeof(KishiKaisei) },
                { "LBeastRock", typeof(LBeastRock) },
                { "LCube", typeof(LCube) },
                { "LMukaikaze", typeof(LMukaikaze) },
                { "LOikaze", typeof(LOikaze) },
                { "LostCustom", typeof(LostCustom) },
                { "LostLight", typeof(LostLight) },
                { "MassatuSlip", typeof(MassatuSlip) },
                { "Meltingth", typeof(Meltingth) },
                { "MyGarden", typeof(MyGarden) },
                { "NaviPlus", typeof(NaviPlus) },
                { "NebulaHole", typeof(NebulaHole) },
                { "NoGuard", typeof(NoGuard) },
                { "OneFullOpen", typeof(OneFullOpen) },
                { "OwataManBody", typeof(OwataManBody) },
                { "ParizeDamage", typeof(ParizeDamage) },
                { "PeaceAqua", typeof(PeaceAqua) },
                { "PeaceEarth", typeof(PeaceEarth) },
                { "PeaceEleki", typeof(PeaceEleki) },
                { "PeaceHeat", typeof(PeaceHeat) },
                { "PeaceLeaf", typeof(PeaceLeaf) },
                { "PeacePoison", typeof(PeacePoison) },
                { "PonkothuBuster", typeof(PonkothuBuster) },
                { "RHoleMake", typeof(RHoleMake) },
                { "RichRich", typeof(RichRich) },
                { "RPanelRepair", typeof(RPanelRepair) },
                { "RShield", typeof(RShield) },
                { "SlowStart", typeof(SlowStart) },
                { "StockCharge", typeof(StockCharge) },
                { "StyleReUse", typeof(StyleReUse) },
                { "UnShuffle", typeof(UnShuffle) },
                { "UsedCure", typeof(UsedCure) },
                { "UsedPain", typeof(UsedPain) },
                { "Yuzuriai", typeof(Yuzuriai) }
            };

            Names = new Dictionary<Type, string>();
            foreach (var kvp in AddOns)
            {
                Names[kvp.Value] = kvp.Key;
            }
        }

        public static Type ToAddOnType(this string name) => AddOns[name];

        public static string ToAddOnName(this Type type) => $"Addon.{Names[type]}";
    }
}
