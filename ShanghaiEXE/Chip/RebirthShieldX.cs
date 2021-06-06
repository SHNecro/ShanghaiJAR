using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class RebirthShieldX : ChipBase
  {
    private int anime = 0;
    private const int speed = 2;
    private bool open;
    private bool close;

    public RebirthShieldX(IAudioEngine s)
      : base(s)
    {
      this.number = 365;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.RebirthShieldXName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 300;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.R;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.RebirthShieldXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 1)
      {
        this.sound.PlaySE(SoundEffect.rockopen);
        character.shield = CharacterBase.SHIELD.ReflectP;
        character.shieldUsed = false;
        character.ReflectP = this.Power(character);
        this.open = true;
      }
      if (this.open && !this.close && (Input.IsUp(Button._A) && character is Player || character.waittime > 30))
      {
        character.shield = CharacterBase.SHIELD.none;
        character.shieldUsed = false;
        character.ReflectP = 0;
        this.close = true;
        this.anime = 9;
      }
      if (this.close)
      {
        ++this.anime;
        if (this.anime < 13)
          return;
        if (character is Player)
          ((Player) character).PluspointGaia(20);
        base.Action(character, battle);
      }
      else if (character.waittime < 9)
      {
        this.anime = character.waittime;
      }
      else
      {
        if (character.waittime >= 30 || this.close)
          return;
        this.anime = 3;
      }
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 58.5f;
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
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(288, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      this._rect = new Rectangle(this.anime * 32, 0, 32, 72);
      this._position = new Vector2(character.positionDirect.X + Shake.X + 16 * this.UnionRebirth(character.union), (float) (character.positionDirect.Y + (double) this.Shake.Y + 16.0));
      dg.DrawImage(dg, "shield", this._rect, false, this._position, character.union != Panel.COLOR.blue, Color.White);
    }
  }
}

