using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EditValueEvent : EventBase
    {
        private int index;
        private bool isVariable;
        private int operation;
        private int referenceType;
        private string referenceData;

        public int Index
        {
            get
            {
                return this.index;
            }

            set
            {
                this.SetValue(ref this.index, value);
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

        public int Operation
        {
            get
            {
                return this.operation;
            }

            set
            {
                this.SetValue(ref this.operation, value);
            }
        }

        public int ReferenceType
        {
            get
            {
                return this.referenceType;
            }

            set
            {
                var editValueReferenceType = (EditValueReferenceTypeNumber)this.ReferenceType;
                var newEditValueReferenceType = (EditValueReferenceTypeNumber)value;
                if (editValueReferenceType != newEditValueReferenceType)
                {
                    if (newEditValueReferenceType == EditValueReferenceTypeNumber.Value)
                    {
                        this.referenceData = "0";
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.Reference
                        && editValueReferenceType != EditValueReferenceTypeNumber.ReferenceReference)
                    {
                        this.referenceData = "0";
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.ReferenceReference
                        && editValueReferenceType != EditValueReferenceTypeNumber.Reference)
                    {
                        this.referenceData = "0";
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.Random)
                    {
                        this.referenceData = "0/5";
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.PlayerValue)
                    {
                        this.referenceData = "0";
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.Position)
                    {
                        this.referenceData = "0";
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.Angle)
                    {
                        this.referenceData = string.Empty;
                    }
                    else if (newEditValueReferenceType == EditValueReferenceTypeNumber.Answer)
                    {
                        this.referenceData = string.Empty;
                    }
                }

                this.SetValue(ref this.referenceType, value);
            }
        }

        public string ReferenceData
        {
            get
            {
                return this.referenceData;
            }

            set
            {
                this.SetValue(ref this.referenceData, value);
                this.OnPropertyChanged(nameof(this.ReferenceDataMin));
                this.OnPropertyChanged(nameof(this.ReferenceDataMax));
            }
        }
        public int ReferenceDataMin
        {
            get
            {
                if ((EditValueReferenceTypeNumber)this.ReferenceType != EditValueReferenceTypeNumber.Random)
                {
                    return 0;
                }
                return int.Parse(this.ReferenceData.Split('/')[0]);
            }

            set
            {
                this.ReferenceData = $"{value}/{this.ReferenceData.Split('/')[1]}";
            }
        }
        public int ReferenceDataMax
        {
            get
            {
                if ((EditValueReferenceTypeNumber)this.ReferenceType != EditValueReferenceTypeNumber.Random)
                {
                    return 0;
                }
                return int.Parse(this.ReferenceData.Split('/')[1]);
            }

            set
            {
                this.ReferenceData = $"{this.ReferenceData.Split('/')[0]}/{value}";
            }
        }

        public override string Info => "Sets or performs an operation on a variable with a value, variable, or other stored value.";

        public override string Name
        {
            get
            {
                var targetString = this.IsVariable ? $"var[var[{this.Index}]]" : $"var[{this.Index}]";
                var operationString = new EnumDescriptionTypeConverter(typeof(EditValueOperatorTypeNumber)).ConvertToString((EditValueOperatorTypeNumber)this.Operation);
                var referenceString = default(string);
                var editValueReferenceType = (EditValueReferenceTypeNumber)this.ReferenceType;
                if (editValueReferenceType == EditValueReferenceTypeNumber.Value)
                {
                    referenceString = $"{this.ReferenceData}";
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.Reference)
                {
                    referenceString = $"var[{this.ReferenceData}]";
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.ReferenceReference)
                {
                    referenceString = $"var[var[{this.ReferenceData}]]";
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.Random)
                {
                    var randString = string.Join(", ", this.ReferenceData.Split('/'));
                    referenceString = $"rand({randString})";
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.PlayerValue)
                {
                    referenceString = new EnumDescriptionTypeConverter(typeof(EditValuePlayerValueTypeNumber)).ConvertToString((EditValuePlayerValueTypeNumber)(int.Parse(this.ReferenceData)));
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.Position)
                {
                    var positionString = new EnumDescriptionTypeConverter(typeof(EditValuePlayerPositionTypeNumber)).ConvertToString((EditValuePlayerPositionTypeNumber)(int.Parse(this.ReferenceData)));
                    referenceString = $"Player {positionString}";
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.Angle)
                {
                    referenceString = "Player Angle";
                }
                else if (editValueReferenceType == EditValueReferenceTypeNumber.Answer)
                {
                    referenceString = "Question Answer";
                }

                return $"{targetString} {operationString} {referenceString}";
            }
        }

        protected override string GetStringValue()
        {
            var isVariableString = this.IsVariable ? "True" : "False";
            return $"editValue:{this.Index}:{isVariableString}:{this.Operation}:{this.ReferenceType}:{this.ReferenceData}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, "Malformed edit value event.", e => e.Length == 6))
            {
                return;
            }

            var newIndex = this.ParseIntOrAddError(entries[1]);
            var newIsVariable = this.ParseBoolOrAddError(entries[2]);
            var newOperation = this.ParseIntOrAddError(entries[3]);
            this.ParseEnumOrAddError<EditValueOperatorTypeNumber>(entries[3]);

            var newReferenceType = this.ParseIntOrAddError(entries[4]);
            var editValueReferenceType = this.ParseEnumOrAddError<EditValueReferenceTypeNumber>(entries[4]);

            var newReferenceData = entries[5];
            if (editValueReferenceType == EditValueReferenceTypeNumber.PlayerValue)
            {
                this.ParseEnumOrAddError<EditValuePlayerValueTypeNumber>(entries[5]);
            }
            else if (editValueReferenceType == EditValueReferenceTypeNumber.Position)
            {
                this.ParseEnumOrAddError<EditValuePlayerPositionTypeNumber>(entries[5]);
            }

            this.Index = newIndex;
            this.IsVariable = newIsVariable;
            this.Operation = newOperation;
            this.ReferenceType = newReferenceType;
            this.ReferenceData = newReferenceData;
        }
    }
}
