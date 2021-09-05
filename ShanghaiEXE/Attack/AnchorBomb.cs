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
    internal class AnchorBomb : AttackBase
    {
        private readonly AnchorBomb.TYPE type;
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private readonly int heattime;
        private readonly int colorA;
        private bool fall;

        public AnchorBomb(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          Point end,
          int t,
          AnchorBomb.TYPE ty,
          int color = -1)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.aqua)
        {
            this.breaking = true;
            this.hitting = false;
            this.speed = s;
            this.positionDirect = v;
            this.time = t;
            this.position = end;
            this.endposition = new Vector2(end.X * 40 + 20, end.Y * 24 + 80);
            this.movex = (v.X - this.endposition.X) / t;
            this.movey = (v.Y - this.endposition.Y) / t;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / time;
            this.rebirth = this.union == Panel.COLOR.red;
            this.type = ty;
            if (color >= 0)
                this.colorA = color;
            else
                this.colorA = (int)this.type;
        }

        public override void Updata()
        {
            if (!this.fall)
            {
                if (this.frame % 5 == 0)
                    this.bright = !this.bright;
                if (this.bright)
                    this.PanelBright();
                if (this.frame == this.time)
                {
                    this.fall = true;
                    this.frame = 0;
                    this.speed = -6;
                }
                else
                {
                    this.positionDirect.X -= this.movex;
                    this.positionDirect.Y -= this.movey;
                    this.plusy += this.speedy;
                    if (speedy > 0.0)
                        this.speedy -= this.plusing;
                    this.plusy += 60 / this.time;
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
                this.FlameControl();
            }
            else if (plusy > 0.0)
            {
                if (this.moveflame && this.speed < 16)
                    this.speed += 4;
                this.FlameControl(4);
                this.plusy -= speed;
                this.frame = 0;
                if (plusy <= 0.0)
                {
                    if (this.StandPanel.Hole)
                    {
                        this.flag = false;
                    }
                    else
                    {
                        int sp = 4;
                        if (this.type == AnchorBomb.TYPE.singleG)
                            this.StandPanel.state = Panel.PANEL._crack;
                        else
                            this.StandPanel.Crack();
                        BombAttack bombAttack = new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.element)
                        {
                            breaking = true
                        };
                        this.parent.attacks.Add(this.StateCopy(bombAttack));
                        this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y, sp));
                        this.sound.PlaySE(SoundEffect.clincher);
                        this.ShakeStart(5, 30);
                        switch (this.type)
                        {
                            case AnchorBomb.TYPE.line:
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y - 1, sp));
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y + 1, sp));
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element)));
                                break;
                            case AnchorBomb.TYPE.closs:
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y - 1, sp));
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X, this.position.Y + 1, sp));
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element)));
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X + 1, this.position.Y, sp));
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.element)));
                                this.parent.effects.Add(new Water(this.sound, this.parent, this.position.X - 1, this.position.Y, sp));
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.element)));
                                break;
                        }
                    }
                }
            }
            else
            {
                this.plusy = 0.0f;
                ++this.frame;
                if (this.frame > 60)
                    this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.fall)
            {
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double num2 = positionDirect.Y - plusy / 3.0 * 2.0;
                shake = this.Shake;
                double y = shake.Y;
                double num3 = num2 + y;
                this._position = new Vector2((float)num1, (float)num3);
                this._rect = new Rectangle(16, 0, 16, 16);
                dg.DrawImage(dg, "bombs", this._rect, false, this._position, this.rebirth, Color.White);
            }
            else
            {
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double num2 = positionDirect.Y - plusy / 3.0 * 2.0;
                shake = this.Shake;
                double y = shake.Y;
                double num3 = num2 + y - 16.0;
                this._position = new Vector2((float)num1, (float)num3);
                this._rect = new Rectangle(48 * this.colorA, 40, 48, 56);
                dg.DrawImage(dg, "bombs", this._rect, false, this._position, this.rebirth, Color.White);
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

        public enum TYPE
        {
            single,
            line,
            closs,
            singleG,
        }
    }
}
