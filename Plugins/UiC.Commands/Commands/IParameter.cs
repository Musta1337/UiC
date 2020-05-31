using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Commands.Commands {
    public interface IParameter {
        IParameterDefinition Definition {
            get;
        }

        object Value {
            get;
        }

        bool IsDefined {
            get;
        }

        void SetValue(string str, TriggerBase trigger);
        void SetDefaultValue(TriggerBase trigger);
    }
}
