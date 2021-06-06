using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Repair50 : ChipBase
  {
    private const int speed = 2;

    public Repair50(IAudioEngine s)
      : base(s)
    {
      this.number = 175;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.Repair50Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 50;
      this.regsize = 12;
      this.reality = 1;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.F;
      this.code[1] = ChipFolder.CODE.I;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.Repair50Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 1)
      {
        this.sound.PlaySE(SoundEffect.repair);
        battle.effects.Add(new Repair(this.sound, battle, new Vector2((int)character.positionDirect.X * this.UnionRebirth(character.union), (int)character.positionDirect.Y + 16), 2, character.position));
        character.Hp += this.subpower;
      }
      if (character.waittime < 12)
        return;
      if (character is Player)
        ((Player) character).PluspointGaia(20);
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
        this._rect = new Rectangle(56, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic9", this._rect, true, p, Color.White);
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

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

