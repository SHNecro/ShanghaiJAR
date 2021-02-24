using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EffectEndEvent : EventBase
    {
        public override string Info => @"Ends all effects of these types on the map:
AliceJump
ItemGet
ShanghaiIN
AlicePhone
AlicePowder
AlicePowderLeft
FightShanhaiBack
FightMarisa
SlashYorihime
AliceBed
ROMHead
DruidAttack";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.EffectEnd);

        protected override string GetStringValue()
        {
            return "effectEnd:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed effect end event \"{value}\".", v => v == "effectEnd:");
        }
    }
}
