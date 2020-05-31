using InfinityScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using UiC.Core;
using UiC.Core.Discord;
using UiC.Core.Discord.Objects;
using UiC.Core.Extensions;
using UiC.Core.Threading;
using UiC.Core.Timers;
using UiC.Loader;

namespace UIC.AntiCheat
{
    public class AntiNoRecoil
    {
        private int countSuspect = 0;
        private List<float> m_recoil = new List<float>();
        private Entity obj;
        public bool stopCheck = false;

        public AntiNoRecoil(Entity obj)
        {
            this.obj = obj;

        }

        public void AutoCheck()
        {
            if (!IfSniper(obj.CurrentWeapon))
                return;

            Check(0);

            UiC_Loader.Instance.AfterDelay(30, () =>
            {
                Check(30);
            });

            UiC_Loader.Instance.AfterDelay(100, () =>
            {
                Check(100);
            });

        }


        public float Check(int delay)
        {
            if (stopCheck)
                return 1;

            Vector3 angles = GetPlayerAngles(obj);
            float recoil = (float)angles.Z;

            m_recoil.Add(recoil);

            if (delay == 100)
                DoWeBan();

            return recoil;
        }

        public Vector3 GetPlayerAngles(Entity entity)
        {
            try
            {
                Vector3 angles = entity.Call<Vector3>("getplayerangles");
                return angles;
            }
            catch
            {
                stopCheck = true;
                return new Vector3(1f, 1f,1f);
            }
        }

        public void DoWeBan()
        {
            if (m_recoil.Count == 3 && m_recoil.All(x => x == 0f))
            {
                countSuspect++;

                if (countSuspect == 3)
                {
                    UICAntiCheat.AntiCheatBanClient(obj, "No-Recoil");
                    countSuspect = 0;
                }
            }
            else
                countSuspect = 0;
        }

        public bool IfSniper(string weapon)
        {
            return weapon.StartsWith("iw5_l96a1") || weapon.StartsWith("iw5_rsass") || weapon.StartsWith("iw5_msr") || weapon.StartsWith("iw5_barrett") || weapon.StartsWith("iw5_dragunov") || weapon.StartsWith("iw5_as50");
        }

    }
}
