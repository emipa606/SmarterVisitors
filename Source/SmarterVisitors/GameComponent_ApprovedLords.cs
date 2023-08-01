using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace SmarterVisitors;

public class GameComponent_ApprovedLords : GameComponent
{
    public Dictionary<Lord, int> LordDelaysDictionary;
    private List<Lord> lordDelaysDictionaryKeys = new List<Lord>();
    private List<int> lordDelaysDictionaryValues = new List<int>();
    public List<long> LordReasonHashes;

    // ReSharper disable once UnusedParameter.Local
    public GameComponent_ApprovedLords(Game game)
    {
        LordReasonHashes = new List<long>();
        LordDelaysDictionary = new Dictionary<Lord, int>();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref LordReasonHashes, "LordReasonHashes", LookMode.Value);
        Scribe_Collections.Look(ref LordDelaysDictionary, "LordDelaysDictionary", LookMode.Reference, LookMode.Value,
            ref lordDelaysDictionaryKeys, ref lordDelaysDictionaryValues);
    }
}