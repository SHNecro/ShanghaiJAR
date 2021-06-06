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
    internal class DreamMeteo : ChipBase
	{
		private const int start = 44;
		private const int speed = 2;
		private const int meteotime = 30;
		private int count;
		private bool eye;
		private bool soundFlag;
		private Point position;
		private ScreenBlack screen;
		private Point animePoint;

		public DreamMeteo(IAudioEngine s)
		  : base(s)
		{
			this.dark = true;
			this.navi = true;
			this.number = 296;
			this.name = NSGame.ShanghaiEXE.Translate("Chip.DreamMeteoName");
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
			var information = NSGame.ShanghaiEXE.Translate("Chip.DreamMeteoDesc");
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
			{
				Charge charge = new Charge(this.sound, battle, this.position.X, this.position.Y);
				charge.positionDirect.Y -= 24f;
				battle.effects.Add(charge);
			}
			if (character.waittime > 44 + num + 50)
			{
				this.FlameControl(1);
				Point point = new Point();
				if (this.count < 5)
				{
					switch (this.frame)
					{
						case 1:
							switch (this.count)
							{
								case 0:
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 0;
									break;
								case 1:
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 1;
									break;
								case 2:
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 0;
									break;
								case 3:
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 1;
									break;
								case 4:
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 1;
									break;
							}
							NSEffect.DreamMeteo dreamMeteo = new NSEffect.DreamMeteo(this.sound, battle, point.X, point.Y, 30, (uint)character.union > 0U);
							if (this.count < 4)
							{
								dreamMeteo.positionDirect.X += 20 * this.UnionRebirth(character.union);
								dreamMeteo.positionDirect.Y += 12f;
							}
							battle.effects.Add(dreamMeteo);
							break;
						case 30:
							this.sound.PlaySE(SoundEffect.bombmiddle);
							switch (this.count)
							{
								case 0:
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 0;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 0;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									this.frame = 0;
									++this.count;
									break;
								case 1:
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 3 : 2;
									point.Y = 2;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 2;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									this.frame = 0;
									++this.count;
									break;
								case 2:
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 0;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 5 : 0;
									point.Y = 0;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 5 : 0;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									this.frame = 0;
									++this.count;
									break;
								case 3:
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 5 : 0;
									point.Y = 1;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 4 : 1;
									point.Y = 2;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									point.X = character.union == Panel.COLOR.red ? 5 : 0;
									point.Y = 2;
									battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
									battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
									this.frame = 0;
									++this.count;
									break;
								case 4:
									this.frame = 11;
									++this.count;
									break;
							}
							this.ShakeStart(8, 8);
							break;
					}
				}
				else if (this.count < 8)
				{
					if (this.frame == 12)
					{
						switch (this.count)
						{
							case 5:
								point.X = character.union == Panel.COLOR.red ? 3 : 2;
								point.Y = 0;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								point.Y = 1;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								point.Y = 2;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								this.frame = 0;
								++this.count;
								break;
							case 6:
								this.sound.PlaySE(SoundEffect.bombmiddle);
								point.X = character.union == Panel.COLOR.red ? 4 : 1;
								point.Y = 0;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								point.Y = 1;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								point.Y = 2;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								this.frame = 0;
								++this.count;
								break;
							case 7:
								this.sound.PlaySE(SoundEffect.bombmiddle);
								point.X = character.union == Panel.COLOR.red ? 5 : 0;
								point.Y = 0;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								point.Y = 1;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								point.Y = 2;
								battle.effects.Add(new ImpactBomb(this.sound, battle, point.X, point.Y));
								battle.attacks.Add(this.Paralyze(new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, this.element)));
								this.frame = 0;
								++this.count;
								break;
						}
						this.ShakeStart(8, 8);
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
						{
							character.barrierType = CharacterBase.BARRIER.PowerAura;
							character.barierPower = 100;
							character.barierTime = 7200;
							this.screen.end = true;
						}
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
                    this._rect = new Rectangle(56 * 1, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDreamMeteoCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDreamMeteoCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceDreamMeteoCombo1Line3")
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
				int y = 0;
				int width = 88;
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

