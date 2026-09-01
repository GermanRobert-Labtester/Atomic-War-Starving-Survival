// SPDX-License-Identifier: MIT
// ASHFALL Core: Heirloom instance tracking, provenance accumulation, and inheritance engine (Plan 21).

using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Phantoms
{
    [Serializable]
    public sealed class HeirloomProvenanceRecord
    {
        public string holder_id = string.Empty;
        public string acquisition_cause = string.Empty;
        public int start_day;
        public int end_day;
        public string transfer_reason = string.Empty;
        public string fate = string.Empty;
        public int unlocked_stage_index = 1;
    }

    [Serializable]
    public sealed class HeirloomInstanceState
    {
        public string instance_id = string.Empty;
        public string heirloom_id = string.Empty;
        public string current_holder_id = string.Empty;
        public List<HeirloomProvenanceRecord> provenance = new List<HeirloomProvenanceRecord>();
        public List<int> unlocked_stages = new List<int> { 1 };
        public bool is_memorialized;
        public bool is_completed;
        public bool is_legacy_selected;
    }

    [Serializable]
    public sealed class HeirloomSystemState
    {
        public string systemId = HeirloomSystem.SystemId;
        public List<HeirloomInstanceState> instances = new List<HeirloomInstanceState>();
    }

    /// <summary>
    /// Engine-agnostic heirloom lifecycle manager. Manages persistent named heirlooms,
    /// appends bounded provenance, handles generational inheritance on survivor death,
    /// and resolves holder-specific memories.
    /// </summary>
    public sealed class HeirloomSystem
    {
        public const string SystemId = "heirloom_system";
        public const int MaxProvenanceEntriesPerInstance = 24;

        private readonly HeirloomCatalog _catalog;
        private readonly ILog _log;
        private readonly Dictionary<string, HeirloomInstanceState> _instances =
            new Dictionary<string, HeirloomInstanceState>(StringComparer.Ordinal);

        public event Action<string, string, string>? OnHeirloomInherited; // instanceId, deceasedId, newHolderId
        public event Action<string, string>? OnHeirloomHolderAssigned;    // instanceId, newHolderId
        public event Action<string, int>? OnHeirloomStageUnlocked;        // instanceId, stageIndex
        public event Action? OnStateChanged;

        public HeirloomSystem(HeirloomCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public IReadOnlyCollection<HeirloomInstanceState> AllInstances => _instances.Values;

        public HeirloomInstanceState? GetInstance(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId)) return null;
            _instances.TryGetValue(instanceId, out var state);
            return state;
        }

        public List<HeirloomInstanceState> GetHeirloomsForHolder(string holderId)
        {
            var results = new List<HeirloomInstanceState>();
            if (string.IsNullOrEmpty(holderId)) return results;
            foreach (var inst in _instances.Values)
            {
                if (string.Equals(inst.current_holder_id, holderId, StringComparison.OrdinalIgnoreCase))
                    results.Add(inst);
            }
            return results;
        }

        public HeirloomInstanceState CreateInstance(
            string heirloomId,
            string initialHolderId,
            int currentDay,
            string acquisitionCause = "initial_discovery")
        {
            string instanceId = $"inst_{heirloomId}_{_instances.Count + 1}";
            var inst = new HeirloomInstanceState
            {
                instance_id = instanceId,
                heirloom_id = heirloomId,
                current_holder_id = initialHolderId ?? string.Empty,
                unlocked_stages = new List<int> { 1 }
            };

            var def = _catalog.GetById(heirloomId);
            if (def != null && def.stages != null && def.stages.Count > 1)
            {
                // Stage 1 is origin, Stage 2 unlocked on recovery
                inst.unlocked_stages.Add(2);
            }

            inst.provenance.Add(new HeirloomProvenanceRecord
            {
                holder_id = initialHolderId ?? "shelter_communal",
                acquisition_cause = acquisitionCause,
                start_day = Math.Max(1, currentDay),
                transfer_reason = acquisitionCause,
                unlocked_stage_index = inst.unlocked_stages.Count
            });

            _instances[instanceId] = inst;
            OnHeirloomHolderAssigned?.Invoke(instanceId, initialHolderId ?? string.Empty);
            OnStateChanged?.Invoke();
            return inst;
        }

        public bool AssignHolder(string instanceId, string newHolderId, int currentDay, string transferReason = "manual_transfer")
        {
            if (!_instances.TryGetValue(instanceId, out var inst)) return false;
            if (string.Equals(inst.current_holder_id, newHolderId, StringComparison.OrdinalIgnoreCase)) return true;

            // Close current provenance record
            if (inst.provenance.Count > 0)
            {
                var last = inst.provenance[inst.provenance.Count - 1];
                if (last.end_day == 0) last.end_day = Math.Max(last.start_day, currentDay);
                last.fate = $"Transferred: {transferReason}";
            }

            inst.current_holder_id = newHolderId ?? string.Empty;

            // Append new provenance record (bounded)
            if (inst.provenance.Count >= MaxProvenanceEntriesPerInstance)
            {
                inst.provenance.RemoveAt(1); // keep original origin at 0, compact oldest intermediate
            }

            inst.provenance.Add(new HeirloomProvenanceRecord
            {
                holder_id = string.IsNullOrEmpty(newHolderId) ? "shelter_communal" : newHolderId,
                acquisition_cause = transferReason,
                start_day = Math.Max(1, currentDay),
                transfer_reason = transferReason,
                unlocked_stage_index = inst.unlocked_stages.Count
            });

            OnHeirloomHolderAssigned?.Invoke(instanceId, inst.current_holder_id);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Handles survivor death: atomically transfers all held heirlooms to the most eligible
        /// kin/bonded successor, unlocks inheritance memory stages, and falls back to communal storage.
        /// </summary>
        public int HandleSurvivorDeath(
            string deceasedId,
            int currentDay,
            GenerationalLineageExtension? lineage = null,
            SurvivorRelationsSystem? relations = null)
        {
            if (string.IsNullOrEmpty(deceasedId)) return 0;
            var held = GetHeirloomsForHolder(deceasedId);
            if (held.Count == 0) return 0;

            string successorId = FindBestSuccessor(deceasedId, lineage, relations);

            for (int i = 0; i < held.Count; i++)
            {
                var inst = held[i];
                string reason = string.IsNullOrEmpty(successorId) ? "death_storage_fallback" : "death_inheritance";

                // Close deceased provenance
                if (inst.provenance.Count > 0)
                {
                    var prev = inst.provenance[inst.provenance.Count - 1];
                    if (prev.end_day == 0) prev.end_day = Math.Max(prev.start_day, currentDay);
                    prev.fate = $"Deceased on Day {currentDay}";
                }

                inst.current_holder_id = successorId;

                // Unlock Stage 3 (Generational Continuity) if exists
                var def = _catalog.GetById(inst.heirloom_id);
                if (def != null && def.stages != null)
                {
                    for (int s = 1; s <= def.stages.Count; s++)
                    {
                        if (!inst.unlocked_stages.Contains(s))
                        {
                            inst.unlocked_stages.Add(s);
                            OnHeirloomStageUnlocked?.Invoke(inst.instance_id, s);
                            break;
                        }
                    }
                }

                if (inst.provenance.Count >= MaxProvenanceEntriesPerInstance)
                {
                    inst.provenance.RemoveAt(1);
                }

                inst.provenance.Add(new HeirloomProvenanceRecord
                {
                    holder_id = string.IsNullOrEmpty(successorId) ? "shelter_communal" : successorId,
                    acquisition_cause = "inherited_on_death",
                    start_day = Math.Max(1, currentDay),
                    transfer_reason = reason,
                    unlocked_stage_index = inst.unlocked_stages.Count
                });

                OnHeirloomInherited?.Invoke(inst.instance_id, deceasedId, successorId);
            }

            OnStateChanged?.Invoke();
            return held.Count;
        }

        private string FindBestSuccessor(
            string deceasedId,
            GenerationalLineageExtension? lineage,
            SurvivorRelationsSystem? relations)
        {
            // 1. Check Lineage kin / child
            if (lineage != null)
            {
                var lineages = lineage.GetLineage(deceasedId);
                for (int i = 0; i < lineages.Count; i++)
                {
                    var l = lineages[i];
                    if (l.isActive && !string.IsNullOrEmpty(l.childId) && !string.Equals(l.childId, deceasedId, StringComparison.OrdinalIgnoreCase))
                    {
                        return l.childId;
                    }
                }
            }

            // 2. Check Relations: highest positive affinity/trust
            if (relations?.State?.relationships != null)
            {
                string bestCandidate = string.Empty;
                float bestScore = 0f;

                foreach (var rel in relations.State.relationships)
                {
                    string other = string.Equals(rel.dwellerA, deceasedId, StringComparison.OrdinalIgnoreCase) ? rel.dwellerB :
                                   string.Equals(rel.dwellerB, deceasedId, StringComparison.OrdinalIgnoreCase) ? rel.dwellerA : string.Empty;

                    if (!string.IsNullOrEmpty(other))
                    {
                        float score = rel.affinity + (rel.trust * 0.5f);
                        if (score > 0 && score > bestScore)
                        {
                            bestScore = score;
                            bestCandidate = other;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(bestCandidate)) return bestCandidate;
            }

            return string.Empty; // fallback to communal
        }

        public (string memoryText, float moraleDelta, float guiltDelta) ResolveHolderMemory(
            string instanceId,
            PhantomSurvivorSnapshot survivor)
        {
            if (!_instances.TryGetValue(instanceId, out var inst))
                return (string.Empty, 0f, 0f);

            var def = _catalog.GetById(inst.heirloom_id);
            if (def == null || def.holder_memories == null || def.holder_memories.Count == 0)
                return (string.Empty, 0f, 0f);

            // 1. Try affinity match
            for (int i = 0; i < def.holder_memories.Count; i++)
            {
                var m = def.holder_memories[i];
                if (MatchesHolderAffinity(m.affinity_key, survivor))
                {
                    string text = m.memory_text.Replace("{name}", survivor.displayName ?? "Survivor");
                    return (text, m.morale_effect, m.guilt_effect);
                }
            }

            // 2. Generic fallback
            for (int i = 0; i < def.holder_memories.Count; i++)
            {
                var m = def.holder_memories[i];
                if (string.Equals(m.affinity_key, "generic", StringComparison.OrdinalIgnoreCase))
                {
                    string text = m.memory_text.Replace("{name}", survivor.displayName ?? "Survivor");
                    return (text, m.morale_effect, m.guilt_effect);
                }
            }

            return (string.Empty, 0f, 0f);
        }

        private static bool MatchesHolderAffinity(string affinityKey, PhantomSurvivorSnapshot sv)
        {
            if (string.IsNullOrEmpty(affinityKey) || sv == null) return false;
            string key = affinityKey.ToLowerInvariant();

            if (key == "kin" || key == "child" || key == "family")
                return true; // kin-priority if evaluated in kin context

            if (!string.IsNullOrEmpty(sv.backgroundId) && key.Contains(sv.backgroundId.ToLowerInvariant()))
                return true;

            if (sv.traitIds != null)
            {
                for (int i = 0; i < sv.traitIds.Count; i++)
                {
                    if (key.Contains(sv.traitIds[i].ToLowerInvariant())) return true;
                }
            }

            return false;
        }

        public void SetMemorialized(string instanceId, bool memorialized)
        {
            if (_instances.TryGetValue(instanceId, out var inst))
            {
                inst.is_memorialized = memorialized;
                OnStateChanged?.Invoke();
            }
        }

        public void SetLegacySelected(string instanceId, bool selected)
        {
            if (_instances.TryGetValue(instanceId, out var inst))
            {
                inst.is_legacy_selected = selected;
                OnStateChanged?.Invoke();
            }
        }

        public HeirloomSystemState CaptureState()
        {
            var state = new HeirloomSystemState { systemId = SystemId };
            var keys = new List<string>(_instances.Keys);
            keys.Sort(string.CompareOrdinal);

            for (int i = 0; i < keys.Count; i++)
            {
                var inst = _instances[keys[i]];
                var copy = new HeirloomInstanceState
                {
                    instance_id = inst.instance_id,
                    heirloom_id = inst.heirloom_id,
                    current_holder_id = inst.current_holder_id,
                    unlocked_stages = new List<int>(inst.unlocked_stages),
                    is_memorialized = inst.is_memorialized,
                    is_completed = inst.is_completed,
                    is_legacy_selected = inst.is_legacy_selected
                };

                for (int p = 0; p < inst.provenance.Count; p++)
                {
                    var prov = inst.provenance[p];
                    copy.provenance.Add(new HeirloomProvenanceRecord
                    {
                        holder_id = prov.holder_id,
                        acquisition_cause = prov.acquisition_cause,
                        start_day = prov.start_day,
                        end_day = prov.end_day,
                        transfer_reason = prov.transfer_reason,
                        fate = prov.fate,
                        unlocked_stage_index = prov.unlocked_stage_index
                    });
                }

                state.instances.Add(copy);
            }

            return state;
        }

        public void RestoreState(HeirloomSystemState state)
        {
            if (state == null) return;
            _instances.Clear();

            if (state.instances != null)
            {
                for (int i = 0; i < state.instances.Count; i++)
                {
                    var inst = state.instances[i];
                    if (inst == null || string.IsNullOrEmpty(inst.instance_id)) continue;

                    var copy = new HeirloomInstanceState
                    {
                        instance_id = inst.instance_id,
                        heirloom_id = inst.heirloom_id,
                        current_holder_id = inst.current_holder_id ?? string.Empty,
                        unlocked_stages = inst.unlocked_stages != null ? new List<int>(inst.unlocked_stages) : new List<int> { 1 },
                        is_memorialized = inst.is_memorialized,
                        is_completed = inst.is_completed,
                        is_legacy_selected = inst.is_legacy_selected,
                        provenance = new List<HeirloomProvenanceRecord>()
                    };

                    if (inst.provenance != null)
                    {
                        for (int p = 0; p < inst.provenance.Count; p++)
                        {
                            var prov = inst.provenance[p];
                            if (prov == null) continue;
                            copy.provenance.Add(new HeirloomProvenanceRecord
                            {
                                holder_id = prov.holder_id ?? string.Empty,
                                acquisition_cause = prov.acquisition_cause ?? string.Empty,
                                start_day = prov.start_day,
                                end_day = prov.end_day,
                                transfer_reason = prov.transfer_reason ?? string.Empty,
                                fate = prov.fate ?? string.Empty,
                                unlocked_stage_index = prov.unlocked_stage_index
                            });
                        }
                    }

                    _instances[copy.instance_id] = copy;
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
