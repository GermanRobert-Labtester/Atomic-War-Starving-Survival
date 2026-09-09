// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 155 Host Wire & Orchestration
// Subsystem    : Oral Lore — songs, shanties, hymns & performance
// Contract     : Cultural knowledge only (Plan 155 §4). Discovery rides the
//                live ExpeditionSystem location-discovery event (expedition/
//                travel producers) and the oral-lore codex surface; the
//                journal is the single first-heard feedback strip. No morale
//                engine, no healing, no route reveals, no faction claims.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using AtomicWar.GodotApp.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private OralLoreHostSession? _oralLore;
        private OralLorePerformanceSystem? _oralLorePerformance;
        private bool _oralLorePerformanceDirty;

        // ── Setup ────────────────────────────────────────────────────────

        public OralLorePerformanceSystem EnsureOralLore()
        {
            if (_oralLorePerformance != null) return _oralLorePerformance;

            if (_oralLore == null)
            {
                _oralLore = new OralLoreHostSession();
                _oralLore.LoadCatalogs(_dataDir);
            }

            _oralLorePerformance = new OralLorePerformanceSystem(_oralLore.Catalog);

            foreach (var (loreId, producer) in OralLorePerformanceSystem.DefaultProducerMap())
            {
                if (!_oralLorePerformance.TryRegisterProducer(loreId, producer))
                    GD.PrintErr($"[Main.OralLore] producer registration failed: {loreId} -> {producer}");
            }

            var saved = OralLoreSaveStore.TryLoad();
            if (saved != null)
            {
                _oralLorePerformance.RestoreState(saved);
            }

            // Journal is the single first-heard feedback strip (deduped per
            // piece by the journal's knowledge ledger; idempotent by contract).
            _oralLorePerformance.OnSongFirstHeard += (loreId, producerId, day) =>
            {
                if (_oralLorePerformance == null) return;
                var song = _oralLorePerformance.Catalog.GetById(loreId);
                if (song == null) return;
                string producerLabel = producerId.StartsWith("room_", StringComparison.Ordinal)
                    ? "in the shelter"
                    : producerId.StartsWith("location_", StringComparison.Ordinal)
                        ? "out in the wastes"
                        : "somewhere worth remembering";
                _journal?.TryAddRawEntry(
                    $"oral_lore_first_heard_{loreId}",
                    $"First heard: \"{song.title}\" ({song.genre.ToLowerInvariant()}, {song.TempoSummary()}) — learned {producerLabel}. {song.performance_context}",
                    null!, day);
            };

            // Producer hook: expedition location discovery surfaces the
            // travel/expedition pieces. Idempotent — revisits return empty.
            if (_expeditions != null)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var heard = _oralLorePerformance?.DiscoverFromProducer(locationId, _simDay);
                    if (heard != null && heard.Count > 0)
                        _oralLorePerformanceDirty = true;
                };
            }

            return _oralLorePerformance;
        }

        private void SetupOralLore()
        {
            EnsureOralLore();
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveOralLore()
        {
            if (_oralLorePerformance != null)
            {
                CaptureSection(
                    "oral_lore",
                    OralLoreSaveStore.TryCapturePersisted(_oralLorePerformance.CaptureState()));
            }
        }
    }
}
