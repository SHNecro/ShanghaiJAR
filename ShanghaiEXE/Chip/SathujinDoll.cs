using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SathujinDoll : ChipBase
    {
        private const int interval = 15;
        private const int speed = 2;
        private Point animePoint;
        private Point[] targets;
        private int shot;
        private bool end;

        public SathujinDoll(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 285;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.SathujinDollName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 130;
            this.subpower = 0;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.SathujinDollDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        4,
        12,
        4,
        4,
        400
            }, new int[5] { 0, 9, 10, 11, 12 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (character.waittime == 0)
            {
                character.animationpoint.X = -1;
                this.sound.PlaySE(SoundEffect.warp);
            }
            if (character.waittime == this.shot * 15 + 20 && this.shot < 6)
            {
                this.sound.PlaySE(SoundEffect.chain);
                for (int index = 0; index < 3; ++index)
                {
                    int num1 = character.union == Panel.COLOR.red ? this.shot : 5 - this.shot;
                    int num2 = index;
                    var point = new Point(num1, num2);
                    if (character.NoObject(point) && character.InAreaCheck(point))
                    {
                        AttackBase a = new ThrowKnife(this.sound, battle, num1, num2, character.union, 10, this.Power(character), 30, 30, 3);
                        a.invincibility = false;
                        battle.attacks.Add(this.Paralyze(a));
                    }
                }
                ++this.shot;
            }
            if (this.shot >= 6 && character.waittime >= 170)
            {
                if (!this.end)
                {
                    this.animePoint.X = -1;
                    battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
                    this.end = true;
                }
                if (this.BlackOutEnd(character, battle))
                    base.Action(character, battle);
            }
            else
                this.animePoint = this.Animation(character.waittime);
        }

        public override void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (!printgraphics)
                return;
            switch (c % 2)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 6, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSathujinDollCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSathujinDollCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceSathujinDollCombo1Line3")
                    };
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
                        this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
                    }
                    return;
            }
        }

        public override void IconRender(
          IRenderer dg,
          Vector2 p,
          bool select,
          bool custom,
          int c,
          bool noicon)
        {
            if (!noicon)
            {
                int num = 0;
                if (select)
                    num = 1;
                this._rect = new Rectangle(624, 80 + num * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, 0, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                this._rect = new Rectangle(48 * this.animePoint.X, 0, 48, 64);
                this._position = new Vector2((float)(character.position.X * 40.0 + 18.0), (float)(character.position.Y * 24.0 + 58.0));
                dg.DrawImage(dg, "sakuya", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}


