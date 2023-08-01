using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace SmarterVisitors;

[HarmonyPatch(typeof(IncidentWorker_VisitorGroup), nameof(IncidentWorker_VisitorGroup.CreateLordJob))]
public static class IncidentWorker_VisitorGroup_CreateLordJob
{
    public static void Postfix(ref LordJob_VisitColony __result, List<Pawn> pawns)
    {
        var possibleSpots =
            pawns[0].Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("SmarterChillSpot"));

        if (!possibleSpots.Any())
        {
            return;
        }

        foreach (var possibleSpot in possibleSpots)
        {
            if (!pawns[0].CanReach(possibleSpot, PathEndMode.OnCell, Danger.Some))
            {
                continue;
            }

            __result.chillSpot = possibleSpot.Position;
            return;
        }
    }
}