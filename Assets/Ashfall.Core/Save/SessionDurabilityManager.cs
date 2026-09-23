// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Save
{
    [Serializable]
    public sealed class SaveSlotDefinition
    {
        [JsonPropertyName("slot_id")]
        public string SlotId { get; set; } = string.Empty;

        [JsonPropertyName("profile_id")]
        public string ProfileId { get; set; } = "default";

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("campaign_day")]
        public int CampaignDay { get; set; } = 1;

        [JsonPropertyName("survivor_count")]
        public int SurvivorCount { get; set; } = 1;

        [JsonPropertyName("last_saved_utc")]
        public string LastSavedUtc { get; set; } = string.Empty;

        [JsonPropertyName("checksum")]
        public string Checksum { get; set; } = string.Empty;

        [JsonPropertyName("is_corrupt")]
        public bool IsCorrupt { get; set; }

        [JsonPropertyName("has_backup")]
        public bool HasBackup { get; set; }

        [JsonPropertyName("backup_checksum")]
        public string BackupChecksum { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SessionSoakSample
    {
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("advance_duration_ms")]
        public float AdvanceDurationMs { get; set; }

        [JsonPropertyName("tracked_state_bytes")]
        public long TrackedStateBytes { get; set; }
    }

    [Serializable]
    public sealed class SessionDurabilityState
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("active_slot_id")]
        public string ActiveSlotId { get; set; } = "slot_01";

        [JsonPropertyName("slots")]
        public List<SaveSlotDefinition> Slots { get; set; } = new List<SaveSlotDefinition>();

        [JsonPropertyName("soak_samples")]
        public List<SessionSoakSample> SoakSamples { get; set; } = new List<SessionSoakSample>();

        [JsonPropertyName("corrupted_slot_ids")]
        public List<string> CorruptedSlotIds { get; set; } = new List<string>();

        [JsonPropertyName("max_slots")]
        public int MaxSlots { get; set; } = 10;
    }

    public sealed class SoakStabilityReport
    {
        public int TotalDaysSampled { get; set; }
        public float AverageAdvanceDurationMs { get; set; }
        public float P95AdvanceDurationMs { get; set; }
        public float MaxAdvanceDurationMs { get; set; }
        public float MemorySlopeBytesPerDay { get; set; }
        public bool IsMonotonicallyStable { get; set; }
        public string StabilityVerdict { get; set; } = string.Empty;
    }

    /// <summary>
    /// Plan 39 / C2[16] — Session Durability: Saves, Slots, Soak, and the Release Gate.
    /// Pure Core engine-free manager for multi-slot save metadata isolation, rolling backup recovery,
    /// interrupted write corruption handling, and long-horizon session soak stability evaluation.
    /// </summary>
    public sealed class SessionDurabilityManager
    {
        private readonly SessionDurabilityState _state;
        private readonly IWallClock _wallClock;

        public SessionDurabilityState State => _state;
        public string ActiveSlotId => _state.ActiveSlotId;
        public IReadOnlyList<SaveSlotDefinition> Slots => _state.Slots;
        public IReadOnlyList<SessionSoakSample> SoakSamples => _state.SoakSamples;

        public Action<string /*slotId*/, string /*reason*/>? CorruptionDetectedSeam { get; set; }
        public Action<string /*slotId*/, bool /*recovered*/>? BackupRestoredSeam { get; set; }
        public Action<int /*day*/, float /*durationMs*/>? DayAdvanceMeasuredSeam { get; set; }

        public SessionDurabilityManager(SessionDurabilityState? state = null, IWallClock? wallClock = null)
        {
            _state = state ?? new SessionDurabilityState();
            _wallClock = wallClock ?? SystemWallClock.Instance;
        }

        public void SetActiveSlot(string slotId)
        {
            if (string.IsNullOrWhiteSpace(slotId)) return;
            _state.ActiveSlotId = slotId;
        }

        public SaveSlotDefinition? GetSlot(string slotId)
        {
            if (string.IsNullOrWhiteSpace(slotId)) return null;
            return _state.Slots.FirstOrDefault(s => string.Equals(s.SlotId, slotId, StringComparison.OrdinalIgnoreCase));
        }

        public bool RegisterOrUpdateSlot(
            string slotId,
            string displayName,
            int campaignDay,
            int survivorCount,
            string checksum,
            string profileId = "default")
        {
            if (string.IsNullOrWhiteSpace(slotId)) return false;

            var existing = GetSlot(slotId);
            if (existing != null)
            {
                existing.DisplayName = displayName;
                existing.CampaignDay = Math.Max(1, campaignDay);
                existing.SurvivorCount = Math.Max(0, survivorCount);
                existing.Checksum = checksum;
                existing.ProfileId = profileId;
                existing.LastSavedUtc = _wallClock.FormatIsoUtc();
                existing.IsCorrupt = false;
                return true;
            }

            if (_state.Slots.Count >= _state.MaxSlots)
            {
                return false;
            }

            var newSlot = new SaveSlotDefinition
            {
                SlotId = slotId,
                ProfileId = profileId,
                DisplayName = displayName,
                CampaignDay = Math.Max(1, campaignDay),
                SurvivorCount = Math.Max(0, survivorCount),
                Checksum = checksum,
                LastSavedUtc = _wallClock.FormatIsoUtc(),
                IsCorrupt = false,
                HasBackup = false
            };

            _state.Slots.Add(newSlot);
            return true;
        }

        public bool CreateBackup(string slotId, string backupChecksum)
        {
            var slot = GetSlot(slotId);
            if (slot == null) return false;

            slot.HasBackup = true;
            slot.BackupChecksum = backupChecksum;
            return true;
        }

        public bool RecordInterruptedWrite(string slotId, string reason)
        {
            var slot = GetSlot(slotId);
            if (slot == null) return false;

            slot.IsCorrupt = true;
            if (!_state.CorruptedSlotIds.Contains(slotId, StringComparer.OrdinalIgnoreCase))
            {
                _state.CorruptedSlotIds.Add(slotId);
            }

            CorruptionDetectedSeam?.Invoke(slotId, reason);
            return true;
        }

        public bool TryRecoverBackup(string slotId, out string recoveredChecksum)
        {
            recoveredChecksum = string.Empty;
            var slot = GetSlot(slotId);
            if (slot == null || !slot.HasBackup || string.IsNullOrWhiteSpace(slot.BackupChecksum))
            {
                BackupRestoredSeam?.Invoke(slotId, false);
                return false;
            }

            slot.Checksum = slot.BackupChecksum;
            slot.IsCorrupt = false;
            _state.CorruptedSlotIds.RemoveAll(id => string.Equals(id, slotId, StringComparison.OrdinalIgnoreCase));
            recoveredChecksum = slot.Checksum;

            BackupRestoredSeam?.Invoke(slotId, true);
            return true;
        }

        public bool ValidateSlotChecksum(string slotId, string candidateChecksum)
        {
            var slot = GetSlot(slotId);
            if (slot == null) return false;

            bool matches = string.Equals(slot.Checksum, candidateChecksum, StringComparison.Ordinal);
            if (!matches)
            {
                RecordInterruptedWrite(slotId, "Checksum mismatch verification failed");
            }
            return matches;
        }

        public void RecordDayAdvance(int day, float durationMs, long stateBytes)
        {
            var sample = new SessionSoakSample
            {
                Day = day,
                AdvanceDurationMs = Math.Max(0f, durationMs),
                TrackedStateBytes = Math.Max(0L, stateBytes)
            };

            _state.SoakSamples.Add(sample);
            DayAdvanceMeasuredSeam?.Invoke(day, durationMs);
        }

        public SoakStabilityReport EvaluateSoakStability(
            float maxAllowedP95Ms = 2500f,
            float maxAllowedSlopeBytesPerDay = 50000f)
        {
            if (_state.SoakSamples.Count == 0)
            {
                return new SoakStabilityReport
                {
                    TotalDaysSampled = 0,
                    IsMonotonicallyStable = true,
                    StabilityVerdict = "No samples recorded"
                };
            }

            var samples = _state.SoakSamples;
            float totalMs = samples.Sum(s => s.AdvanceDurationMs);
            float avgMs = totalMs / samples.Count;
            float maxMs = samples.Max(s => s.AdvanceDurationMs);

            var sortedDurations = samples.Select(s => s.AdvanceDurationMs).OrderBy(v => v).ToList();
            int p95Index = Math.Min(sortedDurations.Count - 1, (int)Math.Ceiling(sortedDurations.Count * 0.95) - 1);
            float p95Ms = sortedDurations[Math.Max(0, p95Index)];

            float slope = 0f;
            if (samples.Count > 1)
            {
                long byteDelta = samples.Last().TrackedStateBytes - samples.First().TrackedStateBytes;
                int dayDelta = Math.Max(1, samples.Last().Day - samples.First().Day);
                slope = (float)byteDelta / dayDelta;
            }

            bool p95Pass = p95Ms <= maxAllowedP95Ms;
            bool slopePass = slope <= maxAllowedSlopeBytesPerDay;
            bool stable = p95Pass && slopePass;

            string verdict = stable
                ? "STABLE: Day-advance latency and memory growth within bounds"
                : $"UNSTABLE: P95 ({p95Ms:F1}ms vs max {maxAllowedP95Ms:F1}ms) or slope ({slope:F1}B/day vs max {maxAllowedSlopeBytesPerDay:F1}B/day) exceeded";

            return new SoakStabilityReport
            {
                TotalDaysSampled = samples.Count,
                AverageAdvanceDurationMs = MathF.Round(avgMs, 2),
                P95AdvanceDurationMs = MathF.Round(p95Ms, 2),
                MaxAdvanceDurationMs = MathF.Round(maxMs, 2),
                MemorySlopeBytesPerDay = MathF.Round(slope, 1),
                IsMonotonicallyStable = stable,
                StabilityVerdict = verdict
            };
        }

        public SessionDurabilityState CaptureState()
        {
            return new SessionDurabilityState
            {
                SchemaVersion = _state.SchemaVersion,
                ActiveSlotId = _state.ActiveSlotId,
                MaxSlots = _state.MaxSlots,
                Slots = _state.Slots.Select(s => new SaveSlotDefinition
                {
                    SlotId = s.SlotId,
                    ProfileId = s.ProfileId,
                    DisplayName = s.DisplayName,
                    CampaignDay = s.CampaignDay,
                    SurvivorCount = s.SurvivorCount,
                    LastSavedUtc = s.LastSavedUtc,
                    Checksum = s.Checksum,
                    IsCorrupt = s.IsCorrupt,
                    HasBackup = s.HasBackup,
                    BackupChecksum = s.BackupChecksum
                }).ToList(),
                SoakSamples = _state.SoakSamples.Select(sam => new SessionSoakSample
                {
                    Day = sam.Day,
                    AdvanceDurationMs = sam.AdvanceDurationMs,
                    TrackedStateBytes = sam.TrackedStateBytes
                }).ToList(),
                CorruptedSlotIds = new List<string>(_state.CorruptedSlotIds)
            };
        }

        public void RestoreState(SessionDurabilityState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.ActiveSlotId = state.ActiveSlotId;
            _state.MaxSlots = state.MaxSlots;
            _state.Slots.Clear();
            _state.SoakSamples.Clear();
            _state.CorruptedSlotIds.Clear();

            if (state.Slots != null)
            {
                _state.Slots.AddRange(state.Slots.Select(s => new SaveSlotDefinition
                {
                    SlotId = s.SlotId,
                    ProfileId = s.ProfileId,
                    DisplayName = s.DisplayName,
                    CampaignDay = s.CampaignDay,
                    SurvivorCount = s.SurvivorCount,
                    LastSavedUtc = s.LastSavedUtc,
                    Checksum = s.Checksum,
                    IsCorrupt = s.IsCorrupt,
                    HasBackup = s.HasBackup,
                    BackupChecksum = s.BackupChecksum
                }));
            }

            if (state.SoakSamples != null)
            {
                _state.SoakSamples.AddRange(state.SoakSamples.Select(sam => new SessionSoakSample
                {
                    Day = sam.Day,
                    AdvanceDurationMs = sam.AdvanceDurationMs,
                    TrackedStateBytes = sam.TrackedStateBytes
                }));
            }

            if (state.CorruptedSlotIds != null)
            {
                _state.CorruptedSlotIds.AddRange(state.CorruptedSlotIds);
            }
        }
    }
}
