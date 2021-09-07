using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSNet;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class MiraiEigouZan : ChipBase
    {
        private List<YoumuShadow> shadows = new List<YoumuShadow>();
        private const int interval = 20;
        private const int speed = 2;
        private int[] motionList;
        private int nowmotion;
        private bool end;
        private bool nohit;
        private Point target;
        private int command;
        protected int xPosition;
        private const int s = 5;
        protected Point animePoint;

        public MiraiEigouZan(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 295;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.MiraiEigouZanName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 90;
            this.subpower = 0;
            this.swordtype = true;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.MiraiEigouZanDesc");
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
            if (this.moveflame && this.nowmotion == 0)
            {
                switch (this.frame)
                {
                    case 1:
                        character.animationpoint.X = -1;
                        this.xPosition = character.position.X;
                        this.sound.PlaySE(SoundEffect.warp);
                        this.animePoint = new Point(2, 0);
                        break;
                    case 2:
                        this.animePoint = new Point(1, 0);
                        break;
                    case 3:
                        this.animePoint = new Point(0, 0);
                        this.target = new Point(character.position.X, character.position.Y);
                        this.target.X = this.TargetX(character, battle) + this.UnionRebirth(character.union);
                        if (this.target.X < 0)
                            this.target.X = 0;
                        if (this.target.X > 5)
                        {
                            this.target.X = 5;
                            break;
                        }
                        break;
                    case 15:
                        if (!this.nohit)
                        {
                            this.sound.PlaySE(SoundEffect.pikin);
                            battle.effects.Add(new Flash(this.sound, battle, character.positionDirect, character.position));
                            this.animePoint = new Point(0, 4);
                            break;
                        }
                        this.animePoint = new Point(1, 0);
                        break;
                    case 18:
                        this.animePoint = this.nohit ? new Point(2, 0) : new Point(1, 4);
                        break;
                    case 19:
                        this.animePoint = new Point(-1, 4);
                        if (this.nohit)
                        {
                            this.frame = 79;
                            break;
                        }
                        break;
                    case 20:
                        Point posi1 = new Point(this.target.X + this.UnionRebirth(character.union), this.target.Y);
                        if (this.CanAttack(character, posi1))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi1.X, posi1.Y, character.union == Panel.COLOR.red ? 1 : 0);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                            break;
                        }
                        break;
                    case 22:
                        Point posi2 = new Point(this.target.X, this.target.Y + 1);
                        if (this.CanAttack(character, posi2))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi2.X, posi2.Y, 3);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                        }
                        posi2 = new Point(this.target.X, this.target.Y - 1);
                        if (this.CanAttack(character, posi2))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi2.X, posi2.Y, 2);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                            break;
                        }
                        break;
                    case 24:
                        Point posi3 = new Point(this.target.X - this.UnionRebirth(character.union), this.target.Y + 1);
                        if (this.CanAttack(character, posi3))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi3.X, posi3.Y, character.union == Panel.COLOR.red ? 0 : 1);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                        }
                        posi3 = new Point(this.target.X + this.UnionRebirth(character.union), this.target.Y - 1);
                        if (this.CanAttack(character, posi3))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi3.X, posi3.Y, character.union == Panel.COLOR.red ? 1 : 0);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                            break;
                        }
                        break;
                    case 26:
                        Point posi4 = new Point(this.target.X + this.UnionRebirth(character.union), this.target.Y + 1);
                        if (this.CanAttack(character, posi4))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi4.X, posi4.Y, character.union == Panel.COLOR.red ? 1 : 0);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                        }
                        posi4 = new Point(this.target.X - this.UnionRebirth(character.union), this.target.Y - 1);
                        if (this.CanAttack(character, posi4))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi4.X, posi4.Y, character.union == Panel.COLOR.red ? 0 : 1);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                            break;
                        }
                        break;
                    case 28:
                        Point posi5 = new Point(this.target.X - this.UnionRebirth(character.union), this.target.Y);
                        if (this.CanAttack(character, posi5))
                        {
                            YoumuShadow youmuShadow = new YoumuShadow(this.sound, battle, posi5.X, posi5.Y, character.union == Panel.COLOR.red ? 0 : 1);
                            this.shadows.Add(youmuShadow);
                            battle.effects.Add(youmuShadow);
                            break;
                        }
                        break;
                    case 40:
                        for (int index = 0; index < this.shadows.Count; ++index)
                            this.shadows[index].jittaika = true;
                        break;
                    case 44:
                        for (int index = 0; index < this.shadows.Count; ++index)
                        {
                            this.sound.PlaySE(SoundEffect.breakObject);
                            this.ShakeStart(4, 4);
                            BombAttack bombAttack = new BombAttack(this.sound, battle, this.target.X, this.target.Y, character.union, this.Power(character), 2, this.element);
                            bombAttack.breakinvi = true;
                            bombAttack.noElementWeak = true;
                            battle.attacks.Add(this.Paralyze(bombAttack));
                        }
                        if (this.shadows.Count > 0)
                        {
                            battle.effects.Add(new NormalChargehit(this.sound, battle, this.target.X, this.target.Y, 2));
                            break;
                        }
                        break;
                    case 80:
                        this.animePoint = new Point(-1, 0);
                        character.animationpoint.X = 0;
                        this.end = true;
                        break;
                }
            }
            if (this.end && this.BlackOutEnd(character, battle))
                base.Action(character, battle);
            this.FlameControl(2);
        }

        private bool CanAttack(CharacterBase character, Point posi)
        {
            return character.NoObject(posi) && character.InAreaCheck(posi);
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
                    this._rect = new Rectangle(56 * 0, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMiraiEigouZanCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMiraiEigouZanCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMiraiEigouZanCombo1Line3")
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
                this._rect = new Rectangle(this.animePoint.X * 104, this.animePoint.Y * 96, 104, 96);
                this._position = new Vector2((float)(xPosition * 40.0 + 28.0) + 4 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 48.0));
                dg.DrawImage(dg, "youmu", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }

        protected Point AnimeMove(int waitflame)
        {
            return CharacterAnimation.Return(new int[4]
            {
        1,
        1,
        1,
        100
            }, new int[4] { 0, 1, 2, 3 }, 5, waitflame);
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
                else if (characterBase is Player)
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
            this.nohit = true;
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
            return num;
        }
    }
}


