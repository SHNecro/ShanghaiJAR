using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DarkSpark : ChipBase
  {
    private const int shotend = 28;

    public DarkSpark(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 256;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DarkSparkName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 350;
      this.regsize = 99;
      this.reality = 5;
      this.subpower = 0;
      this.dark = true;
      this._break = false;
      this.powerprint = true;
      this.shild = true;
      this.code[0] = ChipFolder.CODE.J;
      this.code[1] = ChipFolder.CODE.J;
      this.code[2] = ChipFolder.CODE.J;
      this.code[3] = ChipFolder.CODE.J;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DarkSparkDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.Return(new int[4]
      {
        5,
        15,
        17,
        20
      }, new int[7]{ 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 15)
      {
        character.animationpoint = new Point(5, 0);
        character.shield = CharacterBase.SHIELD.Normal;
      }
      else if (character.waittime < 28)
        character.animationpoint = new Point(6, 0);
      else if (character.waittime < 33)
      {
        character.animationpoint = new Point(5, 0);
        character.shield = CharacterBase.SHIELD.none;
        character.PositionDirectSet();
      }
      else if (character.waittime == 33)
        base.Action(character, battle);
      if (character.waittime == 18)
        this.sound.PlaySE(SoundEffect.beamlong);
      if (character.waittime != 20)
        return;
      int num = this.power + this.pluspower;
      AttackBase a = new Beam(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, true);
      a.breakinvi = true;
      a.color = Color.PaleVioletRed;
      character.parent.attacks.Add(this.Paralyze(a));
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
        this._rect = new Rectangle(56, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic13", this._rect, true, p, Color.White);
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
        int x = 512;
        int num1 = 64;
        int num2 = this.number - 1;
        int num3 = num2 % 40;
        int num4 = num2 / 40;
        int num5 = 0;
        if (select)
          num5 = 1;
        this._rect = new Rectangle(x, num1 + num5 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(480, 600, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 360;
      else if (character.waittime >= 20)
        this._rect.X = 600;
      else if (character.waittime >= 15 && character.waittime < 28)
        this._position.X -= 2 * this.UnionRebirth(character.union);
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

