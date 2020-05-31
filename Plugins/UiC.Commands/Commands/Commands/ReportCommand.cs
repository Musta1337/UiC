using InfinityScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UiC.Core;
using UiC.Core.Discord;
using UiC.Core.Extensions;
using UiC.Core.Managers;

namespace UiC.Commands.Commands {
    public class ReportCommand : CommandBase {

        public ReportCommand() {
            Aliases = new[] { "report" };
            Description = "Report a player";

            AddParameter<string>("player", "player", "Player reported");
            AddParameter<string>("reason", "reason", "Reason", isOptional: true);

        }

        public override void Execute(TriggerBase trigger) {
            string player = trigger.Get<string>("player");

            var playerObj = PlayerManager.Instance.FindPlayer(player, Delimiter);

            if (playerObj == null)
            {
                trigger.ReplyPM(CommandManager.PlayerNotFound, player);
                return;
            }

            string reason = trigger.Get<string>("reason");

            if (string.IsNullOrEmpty(reason))
            {
                reason = "No reason.";
            }

            if (playerObj == trigger.GetSource())
                trigger.ReplyPM("Can't report yourself, ^1noob.");

            WebhookManager.SendWebhookReport(playerObj, trigger.GetSource().Name, reason, Server.Hostname.RemoveColors(), "**Player report**", "UiC Report System", Color.Yellow);
            WebhookManager.SendWebhookReportPublic(playerObj, trigger.GetSource().Name, reason, Server.Hostname.RemoveColors(), "**Player report**", "UiC Report System", Color.Yellow);

            trigger.ReplyPM("Player ^3{0} ^7reported.", playerObj.Name);
        }

    }
}
