using NSAttack;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeGaia : ChargeBase
    {
        public ChargeGaia(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 200;
            this.power = 10;
            this.shorttime = 10;
        }

        public override void Action()
        {
            this.player.animationpoint = this.MoveAnimation(this.player.waittime);
            if (this.player.waittime == 1)
                this.sound.PlaySE(SoundEffect.throwbomb);
            if (this.player.waittime == 18)
            {
                this.sound.PlaySE(SoundEffect.canon);
                AttackBase attackBase = new FootPanel(this.sound, this.player.parent, this.player.position.X + 3 * this.UnionRebirth(this.player.union), this.player.position.Y, this.player.union, !this.player.badstatus[1] ? player.busterPower * this.power : player.busterPower * this.power / 2, 0, FootPanel.MOTION.init, this.player.Element, true);
                attackBase.canCounter = false;
                this.player.parent.attacks.Add(attackBase);
            }
            else
            {
                if (this.player.waittime != 39)
                    return;
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
            }
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
        }

        public Point MoveAnimation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        3,
        3,
        3,
        3,
        3,
        3,
        30
            }, new int[7] { 0, 1, 2, 3, 4, 5, 6 }, 6, waittime);
        }
    }
}
