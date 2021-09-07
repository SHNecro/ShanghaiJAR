using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PowerOfEarth : ChipBase
  {
    private const int start = 1;
    private const int speed = 2;

    public PowerOfEarth(IAudioEngine s)
      : base(s)
    {
      this.number = 52;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PowerOfEarthName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 150;
      this.subpower = 0;
      this.regsize = 30;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.E;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.S;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PowerOfEarthDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime <= 1)
        character.animationpoint = new Point(0, 1);
      else if (character.waittime <= 7)
        character.animationpoint = new Point((character.waittime - 1) / 2, 1);
      else if (character.waittime < 15)
        character.animationpoint = new Point(3, 1);
      else if (character.waittime == 15)
      {
        for (int pY = 0; pY < battle.panel.GetLength(1); ++pY)
        {
          int pX1 = 0;
          if ((uint) battle.panel[pX1, pY].Element > 0U)
          {
            this.ShakeStart(4, 2);
            battle.attacks.Add(this.Paralyze(new FootPanel(this.sound, battle, pX1, pY, character.union, this.Power(character), 0, FootPanel.MOTION.init, battle.panel[pX1, pY].Element, false)));
          }
          int pX2 = 5;
          if ((uint) battle.panel[pX2, pY].Element > 0U)
          {
            this.ShakeStart(4, 2);
            battle.attacks.Add(this.Paralyze(new FootPanel(this.sound, battle, pX2, pY, character.union, this.Power(character), 0, FootPanel.MOTION.init, battle.panel[pX2, pY].Element, false)));
          }
        }
      }
      else if (character.waittime == 23)
      {
        for (int pY = 0; pY < battle.panel.GetLength(1); ++pY)
        {
          int pX1 = 1;
          if ((uint) battle.panel[pX1, pY].Element > 0U)
          {
            this.ShakeStart(4, 2);
            battle.attacks.Add(this.Paralyze(new FootPanel(this.sound, battle, pX1, pY, character.union, this.Power(character), 0, FootPanel.MOTION.init, battle.panel[pX1, pY].Element, false)));
          }
          int pX2 = 4;
          if ((uint) battle.panel[pX2, pY].Element > 0U)
          {
            this.ShakeStart(4, 2);
            battle.attacks.Add(this.Paralyze(new FootPanel(this.sound, battle, pX2, pY, character.union, this.Power(character), 0, FootPanel.MOTION.init, battle.panel[pX2, pY].Element, false)));
          }
        }
      }
      else if (character.waittime == 31)
      {
        for (int pY = 0; pY < battle.panel.GetLength(1); ++pY)
        {
          int pX1 = 2;
          if ((uint) battle.panel[pX1, pY].Element > 0U)
          {
            this.ShakeStart(4, 2);
            battle.attacks.Add(this.Paralyze(new FootPanel(this.sound, battle, pX1, pY, character.union, this.Power(character), 0, FootPanel.MOTION.init, battle.panel[pX1, pY].Element, false)));
          }
          int pX2 = 3;
          if ((uint) battle.panel[pX2, pY].Element > 0U)
          {
            this.ShakeStart(4, 2);
            battle.attacks.Add(this.Paralyze(new FootPanel(this.sound, battle, pX2, pY, character.union, this.Power(character), 0, FootPanel.MOTION.init, battle.panel[pX2, pY].Element, false)));
          }
        }
      }
      else
      {
        if (character.waittime < 31)
          return;
        if (character is Player)
          ((Player) character).PluspointWing(80);
        base.Action(character, battle);
      }
    }

    public static int SteelX(CharacterBase character, SceneBattle battle)
    {
      int num = 99;
      if (num == 99)
      {
        if (character.union == Panel.COLOR.red)
        {
          for (int index1 = 0; index1 < battle.panel.GetLength(0); ++index1)
          {
            for (int index2 = 0; index2 < battle.panel.GetLength(1); ++index2)
            {
              if (!battle.panel[index1, index2].inviolability)
              {
                num = index1;
                break;
              }
            }
            if (num != 99)
              break;
          }
        }
        else
        {
          for (int index1 = battle.panel.GetLength(0) - 1; index1 >= 0; --index1)
          {
            for (int index2 = battle.panel.GetLength(1) - 1; index2 >= 0; --index2)
            {
              if (!battle.panel[index1, index2].inviolability)
              {
                num = index1;
                break;
              }
            }
            if (num != 99)
              break;
          }
        }
      }
      return num;
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
        this._rect = new Rectangle(784, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

