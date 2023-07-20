using Common;
using NSShanghaiEXE.InputOutput.Audio;
using NSEvent;
using System;
using System.Collections.Generic;

namespace NSGame
{
    [Serializable]
    public class Mail
    {
        public string title = "";
        public string parson = "";
        public EventManager manager = new EventManager(null);
        public List<Dialogue> txt = new List<Dialogue>();
        public bool read;

        protected void AddTXT(Dialogue t)
        {
            this.txt.Add(t);
        }

        public EventManager MakeEvent(IAudioEngine s)
        {
            EventManager m = new EventManager(s);
            for (int index = 0; index < this.txt.Count; index += 1)
            {
                var dialogue = this.txt[index];
                m.AddEvent(new CommandMessage(s, m, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, dialogue.Face.Mono, dialogue.Face.Auto, null));
            }
            return m;
        }
    }
}
