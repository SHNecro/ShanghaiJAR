using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeLance : ChargeBase
    {
        private readonly bool aura = false;
        private const int start = 1;
        private const int speed = 2;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeLance(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 150;
            this.power = 20;
            this.shorttime = 10;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            if (this.character.waittime == 5)
            {
                this.sound.PlaySE(SoundEffect.lance);
                this.character.parent.attacks.Add(this.CounterNone(new LanceAttack(this.sound, this.character.parent, this.character.position.X + this.UnionRebirth(this.character.union), this.character.position.Y, this.character.union, this.Power, 2, ChipBase.ELEMENT.normal, false)));
            }
            if (this.character.waittime < 1)
                this.character.animationpoint = new Point(0, 3);
            else if (this.character.waittime < 9)
                this.character.animationpoint = new Point((this.character.waittime - 1) / 2, 3);
            else if (this.character.waittime < 29)
                this.character.animationpoint = new Point(3, 3);
            else if (this.character.waittime < 35)
            {
                this.character.animationpoint = new Point(4, 0);
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
            int x = this.character.animationpoint.X * (this.character.Wide * 2);
            int height1 = this.character.Height;
            int width = this.character.Wide * 2;
            int height2 = this.character.Height;
            this._rect = new Rectangle(x, 0, width, height2);
            this._position = new Vector2(this.character.positionDirect.X + this.character.Wide / 2 * this.UnionRebirth(this.character.union) + Shake.X, this.character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "Lances", this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
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
