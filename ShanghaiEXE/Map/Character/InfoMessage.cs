using Messages;
using NSShanghaiEXE.InputOutput.Audio;
using NSEvent;
using NSGame;

namespace NSMap.Character
{
    public class InfoMessage
    {

        private readonly MyAudio myAudio;
        private readonly SaveData saveData;

        public InfoMessage(MyAudio s, SaveData save)
        {
            this.myAudio = s;
            this.saveData = save;
        }
        public EventManager GetMessage(MessageType command, int number)
        {
            var result = new EventManager(this.myAudio);
            var messages = AllMessages.GetMessage(command, number);
            if ((int)command < 8)
            {
                result.AddEvent(new OpenMassageWindow(this.myAudio, result));
            }
            for (int msgNum = 0; msgNum < messages.Length; msgNum += 1)
            {
                var msg = messages[msgNum];
                var commandMessage = new CommandMessage(this.myAudio, result, msg[0], msg[1], msg[2], msg.Face, msg.Face.Mono, this.saveData);
                result.AddEvent(commandMessage);
            }
            if ((int)command < 8)
            {
                result.AddEvent(new CloseMassageWindow(this.myAudio, result));
            }
            return result;
        }

        public EventManager GetMessage(int command, int number) => this.GetMessage((MessageType)command, number);
    }
}
