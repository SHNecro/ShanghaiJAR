﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NSGame;

namespace NSChip
{
    internal class InfiniteHands : ChipBase
	{
		private const int start = 44;
		private const int speed = 2;
		private int count;
		private bool eye;
		private bool soundFlag;
		private Point position;
		private ScreenBlack screen;
		private Point[] posis;
		private Point animePoint;

		public InfiniteHands(IAudioEngine s)
		  : base(s)
		{
			this.dark = true;
			this.navi = true;
			this.number = 300;
			this.name = NSGame.ShanghaiEXE.Translate("Chip.InfiniteHandsName");
			this.element = ChipBase.ELEMENT.normal;
			this.power = 150;
			this.subpower = 0;
			this._break = false;
			this.shadow = false;
			this.powerprint = true;
			this.code[0] = ChipFolder.CODE.none;
			this.code[1] = ChipFolder.CODE.none;
			this.code[2] = ChipFolder.CODE.none;
			this.code[3] = ChipFolder.CODE.none;
			var information = NSGame.ShanghaiEXE.Translate("Chip.InfiniteHandsDesc");
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
			}, new int[10] { -1, 9, 8, 7, 3, 0, 3, 7, 13, 14 }, 0, waittime);
		}

		public override void Action(CharacterBase character, SceneBattle battle)
		{
			if (this.screen == null)
			{
				this.screen = new ScreenBlack(this.sound, battle, new Vector2(), new Point(), this.element, 0, false);
				battle.effects.Add(screen);
				this.position = new Point(character.union == Panel.COLOR.red ? 1 : 4, 2);
			}
			CharacterBase character1 = character;
			SceneBattle parent = battle;
			string name = this.name;
			int num1 = this.Power(character);
			string power = num1.ToString();
			if (!this.BlackOut(character1, parent, name, power))
				return;
			switch (character.waittime)
			{
				case 1:
					character.animationpoint.X = 1;
					break;
				case 3:
					character.animationpoint.X = 2;
					break;
				case 5:
					character.animationpoint.X = 3;
					break;
				case 7:
					character.animationpoint.X = -1;
					break;
			}
			if (character.waittime % 2 == 0)
				this.eye = !this.eye;
			int num2 = 30;
			if (character.waittime == 44)
			{
				this.sound.PlaySE(SoundEffect.pikin);
				List<Point> source = new List<Point>();
				for (int x = 0; x < battle.panel.GetLength(0); ++x)
				{
					for (int y = 0; y < battle.panel.GetLength(1); ++y)
					{
						if (battle.panel[x, y].color == character.UnionEnemy)
							source.Add(new Point(x, y));
					}
				}
				this.posis = source.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToArray<Point>();
			}
			if (character.waittime == 44 + num2)
				battle.effects.Add(new Charge(this.sound, battle, this.position.X, this.position.Y));
			if (character.waittime > 44 + num2 + 50)
			{
				this.FlameControl(1);
				if (this.count < 45)
				{
					if (this.frame > 10)
					{
						this.frame = 0;
						this.sound.PlaySE(SoundEffect.canon);
						Point point = new Point();
						point.X = character.union == Panel.COLOR.red ? 2 : 3;
						point.Y = 1;
						Vector2 position = this._position;
						position.Y += 80f;
						AttackBase a1 = new DarkHand(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, position, this.posis[this.count % this.posis.Length], 15);
						num1 = this.posis[this.count % this.posis.Length].X;
						string str1 = num1.ToString();
						num1 = this.posis[this.count % this.posis.Length].Y;
						string str2 = num1.ToString();
						battle.attacks.Add(this.Paralyze(a1));
						++this.count;
						AttackBase a2 = new DarkHand(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, position, this.posis[this.count % this.posis.Length], 15);
						num1 = this.posis[this.count % this.posis.Length].X;
						string str3 = num1.ToString();
						num1 = this.posis[this.count % this.posis.Length].Y;
						string str4 = num1.ToString();
						battle.attacks.Add(this.Paralyze(a2));
						++this.count;
						AttackBase a3 = new DarkHand(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, position, this.posis[this.count % this.posis.Length], 15);
						num1 = this.posis[this.count % this.posis.Length].X;
						string str5 = num1.ToString();
						num1 = this.posis[this.count % this.posis.Length].Y;
						string str6 = num1.ToString();
						battle.attacks.Add(this.Paralyze(a3));
						++this.count;
					}
				}
				else if (this.frame > 60)
				{
					switch (this.frame)
					{
						case 61:
							character.animationpoint.X = 3;
							break;
						case 63:
							character.animationpoint.X = 2;
							break;
						case 65:
							character.animationpoint.X = 1;
							break;
						case 67:
							character.animationpoint.X = 0;
							break;
					}
					if (this.frame > 80)
					{
						if (!this.screen.end)
							this.screen.end = true;
						if (this.BlackOutEnd(character, battle))
							base.Action(character, battle);
					}
				}
			}
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
			string[] strArray = new string[3]
			{
				ShanghaiEXE.Translate("Chip.ProgramAdvanceInfiniteHandsCombo1Line1"),
				ShanghaiEXE.Translate("Chip.ProgramAdvanceInfiniteHandsCombo1Line2"),
				ShanghaiEXE.Translate("Chip.ProgramAdvanceInfiniteHandsCombo1Line3")
			};
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
				this._rect = new Rectangle(608, 80 + num * 96, 16, 16);
				dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
			}
			base.IconRender(dg, p, select, custom, c, noicon);
		}

		public override void Render(IRenderer dg, CharacterBase character)
		{
			if (character.animationpoint.X == -1 && character.waittime > 44)
			{
				int y = 416;
				int width = 128;
				int height = 136;
				this._rect = new Rectangle(this.eye ? width : 0, y, width, height);
				this._position = new Vector2((character.union == Panel.COLOR.red ? 1 : 4) * 40 + 20, 1 * 24 + 58);
				dg.DrawImage(dg, "darkPA", this._rect, false, this._position, (uint)character.union > 0U, Color.White);
			}
			else
				this.BlackOutRender(dg, character.union);
		}
	}
}

