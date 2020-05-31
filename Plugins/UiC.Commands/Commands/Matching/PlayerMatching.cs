using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiC.Core.Managers;

namespace UiC.Commands.Commands.Matching
{
    public class PlayerMatching : BaseMatching<Entity>
    {
        public PlayerMatching(string pattern)
            : base(pattern)
        {
        }

        public PlayerMatching(string pattern, Entity caller, string delimiter)
            : base(pattern, caller, delimiter)
        {
        }

        protected override string GetName(Entity obj)
        {
            return obj.Name;
        }

        protected override IEnumerable<Entity> GetSource()
        {
            return PlayerManager.Instance.Players;
        }

        protected override Entity[] GetByNames(string name)
        {
            var character = PlayerManager.Instance.GetByNameLocal(name);

            return character != null ? new[] { character } : new Entity[0];
        }

        public override Entity[] FindMatchs()
        {
            if (Pattern == "*")
                return PlayerManager.Instance.Players.ToArray();

            if (Pattern.StartsWith("!"))
            {
                Pattern = Pattern.Remove(0, 1);
                var list = base.FindMatchs().ToList();
                list.Remove(Caller);
                return list.ToArray();
            }

            return base.FindMatchs();
        }

        protected override BaseCriteria<Entity> GetCriteria(string pattern)
        {
            throw new NotImplementedException();
        }
    }
}
