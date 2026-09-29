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
        private PrisonerSystem? _prisoners;

        // ── Plan 179: Prisoner Management & Interrogation ─────────────────

        public PrisonerSystem EnsurePrisoners()
        {
            if (_prisoners != null) return _prisoners;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("prisoners") : new SeededRng(179);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _prisoners = new PrisonerSystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("interrogation_tactics.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<InterrogationTacticsCatalog>(json);
                        if (catalog?.tactics != null)
                        {
                            foreach (var t in catalog.tactics)
                                _prisoners.RegisterTactic(t);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Prisoners] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = PrisonerSaveStore.TryLoad();
            if (saved != null)
            {
                _prisoners.RestoreState(saved);
            }
            else
            {
                MigrateLegacyShelterPrisoners();
            }

            _prisoners.OnIntelExtracted += (captiveId, intelId, isTrue) =>
            {
                string veracity = isTrue ? "Verified" : "Unconfirmed/Suspect";
                _journal?.TryAddRawEntry("prisoner_intel", $"Interrogation intel recovered from {captiveId} (Report: {intelId}, Status: {veracity}).", null!, _simDay);
            };

            _prisoners.OnPrisonerEscaped += (captiveId) =>
            {
                _journal?.TryAddRawEntry("prison_break", $"Security alert: Captive {captiveId} has breached confinement and escaped into the wasteland!", null!, _simDay);
            };

            _prisoners.OnPrisonerRecruited += (captiveId) =>
            {
                _journal?.TryAddRawEntry("captive_recruited", $"Rehabilitation success: Former captive {captiveId} has formally sworn allegiance to the holdfast.", null!, _simDay);
            };

            return _prisoners;
        }

        /// <summary>
        /// ORPHAN-SEAL-W1 (2026-09-23): one-time import of Plan 63's retired
        /// shelter_prisoners section into the single captive ledger. Runs only
        /// when no canonical save exists yet; terminal records (recruited /
        /// paroled) are not resurrected. The old section is left on disk
        /// untouched and is no longer written.
        /// </summary>
        private void MigrateLegacyShelterPrisoners()
        {
            if (_prisoners == null) return;
            var legacy = ShelterPrisonerSaveStore.TryLoad();
            if (legacy?.Prisoners == null || legacy.Prisoners.Count == 0) return;
            int imported = 0;
            for (int i = 0; i < legacy.Prisoners.Count; i++)
            {
                var record = legacy.Prisoners[i];
                if (record == null || string.IsNullOrEmpty(record.PrisonerId)) continue;
                if (record.Status == Ashfall.Core.Shelter.PrisonerStatus.Paroled
                    || record.Status == Ashfall.Core.Shelter.PrisonerStatus.Escaped
                    || record.Status == Ashfall.Core.Shelter.PrisonerStatus.Deceased) continue;
                string sourceFaction = string.IsNullOrEmpty(record.FactionOrigin)
                    ? "faction_unknown"
                    : record.FactionOrigin;
                if (_prisoners.TakePrisoner(record.PrisonerId, sourceFaction, Math.Max(1, _simDay)))
                    imported++;
            }
            if (imported > 0)
            {
                _journal?.TryAddRawEntry("prisoner_legacy_migration",
                    $"{imported} captive record(s) from the retired Plan 63 cells were moved into the single prisoner ledger.",
                    null!, Math.Max(1, _simDay));
            }
        }

        private void SetupPrisoners()
        {
            EnsurePrisoners();
        }

        private void SavePrisoners()
        {
            if (_prisoners != null)
            {
                CaptureSection("prisoner_management", PrisonerSaveStore.TryCapturePersisted(_prisoners.CaptureState()));
            }
        }

        private void HandlePrisonerAction(string action, string param)
        {
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                SetupPrisoners();
                _prisonerPanel.Bind(EnsurePrisoners());
                _prisonerPanel.Open();
                return;
            }
            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                _prisonerPanel.Close();
                return;
            }

            SetupPrisoners();
            var system = EnsurePrisoners();
            if (string.IsNullOrWhiteSpace(param))
            {
                _prisonerPanel.ShowFeedback("No detained captive selected.", isFailure: true);
                _prisonerPanel.RefreshView();
                return;
            }

            switch (action)
            {
                case "interrogate":
                {
                    string? tacticId = system.FirstTacticId;
                    if (string.IsNullOrEmpty(tacticId))
                    {
                        _prisonerPanel.ShowFeedback("No interrogation tactics registered.", isFailure: true);
                        break;
                    }
                    var result = system.Interrogate(param, tacticId, _simDay);
                    string successMsg = result.IntelDiscovered
                        ? (result.IsFalseIntel
                            ? $"Interrogation yielded a lead ({result.ExtractedIntelId}) — authenticity uncertain."
                            : $"Interrogation extracted intel {result.ExtractedIntelId}.")
                        : "Interrogation complete — no new intel this round.";
                    _prisonerPanel.ShowFeedback(
                        result.Success ? successMsg : $"Interrogation failed: {result.FailureCode}",
                        isFailure: !result.Success);
                    break;
                }
                case "recruit":
                {
                    bool ok = system.RecruitPrisoner(param, _simDay);
                    _prisonerPanel.ShowFeedback(
                        ok ? $"Captive {param} recruited into the holdfast." : "Recruitment refused — trust, time, or abuse history blocks it.",
                        isFailure: !ok);
                    break;
                }
                case "release":
                {
                    bool ok = system.ReleasePrisoner(param);
                    _prisonerPanel.ShowFeedback(
                        ok ? $"Captive {param} released." : "Release failed — captive not detained.",
                        isFailure: !ok);
                    break;
                }
                default:
                    _prisonerPanel.ShowFeedback($"Unknown detention action: {action}", isFailure: true);
                    break;
            }
            _prisonerPanel.RefreshView();
        }

    }
}
