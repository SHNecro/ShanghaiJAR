using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements.Enums;
using MapEditor.Models.Elements.Terms;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MapEditor.Models.Elements
{
    public class TermObject : StringRepresentation
    {
        private TermBase instance;

        public TermBase Instance
        {
            get
            {
                return this.instance;
            }

            set
            {
                if (this.Instance != null)
                {
                    this.Instance.PropertyChanged -= this.OnInstancePropertyChanged;
                }

                this.SetValue(ref this.instance, value);
                this.Instance.PropertyChanged += this.OnInstancePropertyChanged;
            }
        }

        public TermCategoryOption Category
        {
            get
            {
                switch (this.Instance)
                {
                    case NoneTerm nt:
                        return TermCategoryOption.None;
                    case FlagTerm ft:
                        return TermCategoryOption.Flag;
                    case VariableTerm vt:
                        return TermCategoryOption.Variable;
                    case ChipTerm ct:
                        return TermCategoryOption.Chip;
                    default:
                        return TermCategoryOption.None;
                }
            }

            set
            {
                switch (value)
                {
                    case TermCategoryOption.None:
                        this.Instance = new NoneTerm();
                        break;
                    case TermCategoryOption.Flag:
                        if (!(this.Instance is FlagTerm))
                        {
                            this.Instance = new FlagTerm { Flag = 0 };
                        }
                        break;
                    case TermCategoryOption.Variable:
                        if (!(this.Instance is VariableTerm))
                        {
                            this.Instance = new VariableTerm { VariableLeft = 0, OperatorType = (int)VariableTermOperatorTypeNumber.Equals, VariableOrConstantRight = 0, IsVariable = false };
                        }
                        break;
                    case TermCategoryOption.Chip:
                        if (!(this.Instance is ChipTerm))
                        {
                            this.Instance = new ChipTerm { Chip = new Chip { ID = 1, CodeNumber = 0 } };
                        }
                        break;
                }
                this.OnPropertyChanged(nameof(this.Category));
                this.OnPropertyChanged(nameof(this.StringValue));
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        public virtual string Name => this.Instance.Name;

        public static TermObject FromString(string value)
        {
            var term = new TermObject();

            if (value.StartsWith("none", StringComparison.InvariantCulture))
            {
                term.Instance = new NoneTerm { StringValue = value };
            }
            else if (value.StartsWith("flag", StringComparison.InvariantCulture) || value.StartsWith("!flag", StringComparison.InvariantCulture))
            {
                term.Instance = new FlagTerm { StringValue = value };
            }
            else if (value.StartsWith("variable", StringComparison.InvariantCulture))
            {
                term.Instance = new VariableTerm { StringValue = value };
            }
            else if (value.StartsWith("havechip", StringComparison.InvariantCulture))
            {
                term.Instance = new ChipTerm { StringValue = value };
            }
            else
            {
                term.Instance = new NoneTerm { StringValue = value };
            }

            return term;
        }

        protected override string GetStringValue() => this.Instance.StringValue;

        protected override void SetStringValue(string value) => this.Instance = TermObject.FromString(value).Instance;

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors() => this.Instance?.Errors ?? new ObservableCollection<Tuple<StringRepresentation[], string>>();

        protected override string GetTypeName() => this.Instance?.TypeName;

        public override void ClearErrors() => this.Instance?.ClearErrors();

        private void OnInstancePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Instance));
            this.OnPropertyChanged(nameof(this.StringValue));
            this.OnPropertyChanged(nameof(this.Name));
        }
    }
}
