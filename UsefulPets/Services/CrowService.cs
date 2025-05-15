using StardewModdingAPI;
using UsefulPets.Util;

namespace UsefulPets.Services
{
    /// <summary>
    /// Handles crow protection logic based on pet friendship level.
    /// Evaluates whether the pet protects crops on a given day and tracks successful protections.
    /// </summary>
    internal class CrowService
    {
        private readonly PetService _petService;
        private readonly IMonitor _monitor;
        private readonly Random _rng = new();

        /// <summary>Tracks how many crops were protected from crows on the current day.</summary>
        public int CropsSavedToday { get; set; }

        /// <summary>Indicates whether crow protection is enabled for the current day.</summary>
        public bool ProtectToday { get; private set; }

        private const string Context = "CrowService";

        /// <summary>
        /// Constructs the crow service using a reference to the pet service and SMAPI monitor.
        /// </summary>
        /// <param name="petService">The pet service used to retrieve pet and friendship data.</param>
        /// <param name="monitor">The SMAPI monitor for logging.</param>
        public CrowService(PetService petService, IMonitor monitor)
        {
            _petService = petService;
            _monitor = monitor;
        }

        /// <summary>
        /// Evaluates whether the pet will protect crops today based on friendship.
        /// Calculates the chance to protect, rolls against it, and sets the outcome.
        /// </summary>
        public void EvaluateCrowProtection()
        {
            try
            {
                ProtectToday = false;
                CropsSavedToday = 0;

                var pet = _petService.GetPlayerPet();
                if (pet == null)
                {
                    LogHelper.Warn(_monitor, Context, "No pet found for the current player.");
                    return;
                }

                var friendship = _petService.GetFriendship(pet);
                LogHelper.Debug(_monitor, Context, $"Pet friendship: {friendship}");

                var chance = Math.Min(friendship / 10, 100);
                var roll = _rng.Next(100);

                ProtectToday = roll < chance;

                if (ProtectToday)
                    LogHelper.Debug(_monitor, Context, $"Crow protection activated! Chance: {chance}%, Roll: {roll}");
                else
                    LogHelper.Debug(_monitor, Context, $"Crow protection skipped. Chance: {chance}%, Roll: {roll}");
            }
            catch (Exception ex)
            {
                LogHelper.Warn(_monitor, Context, $"Unexpected error: {ex}");
            }
        }

        /// <summary>
        /// Reports how many crops were protected if protection was active and any protection occurred.
        /// Logs the result to the debug output.
        /// </summary>
        public void ReportSavedCrops()
        {
            if (ProtectToday && CropsSavedToday > 0)
                LogHelper.Debug(_monitor, Context, $"Your pet protected {CropsSavedToday} crop(s) from crows today!");
        }
    }
}
