using Mlie;
using UnityEngine;
using Verse;

namespace SmarterVisitors;

[StaticConstructorOnStartup]
internal class SmarterVisitorsMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SmarterVisitorsMod instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SmarterVisitorsMod(ModContentPack content) : base(content)
    {
        instance = this;
        Settings = GetSettings<SmarterVisitorsSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal SmarterVisitorsSettings Settings { get; }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Smarter Visitors";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.CheckboxLabeled("SV.CheckForDanger".Translate(), ref Settings.CheckForDanger,
            "SV.CheckForDangerTT".Translate());
        listing_Standard.CheckboxLabeled("SV.CheckHealth".Translate(), ref Settings.CheckHealth,
            "SV.CheckHealthTT".Translate());
        listing_Standard.CheckboxLabeled("SV.DelayThoughts".Translate(), ref Settings.DelayThoughts,
            "SV.DelayThoughtsTT".Translate());
        if (ModLister.BiotechInstalled || SmarterVisitors.VampiresLoaded)
        {
            listing_Standard.CheckboxLabeled("SV.UVLightSensitivity".Translate(), ref Settings.UVLightSensitivity,
                "SV.UVLightSensitivityTT".Translate());
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SV.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}