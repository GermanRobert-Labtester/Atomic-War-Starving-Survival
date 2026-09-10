using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PersonalLetterCatalogTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void TEST_LTR_01_PersonalLetters_LoadsAll25AuthoredLetters()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(25, catalog.AllLetters.Count);

            // Verify letter_01 (Mother's coat)
            var l1 = catalog.GetById("letter_01_to_mother");
            Assert.NotNull(l1);
            Assert.Equal(12, l1.day);
            Assert.Equal("a_daughter", l1.sender);
            Assert.Equal("Mother", l1.recipient);
            Assert.Equal("the shelter, by the lamp", l1.location);
            Assert.Equal("unsent", l1.letter_type);
            Assert.Equal("aching", l1.tone);
            Assert.Contains("coat", l1.tags);
            Assert.Contains("Mama", l1.content);

            // Verify letter_25 (One sentence)
            var l25 = catalog.GetById("letter_25_one_sentence");
            Assert.NotNull(l25);
            Assert.Equal(3, l25.day);
            Assert.Equal("a_mother", l25.sender);
            Assert.Equal("my daughter", l25.recipient);
            Assert.Equal("fierce", l25.tone);
            Assert.Contains("coat", l25.tags);
            Assert.Contains("going", l25.tags);
            Assert.Contains("Take the coat and go now", l25.content);
        }

        [Fact]
        public void TEST_LTR_02_PersonalLetters_AllEntriesHaveValidFieldsAndUniqueIds()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var l in catalog.AllLetters)
            {
                Assert.False(string.IsNullOrWhiteSpace(l.letter_id), "Missing letter_id");
                Assert.True(seenIds.Add(l.letter_id), $"Duplicate letter_id: {l.letter_id}");
                Assert.True(l.day > 0, $"Invalid day on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.sender), $"Missing sender on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.recipient), $"Missing recipient on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.location), $"Missing location on {l.letter_id}");
                Assert.True(l.letter_type == "unsent" || l.letter_type == "delivered",
                    $"Unexpected letter_type '{l.letter_type}' on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.content), $"Missing content on {l.letter_id}");
                Assert.NotNull(l.tags);
                Assert.True(l.tags.Length > 0, $"Tags empty on {l.letter_id}");
                Assert.NotNull(l.key_themes);
            }
        }

        [Fact]
        public void TEST_LTR_03_PersonalLetters_Load_IsIdempotent()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();

            catalog.Load(json, serializer);
            Assert.Equal(25, catalog.AllLetters.Count);

            // Re-load same payload must not duplicate entries
            catalog.Load(json, serializer);
            Assert.Equal(25, catalog.AllLetters.Count);
        }

        [Fact]
        public void TEST_LTR_04_PersonalLetters_LoadFromDirectory_ResolvesCanonicalFile()
        {
            string dataDir = FindDataDir();
            var catalog = PersonalLetterCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(catalog.AllLetters.Count >= 25);
            Assert.NotNull(catalog.GetById("letter_01_to_mother"));
            Assert.NotNull(catalog.GetById("letter_25_one_sentence"));
        }

        [Fact]
        public void TEST_LTR_05_PersonalLetters_GetByType_FiltersDeliveredAndUnsent()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            var delivered = catalog.GetByType("delivered");
            var unsent = catalog.GetByType("unsent");

            Assert.Equal(9, delivered.Count);
            Assert.Equal(16, unsent.Count);
            Assert.Equal(25, delivered.Count + unsent.Count);

            foreach (var l in delivered)
            {
                Assert.Equal("delivered", l.letter_type, ignoreCase: true);
            }
            foreach (var l in unsent)
            {
                Assert.Equal("unsent", l.letter_type, ignoreCase: true);
            }
        }

        [Fact]
        public void TEST_LTR_06_PersonalLetters_GetBySenderAndRecipient_CaseInsensitive()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            var daughters = catalog.GetBySender("daughter");
            Assert.NotEmpty(daughters);
            Assert.Contains(daughters, l => l.letter_id == "letter_01_to_mother");

            var quartermasterSender = catalog.GetBySender("QUARTERMASTER");
            Assert.Single(quartermasterSender);
            Assert.Equal("letter_17_to_the_engineer", quartermasterSender[0].letter_id);

            var quartermasterRecipient = catalog.GetByRecipient("QUARTERMASTER");
            Assert.Single(quartermasterRecipient);
            Assert.Equal("letter_08_confession_theft", quartermasterRecipient[0].letter_id);

            var pavel = catalog.GetByRecipient("pavel");
            Assert.Single(pavel);
            Assert.Equal("letter_02_to_son", pavel[0].letter_id);

            var loverM = catalog.GetByRecipient("M.");
            Assert.Equal(2, loverM.Count);
            Assert.Contains(loverM, l => l.letter_id == "letter_04_to_lover_returning");
            Assert.Contains(loverM, l => l.letter_id == "letter_24_love_without_the_word");
        }

        [Fact]
        public void TEST_LTR_07_PersonalLetters_GetBySearch_FindsKeywords()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            var coats = catalog.GetBySearch("coat");
            Assert.True(coats.Count >= 2); // letter_01, letter_25

            var watches = catalog.GetBySearch("watch");
            Assert.NotEmpty(watches);

            var flours = catalog.GetBySearch("flour");
            Assert.NotEmpty(flours);

            var potatoes = catalog.GetBySearch("potato");
            Assert.NotEmpty(potatoes);

            Assert.Empty(catalog.GetBySearch("gibberish_term_xyz_12345"));
        }

        [Fact]
        public void TEST_LTR_08_PersonalLetters_Projection_MapsCanonicalRooms()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            var validRooms = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "room_bunker_corridor", "room_water_pump", "room_workshop",
                "room_filtration", "room_greenhouse", "room_airlock",
                "room_storage_bay", "room_clinic", "room_kitchen",
                "room_bunks", "room_main"
            };

            foreach (var l in catalog.AllLetters)
            {
                string roomId = PersonalLetterProjection.ResolveRoomForLetter(l.letter_id);
                Assert.True(validRooms.Contains(roomId), $"Letter {l.letter_id} mapped to invalid room {roomId}");
            }

            var bunkLetters = PersonalLetterProjection.GetLettersForRoom(catalog, "room_bunks");
            Assert.Equal(5, bunkLetters.Count); // letter_01, letter_04, letter_05, letter_15, letter_24
        }

        [Fact]
        public void TEST_LTR_09_PersonalLetters_Projection_ClassifiesTruthAndProvenance()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            foreach (var l in catalog.AllLetters)
            {
                var truthClass = PersonalLetterProjection.ResolveTruthClass(l.letter_id);
                Assert.True(Enum.IsDefined(typeof(LetterTruthClass), truthClass),
                    $"Letter {l.letter_id} has undefined truth class {truthClass}");
            }

            var unsentArtifacts = PersonalLetterProjection.GetLettersForTruthClass(catalog, LetterTruthClass.UnsentPrivateArtifact);
            Assert.Equal(9, unsentArtifacts.Count);

            var delivered = PersonalLetterProjection.GetLettersForTruthClass(catalog, LetterTruthClass.DeliveredCorrespondence);
            Assert.Equal(7, delivered.Count);

            var historical = PersonalLetterProjection.GetLettersForTruthClass(catalog, LetterTruthClass.HistoricalTestimony);
            Assert.Equal(3, historical.Count);

            var contemporary = PersonalLetterProjection.GetLettersForTruthClass(catalog, LetterTruthClass.CampaignContemporary);
            Assert.Equal(4, contemporary.Count);

            var ambiguous = PersonalLetterProjection.GetLettersForTruthClass(catalog, LetterTruthClass.AmbiguousTestimony);
            Assert.Equal(2, ambiguous.Count);
        }

        [Fact]
        public void TEST_LTR_10_PersonalLetters_Projection_ZeroMutationContract()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            int initialCount = catalog.AllLetters.Count;

            // Execute intensive read and projection operations
            for (int i = 0; i < initialCount; i++)
            {
                var letter = catalog.AllLetters[i];
                string room = PersonalLetterProjection.ResolveRoomForLetter(letter.letter_id);
                var truth = PersonalLetterProjection.ResolveTruthClass(letter.letter_id);
                var search = catalog.GetBySearch(letter.sender);
                var byType = catalog.GetByType(letter.letter_type);
                Assert.NotNull(room);
                Assert.NotNull(search);
                Assert.NotNull(byType);
            }

            // Verify zero mutation of catalog data
            Assert.Equal(initialCount, catalog.AllLetters.Count);
        }

        [Fact]
        public void TEST_LTR_11_PersonalLetters_DeterministicOrdering()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();
            catalog.Load(json, serializer);

            var ascending = catalog.GetSortedByDay(ascending: true);
            Assert.Equal(25, ascending.Count);
            for (int i = 0; i < ascending.Count - 1; i++)
            {
                Assert.True(ascending[i].day <= ascending[i + 1].day,
                    $"Ascending order violated at {i}: {ascending[i].day} > {ascending[i + 1].day}");
            }

            var descending = catalog.GetSortedByDay(ascending: false);
            Assert.Equal(25, descending.Count);
            for (int i = 0; i < descending.Count - 1; i++)
            {
                Assert.True(descending[i].day >= descending[i + 1].day,
                    $"Descending order violated at {i}: {descending[i].day} < {descending[i + 1].day}");
            }
        }

        [Fact]
        public void TEST_LTR_12_PersonalLetters_Clear_ResetsCatalog()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "letters_expansion.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new PersonalLetterCatalog();

            catalog.Load(json, serializer);
            Assert.Equal(25, catalog.AllLetters.Count);

            catalog.Clear();
            Assert.Equal(0, catalog.AllLetters.Count);
            Assert.Null(catalog.GetById("letter_01_to_mother"));

            catalog.Load(json, serializer);
            Assert.Equal(25, catalog.AllLetters.Count);
        }
    }
}
