using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSNet;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSChip
{
    public class ChipBase : AllBase
    {
        public string name = "";
        public ChipBase.ELEMENT element = ChipBase.ELEMENT.normal;
        public int power = 0;
        public int subpower = 0;
        public int pluspower = 0;
        public int regsize = 0;
        public byte reality = 1;
        public bool swordtype = false;
        public int number = 0;
        public ChipFolder.CODE[] code = new ChipFolder.CODE[4];
        public bool dark = false;
        public bool navi = false;
        public bool printIcon = true;
        public bool shadow = true;
        public string[] information = new string[3];
        public float sortNumber = -1f;
        public Point rockOnPoint;
        public bool infight;
        public bool sideOnly;
        public bool paralyze;
        public bool _break;
        public bool plusing;
        public bool shild;
        public bool obje;
        public bool powerprint;
        protected int flamesub;
        public bool inviNo;
        public bool chipUseEnd;
        public bool crack;
        public bool chargeFlag;
        public int[] randomSeed;
        private int randomSeedUse;
        public const int manySeeds = 10;
        public int userNum;
        private int BlackOutFlame;
        public bool blackOutInit;
        private string blackOutName;
        private string blackOutPower;
        public bool blackOutLend;
        public bool timeStopper;
        private bool boEndOK1;
        private bool boEndOK2;
        public string powertxt;
        public int nameAlpha;
        public string libraryDisplayId;

        public ChipBase(IAudioEngine s)
          : base(s)
        {
            for (int index = 0; index < this.information.Length; ++index)
                this.information[index] = "";
            this.randomSeed = new int[10];
            for (int index = 0; index < this.randomSeed.Length; ++index)
                this.randomSeed[index] = this.Random.Next(100);
        }

        public int RandomSeedUse()
        {
            int num = this.randomSeed[this.randomSeedUse];
            this.randomSeed[this.randomSeedUse] *= this.randomSeedUse + 1;
            if (this.randomSeed[this.randomSeedUse] > 100000)
                this.randomSeed[this.randomSeedUse] %= 100000;
            ++this.randomSeedUse;
            if (this.randomSeedUse >= this.randomSeed.Length)
                this.randomSeedUse = 0;
            return num;
        }

        public virtual void Init()
        {
            if (this.navi)
                this.timeStopper = true;
            this.sortNumber = number;
        }

        public int Power(CharacterBase character)
        {
            int num = this.power + this.pluspower;
            if (num > 0)
                return !character.badstatus[1] ? num : num / 2;
            return 0;
        }

        public string PowerToString(CharacterBase character)
        {
            int num = this.Power(character);
            if (num > 0)
                return num.ToString();
            return "";
        }

        public int UnionRebirth(Panel.COLOR union)
        {
            return union == Panel.COLOR.red ? 1 : -1;
        }

        public void ActionBO(CharacterBase character, SceneBattle battle)
        {
            if (!this.chipUseEnd)
            {
                this.Action(character, battle);
                ++character.waittime;
            }
            else
                this.ActionEnd(character, battle);
        }

        public virtual void Action(CharacterBase character, SceneBattle battle)
        {
            character.animationpoint.X = 0;
            character.animationpoint.Y = 0;
            character.waittime = 0;
            this.chipUseEnd = true;
        }

        public virtual void ActionEnd(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOutEnd(character, battle))
                return;
            if ((uint)character.step > 0U)
            {
                character.flying = character.flyflag;
                if (character.step == CharacterBase.STEP.shadow)
                    character.nohit = false;
                character.step = CharacterBase.STEP.none;
                character.position = character.stepPosition;
                character.PositionDirectSet();
                character.flying = character.flyflag;
            }
            if (character is Player)
            {
                Player player = (Player)character;
                if ((uint)this.element > 0U)
                    player.PluspointWitch(4);
                if (this.swordtype)
                    player.PluspointShinobi(14);
                character.animationpoint = new Point(0, 0);
                player.motion = Player.PLAYERMOTION._neutral;
            }
            else if (character is ChipUsingNaviBase)
            {
                var chipUsingNavi = (ChipUsingNaviBase)character;
                chipUsingNavi.Motion = NaviBase.MOTION.neutral;
                chipUsingNavi.counterTiming = false;
            }
            character.animationpoint = new Point(0, 0);
        }

        public virtual void HaveUpdate(CharacterBase character)
        {
            this.userNum = character.number;
        }

        public virtual void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (!printstatus)
                return;
            int[] numArray = this.ChangeCount(this.power);
            this._rect = new Rectangle(216, 104, 56, 16);
            this._position = new Vector2(p.X, p.Y + 48f);
            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(216 + (int)this.element * 16, 88, 16, 16);
            this._position = new Vector2(p.X + 8f, p.Y + 48f);
            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
            int num1 = 0;
            if (this.swordtype && num1 < 16)
            {
                this._rect = new Rectangle(336, 88, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (this._break && num1 < 16)
            {
                this._rect = new Rectangle(328, 96, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (this.crack && num1 < 16)
            {
                this._rect = new Rectangle(336, 96, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (this.plusing && num1 < 16)
            {
                this._rect = new Rectangle(552, 80, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (this.shild && num1 < 16)
            {
                this._rect = new Rectangle(552, 88, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (this.obje && num1 < 16)
            {
                this._rect = new Rectangle(560, 80, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (num1 < 16)
            {
                this._rect = new Rectangle(328, 88, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                num1 += 8;
            }
            if (num1 < 16)
            {
                this._rect = new Rectangle(328, 88, 8, 8);
                this._position = new Vector2(p.X + 23f, p.Y + 48f + num1);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                int num2 = num1 + 8;
            }
            if (this.powerprint)
            {
                for (int index = 0; index < numArray.Length; ++index)
                {
                    this._rect = new Rectangle(numArray[index] * 8, 0, 8, 16);
                    this._position = new Vector2(p.X + 47f - index * 8, p.Y + 48f);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
            }
            this._rect = new Rectangle((int)this.code[c] * 8, 48, 8, 16);
            this._position = new Vector2(p.X, p.Y + 48f);
            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
        }

        public Point[] GetRandamPanel(
          int many,
          Panel.COLOR union,
          bool underchara,
          CharacterBase character,
          bool hole)
        {
            SceneBattle parent = character.parent;
            List<Point> pointList = new List<Point>();
            List<int> intList = new List<int>();
            List<Vector3> source = new List<Vector3>();
            if (union == Panel.COLOR.blue)
            {
                for (int x = parent.panel.GetLength(0) - 1; x >= 0; --x)
                {
                    for (int y = 0; y < parent.panel.GetLength(1); ++y)
                    {
                        if (parent.panel[x, y].color == union && (hole || !parent.panel[x, y].Hole))
                        {
                            if (underchara)
                                source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                            else if (!parent.OnPanelCheck(x, y, false))
                                source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < parent.panel.GetLength(0); ++x)
                {
                    for (int y = 0; y < parent.panel.GetLength(1); ++y)
                    {
                        if (parent.panel[x, y].color == union && (hole || !parent.panel[x, y].Hole))
                        {
                            if (underchara)
                                source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                            else if (!parent.OnPanelCheck(x, y, false))
                                source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                        }
                    }
                }
            }
            List<Vector3> list = source.OrderByDescending<Vector3, float>(n => n.Z).ToList<Vector3>();
            int index = 0;
            while (pointList.Count < many)
            {
                pointList.Add(new Point((int)list[index].X, (int)list[index].Y));
                ++index;
                if (index >= list.Count)
                    index = 0;
            }
            return pointList.ToArray();
        }

        public virtual void IconRender(
          IRenderer dg,
          Vector2 p,
          bool select,
          bool custom,
          int c,
          bool noicon)
        {
            if (!custom)
                return;
            this._rect = new Rectangle((int)this.code[c] * 8, 64, 8, 8);
            this._position = new Vector2(p.X + 4f, p.Y + 16f);
            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
        }

        public Rectangle IconRect(bool select)
        {
            int num1 = this.number - 1;
            int num2 = num1 % 40;
            int num3 = num1 / 40;
            int num4 = 0;
            if (select)
                num4 = 1;
            return new Rectangle(num2 * 16, num3 * 16 + num4 * 96, 16, 16);
        }

        public virtual void Render(IRenderer dg, CharacterBase player)
        {
        }

        protected Point RandomTarget(CharacterBase character, SceneBattle battle)
        {
            List<Point> pointList = new List<Point>();
            foreach (Player player in battle.players)
            {
                if (player.union != character.union)
                    pointList.Add(player.position);
            }
            foreach (EnemyBase enemy in battle.enemys)
            {
                if (enemy.union != character.union)
                    pointList.Add(enemy.position);
            }
            try
            {
                int index = this.RandomSeedUse() % pointList.Count;
                return pointList[index];
            }
            catch
            {
                return this.RandomPanel(false, false, false, character.UnionEnemy, battle, 0);
            }
        }

        protected Point RandomPanel(
          bool near,
          bool target,
          bool froat,
          Panel.COLOR union,
          SceneBattle parent,
          int seed)
        {
            List<Point> pointList = new List<Point>();
            if (union == Panel.COLOR.blue)
            {
                for (int x = parent.panel.GetLength(0) - 1; x >= 0; --x)
                {
                    for (int y = 0; y < parent.panel.GetLength(1); ++y)
                    {
                        if (parent.panel[x, y].color == union)
                            pointList.Add(new Point(x, y));
                    }
                }
            }
            else
            {
                for (int x = 0; x < parent.panel.GetLength(0); ++x)
                {
                    for (int y = 0; y < parent.panel.GetLength(1); ++y)
                    {
                        if (parent.panel[x, y].color == union)
                            pointList.Add(new Point(x, y));
                    }
                }
            }
            if (near)
            {
                int[] n = new int[3];
                for (int j = 0; j < parent.panel.GetLength(1); j++)
                {
                    n[j] = union == Panel.COLOR.red ? 0 : 7;
                    foreach (Point point in pointList)
                    {
                        if (point.Y == j)
                        {
                            if (union == Panel.COLOR.blue && n[j] > point.X)
                                n[j] = point.X;
                            else if (union == Panel.COLOR.red && n[j] < point.X)
                                n[j] = point.X;
                        }
                    }
                    if (union == Panel.COLOR.blue)
                        pointList.RemoveAll(c =>
                       {
                           if (c.Y == j)
                               return c.X > n[j];
                           return false;
                       });
                    else
                        pointList.RemoveAll(c =>
                       {
                           if (c.Y == j)
                               return c.X < n[j];
                           return false;
                       });
                }
            }
            return pointList[this.RandomSeedUse() % pointList.Count];
        }

        public bool BlackOut(CharacterBase character, SceneBattle parent, string name, string power)
        {
            if (this.BlackOutFlame <= 60)
            {
                if (!this.blackOutInit)
                {
                    this.boEndOK1 = false;
                    this.boEndOK2 = false;
                    this.blackOutLend = false;
                    this.blackOutName = name;
                    this.blackOutPower = !(power != "0") ? "" : power;
                    this.userNum = character.number;
                    parent.blackOutChips.Insert(0, this);
                    if (parent.blackOutChips.Count >= 3)
                    {
                        parent.blackOutStopper = true;
                        parent.blackOutChips.RemoveAt(2);
                    }
                    if (parent.blackOut)
                        parent.effects.Add(new WeakPoint(this.sound, parent, new Vector2(120f, 48f), new Point()));
                    this.blackOutInit = true;
                    parent.blackOut = true;
                    parent.blackOuter = character.union;
                    parent.backscreencolor = 0;
                    parent.backscreen = 0;
                    this.nameAlpha = 0;
                }
                if (parent.backscreen < 100)
                    parent.backscreen += 10;
                if (this.nameAlpha < byte.MaxValue && this.BlackOutFlame >= 10 && this.BlackOutFlame < 20)
                    this.nameAlpha += 51;
                if (this.BlackOutFlame == 5)
                    this.sound.PlaySE(SoundEffect.barrier);
                if (this.nameAlpha > 0 && this.BlackOutFlame >= 50)
                {
                    parent.blackOutStopper = true;
                    this.nameAlpha -= 51;
                }
                ++this.BlackOutFlame;
                if (this.BlackOutFlame > 60)
                    character.waittime = -1;
                return false;
            }
            this.blackOutLend = true;
            return true;
        }

        public bool BlackOutEnd(CharacterBase character, SceneBattle parent)
        {
            if (true /* prev: if not netbattle */ || this.boEndOK2)
            {
                if (parent.blackOutChips.Count > 1)
                {
                    parent.blackOutChips.RemoveAt(0);
                    foreach (CharacterBase characterBase in parent.AllChara())
                    {
                        if (characterBase.number == parent.blackOutChips[0].userNum)
                            characterBase.waittime = 0;
                    }
                    return true;
                }
                if (character.animationpoint.X < 0)
                {
                    character.animationpoint.X = 0;
                    this.nameAlpha = 0;
                }
                if (parent.backscreen > 0)
                {
                    parent.backscreen -= 10;
                    if (parent.backscreen < 0)
                        parent.backscreen = 0;
                    return false;
                }
                parent.blackOut = false;
                parent.blackOutStopper = false;
                parent.blackOutChips.Clear();
                return true;
            }
            this.boEndOK2 = true;
            return false;
        }

        public void BlackOutRender(IRenderer dg, Panel.COLOR uni)
        {
            if (this.blackOutName != null)
            {
                if (this.nameAlpha > byte.MaxValue)
                    this.nameAlpha = byte.MaxValue;
                if (this.nameAlpha < 0)
                    this.nameAlpha = 0;
                int length = this.blackOutName.Length;
                this._position = new Vector2(8f, 32f);
                if (uni == Panel.COLOR.blue)
                    this._position.X += 120f;
                this.TextRender(dg, this.blackOutName, false, this._position, true, Color.FromArgb(this.nameAlpha, Color.White));
                if (!this.powerprint)
                    return;
                this._position = new Vector2(24 + length * 8, 32f);
                if (uni == Panel.COLOR.blue)
                    this._position.X += 120f;
                this.TextRender(dg, this.blackOutPower, false, this._position, true, Color.FromArgb(this.nameAlpha, byte.MaxValue, 230, 30));
            }
            else
                this.blackOutLend = true;
        }

        public AttackBase Paralyze(AttackBase a)
        {
            if (this.paralyze)
            {
                a.badstatus[3] = true;
                a.badstatustime[3] = 120;
            }
            if (this.inviNo)
                a.breakinvi = true;
            if (this._break)
                a.breaking = true;
            return a;
        }

        public AttackBase Paralyze(AttackBase a, CharacterBase c)
        {
            this.Paralyze(a);
            if (c is Player)
            {
                Player player = (Player)c;
                if (player.style == Player.STYLE.shinobi)
                {
                    if (this.swordtype)
                        a.parry = true;
                    if (this._break)
                        player.PluspointFighter(4);
                    if (this.plusing)
                        player.PluspointDoctor(4);
                    if (this.crack)
                        player.PluspointWing(4);
                    if (this.shild)
                        player.PluspointGaia(4);
                    if (this.obje)
                        player.PluspointGaia(4);
                }
            }
            return a;
        }

        public enum ELEMENT
        {
            normal,
            heat,
            aqua,
            eleki,
            leaf,
            poison,
            earth,
        }
    }
}
