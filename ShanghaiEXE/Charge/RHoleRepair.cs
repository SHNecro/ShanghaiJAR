using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class RHoleRepair : ChargeBase
    {
        private const int start = 1;
        private const int speed = 2;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public RHoleRepair(IAudioEngine s, Player p)
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
                this.player.Rtimer = 600;
            }
            if (this.character.waittime != 5)
                return;
            this.sound.PlaySE(SoundEffect.eriasteal2);
            for (int pX = 0; pX < this.battle.panel.GetLength(0); ++pX)
            {
                for (int pY = 0; pY < this.battle.panel.GetLength(1); ++pY)
                {
                    if (this.battle.panel[pX, pY].State == Panel.PANEL._break && this.battle.panel[pX, pY].color == this.character.union)
                    {
                        this.battle.panel[pX, pY].state = Panel.PANEL._nomal;
                        this.battle.effects.Add(new Smoke(this.sound, this.battle, pX, pY, this.element));
                    }
                }
            }
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
