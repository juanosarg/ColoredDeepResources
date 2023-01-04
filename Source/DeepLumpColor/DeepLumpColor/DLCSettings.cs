using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DeepLumpColor
{
    public class DLCSettings : ModSettings
    {
        public float opacityMultiplier = 0.75f;

        public Dictionary<string, Color> deepColors = new Dictionary<string, Color>();
        public Dictionary<string, Color> deepColorsSave = new Dictionary<string, Color>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref opacityMultiplier, "opacityMultiplier");
            Scribe_Collections.Look(ref deepColors, "deepColors", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref deepColorsSave, "deepColorsSave", LookMode.Value, LookMode.Value);
        }
    }
}
