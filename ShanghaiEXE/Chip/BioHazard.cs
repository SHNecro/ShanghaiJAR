using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class BioHazard : ChipBase
    {
        private const int interval = 20;
        private const int speed = 2;
        private bool ballrend;
        private const int s = 5;
        private Point animePoint;

        public BioHazard(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 292;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.BioHazardName");
            this.element = ChipBase.ELEMENT.poison;
            this.power = 0;
            this.subpower = 10;
            this._break = false;
            this.shadow = false;
            this.powerprint = false;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.BioHazardDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        protected Point Animation(int waittime)
        {
            int[] interval = new int[6] { 4, 1, 1, 1, 1, 60 };
            int[] xpoint = new int[6] { 4, 5, 6, 7, 8, 8 };
            for (int index = 0; index < interval.Length; ++index)
                interval[index] *= 5;
            int y = 0;
            return CharacterAnimation.ReturnKai(interval, xpoint, y, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
            {
                if (character.waittime <= 100)
                    this.animePoint = this.Animation(character.waittime);
                switch (character.waittime)
                {
                    case 0:
                        battle.effects.Add(new Bomber(this.sound, battle, character.position.X, character.position.Y, Bomber.BOMBERTYPE.poison, 2));
                        character.animationpoint.X = -1;
                        this.sound.PlaySE(SoundEffect.bomb);
                        break;
                    case 30:
                        Point point = new Point(character.position.X + this.UnionRebirth(character.union), character.position.Y);
                        this.sound.PlaySE(SoundEffect.enterenemy);
                        battle.effects.Add(new MoveEnemy(this.sound, battle, point.X, point.Y));
                        if (character.InAreaCheck(point) && character.NoObject(point) && !battle.panel[point.X, point.Y].Hole)
                        {
                            ObjectBase objectBase = new BigSuzuran(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, 500, 0);
                            if (character is Player && ((Player)character).addonSkill[67])
                                objectBase.HPset(objectBase.Hp * 2);
                            battle.objects.Add(objectBase);
                            break;
                        }
                        break;
                    case 100:
                        this.animePoint.X = -1;
                        battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
                        break;
                }
            }
            if (character.waittime < 110 || !this.BlackOutEnd(character, battle))
                return;
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
                    this._rect = new Rectangle(56 * 5, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceBioHazardCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceBioHazardCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceBioHazardCombo1Line3")
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
                this._position = new Vector2((float)(character.position.X * 40.0 + 22.0) + 4 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 58.0));
                dg.DrawImage(dg, "medicine", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
                if (!this.ballrend)
                    return;
                this._position = new Vector2(character.positionDirect.X - 8f, character.positionDirect.Y - 48f);
                this._rect = new Rectangle(character.waittime % 7 * 40, 352, 40, 40);
                dg.DrawImage(dg, "towers", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}


