using HarmonyLib;
using StardewModdingAPI;
using StardewValley.TerrainFeatures;
using UsefulPets.Util;

namespace UsefulPets.Patches
{
    /// <summary>
    /// Patch that intercepts <see cref="HoeDirt.destroyCrop"/> to optionally block crow-driven crop destruction
    /// based on protection logic evaluated earlier in the day by <see cref="Services.CrowService"/>.
    /// </summary>
    [HarmonyPatch(typeof(HoeDirt), nameof(HoeDirt.destroyCrop))]
    internal static class DestroyCropPatch
    {
        /// <summary>
        /// Prefix patch for <c>HoeDirt.destroyCrop(bool)</c>. If crow protection is active, prevents the crop from being destroyed.
        /// Otherwise, lets the base method execute normally.
        /// </summary>
        /// <param name="__instance">The <see cref="HoeDirt"/> instance representing the soil where the crop lives.</param>
        /// <param name="showAnimation">Whether to show the destruction animation (passed from base game).</param>
        /// <returns><c>false</c> to cancel the original crop destruction; <c>true</c> to allow it.</returns>
        public static bool Prefix(HoeDirt __instance, bool showAnimation) =>
            SafeHelper.RunTrue(ModEntry.Instance.Monitor, () =>
            {
                var crowService = ModEntry.Instance.CrowService;

                if (!crowService.ProtectToday)
                    return true;

                crowService.CropsSavedToday++;
                LogHelper.Debug(ModEntry.Instance.Monitor, "DestroyCropPatch", "Blocked crow from destroying a crop.");

                return false;
            }, "DestroyCropPatch.Prefix");
    }
}