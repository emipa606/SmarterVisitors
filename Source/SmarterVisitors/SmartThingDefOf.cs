using RimWorld;
using Verse;

namespace SmarterVisitors;

[DefOf]
public static class SmartThingDefOf
{
    public static ThingDef SmarterChillSpot;

    static SmartThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(SmartThingDefOf));
    }
}