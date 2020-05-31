using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Commands.Commands {
    public abstract class SubCommand : CommandBase {
        public Type ParentCommandType {
            get;
            protected set;
        }

        public SubCommandContainer ParentCommand {
            get;
            private set;
        }

        public void SetParentCommand(SubCommandContainer command) {
            if (ParentCommand != null)
                throw new Exception("Parent command already set");

            ParentCommand = command;
        }

        public override string[] GetFullAliases() {
            return ParentCommand.Aliases.SelectMany(x => Aliases.Select(y => x + " " + y)).ToArray();
        }

        public override string ToString() {
            return GetType().Name;
        }
    }
}
