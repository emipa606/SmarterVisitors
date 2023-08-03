using Verse;

namespace SmarterVisitors;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class SmarterVisitorsSettings : ModSettings
{
    public int AddExtraFood = 1;
    public bool CheckForDanger = true;
    public bool CheckHealth = true;
    public bool DelayThoughts = true;
    public bool UVLightSensitivity = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref AddExtraFood, "AddExtraFood", 1);
        Scribe_Values.Look(ref CheckForDanger, "CheckForDanger", true);
        Scribe_Values.Look(ref UVLightSensitivity, "UVLightSensitivity", true);
        Scribe_Values.Look(ref CheckHealth, "CheckHealth", true);
        Scribe_Values.Look(ref DelayThoughts, "DelayThoughts", true);
    }
}