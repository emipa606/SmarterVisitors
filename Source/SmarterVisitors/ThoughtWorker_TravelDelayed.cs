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

        switch (SmarterVisitors.GetDelayValue(lord))
        {
            case 0:
                return ThoughtState.Inactive;
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