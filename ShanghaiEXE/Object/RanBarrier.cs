using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class RanBarrier : ObjectBase
    {
        private bool breaked;
        private int process;
        private readonly bool sp;
        private readonly bool barrieranime;
        private readonly int roop;
        private int nowroop;

        public RanBarrier(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          int power,
          int speed,
          Panel.COLOR union,
          int roop,
          bool sp)
          : base(s, p, pX, pY, union)
        {
            this.roop = roop;
            this.sp = sp;
            this.height = 128;
            this.wide = 96;
            this.hp = 10;
            this.hitPower = power;
            this.hpmax = this.hp;
            this.hitbreak = false;
            this.unionhit = false;
            this.overslip = false;
            this.Noslip = true;
            this.effecting = true;
            this.speed = speed;
            this.rebirth = this.union == Panel.COLOR.red;
            this.guard = CharacterBase.GUARD.guard;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                switch (this.process)
                {
                    case 0:
                        ++this.animationpoint.X;
                        if (this.animationpoint.X >= 3)
                        {
                            ++this.process;
                            this.sound.PlaySE(SoundEffect.rockopen);
                            break;
                        }
                        break;
                    case 1:
                        ++this.animationpoint.X;
                        if (this.animationpoint.X >= 5)
                        {
                            this.animationpoint.X = 3;
                            if (this.roop >= 0)
                            {
                                ++this.nowroop;
                                if (this.nowroop >= this.roop)
                                    ++this.process;
                            }
                            break;
                        }
                        break;
                    case 2:
                        --this.animationpoint.X;
                        if (this.animationpoint.X < 0)
                        {
                            this.animationpoint.X = 0;
                            this.flag = false;
                            break;
                        }
                        break;
                }
            }
            if (this.process == 1)
                this.AttackMake(this.hitPower, 0, 0);
            this.FlameControl(this.speed);
            base.Updata();
        }

        public override void Break()
        {
            if (this.breaked)
                return;
            this.process = 2;
            this.animationpoint.X = 3;
            this.breaked = true;
        }

        public override void Render(IRenderer dg)
        {
            if (!this.sp)
                this._rect = new Rectangle(2816 + this.animationpoint.X * 128, 0, 128, 96);
            else
                this._rect = new Rectangle(2816 + this.animationpoint.X * 128, 192, 128, 96);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "ran", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
