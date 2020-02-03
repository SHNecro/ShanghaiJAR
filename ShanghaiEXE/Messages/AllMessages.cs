using Common;
using System.Collections.Generic;

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
            messages[MessageType.ShanghaiInfo] = AllMessages.GetShanghaiInfo();
            messages[MessageType.AliceInfo] = AllMessages.GetAliceInfo();
            messages[MessageType.ShanghaiRequestInfo] = AllMessages.GetShanghaiRequestInfo();
            messages[MessageType.AliceRequestInfo] = AllMessages.GetAliceRequestInfo();
            messages[MessageType.HumorInfo] = AllMessages.GetHumorInfo();
            messages[MessageType.UNKNOWN1] = AllMessages.GetUNKNOWN1();
            messages[MessageType.UNKNOWN2] = AllMessages.GetUNKNOWN2();
            messages[MessageType.EirinCallInfo] = AllMessages.GetEirinCallInfo();
            messages[MessageType.RequestBoard] = AllMessages.GetRequestBoard();
            messages[MessageType.RequestBoardComplete] = AllMessages.GetRequestBoardComplete();
            messages[MessageType.GenBoard] = AllMessages.GetGenBoard();
            messages[MessageType.UniversityBoard] = AllMessages.GetUniversityBoard();
            messages[MessageType.EienBoard] = AllMessages.GetEienBoard();
            messages[MessageType.UnderBoard] = AllMessages.GetUnderBoard();
            messages[MessageType.UnderBattleBoard] = AllMessages.GetUnderBattleBoard();
            messages[MessageType.UNKNOWN3] = AllMessages.GetUNKNOWN3();
        }
        
        public static Dialogue[] GetMessage(MessageType msgType, int number) => messages[msgType][number];
    }
}