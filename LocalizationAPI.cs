using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;

namespace CommonAPI
{
    /// <summary>
    /// API for accessing localized game strings.
    /// </summary>
    public static class LocalizationAPI
    {
        /// <summary>
        /// Retrieves the name for a character. Compatible with CrewBoom characters. Returns null if invalid.
        /// </summary>
        public static string GetCharacterName(Characters character)
        {
            var localizer = Core.Instance.Localizer;
            if (character < Characters.MAX)
                return localizer.GetCharacterName(character);
            if (CrewBoomAPI.CrewBoomAPIDatabase.IsInitialized)
            {
                if (CrewBoom.CharacterDatabase.GetCharacter(character, out CrewBoom.Data.CustomCharacter crewBoomCharacter))
                    return crewBoomCharacter.Definition.CharacterName;
            }
            return null;
        }
    }
}
