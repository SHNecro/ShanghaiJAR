using NSAttack;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeDoctor : ChargeBase
    {
        public ChargeDoctor(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 200;
            this.power = 5;
            this.shorttime = 10;
        }

        public override void Action()
        {
            this.player.animationpoint = this.MoveAnimation(this.player.waittime);
            if (this.player.waittime == 18)
            {
                this.player.animationpoint = new Point(6, 0);
                this.sound.PlaySE(SoundEffect.chain);
                AttackBase attackBase = new InjectBullet(this.sound, this.player.parent, this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y, this.player.union, this.Power, this.player.picturename, this.player.Element);
                attackBase.canCounter = false;
                this.player.parent.attacks.Add(attackBase);
            }
            if (this.player.waittime != 28)
                return;
            this.End();
            this.player.animationpoint = new Point(0, 0);
            this.player.motion = Player.PLAYERMOTION._neutral;
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
        }

        public Point MoveAnimation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        4,
        14,
        4,
        18
            }, new int[4] { 0, 1, 2, 3 }, 6, waittime);
        }
    }
}
