using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSNet;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class WhiteCard : ChipBase
  {
    private const int speed = 2;
    private const int plus = 2;

    public WhiteCard(IAudioEngine s)
      : base(s)
    {
      this.number = 141;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.WhiteCardName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 50;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.asterisk;
      this.code[1] = ChipFolder.CODE.asterisk;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.WhiteCardDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      this.sound.PlaySE(SoundEffect.docking);
      if (character is Player)
      {
        Player player = (Player) character;
        List<ChipFolder> chipFolderList = new List<ChipFolder>();
        for (int index = 0; index < 30; ++index)
        {
          ChipFolder chipFolder = battle.main.chipfolder[battle.parent.savedata.efolder, index];
          if (chipFolder.chip.powerprint && !chipFolder.chip.navi && !chipFolder.chip.dark)
            chipFolderList.Add(battle.main.chipfolder[battle.parent.savedata.efolder, index]);
        }
        if (chipFolderList.Count > 0)
        {
          for (int index = 0; index < 2; ++index)
          {
            ChipFolder chipFolder = new ChipFolder(this.sound);
            chipFolder.chip = chipFolder.ReturnChip(chipFolderList[this.Random.Next(chipFolderList.Count)].chip.number);
            player.haveChip.Insert(0, chipFolder.chip);
          }
          player.numOfChips += 2;
          player.haveChip.RemoveAll(a => a == null);
        }
      }
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
        this._rect = new Rectangle(336, 0, 56, 48);
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

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

