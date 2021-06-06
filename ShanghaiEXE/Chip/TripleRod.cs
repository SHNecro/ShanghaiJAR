using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class TripleRod : ChipBase
  {
    private int[] motionX = new int[8]
    {
      0,
      1,
      2,
      3,
      2,
      3,
      2,
      3
    };
    private const int start = 3;
    private const int speed = 2;
    private int waittime;

    public TripleRod(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.swordtype = true;
      this.number = 139;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TripleRodName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 90;
      this.subpower = 0;
      this.regsize = 40;
      this.reality = 5;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.R;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.Z;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TripleRodDesc");
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
        character.animationpoint.Y = 3;
        switch (this.waittime)
        {
          case 2:
            this.sound.PlaySE(SoundEffect.Zblade);
            IAudioEngine sound = this.sound;
            SceneBattle parent = character.parent;
            int x = character.position.X;
            this.UnionRebirth(character.union);
            int pX = x + 0;
            int y = character.position.Y;
            int union = (int) character.union;
            int po = this.Power(character);
            int element = (int) this.element;
            AttackBase a1 = new DrillAttack(sound, parent, pX, y, (Panel.COLOR)union, po, 2, (ChipBase.ELEMENT)element);
            a1.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a1));
            break;
          case 4:
            this.sound.PlaySE(SoundEffect.Zblade);
            AttackBase a2 = new DrillAttack(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element);
            a2.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a2));
            break;
          case 6:
            this.sound.PlaySE(SoundEffect.Zblade);
            AttackBase a3 = new DrillAttack(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element);
            a3.invincibility = false;
            character.parent.attacks.Add(this.Paralyze(a3));
            break;
          case 7:
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
        this._rect = new Rectangle(56, 0, 56, 48);
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
      this._rect = new Rectangle(this.waittime * (character.Wide * 2), 3 * character.Height, character.Wide * 2, character.Height);
      this._position = new Vector2(character.positionDirect.X + character.Wide / 2 * this.UnionRebirth(character.union) + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, "Lances", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

