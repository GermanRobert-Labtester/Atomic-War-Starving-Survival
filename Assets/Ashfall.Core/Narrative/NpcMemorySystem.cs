// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Type of player action recorded in personal NPC memory.
    /// </summary>
    public enum NpcMemoryActionType
    {
        Helped = 0,
        Refused = 1,
        Ignored = 2,
        Betrayed = 3,
        SavedLife = 4,
        GiftedFood = 5,
        Healed = 6,
        Restitution = 7
    }

    /// <summary>
    /// Derived conversational/greeting disposition of an NPC based on relationship memory.
    /// </summary>
    public enum NpcDialogueTone
    {
        Neutral = 0,
        HighTrust = 1,
        HighGrudge = 2,
        FavorOwed = 3,
        Betrayed = 4,
        Reconciled = 5
    }

    /// <summary>
    /// One specific remembered interaction between player and NPC.
    /// </summary>
    [Serializable]
    public class NpcMemoryEntry
    {
        public string NpcId { get; set; } = string.Empty;
        public NpcMemoryActionType Action { get; set; }
        public string TargetId { get; set; } = string.Empty;
        public int Day { get; set; }
        public float Intensity { get; set; } = 50f;
        public bool Forgiven { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        public NpcMemoryEntry() { }

        public NpcMemoryEntry(
            string npcId,
            NpcMemoryActionType action,
            string targetId,
            int day,
            float intensity,
            bool forgiven,
            IEnumerable<string>? tags = null)
        {
            NpcId = npcId ?? string.Empty;
            Action = action;
            TargetId = targetId ?? string.Empty;
            Day = day;
            Intensity = Math.Clamp(intensity, 0f, 100f);
            Forgiven = forgiven;
            if (tags != null) Tags.AddRange(tags);
        }
    }

    /// <summary>
    /// Aggregated relationship state of one named NPC toward the player.
    /// </summary>
    [Serializable]
    public class NpcRelationship
    {
        public string NpcId { get; set; } = string.Empty;
        public float PersonalTrust { get; set; } // -100 to +100
        public float GrudgeLevel { get; set; }   // 0 to 100
        public float FavorOwed { get; set; }     // 0 to 100
        public int LastInteractionDay { get; set; } = -1;
        public List<NpcMemoryEntry> Memories { get; set; } = new List<NpcMemoryEntry>();

        public NpcRelationship() { }

        public NpcRelationship(string npcId)
        {
            NpcId = npcId ?? string.Empty;
            PersonalTrust = 0f;
            GrudgeLevel = 0f;
            FavorOwed = 0f;
        }
    }

    /// <summary>
    /// One authored dialogue template triggered by NPC relationship memory tone.
    /// </summary>
    [Serializable]
    public sealed class NpcMemoryDialogueDef
    {
        public string tone { get; set; } = string.Empty;
        public string template_id { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    }

    /// <summary>
    /// Catalog root for npc_memory_dialogue.json.
    /// </summary>
    [Serializable]
    public sealed class NpcMemoryDialogueCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<NpcMemoryDialogueDef> dialogue_templates { get; set; } = new List<NpcMemoryDialogueDef>();
    }

    /// <summary>
    /// Save-state DTO for NpcMemorySystem.
    /// </summary>
    [Serializable]
    public class NpcMemorySaveState
    {
        public int SchemaVersion { get; set; } = 1;
        public int LastDecayedDay { get; set; } = -1;
        public List<NpcRelationship> Relationships { get; set; } = new List<NpcRelationship>();
    }

    /// <summary>
    /// Per-NPC memory authority: records player actions toward specific NPCs,
    /// derives individual relationship standing (trust, grudge, favors owed),
    /// decays temporal memories, provides forgiveness reconciliation mechanics,
    /// and projects trade discounts/embargoes and dialogue tone without competing
    /// with faction-level standing.
    /// </summary>
    public sealed class NpcMemorySystem
    {
        public const int CurrentSchemaVersion = 1;

        private readonly Dictionary<string, NpcRelationship> _relationships =
            new Dictionary<string, NpcRelationship>(StringComparer.Ordinal);
        private readonly List<NpcMemoryDialogueDef> _dialogueTemplates = new List<NpcMemoryDialogueDef>();

        public int LastDecayedDay { get; private set; } = -1;

        public event Action<string, NpcMemoryEntry>? OnMemoryRecorded;
        public event Action<string, string, float>? OnGrudgeForgiven;

        public void LoadDialogueCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<NpcMemoryDialogueCatalog>(json, options);
                if (catalog?.dialogue_templates != null)
                {
                    _dialogueTemplates.Clear();
                    _dialogueTemplates.AddRange(catalog.dialogue_templates);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<NpcMemoryDialogueDef> GetAllDialogueTemplates() => _dialogueTemplates;

        public IReadOnlyList<NpcMemoryDialogueDef> GetDialogueTemplatesForTone(NpcDialogueTone tone)
        {
            string toneKey = tone switch
            {
                NpcDialogueTone.HighTrust => "high_trust",
                NpcDialogueTone.HighGrudge => "high_grudge",
                NpcDialogueTone.FavorOwed => "favor_owed",
                NpcDialogueTone.Betrayed => "betrayed",
                NpcDialogueTone.Reconciled => "reconciled",
                _ => "neutral"
            };

            return _dialogueTemplates
                .Where(t => string.Equals(t.tone, toneKey, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public NpcRelationship GetOrCreate(string npcId)
        {
            if (string.IsNullOrEmpty(npcId))
                return new NpcRelationship("anonymous");

            if (!_relationships.TryGetValue(npcId, out var rel))
            {
                rel = new NpcRelationship(npcId);
                _relationships[npcId] = rel;
            }
            return rel;
        }

        public NpcRelationship? Get(string npcId)
        {
            if (string.IsNullOrEmpty(npcId)) return null;
            return _relationships.TryGetValue(npcId, out var rel) ? rel : null;
        }

        /// <summary>
        /// Record a player action affecting this specific NPC.
        /// Deterministic calculation of trust, grudge, and favor deltas.
        /// </summary>
        public NpcMemoryEntry RecordAction(
            string npcId,
            NpcMemoryActionType action,
            int day,
            string targetId = "",
            float intensity = 50f,
            params string[] tags)
        {
            var rel = GetOrCreate(npcId);
            rel.LastInteractionDay = day;

            var entry = new NpcMemoryEntry(npcId, action, targetId, day, intensity, false, tags);
            rel.Memories.Add(entry);

            // Authoritative delta table from Plan 147 §Task 1
            switch (action)
            {
                case NpcMemoryActionType.Helped:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust + 10f, -100f, 100f);
                    rel.FavorOwed = Math.Clamp(rel.FavorOwed + 5f, 0f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel - 5f, 0f, 100f);
                    break;

                case NpcMemoryActionType.SavedLife:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust + 40f, -100f, 100f);
                    rel.FavorOwed = Math.Clamp(rel.FavorOwed + 20f, 0f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel - 20f, 0f, 100f);
                    break;

                case NpcMemoryActionType.GiftedFood:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust + 15f, -100f, 100f);
                    rel.FavorOwed = Math.Clamp(rel.FavorOwed + 10f, 0f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel - 5f, 0f, 100f);
                    break;

                case NpcMemoryActionType.Healed:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust + 25f, -100f, 100f);
                    rel.FavorOwed = Math.Clamp(rel.FavorOwed + 15f, 0f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel - 10f, 0f, 100f);
                    break;

                case NpcMemoryActionType.Refused:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust - 15f, -100f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel + 10f, 0f, 100f);
                    break;

                case NpcMemoryActionType.Ignored:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust - 5f, -100f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel + 5f, 0f, 100f);
                    break;

                case NpcMemoryActionType.Betrayed:
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust - 35f, -100f, 100f);
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel + 40f, 0f, 100f);
                    rel.FavorOwed = 0f; // betrayal wipes out any favors owed to player
                    break;

                case NpcMemoryActionType.Restitution:
                    rel.GrudgeLevel = Math.Clamp(rel.GrudgeLevel - 25f, 0f, 100f);
                    rel.PersonalTrust = Math.Clamp(rel.PersonalTrust + 5f, -100f, 100f);
                    entry.Forgiven = true;
                    break;
            }

            OnMemoryRecorded?.Invoke(npcId, entry);
            return entry;
        }

        /// <summary>
        /// Daily decay: old memories and grudges soften by 1% per day unless permanent.
        /// Betrayal memories soften at half rate.
        /// </summary>
        public void TickDailyDecay(int currentDay, float decayRatePerDay = 1.0f)
        {
            if (currentDay <= LastDecayedDay) return;
            int elapsedDays = LastDecayedDay < 0 ? 1 : Math.Max(1, currentDay - LastDecayedDay);
            LastDecayedDay = currentDay;

            float totalDecay = elapsedDays * decayRatePerDay;

            foreach (var kv in _relationships)
            {
                var rel = kv.Value;

                // Grudge decays toward zero
                if (rel.GrudgeLevel > 0f)
                {
                    bool hasUnforgivenBetrayal = false;
                    for (int i = 0; i < rel.Memories.Count; i++)
                    {
                        if (rel.Memories[i].Action == NpcMemoryActionType.Betrayed && !rel.Memories[i].Forgiven)
                        {
                            hasUnforgivenBetrayal = true;
                            break;
                        }
                    }

                    float effectiveDecay = hasUnforgivenBetrayal ? totalDecay * 0.5f : totalDecay;
                    rel.GrudgeLevel = Math.Max(0f, rel.GrudgeLevel - effectiveDecay);
                }

                // Favors also slightly decay if unattended over very long times
                if (rel.FavorOwed > 0f && elapsedDays >= 10)
                {
                    rel.FavorOwed = Math.Max(0f, rel.FavorOwed - (elapsedDays * 0.2f));
                }

                // Memory intensities decay
                for (int i = 0; i < rel.Memories.Count; i++)
                {
                    var m = rel.Memories[i];
                    m.Intensity = Math.Max(5f, m.Intensity - (totalDecay * 0.5f));
                }
            }
        }

        /// <summary>
        /// Player makes amends or apologizes, reducing grudge and setting matching memories to forgiven.
        /// </summary>
        public bool Forgive(string npcId, string reason, float restitutionAmount = 0f)
        {
            var rel = Get(npcId);
            if (rel == null || rel.GrudgeLevel <= 0f) return false;

            float grudgeReduction = 20f + Math.Clamp(restitutionAmount * 0.5f, 0f, 60f);
            rel.GrudgeLevel = Math.Max(0f, rel.GrudgeLevel - grudgeReduction);
            rel.PersonalTrust = Math.Clamp(rel.PersonalTrust + 10f, -100f, 100f);

            // Mark oldest unforgiven negative memory as forgiven
            for (int i = 0; i < rel.Memories.Count; i++)
            {
                var m = rel.Memories[i];
                if (!m.Forgiven && (m.Action == NpcMemoryActionType.Refused || m.Action == NpcMemoryActionType.Betrayed))
                {
                    m.Forgiven = true;
                    break;
                }
            }

            OnGrudgeForgiven?.Invoke(npcId, reason, grudgeReduction);
            return true;
        }

        /// <summary>
        /// Projects trade price multiplier based on personal relationship:
        /// High trust yields up to 15% discount (0.85x).
        /// High grudge yields up to 30% surcharge (1.30x).
        /// Severe grudge (>75) triggers trade refusal (returns PositiveInfinity).
        /// </summary>
        public float GetTradePriceMultiplier(string npcId)
        {
            var rel = Get(npcId);
            if (rel == null) return 1.0f;

            if (rel.GrudgeLevel > 75f)
                return float.PositiveInfinity; // Trade embargo / refused

            if (rel.GrudgeLevel >= 40f)
                return 1.30f; // Severe markup

            if (rel.GrudgeLevel >= 15f)
                return 1.15f; // Moderate markup

            if (rel.PersonalTrust >= 50f)
                return 0.85f; // Trusted ally discount

            if (rel.PersonalTrust >= 20f)
                return 0.95f; // Friendly discount

            return 1.0f; // Neutral
        }

        /// <summary>
        /// Whether this NPC refuses to trade or interact due to severe grudge or betrayal.
        /// </summary>
        public bool IsTradeRefused(string npcId)
        {
            var rel = Get(npcId);
            return rel != null && rel.GrudgeLevel > 75f;
        }

        /// <summary>
        /// Returns conversational disposition tone for dialogue selection.
        /// </summary>
        public NpcDialogueTone GetDialogueTone(string npcId)
        {
            var rel = Get(npcId);
            if (rel == null) return NpcDialogueTone.Neutral;

            if (rel.GrudgeLevel > 75f)
                return NpcDialogueTone.Betrayed;

            if (rel.GrudgeLevel >= 30f)
                return NpcDialogueTone.HighGrudge;

            if (rel.FavorOwed >= 20f)
                return NpcDialogueTone.FavorOwed;

            if (rel.PersonalTrust >= 30f)
                return NpcDialogueTone.HighTrust;

            // Check if recent restitution was made
            for (int i = rel.Memories.Count - 1; i >= 0 && i >= rel.Memories.Count - 3; i--)
            {
                if (rel.Memories[i].Action == NpcMemoryActionType.Restitution)
                    return NpcDialogueTone.Reconciled;
            }

            return NpcDialogueTone.Neutral;
        }

        /// <summary>
        /// Capture save state with schema versioning.
        /// </summary>
        public NpcMemorySaveState CaptureState()
        {
            var state = new NpcMemorySaveState
            {
                SchemaVersion = CurrentSchemaVersion,
                LastDecayedDay = LastDecayedDay
            };

            foreach (var kv in _relationships)
            {
                state.Relationships.Add(kv.Value);
            }

            return state;
        }

        /// <summary>
        /// Restore save state with schema migration safety.
        /// </summary>
        public void RestoreState(NpcMemorySaveState? state)
        {
            _relationships.Clear();
            if (state == null)
            {
                LastDecayedDay = -1;
                return;
            }

            LastDecayedDay = state.LastDecayedDay;
            if (state.Relationships != null)
            {
                for (int i = 0; i < state.Relationships.Count; i++)
                {
                    var rel = state.Relationships[i];
                    if (rel != null && !string.IsNullOrEmpty(rel.NpcId))
                    {
                        _relationships[rel.NpcId] = rel;
                    }
                }
            }
        }
    }
}
