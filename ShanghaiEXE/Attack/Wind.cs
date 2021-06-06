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
    internal class Wind : AttackBase
    {
        private const int hit = 3;
        private readonly int movespeed;
        private readonly int movelong;
        private readonly bool push;
        private int realflame;

        public Wind(IAudioEngine so, SceneBattle p, int pX, int pY, Panel.COLOR u, bool push)
          : base(so, p, pX, pY, u, 0, ChipBase.ELEMENT.normal)
        {
            if (!this.flag)
                return;
            this.push = push;
            this.invincibility = false;
            this.movespeed = 8;
            this.speed = 8;
            this.realflame = 30;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 50);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 50);
            this.frame = 0;
            if (this.union == Panel.COLOR.blue)
                this.movespeed *= -1;
            this.position.X = (int)(positionDirect.X / 40.0);
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.StandPanel.state == Panel.PANEL._sand)
            {
                this.StandPanel.state = Panel.PANEL._nomal;
                this.flag = false;
                this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X, this.position.Y, ChipBase.ELEMENT.earth));
            }
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                    this.frame = 0;
            }
            ++this.realflame;
            foreach (CharacterBase characterBase in this.parent.AllChara())
            {
                if (characterBase.position == this.position && characterBase.union == this.UnionEnemy && !characterBase.nohit)
                {
                    characterBase.Knockbuck(this.push, true, this.union);
                    if (characterBase.barrierType != CharacterBase.BARRIER.PowerAura || characterBase.barierPower < 200)
                    {
                        characterBase.barierTime = 0;
                        characterBase.barrierType = CharacterBase.BARRIER.None;
                    }
                }
            }
            if (this.push)
                this.positionDirect.X += movespeed;
            else
                this.positionDirect.X -= movespeed;
            this.position.X = (int)(positionDirect.X / 40.0);
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
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
            double num2 = positionDirect.Y + 8.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(64, 496, 32, 24);
            dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, Color.White);
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
            p.Knockbuck(true, false, this.union);
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            e.Knockbuck(true, false, this.union);
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            o.Knockbuck(true, o.overslip, this.union);
            return base.HitEvent(o);
        }
    }
}
