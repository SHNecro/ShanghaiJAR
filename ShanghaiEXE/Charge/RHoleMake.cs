using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class RHoleMake : ChargeBase
    {
        private const int start = 1;
        private const int speed = 2;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public RHoleMake(IAudioEngine s, Player p)
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
            if (this.character.waittime <= 1)
                this.character.animationpoint = new Point(0, 1);
            else if (this.character.waittime <= 7)
                this.character.animationpoint = new Point((this.character.waittime - 1) / 2, 1);
            else if (this.character.waittime <= 15)
            {
                this.character.animationpoint = new Point(3, 1);
            }
            else
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
                this.player.Rtimer = 30;
            }
            if (this.character.waittime != 5)
                return;
            this.battle.effects.Add(new Shock(this.sound, this.battle, this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y, 2, this.player.union));
            this.sound.PlaySE(SoundEffect.waveshort);
            if (!this.battle.panel[this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y].OnCharaCheck())
                this.battle.panel[this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y].State = Panel.PANEL._break;
            else
                this.battle.panel[this.player.position.X + this.UnionRebirth(this.player.union), this.player.position.Y].State = Panel.PANEL._crack;
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            this._rect = new Rectangle(this.character.animationpoint.X * this.character.Wide, 4 * this.character.Height, this.character.Wide, this.character.Height);
            this._position = new Vector2(this.character.positionDirect.X + Shake.X, this.character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, this.character.picturename, this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
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
