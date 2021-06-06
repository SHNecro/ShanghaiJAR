using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class StandBurner : ObjectBase
    {
        private bool breaked;
        private StandBurner.MOTION motion;

        public StandBurner(IAudioEngine s, SceneBattle p, int pX, int pY, int power, Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 48;
            this.wide = 40;
            this.hp = 80;
            this.hitPower = power;
            this.hpmax = this.hp;
            this.unionhit = false;
            this.noslip = true;
            this.nohit = true;
            this.animationpoint.X = 12;
            this.motion = StandBurner.MOTION.up;
            this.guard = CharacterBase.GUARD.guard;
            this.positionre = this.position;
            this.PositionDirectSet();
            this.speed = 2;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                switch (this.motion)
                {
                    case StandBurner.MOTION.up:
                        --this.animationpoint.X;
                        switch (this.animationpoint.X)
                        {
                            case 0:
                                this.motion = StandBurner.MOTION.fire;
                                this.frame = 0;
                                this.speed = 4;
                                break;
                            case 6:
                                this.nohit = false;
                                break;
                        }
                        break;
                    case StandBurner.MOTION.fire:
                        switch (this.frame)
                        {
                            case 1:
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, new Point(2, 0), 60, true));
                                break;
                            case 12:
                                this.sound.PlaySE(SoundEffect.fire);
                                AttackBase attackBase1 = new ElementFire(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.hitPower, 18, ChipBase.ELEMENT.heat, false, 0);
                                attackBase1.positionDirect.X -= 48 * this.UnionRebirth;
                                attackBase1.positionDirect.Y += 2f;
                                this.parent.attacks.Add(attackBase1);
                                break;
                            case 15:
                                this.sound.PlaySE(SoundEffect.fire);
                                AttackBase attackBase2 = new ElementFire(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y, this.union, this.hitPower, 18, ChipBase.ELEMENT.heat, false, 0);
                                attackBase2.positionDirect.X -= 48 * this.UnionRebirth;
                                attackBase2.positionDirect.Y += 2f;
                                this.parent.attacks.Add(attackBase2);
                                break;
                            case 18:
                                this.sound.PlaySE(SoundEffect.fire);
                                AttackBase attackBase3 = new ElementFire(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y, this.union, this.hitPower, 18, ChipBase.ELEMENT.heat, false, 0);
                                attackBase3.positionDirect.X -= 48 * this.UnionRebirth;
                                attackBase3.positionDirect.Y += 2f;
                                this.parent.attacks.Add(attackBase3);
                                break;
                            case 100:
                                this.motion = StandBurner.MOTION.down;
                                this.frame = 0;
                                this.speed = 2;
                                break;
                        }
                        break;
                    case StandBurner.MOTION.down:
                        ++this.animationpoint.X;
                        switch (this.animationpoint.X)
                        {
                            case 6:
                                this.nohit = true;
                                break;
                            case 12:
                                this.MoveRandom(false, false);
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                this.motion = StandBurner.MOTION.up;
                                this.frame = 0;
                                break;
                        }
                        break;
                }
            }
            this.FlameControl(this.speed);
            base.Updata();
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 72);
        }

        public override void Break()
        {
            if (!this.breaked)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.clincher);
                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(16 + this.animationpoint.X * 48, 304, 48, 40);
            else
                this._rect = new Rectangle(16 + this.animationpoint.X * 48, 344, 48, 40);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "objects1", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private enum MOTION
        {
            up,
            fire,
            down,
        }
    }
}
