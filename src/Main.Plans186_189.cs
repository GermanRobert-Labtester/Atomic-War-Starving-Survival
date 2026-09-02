// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 186-189 Host Wire & Orchestration
// Subsystems   : Radioactive Fallout, Cannibalism & Desperation,
//                Mercenary Bounties, Archaeology & Lore Excavation
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.Survivors;
using Ashfall.Core.Economy;
using Ashfall.Core.Archaeology;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private FalloutSystem? _fallout;
        private DesperationSystem? _desperation;
        private MercenarySystem? _mercenary;
        private ArchaeologySystem? _archaeology;

        // ── Plan 186: Radioactive Fallout & Wind Dispersal ────────────────

        public FalloutSystem EnsureFallout()
        {
            if (_fallout != null) return _fallout;

            _fallout = new FalloutSystem(new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/fallout_patterns.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<FalloutCatalogContainer>(json);
                        if (container?.patterns != null)
                        {
                            foreach (var p in container.patterns)
                                _fallout.RegisterPattern(p);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Fallout] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = FalloutSaveStore.TryLoad();
            if (saved != null)
            {
                _fallout.RestoreState(saved);
            }

            _fallout.OnFalloutWarning += (cloud, zone, dist) =>
            {
                _journal?.TryAddRawEntry("fallout_warning", $"Radioactive fallout warning: {cloud.patternId} approaching {zone} (Distance: {dist:F1}km).", null!, _simDay);
            };

            _fallout.OnGroundwaterTainted += (zone) =>
            {
                _journal?.TryAddRawEntry("groundwater_taint", $"Prolonged fallout deposit has contaminated water table at {zone}!", null!, _simDay);
            };

            return _fallout;
        }

        private void SetupFallout()
        {
            EnsureFallout();
        }

        private void SaveFallout()
        {
            if (_fallout != null)
            {
                CaptureSection("fallout", FalloutSaveStore.TryCapturePersisted(_fallout.State));
            }
        }

        // ── Plan 187: Cannibalism & Desperation Mechanics ─────────────────

        public DesperationSystem EnsureDesperation()
        {
            if (_desperation != null) return _desperation;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("desperation") : new SeededRng(187);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs ?? new Ashfall.Core.Survivors.NeedsSystem();
            var disease = _disease?.Engine;

            _desperation = new DesperationSystem(rng, inv, needs, disease, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/desperation_events.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<DesperationCatalogContainer>(json);
                        if (container?.events != null)
                        {
                            foreach (var ev in container.events)
                                _desperation.RegisterEvent(ev);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Desperation] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = DesperationSaveStore.TryLoad();
            if (saved != null)
            {
                _desperation.RestoreState(saved);
            }

            _desperation.OnTabooBroken += (record) =>
            {
                _journal?.TryAddRawEntry("taboo_broken", $"SURVIVAL TABOO BROKEN: Dweller {record.actorId} harvested fallen dweller {record.corpseId}.", null!, _simDay);
            };

            return _desperation;
        }

        private void SetupDesperation()
        {
            EnsureDesperation();
        }

        private void SaveDesperation()
        {
            if (_desperation != null)
            {
                CaptureSection("desperation", DesperationSaveStore.TryCapturePersisted(_desperation.State));
            }
        }

        // ── Plan 188: Bounties & Mercenary Contracts ──────────────────────

        public MercenarySystem EnsureMercenary()
        {
            if (_mercenary != null) return _mercenary;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("mercenary") : new SeededRng(188);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _mercenary = new MercenarySystem(rng, inv, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/bounty_board.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<BountyCatalogContainer>(json);
                        if (container?.templates != null)
                        {
                            foreach (var t in container.templates)
                                _mercenary.RegisterTemplate(t);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Mercenary] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = MercenarySaveStore.TryLoad();
            if (saved != null)
            {
                _mercenary.RestoreState(saved);
            }

            _mercenary.OnBountyClaimed += (contract) =>
            {
                _journal?.TryAddRawEntry("bounty_claimed", $"Bounty contract {contract.contractId} fulfilled. Reward: {contract.rewardAmount} scrap.", null!, _simDay);
            };

            _mercenary.OnMercenaryBetrayed += (contract) =>
            {
                _journal?.TryAddRawEntry("mercenary_betrayal", $"Hired mercenaries betrayed contract {contract.contractId} on target {contract.targetId}!", null!, _simDay);
            };

            return _mercenary;
        }

        private void SetupMercenary()
        {
            EnsureMercenary();
        }

        private void SaveMercenary()
        {
            if (_mercenary != null)
            {
                CaptureSection("mercenary_bounties", MercenarySaveStore.TryCapturePersisted(_mercenary.State));
            }
        }

        // ── Plan 189: Archaeology & Lore Excavation ───────────────────────

        public ArchaeologySystem EnsureArchaeology()
        {
            if (_archaeology != null) return _archaeology;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("archaeology") : new SeededRng(189);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var research = EnsureSharedResearch();

            _archaeology = new ArchaeologySystem(rng, inv, research, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/lore_archives.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<ArchaeologyCatalogContainer>(json);
                        if (container?.archives != null)
                        {
                            foreach (var a in container.archives)
                                _archaeology.RegisterArchive(a);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Archaeology] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = ArchaeologySaveStore.TryLoad();
            if (saved != null)
            {
                _archaeology.RestoreState(saved);
            }

            _archaeology.OnLoreUnlocked += (archive, points) =>
            {
                _journal?.TryAddRawEntry("lore_unlocked", $"Pre-war archive '{archive.titleKey}' successfully decrypted (+{points} R&D).", null!, _simDay);
            };

            return _archaeology;
        }

        private void SetupArchaeology()
        {
            EnsureArchaeology();
        }

        private void SaveArchaeology()
        {
            if (_archaeology != null)
            {
                CaptureSection("archaeology", ArchaeologySaveStore.TryCapturePersisted(_archaeology.State));
            }
        }

        // ── Daily / Hourly Tick Coordination ─────────────────────────────

        public void TickPlans186_189(int day, float deltaHours)
        {
            if (_fallout != null)
            {
                var zones = new Dictionary<string, (float x, float y)>(StringComparer.Ordinal)
                {
                    { "loc_holdfast", (0f, 0f) },
                    { "loc_river_delta", (25f, -10f) },
                    { "loc_silo_ruins", (-30f, 40f) },
                    { "loc_chemical_plant", (15f, 35f) }
                };
                _fallout.Tick(deltaHours, 45.0f, 15.0f, zones);
            }

            _mercenary?.TickDay(day);
        }
    }
}
