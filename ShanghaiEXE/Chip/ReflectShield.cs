using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ReflectShield : ChipBase
  {
    private int anime = 0;
    private const int speed = 2;
    private bool open;
    private bool close;

    public ReflectShield(IAudioEngine s)
      : base(s)
    {
      this.number = 58;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ReflectShieldName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 45;
      this.reality = 3;
      this.shild = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.F;
      this.code[1] = ChipFolder.CODE.J;
      this.code[2] = ChipFolder.CODE.K;
      this.code[3] = ChipFolder.CODE.O;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ReflectShieldDesc");
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
        character.shield = CharacterBase.SHIELD.Reflect;
        character.shieldUsed = false;
        character.ReflectP = this.Power(character);
        if (!this.BlackOutEnd(character, battle))
        {
          this.anime = 3;
          character.waittime = 0;
          return;
        }
      }
      if (this.open && !this.close && (Input.IsUp(Button._A) && character is Player || character.waittime > 30))
      {
        character.shield = CharacterBase.SHIELD.none;
        character.shieldUsed = false;
        character.ReflectP = this.Power(character);
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
      else if (character.waittime >= 0 && character.waittime < 9)
      {
        this.anime = character.waittime;
        if (character.waittime != 5)
          return;
        this.open = true;
      }
      else
      {
        if (character.waittime >= 30 || this.close)
          return;
        this.anime = 3;
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
        this._rect = new Rectangle(448, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic14", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(this.anime * 32, 144, 32, 72);
      this._position = new Vector2(character.positionDirect.X + Shake.X + 16 * this.UnionRebirth(character.union), (float) (character.positionDirect.Y + (double) this.Shake.Y + 16.0));
      dg.DrawImage(dg, "shield", this._rect, false, this._position, character.union != Panel.COLOR.blue, Color.White);
    }
  }
}

