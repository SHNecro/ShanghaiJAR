using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;
using NSGame;

namespace NSChip
{
    internal class ProtonThunder : ChipBase
	{
		private const int start = 44;
		private const int speed = 2;
		private int count;
		private bool eye;
		private bool soundFlag;
		private Point position;
		private ScreenBlack screen;
		private Point animePoint;

		public ProtonThunder(IAudioEngine s)
		  : base(s)
		{
			this.dark = true;
			this.navi = true;
			this.number = 298;
			this.name = NSGame.ShanghaiEXE.Translate("Chip.ProtonThunderName");
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
			var information = NSGame.ShanghaiEXE.Translate("Chip.ProtonThunderDesc");
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
			if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
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
			int num = 30;
			if (character.waittime == 44)
				this.sound.PlaySE(SoundEffect.pikin);
			if (character.waittime == 44 + num)
				battle.effects.Add(new Charge(this.sound, battle, this.position.X, this.position.Y));
			if (character.waittime > 44 + num + 50)
			{
				this.FlameControl(1);
				if (this.count < 10)
				{
					if (this.frame > 20)
					{
						this.frame = 0;
						this.sound.PlaySE(SoundEffect.thunder);
						AttackBase a = new ElekiFang(this.sound, battle, character.union == Panel.COLOR.red ? 2 : 3, 1, character.union, this.Power(character), 2, this.element, this.count % 2 == 0);
						a.knock = true;
						battle.attacks.Add(this.Paralyze(a));
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
            switch (c % 2)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 3, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
				        ShanghaiEXE.Translate("Chip.ProgramAdvanceProtonThunderCombo1Line1"),
				        ShanghaiEXE.Translate("Chip.ProgramAdvanceProtonThunderCombo1Line2"),
				        ShanghaiEXE.Translate("Chip.ProgramAdvanceProtonThunderCombo1Line3")
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
				this._rect = new Rectangle(608, 80 + num * 96, 16, 16);
				dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
			}
			base.IconRender(dg, p, select, custom, 0, noicon);
		}

		public override void Render(IRenderer dg, CharacterBase character)
		{
			if (character.animationpoint.X == -1 && character.waittime > 44)
			{
				int y = 184;
				int width = 104;
				int height = 88;
				this._rect = new Rectangle(this.eye ? width : 0, y, width, height);
				this._position = new Vector2((character.union == Panel.COLOR.red ? 1 : 4) * 40 + 20, 1 * 24 + 58);
				dg.DrawImage(dg, "darkPA", this._rect, false, this._position, (uint)character.union > 0U, Color.White);
			}
			else
				this.BlackOutRender(dg, character.union);
		}
	}
}

