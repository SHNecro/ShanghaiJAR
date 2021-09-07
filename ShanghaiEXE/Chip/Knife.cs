using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Knife : ChipBase
  {
    private const int start = 3;
    private const int speed = 1;
    private bool command;

    public Knife(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.swordtype = true;
      this.number = 59;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.KnifeName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 8;
      this.reality = 1;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.K;
      this.code[1] = ChipFolder.CODE.L;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.KnifeDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character is Player)
      {
        Player player = (Player) character;
        if (Input.IsPush(Button._A) && !this.command && this.commandTime < 10)
        {
          character.waittime = 0;
          this.CommandInput("上下左右", player);
          if (this.CommandCheck("下右"))
          {
            this.sound.PlaySE(SoundEffect.CommandSuccess);
            this.command = true;
          }
        }
      }
      if (!this.command)
      {
        if (character.waittime == 3)
          this.sound.PlaySE(SoundEffect.sword);
        if (character.waittime <= 3)
          character.animationpoint = new Point(0, 1);
        else if (character.waittime <= 9)
          character.animationpoint = new Point((character.waittime - 3) / 1, 1);
        else
          base.Action(character, battle);
        if (character.waittime != 5)
          return;
        int num = this.power + this.pluspower;
        character.parent.attacks.Add(this.Paralyze(new KnifeAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, false), character));
      }
      else
      {
        if (character.waittime <= 3)
          character.animationpoint = new Point(0, 1);
        else if (character.waittime <= 6)
          character.animationpoint = new Point((character.waittime - 3) / 1, 1);
        else if (character.waittime <= 10)
          character.animationpoint = new Point(3, 1);
        else
          base.Action(character, battle);
        if (character.waittime == 5)
        {
          int num = this.power + this.pluspower;
          this.sound.PlaySE(SoundEffect.knife);
          character.parent.attacks.Add(this.Paralyze(new ThrowKnife(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, 10, this.Power(character), 0, 0, 3)));
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
      if (printgraphics)
      {
        this._rect = new Rectangle(168, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic1", this._rect, true, p, Color.White);
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
      if (!this.command)
      {
        this._rect = new Rectangle(character.animationpoint.X * character.Wide, 5 * character.Height, character.Wide, character.Height);
        double x1 = character.positionDirect.X;
        Point shake = this.Shake;
        double x2 = shake.X;
        double num1 = x1 + x2;
        double y1 = character.positionDirect.Y;
        shake = this.Shake;
        double y2 = shake.Y;
        double num2 = y1 + y2;
        this._position = new Vector2((float) num1, (float) num2);
        dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
      else
      {
        this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
        double x1 = character.positionDirect.X;
        Point shake = this.Shake;
        double x2 = shake.X;
        double num1 = x1 + x2;
        double y1 = character.positionDirect.Y;
        shake = this.Shake;
        double y2 = shake.Y;
        double num2 = y1 + y2;
        this._position = new Vector2((float) num1, (float) num2);
        dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

