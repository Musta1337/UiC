using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UiC.Commands.Commands {

    public class CommandInfo {
        public CommandInfo() {

        }


        public CommandInfo(CommandBase command) {
            Aliases = command.Aliases.ToList();
            Name = command.GetType().Name;
            Description = command.Description;
        }


        public string Name {
            get;
            set;
        }

        public List<string> Aliases
        {
            get;
            set;
        }

        public int RequiredPower {
            get;
            set;
        }

        public bool RequireLogin
        {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public string Usage {
            get;
            set;
        }

    }
}
