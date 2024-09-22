using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using VehicleFramework;
using UnityEngine;

namespace OdysseyVehicle
{

    [HarmonyPatch(typeof(VehicleBatteryInput))]
    class VehicleBatteryInputPatcher
    {
        const float openDuration = 0.5f;
        static float timeUntilClose = 0f;
        static Coroutine closeDoorCor = null;
        public static IEnumerator closeDoorSoon(Animator mainAnimator)
        {
            while (timeUntilClose > 0)
            {
                timeUntilClose -= Time.deltaTime;
                yield return null;
            }
            mainAnimator.SetBool("OD_battery_door", false);
            closeDoorCor = null;
            yield break;
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VehicleBatteryInput.OnHandHover))]
        public static void VehicleUpgradeConsoleInputOnHandHoverPostfix(VehicleBatteryInput __instance)
        {
            // control opening the modules hatch
            if (__instance.GetComponentInParent<Odyssey>() != null && __instance.transform.name.Contains("PanelInsertRight"))
            {
                __instance.GetComponentInParent<Odyssey>().mainAnimator.SetBool("OD_battery_door", true);
                timeUntilClose = openDuration;
                if (closeDoorCor == null)
                {
                    closeDoorCor = UWE.CoroutineHost.StartCoroutine(closeDoorSoon(__instance.GetComponentInParent<Odyssey>().mainAnimator));
                }
            }
        }
    }
}
