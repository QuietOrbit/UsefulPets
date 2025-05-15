using StardewModdingAPI;
using StardewModdingAPI.Events;
using UsefulPets.Services;
using UsefulPets.Util;

namespace UsefulPets.Events
{
    /// <summary>
    /// Handles game events related to crow protection and delegates logic to <see cref="CrowService"/>.
    /// </summary>
    internal class CrowEventHandler
    {
        private readonly IMonitor _monitor;

        /// <summary>
        /// Gets the crow service responsible for evaluating and tracking daily crop protection.
        /// </summary>
        public CrowService CrowService { get; }

        /// <summary>
        /// Constructs the event handler and registers SMAPI event callbacks.
        /// </summary>
        /// <param name="helper">SMAPI helper instance used to hook game events.</param>
        /// <param name="monitor">Monitor used for logging within the handler and downstream service.</param>
        /// <param name="petService">Service used to retrieve the player's pet and friendship data.</param>
        public CrowEventHandler(IModHelper helper, IMonitor monitor, PetService petService)
        {
            _monitor = monitor;
            CrowService = new CrowService(petService, _monitor);

            helper.Events.GameLoop.DayStarted += OnDayStarted;
        }

        /// <summary>
        /// Callback for the start of a new in-game day. Evaluates crow protection for that day.
        /// </summary>
        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            SafeHelper.Run(_monitor, () =>
            {
                CrowService.EvaluateCrowProtection();
                CrowService.ReportSavedCrops();
            }, "CrowEventHandler.DayStarted");
        }
    }
}