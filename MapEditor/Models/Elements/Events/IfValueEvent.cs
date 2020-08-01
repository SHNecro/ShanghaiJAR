using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class IfValueEvent : EventBase
    {
        private int variableLeft;
        private int operatorType;
        private int variableOrConstantRight;
        private bool isVariable;
        private int statementID;

        public int VariableLeft
        {
            get { return this.variableLeft; }
            set { this.SetValue(ref this.variableLeft, value); }
        }

        public int OperatorType
        {
            get { return this.operatorType; }
            set { this.SetValue(ref this.operatorType, value); }
        }

        public int VariableOrConstantRight
        {
            get { return this.variableOrConstantRight; }
            set { this.SetValue(ref this.variableOrConstantRight, value); }
        }

        public bool IsVariable
        {
            get { return this.isVariable; }
            set { this.SetValue(ref this.isVariable, value); }
        }

        public int StatementID
        {
            get { return this.statementID; }
            set { this.SetValue(ref this.statementID, value); }
        }

        public override string Info => "Executes if the specified variable satisfies a condition.";

        public override string Name
        {
            get
            {
                var operatorString = new EnumDescriptionTypeConverter(typeof(IfValueOperatorTypeNumber)).ConvertToString((IfValueOperatorTypeNumber)this.OperatorType);
                var variableOrConstantRightString = this.IsVariable ? $"var[{this.VariableOrConstantRight}]" : $"{this.VariableOrConstantRight}";
                return $"If {this.StatementID}: var[{this.VariableLeft}] {operatorString} {variableOrConstantRightString}";
            }
        }

        protected override string GetStringValue()
        {
            return $"ifValue:{this.VariableLeft}:{this.IsVariable}:{this.VariableOrConstantRight}:{this.OperatorType}:{this.StatementID}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed if value event \"{value}\".", e => e.Length == 6 && e[0] == "ifValue"))
            {
                return;
            }

            var newVariableLeft = this.ParseIntOrAddError(entries[1], () => this.VariableLeft, vl => vl >= 0, vl => $"Invalid target variable {vl} (>= 0)");

            var newIsVariable = this.ParseBoolOrAddError(entries[2]);

            var newVariableOrConstantRight = this.ParseIntOrAddError(entries[3], () => this.VariableOrConstantRight, vocr => !newIsVariable || vocr >= 0, vocr => $"Invalid comparison variable {vocr} (>= 0)");

            var newOperatorType = this.ParseIntOrAddError(entries[4]);
            this.ParseEnumOrAddError<IfValueOperatorTypeNumber>(entries[4]);

            var newStatementID = this.ParseIntOrAddError(entries[5]);

            this.VariableLeft = newVariableLeft;
            this.OperatorType = newOperatorType;
            this.VariableOrConstantRight = newVariableOrConstantRight;
            this.IsVariable = newIsVariable;
            this.StatementID = newStatementID;
        }
    }
}
