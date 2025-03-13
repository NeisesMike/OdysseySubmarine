﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System.Collections;
using Nautilus.Options.Attributes;
using Nautilus.Options;
using Nautilus.Json;
using Nautilus.Handlers;
using Nautilus.Utility;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Bootstrap;

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

    [BepInPlugin("com.mikjaw.subnautica.odyssey.mod", "Odyssey", "1.6.3")]
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
