using System;
using System.Collections.Generic;
using System.Xml;

namespace MapEditor.Models.Elements
{
    public class MailDefinition
    {
        public string Sender { get; set; }
        public string Subject { get; set; }

        public string Name => $"{this.Sender}: {this.Subject}";
    }
}
