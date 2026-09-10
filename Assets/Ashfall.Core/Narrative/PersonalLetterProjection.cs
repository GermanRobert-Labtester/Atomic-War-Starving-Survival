using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum LetterTruthClass
    {
        HistoricalTestimony,
        CampaignContemporary,
        DeliveredCorrespondence,
        UnsentPrivateArtifact,
        AmbiguousTestimony,
        UnsafeUnresolved
    }

    /// <summary>
    /// Plan 150: Projects authored personal correspondence to canonical shelter rooms (room_*),
    /// truth and provenance classes, and discovery contexts without mutating simulation state.
    /// </summary>
    public static class PersonalLetterProjection
    {
        private static readonly Dictionary<string, string> LetterToRoomMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["letter_01_to_mother"] = "room_bunks",
                ["letter_02_to_son"] = "room_airlock",
                ["letter_03_to_sister"] = "room_kitchen",
                ["letter_04_to_lover_returning"] = "room_bunks",
                ["letter_05_to_lover_gone"] = "room_bunks",
                ["letter_06_to_grown_daughter"] = "room_greenhouse",
                ["letter_07_last_letter"] = "room_airlock",
                ["letter_08_confession_theft"] = "room_storage_bay",
                ["letter_09_confession_cowardice"] = "room_bunker_corridor",
                ["letter_10_confession_mercy"] = "room_clinic",
                ["letter_11_to_the_dead_husband"] = "room_clinic",
                ["letter_12_to_the_dead_child"] = "room_bunker_corridor",
                ["letter_13_to_whoever_finds_this"] = "room_airlock",
                ["letter_14_thank_you_to_scavenger"] = "room_workshop",
                ["letter_15_child_to_father"] = "room_bunks",
                ["letter_16_to_old_friend"] = "room_kitchen",
                ["letter_17_to_the_engineer"] = "room_filtration",
                ["letter_18_the_list_of_names"] = "room_main",
                ["letter_19_to_the_house"] = "room_airlock",
                ["letter_20_apology_to_stranger"] = "room_water_pump",
                ["letter_21_to_the_teacher"] = "room_bunker_corridor",
                ["letter_22_to_younger_self"] = "room_greenhouse",
                ["letter_23_warning_to_the_next"] = "room_airlock",
                ["letter_24_love_without_the_word"] = "room_bunks",
                ["letter_25_one_sentence"] = "room_airlock",
                ["b2_38"] = "room_workshop",
            };

        private static readonly Dictionary<string, LetterTruthClass> LetterToTruthClassMap =
            new Dictionary<string, LetterTruthClass>(StringComparer.OrdinalIgnoreCase)
            {
                ["letter_01_to_mother"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_02_to_son"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_03_to_sister"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_04_to_lover_returning"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_05_to_lover_gone"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_06_to_grown_daughter"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_07_last_letter"] = LetterTruthClass.HistoricalTestimony,
                ["letter_08_confession_theft"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_09_confession_cowardice"] = LetterTruthClass.AmbiguousTestimony,
                ["letter_10_confession_mercy"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_11_to_the_dead_husband"] = LetterTruthClass.CampaignContemporary,
                ["letter_12_to_the_dead_child"] = LetterTruthClass.CampaignContemporary,
                ["letter_13_to_whoever_finds_this"] = LetterTruthClass.HistoricalTestimony,
                ["letter_14_thank_you_to_scavenger"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_15_child_to_father"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_16_to_old_friend"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_17_to_the_engineer"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_18_the_list_of_names"] = LetterTruthClass.CampaignContemporary,
                ["letter_19_to_the_house"] = LetterTruthClass.AmbiguousTestimony,
                ["letter_20_apology_to_stranger"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_21_to_the_teacher"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_22_to_younger_self"] = LetterTruthClass.CampaignContemporary,
                ["letter_23_warning_to_the_next"] = LetterTruthClass.DeliveredCorrespondence,
                ["letter_24_love_without_the_word"] = LetterTruthClass.UnsentPrivateArtifact,
                ["letter_25_one_sentence"] = LetterTruthClass.HistoricalTestimony,
                ["b2_38"] = LetterTruthClass.CampaignContemporary,
            };

        public static string ResolveRoomForLetter(string letterId)
        {
            if (string.IsNullOrEmpty(letterId)) return "room_bunker_corridor";
            if (LetterToRoomMap.TryGetValue(letterId, out var roomId))
            {
                return roomId;
            }
            return "room_bunker_corridor";
        }

        public static LetterTruthClass ResolveTruthClass(string letterId)
        {
            if (string.IsNullOrEmpty(letterId)) return LetterTruthClass.AmbiguousTestimony;
            if (LetterToTruthClassMap.TryGetValue(letterId, out var truthClass))
            {
                return truthClass;
            }
            return LetterTruthClass.AmbiguousTestimony;
        }

        public static List<PersonalLetterEntry> GetLettersForRoom(PersonalLetterCatalog catalog, string roomId)
        {
            var results = new List<PersonalLetterEntry>();
            if (catalog == null || string.IsNullOrEmpty(roomId)) return results;

            for (int i = 0; i < catalog.AllLetters.Count; i++)
            {
                var l = catalog.AllLetters[i];
                if (string.Equals(ResolveRoomForLetter(l.letter_id), roomId, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public static List<PersonalLetterEntry> GetLettersForTruthClass(PersonalLetterCatalog catalog, LetterTruthClass truthClass)
        {
            var results = new List<PersonalLetterEntry>();
            if (catalog == null) return results;

            for (int i = 0; i < catalog.AllLetters.Count; i++)
            {
                var l = catalog.AllLetters[i];
                if (ResolveTruthClass(l.letter_id) == truthClass)
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public static List<PersonalLetterEntry> GetLettersForType(PersonalLetterCatalog catalog, string letterType)
        {
            var results = new List<PersonalLetterEntry>();
            if (catalog == null || string.IsNullOrEmpty(letterType)) return results;

            for (int i = 0; i < catalog.AllLetters.Count; i++)
            {
                var l = catalog.AllLetters[i];
                if (string.Equals(l.letter_type, letterType, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(l);
                }
            }
            return results;
        }
    }
}
