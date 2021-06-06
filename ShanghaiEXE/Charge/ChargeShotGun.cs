using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeShotGun : ChargeBase
    {
        private const int shotend = 10;
        private const int shotinterval = 4;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeShotGun(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 200;
            this.power = 5;
            this.shorttime = 10;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            if (this.character.waittime < 1)
                this.character.animationpoint = new Point(4, 0);
            else if (this.character.waittime < 3)
                this.character.animationpoint = new Point(5, 0);
            else if (this.character.waittime < 10)
                this.character.animationpoint = new Point(6, 0);
            else if (this.character.waittime == 5)
                this.character.animationpoint = new Point(5, 0);
            else if (this.character.waittime == 15)
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
            }
            if (this.character.waittime != 6)
                return;
            this.battle.effects.Add(new BulletBigShells(this.sound, this.battle, this.character.position, this.character.positionDirect.X + 4 * this.character.UnionRebirth, this.character.positionDirect.Y, 26, this.character.union, 20 + this.Random.Next(20), 2, 0));
            this.character.parent.attacks.Add(this.CounterNone(new ShotGun(this.sound, this.character.parent, this.character.position.X + this.UnionRebirth(this.character.union), this.character.position.Y, this.character.union, this.Power, this.element)));
            this.character.parent.attacks.Add(this.CounterNone(new ShotGun(this.sound, this.character.parent, this.character.position.X + 2 * this.UnionRebirth(this.character.union), this.character.position.Y - 1, this.character.union, this.Power, this.element)));
            this.character.parent.attacks.Add(this.CounterNone(new ShotGun(this.sound, this.character.parent, this.character.position.X + 2 * this.UnionRebirth(this.character.union), this.character.position.Y, this.character.union, this.Power, this.element)));
            this.character.parent.attacks.Add(this.CounterNone(new ShotGun(this.sound, this.character.parent, this.character.position.X + 2 * this.UnionRebirth(this.character.union), this.character.position.Y + 1, this.character.union, this.Power, this.element)));
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            if (this.character.waittime < 5)
                return;
            this._rect = new Rectangle(this.character.Wide, 6 * this.character.Height, this.character.Wide, this.character.Height);
            this._position = new Vector2(this.character.positionDirect.X + Shake.X, this.character.positionDirect.Y + Shake.Y);
            if (this.character.waittime < 6)
                this._rect.X = 0;
            else if (this.character.waittime >= 8 && this.character.waittime < 10)
                this._position.X -= 2 * this.UnionRebirth(this.character.union);
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
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

        public static Point Animation(int waittime)
        {
            int[] interval = new int[12];
            for (int index = 0; index < 12; ++index)
                interval[index] = 4 * index;
            int[] xpoint = new int[14]
            {
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6
            };
            int y = 0;
            return CharacterAnimation.Return(interval, xpoint, y, waittime);
        }
    }
}
