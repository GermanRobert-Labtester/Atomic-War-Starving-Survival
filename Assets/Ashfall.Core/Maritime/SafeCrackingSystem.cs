using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Maritime
{
    // ── Safe definition ──────────────────────────────────────────────

    /// <summary>Data-driven safe/container definition.</summary>
    [Serializable]
    public class SafeDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string roomId = string.Empty;
        public int difficulty = 3;           // number of tumblers (1-6)
        public int maxAttempts = 10;         // before jamming
        public float noisePerAttempt = 0.2f; // 0..1 cumulative
        public float alarmThreshold = 0.8f;  // noise above this triggers alarm
        public List<SafeLootEntry> loot = new List<SafeLootEntry>();
    }

    /// <summary>One loot entry in a safe.</summary>
    [Serializable]
    public class SafeLootEntry
    {
        public string itemId = string.Empty;
        public int minQuantity = 1;
        public int maxQuantity = 1;
        public float weightKg = 1f;
    }

    // ── Safe state ──────────────────────────────────────────────────

    /// <summary>Runtime state of one safe instance.</summary>
    [Serializable]
    public class SafeInstanceState
    {
        public string safeId = string.Empty;
        public string locationId = string.Empty;
        public string roomId = string.Empty;
        public int difficulty = 3;
        public int attemptsUsed = 0;
        public int maxAttempts = 10;
        public float cumulativeNoise = 0f;
        public float alarmThreshold = 0.8f;
        public bool isOpened = false;
        public bool isJammed = false;
        public bool alarmTriggered = false;
        public bool lootTransferred = false;
        public int openedDay = -1;
        public List<SafeLootEntry> loot = new List<SafeLootEntry>();
        // Deterministic combination: derived from seed + safeId, never serialized
        public int[] combination = Array.Empty<int>();
    }

    /// <summary>System-wide safe cracking state (save DTO).</summary>
    [Serializable]
    public class SafeCrackingState
    {
        public string systemId = SafeCrackingSystem.SystemId;
        public List<SafeInstanceState> safes = new List<SafeInstanceState>();
    }

    /// <summary>Result of a safe cracking attempt.</summary>
    public enum SafeAttemptResult
    {
        Success,        // safe opened
        PartialHint,    // got feedback on tumblers
        Failed,         // wrong combination
        ToolDamaged,    // lockpick broke
        NoiseWarning,   // noise approaching threshold
        AlarmTriggered, // noise exceeded threshold
        Jammed,         // max attempts reached
        AlreadyOpened,  // safe was already opened
        InvalidInput    // bad parameters
    }

    /// <summary>Feedback from an attempt (for UI presentation).</summary>
    public class SafeAttemptFeedback
    {
        public SafeAttemptResult Result;
        public int CorrectTumblers;     // how many tumblers are in the right position
        public int TotalTumblers;       // total tumblers in the safe
        public float NoiseLevel;        // current cumulative noise
        public float ToolCondition;     // remaining tool condition
        public string Message;          // human-readable feedback
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL — Deterministic safe cracking system.
    /// Resolves safe/container opening through seeded tumbler combinations.
    /// The UI presents dial rotation and audio cues, but Core owns the
    /// actual combination and outcome. Loot transfers through existing
    /// inventory/scavenge paths.
    ///
    /// Determinism: combination is derived from seed + safeId hash.
    /// Same safe + same seed = same combination every time.
    /// </summary>
    public class SafeCrackingSystem
    {
        public const string SystemId = "safe_cracking_system";
        public const float BaseToolCondition = 1.0f;
        public const float ToolDamagePerAttempt = 0.08f;
        public const float ToolDamageOnFail = 0.15f;
        public const int MaxDifficulty = 6;
        public const int MinDifficulty = 1;

        private readonly SafeCrackingState _state = new SafeCrackingState();
        private readonly Dictionary<string, SafeInstanceState> _safes = new Dictionary<string, SafeInstanceState>();
        private readonly int _seed;

        // Events
        public event Action<string> OnSafeInspected;         // safeId
        public event Action<string, SafeAttemptResult> OnAttemptMade; // safeId, result
        public event Action<string> OnToolDamaged;           // safeId
        public event Action<string> OnNoiseGenerated;        // safeId
        public event Action<string> OnSafeOpened;            // safeId
        public event Action<string> OnSafeJammed;            // safeId
        public event Action<string> OnAlarmTriggered;        // safeId
        public event Action<string> OnLootTransferred;       // safeId
        public event Action<SafeCrackingState> OnStateChanged;

        public SafeCrackingState State => _state;
        public IReadOnlyDictionary<string, SafeInstanceState> Safes => _safes;

        public SafeCrackingSystem(int seed = 42)
        {
            _seed = seed;
        }

        // ── Safe registration ────────────────────────────────────────

        /// <summary>Register a safe from a definition. Called when a room is entered.</summary>
        public bool RegisterSafe(SafeDefinition def, string locationId)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return false;
            if (_safes.ContainsKey(def.id)) return false;

            var instance = new SafeInstanceState
            {
                safeId = def.id,
                locationId = locationId,
                roomId = def.roomId,
                difficulty = Math.Clamp(def.difficulty, MinDifficulty, MaxDifficulty),
                maxAttempts = Math.Max(1, def.maxAttempts),
                alarmThreshold = Math.Clamp(def.alarmThreshold, 0.1f, 1f),
                loot = new List<SafeLootEntry>(def.loot)
            };

            // Generate deterministic combination
            instance.combination = GenerateCombination(def.id, instance.difficulty);

            _safes[def.id] = instance;
            _state.safes.Add(instance);
            RaiseChanged();
            return true;
        }

        // ── Inspection ───────────────────────────────────────────────

        /// <summary>Inspect a safe (reveals difficulty and condition).</summary>
        public SafeInstanceState? InspectSafe(string safeId)
        {
            if (!_safes.TryGetValue(safeId, out var safe)) return null;
            OnSafeInspected?.Invoke(safeId);
            return safe;
        }

        // ── Attempt ──────────────────────────────────────────────────

        /// <summary>
        /// Make an attempt to open the safe with a guessed combination.
        /// The guess is an array of integers (one per tumbler, 0-9).
        /// Returns feedback including how many tumblers are correct.
        /// </summary>
        public SafeAttemptFeedback Attempt(string safeId, int[] guess, float toolCondition, ISeededRng rng)
        {
            if (!_safes.TryGetValue(safeId, out var safe))
                return new SafeAttemptFeedback { Result = SafeAttemptResult.InvalidInput, Message = "Unknown safe." };

            if (safe.isOpened)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.AlreadyOpened, Message = "Safe is already open." };

            if (safe.isJammed)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.Jammed, Message = "Safe mechanism is jammed." };

            if (guess == null || guess.Length != safe.difficulty)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.InvalidInput, Message = $"Guess must have {safe.difficulty} digits." };

            if (toolCondition < ToolDamagePerAttempt)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.ToolDamaged, Message = "Lockpick is too damaged to use." };

            safe.attemptsUsed++;

            // Count correct tumblers
            int correct = 0;
            for (int i = 0; i < safe.difficulty; i++)
            {
                if (guess[i] == safe.combination[i])
                    correct++;
            }

            // Generate noise
            float noiseIncrease = 0.1f + (safe.difficulty * 0.02f);
            if (rng != null) noiseIncrease += (float)rng.NextDouble() * 0.05f;
            safe.cumulativeNoise = Math.Min(1f, safe.cumulativeNoise + noiseIncrease);
            OnNoiseGenerated?.Invoke(safeId);

            // Tool damage
            float toolDamage = ToolDamagePerAttempt;
            if (correct == 0) toolDamage = ToolDamageOnFail; // worse damage on complete miss
            float newToolCondition = Math.Max(0f, toolCondition - toolDamage);

            // Check alarm
            if (safe.cumulativeNoise >= safe.alarmThreshold && !safe.alarmTriggered)
            {
                safe.alarmTriggered = true;
                OnAlarmTriggered?.Invoke(safeId);
            }

            // Check jam
            if (safe.attemptsUsed >= safe.maxAttempts && !safe.isOpened)
            {
                safe.isJammed = true;
                OnSafeJammed?.Invoke(safeId);
            }

            // Determine result
            SafeAttemptResult result;
            string message;

            if (correct == safe.difficulty)
            {
                // Success!
                safe.isOpened = true;
                safe.openedDay = 0; // caller should set day
                result = SafeAttemptResult.Success;
                message = "Safe opened!";
                OnSafeOpened?.Invoke(safeId);
            }
            else if (correct > 0)
            {
                result = SafeAttemptResult.PartialHint;
                message = $"{correct} of {safe.difficulty} tumblers correct.";
            }
            else
            {
                result = SafeAttemptResult.Failed;
                message = "No tumblers correct.";
            }

            if (newToolCondition < toolCondition && newToolCondition <= 0.2f)
            {
                OnToolDamaged?.Invoke(safeId);
            }

            OnAttemptMade?.Invoke(safeId, result);
            RaiseChanged();

            return new SafeAttemptFeedback
            {
                Result = result,
                CorrectTumblers = correct,
                TotalTumblers = safe.difficulty,
                NoiseLevel = safe.cumulativeNoise,
                ToolCondition = newToolCondition,
                Message = message
            };
        }

        // ── Accessibility mode ───────────────────────────────────────

        /// <summary>
        /// Accessibility alternate mode: simplified deterministic interaction.
        /// Uses the same Core outcome rules but with a simpler input (direction hints).
        /// The player provides a "confidence" value (0..1) and the system resolves
        /// based on skill, tool condition, and deterministic RNG.
        /// </summary>
        public SafeAttemptFeedback AttemptAccessible(string safeId, float confidence, float toolCondition, float skillLevel, ISeededRng rng)
        {
            if (!_safes.TryGetValue(safeId, out var safe))
                return new SafeAttemptFeedback { Result = SafeAttemptResult.InvalidInput, Message = "Unknown safe." };

            if (safe.isOpened)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.AlreadyOpened, Message = "Safe is already open." };

            if (safe.isJammed)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.Jammed, Message = "Safe mechanism is jammed." };

            if (toolCondition < ToolDamagePerAttempt)
                return new SafeAttemptFeedback { Result = SafeAttemptResult.ToolDamaged, Message = "Lockpick is too damaged to use." };

            safe.attemptsUsed++;

            // Success chance based on confidence, skill, and difficulty
            float baseChance = 0.15f + (confidence * 0.3f) + (skillLevel * 0.2f);
            float difficultyPenalty = safe.difficulty * 0.08f;
            float successChance = Math.Clamp(baseChance - difficultyPenalty, 0.05f, 0.6f);

            // Noise
            float noiseIncrease = 0.1f + (safe.difficulty * 0.02f);
            if (rng != null) noiseIncrease += (float)rng.NextDouble() * 0.05f;
            safe.cumulativeNoise = Math.Min(1f, safe.cumulativeNoise + noiseIncrease);
            OnNoiseGenerated?.Invoke(safeId);

            // Tool damage
            float toolDamage = ToolDamagePerAttempt;
            float newToolCondition = Math.Max(0f, toolCondition - toolDamage);

            // Alarm check
            if (safe.cumulativeNoise >= safe.alarmThreshold && !safe.alarmTriggered)
            {
                safe.alarmTriggered = true;
                OnAlarmTriggered?.Invoke(safeId);
            }

            // Jam check
            if (safe.attemptsUsed >= safe.maxAttempts && !safe.isOpened)
            {
                safe.isJammed = true;
                OnSafeJammed?.Invoke(safeId);
            }

            // Roll success
            bool success = rng != null && rng.NextDouble() < successChance;

            SafeAttemptResult result;
            string message;

            if (success)
            {
                safe.isOpened = true;
                result = SafeAttemptResult.Success;
                message = "Safe opened!";
                OnSafeOpened?.Invoke(safeId);
            }
            else
            {
                // Partial hint: how close were we?
                int correct = rng != null ? rng.Next(0, safe.difficulty) : 0;
                if (correct > 0)
                {
                    result = SafeAttemptResult.PartialHint;
                    message = $"Close — {correct} of {safe.difficulty} tumblers feel right.";
                }
                else
                {
                    result = SafeAttemptResult.Failed;
                    message = "The lock resists.";
                    toolDamage = ToolDamageOnFail;
                    newToolCondition = Math.Max(0f, toolCondition - toolDamage);
                }
            }

            OnAttemptMade?.Invoke(safeId, result);
            RaiseChanged();

            return new SafeAttemptFeedback
            {
                Result = result,
                CorrectTumblers = success ? safe.difficulty : 0,
                TotalTumblers = safe.difficulty,
                NoiseLevel = safe.cumulativeNoise,
                ToolCondition = newToolCondition,
                Message = message
            };
        }

        // ── Loot transfer ────────────────────────────────────────────

        /// <summary>
        /// Transfer loot from an opened safe. Returns the loot entries.
        /// Caller is responsible for adding to inventory.
        /// Can only be called once per safe.
        /// </summary>
        public List<SafeLootEntry>? TransferLoot(string safeId, ISeededRng rng)
        {
            if (!_safes.TryGetValue(safeId, out var safe)) return null;
            if (!safe.isOpened) return null;
            if (safe.lootTransferred) return null;

            var result = new List<SafeLootEntry>();
            foreach (var entry in safe.loot)
            {
                int qty = entry.minQuantity;
                if (entry.maxQuantity > entry.minQuantity && rng != null)
                {
                    qty = entry.minQuantity + rng.Next(0, entry.maxQuantity - entry.minQuantity + 1);
                }
                if (qty > 0)
                {
                    result.Add(new SafeLootEntry
                    {
                        itemId = entry.itemId,
                        minQuantity = qty,
                        maxQuantity = qty,
                        weightKg = entry.weightKg
                    });
                }
            }

            safe.lootTransferred = true;
            OnLootTransferred?.Invoke(safeId);
            RaiseChanged();
            return result;
        }

        // ── Abandon ──────────────────────────────────────────────────

        /// <summary>Abandon a safe (give up). Safe remains in current state.</summary>
        public bool Abandon(string safeId)
        {
            if (!_safes.TryGetValue(safeId, out var safe)) return false;
            // Just stop — no state change needed
            return true;
        }

        // ── Queries ──────────────────────────────────────────────────

        public SafeInstanceState? GetSafe(string safeId)
        {
            return _safes.TryGetValue(safeId, out var safe) ? safe : null;
        }

        public bool IsOpened(string safeId)
        {
            return _safes.TryGetValue(safeId, out var safe) && safe.isOpened;
        }

        public bool IsJammed(string safeId)
        {
            return _safes.TryGetValue(safeId, out var safe) && safe.isJammed;
        }

        // ── Combination generation ───────────────────────────────────

        /// <summary>
        /// Generate a deterministic combination from seed + safeId.
        /// Each tumbler is a value 0-9.
        /// </summary>
        private int[] GenerateCombination(string safeId, int difficulty)
        {
            var combo = new int[difficulty];
            int hash = _seed;
            for (int i = 0; i < safeId.Length; i++)
            {
                hash = unchecked(hash * 31 + safeId[i]);
            }
            for (int i = 0; i < difficulty; i++)
            {
                hash = unchecked(hash * 397 + i);
                combo[i] = Math.Abs(hash) % 10;
            }
            return combo;
        }

        // ── Save / Load ──────────────────────────────────────────────

        public SafeCrackingState CaptureState()
        {
            var copy = new SafeCrackingState
            {
                systemId = _state.systemId
            };
            var sorted = new List<SafeInstanceState>(_state.safes);
            sorted.Sort((a, b) => string.CompareOrdinal(a.safeId, b.safeId));
            foreach (var s in sorted)
            {
                copy.safes.Add(new SafeInstanceState
                {
                    safeId = s.safeId,
                    locationId = s.locationId,
                    roomId = s.roomId,
                    difficulty = s.difficulty,
                    attemptsUsed = s.attemptsUsed,
                    maxAttempts = s.maxAttempts,
                    cumulativeNoise = s.cumulativeNoise,
                    alarmThreshold = s.alarmThreshold,
                    isOpened = s.isOpened,
                    isJammed = s.isJammed,
                    alarmTriggered = s.alarmTriggered,
                    lootTransferred = s.lootTransferred,
                    openedDay = s.openedDay,
                    loot = new List<SafeLootEntry>(s.loot),
                    combination = (int[])s.combination.Clone()
                });
            }
            return copy;
        }

        public void RestoreState(SafeCrackingState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _safes.Clear();
            _state.safes.Clear();
            if (saved.safes != null)
            {
                foreach (var s in saved.safes)
                {
                    if (s == null || string.IsNullOrEmpty(s.safeId)) continue;
                    var copy = new SafeInstanceState
                    {
                        safeId = s.safeId,
                        locationId = s.locationId,
                        roomId = s.roomId,
                        difficulty = Math.Clamp(s.difficulty, MinDifficulty, MaxDifficulty),
                        attemptsUsed = Math.Max(0, s.attemptsUsed),
                        maxAttempts = Math.Max(1, s.maxAttempts),
                        cumulativeNoise = Math.Clamp(s.cumulativeNoise, 0f, 1f),
                        alarmThreshold = Math.Clamp(s.alarmThreshold, 0.1f, 1f),
                        isOpened = s.isOpened,
                        isJammed = s.isJammed,
                        alarmTriggered = s.alarmTriggered,
                        lootTransferred = s.lootTransferred,
                        openedDay = s.openedDay,
                        loot = s.loot != null ? new List<SafeLootEntry>(s.loot) : new List<SafeLootEntry>(),
                        combination = s.combination ?? Array.Empty<int>()
                    };
                    _safes[copy.safeId] = copy;
                    _state.safes.Add(copy);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
