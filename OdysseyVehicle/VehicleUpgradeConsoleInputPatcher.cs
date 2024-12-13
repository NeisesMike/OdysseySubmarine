using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using VehicleFramework;
using UnityEngine;

namespace OdysseyVehicle
{

    [HarmonyPatch(typeof(VehicleUpgradeConsoleInput))]
    class VehicleUpgradeConsoleInputPatcher
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
            mainAnimator.SetBool("OD_module_door", false);
            closeDoorCor = null;
            yield break;
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VehicleUpgradeConsoleInput.OnHandHover))]
        public static void VehicleUpgradeConsoleInputOnHandHoverPostfix(VehicleUpgradeConsoleInput __instance, Sequence ___sequence)
        {
            // control opening the modules hatch
            if (__instance.GetComponentInParent<Odyssey>() != null)
            {
                __instance.GetComponentInParent<Odyssey>().mainAnimator.SetBool("OD_module_door", true);
                timeUntilClose = openDuration;
                if (closeDoorCor == null)
                {
                    closeDoorCor = UWE.CoroutineHost.StartCoroutine(closeDoorSoon(__instance.GetComponentInParent<Odyssey>().mainAnimator));
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(VehicleUpgradeConsoleInput.OpenPDA))]
        public static void VehicleUpgradeConsoleInputOpenPDAPostfix(VehicleUpgradeConsoleInput __instance, Sequence ___sequence)
        {
            // control opening the modules hatch
            if (__instance.GetComponentInParent<Odyssey>() != null)
            {
                UWE.CoroutineHost.StopCoroutine(closeDoorCor);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(VehicleUpgradeConsoleInput.OnClosePDA))]
        public static void VehicleUpgradeConsoleInputOnClosePDAPostfix(VehicleUpgradeConsoleInput __instance, Sequence ___sequence)
        {
            // control opening the modules hatch
            if (__instance.GetComponentInParent<Odyssey>() != null)
            {
                closeDoorCor = UWE.CoroutineHost.StartCoroutine(closeDoorSoon(__instance.GetComponentInParent<Odyssey>().mainAnimator));
            }
        }
    }
}
