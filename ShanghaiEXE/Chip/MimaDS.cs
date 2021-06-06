using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MimaDS : ChipBase
    {
        private const int start = 44;
        private const int speed = 2;
        protected int color;
        private Point animePoint;
        public int waittime = 0;

        public MimaDS(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.libraryDisplayId = NSGame.ShanghaiEXE.Translate("DataList.IllegalChipDisplayId");
            this.number = 269;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.MimaDSName");
            this.element = ChipBase.ELEMENT.poison;
            this.power = 0;
            this.subpower = 0;
            this.regsize = 99;
            this.reality = 5;
            this._break = true;
            this.shadow = false;
            this.dark = true;
            this.powerprint = false;
            this.color = 0;
            this.code[0] = ChipFolder.CODE.M;
            this.code[1] = ChipFolder.CODE.M;
            this.code[2] = ChipFolder.CODE.M;
            this.code[3] = ChipFolder.CODE.M;
            var information = NSGame.ShanghaiEXE.Translate("Chip.MimaDSDesc");
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
            }, new int[10] { -1, 0, 1, 2, 3, 0, 3, 7, 13, 14 }, 3, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;

            this.animePoint = this.AnimeReincarnation(this.waittime % 6);

            
            this.sound.PlaySE(SoundEffect.warp);
            //break;
            //this.animationpoint = this.AnimeReincarnation(this.waittime % 6);
            switch (this.waittime)
            {
                case 1:
                    this.animePoint.X = 0;
                    this.animePoint.Y = 0;
                    character.animationpoint.X = -1;

                    this.sound.PlaySE(SoundEffect.thunder);
                    //battle.effects.Add(new f);
                    
                    character.parent.effects.Add(new FlashFead(this.sound, battle, Color.White, 15));
                    break;
                case 15:
                    this.sound.PlaySE(SoundEffect.thunder);
                    battle.effects.Add(new FlashFead(this.sound, battle, Color.White, 15));
                    break;
                case 30:
                    this.sound.PlaySE(SoundEffect.bombbig);
                    this.sound.PlaySE(SoundEffect.damageplayer);
                    battle.effects.Add(new FlashFead(this.sound, battle, Color.White, 90));
                    this.ShakeStart(4, 60);
                    break;
                case 31:
                    foreach (CharacterBase characterBase in battle.AllChara())
                    {
                        if (characterBase.union == character.UnionEnemy) //&& (!(characterBase is DammyEnemy) || !characterBase.nohit) && character.InAreaCheck(character.position))
                            characterBase.HPhalf();
                            //pointList.Add(characterBase.position);
                    }

                    //this.parent.player.HPhalf();
                    break;
                case 90:
                    this.animePoint.X = 0;
                    //this.nohit = false;
                    break;
            }
            
            if (this.waittime < 30)
            {
                this.animePoint.X = 0;
                this.animePoint.Y = 0;

            }

            ++this.waittime;

            /*switch (character.waittime)
            {
                case 1:
                    this.animePoint.X = 0;
                    this.animePoint.Y = 0;
                    character.animationpoint.X = -1;
                    this.sound.PlaySE(SoundEffect.warp);
                    break;
                case 10:
                    //this.sound.PlaySE(SoundEffect.futon);
                    this.animePoint.X = 0;
                    this.animePoint.Y = 0;
                    break;
                case 15:
                    //this.animePoint.X = 1;
                    break;
                case 20:
                    //this.animePoint.X = 2;
                    break;
                case 44:
                    this.sound.PlaySE(SoundEffect.shoot);
                    character.parent.attacks.Add(this.Paralyze(new MimaCharge(this.sound, character.parent, 5, character.position.Y, character.union, this.Power(character), 1, character.positionDirect, this.element, -8)));
                    //AttackBase attackBase1 = this.Paralyze(new UthuhoChip(this.sound, character.parent, character.position.X, character.position.Y - 1, character.union, this.Power(character), this.color));
                    //attackBase1.breaking = false;

                    //new MimaCharge(this.sound, character.parent, character.position.X, 1, character.union, this.Power(character), 1, this.positionDirect, this.element, 8);

                    //character.parent.attacks.Add(attackBase1);
                    //AttackBase attackBase2 = this.Paralyze(new UthuhoChip(this.sound, character.parent, character.position.X, character.position.Y + 1, character.union, this.Power(character), this.color));
                    //attackBase2.breaking = false;
                    //character.parent.attacks.Add(attackBase2);
                    break;
            }*/
            if (character.waittime > 100 && this.BlackOutEnd(character, battle))
                base.Action(character, battle);
        }

        

        private Point AnimeReincarnation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[6] { 0, 1, 2, 3, 4, 5 }, 5, waittime);
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
                this._rect = new Rectangle(0, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic23", this._rect, true, p, Color.White);
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
                this._rect = new Rectangle(16, 80 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, true);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                this._rect = new Rectangle(80 * this.animePoint.X, 80*this.animePoint.Y, 80, 80);
                this._position = new Vector2((float)(character.position.X * 40.0 + 8.0 + 24.0), (float)(character.position.Y * 24.0 + 44.0));
                dg.DrawImage(dg, "mima", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }
        /*
        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1 && character.waittime < 44)
            {
                int num = 0;
                if (this.color == 1)
                    num = 2160;
                if (this.color == 2)
                    num = 1440;
                this._rect = new Rectangle(80 * this.animePoint.X, num + 480 * this.animePoint.Y, 80, 80);
                this._rect = new Rectangle(this.animePoint.X * 80, 480 * 0 + this.animePoint.Y * 80, 80, 80);

                this._position = new Vector2(character.position.X * 40f + 24 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 22.0));
                dg.DrawImage(dg, "Mima", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
            }
            else
                this.BlackOutRender(dg, character.union);
        }*/
    }
}

