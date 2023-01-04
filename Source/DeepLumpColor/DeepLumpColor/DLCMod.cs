using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DeepLumpColor
{
    public class DLCMod : Mod
    {
        public static DLCSettings settings;

        public static List<string> thingDefsName;

        private static Vector2 scrollPosition = Vector2.zero;

        private string opacityBuffer;
        private bool wheelDragging;
        private readonly bool tempDragging;
        private readonly float borderOffsest = 30f;
        private readonly float lineHeight = 32f;
        private readonly float offset = 10f;
        private readonly float sLineHeight = 30f;
        private readonly float startPos = 48f;

        public DLCMod(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<DLCSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float y = startPos;

            // Opacity
            Rect opacityRect = new Rect(inRect.x, y, inRect.width, sLineHeight);
            Widgets.Label(opacityRect.LeftHalf().Rounded(), "DeepLC_Multiplier".Translate());
            Widgets.TextFieldPercent(opacityRect.RightHalf().Rounded(), ref settings.opacityMultiplier, ref opacityBuffer);
            y += offset + sLineHeight;

            // Color settings
            Rect outRect = new Rect(inRect.x, y, inRect.width, inRect.height - (offset + sLineHeight));
            Rect viewRect = new Rect(inRect.x, y, inRect.width - borderOffsest, thingDefsName.Count * lineHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);

            var lst = new Listing_Standard();
            lst.Begin(viewRect);

            for (int num = 0; num < thingDefsName.Count; num++)
            {
                var thingDef = DefDatabase<ThingDef>.GetNamed(thingDefsName[num]);
                var lineRect = lst.GetRect(lineHeight);

                var rectFHalf = lineRect.LeftHalf();

                var iconRect = new Rect(rectFHalf.x, rectFHalf.y, sLineHeight, sLineHeight);
                Widgets.ThingIcon(iconRect, thingDef);

                var labelRect = new Rect(rectFHalf.x + sLineHeight + (offset * 2), rectFHalf.y, rectFHalf.width - sLineHeight - (offset * 2), sLineHeight);
                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(labelRect, ColoredText.Colorize(thingDef.label.CapitalizeFirst(), settings.deepColors[thingDef.defName]));
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;

                var rectSHalf = lineRect.RightHalf();

                var wheelRect = new Rect(rectSHalf.x + sLineHeight / 2, rectSHalf.y, sLineHeight, sLineHeight);
                var c = settings.deepColors[thingDef.defName];
                Widgets.HSVColorWheel(wheelRect, ref c, ref wheelDragging);

                var resetRect = new Rect(rectSHalf.x + sLineHeight + (offset * 2), rectSHalf.y, rectSHalf.width - sLineHeight - (offset * 2), sLineHeight);

                BrightnessChange(resetRect.LeftHalf(), ref c);
                if (Widgets.ButtonText(resetRect.RightHalf(), "DeepLC_Reset".Translate()))
                {
                    c = settings.deepColorsSave[thingDef.defName];
                }

                settings.deepColors[thingDef.defName] = c;
            }

            lst.End();
            Widgets.EndScrollView();
            settings.Write();
        }

        private void BrightnessChange(Rect rect, ref Color color)
        {
            var middleWith = (int)(rect.width - (sLineHeight * 2));

            var lessRect = new Rect(rect.x, rect.y, sLineHeight, rect.height);
            if (Widgets.ButtonText(lessRect, "-") && color.r / 1.5f >= 0 && color.g / 1.5f >= 0 && color.b / 1.5f >= 0)
            {
                color.r /= 1.5f;
                color.g /= 1.5f;
                color.b /= 1.5f;
            }

            var middleRect = new Rect(rect.x + sLineHeight, rect.y, middleWith, rect.height);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(middleRect, "DeepLC_Bright".Translate());
            Text.Anchor = TextAnchor.UpperLeft;

            var plusRect = new Rect(rect.x + sLineHeight + middleWith, rect.y, sLineHeight, rect.height);
            if (Widgets.ButtonText(plusRect, "+") && color.r * 1.5f <= 255 && color.g * 1.5f <= 255 && color.b * 1.5f <= 255)
            {
                color.r *= 1.5f;
                color.g *= 1.5f;
                color.b *= 1.5f;
            }
        }

        public override string SettingsCategory() => "DeepLC_Name".Translate();

        public override void WriteSettings()
        {
            base.WriteSettings();
            DLCUtils.ApplySettings();
        }
    }
}
