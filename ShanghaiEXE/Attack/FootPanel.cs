using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class FootPanel : AttackBase
    {
        private FootPanel.MOTION motion;
        private readonly int time;

        public FootPanel(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int c,
          FootPanel.MOTION m,
          ChipBase.ELEMENT ele,
          bool shake = true)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.OverPosition(new Point(pX, pY)))
                this.flag = false;
            else if (this.parent.panel[this.position.X, this.position.Y].state != Panel.PANEL._break)
            {
                this.element = ele;
                this.time = 1;
                this.speed = 2;
                this.animationpoint.X = 0;
                this.hitrange = new Point(0, 0);
                this.hitting = false;
                this.rebirth = this.union == Panel.COLOR.blue;
                this.positionre = this.position;
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
                this.animationpoint.Y = c;
                switch (this.element)
                {
                    case ChipBase.ELEMENT.heat:
                        this.sound.PlaySE(SoundEffect.heat);
                        this.parent.panel[this.position.X, this.position.Y].State = Panel.PANEL._burner;
                        break;
                    case ChipBase.ELEMENT.aqua:
                        this.sound.PlaySE(SoundEffect.shotwave);
                        this.parent.panel[this.position.X, this.position.Y].State = Panel.PANEL._ice;
                        break;
                    case ChipBase.ELEMENT.eleki:
                        this.sound.PlaySE(SoundEffect.bomb);
                        this.parent.panel[this.position.X, this.position.Y].State = Panel.PANEL._thunder;
                        break;
                    case ChipBase.ELEMENT.leaf:
                        this.sound.PlaySE(SoundEffect.shotwave);
                        this.parent.panel[this.position.X, this.position.Y].State = Panel.PANEL._grass;
                        break;
                    case ChipBase.ELEMENT.poison:
                        this.hitting = true;
                        this.sound.PlaySE(SoundEffect.bomb);
                        this.parent.panel[this.position.X, this.position.Y].State = Panel.PANEL._poison;
                        break;
                    case ChipBase.ELEMENT.earth:
                        this.sound.PlaySE(SoundEffect.sand);
                        this.parent.panel[this.position.X, this.position.Y].State = Panel.PANEL._sand;
                        break;
                }
                if (shake)
                {
                    this.ShakeStart(2, 40);
                    this.sound.PlaySE(SoundEffect.fire);
                    foreach (CharacterBase characterBase in this.parent.AllChara())
                    {
                        if (characterBase.union != this.union)
                        {
                            characterBase.badstatus[7] = true;
                            characterBase.badstatustime[7] = 40;
                        }
                    }
                }
                this.motion = m;
                this.frame = 0;
            }
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                switch (this.element)
                {
                    case ChipBase.ELEMENT.heat:
                    case ChipBase.ELEMENT.aqua:
                    case ChipBase.ELEMENT.leaf:
                    case ChipBase.ELEMENT.earth:
                        switch (this.motion)
                        {
                            case FootPanel.MOTION.init:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 3)
                                {
                                    this.hitting = true;
                                    this.motion = FootPanel.MOTION.set;
                                    break;
                                }
                                break;
                            case FootPanel.MOTION.set:
                                this.animationpoint.X = 3;
                                if (this.frame >= this.time)
                                {
                                    this.hitting = false;
                                    this.frame = 0;
                                    this.motion = FootPanel.MOTION.end;
                                    break;
                                }
                                break;
                            case FootPanel.MOTION.end:
                                this.animationpoint.X = 4 - this.frame;
                                if (this.frame >= 3)
                                {
                                    this.flag = false;
                                    break;
                                }
                                break;
                        }
                        break;
                    case ChipBase.ELEMENT.eleki:
                        switch (this.motion)
                        {
                            case FootPanel.MOTION.init:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 3)
                                {
                                    this.hitting = true;
                                    this.motion = FootPanel.MOTION.set;
                                    break;
                                }
                                break;
                            case FootPanel.MOTION.set:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 3)
                                {
                                    this.hitting = false;
                                    this.frame = 0;
                                    this.motion = FootPanel.MOTION.end;
                                    break;
                                }
                                break;
                            case FootPanel.MOTION.end:
                                this.flag = false;
                                break;
                        }
                        break;
                    case ChipBase.ELEMENT.poison:
                        this.animationpoint.X = this.frame;
                        if (this.frame >= 4)
                        {
                            this.hitting = false;
                            break;
                        }
                        if (this.frame >= 13)
                        {
                            this.flag = false;
                            break;
                        }
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 30f);
                    this._rect = new Rectangle(this.animationpoint.X * 32, 0, 32, 80);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.aqua:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 30f);
                    this._rect = new Rectangle(this.animationpoint.X * 32, 80, 32, 80);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.eleki:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 70f);
                    this._rect = new Rectangle(128 + this.animationpoint.X * 16, 0, 16, 136);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.leaf:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 30f);
                    this._rect = new Rectangle(this.animationpoint.X * 32, 160, 32, 80);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.poison:
                    this._rect = new Rectangle(this.animationpoint.X * 48, 336, 48, 48);
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 8f);
                    dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.earth:
                    this._position = this.positionDirect;
                    this._rect = new Rectangle(this.animationpoint.X * 40 + 192, this.animationpoint.Y * 24, 40, 24);
                    dg.DrawImage(dg, "rieber", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
            }
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }

        public enum MOTION
        {
            init,
            set,
            end,
        }
    }
}
