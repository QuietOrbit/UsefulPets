using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using UsefulPets.Services;

namespace UsefulPets
{
    /// <summary>The main entry point for the Useful Pets mod.</summary>
    internal sealed class ModEntry : Mod
    {
        private PetService _petService = null!;
        private int _crowsChasedToday;
        private readonly Random _rng = new();

        /// <summary>Initializes the mod and hooks into game events.</summary>
        /// <param name="helper">SMAPI helper for event registration and mod access.</param>
        public override void Entry(IModHelper helper)
        {
            _petService = new PetService();

            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        /// <summary>Triggered at the start of each in-game day to calculate pet activity.</summary>
        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            if (!_petService.HasPet())
            {
                Monitor.Log("No pet found for the current player.", LogLevel.Warn);
                return;
            }

            var pet = _petService.GetPlayerPet();
            if (pet == null)
            {
                Monitor.Log("Pet instance is unexpectedly null.", LogLevel.Warn);
                return;
            }

            var friendship = _petService.GetFriendship(pet);
            Monitor.Log("Pet friendship: " + friendship, LogLevel.Info);

            var attempts = Math.Min(friendship / 200, 5);
            _crowsChasedToday = 0;

            for (var i = 0; i < attempts; i++)
            {
                if (_rng.NextDouble() < 0.3)
                    _crowsChasedToday++;
            }

            Monitor.Log("Your pet chased away " + _crowsChasedToday + " crow(s) today!", LogLevel.Info);
        }

        /// <summary>Triggered when the player interacts with the world using an action button.</summary>
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || !e.Button.IsActionButton())
                return;

            var clickedTile = e.Cursor.GrabTile;
            var character = Game1.currentLocation.characters
                .FirstOrDefault(c => c.Tile == clickedTile);

            if (character is not Pet pet)
                return;

            var petName = _petService.GetPetName(pet);
            var message = _crowsChasedToday > 0
                ? $"{petName} chased away {_crowsChasedToday} crow(s) today!"
                : $"{petName} didn't see any crows today, but they stayed alert.";

            Game1.drawObjectDialogue(message);
        }
    }
}
