using System.Collections.Generic;
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
        if (SmarterVisitorsMod.Instance.Settings.AddExtraFood > 0)
        {
            foreach (var pawn in pawns)
            {
                for (var i = 0; i < SmarterVisitorsMod.Instance.Settings.AddExtraFood; i++)
                {
                    PawnInventoryGenerator.GiveRandomFood(pawn);
                }
            }
        }

        var possibleSpots =
            pawns[0].Map.listerBuildings.AllBuildingsColonistOfDef(SmartThingDefOf.SmarterChillSpot);

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

            AccessTools.Field(typeof(LordJob_VisitColony), "chillSpot").SetValue(__result, possibleSpot.Position);
            return;
        }
    }
}