using NSAttack;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeNomal : ChargeBase
    {
        public ChargeNomal(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 160;
            this.power = 10;
            this.shorttime = 15;
        }

        public override void Action()
        {
            if (this.player.waittime == 2)
                this.player.animationpoint = new Point(5, 6);
            else if (this.player.waittime == 4)
                this.sound.PlaySE(SoundEffect.buster);
            if (this.player.waittime == 6)
            {
                this.player.animationpoint = new Point(6, 6);
                this.player.parent.attacks.Add(new BustorShot(this.sound, this.player.parent, this.player.position.X, this.player.position.Y, this.player.union, !this.player.badstatus[1] ? player.busterPower * this.power : player.busterPower * this.power / 2, BustorShot.SHOT.normalcharge, ChipBase.ELEMENT.normal, false, 0));
            }
            if (this.player.waittime == 8)
                this.player.animationpoint = new Point(5, 6);
            if (this.player.waittime != 14)
                return;
            this.End();
            this.player.animationpoint = new Point(0, 0);
            this.player.motion = Player.PLAYERMOTION._neutral;
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            Point point = new Point(this.player.animationpoint.X, 4);
            this._position = new Vector2((int)this.player.positionDirect.X + this.Shake.X, (int)this.player.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(point.X * this.player.Wide, point.Y * this.player.Height, this.player.Wide, this.player.Height);
            dg.DrawImage(dg, picturename, this._rect, false, this._position, this.player.rebirth, Color.White);
            if (this.player.waittime < 2 || this.player.waittime >= 12)
                return;
            int num = this.player.busterBlue ? 96 : 0;
            if (this.player.bustor == Player.BUSTOR.normal)
                this._rect = new Rectangle((this.player.waittime - 2) / 3 * 32 + num, 0, 32, 16);
            else if (this.player.bustor == Player.BUSTOR.assault)
                this._rect = new Rectangle((this.player.waittime - 2) / 3 * 32 + num, 16, 32, 16);
            this._position = new Vector2((int)this.player.positionDirect.X + 44 * this.player.UnionRebirth + this.Shake.X, (int)this.player.positionDirect.Y + 15 + this.Shake.Y);
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.player.rebirth, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }
    }
}
