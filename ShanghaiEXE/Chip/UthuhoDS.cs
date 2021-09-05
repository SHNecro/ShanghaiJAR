using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class UthuhoDS : ChipBase
	{
		private const int start = 44;
		private const int speed = 2;
		protected int color;
		private Point animePoint;
		private bool end = false;

		public UthuhoDS(IAudioEngine s)
		: base(s)
		{
			this.navi = true;
			this.libraryDisplayId = NSGame.ShanghaiEXE.Translate("DataList.IllegalChipDisplayId");
			this.number = 267;
			this.name = NSGame.ShanghaiEXE.Translate("Chip.UthuhoDSName");
			this.element = ChipBase.ELEMENT.heat;
			this.power = 450;
			this.subpower = 0;
			this.regsize = 99;
			this.reality = 5;
			this.color = 1;
			this._break = true;
			this.dark = true;
			this.shadow = false;
			this.powerprint = true;

			this.code[0] = ChipFolder.CODE.U;
			this.code[1] = ChipFolder.CODE.U;
			this.code[2] = ChipFolder.CODE.U;
			this.code[3] = ChipFolder.CODE.U;
			var information = NSGame.ShanghaiEXE.Translate("Chip.UthuhoDSDesc");
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

		private Point AnimeGigantFlear(int waittime)
		{
			return CharacterAnimation.ReturnKai(new int[16]
			{
				1,
				1,
				1,
				1,
				1,
				4,
				1,
				1,
				1,
				1,
				15,
				1,
				1,
				1,
				1,
				100
			}, new int[16]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				9,
				8,
				7,
				6,
				5
			}, 4, waittime);
		}

		public override void Action(CharacterBase character, SceneBattle battle)
		{
			if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
				return;
			if (character.waittime > 7)
				this.animePoint = this.AnimeGigantFlear(character.waittime / 4);
			/*if (character.waittime > 40)
            {
                this.animePoint.X = 0;
                this.animePoint.Y = 0;
            }*/


			switch (character.waittime)
			{
				case 1:
					this.animePoint.X = 0;
					this.animePoint.Y = 0;
					character.animationpoint.X = -1;
					this.sound.PlaySE(SoundEffect.warp);
					break;
				case 8:
					this.sound.PlaySE(SoundEffect.dark);
					this.sound.PlaySE(SoundEffect.charge);
					character.animationpoint.Y = 4;
					break;
				case 14:
					battle.attacks.Add(this.Paralyze(new GigantFlear(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, false)));
					break;
				case 120:
					//character.waittime = 101;
					this.end = true;
					break;

			}
			if (this.end && this.BlackOutEnd(character, battle))
				base.Action(character, battle);
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
				this._rect = new Rectangle(896, 0, 56, 48);
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
			if (character.animationpoint.X == -1 && character.waittime < 44)
			{
				int num = 0;
				if (this.color == 1)
					num = 2160;
				if (this.color == 2)
					num = 1440;
				this._rect = new Rectangle(120 * this.animePoint.X, num + 144 * this.animePoint.Y, 120, 144);
				this._position = new Vector2(character.position.X * 40f + 24 * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 22.0));
				dg.DrawImage(dg, "Uthuho", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
			}
			else
				this.BlackOutRender(dg, character.union);
		}
	}
}

