using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSObject;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSChip
{
    internal class DrillBreaker : ChipBase
    {
        private int[] drillY = new int[3];
        private Point[] posis = new Point[3];
        private int manyDorills = 3;
        private const int speed = 2;
        private int scene;
        private Point animePoint;

        public DrillBreaker(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 293;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.DrillBreakerName");
            this.element = ChipBase.ELEMENT.eleki;
            this.power = 200;
            this.subpower = 0;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.DrillBreakerDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        1,
        8,
        16,
        4,
        4,
        52,
        4
            }, new int[7] { -1, 1, 8, 9, 10, 10, -1 }, 0, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            if (this.moveflame)
            {
                switch (this.scene)
                {
                    case 0:
                        switch (this.frame)
                        {
                            case 1:
                                for (int index = 0; index < this.drillY.Length; ++index)
                                    this.drillY[index] = index;
                                this.drillY = ((IEnumerable<int>)this.drillY).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
                                character.animationpoint.X = -1;
                                this.sound.PlaySE(SoundEffect.warp);
                                break;
                            case 20:
                                this.sound.PlaySE(SoundEffect.drill1);
                                battle.objects.Add(new MetalDorill(this.sound, battle, character.union == Panel.COLOR.red ? 0 : 5, this.drillY[0], this.Power(character), true, 5, character.union));
                                break;
                            case 35:
                                this.sound.PlaySE(SoundEffect.drill1);
                                battle.objects.Add(new MetalDorill(this.sound, battle, character.union == Panel.COLOR.red ? 0 : 5, this.drillY[1], this.Power(character), true, 5, character.union));
                                break;
                            case 50:
                                this.sound.PlaySE(SoundEffect.drill1);
                                battle.objects.Add(new MetalDorill(this.sound, battle, character.union == Panel.COLOR.red ? 0 : 5, this.drillY[2], this.Power(character), true, 5, character.union));
                                break;
                            case 100:
                                this.frame = 0;
                                ++this.scene;
                                break;
                        }
                        break;
                    case 1:
                        switch (this.frame)
                        {
                            case 1:
                                this.posis = this.GetRandamPanel(this.manyDorills, character.UnionEnemy, true, character, true);
                                this.posis[0] = this.RandomTarget(character, battle);
                                for (int index = 0; index < this.manyDorills; ++index)
                                    battle.attacks.Add(new Dummy(this.sound, battle, this.posis[index].X, this.posis[index].Y, character.union, new Point(), 30, true));
                                break;
                            case 30:
                                this.sound.PlaySE(SoundEffect.drill1);
                                this.sound.PlaySE(SoundEffect.breakObject);
                                this.ShakeStart(8, 4);
                                for (int index = 0; index < this.manyDorills; ++index)
                                {
                                    battle.effects.Add(new UPDrill(this.sound, battle, this.posis[index].X, this.posis[index].Y));
                                    AttackBase a = new BombAttack(this.sound, battle, this.posis[index].X, this.posis[index].Y, character.union, this.Power(character), 1, this.element);
                                    a.breaking = true;
                                    battle.attacks.Add(this.Paralyze(a));
                                    a.StandPanel.Crack();
                                    a.StandPanel.Crack();
                                }
                                break;
                            case 60:
                                ++this.scene;
                                this.frame = 0;
                                break;
                        }
                        break;
                    case 2:
                        this.animePoint = this.Animation(this.frame);
                        switch (this.frame)
                        {
                            case 1:
                                this.posis = this.GetRandamPanel(this.manyDorills, character.UnionEnemy, true, character, true);
                                for (int index = 0; index < this.manyDorills; ++index)
                                    battle.attacks.Add(new Dummy(this.sound, battle, this.posis[index].X, this.posis[index].Y, character.union, new Point(), 30, true));
                                break;
                            case 32:
                                for (int index = 0; index < this.manyDorills; ++index)
                                {
                                    AttackBase a = new CrackThunder(this.sound, character.parent, this.posis[index].X, this.posis[index].Y, character.union, this.Power(character), false);
                                    character.parent.attacks.Add(this.Paralyze(a));
                                }
                                break;
                            case 64:
                                character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
                                break;
                        }
                        if (this.frame > 64 && this.BlackOutEnd(character, battle))
                        {
                            base.Action(character, battle);
                            break;
                        }
                        break;
                }
            }
            this.FlameControl(1);
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
                    this._rect = new Rectangle(56 * 6, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDrillBreakerCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDrillBreakerCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDrillBreakerCombo1Line3")
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
                this._rect = new Rectangle(80 * this.animePoint.X, 0, 80, 72);
                this._position = new Vector2((float)(character.position.X * 40.0 + 20.0), (float)(character.position.Y * 24.0 + 54.0));
                dg.DrawImage(dg, "iku", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
    }
}


