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
    internal class RainAnchor : ChipBase
    {
        private Point[,] target = new Point[2, 9];
        private const int aspeed = 3;
        private int waittime;
        private int spin;
        private int action;
        private const int start = 44;
        private const int speed = 2;
        private Point animePoint;

        public RainAnchor(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 294;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.RainAnchorName");
            this.element = ChipBase.ELEMENT.aqua;
            this.power = 120;
            this.subpower = 0;
            this._break = true;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.RainAnchorDesc");
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
        12,
        4,
        4,
        4,
        4
            }, new int[10] { -1, 9, 8, 7, 3, 0, 3, 7, 13, 14 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (character.waittime == 1)
            {
                character.animationpoint.X = -1;
                this.sound.PlaySE(SoundEffect.warp);
            }
            switch (this.action)
            {
                case 0:
                    this.animePoint.X = 0;
                    if (this.waittime >= 24)
                    {
                        ++this.action;
                        this.waittime = 0;
                        break;
                    }
                    break;
                case 1:
                case 2:
                case 3:
                    this.animePoint = this.AnimeAncerThrow(this.waittime);
                    if (this.waittime == 9)
                    {
                        this.sound.PlaySE(SoundEffect.throwbomb);
                        AnchorBomb.TYPE ty = AnchorBomb.TYPE.singleG;
                        if (this.action == 3)
                            ty = AnchorBomb.TYPE.single;
                        Point end = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 30, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y + 1);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 35, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y - 1);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 40, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 45, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y + 1);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 50, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y - 1);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 55, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 4 * this.UnionRebirth(character.union), character.position.Y);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 60, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 4 * this.UnionRebirth(character.union), character.position.Y + 1);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 65, ty, this.Random.Next(6))));
                        end = new Point(character.position.X + 4 * this.UnionRebirth(character.union), character.position.Y - 1);
                        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 70, ty, this.Random.Next(6))));
                        break;
                    }
                    if (this.waittime >= 72)
                    {
                        ++this.action;
                        this.waittime = 0;
                        break;
                    }
                    break;
                case 4:
                    if (this.waittime >= 120 && this.BlackOutEnd(character, battle))
                    {
                        base.Action(character, battle);
                        break;
                    }
                    break;
            }
            ++this.waittime;
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
                    this._rect = new Rectangle(56 * 7, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceRainAnchorCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceRainAnchorCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceRainAnchorCombo1Line3")
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
                this._rect = new Rectangle(72 * this.animePoint.X, 0, 72, 80);
                this._position = new Vector2((float)(character.position.X * 40.0 + 8.0 + (character.union == Panel.COLOR.red ? 24.0 : 0.0)), (float)(character.position.Y * 24.0 + 58.0));
                dg.DrawImage(dg, "mrasa", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }

        private Point AnimeCanonReady(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        3,
        3,
        3,
        3,
        3,
        3,
        3
            }, new int[7] { 2, 3, 20, 21, 22, 23, 24 }, 0, waittime);
        }

        private Point AnimeAncerThrow(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        3,
        3,
        3,
        100
            }, new int[4] { 3, 4, 5, 6 }, 0, waittime);
        }
    }
}


