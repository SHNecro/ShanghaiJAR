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
    internal class ChenBoomerang : AttackBase
    {
        private int attackProcess;
        private bool attackUP;
        private new readonly int color;
        private readonly int attackSpeed;
        private int spins;
        private Shadow shadow_;
        private const int plusy = 70;

        public ChenBoomerang(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int color,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.invincibility = true;
            this.color = color;
            this.speed = s;
            this.attackSpeed = 16;
            this.effecting = true;
            this.hitting = true;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 64);
            if (this.InAreaCheck(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
                return;
            switch (this.position.Y)
            {
                case 0:
                    this.attackUP = false;
                    this.attackProcess = 1;
                    break;
                case 2:
                    this.attackUP = true;
                    this.attackProcess = 1;
                    break;
                default:
                    this.HitFlagReset();
                    this.attackProcess = 2;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 64);
        }

        public override void Updata()
        {
            if (this.parent.panel[this.position.X, this.position.Y].Hole)
            {
                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                this.flag = false;
            }
            this.PanelBright();
            if (this.shadow_ == null)
            {
                IAudioEngine sound = this.sound;
                SceneBattle parent = this.parent;
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double num2 = positionDirect.Y + 24.0;
                shake = this.Shake;
                double y = shake.Y;
                double num3 = num2 + y;
                Vector2 pd = new Vector2((float)num1, (float)num3);
                Point position = this.position;
                this.shadow_ = new Shadow(sound, parent, pd, position, this);
                this.parent.effects.Add(shadow_);
            }
            else
                this.shadow_.positionDirect = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + 24f + Shake.Y);
            if (this.moveflame && this.frame % 3 == 0)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 3)
                    this.animationpoint.X = 0;
            }
            if (this.attackProcess == 0)
            {
                if (this.SlideMove(attackSpeed, 0))
                {
                    this.SlideMoveEnd();
                    if (!this.InAreaCheck(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
                    {
                        switch (this.position.Y)
                        {
                            case 0:
                                this.attackUP = false;
                                this.attackProcess = 1;
                                break;
                            case 2:
                                this.attackUP = true;
                                this.attackProcess = 1;
                                break;
                            default:
                                this.HitFlagReset();
                                this.attackProcess = 2;
                                break;
                        }
                    }
                }
            }
            else if (this.attackProcess == 1)
            {
                if (this.SlideMove(attackSpeed, this.attackUP ? 2 : 3))
                {
                    this.SlideMoveEnd();
                    if (!this.InAreaCheck(new Point(this.position.X, this.position.Y + (this.attackUP ? -1 : 1))))
                        this.attackProcess = 2;
                }
            }
            else if (this.attackProcess == 2)
            {
                if (this.SlideMove(attackSpeed, 1))
                {
                    this.SlideMoveEnd();
                    if (!this.InAreaCheck(new Point(this.position.X - this.UnionRebirth, this.position.Y)))
                    {
                        ++this.spins;
                        if (this.spins < 3)
                        {
                            switch (this.position.Y)
                            {
                                case 0:
                                    this.attackUP = false;
                                    this.attackProcess = 3;
                                    break;
                                case 2:
                                    this.attackUP = true;
                                    this.attackProcess = 3;
                                    break;
                                default:
                                    this.HitFlagReset();
                                    this.attackProcess = 0;
                                    break;
                            }
                        }
                        else
                        {
                            this.attackProcess = 0;
                            this.effecting = false;
                            this.flag = false;
                        }
                    }
                }
            }
            else if (this.attackProcess == 3 && this.SlideMove(attackSpeed, this.attackUP ? 2 : 3))
            {
                this.SlideMoveEnd();
                if (!this.InAreaCheck(new Point(this.position.X, this.position.Y + (this.attackUP ? -1 : 1))))
                {
                    this.HitFlagReset();
                    this.sound.PlaySE(SoundEffect.knife);
                    switch (this.position.Y)
                    {
                        case 0:
                            this.attackUP = false;
                            this.attackProcess = 0;
                            break;
                        case 2:
                            this.attackUP = true;
                            this.attackProcess = 0;
                            break;
                        default:
                            this.HitFlagReset();
                            this.attackProcess = 2;
                            break;
                    }
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((float)(positionDirect.X + (double)this.Shake.X - 12.0), (float)(positionDirect.Y + (double)this.Shake.Y - 16.0));
            this._rect = new Rectangle((this.animationpoint.X + 6) * 128, this.color * 96 * 2, 128, 96);
            dg.DrawImage(dg, "chen", this._rect, false, this._position, this.rebirth, Color.White);
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
    }
}
