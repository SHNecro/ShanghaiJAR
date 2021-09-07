using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSChip
{
    internal class RanDS : ChipBase
    {
        protected int hit = 3;
        public int waittime;
        private const int speed = 2;
        protected int attackMode;
        protected int[] targetY;
        protected int atacks;
        private Charge chargeEffect;
        protected Point animePoint;
        private List<Point> targetMulti = new List<Point>();
        private int attackProcess;
        private int index = 0;
        private int stop = 0;

        public RanDS(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.libraryDisplayId = NSGame.ShanghaiEXE.Translate("DataList.IllegalChipDisplayId");
            this.number = 270;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.RanDSName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 50;
            this.subpower = 0;
            this.regsize = 99;
            this.reality = 5;
            this.hit = 5;
            this.targetY = new int[12];
            //this.targetMulti = new List<Point>();
            this._break = false;
            this.dark = true;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.R;
            this.code[1] = ChipFolder.CODE.R;
            this.code[2] = ChipFolder.CODE.R;
            this.code[3] = ChipFolder.CODE.R;
            var information = NSGame.ShanghaiEXE.Translate("Chip.RanDSDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();

        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[18]
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
        60,
        4,
        4,
        4,
        4,
        2,
        2,
        4
            }, new int[19]
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
        8,
        9,
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

        private Point AnimeSingleShot(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        8,
        3,
        3,
        100
            }, new int[4] { 0, 3, 4, 5 }, 0, waittime);
        }

        private Point AnimeMachinegunRay(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        3,
        3,
        3,
        3,
        3,
        100
            }, new int[4] { 5, 6, 7, 5 }, 0, waittime);
        }

        private Point AnimeSpread1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        2,
        2,
        2,
        1000
            }, new int[4] { 8, 9, 10, 11 }, 0, waittime);
        }

        private Point AnimeSpread2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        2,
        2,
        2,
        100
            }, new int[4] { 11, 12, 13, 11 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            switch (this.attackMode)
            {
                case 0:
                    if (this.waittime > 2)
                        this.animePoint = this.AnimeSingleShot(this.waittime);
                    switch (this.waittime)
                    {
                        case 1:
                            character.animationpoint.X = -1;
                            this.animePoint.X = 0;
                            this.sound.PlaySE(SoundEffect.warp);
                            break;
                        case 18:
                            this.animePoint.X = 5;
                            ++this.attackMode;
                            this.waittime = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (this.waittime)
                    {
                        case 1:
                            this.animePoint.X = 5;
                            break;
                        case 8:
                            this.chargeEffect = new Charge(this.sound, battle, character.position.X, character.position.Y);
                            battle.effects.Add(chargeEffect);
                            break;
                        case 40:
                            this.chargeEffect.chargeEffect = 2;
                            break;
                        case 72:
                            this.chargeEffect.flag = false;
                            for (int index = 0; index < this.targetY.Length; ++index)
                                this.targetY[index] = index >= this.hit * 3 ? this.Random.Next(3) : index % 3;
                            this.targetY = ((IEnumerable<int>)this.targetY).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
                            this.targetMulti = ((IEnumerable<Point>)character.RandomMultiPanel(Math.Min(6, 8 + 2), character.UnionEnemy, true)).ToList<Point>();
                            ++this.attackMode;
                            this.waittime = 0;
                            break;
                    }
                    break;
                case 2:
                    this.animePoint = this.AnimeSpread2(this.waittime);
                    
                    switch (this.waittime)
                    {
                        case 3:
                            /*
                            if (index == targetMulti.Count)
                            {
                                attackMode++;
                                stop = 1;
                                break;
                            }*/

                            for (int q = 0; q < targetMulti.Count; q++)
                            {
                                this.sound.PlaySE(SoundEffect.gun);
                                //battle.attacks.Add(this.Paralyze(new BustorShot(this.sound, battle, character.position.X + this.UnionRebirth(character.union), this.targetY[this.atacks], character.union, this.Power(character), BustorShot.SHOT.ranShot, this.element, false, 6)));
                                battle.effects.Add(new GunHit(this.sound, battle, this.targetMulti[q].X, this.targetMulti[q].Y, character.union));
                                battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, this.targetMulti[q].X, this.targetMulti[q].Y, character.union, base.Power(character), 0, this.element)));
                                //index++;

                            }
                            this.targetMulti = ((IEnumerable<Point>)character.RandomMultiPanel(Math.Min(6, 8 + 2), character.UnionEnemy, true)).ToList<Point>();
                            
                            /*
                            this.sound.PlaySE(SoundEffect.gun);
                            //battle.attacks.Add(this.Paralyze(new BustorShot(this.sound, battle, character.position.X + this.UnionRebirth(character.union), this.targetY[this.atacks], character.union, this.Power(character), BustorShot.SHOT.ranShot, this.element, false, 6)));
                            battle.effects.Add(new GunHit(this.sound, battle, this.targetMulti[index].X, this.targetMulti[index].Y, character.union));
                            battle.attacks.Add(new BombAttack(this.sound, battle, this.targetMulti[index].X, this.targetMulti[index].Y, character.union, base.Power(character), 0, this.element));
                            //index++;*/

                            //Debug.WriteLine(atacks);
                            break;
                        case 9:
                            ++this.atacks;
                            if (this.atacks >= this.targetY.Length)
                            {
                                ++this.attackMode;
                                this.animePoint.X = 2;
                            }
                            this.waittime = 0;
                            break;
                            //break;
                            /*++index;
                            if (index >= targetMulti.Count)
                            {
                                ++this.attackMode;
                                this.animePoint = this.AnimeSingleShot(this.waittime);
                                this.animePoint.X = 2;
                            }
                            this.waittime = 0;
                            break;*/
                    }
                    break;
                case 3:
                    switch (this.waittime)
                    {
                        case 30:
                            this.animePoint.X = -1;
                            character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
                            break;
                        case 40:
                            character.animationpoint.X = 0;
                            break;
                    }
                    if (this.waittime > 40 && this.BlackOutEnd(character, battle))
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
            if (printgraphics)
            {
                this._rect = new Rectangle(784, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic23", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, false, printstatus);
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
                this._rect = new Rectangle(16, 80 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, true);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                this._rect = new Rectangle(128 * this.animePoint.X, 0, 128, 96);
                this._position = new Vector2((float)(character.position.X * 40.0 + 8.0 + 24.0), (float)(character.position.Y * 24.0 + 44.0));
                dg.DrawImage(dg, "ranDS", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}

