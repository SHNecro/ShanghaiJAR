using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DarkHurricane : ChipBase
  {
    private const int start = 5;
    private const int speed = 3;

    public DarkHurricane(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 263;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DarkHurricaneName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 40;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.powerprint = true;
      this.dark = true;
      this.code[0] = ChipFolder.CODE.F;
      this.code[1] = ChipFolder.CODE.F;
      this.code[2] = ChipFolder.CODE.F;
      this.code[3] = ChipFolder.CODE.F;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DarkHurricaneDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 5)
        this.sound.PlaySE(SoundEffect.sword);
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
      character.parent.attacks.Add(this.Paralyze(new NSAttack.Storm(this.sound, battle, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)
      {
          cEle = ChipBase.ELEMENT.poison
      }));
      character.parent.attacks.Add(this.Paralyze(new NSAttack.Storm(this.sound, battle, character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)
      {
          cEle = ChipBase.ELEMENT.poison
      }));
      character.parent.attacks.Add(this.Paralyze(new NSAttack.Storm(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)
      {
          cEle = ChipBase.ELEMENT.poison
      }));
      character.parent.attacks.Add(this.Paralyze(new NSAttack.Storm(this.sound, battle, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.Power(character), this.element)
      {
          cEle = ChipBase.ELEMENT.poison
      }));
      character.parent.attacks.Add(this.Paralyze(new NSAttack.Storm(this.sound, battle, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.Power(character), this.element)
      {
          cEle = ChipBase.ELEMENT.poison
      }));
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
        this._rect = new Rectangle(392, 0, 56, 48);
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
        int x = 560;
        int num1 = 80;
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
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, "slash", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

