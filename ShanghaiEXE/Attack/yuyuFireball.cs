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
    internal class yuyuFireball : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;

        public yuyuFireball(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.invincibility = true;
            //this.breakinvi = true;
            this.breaking = true;
            this.speed = s;
            //this.positionDirect = v;
            this.positionDirect.X = this.position.X*40;
            this.positionDirect.Y = 82 + 24 * this.position.Y;
            this.positionold = this.position;
            this.OldPD = this.positionDirect;
            //this.animationpoint.X = 2;
        }

        public override void Updata()
        {
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
                    if (this.animationpoint.X > 7)
                        this.animationpoint.X = 6;
                }
                if (this.movestart)
                {
                    /*
                    if (this.position.X != this.positionold.X)
                    {
                        this.positionold.X = this.position.X;
                        //this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.positionold.X, this.positionold.Y, this.union, 100, 2, 120));
                    }*/
                    this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                    this.position.X = this.Calcposition(this.positionDirect, 36, true).X;
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
            }
            if (!this.movestart) { }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            int xOff, yOff;
            xOff = 20-5;
            yOff = -14;
            this._position = new Vector2(this.positionDirect.X + Shake.X + xOff, this.positionDirect.Y + Shake.Y + yOff);
            this._rect = new Rectangle(this.animationpoint.X * 50, 0, 50, 35);
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
