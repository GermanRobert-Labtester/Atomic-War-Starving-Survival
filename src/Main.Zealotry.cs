// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 175 — Zealotry host wire
// System       : ZealotrySystem (fictional ideological pressure layer)
// Authority    : Core owns conversion/fervor/dissent/escalation; the host
//                adopts existing friction beliefs, fulfills ritual demands
//                through the canonical inventory, routes belief-crisis morale
//                damage through NeedsSystem (bounded), and persists state.
//                No real-world belief content is referenced anywhere (§1.6).
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ZealotrySystem? _zealotry;

        // ── Plan 175: Fictional Ideological Pressure ────────────────────

        public ZealotrySystem EnsureZealotry()
        {
            if (_zealotry != null) return _zealotry;

            SetupSurvivorSocial();   // friction beliefs are the adoption source

            var profiles = new List<ZealotryBeliefProfile>();
            string catalogPath = CatalogPath.ResolveCatalog("wasteland_religions.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var root = System.Text.Json.JsonSerializer.Deserialize<ZealotryCatalogRoot>(json);
                        if (root?.religions != null)
                        {
                            var load = new ZealotryCatalogLoadResult();
                            foreach (var def in root.religions)
                                if (def != null) load.Religions.Add(def);
                            profiles.AddRange(ZealotryCatalogLoader.ToProfiles(load));
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Zealotry] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            _zealotry = new ZealotrySystem(profiles);

            var saved = ZealotrySaveStore.TryLoad();
            if (saved != null)
            {
                _zealotry.RestoreState(saved);
            }
            else
            {
                // Old-save baseline (§10): adopt ONLY existing friction beliefs —
                // mild adherence, never automatic conversion, never fabricated.
                AdoptExistingFrictionBelievers();
            }

            _zealotry.OnConverted += (b, beliefId) =>
            {
                _journal?.TryAddRawEntry($"zealotry_converted_{b.survivor_id}",
                    $"{b.survivor_id} has come to hold with {beliefId.Replace("belief_", "").Replace('_', ' ')}.",
                    null!, _simDay);
            };
            _zealotry.OnConversionResisted += b =>
            {
                _journal?.TryAddRawEntry($"zealotry_resisted_{b.survivor_id}",
                    $"{b.survivor_id} heard the recruiting words and kept their own counsel.",
                    null!, _simDay);
            };
            _zealotry.OnCrisisStarted += b =>
            {
                _journal?.TryAddRawEntry($"zealotry_crisis_{b.survivor_id}",
                    $"{b.survivor_id}'s faith in {b.belief_id.Replace("belief_", "").Replace('_', ' ')} has shattered.",
                    null!, _simDay);
                // §7.8 — shattered belief causes BOUNDED morale damage through
                // the canonical morale authority (negative delta = damage).
                var needs = _survivors?.Needs;
                var state = needs?.Get(b.survivor_id);
                if (state != null && state.IsAliveState)
                {
                    float damage = 20f + b.conviction / 4f;   // bounded: ≤ 45
                    needs!.Modify(state, NeedKind.Morale, -damage);
                }
            };
            _zealotry.OnRitualDemanded += demand =>
            {
                _journal?.TryAddRawEntry($"zealotry_demand_{demand.BeliefId}",
                    $"The {demand.BeliefId.Replace("belief_", "").Replace('_', ' ')} ask for offerings before the next rite.",
                    null!, _simDay);
            };
            _zealotry.OnEscalationStageChanged += stage =>
            {
                if (stage == ZealotEscalationStage.None) return;
                _journal?.TryAddRawEntry($"zealotry_tension_{stage}",
                    $"Doctrinal tension inside the shelter has reached a new stage: {stage}.",
                    null!, _simDay);
            };
            _zealotry.OnLeaderRegistered += (survivorId, beliefId) =>
            {
                _journal?.TryAddRawEntry($"zealotry_leader_{survivorId}",
                    $"{survivorId} now speaks for the {beliefId.Replace("belief_", "").Replace('_', ' ')}.",
                    null!, _simDay);
            };

            return _zealotry;
        }

        private void SetupZealotry()
        {
            EnsureZealotry();
        }

        private void SaveZealotry()
        {
            if (_zealotry != null)
            {
                CaptureSection("zealotry", ZealotrySaveStore.TryCapturePersisted(_zealotry.CaptureState()));
            }
        }

        /// <summary>Old-save adoption: existing friction beliefs become mild
        /// adherence under the zealotry layer. Never auto-converts anyone and
        /// never fabricates a belief (§10).</summary>
        private void AdoptExistingFrictionBelievers()
        {
            if (_zealotry == null || _survivorSocial == null) return;
            var roster = _survivors?.Roster?.Roster;
            if (roster == null) return;
            int adoptCounter = 0;
            foreach (var entry in roster)
            {
                if (entry == null || !entry.isAlive) continue;
                string belief = _survivorSocial.Friction.GetBelief(entry.survivorId);
                if (string.IsNullOrEmpty(belief)) continue;
                if (_zealotry.Profile(belief) == null) continue;   // only authored profiles
                if (_zealotry.Believer(entry.survivorId) != null) continue;
                int adoptIndex = adoptCounter++;
                _zealotry.TryConvert(entry.survivorId, belief, _simDay,
                    new ConversionContext { Stress01 = 0.4f, LeaderCharisma01 = 0.2f, LeaderOfSameBelief = false },
                    new SeededRng(unchecked(175 * 397 + adoptIndex)));
            }
        }

        /// <summary>
        /// Daily zealotry tick (Plan 175). Deterministic: Core TickDay, then the
        /// bounded ritual cadence — every third day the largest belief group
        /// may demand offerings; resources are consumed through the CANONICAL
        /// inventory and the ritual resolves on real availability.
        /// </summary>
        public void TickZealotryDay(int day)
        {
            if (_zealotry == null) return;
            _zealotry.TickDay(day);

            // Bounded ritual cadence: emit at most one demand per cycle.
            if (day % 3 == 0)
            {
                string? largest = FindLargestBeliefGroup();
                if (largest != null && _zealotry.State.unresolved_demands.Count == 0)
                {
                    var demand = _zealotry.EmitRitualDemand(largest, day);
                    if (demand != null)
                    {
                        var inv = _inventory?.Inventory;
                        bool allAvailable = inv != null;
                        if (allAvailable)
                        {
                            foreach (var itemId in demand.ItemIds)
                                if (inv!.CountById(itemId) <= 0) { allAvailable = false; break; }
                        }
                        if (allAvailable)
                        {
                            foreach (var itemId in demand.ItemIds)
                                inv!.RemoveById(itemId, 1);
                        }
                        _zealotry.ResolveRitualDemand(largest, day, allAvailable);
                    }
                }
            }
        }

        private string? FindLargestBeliefGroup()
        {
            if (_zealotry == null) return null;
            string? best = null;
            int bestCount = 0;
            var counts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var b in _zealotry.State.believers)
            {
                if (b == null || string.IsNullOrEmpty(b.belief_id)) continue;
                counts.TryGetValue(b.belief_id, out int c);
                counts[b.belief_id] = c + 1;
                if (counts[b.belief_id] > bestCount
                    || (counts[b.belief_id] == bestCount && best != null && string.CompareOrdinal(b.belief_id, best) < 0))
                {
                    bestCount = counts[b.belief_id];
                    best = b.belief_id;
                }
            }
            return best;
        }

        /// <summary>Host command: register a charismatic leader (panel/route seam).</summary>
        public bool ZealotryRegisterLeader(string survivorId, string beliefId)
            => _zealotry?.RegisterLeader(survivorId, beliefId) ?? false;

        /// <summary>Host command: shrine presence from the construction authority.</summary>
        public void ZealotrySetShrine(string beliefId, bool present)
            => _zealotry?.SetShrine(beliefId, present);

        /// <summary>Host command: PsyOps broadcast route (§7.13) — reach applies
        /// to existing adherents only, capped by the authored profile.</summary>
        public void ZealotryApplyBroadcast(string beliefId, float reach01)
            => _zealotry?.ApplyBroadcast(beliefId, reach01);

        /// <summary>Host command: belief crisis (failed prophecy / doctrinal
        /// event) — bounded fervor collapse; morale damage routes via the event.</summary>
        public void ZealotryTriggerCrisis(string beliefId)
            => _zealotry?.TriggerCrisis(beliefId, _simDay);
    }
}
