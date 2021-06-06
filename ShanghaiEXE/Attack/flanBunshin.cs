using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class flanBunshin : AttackBase
    {
        //private readonly FlanGear.MOTION motion;
        private readonly int time;
        int[] knifeX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] knifeY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int passPower;
        int preX;
        int preY;

        public flanBunshin(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.passPower = po;
            this.preX = pX;
            this.preY = pY;

            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 0, this.position.Y * 24 + 82);
            this.frame = 0;
            /*
            if (this.InArea)
            {
                if (this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._break)
                    return;
                this.canCounter = false;
                this.time = 1;
                this.speed = 3;
                this.invincibility = true;
                this.animationpoint.X = 0;
                this.hitrange = new Point(0, 0);
                this.breaking = true;
                this.hitting = false;
                this.rebirth = this.union == Panel.COLOR.red;
                this.positionre = this.position;
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
                this.sound.PlaySE(SoundEffect.wave);
                this.motion = m;
                this.frame = 0;
            }
            else
                this.flag = false;
            */
        }

        public override void Updata()
        {
            this.animationpoint = this.gearSet(this.frame);
            if (this.over)
                return;

            switch (this.frame)
            {
                case 2:
                    //this.preX = this.position.X;
                    //this.preY = this.position.Y;

                    this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, 0);

                    Point point1 = this.positionre;
                    this.positionre = this.position;
                    this.position.X = this.preX;
                    this.position.Y = this.preY;
                    knifeX[0] = point1.X;
                    knifeY[0] = point1.Y;
                    int gTime = 60 + 22;
                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y, this.union, new Point(), gTime, true));
                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X - 1, point1.Y, this.union, new Point(), gTime, true));
                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X + 1, point1.Y, this.union, new Point(), gTime, true));
                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y - 1, this.union, new Point(), gTime, true));
                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, point1.X, point1.Y + 1, this.union, new Point(), gTime, true));
                    break;
                case 34:
                    //this.counterTiming = false;
                    //this.sound.PlaySE(SoundEffect.beam);
                    //Point point1 = this.RandomTarget();

                    //this.parent.attacks.Add(new Beam(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, false));
                    this.parent.attacks.Add(new FlanGear(this.sound, this.parent, knifeX[0], knifeY[0], this.union, this.passPower, 0));
                    this.over = true;
                    this.flag = false;
                    break;
                
                
            }

            this.FlameControl();
            /*
            if (this.moveflame)
            {
                //this.animationpoint.X = this.frame;
                //if (60 % this.frame == 12) { this.animationpoint.X++; }
                if (this.frame == 2) { this.animationpoint.X++; }
                if (this.frame == 4) { this.animationpoint.X++; }
                if (this.frame == 6) { this.animationpoint.X++; }
                if (this.frame == 8) { this.animationpoint.X++; }
                if (this.frame > 8 && (this.frame % 2 == 0)) { this.animationpoint.X = 3; }
                if (this.frame > 8 && (this.frame % 2 != 0)) { this.animationpoint.X = 4; }
                //this.hitting = true;
                switch (this.frame)
                {
                    case 2:
                        this.hitting = true;
                        break;
                    case 19:
                        this.hitting = false;
                        break;
                    case 20:
                        this.flag = false;
                        this.over = true;
                        break;
                }
            }*/
            //this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            int xOff = 4;
            int yOff = -12;

            this._position = new Vector2(this.positionDirect.X + Shake.X + xOff, this.positionDirect.Y - 4f + Shake.Y + yOff);
            //this._position = new Vector2(this.position.X, this.position.Y);
            this._rect = new Rectangle(this.animationpoint.X * 78, 0, 78, 78);
            dg.DrawImage(dg, "flandre", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private Point gearSet(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        4,
        4,
        4,
        4,
        4,
        4
            }, new int[6] { 9, 10, 11, 12, 13, 14 }, 0, waittime);
        }

    }
}
