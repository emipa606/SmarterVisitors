using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace SmarterVisitors;

[HarmonyPatch(typeof(Trigger_TicksPassed), nameof(Trigger_TicksPassed.ActivateOn))]
public static class Trigger_TicksPassed_ActivateOn
{
    public static bool Prefix(ref Trigger_TicksPassed __instance, Lord lord, TriggerSignal signal, ref bool __result)
    {
        if (!SmarterVisitorsMod.Instance.Settings.CheckForDanger)
        {
            return true;
        }

        __result = false;
        if (signal.type != TriggerSignalType.Tick)
        {
            return false;
        }

        if (lord == null || !lord.LordJob.GetType().Name.EndsWith("LordJob_VisitColony"))
        {
            return true;
        }

        if (__instance.data is not TriggerData_TicksPassed)
        {
            BackCompatibility.TriggerDataTicksPassedNull(__instance);
        }

        var component = Current.Game.GetComponent<GameComponent_ApprovedLords>();

        if (component == null)
        {
            Log.Warning("[SmarterVisitors]: Failed to find gamecomponent. Will not be able to delay guests");
            return true;
        }

        component.LordReasonHashes ??= [];

        var data = __instance.Data;
        var currentDelay = SmarterVisitors.GetDelayValue(lord);
        if (data.ticksPassed > __instance.duration + GenDate.TicksPerHour &&
            currentDelay > 0)
        {
            // Probably after loading save as the changed duration is not saved, resetting to saved value
            __instance.duration += currentDelay;
        }

        if (data.ticksPassed > __instance.duration)
        {
            __result = true;
            return false;
        }

        data.ticksPassed++;
        if (data.ticksPassed <= __instance.duration)
        {
            return false;
        }

        if (SmarterVisitors.CheckCanGo(lord.Map, lord.faction, out var reasons))
        {
            if (!SmarterVisitors.CheckIfOkTimeOfDay(lord) || !SmarterVisitors.CheckIfOkHealth(lord))
            {
                __instance.duration += GenDate.TicksPerHour;
                return false;
            }

            __result = true;
            return false;
        }

        var identifier = (long)lord.GetHashCode() + reasons.RawText.GetHashCode();
        if (component.LordReasonHashes.Contains(identifier))
        {
            __result = true;
            return false;
        }

        var instance = __instance;

        var askDialog = new Dialog_MessageBox("SV.VisitorsLeaving".Translate(reasons), "SV.Allow".Translate(),
            buttonAAction, "SV.Deny".Translate(), buttonDAction, "SV.VisitorsLeavingTitle".Translate(lord.faction))
        {
            buttonCAction = buttonWAction,
            buttonCText = "SV.WaitALittle".Translate()
        };
        Find.WindowStack.Add(askDialog);
        return false;

        void buttonAAction()
        {
            instance.duration += GenDate.TicksPerDay;
            SmarterVisitors.SetDelayValue(lord, GenDate.TicksPerDay);
        }

        void buttonWAction()
        {
            instance.duration += GenDate.TicksPerHour;
            SmarterVisitors.SetDelayValue(lord, GenDate.TicksPerHour);
        }

        void buttonDAction()
        {
            component.LordReasonHashes.Add(identifier);
            component.LordDelaysDictionary.Remove(lord);

            instance.ActivateOn(lord, signal);
        }
    }
}