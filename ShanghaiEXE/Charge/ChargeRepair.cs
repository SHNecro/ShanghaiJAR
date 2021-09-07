using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeRepair : ChargeBase
    {
        private readonly bool aura = false;
        private const int start = 1;
        private const int speed = 2;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeRepair(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 250;
            this.power = 10;
            this.shorttime = 20;
            this.character = player;
            this.battle = this.player.parent;
            this.shadow = false;
        }

        public override void Action()
        {
            if (this.character.waittime <= 1)
                this.character.animationpoint = new Point(0, 1);
            else if (this.character.waittime <= 7)
                this.character.animationpoint = new Point((this.character.waittime - 1) / 2, 1);
            else if (this.character.waittime < 15)
                this.character.animationpoint = new Point(3, 1);
            else if (this.character.waittime == 15)
            {
                this.sound.PlaySE(SoundEffect.repair);
                this.battle.effects.Add(new Repair(this.sound, this.battle, new Vector2((int)this.character.positionDirect.X * this.UnionRebirth(this.character.union), (int)this.character.positionDirect.Y + 16), 2, this.character.position));
                this.character.Hp += this.Power;
            }
            else
            {
                if (this.character.waittime < 31)
                    return;
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
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
