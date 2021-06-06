using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DarkFang : ChipBase
  {
    private const int speed = 4;
    private const int shotend = 16;

    public DarkFang(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = 260;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DarkFangName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 250;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.dark = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.P;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.P;
      this.code[3] = ChipFolder.CODE.P;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DarkFangDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 15)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 16)
        character.animationpoint = new Point(6, 0);
      else if (character.waittime < 21)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime == 40)
        base.Action(character, battle);
      if (character.waittime != 16)
        return;
      this.sound.PlaySE(SoundEffect.thunder);
      int num = this.power + this.pluspower;
      ElekiFang elekiFang = new ElekiFang(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 1, this.element, false);
      elekiFang.BadStatusSet(CharacterBase.BADSTATUS.paralyze, 180);
      character.parent.attacks.Add(this.Paralyze(elekiFang));
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
        this._rect = new Rectangle(616, 0, 56, 48);
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
        int x = 576;
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
      this._rect = new Rectangle(11 * character.Wide, 6 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X -= 120;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

