using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Commands.Commands {
    public interface ICommandsUser {
        List<KeyValuePair<string, Exception>> CommandsErrors {
            get;
        }
    }
}
