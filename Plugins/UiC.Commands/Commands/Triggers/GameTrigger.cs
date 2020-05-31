using InfinityScript;
using UiC.Commands;

namespace UiC.Commands.Commands.Triggers
{
    public class GameTrigger : TriggerBase {
        public GameTrigger(string args, Entity player)
            : base(args) {
            Player = player;
        }

        public override bool CanFormat => true;

        public Entity Player {
            get;
            protected set;
        }


        public override Entity GetSource() {
            return Player;
        }

        public override void Log() {

        }

    }
}
