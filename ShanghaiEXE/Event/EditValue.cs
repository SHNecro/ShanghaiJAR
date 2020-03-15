using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;
using System.Windows.Forms;

namespace NSEvent
{
    internal class EditValue : EventBase
    {
        public int number;
        private readonly bool numberORvalue;
        private readonly byte math;
        private readonly byte reference;
        private readonly string referenceNo;
        private readonly Player player;

        public EditValue(
          IAudioEngine s,
          EventManager m,
          int n,
          bool nORv,
          byte ma,
          byte r,
          string rN,
          Player p,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.number = n;
            this.numberORvalue = nORv;
            this.math = ma;
            this.reference = r;
            this.referenceNo = rN;
            this.player = p;
        }

        public override void Update()
        {
            int index = this.numberORvalue ? this.savedata.ValList[this.number] : this.number;
            int b = 0;
            try
            {
                switch (this.reference)
                {
                    case 0:
                        b = int.Parse(this.referenceNo);
                        break;
                    case 1:
                        b = this.savedata.ValList[int.Parse(this.referenceNo)];
                        break;
                    case 2:
                        b = this.savedata.ValList[this.savedata.ValList[int.Parse(this.referenceNo)]];
                        break;
                    case 3:
                        string[] strArray = this.referenceNo.Split('/');
                        b = this.Random.Next(int.Parse(strArray[0]), int.Parse(strArray[1]) + 1);
                        break;
                    case 4:
                        switch (int.Parse(this.referenceNo))
                        {
                            case 0:
                                b = this.savedata.HPMax;
                                break;
                            case 1:
                                b = this.savedata.HPNow;
                                break;
                            case 2:
                                b = this.savedata.Money;
                                break;
                            case 3:
                                b = this.savedata.havePeace[0];
                                break;
                            case 4:
                                b = this.savedata.havePeace[1];
                                break;
                            case 5:
                                b = this.savedata.havePeace[2];
                                break;
                        }
                        break;
                    case 5:
                        switch (int.Parse(this.referenceNo))
                        {
                            case 0:
                                b = (int)this.player.Position.X;
                                break;
                            case 1:
                                b = (int)this.player.Position.Y;
                                break;
                            case 2:
                                b = this.player.floor;
                                break;
                        }
                        break;
                    case 6:
                        b = (int)this.player.Angle;
                        break;
                    case 7:
                        b = this.savedata.selectQuestion;
                        break;
                }
            }
            catch
            {
                int num = (int)MessageBox.Show("Variable Error:", "Assignment value invalid " + this.referenceNo.ToString());
            }
            switch (this.math)
            {
                case 0:
                    this.savedata.ValList[index] = b;
                    break;
                case 1:
                    this.savedata.ValList[index] += b;
                    break;
                case 2:
                    this.savedata.ValList[index] -= b;
                    break;
                case 3:
                    this.savedata.ValList[index] *= b;
                    break;
                case 4:
                    this.savedata.ValList[index] /= b;
                    break;
                case 5:
                    this.savedata.ValList[index] %= b;
                    break;
                case 6:
                    this.savedata.ValList[index] = (int)MyMath.Pow(this.savedata.ValList[index], b);
                    break;
            }
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
