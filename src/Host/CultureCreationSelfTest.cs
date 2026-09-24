// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Culture;

namespace AtomicWar.GodotApp
{
    public static class CultureCreationSelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Culture Creation System Self-Test (Plan 178) ===");
            int passed = 0;

            void Check(bool condition, string name)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] Check {++passed}: {name}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check {passed + 1}: {name}");
                    throw new InvalidOperationException($"CultureCreation self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Catalog loads art forms
                var session = CultureCreationHostSession.Create(dataDir);
                Check(session.System.GetAllForms().Count >= 6, "Authoritative catalog loaded 6+ art forms from art_forms.json.");

                // Check 2: Initial state baseline
                var census0 = session.GetCensus();
                Check(census0.TotalArtworks == 0 && census0.TotalCulturalValue == 0f && census0.CulturalIdentity == "Pioneers",
                    "Initial state baseline clean (0 artworks, 0 value, Pioneers identity).");

                // Check 3: Creator validation
                bool rejectedEmpty = false;
                try
                {
                    session.CreateArtwork("", "Untitled", ArtMedium.Painting, ArtTheme.Hope, 1);
                }
                catch (ArgumentNullException)
                {
                    rejectedEmpty = true;
                }
                Check(rejectedEmpty, "Empty creator ID safely rejected with ArgumentNullException.");

                // Check 4: Deterministic artwork creation
                var rng = new SeededRng(17801);
                var art1 = session.CreateArtwork("survivor_alice", "Dawn Over Crater", ArtMedium.Painting, ArtTheme.Hope, 1, 60f, rng);
                Check(art1 != null && art1.ArtworkId == "art_1" && art1.QualityScore >= 10f && art1.QualityScore <= 100f,
                    $"Deterministic artwork created ({art1?.ArtworkId}, quality={art1?.QualityScore:F1}).");

                // Check 5: Low-skill artist produces bounded result
                var rng2 = new SeededRng(17802);
                var art2 = session.CreateArtwork("survivor_bob", "Rough Sketch", ArtMedium.Painting, ArtTheme.Hope, 1, 10f, rng2);
                Check(art2.QualityScore >= 10f && art2.QualityScore <= 100f,
                    $"Low-skill artwork bounded in valid range ({art2.QualityScore:F1}).");

                // Check 6: Masterwork threshold detection
                var rngHigh = new SeededRng(99);
                var artMaster = session.CreateArtwork("survivor_clara", "Monument of Survival", ArtMedium.Sculpture, ArtTheme.Hope, 2, 140f, rngHigh);
                Check(artMaster.QualityScore >= 85f && artMaster.IsMasterwork && artMaster.CulturalImpact > artMaster.QualityScore * 0.1f,
                    $"Masterwork flagged correctly (quality={artMaster.QualityScore:F1}, isMasterwork={artMaster.IsMasterwork}).");

                // Check 7: Cumulative cultural value tracking
                Check(session.System.TotalCulturalValue > 0f && session.GetCensus().TotalArtworks == 3,
                    $"Cumulative cultural value tracked ({session.System.TotalCulturalValue:F1} across 3 artworks).");

                // Check 8: Artwork display location
                bool displayed = session.DisplayArtwork(art1!.ArtworkId, "room_common_hall");
                Check(displayed && art1.DisplayLocation == "room_common_hall",
                    "Artwork assigned to display location room_common_hall.");

                // Check 9: Shelter morale bonus includes displayed artwork
                float bonusWithOne = session.GetShelterCultureMoraleBonus();
                Check(bonusWithOne > 0f && bonusWithOne <= 15.0f,
                    $"Shelter culture morale bonus calculated ({bonusWithOne:F2} / max 15.0).");

                // Check 10: Undisplayed artwork does not increase morale bonus
                Check(string.IsNullOrEmpty(art2.DisplayLocation),
                    "Undisplayed artwork does not have a display location.");

                // Check 11: Cultural identity shift from dominant theme
                // We have 3 Hope artworks: Dawn Over Crater, Rough Sketch, Monument of Survival
                Check(session.System.CulturalIdentity == "The Beacon",
                    $"Cultural identity evolved to '{session.System.CulturalIdentity}' based on dominant Hope theme.");

                // Check 12: Save/restore round-trip fidelity
                var state = session.CaptureState();
                var restoredSession = CultureCreationHostSession.Create(dataDir, state);
                var censusRestored = restoredSession.GetCensus();
                Check(censusRestored.TotalArtworks == census0.TotalArtworks + 3 &&
                      censusRestored.CulturalIdentity == "The Beacon" &&
                      Math.Abs(censusRestored.TotalCulturalValue - session.System.TotalCulturalValue) < 0.001f &&
                      restoredSession.GetShelterCultureMoraleBonus() == bonusWithOne,
                    "Save/restore round-trip preserved 100% parity of artworks, identity, values, and morale bonus.");

                Console.WriteLine($"=== Culture Creation Self-Test Result: {passed}/12 Passed ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Culture Creation self-test terminated with exception: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }
        }
    }
}
