using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeBlastCanon : ChargeBase
    {
        private const int shotend = 58;
        private const int shotstart = 40;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeBlastCanon(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 210;
            this.power = 30;
            this.shorttime = 10;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            if (this.character.waittime < 5)
                this.character.animationpoint = new Point(4, 0);
            else if (this.character.waittime < 40)
                this.character.animationpoint = new Point(5, 0);
            else if (this.character.waittime < 58)
            {
                this.character.animationpoint = new Point(6, 0);
                if (this.character.waittime < 22)
                    this.character.positionDirect.X -= (this.character.waittime - 15) * this.UnionRebirth(this.character.union);
            }
            else if (this.character.waittime < 63)
            {
                this.character.animationpoint = new Point(5, 0);
                this.character.PositionDirectSet();
            }
            else if (this.character.waittime == 63)
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
            }
            if (this.character.waittime == 43)
            {
                this.ShakeStart(10, 5);
                this.sound.PlaySE(SoundEffect.canon);
                this.battle.effects.Add(new BulletBigShells(this.sound, this.battle, this.character.position, this.character.positionDirect.X + 4 * this.character.UnionRebirth, this.character.positionDirect.Y + 16f, 26, this.character.union, 20 + this.Random.Next(20), 2, 0));
            }
            if (this.character.waittime >= 40 && this.character.waittime < 41)
                this.character.positionDirect.X -= 6 * this.UnionRebirth(this.character.union);
            if (this.character.waittime != 45)
                return;
            this.character.parent.attacks.Add(this.CounterNone(new BustorShot(this.sound, this.character.parent, this.character.position.X, this.character.position.Y, this.character.union, this.Power, BustorShot.SHOT.seedcanon, ChipBase.ELEMENT.normal, false, 0)));
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            if (this.character.waittime < 5)
                return;
            this._rect = new Rectangle(960, 720, this.character.Wide, this.character.Height);
            this._position = new Vector2(this.character.positionDirect.X + Shake.X, this.character.positionDirect.Y + Shake.Y);
            if (this.character.waittime < 10)
                this._rect.X = 360;
            else if (this.character.waittime < 11)
                this._rect.X = 480;
            else if (this.character.waittime < 12)
                this._rect.X = 600;
            else if (this.character.waittime < 13)
                this._rect.X = 720;
            else if (this.character.waittime < 14)
                this._rect.X = 840;
            else if (this.character.waittime < 15)
                this._rect.X = 960;
            else if (this.character.waittime > 40 && this.character.waittime < 42)
                this._rect.X = 1080;
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
            if (this.character.waittime >= 40)
            {
                this._position = this.character.positionDirect;
                this._position.X += 48 * this.UnionRebirth(this.character.union);
                this._position.Y += 14f;
                this._rect = new Rectangle((this.character.waittime - 40) / 3 * 64, 32, 64, 64);
                dg.DrawImage(dg, "shot", this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
            }
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
