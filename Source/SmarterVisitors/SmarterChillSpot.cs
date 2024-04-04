using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace SmarterVisitors;

public class SmarterChillSpot : Building
{
    public SmarterChillSpot()
    {
        var chillSpot = Current.Game?.CurrentMap?.listerBuildings
            ?.AllBuildingsColonistOfDef(SmartThingDefOf.SmarterChillSpot)?.FirstOrDefault();
        if (chillSpot == null)
        {
            return;
        }

        Messages.Message("SV.ChillSpotDestroyed".Translate(),
            new LookTargets(new GlobalTargetInfo(chillSpot.Position, chillSpot.Map)), MessageTypeDefOf.NeutralEvent,
            false);
        chillSpot.Destroy();
    }
}