using Data;
using NSShanghaiEXE.InputOutput.Audio;
using NSEvent;
using NSGame;
using Common;

namespace NSMap.Character
{
    public class InfoMessage
    {

        private readonly IAudioEngine IAudioEngine;
        private readonly SaveData saveData;

        public InfoMessage(IAudioEngine s, SaveData save)
        {
            this.IAudioEngine = s;
            this.saveData = save;
        }
        public EventManager GetMessage(MessageType command, int number)
        {
            var result = new EventManager(this.IAudioEngine);
            var messages = Messages.GetMessage(command, number);
            if ((int)command < 8)
            {
                result.AddEvent(new OpenMassageWindow(this.IAudioEngine, result));
            }
            for (int msgNum = 0; msgNum < messages.Length; msgNum += 1)
            {
                var msg = messages[msgNum];
                var commandMessage = new CommandMessage(this.IAudioEngine, result, msg[0], msg[1], msg[2], msg.Face, msg.Face.Mono, msg.Face.Auto, this.saveData);
                result.AddEvent(commandMessage);
            }
            if ((int)command < 8)
            {
                result.AddEvent(new CloseMassageWindow(this.IAudioEngine, result));
            }
            return result;
        }

        public EventManager GetMessage(int command, int number) => this.GetMessage((MessageType)command, number);
    }
}
