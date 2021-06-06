using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Kikuri : ChipBase
	{
		private int speed = 3;
		private Point animePoint;
		private Point target;

		public Kikuri(IAudioEngine s)
		  : base(s)
		{
			this.dark = true;
			this.libraryDisplayId = NSGame.ShanghaiEXE.Translate("DataList.IllegalChipDisplayId");
			this.navi = true;
			this.number = 430;
			this.name = NSGame.ShanghaiEXE.Translate("Chip.KikuriName");
			this.element = ChipBase.ELEMENT.normal;
			this.power = 300;
			this.subpower = 100;
			this.regsize = 99;
			this.reality = 5;
			this._break = false;
			this.shadow = false;
			this.powerprint = true;
			this.code[0] = ChipFolder.CODE.K;
			this.code[1] = ChipFolder.CODE.K;
			this.code[2] = ChipFolder.CODE.K;
			this.code[3] = ChipFolder.CODE.K;
			var information = NSGame.ShanghaiEXE.Translate("Chip.KikuriDesc");
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
				1,
				1,
				1,
				1,
				1,
				30,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				1000
			}, new int[18]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				11,
				12,
				13
			}, 0, waittime);
		}

		public override void Action(CharacterBase character, SceneBattle battle)
		{
			if (this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
			{
				this.animePoint = this.Animation(character.waittime);
				switch (character.waittime)
				{
					case 1:
						character.animationpoint.X = -1;
						this.sound.PlaySE(SoundEffect.dark);
						break;
					case 360:
						battle.attacks.Add(this.Paralyze(new SatelliteBarn(this.sound, battle, character.union == Panel.COLOR.red ? 3 : 2, 1, character.union, this.Power(character), 4, this.element)));
						break;
				}
				if (character.waittime >= 50 && character.waittime <= 320 && character.waittime % 10 == 0)
					this.MeteorRay(character, battle);
				if (character.waittime > 450 && this.BlackOutEnd(character, battle))
					base.Action(character, battle);
			}
			this.FlameControl(this.speed);
		}

		private void MeteorRay(CharacterBase character, SceneBattle battle)
		{
			this.sound.PlaySE(SoundEffect.beam);
			this.target = this.RandomPanel(false, false, false, character.UnionEnemy, battle, 2);
			battle.attacks.Add(new MeteorRay(this.sound, battle, this.target.X, this.target.Y, character.union, this.subpower, 4, this.element));
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
				this._rect = new Rectangle(840, 0, 56, 48);
				dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
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
			base.IconRender(dg, p, select, custom, c, noicon);
		}

		public override void Render(IRenderer dg, CharacterBase character)
		{
			if (character.animationpoint.X == -1)
			{
				this._rect = new Rectangle(88 * this.animePoint.X, 0, 88, 160);
				this._position = new Vector2(44f, 100f);
				dg.DrawImage(dg, "kikuri", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
			}
			else
				this.BlackOutRender(dg, character.union);
		}
	}
}

