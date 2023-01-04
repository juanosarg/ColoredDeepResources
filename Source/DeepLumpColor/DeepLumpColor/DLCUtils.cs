using System.Collections.Generic;
using UnityEngine;
using Verse;
using VFECore;

namespace DeepLumpColor
{
    public static class DLCUtils
    {
        public static void ApplySettings()
        {
            for (int i = 0; i < DLCMod.thingDefsName.Count; i++)
            {
                var def = DefDatabase<ThingDef>.GetNamed(DLCMod.thingDefsName[i]);
                var color = def.stuffProps != null ? def.stuffProps.color : Color.green;

                // Search settings for a defined color
                var containDefName = DLCMod.settings.deepColors.ContainsKey(def.defName);
                if (containDefName)
                    color = DLCMod.settings.deepColors[def.defName];

                // Try get the extension
                var ext = def.GetModExtension<ThingDefExtension>();
                if (ext == null)
                {
                    // Add it if it's not here
                    var newExt = new ThingDefExtension
                    {
                        deepColor = color,
                        transparencyMultiplier = DLCMod.settings.opacityMultiplier
                    };

                    if (def.modExtensions == null)
                        def.modExtensions = new List<DefModExtension>();

                    def.modExtensions.Add(newExt);

                    if (!containDefName)
                    {
                        DLCMod.settings.deepColors.Add(def.defName, color);
                        DLCMod.settings.deepColorsSave.Add(def.defName, color);
                        // Log.Message($"Adding {def.defName} with {ColoredText.Colorize("this color", color)}");
                    }
                }
                else
                {
                    if (!containDefName)
                    {
                        DLCMod.settings.deepColors.Add(def.defName, ext.deepColor);
                        DLCMod.settings.deepColorsSave.Add(def.defName, ext.deepColor);
                        // Log.Message($"Adding {def.defName} with {ColoredText.Colorize("this color", ext.deepColor)}");
                    }
                    else
                    {
                        ext.deepColor = color;
                    }

                    // Modify it if it's already here
                    ext.transparencyMultiplier = DLCMod.settings.opacityMultiplier;
                }
            }

            if (Find.CurrentMap is Map map)
            {
                var drawer = (CellBoolDrawer)DLCStartup.resourceGridDrawer.GetValue(map.deepResourceGrid);
                drawer.SetDirty();
            }
        }
    }
}
