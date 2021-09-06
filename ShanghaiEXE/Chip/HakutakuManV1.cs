using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class HakutakuManV1 : ChipBase
    {
        private List<Point> target = new List<Point>();
        private int targetNow = -1;
        private const int interval = 20;
        private const int speed = 2;
        protected Point posi;
        protected bool beast;
        protected bool end;
        private const int s = 5;
        protected Point animePoint;

        public HakutakuManV1(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 206;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.HakutakuManV1Name");
            this.element = ChipBase.ELEMENT.leaf;
            this.power = 70;
            this.subpower = 0;
            this.regsize = 38;
            this.reality = 3;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.H;
            this.code[1] = ChipFolder.CODE.K;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            var information = NSGame.ShanghaiEXE.Translate("Chip.HakutakuManV1Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Anime(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        1,
        1,
        1,
        4,
        1,
        1,
        1
            }, new int[7] { 4, 3, 2, 0, 2, 3, 4 }, 0, waittime);
        }

        private Point AnimeWide(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        1,
        1,
        1,
        2,
        1,
        2,
        1,
        1,
        1
            }, new int[10] { 4, 3, 2, 0, 5, 6, 7, 2, 3, 4 }, 0, waittime);
        }

        private Point AnimeLong(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        1,
        1,
        1,
        2,
        1,
        2,
        1,
        1,
        1
            }, new int[10] { 4, 3, 2, 0, 8, 9, 10, 2, 3, 4 }, 0, waittime);
        }

        private Point AnimeWideBO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[11]
            {
        1,
        1,
        1,
        1,
        2,
        1,
        1,
        2,
        1,
        1,
        1
            }, new int[11] { 4, 3, 2, 0, 5, 6, 7, 8, 2, 3, 4 }, 0, waittime);
        }

        private Point AnimeLongBO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[11]
            {
        1,
        1,
        1,
        1,
        2,
        1,
        1,
        2,
        1,
        1,
        1
            }, new int[11] { 4, 3, 2, 0, 9, 10, 11, 12, 2, 3, 4 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (this.end)
            {
                this.animePoint.X = -1;
                if (this.BlackOutEnd(character, battle))
                    base.Action(character, battle);
            }
            else if (this.moveflame)
            {
                if (this.targetNow < 0)
                {
                    this.animePoint = this.Anime(this.frame);
                    switch (this.frame)
                    {
                        case 1:
                            this.posi = character.position;
                            character.animationpoint.X = -1;
                            if (character.StandPanel.state == Panel.PANEL._grass)
                            {
                                this.beast = true;
                                this.power *= 2;
                                this.sound.PlaySE(SoundEffect.docking);
                            }
                            this.sound.PlaySE(SoundEffect.warp);
                            foreach (CharacterBase characterBase in battle.AllChara())
                            {
                                if (characterBase.union == character.UnionEnemy)
                                {
                                    Point position = characterBase.position;
                                    position.X -= this.UnionRebirth(character.union);
                                    if (character.InAreaCheck(position) && ((character.NoObject(position) || position == character.position) && !battle.panel[position.X, position.Y].Hole && (!(characterBase is DammyEnemy) || !characterBase.nohit) && character.InAreaCheck(character.position)))
                                        this.target.Add(position);
                                }
                            }
                            break;
                        case 10:
                            this.frame = 0;
                            ++this.targetNow;
                            if (this.targetNow >= this.target.Count)
                            {
                                this.end = true;
                                break;
                            }
                            this.posi = this.target[this.targetNow];
                            break;
                    }
                }
                else
                {
                    this.animePoint = this.targetNow % 2 != 1 ? (this.beast ? this.AnimeWideBO(this.frame) : this.AnimeWide(this.frame)) : (this.beast ? this.AnimeLongBO(this.frame) : this.AnimeLong(this.frame));
                    switch (this.frame)
                    {
                        case 9:
                            this.sound.PlaySE(SoundEffect.shotwave);
                            AttackBase a = this.beast ? new SwordAttack(this.sound, battle, this.posi.X + this.UnionRebirth(character.union), this.posi.Y, character.union, this.Power(character), 3, this.element, false, false) : (AttackBase)new KnifeAttack(this.sound, battle, this.posi.X + this.UnionRebirth(character.union), this.posi.Y, character.union, this.Power(character), 3, this.element, false);
                            battle.attacks.Add(this.Paralyze(a));
                            break;
                        case 12:
                            this.frame = 0;
                            ++this.targetNow;
                            if (this.targetNow >= this.target.Count)
                            {
                                this.end = true;
                                break;
                            }
                            this.posi = this.target[this.targetNow];
                            break;
                    }
                }
            }
            this.FlameControl(3);
        }

        public override void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (printgraphics)
            {
                this._rect = new Rectangle(672, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, printgraphics, printstatus);
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
                int num1 = this.number - 1;
                int num2 = num1 % 40;
                int num3 = num1 / 40;
                int num4 = 0;
                if (select)
                    num4 = 1;
                this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                if (this.end)
                    return;
                this._rect = new Rectangle(88 * this.animePoint.X, this.beast ? 264 : 0, 88, 88);
                this._position = new Vector2((float)(posi.X * 40.0 + 16.0), (float)(posi.Y * 24.0 + 50.0));
                dg.DrawImage(dg, "hakutaku", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }

        protected Point AnimeMove(int waitflame)
        {
            return CharacterAnimation.Return(new int[9]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        7,
        8,
        9
            }, new int[8] { 2, 1, 0, 1, 2, 2, 1, 0 }, 1, waitflame);
        }

        protected Point AnimeSlash1(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        5,
        6,
        7,
        8,
        9,
        10,
        100
            }, new int[7] { 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        protected Point AnimeSlash2(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        5,
        6,
        7,
        8,
        9,
        10,
        100
            }, new int[7] { 12, 13, 14, 15, 16, 17, 18 }, 0, waitflame);
        }

        private Point AnimeSlash3(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        5,
        6,
        7,
        8,
        9,
        10,
        100
            }, new int[7] { 19, 20, 21, 22, 23, 24, 25 }, 0, waitflame);
        }

        protected int TargetX(CharacterBase character, SceneBattle battle)
        {
            List<CharacterBase> characterBaseList = new List<CharacterBase>();
            foreach (CharacterBase characterBase in battle.AllHitter())
            {
                if (characterBase is EnemyBase)
                {
                    if (characterBase.union == character.UnionEnemy)
                        characterBaseList.Add(characterBase);
                }
                else if (characterBase is ObjectBase)
                {
                    ObjectBase objectBase = (ObjectBase)characterBase;
                    if ((objectBase.unionhit || objectBase.union == character.union) && character.UnionEnemy == objectBase.StandPanel.color)
                        characterBaseList.Add(characterBase);
                }
            }
            bool flag = false;
            int num = character.union == Panel.COLOR.red ? 6 : -1;
            foreach (CharacterBase characterBase in characterBaseList)
            {
                if (characterBase.position.Y == character.position.Y)
                {
                    flag = true;
                    if (character.union == Panel.COLOR.red)
                    {
                        if (num > characterBase.position.X)
                            num = characterBase.position.X;
                    }
                    else if (num < characterBase.position.X)
                        num = characterBase.position.X;
                }
            }
            if (flag)
                return num - this.UnionRebirth(character.union);
            foreach (CharacterBase characterBase in characterBaseList)
            {
                if (characterBase.position.Y != character.position.Y)
                {
                    if (character.union == Panel.COLOR.red)
                    {
                        if (num > characterBase.position.X)
                            num = characterBase.position.X;
                    }
                    else if (num < characterBase.position.X)
                        num = characterBase.position.X;
                }
            }
            return num - this.UnionRebirth(character.union);
        }
    }
}

