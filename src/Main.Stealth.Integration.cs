// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private StealthSystem? _stealth;

        // ── Plan 181: Stealth & Camouflage Mechanics ──────────────────────

        public StealthSystem EnsureStealth()
        {
            if (_stealth != null) return _stealth;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("stealth") : new SeededRng(181);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _stealth = new StealthSystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("camouflage_gear.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<CamouflageGearCatalog>(json);
                        if (catalog?.gear != null)
                        {
                            foreach (var g in catalog.gear)
                                _stealth.RegisterCamouflageGear(g);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Stealth] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            _stealth.RegisterWeaponNoise(new WeaponNoiseProfile
            {
                weapon_id = "weapon_assault_rifle",
                handling_noise = 0.12f,
                melee_noise = 0.18f,
                fired_noise = 0.85f,
                is_suppressed = false
            });
            _stealth.RegisterWeaponNoise(new WeaponNoiseProfile
            {
                weapon_id = "weapon_pipe_rifle",
                handling_noise = 0.16f,
                melee_noise = 0.22f,
                fired_noise = 0.92f,
                is_suppressed = false
            });
            _stealth.RegisterWeaponNoise(new WeaponNoiseProfile
            {
                weapon_id = "weapon_suppressed_rifle",
                handling_noise = 0.10f,
                melee_noise = 0.18f,
                fired_noise = 0.28f,
                is_suppressed = true
            });

            var saved = StealthSaveStore.TryLoad();
            if (saved != null)
            {
                _stealth.RestoreState(saved);
            }

            _stealth.OnStealthBroken += (expeditionId, reason) =>
            {
                _journal?.TryAddRawEntry("stealth_broken", $"Expedition {expeditionId} had its concealment broken! Trigger: {reason}.", null!, _simDay);
            };

            return _stealth;
        }

        private void SetupStealth()
        {
            EnsureStealth();
        }

        private void SaveStealth()
        {
            if (_stealth != null)
            {
                CaptureSection("expedition_stealth", StealthSaveStore.TryCapturePersisted(_stealth.CaptureState()));
            }
        }

    }
}
