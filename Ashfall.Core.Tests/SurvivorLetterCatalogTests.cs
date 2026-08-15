using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class SurvivorLetterCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void SurvivorLetters_LoadsAll25CanonicalLetters()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "survivor_letters_lost_kin.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new SurvivorLetterCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(25, catalog.AllLetters.Count);

            // Test first letter (Dmitri to Mother in Leningrad)
            var l1 = catalog.GetById("letter_01_dmitri_to_mother_in_leningrad");
            Assert.NotNull(l1);
            Assert.Equal("SLOT-01-NORTH", l1.pigeonhole_number);
            Assert.Equal("Chief Engineer Dmitri Morozov", l1.author_dweller);
            Assert.Contains("Fontanka Embankment", l1.destination_address);
            Assert.Contains("lingonberry jam", l1.letter_text);
            Assert.Contains("Valdai Hills", l1.galina_dead_letter_note);

            // Test author search
            var sonyaLetters = catalog.GetByAuthor("Sonya");
            Assert.True(sonyaLetters.Count >= 2); // Sonya Vel (Age 7), Sonya Vel (Age 14)

            // Test destination search
            var moscowLetters = catalog.GetByDestination("Moscow");
            Assert.True(moscowLetters.Count >= 2); // Dr. Vel, Galina

            // Test finale letter (Postmaster's General Manifesto)
            var l25 = catalog.GetById("letter_25_the_final_postmasters_general_manifesto");
            Assert.NotNull(l25);
            Assert.Equal("SLOT-25-DESTINY", l25.pigeonhole_number);
            Assert.Contains("Dead Letter Office is hereby dissolved", l25.letter_text);
            Assert.Contains("The sun was warm", l25.galina_dead_letter_note);

            // Test tag search
            var family = catalog.GetByTag("family");
            Assert.True(family.Count >= 6);
        }

        [Fact]
        public void SurvivorLetters_AllEntriesHaveValidFieldsAndUniquePigeonholes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "survivor_letters_lost_kin.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new SurvivorLetterCatalog();
            catalog.Load(json, serializer);

            var seenSlots = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var l in catalog.AllLetters)
            {
                Assert.False(string.IsNullOrWhiteSpace(l.letter_id), "Missing letter_id");
                Assert.False(string.IsNullOrWhiteSpace(l.pigeonhole_number), $"Missing slot on {l.letter_id}");
                Assert.True(seenSlots.Add(l.pigeonhole_number), $"Duplicate slot: {l.pigeonhole_number}");

                Assert.False(string.IsNullOrWhiteSpace(l.author_dweller), $"Missing author on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.intended_recipient), $"Missing recipient on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.destination_address), $"Missing address on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.dispatch_attempt_date), $"Missing date on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.envelope_condition), $"Missing condition on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.letter_text), $"Missing letter text on {l.letter_id}");
                Assert.True(l.letter_text.Length > 40, $"Letter text too brief on {l.letter_id}");
                Assert.False(string.IsNullOrWhiteSpace(l.galina_dead_letter_note), $"Missing dead letter note on {l.letter_id}");
                Assert.True(l.galina_dead_letter_note.Length > 25, $"Dead letter note too brief on {l.letter_id}");
                Assert.NotNull(l.tags);
                Assert.True(l.tags.Length > 0, $"Tags empty on {l.letter_id}");
            }
        }
    }
}
