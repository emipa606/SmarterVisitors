using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace SmarterVisitors;

public class FloatMenuOptionProvider_DismissVisitor : FloatMenuOptionProvider
{
    private static readonly JobDef DismissVisitorJobDef = DefDatabase<JobDef>.GetNamedSilentFail("SV_DismissVisitor");
    public override bool Drafted => true;

    public override bool Undrafted => true;

    public override bool Multiselect => false;

    public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
    {
        if (clickedPawn == null || !clickedPawn.TryGetLord(out var lord) || lord.LordJob is not LordJob_VisitColony)
        {
            yield break;
        }

        if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly))
        {
            yield return new FloatMenuOption("CannotReach".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(),
                null);
            yield break;
        }

        if (context.FirstSelectedPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
        {
            yield return new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap),
                null);
            yield break;
        }

        if (SmarterVisitors.TimeToLeaveLords?.Contains(lord) == true)
        {
            yield return new FloatMenuOption("SV.DismissedVisitor".Translate(), null);
            yield break;
        }

        yield return FloatMenuUtility.DecoratePrioritizedTask(
            new FloatMenuOption("SV.DismissVisitor".Translate(), action, MenuOptionPriority.InitiateSocial, null,
                clickedPawn), context.FirstSelectedPawn, clickedPawn);
        yield break;

        void action()
        {
            var job = JobMaker.MakeJob(DismissVisitorJobDef, clickedPawn);
            job.playerForced = true;
            context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }
    }
}