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
    internal class ShotGun : AttackBase
    {
        private int roop = 0;
        public int hit = 3;

        public ShotGun(
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
            this.invincibility = false;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 4, this.position.Y * 24 + 42);
            this.frame = 0;
            this.sound.PlaySE(SoundEffect.bomb);
            this.parent.effects.Add(new Basterhit(this.sound, this.parent, this.position.X, this.position.Y, 1));
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                {
                    this.frame = 0;
                    ++this.roop;
                    if (this.roop >= this.hit)
                    {
                        this.flag = false;
                    }
                    else
                    {
                        this.sound.PlaySE(SoundEffect.bomb);
                        this.parent.effects.Add(new Basterhit(this.sound, this.parent, this.position.X, this.position.Y, 1));
                        if (this.InArea)
                            this.hitflag[this.position.X, this.position.Y] = false;
                    }
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
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
