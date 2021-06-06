using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class RShield : ChargeBase
    {
        private int anime = 0;
        private const int speed = 2;
        private bool open;
        private bool close;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public RShield(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 250;
            this.power = 10;
            this.shorttime = 20;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            if (this.character.waittime == 1)
            {
                this.sound.PlaySE(SoundEffect.rockopen);
                this.character.shield = CharacterBase.SHIELD.Normal;
                this.character.shieldUsed = false;
                this.character.ReflectP = 0;
                this.open = true;
                this.close = false;
                this.anime = 0;
            }
            if (this.open && !this.close && this.character.waittime > 90)
            {
                this.character.shield = CharacterBase.SHIELD.none;
                this.character.shieldUsed = false;
                this.character.ReflectP = 0;
                this.close = true;
                this.anime = 9;
            }
            if (this.close)
            {
                ++this.anime;
                if (this.anime < 13)
                    return;
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
                this.close = false;
                this.open = false;
                this.anime = 0;
                this.player.Rtimer = 60;
            }
            else if (this.character.waittime < 9)
            {
                this.anime = this.character.waittime;
            }
            else
            {
                if (this.character.waittime >= 30 || this.close)
                    return;
                this.anime = 3;
            }
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            if (this.anime >= 13)
                return;
            this._rect = new Rectangle(this.anime * 32, 360, 32, 72);
            double x1 = character.positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2 + 16 * this.UnionRebirth(this.character.union);
            double y1 = character.positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2 + 16.0;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "shield", this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
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
