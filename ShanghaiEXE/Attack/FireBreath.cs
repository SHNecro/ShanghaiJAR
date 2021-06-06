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
    internal class FireBreath : AttackBase
    {
        private readonly int nextmake;
        private readonly int panelchange;
        private readonly int endX;

        public FireBreath(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          int endX)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.endX = endX;
            this.nextmake = s;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.blue)
                this.positionDirect = new Vector2(this.position.X * 40 - 8, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 8, this.position.Y * 24 + 42);
            this.frame = 0;
            this.sound.PlaySE(SoundEffect.heat);
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                {
                    this.flag = false;
                    this.parent.effects.Add(new AfterFire(this.sound, this.parent, this.positionDirect, this.position, this.element, this.speed, this.rebirth));
                }
                if (this.frame == this.nextmake)
                {
                    this.hitting = false;
                    this.positionre.X = this.union == Panel.COLOR.red ? this.position.X + 1 : this.position.X - 1;
                    if (this.positionre.X >= 0 && this.positionre.X < 6 && this.positionre.Y >= 0 && this.positionre.Y < 3)
                    {
                        if (this.positionre.X == this.endX)
                        {
                            this.ShakeStart(5, 30);
                            this.sound.PlaySE(SoundEffect.bombmiddle);
                            this.parent.effects.Add(new Bomber(this.sound, this.parent, this.positionre.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 1));
                            this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.positionre.X, this.position.Y, this.union, this.power, 1, this.element)));
                            if (this.element == ChipBase.ELEMENT.heat)
                                this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.positionre.X, this.position.Y, this.union, this.power, 1, 180));
                            if (this.InAreaCheck(new Point(this.positionre.X, this.position.Y)))
                                this.parent.panel[this.positionre.X, this.position.Y].State = Panel.PANEL._crack;
                        }
                        else
                            this.parent.attacks.Add(this.StateCopy(new FireBreath(this.sound, this.parent, this.positionre.X, this.position.Y, this.union, this.power, this.nextmake, this.element, this.endX)));
                    }
                    this.positionre = this.position;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
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
            this._rect = new Rectangle(this.animationpoint.X * 64, 664 + 48 * (int)this.element, 64, 48);
            dg.DrawImage(dg, "towers", this._rect, true, this._position, !this.rebirth, Color.White);
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
