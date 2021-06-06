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
    internal class GravityBallAttack : AttackBase
    {
        private bool movestart = false;
        public float Yspeed = 0.5f;
        public float Xspeed = 1f;
        private const int plusy = 70;

        public GravityBallAttack(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.invincibility = false;
            this.speed = s;
            this.badstatus[6] = true;
            this.badstatustime[6] = 180;
            this.positionDirect = v;
            this.positionDirect.X = this.position.X * 40 + 20;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.sound.PlaySE(SoundEffect.chain);
            }
            if (this.moveflame)
            {
                if (this.frame % 3 == 0)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 3)
                        this.animationpoint.X = 0;
                }
                if (this.movestart)
                {
                    int num1 = 0;
                    int num2;
                    if (this.union == Panel.COLOR.blue)
                    {
                        num2 = 70 + this.parent.player.position.Y * 24;
                    }
                    else
                    {
                        int num3 = 99;
                        foreach (CharacterBase characterBase in this.parent.AllChara())
                        {
                            if (num3 > characterBase.position.X && characterBase.union == Panel.COLOR.blue)
                            {
                                num3 = characterBase.position.X;
                                num1 = characterBase.position.Y;
                            }
                        }
                        num2 = 70 + num1 * 24;
                    }
                    if (positionDirect.Y > (double)num2)
                        this.positionDirect.Y -= this.Yspeed;
                    else if (positionDirect.Y < (double)num2)
                        this.positionDirect.Y += this.Yspeed;
                    this.positionDirect.X += this.union == Panel.COLOR.red ? this.Xspeed : -this.Xspeed;
                    this.position = this.Calcposition(this.positionDirect, 36, false);
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
            }
            if (!this.movestart) { }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 32, 256, 32, 32);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, ChipBase.ELEMENT.eleki));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, ChipBase.ELEMENT.eleki));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, ChipBase.ELEMENT.eleki));
            return true;
        }
    }
}
