using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSAddOn
{
    [Serializable]
    public class AddOnBase
    {
        public int ID = 0;
        public int UseHz = 0;
        public int UseCore = 0;
        public bool Plus = false;
        public string[] infomasion = new string[3];
        public const int ManyColor = 8;
        public AddOnBase.ProgramColor color;
        protected const string texture = "menuwindows";
        public string name;

        public static AddOnBase AddOnSet(int number, int color)
        {
            Dictionary<int, AddOnBase> dictionary = new Dictionary<int, AddOnBase>
            {
                [0] = new BustorPower((AddOnBase.ProgramColor)color),
                [1] = new BustorRapid((AddOnBase.ProgramColor)color),
                [2] = new BustorCharge((AddOnBase.ProgramColor)color),
                [3] = new AssaultBuster((AddOnBase.ProgramColor)color),
                [4] = new BlueBuster((AddOnBase.ProgramColor)color),
                [5] = new ChageBypass((AddOnBase.ProgramColor)color),
                [6] = new BustorSet((AddOnBase.ProgramColor)color),
                [8] = new HPPlus50((AddOnBase.ProgramColor)color),
                [9] = new HPPlus100((AddOnBase.ProgramColor)color),
                [10] = new HPPlus200((AddOnBase.ProgramColor)color),
                [11] = new HPPlus500((AddOnBase.ProgramColor)color),
                [12] = new RichRich((AddOnBase.ProgramColor)color),
                [13] = new DataSalvage((AddOnBase.ProgramColor)color),
                [14] = new HPPlus300((AddOnBase.ProgramColor)color),
                [16] = new StockCharge((AddOnBase.ProgramColor)color),
                [17] = new DamageGhost((AddOnBase.ProgramColor)color),
                [18] = new FirstAdd((AddOnBase.ProgramColor)color),
                [19] = new FirstBarrier((AddOnBase.ProgramColor)color),
                [20] = new OneFullOpen((AddOnBase.ProgramColor)color),
                [21] = new AutoADD((AddOnBase.ProgramColor)color),
                [22] = new AutoCharge((AddOnBase.ProgramColor)color),
                [23] = new PeaceHeat((AddOnBase.ProgramColor)color),
                [24] = new PeaceAqua((AddOnBase.ProgramColor)color),
                [25] = new PeaceLeaf((AddOnBase.ProgramColor)color),
                [26] = new PeaceEleki((AddOnBase.ProgramColor)color),
                [27] = new PeacePoison((AddOnBase.ProgramColor)color),
                [28] = new PeaceEarth((AddOnBase.ProgramColor)color),
                [29] = new EriaGuard((AddOnBase.ProgramColor)color),
                [30] = new MyGarden((AddOnBase.ProgramColor)color),
                [31] = new Yuzuriai((AddOnBase.ProgramColor)color),
                [32] = new StyleReUse((AddOnBase.ProgramColor)color),
                [33] = new Haisui((AddOnBase.ProgramColor)color),
                [34] = new ChipSizeMinus((AddOnBase.ProgramColor)color),
                [35] = new ChipSizePlus((AddOnBase.ProgramColor)color),
                [36] = new NaviPlus((AddOnBase.ProgramColor)color),
                [37] = new DarkPlus((AddOnBase.ProgramColor)color),
                [38] = new UsedCure((AddOnBase.ProgramColor)color),
                [39] = new UsedPain((AddOnBase.ProgramColor)color),
                [40] = new Grimoire((AddOnBase.ProgramColor)color),
                [41] = new EscapeSoul((AddOnBase.ProgramColor)color),
                [42] = new UnShuffle((AddOnBase.ProgramColor)color),
                [43] = new AngerMind((AddOnBase.ProgramColor)color),
                [44] = new HardObject((AddOnBase.ProgramColor)color),
                [47] = new BuraiStyle((AddOnBase.ProgramColor)color),
                [48] = new ChipChanger((AddOnBase.ProgramColor)color),
                [49] = new BaisokuRunner((AddOnBase.ProgramColor)color),
                [50] = new KishiKaisei((AddOnBase.ProgramColor)color),
                [54] = new HumorSense((AddOnBase.ProgramColor)color),
                [57] = new EirinCall((AddOnBase.ProgramColor)color),
                [58] = new Bullet((AddOnBase.ProgramColor)color),
                [59] = new BulletBig((AddOnBase.ProgramColor)color),
                [61] = new RShield((AddOnBase.ProgramColor)color),
                [62] = new RPanelRepair((AddOnBase.ProgramColor)color),
                [63] = new RHoleMake((AddOnBase.ProgramColor)color),
                [64] = new LCube((AddOnBase.ProgramColor)color),
                [65] = new LMukaikaze((AddOnBase.ProgramColor)color),
                [66] = new LOikaze((AddOnBase.ProgramColor)color),
                [67] = new LBeastRock((AddOnBase.ProgramColor)color),
                [68] = new SlowStart((AddOnBase.ProgramColor)color),
                [69] = new NebulaHole((AddOnBase.ProgramColor)color),
                [70] = new HPDown100((AddOnBase.ProgramColor)color),
                [71] = new AcidBody((AddOnBase.ProgramColor)color),
                [72] = new HideLife((AddOnBase.ProgramColor)color),
                [73] = new LostLight((AddOnBase.ProgramColor)color),
                [74] = new MassatuSlip((AddOnBase.ProgramColor)color),
                [75] = new Meltingth((AddOnBase.ProgramColor)color),
                [76] = new CustomPain((AddOnBase.ProgramColor)color),
                [77] = new HeavyFoot((AddOnBase.ProgramColor)color),
                [80] = new PonkothuBuster((AddOnBase.ProgramColor)color),
                [81] = new OwataManBody((AddOnBase.ProgramColor)color),
                [82] = new NoGuard((AddOnBase.ProgramColor)color),
                [83] = new LostCustom((AddOnBase.ProgramColor)color),
                [84] = new BlackMind((AddOnBase.ProgramColor)color),
                [85] = new ParizeDamage((AddOnBase.ProgramColor)color),
                [86] = new CAuraSword((AddOnBase.ProgramColor)color),
                [87] = new CDustBomb((AddOnBase.ProgramColor)color),
                [88] = new CVulcan((AddOnBase.ProgramColor)color),
                [89] = new CFallKnife((AddOnBase.ProgramColor)color),
                [90] = new CBlastCanon((AddOnBase.ProgramColor)color),
                [91] = new CLance((AddOnBase.ProgramColor)color),
                [92] = new CRepair((AddOnBase.ProgramColor)color),
                [93] = new CShotGun((AddOnBase.ProgramColor)color),
                [94] = new FudouMyoou((AddOnBase.ProgramColor)color),
                [95] = new Scavenger((AddOnBase.ProgramColor)color),
                [96] = new Sacrifice((AddOnBase.ProgramColor)color),
                [97] = new Mammon((AddOnBase.ProgramColor)color),
            };
            return dictionary.ContainsKey(number) ? dictionary[number] : null;
        }

        public AddOnBase(AddOnBase.ProgramColor c)
        {
            this.color = c;
        }

        public virtual void Running(SaveData save)
        {
        }

        public virtual void Render(IRenderer dg, bool set, bool flash, Vector2 posi, AllBase all)
        {
            Rectangle _rect = new Rectangle();
            Vector2 _point = new Vector2();
            _rect = new Rectangle(480, 536, 136, 16);
            _point = new Vector2(posi.X + 8f, posi.Y);
            dg.DrawImage(dg, "menuwindows", _rect, true, _point, Color.White);
            _rect = new Rectangle(!flash ? 496 : 576, 16 * (int)this.color, 80, 16);
            _point = new Vector2(set ? posi.X + 8f : posi.X, posi.Y);
            dg.DrawImage(dg, "menuwindows", _rect, true, _point, Color.White);
            Vector2 position = new Vector2(set ? posi.X + 16f : posi.X + 8f, posi.Y);
            all.TextRender(dg, this.name, false, position, true);
            int[] numArray = all.ChangeCount(this.UseHz);
            position = new Vector2(posi.X + 92f, posi.Y);
            for (int index = 0; index < numArray.Length; ++index)
            {
                _rect = new Rectangle(numArray[index] * 8, 104, 8, 16);
                _point = new Vector2(position.X - index * 8, position.Y);
                dg.DrawImage(dg, "font", _rect, true, _point, this.color != AddOnBase.ProgramColor.dark ? Color.Yellow : Color.MediumPurple);
            }
            _rect = new Rectangle(this.UseCore * 8, 104, 8, 16);
            _point = new Vector2(posi.X + 120f, posi.Y);
            dg.DrawImage(dg, "font", _rect, true, _point, this.color != AddOnBase.ProgramColor.dark ? Color.Yellow : Color.MediumPurple);
        }

        public static string ColorToString(int color)
        {
            switch (color)
            {
                case 0:
                    return "";
                case 1:
                    return ShanghaiEXE.Translate("Addon.ShortPink");
                case 2:
                    return ShanghaiEXE.Translate("Addon.ShortSkyBlue");
                case 3:
                    return ShanghaiEXE.Translate("Addon.ShortRed");
                case 4:
                    return ShanghaiEXE.Translate("Addon.ShortBlue");
                case 5:
                    return ShanghaiEXE.Translate("Addon.ShortGreen");
                case 6:
                    return ShanghaiEXE.Translate("Addon.ShortOrange");
                case 7:
                    return ShanghaiEXE.Translate("Addon.ShortDark");
                default:
                    return "";
            }
        }

        public static string ColorToAlphabet(int color)
        {
            switch (color)
            {
                case 0:
                    return "";
                case 1:
                    return ShanghaiEXE.Translate("Addon.LetterPink");
                case 2:
                    return ShanghaiEXE.Translate("Addon.LetterSkyBlue");
                case 3:
                    return ShanghaiEXE.Translate("Addon.LetterRed");
                case 4:
                    return ShanghaiEXE.Translate("Addon.LetterBlue");
                case 5:
                    return ShanghaiEXE.Translate("Addon.LetterGreen");
                case 6:
                    return ShanghaiEXE.Translate("Addon.LetterOrange");
                case 7:
                    return ShanghaiEXE.Translate("Addon.LetterDark");
                default:
                    return "";
            }
        }

        public enum ProgramColor
        {
            glay,
            pink,
            sky,
            red,
            blue,
            gleen,
            yellow,
            dark,
        }
    }
}
