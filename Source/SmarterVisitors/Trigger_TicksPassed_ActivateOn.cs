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
        if (!SmarterVisitorsMod.instance.Settings.CheckForDanger)
        {
            return true;
        }

        __result = false;
        if (signal.type != TriggerSignalType.Tick)
        {
            return false;
        }

        if (lord == null || lord.LordJob.GetType().Name.EndsWith("LordJob_VisitColony") == false)
        {
            return true;
        }

        if (__instance.data is not TriggerData_TicksPassed)
        {
            BackCompatibility.TriggerDataTicksPassedNull(__instance);
        }

        var component = Current.Game.GetComponent<GameComponent_ApprovedLords>();

        var data = __instance.Data;

        if (data.ticksPassed > __instance.duration + GenDate.TicksPerHour &&
            component.LordDelaysDictionary.TryGetValue(lord, out var value))
        {
            // Probably after loading save as the changed duration is not saved, resetting to saved value
            __instance.duration += value;
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
            if (!SmarterVisitors.CheckIfOkTimeOfDay(lord))
            {
                __instance.duration += GenDate.TicksPerHour;
                return false;
            }

            if (!SmarterVisitors.CheckIfOkHealth(lord))
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

        void ButtonAAction()
        {
            instance.duration += GenDate.TicksPerDay;
            if (component.LordDelaysDictionary.ContainsKey(lord))
            {
                component.LordDelaysDictionary[lord] += GenDate.TicksPerDay;
            }
            else
            {
                component.LordDelaysDictionary[lord] = GenDate.TicksPerDay;
            }
        }

        void ButtonWAction()
        {
            instance.duration += GenDate.TicksPerHour;
            if (component.LordDelaysDictionary.ContainsKey(lord))
            {
                component.LordDelaysDictionary[lord] += GenDate.TicksPerHour;
            }
            else
            {
                component.LordDelaysDictionary[lord] = GenDate.TicksPerHour;
            }
        }

        void ButtonDAction()
        {
            component.LordReasonHashes.Add(identifier);
            if (component.LordDelaysDictionary.ContainsKey(lord))
            {
                component.LordDelaysDictionary.Remove(lord);
            }

            instance.ActivateOn(lord, signal);
        }

        var askDialog = new Dialog_MessageBox("SV.VisitorsLeaving".Translate(reasons), "SV.Allow".Translate(),
            ButtonAAction, "SV.Deny".Translate(), ButtonDAction, "SV.VisitorsLeavingTitle".Translate(lord.faction))
        {
            buttonCAction = ButtonWAction,
            buttonCText = "SV.WaitALittle".Translate()
        };
        Find.WindowStack.Add(askDialog);
        return false;
    }
}