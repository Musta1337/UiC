using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanSystem.IS1.Extensions
{
    public static class EntityExtensions
    {
        public static int GetEntityNumber(this Entity player)
        {
            return player.Call<int>("getentitynumber");
        }

        public static void Kick(this Entity player, string reason)
        {
            Utilities.ExecuteCommand("dropclient " + player.GetEntityNumber() + " \"" + reason + "\"");
        }
    }
}
