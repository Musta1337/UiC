using UiC.Commands.Commands.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;

namespace UiC.Commands.Commands.Commands.Patterns
{
    public abstract class TargetCommand : CommandBase
    {
        protected void AddTargetParameter( bool optional = false, string description = "Defined target")
        {
            AddParameter("target", "t", description, isOptional: optional, converter: ParametersConverter.PlayerConverter);
        }

        public Entity[] GetTargets(TriggerBase trigger)
        {
            Entity[] targets = null;
            if (trigger.IsArgumentDefined("target"))
                targets = trigger.Get<Entity[]>("target");

            else if (trigger is GameTrigger)
                targets = new[] { (trigger as GameTrigger).Player };

            if (targets == null)
                throw new Exception("Target is not defined");

            if (targets.Length == 0)
            {
                throw new Exception("No target found");
            }

            return targets;
        }

        public Entity GetTarget(TriggerBase trigger)
        {
            var targets = GetTargets(trigger);

            if (targets.Length > 1)
                throw new Exception("Only 1 target allowed");

            return targets[0];
        }
    }

    public abstract class TargetSubCommand : SubCommand
    {
        protected void AddTargetParameter(bool optional = false, string description = "Defined target")
        {
            AddParameter("target", "t", description, isOptional: optional, converter: ParametersConverter.PlayerConverter);
        }

        public Entity[] GetTargets(TriggerBase trigger)
        {
            Entity[] targets = null;
            if (trigger.IsArgumentDefined("target"))
                targets = trigger.Get<Entity[]>("target");
            else if (trigger is GameTrigger)
                targets = new[] { (trigger as GameTrigger).Player };

            if (targets == null)
                throw new Exception("Target is not defined");

            if (targets.Length == 0)
                throw new Exception("No target found");

            return targets;
        }

        public Entity GetTarget(TriggerBase trigger)
        {
            var targets = GetTargets(trigger);

            if (targets.Length > 1)
                throw new Exception("Only 1 target allowed");

            return targets[0];
        }
    }
}
