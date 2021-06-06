using NSAttack;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeWing : ChargeBase
    {
        private const int start = 5;
        private const int speed = 3;

        public ChargeWing(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 200;
            this.power = 5;
            this.shorttime = 15;
        }

        public override void Action()
        {
            if (this.player.waittime == 5)
                this.sound.PlaySE(SoundEffect.sword);
            if (this.player.waittime <= 5)
                this.player.animationpoint = new Point(0, 1);
            else if (this.player.waittime <= 23)
                this.player.animationpoint = new Point((this.player.waittime - 5) / 3, 1);
            else if (this.player.waittime < 50)
            {
                this.player.animationpoint = new Point(6, 1);
            }
            else
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
            }
            if (this.player.waittime != 14)
                return;
            AttackBase attackBase = new Storm(this.sound, this.player.parent, this.player.position.X + 2 * this.UnionRebirth(this.player.union), this.player.position.Y, this.player.union, this.Power, 8, this.player.Element);
            attackBase.canCounter = false;
            this.player.parent.attacks.Add(attackBase);
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            this._rect = new Rectangle(this.player.animationpoint.X * this.player.Wide, this.player.Height, this.player.Wide, this.player.Height);
            this._position = new Vector2((int)this.player.positionDirect.X + this.Shake.X, (int)this.player.positionDirect.Y + this.Shake.Y);
            dg.DrawImage(dg, "slash", this._rect, false, this._position, Color.White);
        }
    }
}
