using System;
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
    public static SmarterVisitorsMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SmarterVisitorsMod(ModContentPack content) : base(content)
    {
        Instance = this;
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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.CheckboxLabeled("SV.CheckForDanger".Translate(), ref Settings.CheckForDanger,
            "SV.CheckForDangerTT".Translate());
        listingStandard.CheckboxLabeled("SV.CheckHealth".Translate(), ref Settings.CheckHealth,
            "SV.CheckHealthTT".Translate());
        listingStandard.CheckboxLabeled("SV.DelayThoughts".Translate(), ref Settings.DelayThoughts,
            "SV.DelayThoughtsTT".Translate());
        if (ModLister.BiotechInstalled || SmarterVisitors.VampiresLoaded)
        {
            listingStandard.CheckboxLabeled("SV.UVLightSensitivity".Translate(), ref Settings.UVLightSensitivity,
                "SV.UVLightSensitivityTT".Translate());
        }

        listingStandard.Gap();
        Settings.AddExtraFood = (int)Math.Round(listingStandard.SliderLabeled(
            "SV.AddExtraFood".Translate(Settings.AddExtraFood), Settings.AddExtraFood, 0, 5f,
            tooltip: "SV.AddExtraFoodTT".Translate()));

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("SV.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}