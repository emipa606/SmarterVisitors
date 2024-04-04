using RimWorld;
using Verse;

namespace SmarterVisitors;

[DefOf]
public static class SmartGameConditionDefOf
{
    public static GameConditionDef ToxicSpewer;

    static SmartGameConditionDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(SmartGameConditionDefOf));
    }
}