﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using SlimDX;
using System.Drawing;

namespace NSChip
{
	internal class BigTyphoon : ChipBase
	{
		private const int start = 5;
		private const int speed = 3;

		public BigTyphoon(MyAudio s)
		  : base(s)
		{
			this.number = 275;
			this.name = NSGame.ShanghaiEXE.Translate("Chip.BigTyphoonName");
			this.element = ChipBase.ELEMENT.aqua;
			this.power = 30;
			this.subpower = 0;
			this._break = false;
			this.powerprint = true;
			this.code[0] = ChipFolder.CODE.none;
			this.code[1] = ChipFolder.CODE.none;
			this.code[2] = ChipFolder.CODE.none;
			this.code[3] = ChipFolder.CODE.none;
			var information = NSGame.ShanghaiEXE.Translate("Chip.BigTyphoonDesc");
			this.information[0] = information[0];
			this.information[1] = information[1];
			this.information[2] = information[2];
			this.Init();
		}

		public override void Action(CharacterBase character, SceneBattle battle)
		{
			if (character.waittime == 5)
				this.sound.PlaySE(MyAudio.SOUNDNAMES.sword);
			if (character.waittime <= 5)
				character.animationpoint = new Point(0, 1);
			else if (character.waittime <= 23)
				character.animationpoint = new Point((character.waittime - 5) / 3, 1);
			else if (character.waittime < 26)
				character.animationpoint = new Point(6, 1);
			else if (character.waittime >= 50)
				base.Action(character, battle);
			if (character.waittime != 14)
				return;
			int num = this.power + this.pluspower;
			character.parent.attacks.Add(this.Paralyze(new Tornado(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element, 16)
			{
				roop = 5
			}));
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
			string[] strArray;
			switch (c)
			{
				case 0:
					strArray = new string[3]
					{
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo1Line1"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo1Line2"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo1Line3")
					};
					break;
				case 1:
					strArray = new string[3]
					{
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo2Line1"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo2Line2"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo2Line3")
					};
					break;
				case 2:
					strArray = new string[3]
					{
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo3Line1"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo3Line2"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo3Line3")
					};
					break;
				default:
					strArray = new string[3]
					{
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo4Line1"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo4Line2"),
			ShanghaiEXE.Translate("Chip.ProgramAdvanceBigTyphoonCombo4Line3")
					};
					break;
			}
			for (int index = 0; index < strArray.Length; ++index)
			{
				this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
				this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
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
			base.IconRender(dg, p, select, custom, c, noicon);
		}

		public override void Render(IRenderer dg, CharacterBase character)
		{
			this._rect = new Rectangle(character.animationpoint.X * character.Wide, character.Height, character.Wide, character.Height);
			this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
			dg.DrawImage(dg, "slash", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
		}
	}
}


