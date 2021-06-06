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
    internal class HyperSpanner : ChipBase
    {
        private const int speed = 2;
        protected Point animePoint;
        private const int end = 30;

        public HyperSpanner(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 287;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.HyperSpannerName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 300;
            this.subpower = 0;
            this._break = true;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.HyperSpannerDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[10] { 4, 3, 2, 0, 0, 5, 6, 7, 8, 9 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            int num = 3;
            if (this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
            {
                if (character.waittime / num <= 30)
                    this.animePoint = this.Animation(character.waittime / num);
                if (character.waittime % num == 0)
                {
                    switch (character.waittime / num)
                    {
                        case 1:
                            character.animationpoint.X = -1;
                            this.sound.PlaySE(SoundEffect.warp);
                            break;
                        case 6:
                            this.sound.PlaySE(SoundEffect.knife);
                            int pY = character.position.Y;
                            if (pY >= 2)
                                pY = 1;
                            AttackBase a = new NSAttack.HyperSpanner(this.sound, battle, character.position.X, pY, character.union, this.Power(character), 8, 0, this.element);
                            battle.attacks.Add(this.Paralyze(a));
                            break;
                        case 30:
                            this.animePoint.X = -1;
                            battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
                            break;
                    }
                }
            }
            if (character.waittime / num <= 30 || !this.BlackOutEnd(character, battle))
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
                    this._rect = new Rectangle(56 * 0, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceHyperSpannerCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceHyperSpannerCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceHyperSpannerCombo1Line3")
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
                this._rect = new Rectangle(112 * this.animePoint.X, 0, 112, 96);
                this._position = new Vector2((float)(character.position.X * 40.0 + 20.0), (float)(character.position.Y * 24.0 + 48.0));
                dg.DrawImage(dg, "spannerman", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
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
                flag = true;
                if (character.union == Panel.COLOR.red)
                {
                    if (num > characterBase.position.X)
                        num = characterBase.position.X;
                }
                else if (num < characterBase.position.X)
                    num = characterBase.position.X;
            }
            return num;
        }
    }
}


