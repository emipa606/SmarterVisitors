using System.Collections.Generic;
using Verse;

namespace SmarterVisitors;

public class GameComponent_ApprovedLords : GameComponent
{
    public List<long> LordReasonHashes;

    // ReSharper disable once UnusedParameter.Local
    public GameComponent_ApprovedLords(Game game)
    {
        LordReasonHashes = new List<long>();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref LordReasonHashes, "LordReasonHashes", LookMode.Value);
    }
}