using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeFighter : ChargeBase
    {
        private const int start = 5;
        private const int speed = 3;

        public ChargeFighter(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 170;
            this.power = 30;
            this.shorttime = 10;
        }

        public override void Action()
        {
            if (this.player.waittime == 11)
            {
                this.sound.PlaySE(SoundEffect.lance);
                PunchAttack punchAttack = new PunchAttack(this.sound, this.player.parent, this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y, this.player.union, this.Power, 2, this.player.Element)
                {
                    canCounter = false,
                    breaking = true
                };
                this.player.parent.attacks.Add(punchAttack);
            }
            if (this.player.waittime < 5)
                this.player.animationpoint = new Point(0, 3);
            else if (this.player.waittime < 17)
                this.player.animationpoint = new Point((this.player.waittime - 5) / 3, 3);
            else if (this.player.waittime < 37)
                this.player.animationpoint = new Point(3, 3);
            else if (this.player.waittime < 43)
            {
                this.player.animationpoint = new Point(4, 0);
            }
            else
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
            }
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            this._rect = new Rectangle(this.player.animationpoint.X * (this.player.Wide * 2) - 120, 6 * this.player.Height, this.player.Wide * 2, this.player.Height);
            this._position = new Vector2(this.player.positionDirect.X + this.player.Wide / 2 * this.UnionRebirth(this.player.union) + Shake.X, this.player.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, this.player.picturename, this._rect, false, this._position, this.player.union == Panel.COLOR.blue, Color.White);
        }
    }
}
