using Common;
using NSGame;
using System.Collections.Generic;
using System.Xml;

namespace Data
{
    public enum MessageType
    {
        ShanghaiInfo = 0, AliceInfo, ShanghaiRequestInfo, AliceRequestInfo, HumorInfo,
        UNKNOWN1, UNKNOWN2, EirinCallInfo, RequestBoard, RequestBoardComplete, GenBoard,
        UniversityBoard, EienBoard, UnderBoard, UnderBattleBoard, UNKNOWN3
    }

    public static class Messages
    {
        private static Dictionary<MessageType, Dialogue[][]> messages;
        
        /// <summary>
        /// savedata.message type starts at 1 at HumorInfo
        /// </summary>
        static Messages()
        {
            messages = new Dictionary<MessageType, Dialogue[][]>();
            messages[MessageType.ShanghaiInfo] = Messages.LoadMessages("ShanghaiInfo.xml");
            messages[MessageType.AliceInfo] = Messages.LoadMessages("AliceInfo.xml");
            messages[MessageType.ShanghaiRequestInfo] = Messages.LoadMessages("ShanghaiRequestInfo.xml");
            messages[MessageType.AliceRequestInfo] = Messages.LoadMessages("AliceRequestInfo.xml");
            messages[MessageType.HumorInfo] = Messages.LoadMessages("HumorInfo.xml");
            messages[MessageType.UNKNOWN1] = Messages.LoadMessages("UNKNOWN1.xml");
            messages[MessageType.UNKNOWN2] = Messages.LoadMessages("UNKNOWN2.xml");
            messages[MessageType.EirinCallInfo] = Messages.LoadMessages("EirinCallInfo.xml");
            messages[MessageType.RequestBoard] = Messages.LoadMessages("RequestBoard.xml");
            messages[MessageType.RequestBoardComplete] = Messages.LoadMessages("RequestBoardComplete.xml");
            messages[MessageType.GenBoard] = Messages.LoadMessages("GenBoard.xml");
            messages[MessageType.UniversityBoard] = Messages.LoadMessages("UniversityBoard.xml");
            messages[MessageType.EienBoard] = Messages.LoadMessages("EienBoard.xml");
            messages[MessageType.UnderBoard] = Messages.LoadMessages("UnderBoard.xml");
            messages[MessageType.UnderBattleBoard] = Messages.LoadMessages("UnderBattleBoard.xml");
            messages[MessageType.UNKNOWN3] = Messages.LoadMessages("UNKNOWN3.xml");
        }
        
        public static Dialogue[] GetMessage(MessageType msgType, int number) => messages[msgType][number];

        private static Dialogue[][] LoadMessages(string sourceFile)
        {
            var languageDoc = new XmlDocument();
            languageDoc.Load($"data/data/Messages/{sourceFile}");

            var text = languageDoc.SelectNodes("data/Message");
            var messageArray = new Dialogue[text.Count][];
            foreach (XmlNode message in text)
            {
                var index = int.Parse(message.Attributes["Index"].Value);
                var dialogues = message.ChildNodes;
                messageArray[index] = new Dialogue[dialogues.Count];
                var nodeNumber = 0;
                foreach (XmlNode dialogue in message)
                {
                    messageArray[index][nodeNumber++] = ShanghaiEXE.Translate(dialogue.Attributes["Key"].Value);
                }
            }

            return messageArray;
        }
    }
}