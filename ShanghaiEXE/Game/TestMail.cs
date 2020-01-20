using System;

namespace NSGame
{
    [Serializable]
    internal class TestMail : Mail
    {
        public TestMail(int number)
        {
            switch (number)
            {
                case 0:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail1Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail1Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue6"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail1Dialogue7"));
                    break;
                case 1:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail2Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail2Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail2Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail2Dialogue2"));
                    break;
                case 2:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail3Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail3Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue6"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail3Dialogue7"));
                    break;
                case 3:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail4Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail4Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail4Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail4Dialogue2"));
                    break;
                case 4:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail5Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail5Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail5Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail5Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail5Dialogue3"));
                    break;
                case 5:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail6Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail6Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue6"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail6Dialogue7"));
                    break;
                case 6:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail7Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail7Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue6"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue7"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue8"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue9"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue10"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue11"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue12"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue13"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail7Dialogue14"));
                    break;
                case 7:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail8Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail8Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue6"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail8Dialogue7"));
                    break;
                case 8:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail9Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail9Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail9Dialogue1"));
                    break;
                case 9:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail10Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail10Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail10Dialogue1"));
                    break;
                case 10:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail11Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail11Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail11Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail11Dialogue2"));
                    break;
                case 11:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail12Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail12Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail12Dialogue1"));
                    break;
                case 12:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail13Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail13Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail13Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail13Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail13Dialogue3"));
                    break;
                case 13:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail14Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail14Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail14Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail14Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail14Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail14Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail14Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail14Dialogue6"));
                    break;
                case 14:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail15Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail15Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue6"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue7"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue8"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue9"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue10"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue11"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue12"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail15Dialogue13"));
                    break;
                case 15:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail16Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail16Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail16Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail16Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail16Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail16Dialogue4"));
                    break;
                case 16:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail17Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail17Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail17Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail17Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail17Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail17Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail17Dialogue5"));
                    break;
                case 17:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail18Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail18Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail18Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail18Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail18Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail18Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail18Dialogue5"));
                    break;
                case 18:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail19Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail19Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail19Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail19Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail19Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail19Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail19Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail19Dialogue6"));
                    break;
                case 19:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail20Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail20Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail20Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail20Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail20Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail20Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail20Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail20Dialogue6"));
                    break;
                case 20:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail21Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail21Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail21Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail21Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail21Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail21Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail21Dialogue5"));
                    break;
                case 21:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail22Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail22Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail22Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail22Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail22Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail22Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail22Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail22Dialogue6"));
                    break;
                case 22:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail23Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail23Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail23Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail23Dialogue2"));
                    break;
                case 23:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail24Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail24Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail24Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail24Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail24Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail24Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail24Dialogue5"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail24Dialogue6"));
                    break;
                case 24:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail25Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail25Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail25Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail25Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail25Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail25Dialogue4"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail25Dialogue5"));
                    break;
                case 25:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail26Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail26Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail26Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail26Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail26Dialogue3"));
                    break;
                case 26:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail27Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail27Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail27Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail27Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail27Dialogue3"));
                    break;
                case 27:
                    this.title = ShanghaiEXE.Translate("TestMail.Mail28Title");
                    this.parson = ShanghaiEXE.Translate("TestMail.Mail28Sender");
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail28Dialogue1"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail28Dialogue2"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail28Dialogue3"));
                    this.AddTXT(ShanghaiEXE.Translate("TestMail.Mail28Dialogue4"));
                    break;
            }
        }
    }
}
