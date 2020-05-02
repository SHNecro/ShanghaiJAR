using Common;
using NSGame;
using System.Collections.Generic;
using System.Xml;

namespace Messages
{
    public enum MessageType
    {
        ShanghaiInfo = 0, AliceInfo, ShanghaiRequestInfo, AliceRequestInfo, HumorInfo,
        UNKNOWN1, UNKNOWN2, EirinCallInfo, RequestBoard, RequestBoardComplete, GenBoard,
        UniversityBoard, EienBoard, UnderBoard, UnderBattleBoard, UNKNOWN3
    }

    public static partial class AllMessages
    {
        private static Dictionary<MessageType, Dialogue[][]> messages;
        
        /// <summary>
        /// savedata.message type starts at 1 at HumorInfo
        /// </summary>
        static AllMessages()
        {
            messages = new Dictionary<MessageType, Dialogue[][]>();
            messages[MessageType.ShanghaiInfo] = AllMessages.LoadMessages("ShanghaiInfo.xml");
            messages[MessageType.AliceInfo] = AllMessages.LoadMessages("AliceInfo.xml");
            messages[MessageType.ShanghaiRequestInfo] = AllMessages.LoadMessages("ShanghaiRequestInfo.xml");
            messages[MessageType.AliceRequestInfo] = AllMessages.LoadMessages("AliceRequestInfo.xml");
            messages[MessageType.HumorInfo] = AllMessages.LoadMessages("HumorInfo.xml");
            messages[MessageType.UNKNOWN1] = AllMessages.LoadMessages("UNKNOWN1.xml");
            messages[MessageType.UNKNOWN2] = AllMessages.LoadMessages("UNKNOWN2.xml");
            messages[MessageType.EirinCallInfo] = AllMessages.LoadMessages("EirinCallInfo.xml");
            messages[MessageType.RequestBoard] = AllMessages.LoadMessages("RequestBoard.xml");
            messages[MessageType.RequestBoardComplete] = AllMessages.LoadMessages("RequestBoardComplete.xml");
            messages[MessageType.GenBoard] = AllMessages.LoadMessages("GenBoard.xml");
            messages[MessageType.UniversityBoard] = AllMessages.LoadMessages("UniversityBoard.xml");
            messages[MessageType.EienBoard] = AllMessages.LoadMessages("EienBoard.xml");
            messages[MessageType.UnderBoard] = AllMessages.LoadMessages("UnderBoard.xml");
            messages[MessageType.UnderBattleBoard] = AllMessages.LoadMessages("UnderBattleBoard.xml");
            messages[MessageType.UNKNOWN3] = AllMessages.LoadMessages("UNKNOWN3.xml");
        }
        
        public static Dialogue[] GetMessage(MessageType msgType, int number) => messages[msgType][number];

        private static Dialogue[][] LoadMessages(string sourceFile)
        {
            var languageDoc = new XmlDocument();
            languageDoc.Load($"language/data/{sourceFile}");

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