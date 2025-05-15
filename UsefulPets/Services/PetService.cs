using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;

namespace UsefulPets.Services
{
    /// <summary>
    /// Provides utility methods to access the player's pet and related data.
    /// </summary>
    internal class PetService
    {
        /// <summary>
        /// Returns the current player's pet, or null if they don’t have one.
        /// </summary>
        public Pet? GetPlayerPet()
        {
            return Game1.player?.getPet();
        }

        /// <summary>
        /// Gets the pet's friendship level.
        /// </summary>
        public int GetFriendship(Pet pet)
        {
            return pet.friendshipTowardFarmer.Value;
        }

        /// <summary>
        /// Gets the pet's name, or a fallback if unnamed.
        /// </summary>
        public string GetPetName(Pet pet)
        {
            return string.IsNullOrWhiteSpace(pet.Name) ? "Your pet" : pet.Name;
        }

        /// <summary>
        /// Checks if the player currently has a pet.
        /// </summary>
        public bool HasPet()
        {
            return Game1.player?.hasPet() ?? false;
        }
    }
}