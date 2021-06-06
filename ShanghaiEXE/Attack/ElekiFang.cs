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
    internal class ElekiFang : AttackBase
    {
        public new bool bright = true;
        private readonly int time;
        public bool attack;
        public bool animation;
        private readonly bool single;
        private BombAttack bomb1;
        private BombAttack bomb2;
        private ElekiFangSub ef1;
        private ElekiFangSub ef2;

        public ElekiFang(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          bool single = false)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.single = single;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 48);
            this.frame = 0;
            this.time = 10;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (!this.single)
            {
                if (!this.attack)
                {
                    this.bomb1 = new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.time, new Point(5, 0), this.element);
                    this.bomb2 = new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.time, new Point(5, 0), this.element);
                    this.parent.attacks.Add(this.StateCopy(bomb1));
                    this.parent.attacks.Add(this.StateCopy(bomb2));
                    this.ef1 = new ElekiFangSub(this.sound, this.parent, false, this);
                    this.ef2 = new ElekiFangSub(this.sound, this.parent, true, this);
                    this.parent.effects.Add(ef1);
                    this.parent.effects.Add(ef2);
                    this.attack = true;
                }
                if (this.bright)
                    this.PanelBright();
                if (this.moveflame)
                {
                    this.animation = !this.animation;
                    if (this.frame >= this.time)
                    {
                        this.bomb1.flag = false;
                        this.bomb2.flag = false;
                        this.ef1.flag = false;
                        this.ef2.flag = false;
                        this.flag = false;
                    }
                }
            }
            else
            {
                if (!this.attack)
                {
                    this.bomb1 = new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.time, new Point(5, 0), this.element);
                    this.parent.attacks.Add(this.StateCopy(bomb1));
                    this.attack = true;
                }
                if (this.bright)
                    this.PanelBright();
                if (this.moveflame)
                {
                    this.animation = !this.animation;
                    if (this.frame >= this.time)
                    {
                        this.bomb1.flag = false;
                        this.flag = false;
                    }
                }
            }
            this.FlameControl(2);
        }

        public override void Render(IRenderer dg)
        {
            if (!this.single)
            {
                int y1 = this.animation ? 1232 : 1160;
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y2 = positionDirect.Y;
                shake = this.Shake;
                double y3 = shake.Y;
                double num2 = y2 + y3;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(0, y1, 40, 24);
                dg.DrawImage(dg, "shot", this._rect, true, this._position, this.rebirth, Color.White);
            }
            else
            {
                int y1 = this.animation ? 1304 : 1280;
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y2 = positionDirect.Y;
                shake = this.Shake;
                double y3 = shake.Y;
                double num2 = y2 + y3;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(0, y1, 40, 24);
                dg.DrawImage(dg, "shot", this._rect, true, this._position, this.rebirth, Color.White);
                for (int index = 1; index < 6; ++index)
                {
                    double x3 = positionDirect.X;
                    shake = this.Shake;
                    double x4 = shake.X;
                    double num3 = x3 + x4 + 40 * this.UnionRebirth * index;
                    double y4 = positionDirect.Y;
                    shake = this.Shake;
                    double y5 = shake.Y;
                    double num4 = y4 + y5;
                    this._position = new Vector2((float)num3, (float)num4);
                    this._rect = new Rectangle(40, y1, 40, 24);
                    dg.DrawImage(dg, "shot", this._rect, true, this._position, this.rebirth, Color.White);
                }
            }
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
