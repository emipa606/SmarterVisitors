using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace SmarterVisitors;

[StaticConstructorOnStartup]
public static class SmarterVisitors
{
    public static readonly GeneDef UvGeneDef;
    public static bool VampiresLoaded;

    static SmarterVisitors()
    {
        new Harmony("Mlie.SmarterVisitors").PatchAll(Assembly.GetExecutingAssembly());
        UvGeneDef = DefDatabase<GeneDef>.GetNamedSilentFail("UVSensitivity_Intense");
        VampiresLoaded = ModLister.GetActiveModWithIdentifier("RimOfMadness.Vampires") != null;
    }

    /// <summary>
    ///     This whole method is based on code from the excellent mod Hospitality
    ///     (https://steamcommunity.com/sharedfiles/filedetails/?id=753498552) by Orion
    /// </summary>
    public static bool CheckCanGo(Map map, Faction faction, out TaggedString reasons)
    {
        var fallout = map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout);
        var spewer = map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicSpewer);
        var potentiallyDangerous = map.mapPawns.AllPawnsSpawned
            .Where(p => !p.Dead && !p.IsPrisoner && !p.Downed && !IsFogged(p) && !p.InContainerEnclosed).ToArray();
        var hostileFactions = potentiallyDangerous.Where(p => p.Faction != null).Select(p => p.Faction)
            .Where(f => f.HostileTo(Faction.OfPlayer) || f.HostileTo(faction)).ToArray();
        var winter = map.GameConditionManager.ConditionIsActive(GameConditionDefOf.VolcanicWinter);
        var temp = faction.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) &&
                   faction.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp);
        var manhunters = potentiallyDangerous.Where(p => p.InAggroMentalState);

        reasons = null;

        if (temp && !fallout && !winter && !hostileFactions.Any() && !manhunters.Any())
        {
            return true;
        }

        var reasonList = new List<string>();
        if (fallout)
        {
            reasonList.Add("- " + GameConditionDefOf.ToxicFallout.LabelCap);
        }

        if (spewer)
        {
            reasonList.Add("- " + GameConditionDefOf.ToxicSpewer.LabelCap);
        }

        if (winter)
        {
            reasonList.Add("- " + GameConditionDefOf.VolcanicWinter.LabelCap);
        }

        if (!temp)
        {
            reasonList.Add("- " + "Temperature".Translate());
        }

        reasonList.AddRange(hostileFactions.Select(f => $"- {f.def.pawnsPlural.CapitalizeFirst()}"));

        foreach (var manhunter in manhunters.GroupBy(p => p.MentalStateDef))
        {
            switch (manhunter.Count())
            {
                case > 1:
                    reasonList.Add(
                        $"- {manhunter.First().GetKindLabelPlural()} ({manhunter.First().MentalStateDef.label})");
                    break;
                case 1:
                    reasonList.Add($"- {manhunter.First().LabelShort} ({manhunter.First().MentalStateDef.label})");
                    break;
            }
        }

        reasons = reasonList.Distinct().Aggregate((a, b) => $"{a}\n{b}");
        return false;
    }

    public static bool CheckIfOkTimeOfDay(Lord lord)
    {
        if (!SmarterVisitorsMod.instance.Settings.UVLightSensitivity)
        {
            return true;
        }

        if (lord.Map.skyManager.CurSkyGlow <= 0.1f) // Not daytime
        {
            return true;
        }

        if (VampiresLoaded)
        {
            if (lord.ownedPawns.Any(pawn =>
                    pawn.health?.hediffSet.hediffs?.Any(hediff => hediff.def.defName.StartsWith("ROM_Vampirism")) ==
                    true))
            {
                Log.Message("Vampire in group");
                return false;
            }
        }

        if (!ModsConfig.BiotechActive)
        {
            return true;
        }

        if (UvGeneDef == null)
        {
            return true;
        }

        return lord.ownedPawns.All(pawn => pawn.genes?.GetGene(UvGeneDef) == null);
    }

    public static bool CheckIfOkHealth(Lord lord)
    {
        return !SmarterVisitorsMod.instance.Settings.CheckHealth ||
               lord.ownedPawns.All(pawn => !HealthAIUtility.ShouldSeekMedicalRest(pawn));
    }

    private static bool IsFogged(Pawn pawn)
    {
        return pawn.MapHeld.fogGrid.IsFogged(pawn.PositionHeld);
    }
}