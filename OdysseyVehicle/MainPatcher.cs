using HarmonyLib;
using Nautilus.Utility;
using BepInEx;
using BepInEx.Logging;

namespace OdysseyVehicle
{
    public static class Logger
    {
        public static ManualLogSource MyLog { get; set; }
        public static void Warn(string message)
        {
            MyLog.LogWarning("[OdysseyVehicle] " + message);
        }
        public static void Log(string message)
        {
            MyLog.LogInfo("[OdysseyVehicle] " + message);
        }
        public static void Output(string msg)
        {
            BasicText message = new BasicText(500, 0);
            message.ShowMessage(msg, 5);
        }
    }

    [BepInPlugin("com.mikjaw.subnautica.odyssey.mod", "Odyssey", "1.6.6")]
    [BepInDependency(VehicleFramework.PluginInfo.PLUGIN_GUID, VehicleFramework.PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class MainPatcher : BaseUnityPlugin
    {
        public void Start()
        {
            OdysseyVehicle.Logger.MyLog = base.Logger;
            var harmony = new Harmony("com.mikjaw.subnautica.odyssey.mod");
            harmony.PatchAll();
            UWE.CoroutineHost.StartCoroutine(Odyssey.Register());
        }
    }
}
