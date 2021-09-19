using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class YorihimeV2 : ChipBase
    {
        private const int interval = 20;
        private const int speed = 2;
        private int[] motionList;
        private int nowmotion;
        private bool end;
        private int xPosition;
        private int command;
        private const int s = 5;
        private Point animePoint;

        public YorihimeV2(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 216;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.YorihimeV2Name");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 60;
            this.subpower = 0;
            this.regsize = 47;
            this.reality = 4;
            this.swordtype = true;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.Y;
            this.code[1] = ChipFolder.CODE.W;
            this.code[2] = ChipFolder.CODE.Y;
            this.code[3] = ChipFolder.CODE.W;
            var information = NSGame.ShanghaiEXE.Translate("Chip.YorihimeV2Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
            this.motionList = new int[4] { 0, 1, 2, 3 };
        }

        protected Point Animation(int waittime)
        {
            int[] interval = new int[6] { 4, 1, 1, 8, 1, 60 };
            int[] xpoint = new int[6] { 0, 8, 12, 13, 16, 17 };
            for (int index = 0; index < interval.Length; ++index)
                interval[index] *= 5;
            int y = 0;
            return CharacterAnimation.ReturnKai(interval, xpoint, y, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (character is Player)
            {
                Player player = (Player)character;
                if (this.nowmotion == 0 && this.frame < 3 && (Input.IsPush(Button._A) && this.command == 0) && this.commandTime < 60)
                {
                    this.CommandInput("上下左右B", player);
                    if (this.CommandCheck("下左上右下"))
                        this.command = 4;
                    else if (this.CommandCheck("左B右B"))
                        this.command = 5;
                }
            }
            if (this.moveflame)
            {
                switch (this.nowmotion)
                {
                    case 0:
                        this.animePoint.X = this.AnimeMove(this.frame).X;
                        switch (this.frame)
                        {
                            case 1:
                                character.animationpoint.X = -1;
                                this.xPosition = character.position.X;
                                this.sound.PlaySE(SoundEffect.warp);
                                break;
                            case 2:
                                if (character is Player && (Input.IsPush(Button._A) && this.command == 0 && this.commandTime < 60))
                                {
                                    this.frame = 1;
                                    break;
                                }
                                break;
                            case 3:
                                if (this.CommandCheck("下左上右下"))
                                {
                                    this.sound.PlaySE(SoundEffect.CommandSuccess);
                                    this.command = 4;
                                }
                                else if (this.CommandCheck("左B右B"))
                                {
                                    this.sound.PlaySE(SoundEffect.CommandSuccess);
                                    this.command = 5;
                                }
                                if (this.command == 5)
                                {
                                    this.xPosition = character.position.X;
                                    ++this.nowmotion;
                                    this.frame = 0;
                                    break;
                                }
                                break;
                            case 5:
                                this.xPosition = this.TargetX(character, battle);
                                if (this.xPosition < 0)
                                    this.xPosition = 0;
                                if (this.xPosition > 5)
                                {
                                    this.xPosition = 5;
                                    break;
                                }
                                break;
                            case 9:
                                ++this.nowmotion;
                                this.frame = 0;
                                break;
                        }
                        break;
                    case 1:
                        this.animePoint.X = this.AnimeSlash1(this.frame).X;
                        switch (this.frame)
                        {
                            case 5:
                                if (character is Player && this.command == 0)
                                {
                                    Player player = (Player)character;
                                    if (Input.IsPush(Button.Right))
                                    {
                                        this.sound.PlaySE(SoundEffect.CommandSuccess);
                                        this.command = 1;
                                    }
                                    else if (Input.IsPush(Button.Up))
                                    {
                                        this.sound.PlaySE(SoundEffect.CommandSuccess);
                                        this.command = 2;
                                    }
                                    else if (Input.IsPush(Button.Left))
                                    {
                                        this.sound.PlaySE(SoundEffect.CommandSuccess);
                                        this.command = 3;
                                    }
                                    break;
                                }
                                break;
                            case 6:
                                this.sound.PlaySE(SoundEffect.sword);
                                AttackBase a1 = new FighterSword(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element);
                                switch (this.command)
                                {
                                    case 1:
                                        a1 = new FighterSword(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element);
                                        break;
                                    case 2:
                                        a1 = new SwordAttack(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false, false);
                                        break;
                                    case 3:
                                        a1 = new SwordCloss(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character) / 2, 4, this.element, false);
                                        break;
                                    case 4:
                                        a1 = new Halberd(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false);
                                        break;
                                    case 5:
                                        a1 = new SonicBoom(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 8, this.element, false);
                                        break;
                                }
                                a1.invincibility = false;
                                battle.attacks.Add(this.Paralyze(a1));
                                break;
                            case 10:
                                ++this.nowmotion;
                                this.frame = 0;
                                break;
                        }
                        break;
                    case 2:
                        this.animePoint.X = this.AnimeSlash2(this.frame).X;
                        switch (this.frame)
                        {
                            case 6:
                                this.sound.PlaySE(SoundEffect.sword);
                                AttackBase a2 = new SwordAttack(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false, false);
                                switch (this.command)
                                {
                                    case 1:
                                        a2 = new FighterSword(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element);
                                        break;
                                    case 2:
                                        a2 = new SwordAttack(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false, false);
                                        break;
                                    case 3:
                                        a2 = new SwordCloss(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character) / 2, 4, this.element, false);
                                        break;
                                    case 4:
                                        a2 = new Halberd(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false);
                                        break;
                                    case 5:
                                        a2 = new SonicBoom(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 8, this.element, false);
                                        break;
                                }
                                a2.invincibility = false;
                                battle.attacks.Add(this.Paralyze(a2));
                                break;
                            case 10:
                                ++this.nowmotion;
                                this.frame = 0;
                                break;
                        }
                        break;
                    case 3:
                        this.animePoint.X = this.AnimeSlash3(this.frame).X;
                        switch (this.frame)
                        {
                            case 6:
                                this.sound.PlaySE(SoundEffect.sword);
                                AttackBase a3 = new SwordCloss(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character) / 2, 4, this.element, false);
                                switch (this.command)
                                {
                                    case 1:
                                        a3 = new FighterSword(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element);
                                        break;
                                    case 2:
                                        a3 = new SwordAttack(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false, false);
                                        break;
                                    case 3:
                                        a3 = new SwordCloss(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character) / 2, 4, this.element, false);
                                        break;
                                    case 4:
                                        a3 = new Halberd(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false);
                                        break;
                                    case 5:
                                        a3 = new SonicBoom(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 8, this.element, false);
                                        break;
                                }
                                a3.invincibility = false;
                                battle.attacks.Add(this.Paralyze(a3));
                                break;
                            case 10:
                                ++this.nowmotion;
                                this.frame = 0;
                                break;
                        }
                        break;
                    case 4:
                        this.animePoint.X = this.AnimeSlash2(this.frame).X;
                        switch (this.frame)
                        {
                            case 6:
                                this.sound.PlaySE(SoundEffect.sword);
                                AttackBase a4 = new Halberd(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false);
                                switch (this.command)
                                {
                                    case 1:
                                        a4 = new FighterSword(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element);
                                        break;
                                    case 2:
                                        a4 = new SwordAttack(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false, false);
                                        break;
                                    case 3:
                                        a4 = new SwordCloss(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character) / 2, 4, this.element, false);
                                        break;
                                    case 4:
                                        a4 = new Halberd(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 4, this.element, false);
                                        break;
                                    case 5:
                                        a4 = new SonicBoom(this.sound, battle, this.xPosition + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 8, this.element, false);
                                        break;
                                }
                                a4.invincibility = true;
                                battle.attacks.Add(this.Paralyze(a4));
                                break;
                            case 30:
                                character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, this.xPosition, character.position.Y));
                                this.end = true;
                                ++this.nowmotion;
                                this.frame = 0;
                                break;
                        }
                        break;
                }
            }
            if (this.end && this.BlackOutEnd(character, battle))
                base.Action(character, battle);
            this.FlameControl(2);
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
                this._rect = new Rectangle(224, 0, 56, 48);
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
                this._rect = new Rectangle(96 * this.animePoint.X, 0, 96, 96);
                this._position = new Vector2((float)(xPosition * 40.0 + 48.0) + 4 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 44.0));
                dg.DrawImage(dg, "yorihime", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
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

