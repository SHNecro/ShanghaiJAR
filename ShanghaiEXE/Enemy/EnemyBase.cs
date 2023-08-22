using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEvent;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    public class EnemyBase : CharacterBase
    {
        public ChipFolder[] dropchips = new ChipFolder[5];
        public int whitetime = 0;
        public EnemyBase.VIRUS ID;
        public int havezenny;
        public EnemyBase.ENEMY race;
        public int power;
        protected string name;
        public byte version;
        public bool printNumber;
        protected int roop;
        protected int samontarn;
        public Point helpPosition;
        public Point wantedPosition;
        public bool enemyCount;
        public bool namePrint;
        private int initflame;
        protected Rectangle rd;
        protected SaveData savedata;
        //public SceneMain main;

        public static EnemyBase EnemyMake(int number, EnemyBase e, bool rank)
        {
            IAudioEngine sound = e?.Sound;
            SceneBattle parent = e?.parent;
            Point point = e != null ? e.position : new Point(0, 0);
            byte n = e != null ? (byte)e.number : (byte)0;
            Panel.COLOR u = e != null ? e.union : Panel.COLOR.red;
            byte v = e != null ? e.version : (byte)1;
            int h = 1;
            int c1 = 0;
            int c2 = 0;
            int c3 = 0;
            //SaveData sa = this.savedata;
            //ChipFolder[,] zChip = parent.main.chipfolder;
           // SaveData sev = parent.sev;
            string names = "";
            if (e is NormalNavi normalNavi)
            {
                h = normalNavi.InitialHP;
                c1 = normalNavi.InitialChips[0];
                c2 = normalNavi.InitialChips[1];
                c3 = normalNavi.InitialChips[2];
                names = normalNavi.InitialName;
            }
            Dictionary<int, EnemyBase> dictionary = new Dictionary<int, EnemyBase>();
            EnemyBase enemyBase;
            switch (number)
            {
                case 1:
                    enemyBase = new ReyCanon(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 2:
                    enemyBase = new FlowerTank(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 3:
                    enemyBase = new Gelpark(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 4:
                    enemyBase = new Mossa(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 5:
                    enemyBase = new FireCat(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 6:
                    enemyBase = new Kedamar(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 7:
                    enemyBase = new Poisorlin(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 8:
                    enemyBase = new Screwn(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 9:
                    enemyBase = new Shellrun(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 10:
                    enemyBase = new Brocooler(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 11:
                    enemyBase = new Puchioni(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 12:
                    enemyBase = new EvilEye(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 13:
                    enemyBase = new BibityBat(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 14:
                    enemyBase = new Junks(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 15:
                    enemyBase = new Ikary(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 16:
                    enemyBase = new GekoHuts(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 17:
                    enemyBase = new Zarinear(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 18:
                    enemyBase = new Barlizard(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 19:
                    enemyBase = new Musya(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 20:
                    enemyBase = new SwordDog(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 21:
                    enemyBase = new Lanster(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 22:
                    enemyBase = new Doripper(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 23:
                    enemyBase = new Flea(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 24:
                    enemyBase = new Bronzer(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 25:
                    enemyBase = new Furjirn(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 26:
                    enemyBase = new Bouzu(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 27:
                    enemyBase = new Riveradar(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 28:
                    enemyBase = new Juraigon(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 29:
                    enemyBase = new PaneMole(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 30:
                    enemyBase = new OnoHawk(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 31:
                    enemyBase = new Mantiser(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 32:
                    enemyBase = new Ponpoko(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 33:
                    enemyBase = new Massdliger(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 34:
                    enemyBase = new KorYor(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 35:
                    enemyBase = new Rieber(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 36:
                    enemyBase = new GunBatta(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 37:
                    enemyBase = new DanBeetle(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 38:
                    enemyBase = new RayJune(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 39:
                    enemyBase = new Holenake(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 40:
                    enemyBase = new Woojow(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 41:
                    enemyBase = new BakeBake(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 42:
                    enemyBase = new NormalNavi(sound, parent, point.X, point.Y, n, u, v, h, c1, c2, c3, names);
                    break;
                case 43:
                    enemyBase = new Marisa(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 44:
                    enemyBase = new Sakuya(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 45:
                    enemyBase = new TankMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 46:
                    enemyBase = new SpannerMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                // 47
                case 48:
                    enemyBase = new HakutakuMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 49:
                    enemyBase = new TortoiseMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 50:
                    enemyBase = new BeetleMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 51:
                    enemyBase = new Yorihime(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 52:
                    enemyBase = new Cirno(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 53:
                    enemyBase = new Medicine(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 54:
                    enemyBase = new Iku(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 55:
                    enemyBase = new PyroMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 56:
                    enemyBase = new Mrasa(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 57:
                    enemyBase = new ScissorMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 58:
                    enemyBase = new Chen(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 59:
                    enemyBase = new Ran(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 60:
                    enemyBase = new Uthuho(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 61:
                    enemyBase = new Madman(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 62:
                    enemyBase = new Youmu(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 63:
                    enemyBase = new HeavenBarrier(sound, parent, point.X, point.Y, n, u, v);
                    break;
                // 64
                case 65:
                    enemyBase = new Kikuri(sound, parent, point.X, point.Y, n, u, v);
                    break;
                // 66-67
                case 68:
                    enemyBase = new Mima(sound, parent, point.X, point.Y, n, u, v);
                    break;
                // 69-80
                case 81:
                    enemyBase = new ShanghaiDS(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 82:
                    enemyBase = new Flandre(sound, parent, point.X, point.Y, n, u, v);
                    break;
				case 83:
					enemyBase = new yuyuko(sound, parent, point.X, point.Y, n, u, v);
					break;
                case 84:
                    enemyBase = new DruidMan(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 85:
                    enemyBase = new CirnoBX(sound, parent, point.X, point.Y, n, u, v);
                    break;
                // 86-90
                case 90:
                    enemyBase = new OrinMook1(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 91:
                    enemyBase = new OrinMook2(sound, parent, point.X, point.Y, n, u, v);
                    break;
                case 92:
                    enemyBase = new Orin(sound, parent, point.X, point.Y, n, u, v);
                    break;

                default:
                    enemyBase = null;
                    break;
            }
            try
            {
                if (number > 0)
                    enemyBase.ID = (EnemyBase.VIRUS)number;
            }
            catch
            {
            }
            if (parent != null)
                e.Init();
            return enemyBase;
        }

        public virtual int Power
        {
            get
            {
                return !this.badstatus[1] ? this.power : this.power / 2;
            }
        }

        public IAudioEngine Sound
        {
            set
            {
                this.sound = value;
            }
            get
            {
                return this.sound;
            }
        }

        public SceneBattle Parent
        {
            set
            {
                this.parent = value;
            }
            get
            {
                return this.parent;
            }
        }

        public bool CanUP
        {
            get
            {
                return this.Canmove(new Point(this.position.X, this.position.Y - 1), this.number, this.union);
            }
        }

        public bool CanDown
        {
            get
            {
                return this.Canmove(new Point(this.position.X, this.position.Y + 1), this.number, this.union);
            }
        }

        public bool CanLeft
        {
            get
            {
                return this.Canmove(new Point(this.position.X - 1, this.position.Y), this.number, this.union);
            }
        }

        public bool CanRight
        {
            get
            {
                return this.Canmove(new Point(this.position.X + 1, this.position.Y), this.number, this.union);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public EnemyBase(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p)
        {
            this.HPposition = new Vector2(-100f, -100f);
            this.position = new Point(pX, pY);
            this.positionre = this.position;
            this.number = n;
            this.union = u;
            if (this.union == Panel.COLOR.red)
                this.rebirth = true;
            else
                this.rebirth = false;
            this.positionnow = this.position;
            this.version = v;
            if (this.union != Panel.COLOR.red || this.parent == null)
                return;
            this.samontarn = this.parent.turn;
        }

        public virtual void Init()
        {
            this.positionre = this.position;
            this.positionnow = this.position;
            if (this.union == Panel.COLOR.red)
                this.rebirth = true;
            else
                this.rebirth = false;
            if (this.union != Panel.COLOR.red || this.parent == null)
                return;
            this.samontarn = this.parent.turn;
        }

        public override void Updata()
        {
            if (this.slipping && (this.neutlal || this.knockslip))
                this.Slip(this is ChipUsingNaviBase ? 68 : this.height);
            else if (!this.badstatus[3] && (!this.neutlal || !this.badstatus[7] || this.badstatus[6]))
                this.Moving();
            if (this.union == Panel.COLOR.red && this.parent.turn == this.samontarn + 3)
            {
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.positionDirect, this.position));
                this.sound.PlaySE(SoundEffect.enterenemy);
                this.flag = false;
            }
            if (this.alfha < byte.MaxValue)
                this.alfha += 15;
            base.Updata();
        }

        public void BaseUpdata()
        {
            base.Updata();
        }

        public void HPRend(IRenderer dg)
        {
            this.HPRender(dg, this.HPposition, this.printhp);
        }

        protected virtual void Moving()
        {
        }

        public void HPplus(int plus)
        {
            this.hpmax += plus;
            this.hp += plus;
            this.hpprint += plus;
        }

        public virtual bool EnemyInitAction()
        {
            if (this.initflame == 0)
            {
                this.namePrint = true;
                this.sound.PlaySE(SoundEffect.enterenemy);
            }
            ++this.initflame;
            if (this.initflame < 30)
                this.alfha += 8;
            else if (this.initflame == 30)
                this.alfha = byte.MaxValue;
            else if (this.initflame >= 40)
                return true;
            return false;
        }

		public virtual void Nameprint(IRenderer dg, bool numberprint)
		{
			if (!this.namePrint || this.parent == null || !this.parent.namePrint)
				return;
			var adjustedName = (!this.printNumber || this.version <= 1) ? this.name : (this.name + this.version.ToString());
			AllBase.NAME[] nameArray = this.Nametodata(adjustedName);
			int length = nameArray.Length;
			int nameVersionAdjustmentOffset = 0;

            var specialCharacters = new[] { "V2", "V3", "DS", "BX", "SP", "RV", "EX" };
            foreach (var character in specialCharacters)
            {
                if (this.name != null && this.name.Contains(character))
				{
					--length;
					nameVersionAdjustmentOffset += 8;
				}
            }
			int nameXPosition = 240 - nameArray.Length * 8;
			this.color = this.alfha == 0 ? Color.FromArgb(0, byte.MaxValue, byte.MaxValue, byte.MaxValue) : Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

			// Draw edge of name backround <
			this._rect = new Rectangle(320, 104, 8, 16);
			this._position = new Vector2(nameXPosition - 8 + nameVersionAdjustmentOffset, this.number * 16);
			dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
			// Draw background and name
			for (int index = 0; index < length; ++index)
			{
				this._position = new Vector2(nameXPosition + 8 * index + nameVersionAdjustmentOffset, this.number * 16);
				this._rect = new Rectangle(328, 104, 8, 16);
				dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
			}
			this._position = new Vector2(nameXPosition + nameVersionAdjustmentOffset, this.number * 16);
			DrawBlockCharacters(dg, nameArray, 88, this._position, this.color, out this._rect, out this._position);
			// Draw version number
			if (numberprint && this.version > 1)
			{
				this._position = new Vector2(nameXPosition + 8 * nameArray.Length + nameVersionAdjustmentOffset, this.number * 16);
				this._rect = new Rectangle(328, 104, 8, 16);
				dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
				this._rect = new Rectangle(version * 8, 104, 8, 16);
				dg.DrawImage(dg, "font", this._rect, true, this._position, this.color);
			}
		}


		protected Point Return(int[] interval, int[] xpoint, int y, int waittime)
        {
            int index1 = 0;
            for (int index2 = 1; index2 < interval.Length; ++index2)
            {
                if (waittime <= interval[index2] && waittime > interval[index2 - 1])
                    index1 = index2;
            }
            if (index1 != xpoint.Length)
                return new Point(xpoint[index1], y);
            return new Point(0, 0);
        }

        protected Point ReturnKai(int[] interval, int[] xpoint, int y, int waittime)
        {
            return CharacterAnimation.ReturnKai(interval, xpoint, y, waittime);
        }

        protected void BackMove()
        {
            this.positionre.X = this.union == Panel.COLOR.blue ? 5 : 0;
            if (this.Canmove(this.positionre))
            {
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                this.position = this.positionre;
                this.PositionDirectSet();
                this.frame = 0;
            }
            else
            {
                this.positionre.Y = 0;
                if (this.Canmove(this.positionre, this.number))
                {
                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                    this.position = this.positionre;
                    this.PositionDirectSet();
                }
                else
                {
                    this.positionre.Y = 1;
                    if (this.Canmove(this.positionre, this.number))
                    {
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.position = this.positionre;
                        this.PositionDirectSet();
                    }
                    else
                    {
                        this.positionre.Y = 2;
                        if (this.Canmove(this.positionre, this.number))
                        {
                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                            this.position = this.positionre;
                            this.PositionDirectSet();
                        }
                        else
                            this.PositionDirectSet();
                    }
                }
            }
        }

        protected void Shadow()
        {
            this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), this.positionDirect, this.picturename, this.rebirth, this.position));
        }

        protected void Death(Rectangle r, Rectangle rw, Vector2 v, string n)
        {
            if (this.parent == null || !this.flag)
                return;
            if (this.race == EnemyBase.ENEMY.virus)
            {
                this.parent.effects.Add(new EnemyDeath(this.sound, this.parent, r, rw, v, n, this.rebirth, this.position));
                if (this.union == Panel.COLOR.blue && this.enemyCount)
                    --this.parent.manyenemys;
            }
            else
            {
                if (this.union == Panel.COLOR.blue && this.enemyCount)
                {
                    --this.parent.manyenemys;
                    this.parent.stopEnd = true;
                }
                if (this is Kikuri)
                    this.parent.effects.Add(new KikuriDeath(this.sound, this.parent, r, rw, this.rd, v, n, this.rebirth, this.position));
                else
                    this.parent.effects.Add(new NaviDeath(this.sound, this.parent, r, rw, this.rd, v, n, this.rebirth, this.position));
            }
            if (this.union == Panel.COLOR.blue)
            {
                if (this.parent.simultaneousdel <= 1 && this.parent.simultaneoustime == 0)
                {
                    this.parent.simultaneousdel = 1;
                    this.parent.simultaneoustime = 20;
                }
                else if (this.parent.simultaneoustime > 0)
                {
                    ++this.parent.simultaneousdel;
                    this.parent.printdelete = 60;
                    this.parent.simultaneoustime = 20;
                    if (this.parent.simultaneousdel >= 2 && this.parent.mind.MindNow != MindWindow.MIND.fullsync)
                        this.parent.mind.MindNow = MindWindow.MIND.smile;
                }
            }
            this.parent.blackOutChips.RemoveAll(c => c.userNum == this.number);
            if (this.enemyCount)
            {
                this.parent.dropchip = this.dropchips;
                this.parent.dropzenny = this.havezenny;
            }
            this.flag = false;
        }

        public enum ENEMY
        {
            virus,
            navi,
        }

        public enum VIRUS
        {
            none,
            ReyCanon,
            FlowerTank,
            GelPark,
            Mossa,
            FireCat,
            Kedamar,
            PoisoRin,
            Screwn,
            Shellrun,
            BuraCra,
            PuchiOni,
            EvilEye,
            BibitBat,
            Junks,
            Ikary,
            GekoHuts,
            Zarinear,
            Barlizard,
            Musya,
            SwordDog,
            LanceTer,
            Doripper,
            FireBird,
            Bronzer,
            Fujin,
            Bouzu,
            Ribereader,
            Draragon,
            PaneMole,
            OnoHowk,
            Mantis,
            Ponpoco,
            MasDoraiger,
            KorYor,
            Rieber,
            GunHopper,
            Beatle,
            Raijine,
            Holenake,
            WooJow,
            Bakebake,
            NormalNavi,
            Marisa,
            Sakuya,
            TankMan,
            SpannerMan,
            MissileMan,
            HakutakuMan,
            GenjiMan,
            BeetleMan,
            Yorihime,
            Cirno,
            Medicine,
            Iku,
            ChackaMan,
            Murasa,
            ScissorsMan,
            Chen,
            Ran,
            Uthuho,
            FranDoll,
            Youmu,
            Yuyuko,
            DruidMan,
            Kikuri,
            ShanhaiSP,
            ChilnoBX,
            Mima,
            NetBattle_Shanghai,
        }
    }
}
