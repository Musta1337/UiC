using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;
using UiC.Core.Discord;

namespace UIC.AntiCheat
{
    public static class AntiForceClass
    {
        private static bool CustomClassAllowed = false;

        private static List<string> WeaponsRestricted = new List<string>();
        private static List<string> AttachementsRestricted = new List<string>();
        private static List<string> PerksRestricted = new List<string>();

        private static List<string> WeaponsAllowed = new List<string>();
        private static List<string> AttachmentsAllowed = new List<string>();
        private static List<string> PerksAllowed = new List<string>();

        private static string Players2Directory = @"players2\";
        public static string DsrName;



        public static void Initialize()
        {
            if (!File.Exists(Players2Directory + DsrName))
                return;

            DsrName = GSCFunctions.GetDvar("sv_current_dsr");

            string[] DSR = File.ReadAllLines(Players2Directory + DsrName);

            var customClassLine = DSR.FirstOrDefault(x => x.Contains("allowCustomClasses"));
            if (customClassLine != null)
            {
                var value = customClassLine.Split(' ')[2].Replace("\"", "");
                if (value == "1")
                    CustomClassAllowed = true;
                else
                    CustomClassAllowed = false;
            }

            #region Restricted Weapons
            foreach (var weaponRestricted in DSR.Where(x => x.StartsWith("gameOpt commonOption.weaponRestricted.")))
            {
                var weapon = weaponRestricted.Split('.')[2].Split(' ')[0];

                if (weaponRestricted.Split(' ')[2].Contains("1"))
                    WeaponsRestricted.Add(weapon);
            }
            #endregion

            #region Restricted Attachments
            foreach (var attachmentRestricted in DSR.Where(x => x.StartsWith("gameOpt commonOption.attachmentRestricted.")))
            {
                var attach = attachmentRestricted.Split('.')[2].Split(' ')[0];

                if (attachmentRestricted.Split(' ')[2].Contains("1"))
                    AttachementsRestricted.Add(attach);


            }
            #endregion

            #region Restricted Perks
            foreach (var perkRestricted in DSR.Where(x => x.StartsWith("gameOpt commonOption.perkRestricted.")))
            {
                var weapon = perkRestricted.Split('.')[2].Split(' ')[0];

                if (perkRestricted.Split(' ')[2].Contains("1"))
                    PerksRestricted.Add(weapon);


            }
            #endregion

            #region Allowed
            foreach (var allowed in DSR.Where(x => x.StartsWith("gameOpt defaultClasses.")))
            {
                if (allowed.Contains("class.perks"))
                {
                    var perk = allowed.Split(' ')[2].Replace("\"", "");

                    PerksAllowed.Add(perk);
                }
                else if (allowed.Contains("class.weaponSetups"))
                {
                    if (allowed.Contains("attachment"))
                    {
                        var attachment = allowed.Split(' ')[2].Replace("\"", "");

                        if (attachment != "none")
                        {
                            AttachmentsAllowed.Add(attachment);
                        }
                    }
                    else if (allowed.Contains("weapon"))
                    {
                        var weap = allowed.Split(' ')[2].Replace("\"", "");

                        if (weap != "none")
                        {
                            WeaponsAllowed.Add(weap);
                        }
                    }
                }
            }
            #endregion

            Log.Write(LogLevel.Info, "Initialize End.");

            Log.Write(LogLevel.Info, "Allowed Weapons: " + WeaponsAllowed.Count);

        }

        public static void CheckClass(Entity player)
        {
            var gametype = GSCFunctions.GetDvar("g_gametype");

            if (gametype == "gun" || gametype == "infect")
                return;

            if (player.CurrentWeapon == "none")
                return;

            if (WeaponsAllowed.Count == 0)
                return;

            var weapon = GetWeapon(player.CurrentWeapon);

            if (!WeaponsAllowed.Contains(weapon) && (WeaponsRestricted.Contains(weapon) || !CustomClassAllowed))
            {
                UICAntiCheat.AntiCheatBanClient(player, "Force Class Weapon");
                return;
            }


            foreach (var attachement in GetWeaponAttachment(player.CurrentWeapon))
            {
                if (!AttachmentsAllowed.Contains(attachement) && AttachementsRestricted.Contains(attachement))
                {
                    UICAntiCheat.AntiCheatBanClient(player, "Force Class Attachement");
                    return;
                }
            }

            foreach (var perk in PerksRestricted)
            {
                if (PerksAllowed.Contains(perk))
                    continue;

                if (HasPerk(player, perk))
                {
                        UICAntiCheat.AntiCheatBanClient(player, "Force Class Perk");
                    return;
                }
            }

        }

        private static string GetWeapon(string weapon)
        {
            if (weapon.StartsWith("iw5"))
            {
                var weaponAttachments = weapon.Split('_');
                var weaponName = weaponAttachments[0] + "_" + weaponAttachments[1];

                return weaponName;
            }
            else if (weapon.EndsWith("_mp"))
            {
                var weaponAttachments = weapon.Split('_');
                return weaponAttachments[0];
            }
            return weapon;
        }

        private static string[] GetWeaponAttachment(string weapon)
        {
            var attachments = new List<string>();

            if (weapon.StartsWith("iw5"))
            {
                var weaponAttachments = weapon.Split('_').Where(x => x != "mp").ToArray();

                if (weaponAttachments.Length > 2)
                {
                    attachments.Add(weaponAttachments[2]);

                    if (weaponAttachments.Length > 3)
                    {
                        attachments.Add(weaponAttachments[3]);
                    }
                }
            }

            return attachments.ToArray();
        }

        public static bool IsSniper(string weapon)
        {
            return weapon.StartsWith("iw5_l96a1") || weapon.StartsWith("iw5_rsass") || weapon.StartsWith("iw5_msr") || weapon.StartsWith("iw5_barrett") || weapon.StartsWith("iw5_dragunov") || weapon.StartsWith("iw5_as50");
        }

        private static bool HasPerk(Entity e, string perk)
        {
            return e.HasPerk(perk);
        }

    }

}
