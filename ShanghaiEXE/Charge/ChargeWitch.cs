using NSAttack;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeWitch : ChargeBase
    {
        private int count = 1;
        private const int wait = 16;

        public ChargeWitch(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 180;
            this.power = 20;
            this.shorttime = 10;
        }

        public override void Action()
        {
            this.player.animationpoint = this.MoveAnimation(this.player.waittime);
            if (this.player.waittime == 1)
                this.count = 1;
            if (this.player.waittime == 16 || this.player.waittime == 25 || this.player.waittime == 34)
            {
                this.sound.PlaySE(SoundEffect.fire);
                AttackBase attackBase = new ElementFire(this.sound, this.player.parent, this.player.position.X + this.count * this.player.UnionRebirth, this.player.position.Y, this.player.union, this.Power, 6, this.player.Element, false, 0);
                attackBase.positionDirect.X -= 48 * this.player.UnionRebirth;
                attackBase.canCounter = false;
                this.player.parent.attacks.Add(attackBase);
                ++this.count;
            }
            if (this.player.waittime != 66)
                return;
            this.count = 1;
            this.End();
            this.player.animationpoint = new Point(0, 0);
            this.player.motion = Player.PLAYERMOTION._neutral;
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
        }

        public Point MoveAnimation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        6,
        2,
        2,
        2,
        2,
        2,
        100
            }, new int[7] { 0, 1, 2, 3, 4, 5, 6 }, 6, waittime);
        }
    }
}
