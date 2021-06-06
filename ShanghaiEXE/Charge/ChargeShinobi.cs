using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeShinobi : ChargeBase
    {
        private const int start = 3;
        private const int speed = 2;

        public ChargeShinobi(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 150;
            this.power = 20;
            this.shorttime = 20;
        }

        public override void Action()
        {
            if (this.player.waittime == 3)
                this.sound.PlaySE(SoundEffect.sword);
            this.player.animationpoint = CharacterAnimation.SworsAnimation(this.player.waittime);
            if (this.player.waittime >= 30)
            {
                this.End();
                this.player.motion = Player.PLAYERMOTION._neutral;
            }
            if (this.player.waittime != 10)
                return;
            int power = this.power;
            bool flag = false;
            if (this.player.style == Player.STYLE.shinobi)
                flag = true;
            AttackBase attackBase = new SwordAttack(this.sound, this.player.parent, this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y, this.player.union, !this.player.badstatus[1] ? player.busterPower * this.power : player.busterPower * this.power / 2, 3, this.player.Element, this.player.style == Player.STYLE.shinobi, false);
            attackBase.canCounter = false;
            this.player.parent.attacks.Add(attackBase);
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            if (this.player.waittime > 25)
                return;
            this._rect = new Rectangle(this.player.animationpoint.X * this.player.Wide, 5 * this.player.Height, this.player.Wide, this.player.Height);
            this._position = new Vector2(this.player.positionDirect.X + Shake.X, this.player.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, this.player.picturename, this._rect, false, this._position, this.player.union == Panel.COLOR.blue, Color.White);
        }
    }
}
