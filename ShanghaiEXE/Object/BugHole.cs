using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class BugHole : ObjectBase
    {
        private readonly bool movestart = false;
        private const int plusy = 70;
        private bool breaked;
        private readonly bool shot;

        public BugHole(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 48;
            this.wide = 32;
            this.hp = 1;
            this.hitPower = 50;
            this.hpmax = this.hp;
            this.guard = CharacterBase.GUARD.guard;
            this.unionhit = false;
            this.overslip = false;
            this.effecting = true;
            this.noslip = true;
            this.guard = CharacterBase.GUARD.guard;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                if (this.frame % 6 == 0)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 3)
                        this.animationpoint.X = 0;
                }
                if (!this.parent.blackOut)
                {
                    int num1 = 0;
                    int num2;
                    if (this.union == Panel.COLOR.blue)
                    {
                        num2 = 70 + this.parent.player.position.Y * 24;
                    }
                    else
                    {
                        int num3 = 99;
                        foreach (EnemyBase enemy in this.parent.enemys)
                        {
                            if (num3 > enemy.position.X && enemy.union == Panel.COLOR.blue)
                            {
                                num3 = enemy.position.X;
                                num1 = enemy.position.Y;
                            }
                        }
                        num2 = 70 + num1 * 24;
                    }
                    if (positionDirect.Y > (double)num2)
                        this.positionDirect.Y -= 0.5f;
                    else if (positionDirect.Y < (double)num2)
                        this.positionDirect.Y += 0.5f;
                    this.position = this.Calcposition(this.positionDirect, 36, false);
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
            }
            this.FlameControl();
            base.Updata();
        }

        public override void Break()
        {
            if (!this.breaked || this.StandPanel.Hole)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.dark);
                this.parent.effects.Add(new BugHoleDead(this.sound, this.parent, this.positionDirect, this.position));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(32 * this.animationpoint.X, 1384, 32, 40);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
