using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace SmarterVisitors;

public class JobDriver_DismissVisitor : JobDriver
{
    private Pawn Visitor => (Pawn)TargetThingA;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(Visitor, job, 1, -1, null, errorOnFailed);
    }

    public override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedOrNull(TargetIndex.A);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() =>
            Visitor.GetLord().LordJob is not LordJob_VisitColony ||
            SmarterVisitors.TimeToLeaveLords?.Contains(Visitor.GetLord()) == true);
        var toil = ToilMaker.MakeToil();
        toil.initAction = delegate
        {
            SmarterVisitors.TimeToLeaveLords ??= [];
            SmarterVisitors.TimeToLeaveLords.Add(Visitor.GetLord());
        };
        yield return toil;
    }
}