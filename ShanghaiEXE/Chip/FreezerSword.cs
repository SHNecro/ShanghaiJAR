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
    internal class FreezerSword : ChipBase
    {
        private const int end = 128;
        private const int speed = 2;
        private int sword;
        private Point animePoint;

        public FreezerSword(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 291;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.FreezerSwordName");
            this.element = ChipBase.ELEMENT.aqua;
            this.power = 450;
            this.subpower = 0;
            this._break = true;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.FreezerSwordDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        protected Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        4,
        4,
        4,
        4,
        4,
        4,
        42,
        4,
        100
            }, new int[10] { -1, 9, 8, 7, 3, 0, 7, 5, 4, 3 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (character.waittime < 102)
                this.animePoint = this.Animation(character.waittime);
            switch (character.waittime)
            {
                case 1:
                    character.animationpoint.X = -1;
                    this.sound.PlaySE(SoundEffect.shotwave);
                    Tower tower = new Tower(this.sound, battle, character.position.X, character.position.Y, character.union, 0, -1, ChipBase.ELEMENT.aqua);
                    tower.hitting = false;
                    battle.attacks.Add(tower);
                    break;
                case 28:
                    this.sound.PlaySE(SoundEffect.warp);
                    this.sword = 1;
                    break;
                case 66:
                    this.sword = 2;
                    break;
                case 70:
                    this.sword = 3;
                    break;
                case 74:
                    this.sword = 4;
                    this.sound.PlaySE(SoundEffect.bombmiddle);
                    this.ShakeStart(2, 20);
                    BombAttack bombAttack1 = new BombAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 1, this.element);
                    bombAttack1.StandPanel.Crack();
                    bombAttack1.StandPanel.Crack();
                    bombAttack1.breaking = true;
                    character.parent.effects.Add(new Bomber(this.sound, character.parent, bombAttack1.position.X, bombAttack1.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                    character.parent.attacks.Add(this.Paralyze(bombAttack1));
                    BombAttack bombAttack2 = new BombAttack(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 1, this.element);
                    bombAttack2.StandPanel.Crack();
                    bombAttack2.StandPanel.Crack();
                    bombAttack2.breaking = true;
                    character.parent.effects.Add(new Bomber(this.sound, character.parent, bombAttack2.position.X, bombAttack2.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                    character.parent.attacks.Add(this.Paralyze(bombAttack2));
                    BombAttack bombAttack3 = new BombAttack(this.sound, character.parent, character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 1, this.element);
                    bombAttack3.StandPanel.Crack();
                    bombAttack3.StandPanel.Crack();
                    bombAttack3.breaking = true;
                    character.parent.effects.Add(new Bomber(this.sound, character.parent, bombAttack3.position.X, bombAttack3.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                    character.parent.attacks.Add(this.Paralyze(bombAttack3));
                    break;
                case 128:
                    this.animePoint.X = -1;
                    this.sword = 0;
                    battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
                    break;
            }
            if (character.waittime > 152 && this.BlackOutEnd(character, battle))
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
                    this._rect = new Rectangle(56 * 4, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFreezerSwordCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFreezerSwordCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceFreezerSwordCombo1Line3")
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
                this._rect = new Rectangle(40 * this.animePoint.X, 0, 40, 56);
                this._position = new Vector2((float)(character.position.X * 40.0 + 20.0), (float)(character.position.Y * 24.0 + 58.0));
                dg.DrawImage(dg, "cirno", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
                if (this.sword <= 0)
                    return;
                this._rect = new Rectangle(136 * (this.sword - 1), 320, 136, 144);
                this._position = new Vector2(character.position.X * 40f + 68 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 58.0 - 40.0));
                dg.DrawImage(dg, "sword", this._rect, false, this._position, (uint)character.union > 0U, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}


