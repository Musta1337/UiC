using UiC.Commands.Commands.Matching;
using UiC.Commands.Commands.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;
using UiC.Core.Managers;

namespace UiC.Commands.Commands
{
    public static class ParametersConverter
    {
        public static ConverterHandler<Entity> PlayerConverter = (entry, trigger, delimiter) =>
        {
            Entity target;

            if (trigger is GameTrigger && (trigger as GameTrigger).Player != null)
                target = PlayerManager.Instance.FindPlayer((trigger as GameTrigger).Player, entry, delimiter);
            else
                target = PlayerManager.Instance.FindPlayer(entry, delimiter);


            if (target == null)
                throw new ConverterException(string.Format(CommandManager.PlayerNotFound, entry));

            return target;
        };

        public static ConverterHandler<Entity[]> PlayersConverter = (entry, trigger, delimiter) =>
        {
            var matching = new PlayerMatching(entry,
                trigger is GameTrigger ? (trigger as GameTrigger).Player : null, delimiter);

            return matching.FindMatchs();
        };
    }
}
