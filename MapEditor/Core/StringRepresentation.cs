using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MapEditor.Core
{
	public class StringRepresentation : ViewModelBase, ICloneable
	{
        private Collection<Tuple<Func<bool>, Func<string>>> validationRules;
        private ObservableCollection<Tuple<StringRepresentation[], string>> errors;

        private bool initializedByStringValue;

        public StringRepresentation()
		{
            this.validationRules = new Collection<Tuple<Func<bool>, Func<string>>>();
			this.errors = new ObservableCollection<Tuple<StringRepresentation[], string>>();
		}

		public string StringValue
		{
			get
			{
				return this.HasErrors ? this.RawString : this.GetStringValue();
			}
			set
			{
                this.initializedByStringValue = false;
                this.validationRules.Clear();
				this.RawString = value;
				this.ClearErrors();

				try
				{
					this.SetStringValue(value);
				}
				catch (Exception e)
				{
					Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
                    // More of a code error, but combine handling
					this.Errors.Add(Tuple.Create(new[] { this }, $"{e.GetType().ToString()}: {e.Message}"));
				}

				this.OnPropertyChanged(string.Empty);
                this.initializedByStringValue = true;
            }
		}

        public ObservableCollection<Tuple<StringRepresentation[], string>> Errors => this.GetErrors();

		public bool HasErrors => this.Errors == null || this.Errors.Count != 0;

        public string TypeName => this.GetTypeName();

		public string RawString { get; set; }

        public virtual void ClearErrors()
        {
            this.errors.Clear();
        }

        protected virtual ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
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

        protected int ParseIntOrAddError(string s, Func<int> getterFunc, Func<int, bool> validateFunc, Func<int, string> errorFunc)
        {
            if (getterFunc != null)
            {
                this.validationRules.Add(Tuple.Create(new Func<bool>(() => validateFunc(getterFunc())), new Func<string>(() => errorFunc(getterFunc()))));
            }

            if (int.TryParse(s, out int parsed))
            {
                if (validateFunc == null || validateFunc(parsed))
                {
                    return parsed;
                }
                else
                {
                    var errorFuncOrDefault = errorFunc ?? (i => $"Invalid parameter \"{i}\"");
                    this.Errors.Add(Tuple.Create(new[] { this }, errorFunc(parsed)));
                    return -1;
                }
            }
            else
            {
                this.Errors.Add(Tuple.Create(new[] { this }, $"Failed to parse \"{s}\" as int"));
                return -1;
            }
        }

        protected int ParseIntOrAddError(string s) => this.ParseIntOrAddError(s, null, null, null);
        protected int ParseIntOrAddError(string s, Func<int, bool> validateFunc, Func<int, string> errorFunc) => this.ParseIntOrAddError(s, null, validateFunc, errorFunc);

        protected long ParseLongOrAddError(string s, Func<long> getterFunc, Func<long, bool> validateFunc, Func<long, string> errorFunc)
        {
            if (getterFunc != null)
            {
                this.validationRules.Add(Tuple.Create(new Func<bool>(() => validateFunc(getterFunc())), new Func<string>(() => errorFunc(getterFunc()))));
            }

            if (long.TryParse(s, out long parsed))
            {
                if (validateFunc == null || validateFunc(parsed))
                {
                    return parsed;
                }
                else
                {
                    var errorFuncOrDefault = errorFunc ?? (i => $"Invalid parameter \"{i}\"");
                    this.Errors.Add(Tuple.Create(new[] { this }, errorFunc(parsed)));
                    return -1;
                }
            }
            else
            {
                this.Errors.Add(Tuple.Create(new[] { this }, $"Failed to parse \"{s}\" as long"));
                return -1;
            }
        }

        protected long ParseLongOrAddError(string s) => this.ParseLongOrAddError(s, null, null, null);
        protected long ParseLongOrAddError(string s, Func<long, bool> validateFunc, Func<long, string> errorFunc) => this.ParseLongOrAddError(s, null, validateFunc, errorFunc);

        protected TEnum ParseEnumOrAddError<TEnum>(string s)
			where TEnum : struct
        {
            if (Enum.TryParse(s, out TEnum parsed) && Enum.IsDefined(parsed.GetType(), parsed))
			{
				return parsed;
			}
			else
			{
				this.Errors.Add(Tuple.Create(new[] { this }, $"Failed to parse \"{s}\" as {typeof(TEnum).Name}"));
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
                this.Errors.Add(Tuple.Create(new[] { this }, $"Failed to parse \"{s}\" as boolean."));
                return false;
            }
        }

        protected bool Validate<TVar>(TVar value, Func<TVar> getterFunc, Func<TVar, string> errorFunc, Func<TVar, bool> validateFunc)
        {
            if (getterFunc != null)
            {
                this.validationRules.Add(Tuple.Create(new Func<bool>(() => validateFunc(getterFunc())), new Func<string>(() => errorFunc(getterFunc()))));
            }

            if (validateFunc(value))
            {
                return true;
            }
            else
            {
                this.Errors.Add(Tuple.Create(new[] { this }, errorFunc(value)));
                return false;
            }
        }

        protected bool Validate<TVar>(TVar value, Func<TVar> getterFunc, string error, Func<TVar, bool> validateFunc)
            => this.Validate(value, getterFunc, (s) => error, validateFunc);

        protected bool Validate<TVar>(TVar value, Func<TVar, string> errorFunc, Func<TVar, bool> validateFunc)
            => this.Validate(value, null, errorFunc, validateFunc);

        protected bool Validate<TVar>(TVar value, string error, Func<TVar, bool> validateFunc)
            => this.Validate(value, (s) => error, validateFunc);

        protected ObservableCollection<Tuple<StringRepresentation[], string>> UpdateChildErrorStack(StringRepresentation child)
        {
            if (child == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }

            return new ObservableCollection<Tuple<StringRepresentation[], string>>(child.Errors.Select(tup => Tuple.Create(tup.Item1.Concat(new[] { this }).ToArray(), tup.Item2)));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (this.initializedByStringValue)
            {
                this.ApplyValidation();
            }
        }

        private void ApplyValidation()
        {
            this.ClearErrors();
            foreach (var rule in this.validationRules)
            {
                if (!rule.Item1.Invoke())
                {
                    this.Errors.Add(Tuple.Create(new[] { this }, rule.Item2.Invoke()));
                }
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
