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
    internal class TornadeSide : AttackBase
    {
        public new bool bright = true;
        private int count;
        public bool effect;

        public TornadeSide(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.upprint = true;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 40, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 40, this.position.Y * 24 + 48);
            this.frame = 0;
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.element))));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.power, 1, this.element))));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y, this.union, this.power, 1, this.element))));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y, this.union, this.power, 1, this.element))));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y - 1, this.union, this.power, 1, this.element))));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y + 1, this.union, this.power, 1, this.element))));
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 6)
                {
                    this.animationpoint.X = 0;
                    switch (this.count)
                    {
                        case 2:
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y, this.union, this.power, 1, this.element))));
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y, this.union, this.power, 1, this.element))));
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y - 1, this.union, this.power, 1, this.element))));
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y + 1, this.union, this.power, 1, this.element))));
                            break;
                        case 4:
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y, this.union, this.power, 1, this.element))));
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y - 1, this.union, this.power, 1, this.element))));
                            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y + 1, this.union, this.power, 1, this.element))));
                            break;
                        case 5:
                            this.flag = false;
                            break;
                    }
                    ++this.count;
                }
            }
            this.FlameControl(2);
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(112 * this.animationpoint.X, 336, 112, 56);
            dg.DrawImage(dg, "tornado", this._rect, true, this._position, this.rebirth, Color.White);
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
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
