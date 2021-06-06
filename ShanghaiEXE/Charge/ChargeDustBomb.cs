using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeDustBomb : ChargeBase
    {
        private readonly bool aura = false;
        private const int start = 3;
        private const int speed = 8;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeDustBomb(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 180;
            this.power = 15;
            this.shorttime = 20;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            if (this.character.waittime == 3)
                this.sound.PlaySE(SoundEffect.throwbomb);
            this.character.animationpoint = CharacterAnimation.BombAnimation(this.character.waittime);
            if (this.character.waittime == 6)
                this.battle.attacks.Add(this.CounterNone(new ClossBomb(this.sound, this.character.parent, this.character.position.X, this.character.position.Y, this.character.union, this.Power, 1, new Vector2(this.character.positionDirect.X, this.character.positionDirect.Y - 16f), new Point(this.character.position.X + 3 * this.UnionRebirth(this.character.union), this.character.position.Y), 40, ClossBomb.TYPE.closs, false, ClossBomb.TYPE.closs, false, false)));
            if (this.character.waittime != 25)
                return;
            this.End();
            this.player.animationpoint = new Point(0, 0);
            this.player.motion = Player.PLAYERMOTION._neutral;
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            if (this.character.waittime >= 6)
                return;
            this._rect = new Rectangle(0, 0, 16, 16);
            Point point = new Point();
            if (this.character.waittime <= 3)
            {
                point.X = -22 * this.UnionRebirth(this.character.union);
                point.Y = 22;
            }
            else
            {
                point.X = -10 * this.UnionRebirth(this.character.union);
                point.Y = 4;
            }
            double x1 = character.positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2 + point.X;
            double y1 = character.positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2 + point.Y;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "bombs", this._rect, false, this._position, Color.White);
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
