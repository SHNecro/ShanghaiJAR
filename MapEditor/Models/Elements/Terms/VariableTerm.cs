using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Terms
{
    public class VariableTerm : TermBase
    {
        private int variableLeft;
        private int operatorType;
        private int variableOrConstantRight;
        private bool isVariable;

        public int VariableLeft
        {
            get
            {
                return this.variableLeft;
            }

            set
            {
                this.SetValue(ref this.variableLeft, value);
            }
        }
        public int OperatorType
        {
            get
            {
                return this.operatorType;
            }

            set
            {
                this.SetValue(ref this.operatorType, value);
            }
        }
        public int VariableOrConstantRight
        {
            get
            {
                return this.variableOrConstantRight;
            }

            set
            {
                this.SetValue(ref this.variableOrConstantRight, value);
            }
        }
        public bool IsVariable
        {
            get
            {
                return this.isVariable;
            }

            set
            {
                this.SetValue(ref this.isVariable, value);
            }
        }

        public override string Name
        {
            get
            {
                var op = new EnumDescriptionTypeConverter(typeof(VariableTermOperatorTypeNumber)).ConvertToString((VariableTermOperatorTypeNumber)this.OperatorType);
                var rhsString = this.IsVariable ? $"var[{this.VariableOrConstantRight}]" : this.VariableOrConstantRight.ToString();
                return $"var[{this.VariableLeft}] {op} {rhsString}";
            }
        }

        protected override string GetStringValue()
        {
            var isVariableString = this.IsVariable ? "True" : "False";
            return $"variable/{this.VariableLeft}/{isVariableString}/{(int)this.OperatorType}/{this.VariableOrConstantRight}";
        }

        protected override void SetStringValue(string value)
        {
            var newVariableLeft = default(int);
            var newOperatorType = default(int);
            var newVariableOrConstantRight = default(int);
            var newIsVariable = default(bool);

            var variableParams = value.Split('/');
            if (this.Validate(variableParams, $"Malformed variable term entry \"{value}\".", vp => vp.Length == 5))
            {
                newVariableLeft = this.ParseIntOrAddError(variableParams[1]);
                newOperatorType = this.ParseIntOrAddError(variableParams[3]);
                this.Validate(newOperatorType, () => this.OperatorType, i => $"Invalid operator {i} (0: ==, 1: <=, 2: >=, 3: <, 4: >, 5: !=)", o => o >= 0 && o <= 5);
                newVariableOrConstantRight = this.ParseIntOrAddError(variableParams[4]);
                newIsVariable = this.ParseBoolOrAddError(variableParams[2]);
            }

            this.VariableLeft = newVariableLeft;
            this.OperatorType = newOperatorType;
            this.VariableOrConstantRight = newVariableOrConstantRight;
            this.IsVariable = newIsVariable;
        }
    }
}
