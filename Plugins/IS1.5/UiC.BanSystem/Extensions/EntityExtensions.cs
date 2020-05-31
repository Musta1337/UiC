using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanSystem.IS1_5.Extensions
{
    public static class EntityExtensions
    {
        public static void Kick(this Entity player, string reason)
        {
            Utilities.ExecuteCommand("dropclient " + player.GetEntityNumber() + " \"" + reason + "\"");
        }
    }
}
