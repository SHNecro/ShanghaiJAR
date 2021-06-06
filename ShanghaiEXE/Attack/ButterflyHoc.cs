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
    internal class ButterflyHoc : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        //private readonly int movespeed;
        private int hocSpeed;
        private Vector2 movespeed;
        private bool ready;
        private readonly int aspeed = 4;
        private bool angleDOWN;
        private bool angleLEFT;
        private int animeflame;
        private int refrect;
        private int attackProcess;
        private int manymove = 8;
        private int roopmove;
        private readonly bool enemyaArea;
        private bool up;

        public ButterflyHoc(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele, bool up, int aS)
          : base(so, p, pX, pY, u, po, ele)
        {
            //this.movespeed = movespeed;
            this.invincibility = true;
            //this.breakinvi = true;
            this.breaking = true;
            this.speed = s;
            this.hocSpeed = aS;
            this.positionDirect = v;
            this.positionold = this.position;
            this.OldPD = this.positionDirect;
            //this.animationpoint.X = 2;
            this.up = up;
        }

        public override void Updata()
        {
            this.PanelBright();
            /*
            if (this.ready)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X > 7)
                    this.animationpoint.X = 2;
            }*/

            if (!this.movestart) { }

            if (!this.ready)
            {
                int num = this.waittime / this.aspeed;
                if (this.waittime % this.aspeed == 0)
                {
                    switch (num)
                    {
                        case 1:
                            //this.sound.PlaySE(SoundEffect.shoot);
                            this.sound.PlaySE(SoundEffect.bright);
                            break;
                        case 4://7:
                            //this.counterTiming = false;
                            this.sound.PlaySE(SoundEffect.knife);
                            this.ready = true;
                            this.effecting = true;
                            this.movestart = false;
                            this.waittime = 0;
                            this.attackProcess = 0;
                            this.refrect = 0;

                            this.hitting = true;

                            this.movespeed.X = 5f;
                            this.movespeed.Y = 3f;
                            this.movespeed.X = 5f/ this.hocSpeed;
                            this.movespeed.Y = 3f/ this.hocSpeed;
                            /*
                            int movNum = this.hocSpeed;
                            this.movespeed.X = 5/movNum;
                            this.movespeed.Y = 3/movNum;
                            this.speed = this.speed / movNum;
                            */
                            this.angleDOWN = this.position.Y == 0;
                            if (this.up) { this.angleDOWN = this.position.Y == 1; }
                            this.angleLEFT = this.StandPanel.color == Panel.COLOR.blue;
                            this.manymove = 0;
                            break;
                    }
                }
            }
            else
            {
                if (this.waittime % 3 == 0)
                {
                    ++this.animeflame;
                    if (this.animeflame >= 3)
                        this.animeflame = 0;
                    this.animationpoint.X = 10 + this.animeflame;
                }

                // this shit doesn't work and looks janky as fuck, needs a fix
                //if (this.waittime % 2 == 0)
                //{

                    if (this.manymove >= 8* this.hocSpeed)
                    {
                        bool flag = false;
                        Point poji = new Point(this.position.X, this.position.Y + (this.angleDOWN ? 1 : -1));
                        if (!this.InAreaCheck(poji) || this.parent.panel[poji.X, poji.Y].color == this.union && this.enemyaArea)
                        {
                            this.angleDOWN = !this.angleDOWN;
                            flag = true;
                        }

                        /*
                        poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y);
                        if (!this.InAreaCheck(poji))
                        {
                            this.angleLEFT = !this.angleLEFT;
                            flag = true;
                        }
                        */
                        poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y + (this.angleDOWN ? 1 : -1));
                        this.manymove = 0;
                        ++this.refrect;
                        //if (this.refrect >= 9)
                        if (this.refrect >= 9)
                        {
                            this.roopmove = 0;
                            this.attackProcess = 0;
                            this.effecting = false;
                            /*
                            this.motion = NaviBase.MOTION.move;
                            this.OgreSet();*/
                        }
                        if (flag)
                            this.sound.PlaySE(SoundEffect.bound);
                    }
                    this.positionDirect.X += this.angleLEFT ? -this.movespeed.X : this.movespeed.X;
                    this.positionDirect.Y += this.angleDOWN ? this.movespeed.Y : -this.movespeed.Y;
                    ++this.manymove;
                    if (this.manymove == 4* this.hocSpeed)//(4*this.hocSpeed))
                    {
                        this.position.X += this.angleLEFT ? -1 : 1;
                        this.position.Y += this.angleDOWN ? 1 : -1;
                        this.HitFlagReset();
                    }
                //}
            }
            ++this.waittime;

            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;

            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 16, 56, 16, 30);
            dg.DrawImage(dg, "yuyukoAttack", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            /*
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.ShakeStart(4, 60);
            this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, 2, this.UnionEnemy, this.union, 18));
            for (int index1 = 0; index1 < this.parent.panel.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.parent.panel.GetLength(1); ++index2)
                {
                    if (this.parent.panel[index1, index2].color == this.UnionEnemy)
                        this.parent.panel[index1, index2].State = Panel.PANEL._crack;
                }
            }*/
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;

            this.sound.PlaySE(SoundEffect.dark);
            /*
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.ShakeStart(4, 60);
            this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, 2, this.UnionEnemy, this.union, 18));
            for (int index1 = 0; index1 < this.parent.panel.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.parent.panel.GetLength(1); ++index2)
                {
                    if (this.parent.panel[index1, index2].color == this.UnionEnemy)
                        this.parent.panel[index1, index2].State = Panel.PANEL._crack;
                }
            }*/
            this.flag = false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.sound.PlaySE(SoundEffect.dark);
            /*
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, p.position.X, p.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            */
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.sound.PlaySE(SoundEffect.dark);
            /*
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, e.position.X, e.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            */
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.sound.PlaySE(SoundEffect.dark);
            /*
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, o.position.X, o.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            */
            return true;
        }
    }
}
