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
    internal class CanonBullet : AttackBase
    {
        private const int hit = 8;
        private readonly int movespeed;
        private readonly bool spled;

        public CanonBullet(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Vector2 positionDirect,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          bool spled = false)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.spled = spled;
            this.invincibility = true;
            this.movespeed = 8;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.breaking = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = positionDirect;
            this.frame = 0;
            this.OldPD = positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                if (this.animationpoint.X < 8)
                    ++this.animationpoint.X;
                if (this.frame == 5)
                    this.frame = 0;
            }
            this.positionDirect.X += this.movespeed * this.UnionRebirth;
            this.position.X = this.Calcposition(this.positionDirect, 16, false).X;
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * 16, 952, 16, 8);
            dg.DrawImage(dg, "shot", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            this.ShakeStart(2, 16);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, charaposition.X, charaposition.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            if (this.spled)
            {
                int num1 = -1;
                int num2 = -1;
                for (int index = 0; index < 8; ++index)
                {
                    switch (index)
                    {
                        case 0:
                            num1 = -1;
                            num2 = -1;
                            break;
                        case 1:
                            num1 = 0;
                            num2 = -1;
                            break;
                        case 2:
                            num1 = 1;
                            num2 = -1;
                            break;
                        case 3:
                            num1 = -1;
                            num2 = 0;
                            break;
                        case 4:
                            num1 = 1;
                            num2 = 0;
                            break;
                        case 5:
                            num1 = -1;
                            num2 = 1;
                            break;
                        case 6:
                            num1 = 0;
                            num2 = 1;
                            break;
                        case 7:
                            num1 = 1;
                            num2 = 1;
                            break;
                    }
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, charaposition.X + num1, charaposition.Y + num2, Bomber.BOMBERTYPE.flashbomber, 2));
                    this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, charaposition.X + num1, charaposition.Y + num2, this.union, this.power, 1, this.element)));
                }
            }
            return true;
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
