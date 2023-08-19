using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class OrinSummonUtsuhoVerti : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;
        private int palette;
        private int manymove = 0;

        public OrinSummonUtsuhoVerti(
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
            //this.breaking = true;
            this.speed = s;
            this.positionDirect = v;
            this.positionold = this.position;
            this.OldPD = this.positionDirect;
            //this.animationpoint.X = 2;
            this.palette = pal;
            //this.positionDirect.Y -= 140;
        }

        public override void Updata()
        {
            //this.PanelBright();
            /*
            if (this.frame >= 22 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.frame = 0;
            }
            if (this.moveflame)
            {
                this.animationpoint = AnimeDive(this.frame);
                if (this.movestart)
                {
                    this.positionDirect.Y += movespeed;
                    this.position.Y = this.Calcposition(this.positionDirect, 130, true).X;
                    if (positionDirect.Y > 400.0)
                        this.flag = false;
                }
            }
            if (!this.movestart)
            {
                this.animationpoint = AnimeStartup(this.frame);
            }

            */
            //this.PanelBright();

            if (!this.movestart)
            {
                this.animationpoint = AnimeStartup(this.frame);
                switch (this.frame)
                {
                    case 1:
                        this.sound.PlaySE(SoundEffect.warp);
                        break;
                    case 8:
                        this.sound.PlaySE(SoundEffect.dark);
                        break;
                    case 18:
                        this.sound.PlaySE(SoundEffect.futon);
                        break;
                    case 22:
                        this.movestart = true;
                        //this.hitting = true;
                        this.frame = 0;
                        break;
                }
            }
            /*
            if (this.frame >= 22 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.frame = 0;
            }*/

            if (this.movestart)
            {
                this.animationpoint = AnimeDive(this.frame);
                this.positionDirect.Y += movespeed;
                this.manymove += movespeed;
                /*
                ++this.manymove;
                if (this.manymove == 4 * this.movespeed)
                {
                    //this.position.Y += 1;
                    //this.HitFlagReset();
                }
                */
                int baseAdj = 8;
                int tileHeight = 24;
                if (this.manymove > baseAdj + tileHeight)
                {
                    this.hitting = true;
                    this.position.Y = 0;
                }
                if (this.manymove > baseAdj + tileHeight * 2)
                {
                    this.position.Y = 1;
                }
                if (this.manymove > baseAdj + tileHeight * 3)
                {
                    this.position.Y = 2;
                }
                if (this.manymove > baseAdj + tileHeight * 4)
                {
                    this.hitting = false;
                }

                if (this.hitting)
                {
                    this.PanelBright();
                }

                //this.position.Y = this.Calcposition(this.positionDirect, 130, true).X;
                if (this.positionDirect.Y > 240)
                    this.flag = false;
            }
            else
            {
                
            }

            this.FlameControl();
        }

        private Point AnimeStartup(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[12] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 + 60 },
                new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 },
                0,
                waittime
                );
        }

        public Point AnimeDive(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 2, 2, 2 + 60 },
                new int[3] { 12, 13, 14 },
                0,
                waittime
                );
        }

        public override void Render(IRenderer dg)
        {
            /*
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 50, 0, 50, 50);
            dg.DrawImage(dg, "OrinAttack1", this._rect, false, this._position, this.rebirth, Color.White);

            */

            int adj = 0;
            if (this.palette == 2) { adj = 260; }
            if (this.palette == 3) { adj = 260*2; }
            if (this.palette == 4) { adj = 260*3; }



            this._rect = new Rectangle(140 * this.animationpoint.X, adj + 130 * this.animationpoint.Y, 140, 130);
            //this._position = new Vector2(this.position.X * 40f + 24 * this.UnionRebirth + 42, (float)(this.position.Y * 24.0 + 22.0));
            //this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "OrinAttack2", this._rect, false, this._position, this.union == Panel.COLOR.red, Color.White);

        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            //this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;

            this.sound.PlaySE(SoundEffect.dark);


            //this.flag = false;
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
