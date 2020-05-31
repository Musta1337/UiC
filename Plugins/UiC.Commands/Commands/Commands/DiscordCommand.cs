using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Commands.Commands.Commands
{
    public class DiscordCommand : CommandBase
    {

        public DiscordCommand()
        {
            Aliases = new[] { "discord", "" };
            Description = "Show every discord link";
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.ReplyPM($"Join UiC discord: https://discord.gg/RdwEmuq");
            trigger.ReplyPM($"Do !uic help for all commands");
        }

    }
}
