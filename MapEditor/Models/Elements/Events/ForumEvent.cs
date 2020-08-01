using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class ForumEvent : EventBase
    {
        private int forumNumber;

        public int ForumNumber
        {
            get { return this.forumNumber; }
            set { this.SetValue(ref this.forumNumber, value); }
        }

        public override string Info => "Opens a BBS.";

        public override string Name
        {
            get
            {
                var forumString = new EnumDescriptionTypeConverter(typeof(ForumTypeNumber)).ConvertToString((ForumTypeNumber)this.ForumNumber);
                return $"Open BBS: {forumString}";
            }
        }

        protected override string GetStringValue()
        {
            return $"Forum:{this.ForumNumber}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed BBS event \"{value}\".", e => e.Length == 2 && e[0] == "Forum"))
            {
                return;
            }

            var newForumNumber = this.ParseIntOrAddError(entries[1]);
            this.ParseEnumOrAddError<ForumTypeNumber>(entries[1]);

            this.ForumNumber = newForumNumber;
        }
    }
}
