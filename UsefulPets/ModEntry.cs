using System.Reflection;
using HarmonyLib;
using StardewModdingAPI;
using UsefulPets.Events;
using UsefulPets.Patches;
using UsefulPets.Services;
using UsefulPets.Util;

namespace UsefulPets
{
    /// <summary>The main entry point for the Useful Pets mod.</summary>
    internal sealed class ModEntry : Mod
    {
        /// <summary>Singleton instance for global access to mod services and data.</summary>
        public static ModEntry Instance { get; private set; } = null!;

        /// <summary>Primary service that manages crow protection logic.</summary>
        public CrowService CrowService { get; private set; } = null!;

        /// <summary>Provides access to the player's pet and its friendship level.</summary>
        private PetService PetService { get; set; } = null!;

        private const string Context = "ModEntry";

        /// <summary>
        /// Called by SMAPI when the mod is first loaded. Initializes services and applies Harmony patches.
        /// </summary>
        /// <param name="helper">The SMAPI helper provided to all mods.</param>
        public override void Entry(IModHelper helper)
        {
            Instance = this;

            PetService = new PetService();
            CrowService = new CrowEventHandler(helper, Monitor, PetService).CrowService;

            try
            {
                var harmony = new Harmony(ModManifest.UniqueID);

                var target = typeof(StardewValley.TerrainFeatures.HoeDirt).GetMethod(
                    nameof(StardewValley.TerrainFeatures.HoeDirt.destroyCrop),
                    BindingFlags.Instance | BindingFlags.Public
                );

                var prefix = new HarmonyMethod(typeof(DestroyCropPatch), nameof(DestroyCropPatch.Prefix));

                if (target != null)
                {
                    harmony.Patch(target, prefix: prefix);
                    LogHelper.Debug(Monitor, Context, "Patched HoeDirt.destroyCrop (prefix)");
                }
                else
                {
                    LogHelper.Warn(Monitor, Context, "Could not find HoeDirt.destroyCrop method.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Warn(Monitor, Context, $"Error applying Harmony patches: {ex}");
            }
        }
    }
}
