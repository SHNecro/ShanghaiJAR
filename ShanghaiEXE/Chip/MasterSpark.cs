using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MasterSpark : ChipBase
    {
        private const int speed = 2;
        private Point animePoint;

        public MasterSpark(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 284;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.MasterSparkName");
            this.element = ChipBase.ELEMENT.eleki;
            this.power = 400;
            this.subpower = 0;
            this.regsize = 23;
            this.reality = 3;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.MasterSparkDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[21]
            {
        1,
        2,
        2,
        4,
        12,
        4,
        4,
        4,
        4,
        4,
        74,
        54,
        this.power / 2,
        24,
        4,
        4,
        4,
        4,
        2,
        2,
        4
            }, new int[22]
            {
        -1,
        3,
        2,
        1,
        0,
        4,
        5,
        6,
        7,
        10,
        11,
        12,
        13,
        12,
        5,
        6,
        4,
        0,
        1,
        2,
        3,
        -1
            }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            this.animePoint = this.Animation(character.waittime);
            switch (character.waittime)
            {
                case 1:
                    character.animationpoint.X = -1;
                    this.sound.PlaySE(SoundEffect.warp);
                    break;
                case 30:
                    this.sound.PlaySE(SoundEffect.charge);
                    break;
                case 150:
                    this.sound.PlaySE(SoundEffect.beam);
                    this.sound.PlaySE(SoundEffect.bombbig);
                    AttackBase a = new MasterBeam(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, false);
                    a.positionDirect.Y += 3f;
                    character.parent.attacks.Add(this.Paralyze(a));
                    break;
            }
            if (character.waittime > 230 + this.power / 2 && this.BlackOutEnd(character, battle))
                base.Action(character, battle);
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
                    this._rect = new Rectangle(56 * 5, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMasterSparkCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMasterSparkCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMasterSparkCombo1Line3")
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
                this._position = new Vector2((float)(character.position.X * 40.0 + 16.0 + 8.0) + Shake.X, (float)(character.position.Y * 24.0 + 58.0) + Shake.Y);
                dg.DrawImage(dg, "marisa", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
                if (character.waittime >= 30 && character.waittime < 80)
                {
                    this._rect = new Rectangle(character.waittime % 16 / 2 * 64, 0, 64, 64);
                    double num1 = character.position.X * 40.0 + 16.0 + 8.0;
                    Point shake = this.Shake;
                    double x = shake.X;
                    double num2 = num1 + x;
                    double num3 = character.position.Y * 24.0 + 58.0;
                    shake = this.Shake;
                    double y = shake.Y;
                    double num4 = num3 + y;
                    this._position = new Vector2((float)num2, (float)num4);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
                else
                {
                    if (character.waittime < 80 || character.waittime >= 100)
                        return;
                    this._rect = new Rectangle(character.waittime % 16 / 2 * 64, 64, 64, 64);
                    double num1 = character.position.X * 40.0 + 16.0 + 8.0;
                    Point shake = this.Shake;
                    double x = shake.X;
                    double num2 = num1 + x;
                    double num3 = character.position.Y * 24.0 + 58.0;
                    shake = this.Shake;
                    double y = shake.Y;
                    double num4 = num3 + y;
                    this._position = new Vector2((float)num2, (float)num4);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}


