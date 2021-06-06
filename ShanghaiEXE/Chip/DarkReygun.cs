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
    internal class DarkReygun : ChipBase
  {
    private const int shotend = 28;

    public DarkReygun(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 255;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DarkReygunName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 250;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this.dark = true;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.C;
      this.code[2] = ChipFolder.CODE.C;
      this.code[3] = ChipFolder.CODE.C;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DarkReygunDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void HaveUpdate(CharacterBase character)
    {
      this.power = 250 + (character.HpMax - character.Hp);
      if (this.power > 500)
        this.power = 500;
      base.HaveUpdate(character);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 15)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 28)
      {
        character.animationpoint = new Point(6, 0);
        if (character.waittime < 17)
          character.positionDirect.X -= (character.waittime - 15) * this.UnionRebirth(character.union);
      }
      else if (character.waittime < 33)
      {
        character.animationpoint = new Point(5, 0);
        character.PositionDirectSet();
      }
      else if (character.waittime == 33)
        base.Action(character, battle);
      if (character.waittime == 18)
      {
        this.ShakeStart(5, 5);
        this.sound.PlaySE(SoundEffect.bombmiddle);
        battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X + 4 * character.UnionRebirth, character.positionDirect.Y, 26, character.union, 20 + this.Random.Next(20), 2, 0));
      }
      if (character.waittime != 20)
        return;
      this.HaveUpdate(character);
      character.parent.attacks.Add(this.Paralyze(new BustorShot(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), BustorShot.SHOT.canon, this.element, false, 0)));
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
        this._rect = new Rectangle(0, 0, 56, 48);
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
        int x = 496;
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
      this._rect = new Rectangle(1080, 0, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 0;
      else if (character.waittime >= 15 && character.waittime < 28)
        this._position.X -= 2 * this.UnionRebirth(character.union);
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      if (character.waittime >= 10 && character.waittime < 28)
      {
        this._position = character.positionDirect;
        this._position.X += 24 * this.UnionRebirth(character.union);
        this._position.Y += 14f;
        this._rect = new Rectangle((character.waittime - 10) / 3 * 64, 32, 64, 64);
        dg.DrawImage(dg, "shot", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

