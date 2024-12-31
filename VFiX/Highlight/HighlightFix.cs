using RoR2;

namespace VFiX.Highlight
{
    static class HighlightFix
    {
        public static void Init()
        {
            On.RoR2.OutlineHighlight.NeedsHighlight += OutlineHighlight_NeedsHighlight;
        }

        static bool OutlineHighlight_NeedsHighlight(On.RoR2.OutlineHighlight.orig_NeedsHighlight orig, OutlineHighlight self)
        {
            return orig(self) || OutlineHighlight.onPreRenderOutlineHighlight != null;
        }
    }
}
