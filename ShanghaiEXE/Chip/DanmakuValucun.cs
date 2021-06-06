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
    internal class DanmakuValucun : ChipBase
  {
    private const int shotend = 10;
    private const int shotinterval = 4;

    public DanmakuValucun(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 137;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DanmakuValucunName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 30;
      this.regsize = 86;
      this.reality = 5;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.J;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.V;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DanmakuValucunDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      character.animationpoint = DanmakuValucun.Animation(character.waittime);
      if (character.waittime == 80)
        base.Action(character, battle);
      if (character.waittime == 2)
        this.sound.PlaySE(SoundEffect.machineRunning);
      if (character.waittime % 8 != 4)
        return;
      battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X + 4 * character.UnionRebirth, character.positionDirect.Y + 8f, 26, character.union, 20 + this.Random.Next(20), 2, 0));
      int num = this.power + this.pluspower;
      this.sound.PlaySE(SoundEffect.canon);
      this.ShakeStart(1, 8);
      character.parent.attacks.Add(this.Paralyze(new Vulcan(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), Vulcan.SHOT.Vulcan, this.element, false)));
    }

    public static Point Animation(int waittime)
    {
      int[] interval = new int[12];
      for (int index = 0; index < 12; ++index)
        interval[index] = 4 * index;
      int[] xpoint = new int[14]
      {
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6
      };
      int y = 0;
      return CharacterAnimation.Return(interval, xpoint, y, waittime);
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
      this._rect = new Rectangle(10 * character.Wide, 4 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime % 8 >= 2 && character.waittime % 8 <= 3)
        this._rect.X += 120;
      else if (character.waittime % 8 >= 4 && character.waittime % 8 <= 5)
        this._rect.X += 240;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

