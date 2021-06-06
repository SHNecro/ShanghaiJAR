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
    internal class PoisonBall : AttackBase
    {
        private readonly int time;
        private int anime;
        private new bool bright;
        private Vector2 plusposi;
        private int count;

        private int Anime
        {
            get
            {
                return this.anime;
            }
            set
            {
                this.anime = value;
                if (this.anime <= 6)
                    return;
                this.anime = 0;
            }
        }

        public PoisonBall(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Vector2 positionDirect,
          Panel.COLOR u,
          int po,
          int s,
          int time,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.upprint = true;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(2, 2);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = positionDirect;
            Vector2 vector2 = this.union != Panel.COLOR.red ? new Vector2((pX + 1) * 40 - 16, pY * 24 + 80) : new Vector2(pX * 40 + 16, pY * 24 + 80);
            this.position.X -= this.UnionRebirth;
            --this.position.Y;
            this.frame = 0;
            this.time = time;
            this.plusposi = new Vector2((vector2.X - positionDirect.X) / time, (vector2.Y - positionDirect.Y) / time);
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.count % 10 == 0)
                this.bright = !this.bright;
            if (this.bright)
                this.PanelBright();
            this.positionDirect.X += this.plusposi.X;
            this.positionDirect.Y += this.plusposi.Y;
            if (this.count >= this.time)
            {
                this.sound.PlaySE(SoundEffect.bombmiddle);
                this.hitting = true;
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    for (int index2 = 0; index2 < 3; ++index2)
                    {
                        if (this.InAreaCheck(new Point(this.position.X + index1 * this.UnionRebirth, this.position.Y + index2)))
                            this.parent.panel[this.position.X + index1 * this.UnionRebirth, this.position.Y + index2].State = Panel.PANEL._poison;
                    }
                }
                this.ShakeStart(4, 60);
                this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.poison, 2, this.position, this.hitrange, this.union, 18));
                this.flag = false;
            }
            ++this.count;
            if (this.moveflame)
            {
                ++this.Anime;
                this.animationpoint.X = this.anime;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.frame >= this.time)
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
            this._rect = new Rectangle(this.animationpoint.X * 40, 352, 40, 40);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
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
