using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MapEditor.Core
{
	public class StringRepresentation : ViewModelBase, ICloneable
	{
        private ObservableCollection<string> errors;

        public StringRepresentation()
		{
			this.errors = new ObservableCollection<string>();
		}

		public string StringValue
		{
			get
			{
				return this.HasErrors ? this.RawString : this.GetStringValue();
			}
			set
			{
				this.RawString = value;
				this.ClearErrors();

				try
				{
					this.SetStringValue(value);
				}
				catch (Exception e)
				{
					Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
					this.Errors.Add($"{e.GetType().ToString()}: {e.Message}");
				}

				this.OnPropertyChanged(string.Empty);
			}
		}

        public ObservableCollection<string> Errors => this.GetErrors();

		public bool HasErrors => this.Errors == null || this.Errors.Count != 0;

        public string TypeName => this.GetTypeName();

		public string RawString { get; set; }

        public virtual void ClearErrors()
        {
            this.errors.Clear();
        }

        protected virtual ObservableCollection<string> GetErrors()
        {
            return this.errors;
        }

        protected virtual string GetTypeName() => this.GetType().Name;

		protected virtual string GetStringValue()
		{
			return this.RawString;
		}

		protected virtual void SetStringValue(string value)
		{
			this.RawString = value;
        }

        protected int ParseIntOrAddError(string s, Func<int, bool> validateFunc = null, Func<int, string> errorFunc = null)
        {
            if (int.TryParse(s, out int parsed))
            {
                if (validateFunc == null || validateFunc(parsed))
                {
                    return parsed;
                }
                else
                {
                    var errorFuncOrDefault = errorFunc ?? (i => $"Invalid parameter \"{i}\"");
                    this.Errors.Add(errorFunc(parsed));
                    return -1;
                }
            }
            else
            {
                this.Errors.Add($"Failed to parse \"{s}\" as int");
                return -1;
            }
        }

        protected TEnum ParseEnumOrAddError<TEnum>(string s)
			where TEnum : struct
		{
			if (Enum.TryParse(s, out TEnum parsed) && Enum.IsDefined(parsed.GetType(), parsed))
			{
				return parsed;
			}
			else
			{
				this.Errors.Add($"Failed to parse \"{s}\" as {typeof(TEnum).Name}");
				return default(TEnum);
			}
        }

        protected bool ParseBoolOrAddError(string s)
        {
            if (s == "True")
            {
                return true;
            }
            else if (s == "False")
            {
                return false;
            }
            else
            {
                this.Errors.Add($"Failed to parse \"{s}\" as boolean.");
                return false;
            }
        }

        protected bool AddChildErrors(string location, IEnumerable<StringRepresentation> children)
		{
            foreach (var c in children)
            {
			    var prepend = location == null ? $"{c.TypeName}: " : $"{location}: ";
				foreach(var e in c.Errors)
				{
					this.Errors.Add($"{prepend}{e}");
				}

            }
			if (children.Any(c => c.Errors.Any()))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		protected bool Validate<TVar>(TVar value, string error, Func<TVar, bool> validateFunc)
		{
			if (validateFunc(value))
			{
				return true;
			}
			else
			{
				this.Errors.Add(error);
				return false;
			}
		}

        public object Clone()
        {
            var newInstance = (StringRepresentation)Activator.CreateInstance(this.GetType());
            newInstance.StringValue = this.StringValue;
            return newInstance;
        }
    }
}
