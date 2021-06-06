using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class DisasterCrow : ChipBase
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

        public DisasterCrow(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 288;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.DisasterCrowName");
            this.element = ChipBase.ELEMENT.leaf;
            this.power = 280;
            this.subpower = 0;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.DisasterCrowDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Anime(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        1,
        1,
        1,
        400
            }, new int[4] { 4, 3, 2, 0 }, 0, waittime);
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
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        2,
        1,
        1,
        20
            }, new int[5] { 0, 5, 6, 7, 8 }, 0, waittime);
        }

        private Point AnimeLongBO(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        2,
        1,
        1,
        20
            }, new int[5] { 0, 9, 10, 11, 12 }, 0, waittime);
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
            else if (this.targetNow < 0)
            {
                if (this.moveflame)
                {
                    this.animePoint = this.Anime(this.frame);
                    switch (this.frame)
                    {
                        case 1:
                            this.beast = true;
                            this.posi = character.position;
                            character.animationpoint.X = -1;
                            break;
                        case 10:
                            battle.effects.Add(new Charge(this.sound, battle, character.position.X, character.position.Y + 1));
                            break;
                        case 26:
                            this.targetNow = 1;
                            this.frame = 0;
                            break;
                    }
                }
            }
            else
            {
                switch (this.targetNow)
                {
                    case 1:
                        this.animePoint = this.AnimeWideBO(this.frame);
                        if (this.moveflame)
                        {
                            switch (this.frame)
                            {
                                case 2:
                                    this.ShakeStart(8, 8);
                                    this.sound.PlaySE(SoundEffect.shotwave);
                                    this.sound.PlaySE(SoundEffect.breakObject);
                                    AttackBase a = new SwordAttack(this.sound, battle, this.posi.X + 2 * this.UnionRebirth(character.union), this.posi.Y, character.union, this.Power(character), 3, this.element, false, false);
                                    battle.attacks.Add(this.Paralyze(a));
                                    break;
                                case 6:
                                    this.frame = 0;
                                    ++this.targetNow;
                                    break;
                            }
                            break;
                        }
                        break;
                    case 2:
                        this.animePoint = this.AnimeLongBO(this.frame);
                        if (this.moveflame)
                        {
                            switch (this.frame)
                            {
                                case 2:
                                    this.ShakeStart(8, 8);
                                    this.sound.PlaySE(SoundEffect.shotwave);
                                    this.sound.PlaySE(SoundEffect.breakObject);
                                    AttackBase a = new FighterSword(this.sound, battle, this.posi.X + this.UnionRebirth(character.union), this.posi.Y, character.union, this.Power(character), 3, this.element);
                                    battle.attacks.Add(this.Paralyze(a));
                                    break;
                                case 12:
                                    this.frame = 0;
                                    ++this.targetNow;
                                    break;
                            }
                            break;
                        }
                        break;
                    case 3:
                        this.end = true;
                        break;
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
            if (!printgraphics)
                return;
            switch (c % 2)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 1, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDisasterCrowCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDisasterCrowCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDisasterCrowCombo1Line3")
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
        1,
        2,
        3,
        4,
        5,
        6,
        100
            }, new int[7] { 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        protected Point AnimeSlash2(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        1,
        2,
        3,
        4,
        5,
        6,
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


