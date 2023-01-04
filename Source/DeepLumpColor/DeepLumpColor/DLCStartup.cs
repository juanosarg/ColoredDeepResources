using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace DeepLumpColor
{
    [StaticConstructorOnStartup]
    public static class DLCStartup
    {
        public static FieldInfo resourceGridDrawer = null;

        static DLCStartup()
        {
            resourceGridDrawer = typeof(DeepResourceGrid).GetField("drawer", BindingFlags.NonPublic | BindingFlags.Instance);

            DLCMod.thingDefsName = DefDatabase<ThingDef>.AllDefsListForReading.FindAll(t => t.deepCommonality > 0).Select(t => t.defName).ToList();

            if (DLCMod.settings.deepColors == null)
                DLCMod.settings.deepColors = new Dictionary<string, UnityEngine.Color>();

            if (DLCMod.settings.deepColorsSave == null)
                DLCMod.settings.deepColorsSave = new Dictionary<string, UnityEngine.Color>();

            DLCUtils.ApplySettings();
        }
    }
}
