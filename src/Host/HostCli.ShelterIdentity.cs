// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 166 (Shelter Identity, Naming & Origin).

using System;
using System.IO;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliShelterIdentity
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Shelter Identity & Naming Self-Test (Plan 166) ===");
            int passed = 0;
            int total = 12;

            try
            {
                string path = Path.Combine(dataDir, "shelter_origins.json");

                // Check 1: authored catalog loads through the strict loader
                ShelterOriginsCatalogJson? catalog = null;
                if (File.Exists(path))
                    catalog = ShelterOriginCatalogLoader.LoadFromJson(File.ReadAllText(path));
                if (catalog != null && catalog.origins.Count >= 6)
                {
                    GD.Print($"[PASS] Check 1: Strict loader accepted {catalog.origins.Count} authored origins.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 1: Authored shelter-origins catalog missing or incomplete.");
                }

                // Check 2: strict loader rejects malformed authoring
                if (ExpectReject("{'schema_version':1,'origins':[{'origin_id':'o1','display_name':'O1','starting_bonuses':['b']},{'origin_id':'o1','display_name':'O2','starting_bonuses':['b']}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'origins':[{'origin_id':'o1','display_name':'','starting_bonuses':['b']}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'origins':[{'origin_id':'o1','display_name':'O1','starting_bonuses':['b'],'radiation_shielding_bp':99999}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':2,'origins':[{'origin_id':'o1','display_name':'O1','starting_bonuses':['b']}]}".Replace('\'', '"')))
                {
                    GD.Print("[PASS] Check 2: Strict loader rejected duplicate id, empty name, out-of-range bp, and a future schema.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: A malformed origin catalog was accepted.");
                }

                // Bind the session from the authored catalog.
                var host = ShelterIdentityHostSession.Create(dataDir);

                // Check 3: authored origins replaced the built-in set
                if (host.System.Origins.Count >= 6 && host.GetSelectedOrigin() == null)
                {
                    GD.Print($"[PASS] Check 3: Session holds {host.System.Origins.Count} origins and no origin is pre-selected.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Origin binding wrong ({host.System.Origins.Count} origins).");
                }

                // Check 4: name validation
                bool shortRejected = host.SetShelterName("A").Status != Ashfall.Core.ActionResult.StatusKind.Success;
                bool longRejected = host.SetShelterName(new string('x', 41)).Status != Ashfall.Core.ActionResult.StatusKind.Success;
                bool nameSet = host.SetShelterName("Vault 41").Status == Ashfall.Core.ActionResult.StatusKind.Success;
                if (shortRejected && longRejected && nameSet && host.ShelterName == "Vault 41")
                {
                    GD.Print("[PASS] Check 4: Name bounds enforced (2..40) and a valid name applied.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Name validation wrong (short={shortRejected}, long={longRejected}, set={nameSet}).");
                }

                // Check 5: motto + emblem validation
                bool mottoTooLong = host.SetMotto(new string('m', 121)).Status != Ashfall.Core.ActionResult.StatusKind.Success;
                bool mottoSet = host.SetMotto("We keep the lights on").Status == Ashfall.Core.ActionResult.StatusKind.Success;
                bool emblemBad = host.SetEmblem("", "amber").Status != Ashfall.Core.ActionResult.StatusKind.Success;
                bool emblemSet = host.SetEmblem("Gear", "Amber").Status == Ashfall.Core.ActionResult.StatusKind.Success;
                if (mottoTooLong && mottoSet && emblemBad && emblemSet)
                {
                    GD.Print("[PASS] Check 5: Motto length and emblem validation enforced.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Motto/emblem validation wrong.");
                }

                // Check 6: origin selection gates on authored ids
                bool unknownRejected = host.SelectOrigin("origin_does_not_exist", 1).Status != Ashfall.Core.ActionResult.StatusKind.Success;
                bool selected = host.SelectOrigin("origin_mining_facility", 3, "survivor_ada").Status == Ashfall.Core.ActionResult.StatusKind.Success;
                if (unknownRejected && selected && host.OriginId == "origin_mining_facility")
                {
                    GD.Print("[PASS] Check 6: Unknown origin rejected; authored origin accepted on day 3.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Origin selection wrong (id={host.OriginId}).");
                }

                // Check 7: origin projection exposes authored basis points
                var origin = host.GetSelectedOrigin();
                if (origin != null && origin.radiation_shielding_bp == 1000 && origin.space_modifier_bp == 2500)
                {
                    GD.Print($"[PASS] Check 7: Selected origin projects its authored modifiers (rad={origin.radiation_shielding_bp}bp).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Origin projection missing authored modifiers.");
                }

                // Check 8: community actions drive known-for tags and infamy
                for (int i = 0; i < 5; i++) host.RecordCommunityAction("trade");
                for (int i = 0; i < 5; i++) host.RecordCommunityAction("medical");
                var tags = host.GetKnownForTags();
                if (tags.Contains("Traders") && tags.Contains("Healers") && host.Infamy == 0)
                {
                    GD.Print($"[PASS] Check 8: Community actions produced tags [{string.Join(", ", tags)}] with infamy {host.Infamy}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Known-for projection wrong ([{string.Join(", ", tags)}], infamy={host.Infamy}).");
                }

                // Check 9: raid increments infamy and can be offset by medical
                int before = host.Infamy;
                host.RecordCommunityAction("raid", 4);
                host.RecordCommunityAction("medical", 2);
                if (host.Infamy == before + 8 - 2)
                {
                    GD.Print($"[PASS] Check 9: Infamy moved {before} -> {host.Infamy} from raid/medical actions.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Infamy arithmetic wrong ({before} -> {host.Infamy}).");
                }

                // Check 10: narrative token interpolation
                string text = host.FormatText("{shelter_name} / {motto} / {origin_name} / {founder}");
                if (text.Contains("Vault 41") && text.Contains("We keep the lights on")
                    && text.Contains("Subterranean Mining Facility") && text.Contains("survivor_ada"))
                {
                    GD.Print($"[PASS] Check 10: Token interpolation resolved all placeholders: \"{text}\".");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Token interpolation wrong: \"{text}\".");
                }

                // Check 11: census + capture/restore round-trip
                var state = host.CaptureState();
                var fresh = ShelterIdentityHostSession.Create(dataDir);
                fresh.RestoreState(state);
                var census = fresh.Census;
                if (fresh.ShelterName == "Vault 41"
                    && fresh.OriginId == "origin_mining_facility"
                    && census.OriginCount >= 6
                    && census.KnownForTagCount >= 2
                    && census.Infamy == host.Infamy)
                {
                    GD.Print($"[PASS] Check 11: Capture/restore preserved identity (origins={census.OriginCount}, tags={census.KnownForTagCount}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Capture/restore lost identity ({census.OriginCount} origins).");
                }

                // Check 12: schema gate rejects a newer payload
                bool gated = false;
                try
                {
                    var newer = host.CaptureState();
                    newer.schema_version = 99;
                    fresh.RestoreState(newer);
                }
                catch (InvalidOperationException)
                {
                    gated = true;
                }
                if (gated)
                {
                    GD.Print("[PASS] Check 12: RestoreState rejected an unsupported future schema.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: RestoreState accepted an unsupported schema.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in shelter identity self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Shelter Identity Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static bool ExpectReject(string json)
        {
            try
            {
                ShelterOriginCatalogLoader.LoadFromJson(json);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}
