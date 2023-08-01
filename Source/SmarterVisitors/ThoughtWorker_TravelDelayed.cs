using RimWorld;
using Verse;
using Verse.AI.Group;

namespace SmarterVisitors;

public class ThoughtWorker_TravelDelayed : ThoughtWorker
{
    public override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!SmarterVisitorsMod.instance.Settings.DelayThoughts)
        {
            return ThoughtState.Inactive;
        }

        var lord = p.GetLord();
        if (lord == null)
        {
            return ThoughtState.Inactive;
        }

        var component = Current.Game.GetComponent<GameComponent_ApprovedLords>();
        if (!component.LordDelaysDictionary.ContainsKey(lord))
        {
            return ThoughtState.Inactive;
        }

        switch (component.LordDelaysDictionary[lord])
        {
            case < GenDate.TicksPerHour * 3:
                return ThoughtState.ActiveAtStage(0);
            case < GenDate.TicksPerDay:
                return ThoughtState.ActiveAtStage(1);
            case < GenDate.TicksPerDay * 2:
                return ThoughtState.ActiveAtStage(2);
            default:
                return ThoughtState.ActiveAtStage(3);
        }
    }
}