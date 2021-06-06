using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSNet
{
    public class NetEnemy : EnemyBase
    {
        public Virus[] viruss = new Virus[3];
        public int[] enemyName = new int[10];
        public string nextStyleP;
        public Player.STYLE nextStyle;
        public ChipBase.ELEMENT nextelEment;
        public NetPlayer player;

        public NetEnemy(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          SceneMain main,
          byte bp,
          byte br,
          byte bc,
          MindWindow m,
          SaveData save)
          : base(s, p, pX, pY, 0, Panel.COLOR.blue, 1)
        {
            this.player = new NetPlayer(s, p, pX, pY, main, bp, br, bc, m, save, this);
        }

        public void ParamSet(
          int HP,
          bool[] addonSkill,
          string stylename,
          int[] virusNo,
          int[] virusBug,
          int[] virusFreeze,
          int[] virusError)
        {
            this.HPset(HP);
            this.union = Panel.COLOR.blue;
            this.player.union = this.union;
            this.player.rebirth = true;
            this.player.HPset(HP);
            this.player.picturename = stylename;
            this.player.addonSkill = addonSkill;
            for (int index = 0; index < this.viruss.Length; ++index)
            {
                if (virusNo[index] >= 0)
                {
                    this.viruss[index].type = virusNo[index];
                    this.viruss[index].eatBug = virusBug[index];
                    this.viruss[index].eatFreeze = virusFreeze[index];
                    this.viruss[index].eatError = virusError[index];
                }
            }
        }

        public override void Updata()
        {
            if (!this.blackOutObject)
                this.player.blackOutObject = false;
            this.player.badstatus = this.badstatus;
            this.positionre = this.player.positionre;
            this.position = this.player.position;
            this.player.Updata();
            base.Updata();
        }

        public override void Render(IRenderer dg)
        {
            this.player.Render(dg);
            this.HPposition = new Vector2(this.player.positionDirect.X, (float)(player.positionDirect.Y + 2.0 + this.player.Height / 2 - 16.0));
            this.Nameprint(dg, false);
        }

        public override void Nameprint(IRenderer dg, bool numberprint)
        {
            if (this.parent == null || !this.parent.namePrint)
                return;
            List<AllBase.NAME> nameList = new List<AllBase.NAME>();
            for (int index = 0; index < this.enemyName.Length; ++index)
                nameList.Add((AllBase.NAME)this.enemyName[index]);
            for (int index = nameList.Count - 1; index >= 0 && nameList[index] == AllBase.NAME.no; --index)
                nameList[index] = AllBase.NAME.none;
            nameList.RemoveAll(s => s == AllBase.NAME.none);
            AllBase.NAME[] array = nameList.ToArray();
            int length = array.Length;
            int num1 = 0;
            int num2 = !numberprint ? 240 - array.Length * 8 : 240 - array.Length * 8;
            this.color = this.alfha == 0 ? Color.FromArgb(0, byte.MaxValue, byte.MaxValue, byte.MaxValue) : Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            this._rect = new Rectangle(320, 104, 8, 16);
            this._position = new Vector2(num2 - 8 + num1, this.number * 16);
            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
            for (int index = 0; index < length; ++index)
            {
                this._position = new Vector2(num2 + 8 * index + num1, this.number * 16);
                this._rect = new Rectangle(328, 104, 8, 16);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
                this._rect = new Rectangle((int)array[index] * 8, 88, 8, 16);
                dg.DrawImage(dg, "font", this._rect, true, this._position, this.color);
            }
            if (numberprint && this.version > 1)
            {
                this._position = new Vector2(num2 + 8 * array.Length + num1, this.number * 16);
                this._rect = new Rectangle(328, 104, 8, 16);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
                this._rect = new Rectangle(version * 8, 104, 8, 16);
                dg.DrawImage(dg, "font", this._rect, true, this._position, this.color);
            }
        }

        public override void RenderUP(IRenderer dg)
        {
            this.HPRender(dg, this.HPposition, this.printhp);
        }
    }
}
