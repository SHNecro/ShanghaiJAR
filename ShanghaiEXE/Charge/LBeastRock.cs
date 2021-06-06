using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class LBeastRock : ChargeBase
    {
        private const int start = 1;
        private const int speed = 2;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public LBeastRock(IAudioEngine s, Player p)
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
            this.character.rockonMode = !this.character.rockonMode;
            this.End();
            this.player.animationpoint = new Point(0, 0);
            this.player.motion = Player.PLAYERMOTION._neutral;
            this.player.Ltimer = 0;
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
