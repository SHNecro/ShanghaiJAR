using NSAttack;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSBattle
{
    public class Panel : AllBase
    {
        private bool bright = false;
        private Panel.PANEL stateold = Panel.PANEL._nomal;
        private const int poisonanimation = 10;
        private const int bandtanimation = 3;
        private const int thunderwait = 100;
        private const int defaulttime = 180;
        public int flashtime;
        public bool noRender;
        public bool animationON;
        private bool animechange;
        private int anime;
        private bool rebirth;
        private int counter;
        private Point position;
        private int thundercount;
        public Panel.PANEL state;
        public Panel.COLOR color;
        public Panel.COLOR colordefault;
        public bool inviolability;
        public bool bashed;
        public int breaktime;
        private const int breaktime_ = 600;
        private bool breaktoggle;
        private readonly SceneBattle parent;
        private bool flashing;

        public ChipBase.ELEMENT Element
        {
            get
            {
                switch (this.state)
                {
                    case Panel.PANEL._grass:
                        return ChipBase.ELEMENT.leaf;
                    case Panel.PANEL._ice:
                        return ChipBase.ELEMENT.aqua;
                    case Panel.PANEL._sand:
                        return ChipBase.ELEMENT.earth;
                    case Panel.PANEL._poison:
                        return ChipBase.ELEMENT.poison;
                    case Panel.PANEL._burner:
                        return ChipBase.ELEMENT.heat;
                    case Panel.PANEL._thunder:
                        return ChipBase.ELEMENT.eleki;
                    default:
                        return ChipBase.ELEMENT.normal;
                }
            }
        }

        public Panel.PANEL State
        {
            get
            {
                return this.state;
            }
            set
            {
                switch (value)
                {
                    case Panel.PANEL._crack:
                        this.Crack();
                        break;
                    case Panel.PANEL._break:
                        this.Break();
                        break;
                    default:
                        if (this.Hole || this.state == Panel.PANEL._un || this.state == value)
                            break;
                        this.state = value;
                        this.anime = 0;
                        break;
                }
            }
        }

        public bool Hole
        {
            get
            {
                return this.state == Panel.PANEL._break || this.state == Panel.PANEL._none || this.state == Panel.PANEL._un;
            }
        }

        public Panel(IAudioEngine s, SceneBattle p, int x, int y)
          : base(s)
        {
            this.parent = p;
            this.position = new Point(x, y);
        }

        public bool OnCharaCheck()
        {
            bool flag = false;
            foreach (CharacterBase characterBase in this.parent.AllHitter())
            {
                if (!characterBase.Flying
                    && ((this.position.X == characterBase.position.X && this.position.Y == characterBase.position.Y)
                        || (characterBase.positionReserved != null && this.position.X == characterBase.positionReserved.Value.X && this.position.Y == characterBase.positionReserved.Value.Y)))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public void Crack()
        {
            switch (this.state)
            {
                case Panel.PANEL._crack:
                    if (this.OnCharaCheck())
                        break;
                    this.state = Panel.PANEL._break;
                    break;
                case Panel.PANEL._break:
                    break;
                case Panel.PANEL._none:
                case Panel.PANEL._un:
                    break;
                default:
                    this.state = Panel.PANEL._crack;
                    break;
            }
        }

        public void Break()
        {
            switch (this.state)
            {
                case Panel.PANEL._break:
                    break;
                case Panel.PANEL._none:
                case Panel.PANEL._un:
                    break;
                default:
                    if (!this.OnCharaCheck())
                    {
                        this.state = Panel.PANEL._break;
                        break;
                    }
                    this.state = Panel.PANEL._crack;
                    break;
            }
        }

        public void Update()
        {
            if (this.flashtime > 0)
            {
                --this.flashtime;
                if (this.flashtime % 8 == 0)
                    this.flashing = !this.flashing;
            }
            else if (this.flashing)
                this.flashing = false;
            if (this.bashed && (this.parent.bashtime <= 0 && this.color != this.colordefault))
            {
                bool flag = false;
                if (this.colordefault == Panel.COLOR.blue && this.parent.redRight < this.position.X)
                    flag = true;
                else if (this.colordefault == Panel.COLOR.red && this.parent.blueLeft > this.position.X)
                    flag = true;
                if (flag)
                {
                    this.flashtime = 180;
                    this.color = this.colordefault;
                    this.inviolability = false;
                    this.bashed = false;
                }
            }
            if (this.state != this.stateold)
            {
                this.stateold = this.state;
                this.StateInit();
            }
            switch (this.state)
            {
                case Panel.PANEL._poison:
                    ++this.frame;
                    if (this.frame == 10)
                    {
                        this.frame = 0;
                        if (this.anime <= 0 && this.rebirth)
                            this.rebirth = false;
                        if (this.anime >= 2 && !this.rebirth)
                            this.rebirth = true;
                        this.anime += this.rebirth ? -1 : 1;
                        break;
                    }
                    break;
                case Panel.PANEL._burner:
                case Panel.PANEL._thunder:
                    if (!this.parent.blackOut)
                    {
                        if (this.animationON)
                        {
                            ++this.frame;
                            if (this.frame >= 3)
                            {
                                this.frame = 0;
                                this.anime += this.rebirth ? -1 : 1;
                                this.rebirth = !this.rebirth;
                                if (!this.rebirth)
                                {
                                    ++this.counter;
                                    if (this.counter >= 9)
                                    {
                                        this.counter = 0;
                                        if (this.animechange)
                                        {
                                            if (this.state == Panel.PANEL._burner)
                                                this.parent.attacks.Add(new FootFire(this.sound, this.parent, this.position.X, this.position.Y, this.color == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue, 10, FootFire.MOTION.init));
                                            else
                                                this.parent.attacks.Add(new ParaizeThunder(this.sound, this.parent, this.position.X, this.position.Y, this.color == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue, 0, ParaizeThunder.MOTION.init));
                                            this.animechange = false;
                                            this.animationON = false;
                                            this.anime = 0;
                                            if (this.state == Panel.PANEL._thunder)
                                                this.thundercount = this.Random.Next(50);
                                        }
                                        else
                                        {
                                            ++this.anime;
                                            this.animechange = true;
                                        }
                                    }
                                }

                                if (this.anime < 0)
                                {
                                    this.anime = 0;
                                }
                                else if (this.anime > 2)
                                {
                                    this.anime = 1;
                                }
                            }
                        }
                        else if (this.state == Panel.PANEL._thunder)
                        {
                            ++this.thundercount;
                            if (this.thundercount <= 10)
                                this.thundercount = this.Random.Next(50);
                            if (this.thundercount >= 100)
                            {
                                this.thundercount = 0;
                                this.animationON = true;
                            }
                        }
                        break;
                    }
                    break;
                default:
                    this.anime = 0;
                    break;
            }
            if (this.breaktime > 0)
            {
                if (this.state == Panel.PANEL._break)
                {
                    if (!this.parent.blackOut)
                    {
                        --this.breaktime;
                        if (this.breaktime < 180 && this.breaktime % 3 == 0)
                            this.breaktoggle = !this.breaktoggle;
                        if (this.breaktime > 0)
                            return;
                        this.state = Panel.PANEL._nomal;
                    }
                }
                else
                    this.breaktime = 0;
            }
            else
            {
                if (this.state != Panel.PANEL._break)
                    return;
                this.breaktime = 600;
                this.breaktoggle = false;
            }
        }

        private void StateInit()
        {
            switch (this.state)
            {
                case Panel.PANEL._nomal:
                case Panel.PANEL._crack:
                case Panel.PANEL._grass:
                case Panel.PANEL._ice:
                case Panel.PANEL._sand:
                    if ((uint)this.counter > 0U)
                        this.counter = 0;
                    if ((uint)this.anime > 0U)
                        this.anime = 0;
                    if (!this.animationON)
                        break;
                    this.animationON = false;
                    break;
            }
        }

        public void BrightReset()
        {
            this.bright = false;
        }

        public void Bright()
        {
            this.bright = true;
        }

        public void Render(IRenderer dg)
        {
            if (!this.noRender)
            {
                int num = (int)this.color;
                if (this.flashing)
                {
                    switch (this.color)
                    {
                        case Panel.COLOR.red:
                            num = 1;
                            break;
                        case Panel.COLOR.blue:
                            num = 0;
                            break;
                    }
                }
                switch (this.state)
                {
                    case Panel.PANEL._break:
                        if (this.breaktoggle)
                        {
                            this._rect = new Rectangle(40 * num, 0, 40, 32);
                            break;
                        }
                        this._rect = new Rectangle(40 * num, (int)this.state * 32, 40, 32);
                        break;
                    case Panel.PANEL._poison:
                    case Panel.PANEL._burner:
                    case Panel.PANEL._thunder:
                        this._rect = new Rectangle(40 * num + this.anime * 80, (int)this.state * 32, 40, 32);
                        break;
                    case Panel.PANEL._un:
                        this._rect = new Rectangle(0, 0, 0, 0);
                        break;
                    default:
                        this._rect = new Rectangle(40 * num, (int)this.state * 32, 40, 32);
                        break;
                }
                this._position = new Vector2(40 * this.position.X + this.Shake.X, 70 + 24 * this.position.Y + this.Shake.X);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                if (this.inviolability
                    && (this.position.X >= 1 || this.parent.panel[this.position.X + 1, this.position.Y].inviolability)
                    && (this.position.X <= 4 || this.parent.panel[this.position.X - 1, this.position.Y].inviolability))
                {
                    this._rect = new Rectangle(80, 288, 40, 32);
                    this._position = new Vector2(40 * this.position.X + this.Shake.X, 70 + 24 * this.position.Y + this.Shake.X);
                    dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                }
            }
            if (!this.bright || this.state == Panel.PANEL._un)
                return;
            this._rect = new Rectangle(512, 0, 40, 24);
            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
        }

        public enum PANEL
        {
            _nomal,
            _crack,
            _break,
            _grass,
            _ice,
            _sand,
            _poison,
            _burner,
            _thunder,
            _none,
            _un,
        }

        public enum COLOR
        {
            red,
            blue,
        }
    }
}
