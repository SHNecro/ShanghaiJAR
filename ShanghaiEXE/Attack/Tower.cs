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
    internal class Tower : AttackBase
    {
        private Tower.MOTION motion;
        private readonly int time;
        private readonly int nextmake;
        private int count;
        public bool make;

        public Tower(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.InArea)
                this.flag = false;
            else if (!this.StandPanel.Hole)
            {
                this.motion = Tower.MOTION.init;
                this.nextmake = s;
                this.element = ele;
                this.time = 1;
                this.speed = 2;
                this.animationpoint.X = 0;
                this.hitrange = new Point(0, 0);
                this.hitting = false;
                this.rebirth = this.union == Panel.COLOR.blue;
                this.positionre = this.position;
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
                this.frame = 0;
                switch (this.element)
                {
                    case ChipBase.ELEMENT.normal:
                        this.sound.PlaySE(SoundEffect.shotwave);
                        break;
                    case ChipBase.ELEMENT.heat:
                        this.sound.PlaySE(SoundEffect.heat);
                        break;
                    case ChipBase.ELEMENT.aqua:
                        this.sound.PlaySE(SoundEffect.shotwave);
                        break;
                    case ChipBase.ELEMENT.eleki:
                        this.sound.PlaySE(SoundEffect.bomb);
                        break;
                    case ChipBase.ELEMENT.leaf:
                        this.sound.PlaySE(SoundEffect.shotwave);
                        break;
                    case ChipBase.ELEMENT.poison:
                        this.hitting = true;
                        this.sound.PlaySE(SoundEffect.bomb);
                        break;
                    case ChipBase.ELEMENT.earth:
                        this.sound.PlaySE(SoundEffect.sand);
                        break;
                }
            }
            else
                this.flag = false;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                if (this.count == this.nextmake)
                {
                    this.make = true;
                    this.hitting = false;
                    this.positionre.X = this.position.X + this.UnionRebirth;
                    if (this.positionre.X >= 0
                        && this.positionre.X < 6
                        && this.positionre.Y >= 0
                        && this.positionre.Y < 3
                        && (!this.parent.panel[this.positionre.X, this.positionre.Y].Hole))
                    {
                        Tower tower = new Tower(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.power, this.nextmake, this.element)
                        {
                            badstatus = this.badstatus,
                            badstatustime = this.badstatustime
                        };
                        this.parent.attacks.Add(tower);
                    }
                    this.positionre = this.position;
                }
                ++this.count;
                if (this.frame >= 100)
                    this.flag = false;
                switch (this.element)
                {
                    case ChipBase.ELEMENT.normal:
                        switch (this.motion)
                        {
                            case Tower.MOTION.init:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 5)
                                {
                                    this.hitting = true;
                                    this.motion = Tower.MOTION.set;
                                    break;
                                }
                                break;
                            case Tower.MOTION.set:
                                this.animationpoint.X = 3;
                                if (this.frame >= this.time)
                                {
                                    this.hitting = false;
                                    this.frame = 0;
                                    this.motion = Tower.MOTION.end;
                                    break;
                                }
                                break;
                            case Tower.MOTION.end:
                                this.animationpoint.X = 5 - this.frame;
                                if (this.frame >= 5)
                                {
                                    this.frame = 0;
                                    this.flag = false;
                                    break;
                                }
                                break;
                        }
                        break;
                    case ChipBase.ELEMENT.heat:
                    case ChipBase.ELEMENT.aqua:
                    case ChipBase.ELEMENT.leaf:
                    case ChipBase.ELEMENT.earth:
                        switch (this.motion)
                        {
                            case Tower.MOTION.init:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 3)
                                {
                                    this.hitting = true;
                                    this.motion = Tower.MOTION.set;
                                    break;
                                }
                                break;
                            case Tower.MOTION.set:
                                this.animationpoint.X = 3;
                                if (this.frame >= this.time)
                                {
                                    this.hitting = false;
                                    this.frame = 0;
                                    this.motion = Tower.MOTION.end;
                                    if (this.element == ChipBase.ELEMENT.heat && this.StandPanel.state == Panel.PANEL._grass)
                                        this.StandPanel.state = Panel.PANEL._nomal;
                                    break;
                                }
                                break;
                            case Tower.MOTION.end:
                                this.animationpoint.X = 4 - this.frame;
                                if (this.frame >= 3 && this.make)
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
                            case Tower.MOTION.init:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 3)
                                {
                                    this.hitting = true;
                                    this.motion = Tower.MOTION.set;
                                    break;
                                }
                                break;
                            case Tower.MOTION.set:
                                this.animationpoint.X = this.frame;
                                if (this.frame >= 3 && this.make)
                                {
                                    this.hitting = false;
                                    this.frame = 0;
                                    this.motion = Tower.MOTION.end;
                                    break;
                                }
                                break;
                            case Tower.MOTION.end:
                                this.flag = false;
                                break;
                        }
                        break;
                    case ChipBase.ELEMENT.poison:
                        this.animationpoint.X = this.frame;
                        if (this.frame == 4)
                        {
                            this.hitting = false;
                            if (this.StandPanel.state == Panel.PANEL._grass)
                                this.StandPanel.state = Panel.PANEL._poison;
                        }
                        if (this.frame >= 13 && this.make)
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
                case ChipBase.ELEMENT.normal:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 24f);
                    this._rect = new Rectangle(16 + this.animationpoint.X * 16, 448, 16, 56);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, !this.rebirth, Color.White);
                    break;
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
                    this._rect = new Rectangle(128 + this.animationpoint.X * 16, 0, 24, 136);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.leaf:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 30f);
                    this._rect = new Rectangle(this.animationpoint.X * 32, 160, 32, 80);
                    dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.poison:
                    this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - 20f);
                    this._rect = new Rectangle(this.animationpoint.X * 40, 32, 40, 56);
                    dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, Color.White);
                    break;
                case ChipBase.ELEMENT.earth:
                    this._position = this.positionDirect;
                    this._rect = new Rectangle(this.animationpoint.X * 40, 320, 40, 24);
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
