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
    internal class KishinCanon : ChipBase
    {
        private Point[,] target = new Point[2, 9];
        private const int aspeed = 3;
        private int waittime;
        private int spin;
        private int action;
        private const int start = 44;
        private const int speed = 2;
        private Point animePoint;

        public KishinCanon(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 286;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.KishinCanonName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 150;
            this.subpower = 0;
            this._break = true;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.KishinCanonDesc");
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
                    this.animePoint = this.AnimeCanonReady(this.waittime);
                    if (this.waittime >= 21)
                    {
                        ++this.action;
                        this.waittime = 0;
                        break;
                    }
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    this.animePoint = this.AnimeCanon(this.waittime);
                    if (this.waittime == 3)
                    {
                        this.sound.PlaySE(SoundEffect.canon);
                        battle.attacks.Add(this.Paralyze(new CanonBullet(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, new Vector2(character.positionDirect.X + 32 * this.UnionRebirth(character.union), character.positionDirect.Y + 8f), character.union, this.Power(character), this.element, true)));
                        battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X - 16 * this.UnionRebirth(character.union), character.positionDirect.Y + 16f, 32, character.union, 40 + this.Random.Next(20), 2, 0));
                        break;
                    }
                    if (this.waittime >= 24)
                    {
                        ++this.action;
                        this.waittime = 0;
                        break;
                    }
                    break;
                case 5:
                    if (this.waittime >= 24 && this.BlackOutEnd(character, battle))
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
                    this._rect = new Rectangle(56 * 7, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceKishinCanonCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceKishinCanonCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceKishinCanonCombo1Line3")
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
                this._rect = new Rectangle(88 * this.animePoint.X, 0, 88, 64);
                this._position = new Vector2((float)(character.position.X * 40.0 + 8.0 + (character.union == Panel.COLOR.red ? 24.0 : 0.0)), (float)(character.position.Y * 24.0 + 58.0));
                dg.DrawImage(dg, "tankman", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
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

        private Point AnimeCanon(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        3,
        3,
        3,
        3,
        3
            }, new int[6] { 24, 25, 26, 27, 28, 24 }, 0, waittime);
        }

        private Point AnimeGatlingReady(int waittime)
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
            }, new int[7] { 2, 3, 4, 5, 6, 7, 8 }, 0, waittime);
        }

        private Point AnimeGatling1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        3,
        3,
        3
            }, new int[3] { 8, 9, 10 }, 0, waittime);
        }

        private Point AnimeGatling2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        3,
        3,
        3,
        3
            }, new int[4] { 8, 11, 12, 8 }, 0, waittime);
        }
    }
}


