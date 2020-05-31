using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiC.Core.Reflection;

namespace UiC.Core.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {

        public List<Entity> Players { get; set; } = new List<Entity>();

        public Entity FindPlayer(Entity trigger, string arg, string delimiter)
        {
            Console.WriteLine($"{trigger.Name} {arg} {delimiter}");
            if (string.IsNullOrEmpty(arg))
            {
                return trigger;
            }

            switch (delimiter)
            {
                case "!":
                    return GetByNameLocal(arg);
                case "@":
                    return GetByIdLocal(arg);
                default:
                    return null;
            }
        }

        public Entity FindPlayer(string arg, string delimiter)
        {
            switch (delimiter)
            {
                case "!":
                    return GetByNameLocal(arg);
                case "@":
                    return GetByIdLocal(arg);
                default:
                    return null;
            }
        }


        public Entity GetByIdLocal(string id)
        {
            return Players.FirstOrDefault(x => x.EntRef.ToString() == id);
        }

        public Entity GetByNameLocal(string name)
        {
            return Players.FirstOrDefault(x => x.Name.ToLower().Contains(name.ToLower()));
        }


    }
}
