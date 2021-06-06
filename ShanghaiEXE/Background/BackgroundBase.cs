using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSBackground
{
    public class BackgroundBase
    {
        protected string picturename;
        protected Point size;
        protected int speed;
        private int speedcontrol;
        protected int animespeed;
        protected int flames;
        private int flame;
        private Point scrollflame;
        protected Point scroll;
        protected Point scrollspeed;
        protected bool design;
        protected BackgroundBase.BACKTYPE type;
        protected Color backcolor;
        protected int[,] animasion;

		private static Dictionary<int, BackgroundBase> backgrounds = new Dictionary<int, BackgroundBase>()
		{
			[0] = new BackDefault(),
            [1] = new BackgroundBase(Color.FromArgb(92, 32, 92)),
            [2] = new BackgroundBase(Color.FromArgb(0, 64, 0)),
            [3] = new BackgroundBase(Color.FromArgb(55, 150, byte.MaxValue)),
            [4] = new AriceBack(),
            [5] = new GenNetBack(),
            [6] = new RefrigeratorBack(),
            [7] = new SityNetBack(),
            [8] = new AirBack(),
            [9] = new MariBack(),
            [10] = new RemiHPback(),
            [11] = new rikaHPBack(),
            [12] = new EienBack(),
            [13] = new UraBack(),
            [14] = new SchoolBack(),
            [15] = new BackgroundBase(Color.FromArgb(0, 0, 0)),
            [16] = new NAback(),
            [17] = new TournamentBack(),
            [18] = new JinjaBack(),
            [19] = new EienSchoolBack(),
            [20] = new NeckBack(),
            [21] = new SeirenNetBack(),
            [22] = new SeirenNetBack2(),
            [23] = new IngBack(),
            [24] = new HotelBack(),
            [25] = new ClockTowerBack(),
            [26] = new UraBack2(),
            [27] = new CompanyServerBack(),
            [28] = new TenshiBack(),
            [29] = new BackDefault(),
            [30] = new LostShipBack(),
            [31] = new ROMback(0),
            [32] = new ROMback(1),
            [33] = new ROMback(2),
            [34] = new ROMback(3),
            [35] = new HospitalBack(),
            [36] = new HeavenBack(),
            [37] = new FinalFloorBack(),
            [38] = new CentralBack(),
            [39] = new BackgroundBase(Color.FromArgb(87, 206, 234)),
            [40] = new DeepUraBack(),
        };

		public static BackgroundBase BackMake(int number)
		{
			return BackgroundBase.backgrounds[number];
		}

		public BackgroundBase(Color backcolor)
        {
            this.backcolor = backcolor;
            this.animasion = new int[2, 256];
            for (int index = 0; index < this.animasion.GetLength(1); ++index)
            {
                this.animasion[0, index] = index;
                this.animasion[1, index] = 1;
            }
        }

        public virtual void Update()
        {
            if (!this.design)
                return;
            ++this.speedcontrol;
            if (this.speedcontrol >= 256)
                this.speedcontrol = 0;
            if (this.speedcontrol % (this.speed * this.animasion[1, this.flame]) == 0)
            {
                ++this.flame;
                if (this.flame >= this.flames)
                    this.flame = 0;
            }
            if (this.type == BackgroundBase.BACKTYPE.scroll)
            {
                if (this.speedcontrol % this.animespeed == 0)
                {
                    ++this.scrollflame.X;
                    ++this.scrollflame.Y;
                }
                if (this.scrollspeed.X > 0)
                {
                    if (this.scrollflame.X >= this.scrollspeed.X)
                    {
                        this.scrollflame.X = 0;
                        ++this.scroll.X;
                        if (this.scroll.X >= this.size.X)
                            this.scroll.X = 0;
                    }
                    ++this.scrollflame.X;
                }
                if (this.scrollspeed.Y > 0)
                {
                    if (this.scrollflame.Y >= this.scrollspeed.X)
                    {
                        this.scrollflame.Y = 0;
                        ++this.scroll.Y;
                        if (this.scroll.Y >= this.size.Y)
                            this.scroll.Y = 0;
                    }
                    ++this.scrollflame.Y;
                }
            }
        }

        public void UpdateHighSpeed()
        {
            this.scroll.X += this.scrollspeed.X;
            if (this.scroll.X >= this.size.X)
                this.scroll.X = 0;
            this.scroll.Y += this.scrollspeed.Y;
            if (this.scroll.Y < this.size.Y)
                return;
            this.scroll.Y = 0;
        }

        public virtual void Render(IRenderer dg)
        {
            Color backcolor = this.backcolor;
            Rectangle _rect1 = new Rectangle(0, 0, 240, 160);
            Vector2 _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect1, true, _point, backcolor);
            if (!this.design)
                return;
            Point point = new Point();
            int num1 = 240 / this.size.X;
            int num2 = 160 / this.size.Y;
            int num3 = num1 + 3;
            int num4 = num2 + 3;
            if (this.type == BackgroundBase.BACKTYPE.scroll)
                point = this.size;
            for (int index1 = 0; index1 < num3; ++index1)
            {
                for (int index2 = 0; index2 < num4; ++index2)
                {
                    Rectangle _rect2 = new Rectangle(this.animasion[0, this.flame] * this.size.X, 0, this.size.X, this.size.Y);
                    _point = new Vector2(index1 * this.size.X + this.scroll.X - point.X, index2 * this.size.Y + this.scroll.Y - point.Y);
                    dg.DrawImage(dg, this.picturename, _rect2, false, _point, Color.White);
                }
            }
        }

        protected enum BACKTYPE
        {
            scroll,
            stop,
        }
    }
}
