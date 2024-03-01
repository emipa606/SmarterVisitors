using RimWorld;
using Verse;

namespace SmarterVisitors;

[DefOf]
public static class ThingDefOf
{
    public static ThingDef SmarterChillSpot;

    static ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}