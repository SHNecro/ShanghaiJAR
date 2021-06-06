using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DigDrill1 : ChipBase
  {
    protected int drillAnime = 0;
    private const int start = 5;
    private const int speed = 3;
    private int hitcount;

    public DigDrill1(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.number = 74;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DigDrill1Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 50;
      this.subpower = 0;
      this.regsize = 22;
      this.reality = 1;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.D;
      this.code[2] = ChipFolder.CODE.M;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DigDrill1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 17)
      {
        this.sound.PlaySE(SoundEffect.drill2);
        this.drillAnime = 2;
      }
      if (character.waittime < 5)
        character.animationpoint = new Point(0, 3);
      else if (character.waittime < 17)
        character.animationpoint = new Point((character.waittime - 5) / 3, 3);
      else if (character.waittime < 44)
      {
        if ((character.waittime - 5) % 3 != 0)
          return;
        ++this.drillAnime;
        if (this.drillAnime >= 3)
        {
          DrillAttack drillAttack = new DrillAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element);
          this.drillAnime = 0;
          ++this.hitcount;
          if (this.hitcount < 3)
            drillAttack.panelChange = false;
          else
            drillAttack.hitted = true;
          character.parent.attacks.Add(this.Paralyze(drillAttack));
        }
      }
      else if (character.waittime < 53)
        character.animationpoint = new Point(4, 0);
      else
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
        this._rect = new Rectangle(504, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
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
        this._rect = this.IconRect(select);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime <= 11 || character.waittime >= 53)
        return;
      this._rect = new Rectangle(character.animationpoint.X * (character.Wide * 2) + this.drillAnime * (character.Wide * 2), character.Height, character.Wide * 2, character.Height);
      double num1 = character.positionDirect.X + (double) (character.Wide / 2 * this.UnionRebirth(character.union));
      Point shake = this.Shake;
      double x = shake.X;
      double num2 = num1 + x;
      double y1 = character.positionDirect.Y;
      shake = this.Shake;
      double y2 = shake.Y;
      double num3 = y1 + y2;
      this._position = new Vector2((float) num2, (float) num3);
      dg.DrawImage(dg, "Lances", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

