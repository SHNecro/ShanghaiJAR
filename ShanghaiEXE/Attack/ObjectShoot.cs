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
    internal class ObjectShoot : AttackBase
    {
        private const int hit = 8;
        private readonly int objects;
        private readonly int movespeed;

        public ObjectShoot(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Vector2 positionDirect,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.objects = this.Random.Next(10);
            this.invincibility = false;
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
            if (this.moveflame && this.frame == 5)
                this.frame = 0;
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
            switch (this.objects)
            {
                case 0:
                    this._rect = new Rectangle(0, 0, 32, 48);
                    this.picturename = "objects1";
                    break;
                case 1:
                    this._rect = new Rectangle(0, 48, 32, 48);
                    this.picturename = "objects1";
                    break;
                case 2:
                    this._rect = new Rectangle(0, 144, 40, 40);
                    this.picturename = "objects1";
                    break;
                case 3:
                    this._rect = new Rectangle(16, 0, 16, 16);
                    this.picturename = "bombs";
                    break;
                case 4:
                    this._rect = new Rectangle(32, 0, 16, 16);
                    this.picturename = "bombs";
                    break;
                case 5:
                    this._rect = new Rectangle(0, 256, 32, 32);
                    this.picturename = "bomber";
                    break;
                case 6:
                    this._rect = new Rectangle(0, 808, 32, 32);
                    this.picturename = "shot";
                    break;
                case 7:
                    this._rect = new Rectangle(80, 960, 56, 56);
                    this.picturename = "shot";
                    break;
                case 8:
                    this._rect = new Rectangle(120, 40, 40, 40);
                    this.picturename = "junks";
                    break;
                default:
                    this._rect = new Rectangle(360, 0, 40, 48);
                    this.picturename = "cirno";
                    break;
            }
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, this.picturename, this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            this.ShakeStart(2, 16);
            this.sound.PlaySE(SoundEffect.breakObject);
            this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, true, 0));
            this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, false, 0));
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            this.ShakeStart(2, 16);
            this.sound.PlaySE(SoundEffect.breakObject);
            this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, true, 0));
            this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, false, 0));
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
