using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ThujigiriCross : ChipBase
  {
    private const int start = 3;
    private const int speed = 2;

    public ThujigiriCross(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.swordtype = true;
      this.number = 69;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ThujigiriCrossName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 49;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.N;
      this.code[1] = ChipFolder.CODE.T;
      this.code[2] = ChipFolder.CODE.U;
      this.code[3] = ChipFolder.CODE.Z;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ThujigiriCrossDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 3)
      {
        if (character.Step())
        {
          this.sound.PlaySE(SoundEffect.sword);
        }
        else
        {
          base.Action(character, battle);
          return;
        }
      }
      character.animationpoint = CharacterAnimation.SworsAnimation(character.waittime);
      if (character.waittime >= 30)
      {
        Rectangle r = new Rectangle(character.animationpoint.X * character.Wide, character.animationpoint.Y * character.Height, character.Wide, character.Height);
        battle.effects.Add(new StepShadow(this.sound, battle, r, character.positionDirect, character.picturename, character.union == Panel.COLOR.blue, character.position));
        base.Action(character, battle);
      }
      if (character.waittime != 10)
        return;
      int num = this.power + this.pluspower;
      bool par = false;
      if (character is Player && ((Player) character).style == Player.STYLE.shinobi)
        par = true;
      character.parent.attacks.Add(this.Paralyze(new SwordCloss(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 3, this.element, par), character));
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
        this._rect = new Rectangle(728, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic5", this._rect, true, p, Color.White);
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
      if (character.waittime > 25)
        return;
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, 5 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

