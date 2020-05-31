using InfinityScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UiC.Core.Discord.Objects;
using UiC.Core.Extensions;
using UiC.Core.Managers;
using UiC.Core.Misc;
using UiC.Core.Threading;

namespace UiC.Core.Discord
{
    public static class WebhookManager
    {

        //
        public static void SendMessageLog(string playerName, string message, string server, string title, string footer, Color color)
        {
            var webhook = new Webhook("https://discordapp.com/api/webhooks/467126640041000970/Jvq96CHExzyDqH1Nu-VmMdUFWtETLAEjBuasHZdCyVCM9fKKaJel0whPzvqS_YFKNv7b") {
                Content = $"{server} | {playerName}: {message}"
            };

            WebRequestManager.SendWebhook(webhook);
        }

        public static void SendWebhook(string playerName, string reason, string server, string title, string footer, Color color)
        {
            var webhook = new Webhook("https://discordapp.com/api/webhooks/465333437386194945/Vp3KRI8JvdG3cW5zEw3GY0r-vo9-H6Rucnq593NlThlKpxJTQQ1fDasx822214Q8Wj_M") {
                Embeds = new List<Embed>() {
                        new Embed() {
                            Title = "",
                            Description = title,
                            Color = color.ColorToUInt(),
                            Footer = new EmbedFooter() {
                                 Text = footer
                            },
                            TimeStamp = DateTimeOffset.Now,
                            Fields = new List<EmbedField>() {
                                  new EmbedField() {
                                       Name = "Player", Value = playerName, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Reason", Value = reason, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Server", Value = server, Inline = false
                                  }
                             }
                        }
                   }
            };

            WebRequestManager.SendWebhook(webhook);
        }

        public static void SendWebhookSuspect(Entity player, string reason, string server, string title, string footer, string dsr, Color color)
        {
            XnAddr xnaddr = new XnAddr(player, player.EntRef);

            var webhook = new Webhook("https://discordapp.com/api/webhooks/466068570221314049/pWCIvZ3A6_iF7LGcNNJzDugqCwqDuFCD6v0mra8dX1cRCuIIggZFJH2mtJVTnXdgE5mY") {
                Embeds = new List<Embed>() {
                        new Embed() {
                            Title = "",
                            Description = title,
                            Color = color.ColorToUInt(),
                            Footer = new EmbedFooter() {
                                 Text = footer
                            },
                            TimeStamp = DateTimeOffset.Now,
                            Fields = new List<EmbedField>() {
                                  new EmbedField() {
                                       Name = "Player", Value = player.Name, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Suspected by", Value = "AntiCheat", Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "XnAddress", Value = xnaddr.Value, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Hwid", Value = player.HWID, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Guid", Value = player.GUID.ToString(), Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "IP", Value = player.IP.Address.ToString(), Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Weapon", Value = player.CurrentWeapon, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Reason", Value = reason, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Server", Value = server, Inline = false
                                  }
                             }
                        }
                   }
            };
            WebRequestManager.SendWebhook(webhook);
        }

        public static void SendWebhookReport(Entity player, string reporter, string reason, string server, string title, string footer, Color color)
        {

            XnAddr xnaddr = new XnAddr(player, player.EntRef);

            var webhook = new Webhook("https://discordapp.com/api/webhooks/466068570221314049/pWCIvZ3A6_iF7LGcNNJzDugqCwqDuFCD6v0mra8dX1cRCuIIggZFJH2mtJVTnXdgE5mY") {
                Embeds = new List<Embed>() {
                        new Embed() {
                            Title = "",
                            Description = title,
                            Color = color.ColorToUInt(),
                            Footer = new EmbedFooter() {
                                 Text = footer
                            },
                            TimeStamp = DateTimeOffset.Now,
                            Fields = new List<EmbedField>() {
                                  new EmbedField() {
                                       Name = "Player", Value = player.Name, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Suspected by", Value = reporter, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "XnAddress", Value = xnaddr.Value, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Hwid", Value = player.HWID, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Guid", Value = player.GUID.ToString(), Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "IP", Value = player.IP.Address.ToString(), Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Weapon", Value = player.CurrentWeapon, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Reason", Value = reason, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Server", Value = server, Inline = false
                                  }
                             }
                        }
                   }
            };
            WebRequestManager.SendWebhook(webhook);
        }

        public static void SendWebhookReportPublic(Entity player, string reporter, string reason, string server, string title, string footer, Color color)
        {
            var webhook = new Webhook("https://discordapp.com/api/webhooks/465954766149648384/Z8Ru1dTnx4-VArysRGLsTFSaeak-y-9_u5VGW2mQtWKDTyAUtDgt3Yky8QiDibBgW2dN") {
                Embeds = new List<Embed>() {
                        new Embed() {
                            Title = "",
                            Description = title,
                            Color = color.ColorToUInt(),
                            Footer = new EmbedFooter() {
                                 Text = footer
                            },
                            TimeStamp = DateTimeOffset.Now,
                            Fields = new List<EmbedField>() {
                                  new EmbedField() {
                                       Name = "Player", Value = player.Name, Inline = true
                                  },
                                  new EmbedField() {
                                       Name = "Suspected by", Value = reporter, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Reason", Value = reason, Inline = true
                                  },
                                  new EmbedField() {
                                      Name = "Server", Value = server, Inline = false
                                  }
                             }
                        }
                   }
            };

            WebRequestManager.SendWebhook(webhook);
        }


    }
}
