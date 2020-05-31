using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Loader.Extensions;

namespace UiC.Commands.Commands {
    public delegate T ConverterHandler<out T>(string str, TriggerBase trigger, string delimiter);

    public class ParameterDefinition<T> : IParameterDefinition<T> {
        public ParameterDefinition(string name, string shortName = "", string description = "", T defaultValue = default(T), bool isOptional = false,
                                ConverterHandler<T> converter = null, string delimiter = "!") {
            Name = name;
            ShortName = shortName;
            Description = description;
            DefaultValue = defaultValue;
            IsOptional = isOptional;
            Converter = converter;
            Delimiter = delimiter;

            if (!Equals(defaultValue, default(T)))
                IsOptional = true;
        }

        #region ICommandParameter<T> Members

        public ConverterHandler<T> Converter {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public string ShortName {
            get;
            private set;
        }

        public string Description {
            get;
            private set;
        }

        public string Delimiter
        {
            get;
            set;
        }

        /// <summary>
        /// A parameter is optional whenever the DefaultValue has been set
        /// </summary>
        public bool IsOptional {
            get;
            private set;
        }

        public T DefaultValue {
            get;
            private set;
        }

        public Type ValueType {
            get { return typeof(T); }
        }

        public object ConvertString(string value, TriggerBase trigger) {
            if (string.IsNullOrEmpty(value))
                return DefaultValue;

            if (Converter != null && trigger != null)
                return Converter(value, trigger, Delimiter);

            if (ValueType == typeof(string))
                return value;

            if (ValueType.HasInterface(typeof(IConvertible)))
                return Convert.ChangeType(value, typeof(T));

            return ValueType.IsEnum ? Enum.Parse(ValueType, value) : DefaultValue;
        }

        /// <summary>
        /// Create a Parameter instance with the right generic type
        /// </summary>
        /// <returns></returns>
        public IParameter CreateParameter() {
            return new Parameter<T>(this);
        }

        public string GetUsage() {
            var usage = Name != ShortName ? Name + "/" + ShortName : Name;

            if (!Equals(DefaultValue, default(T))) {
                usage += "=" + DefaultValue;
            }

            return !IsOptional ? usage : "[" + usage + "]";
        }

        #endregion

        public override string ToString() {
            return GetUsage();
        }
    }
}
