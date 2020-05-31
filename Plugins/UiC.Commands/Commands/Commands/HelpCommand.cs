using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UiC.Commands;
using UiC.Commands.Commands;
using UiC.Commands.Commands.Triggers;
using UiC.Core.Enums;

namespace AlpaS.Commands.Commands
{
    public class HelpCommand : CommandBase
    {
        public HelpCommand()
        {
            Aliases = new[] { "help", "?" };
            Description = "List all available commands";
        }

        public override void Execute(TriggerBase trigger)
        {
            var commands = CommandManager.Instance.AvailableCommands.OrderBy(x => x.Aliases[0]).Select(w => w.Aliases[0]);

            trigger.ReplyPM("Commands available: ", commands.FirstOrDefault());

            Timer timer = new Timer(500) {
                AutoReset = true
            };

            timer.Start();

            int i = 1;

            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                if (++i > commands.Count())
                {
                    timer.Stop();
                    return;
                }

                trigger.ReplyPM(commands.ToList()[i]);
            };

        }
    }
}
