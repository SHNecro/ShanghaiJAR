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
    internal class yuyuMine : AttackBase
    {
        private readonly int roop = 0;
        public int hits = 8;
        private readonly Panel.PANEL panel;
        public bool stealth;
        private int count;

        public yuyuMine(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          Panel.PANEL panel,
          bool stealth,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.stealth = stealth;
            this.panel = panel;
            this.downprint = true;
            this.invincibility = true;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 8, this.position.Y * 24 + 70);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 - 8, this.position.Y * 24 + 70);
            this.frame = 0;
            this.count = 0;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            
            this.count++;
            int frDel = 3;
            int aniLim = 5;
            if (this.count == frDel)
            {
                this.animationpoint.X++;
                if (this.animationpoint.X > aniLim) { this.animationpoint.X = 1; }
                this.count = 0;
            }
            /*
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X > 7)
                    this.animationpoint.X = 0;
            }*/


            if (this.frame >= 2800 || this.StandPanel.Hole)
            {
                this.parent.effects.Add(new Smoke(this.sound, this.parent, this.positionre.X, this.positionre.Y, ChipBase.ELEMENT.normal));
                this.sound.PlaySE(SoundEffect.heat);
                this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || (!this.flag || this.stealth && this.union != Panel.COLOR.red))
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            int xOff, yOff;
            xOff = 8+4;
            yOff = 3;
            this._position = new Vector2((float)num1+xOff, (float)num2+yOff);
            this._rect = new Rectangle(animationpoint.X*50, 36, 50, 20);
            dg.DrawImage(dg, "yuyukoAttack", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!this.flag || !base.HitCheck(charaposition))
                return false;
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.ShakeStart(4, 60);
            for (int index1 = 0; index1 < this.parent.panel.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.parent.panel.GetLength(1); ++index2)
                {
                    if (this.parent.panel[index1, index2].color == this.UnionEnemy)
                        this.parent.panel[index1, index2].State = this.panel;
                }
            }
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!this.flag || !base.HitCheck(charaposition))
                return false;
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.ShakeStart(4, 60);
            for (int index1 = 0; index1 < this.parent.panel.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.parent.panel.GetLength(1); ++index2)
                {
                    if (this.parent.panel[index1, index2].color == this.UnionEnemy)
                        this.parent.panel[index1, index2].State = this.panel;
                }
            }
            this.flag = false;
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
