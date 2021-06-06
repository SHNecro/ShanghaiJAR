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
    internal class HyperSpanner : AttackBase
    {
        private int attackProcess;
        private bool attackUP;
        private readonly int color;
        private readonly bool utern;
        private readonly int attackSpeed;
        private const int plusy = 70;

        public HyperSpanner(
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
            this.color = color;
            this.speed = s;
            this.attackSpeed = 6;
            this.effecting = true;
            this.breaking = true;
            this.hitting = true;
            this.hitrange = new Point(1, 1);
            if (!this.InAreaCheck(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
            {
                switch (this.position.Y)
                {
                    case 0:
                        this.attackUP = false;
                        break;
                    case 1:
                        this.attackUP = true;
                        break;
                    default:
                        this.attackUP = true;
                        this.position.Y = 1;
                        break;
                }
            }
            this.PositionDirectSet();
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 64);
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.frame % 3 == 0)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 4)
                    this.animationpoint.X = 0;
            }
            if (this.attackProcess == 0)
            {
                if (this.SlideMove(attackSpeed, 0))
                {
                    this.SlideMoveEnd();
                    if (!this.InAreaCheck(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
                    {
                        if (this.position.Y == 0)
                        {
                            this.attackUP = false;
                            this.attackProcess = 1;
                        }
                        else
                        {
                            this.attackUP = true;
                            this.attackProcess = 1;
                        }
                        this.HitFlagReset();
                    }
                }
            }
            else if (this.attackProcess == 1)
            {
                if (this.SlideMove(attackSpeed, this.attackUP ? 2 : 3))
                {
                    this.SlideMoveEnd();
                    if (!this.InAreaCheck(new Point(this.position.X, this.position.Y + (this.attackUP ? -1 : 2))))
                    {
                        this.attackProcess = 2;
                        this.HitFlagReset();
                    }
                }
            }
            else if (this.SlideMove(attackSpeed, 1))
            {
                this.SlideMoveEnd();
                if (!this.InAreaCheck(new Point(this.position.X - this.UnionRebirth, this.position.Y)))
                {
                    this.attackProcess = 0;
                    this.effecting = false;
                    this.flag = false;
                }
            }
            this.FlameControl(1);
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y + 24.0));
            this._rect = new Rectangle(this.animationpoint.X * 80, 384, 80, 72);
            dg.DrawImage(dg, "objects1", this._rect, false, this._position, this.rebirth, Color.White);
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
