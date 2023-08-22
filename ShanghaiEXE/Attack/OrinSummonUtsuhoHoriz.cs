using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;
using NSAttack;

namespace NSAttack
{
    internal class OrinSummonUtsuhoHoriz : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;
        private int palette;

        public OrinSummonUtsuhoHoriz(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed,
          int pal)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.invincibility = true;
            //this.breakinvi = true;
            this.breaking = true;
            this.speed = s;
            this.positionDirect = v;
            this.positionold = this.position;
            this.OldPD = this.positionDirect;
            //this.animationpoint.X = 2;
            this.palette = pal;
        }

        public override void Updata()
        {
            /*
            this.PanelBright();
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
            }
            if (this.moveflame)
            {
                if (this.moveflame)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 3)
                        this.animationpoint.X = 0;
                }
                if (this.movestart)
                {
                    this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                    this.position.X = this.Calcposition(this.positionDirect, 36, true).X;
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
            }
            if (!this.movestart) { }
            this.FlameControl();
            */
            this.PanelBright();
            switch (this.frame)
            {
                case 1:
                    this.animationpoint.X = 0;
                    this.animationpoint.Y = 0;
                    this.sound.PlaySE(SoundEffect.warp);
                    break;
                case 8:
                    this.sound.PlaySE(SoundEffect.futon);
                    this.animationpoint.X = 0;
                    this.animationpoint.Y = 3;
                    break;
                case 12:
                    this.animationpoint.X = 1;
                    break;
                case 18:
                    this.animationpoint.X = 2;
                    break;
                case 30:
                    this.sound.PlaySE(SoundEffect.shoot);

                    var sp = 0;
                    switch (this.palette)
                    {
                        case 2: sp = 2; break;
                        case 3: sp = 1; break;
                        case 4: sp = 3; break;
					}
                    UthuhoChip attackBase2 = new UthuhoChip(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, sp);
                    attackBase2.breaking = true;
                        
                    this.parent.attacks.Add(attackBase2);
                    this.flag = false;
                    break;
            }

            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            /*
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 50, 0, 50, 50);
            dg.DrawImage(dg, "OrinAttack1", this._rect, false, this._position, this.rebirth, Color.White);

            */
            
                int num = 0;
                if (this.palette == 3 || this.palette == 4)
                    num = 2160;
                if (this.palette == 2)
                    num = 1440;
                this._rect = new Rectangle(120 * this.animationpoint.X, num + 144 * this.animationpoint.Y, 120, 144);
                this._position = new Vector2(this.position.X * 40f + 24 * this.UnionRebirth + 32, (float)(this.position.Y * 24.0 + 22.0));
                if (this.palette != 4)
                    dg.DrawImage(dg, "Uthuho", this._rect, false, this._position, this.union == Panel.COLOR.red, Color.White);
                else
                    dg.DrawImage(dg, "UthuhoAlter", this._rect, false, this._position, this.union == Panel.COLOR.red, Color.White);

        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;

            this.sound.PlaySE(SoundEffect.dark);


            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.sound.PlaySE(SoundEffect.dark);


            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            return true;
        }
    }
}
