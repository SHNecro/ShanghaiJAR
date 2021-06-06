using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class Flyng : ChipBase
  {
    private const int speed = 2;
    private bool fly;

    public Flyng(IAudioEngine s)
      : base(s)
    {
      this.printIcon = false;
      this.number = 0;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.FlyngName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this._break = false;
      this.powerprint = false;
      this.shadow = false;
      this.code[0] = ChipFolder.CODE.none;
      this.code[1] = ChipFolder.CODE.none;
      this.code[2] = ChipFolder.CODE.none;
      this.code[3] = ChipFolder.CODE.none;
      var information = NSGame.ShanghaiEXE.Translate("Chip.FlyngDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!(character is Player))
        return;
      Player player = (Player) character;
      if (!this.fly)
      {
        Vector2 pd = new Vector2(character.positionDirect.X, character.positionDirect.Y);
        battle.effects.Add(new MoveEnemy(this.sound, battle, pd, character.position));
        this.sound.PlaySE(SoundEffect.lance);
        player.printplayer = false;
        this.fly = true;
      }
      else
      {
        ++this.frame;
        for (int index = 0; index < 6; ++index)
        {
          if (Input.IsPress((Button) index) || this.frame > 180)
          {
            player.printplayer = true;
            base.Action(character, battle);
            break;
          }
        }
      }
    }

    public override void GraphicsRender(
      IRenderer dg,
      Vector2 p,
      int c,
      bool printgraphics,
      bool printstatus)
    {
    }

    public override void IconRender(
      IRenderer dg,
      Vector2 p,
      bool select,
      bool custom,
      int c,
      bool noicon)
    {
    }

    public override void Render(IRenderer dg, CharacterBase player)
    {
      this._rect = new Rectangle(0, 120, player.Wide, player.Height);
      this._position = player.positionDirect;
      dg.DrawImage(dg, "Silhouette", this._rect, false, this._position, Color.White);
    }
  }
}

