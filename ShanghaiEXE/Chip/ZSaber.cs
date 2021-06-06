using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ZSaber : ChipBase
  {
    private int[] motionX = new int[14]
    {
      0,
      2,
      3,
      3,
      2,
      3,
      3,
      1,
      1,
      1,
      1,
      1,
      1,
      1
    };
    private int[] motionY = new int[14]
    {
      1,
      1,
      1,
      1,
      2,
      2,
      2,
      3,
      1,
      0,
      0,
      0,
      0,
      0
    };
    private const int start = 3;
    private const int speed = 2;
    private int waittime;

    public ZSaber(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.swordtype = true;
      this.number = 138;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ZSaberName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 40;
      this.reality = 5;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.E;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.O;
      this.code[3] = ChipFolder.CODE.Z;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ZSaberDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (this.moveflame)
      {
        character.animationpoint.X = this.motionX[this.waittime];
        character.animationpoint.Y = this.motionY[this.waittime];
        switch (this.waittime)
        {
          case 1:
            this.sound.PlaySE(SoundEffect.Zblade);
            AttackBase a1 = new LanceAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, true);
            a1.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a1));
            break;
          case 4:
            this.sound.PlaySE(SoundEffect.Zblade);
            AttackBase a2 = new LanceAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, true);
            a2.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a2));
            break;
          case 7:
            this.sound.PlaySE(SoundEffect.Zblade);
            character.parent.attacks.Add(this.Paralyze(new LanceAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, true)));
            break;
          case 12:
            base.Action(character, battle);
            break;
        }
        ++this.waittime;
      }
      this.FlameControl(4);
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
        this._rect = new Rectangle(112, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic10", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(this.waittime * character.Wide, 3 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, "slash", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

