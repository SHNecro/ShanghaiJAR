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
    internal class OrinGigantFlear : AttackBase
    {
        private int boomTime = 60;

        public OrinGigantFlear(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          bool pl, int tm = 60)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            if (!this.flag)
                return;
            this.invincibility = true;
            this.breakinvi = true;
            this.breaking = true;
            this.upprint = false;
            this.speed = s;
            this.animationpoint.X = 10;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.boomTime = tm;
            if (pl)
            {
                if (this.union == Panel.COLOR.red)
                    this.positionDirect = new Vector2(this.position.X * 40 + 5, this.position.Y * 24 + 42);
                else
                    this.positionDirect = new Vector2((this.position.X + 1) * 40 - 5, this.position.Y * 24 + 42);
            }
            else if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 2, this.position.Y * 24 + 28);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 28);
            this.frame = 0;
            this.color = Color.White;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                switch (this.frame)
                {
                    case 1:
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y, this.union, new Point(9, 0), 60, true));
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y - 1, this.union, new Point(1, 2), 60, true));
                        break;
                }
                if (this.frame > 4 && this.hitrange.X < 6)
                    ++this.hitrange.X;
                if (this.frame > 20 && this.hitrange.X >= 6 && this.animationpoint.X > 5)
                    --this.animationpoint.X;
                else if (this.animationpoint.X <= 5)
                {
                    this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.White, 90));
                    this.sound.PlaySE(SoundEffect.bombbig);
                    this.ShakeStart(4, 90);
                    var beamAttack = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.boomTime, new Point(9, 0), ChipBase.ELEMENT.heat));
                    beamAttack.breaking = true;
                    this.parent.attacks.Add(beamAttack);
                    this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y - 1, this.union, this.power, 1, this.boomTime, new Point(1, 0), ChipBase.ELEMENT.heat)));
                    this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y + 1, this.union, this.power, 1, this.boomTime, new Point(1, 0), ChipBase.ELEMENT.heat)));
                    this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, 2, new Point(this.position.X + this.UnionRebirth, this.position.Y - 1), new Point(1, 2), this.union, 36));
                    this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, 2, new Point(this.position.X, this.position.Y), new Point(8, 0), this.union, 36));
                    this.hitting = false;
                    this.flag = false;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 48, 1128, 48, 32);
            dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, this.color);
            for (int index = 1; index < this.hitrange.X; ++index)
            {
                this._position = new Vector2(this.positionDirect.X + index * 48 * this.UnionRebirth + Shake.X, this.positionDirect.Y + Shake.Y);
                if (this.animationpoint.X < 6)
                    this._rect = new Rectangle((index + 1 < this.hitrange.X ? 1 : 0) * 48, 1160, 48, 32);
                else
                    this._rect = new Rectangle((this.animationpoint.X - 5) * 48, 1160, 48, 32);
                dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, this.color);
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
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }
    }
}
