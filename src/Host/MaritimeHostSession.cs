using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Maritime;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Maritime suite (Expansion 09 — The Black
    /// Flottilla). Wires the four engine-agnostic maritime systems that were
    /// selftest-only: stealth dive, procedural scavenge, psychological
    /// contamination, and the variable loot node definitions. No gameplay
    /// rules here — hosts only present and wire.
    /// </summary>
    public sealed class MaritimeHostSession
    : HostSessionBase{
        public const int DemoSeed = 9909;

        public StealthDiveInstance Dive { get; }
        public ProceduralScavengeSystem Scavenge { get; }
        public PsychologicalContaminationSystem Psychology { get; }
        public SafeCrackingSystem SafeCrack { get; }
        public List<VariableLootNode> LootNodes { get; } = new List<VariableLootNode>();

        public string LastEvent { get; private set; } = string.Empty;
        public MaritimeHostSession(
            StealthDiveInstance dive = null!,
            ProceduralScavengeSystem scavenge = null!,
            PsychologicalContaminationSystem psychology = null!,
            SafeCrackingSystem safeCrack = null!)
        {
            Dive = dive ?? new StealthDiveInstance();
            Scavenge = scavenge ?? new ProceduralScavengeSystem(new SeededRng(DemoSeed));
            Psychology = psychology ?? new PsychologicalContaminationSystem();
            SafeCrack = safeCrack ?? new SafeCrackingSystem(DemoSeed);
            SeedLootNodes();
            WireEvents();
        }

        private void WireEvents()
        {
            Dive.OnDiveEnded += _ => { LastEvent = "Dive ended."; RaiseStateChanged(); };
            Dive.OnRoomEntered += _ => { LastEvent = "Dive moved to next room."; RaiseStateChanged(); };
            Dive.OnAirWarning += air => { LastEvent = $"WARNING: Low oxygen ({air:F0}s remaining)!"; RaiseStateChanged(); };
            Dive.OnDecompressionStarted += req => { LastEvent = $"Decompression stop initiated ({req:F0}s required)."; RaiseStateChanged(); };
            Dive.OnDecompressionCompleted += () => { LastEvent = "Decompression stop cleared."; RaiseStateChanged(); };
            Dive.OnDiverLost += id => { LastEvent = $"CRITICAL: Diver {id} lost in deep hull!"; RaiseStateChanged(); };
            Scavenge.OnLootRolled += (_, _, _) => RaiseStateChanged();
            Scavenge.OnItemDegraded += (_, _) => RaiseStateChanged();
            Psychology.OnContaminationApplied += (_, _) => RaiseStateChanged();
            Psychology.OnContaminationExpired += (_, _) => RaiseStateChanged();
            SafeCrack.OnStateChanged += _ => RaiseStateChanged();
            SafeCrack.OnSafeOpened += id => { LastEvent = $"Safe {id} opened!"; RaiseStateChanged(); };
            SafeCrack.OnSafeJammed += id => { LastEvent = $"Safe {id} jammed!"; RaiseStateChanged(); };
            SafeCrack.OnAlarmTriggered += id => { LastEvent = $"Alarm triggered at safe {id}!"; RaiseStateChanged(); };
        }

        public static MaritimeHostSession Create(string dataDir)
        {
            var session = new MaritimeHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var catalog = DiveSiteCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                session.Dive.LoadCatalog(catalog);
            }
            var save = MaritimeSaveStore.TryLoad();
            if (save != null)
            {
                session.Dive.RestoreState(save.Dive);
                session.Scavenge.RestoreState(save.Scavenge);
                session.Psychology.RestoreState(save.Psychology);
                if (save.SafeCrack != null) session.SafeCrack.RestoreState(save.SafeCrack);
                session.LastEvent = "Maritime state restored from save.";
            }
            return session;
        }

        private void SeedLootNodes()
        {
            LootNodes.Add(new VariableLootNode
            {
                ItemId = "item_ro_resin", MinQty = 1, MaxQty = 3, SpawnChance = 0.35f,
                DegradationChance = 0.2f, DegradedItemId = "scrap_mechanical",
                Description = "Wrapped in oilcloth. The resin still smells faintly of the plant."
            });
            LootNodes.Add(new VariableLootNode
            {
                ItemId = "item_process_barrel", MinQty = 1, MaxQty = 2, SpawnChance = 0.25f,
                DegradationChance = 0.15f, DegradedItemId = "scrap_metal",
                Description = "A ribbed poly barrel, sealed with tar."
            });
            LootNodes.Add(new VariableLootNode
            {
                ItemId = "canned_food", MinQty = 2, MaxQty = 5, SpawnChance = 0.45f,
                DegradationChance = 0.3f, DegradedItemId = "spoiled_canned_food",
                Description = "Labels bleached by salt air. The rims are still sealed."
            });
            LootNodes.Add(new VariableLootNode
            {
                ItemId = "clean_water", MinQty = 2, MaxQty = 4, SpawnChance = 0.4f,
                DegradationChance = 0.1f, DegradedItemId = "irradiated_water",
                Description = "Jugs stacked against the bulkhead, condensation beaded on them."
            });
        }

        // ── Production Maritime Actions ──────────────────────────────

        public string StartDive(string diverId, string operatorId, float initialAir = 120f, string siteId = "")
        {
            Dive.StartDive(diverId, operatorId, initialAir, siteId);
            LastEvent = $"Dive started: {diverId} (air {initialAir:F0}s).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string TickDive(float seconds)
        {
            if (!Dive.IsActive) return "No active dive.";
            Dive.Tick(seconds);
            LastEvent = $"Dive tick {seconds}s · air {Dive.AirSupplySeconds:F0}s · room {Dive.CurrentRoomIndex + 1}/4.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string TickDiveDemo(float seconds) => TickDive(seconds);

        public string CrankDiveCompressor()
        {
            if (!Dive.IsActive) return "No active dive.";
            Dive.CrankCompressor();
            LastEvent = $"Compressor cranked. Air {Dive.AirSupplySeconds:F0}s.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string CrankDiveDemo() => CrankDiveCompressor();

        public string AdvanceDiveRoom(int noise = 10)
        {
            if (!Dive.IsActive) return "No active dive.";
            bool ok = Dive.AdvanceToNextRoom(noise);
            LastEvent = ok
                ? $"Advanced to room {Dive.CurrentRoomIndex + 1} (noise {Dive.NoiseLevel})."
                : "Cannot advance (at the deep hold or dive inactive).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AdvanceDiveDemo(int noise) => AdvanceDiveRoom(noise);

        public string AbortDive(bool emergency = false)
        {
            if (!Dive.IsActive) return "No active dive.";
            Dive.AbortDive(emergency);
            LastEvent = emergency ? "Emergency ascent aborted!" : "Controlled ascent completed.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AbortDiveDemo(bool emergency = false) => AbortDive(emergency);

        public string Decompress(float seconds = 10f)
        {
            if (!Dive.IsActive) return "No active dive.";
            Dive.StartDecompression();
            Dive.Tick(seconds);
            LastEvent = $"Decompression stop: {Dive.DecompressionProgressSeconds:F1}s / {Dive.DecompressionRequiredSeconds:F1}s.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string DecompressDemo(float seconds = 10f) => Decompress(seconds);

        public string ScavengeLocation(string locationId)
        {
            Scavenge.SetCurrentDay(1);
            var rolls = Scavenge.RollLootTable(locationId, LootNodes, locationRads: 2f, hasBioHazard: false);
            if (rolls == null || rolls.Count == 0)
            {
                LastEvent = "Scavenge found nothing at " + locationId + ".";
                return LastEvent;
            }
            var sb = new System.Text.StringBuilder("Scavenged at " + locationId + ":");
            for (int i = 0; i < rolls.Count; i++)
                sb.Append("\n  ").Append(rolls[i].Quantity).Append(" × ").Append(rolls[i].ItemId)
                    .Append(rolls[i].IsDegraded ? " (degraded)" : "")
                    .Append(rolls[i].IsContaminated ? " (contaminated)" : "");
            LastEvent = sb.ToString();
            RaiseStateChanged();
            return LastEvent;
        }

        public string ScavengeDemo(string locationId) => ScavengeLocation(locationId);

        public string Contaminate(string survivorId, string locationId)
        {
            Psychology.ApplyContamination(survivorId, locationId, moraleAtVisit: 50f, "generic");
            LastEvent = Psychology.HasContamination(survivorId, PsychologicalContaminationSystem.Contam_ThousandYardStare)
                ? survivorId + " shows the thousand-yard stare."
                : survivorId + " visited " + locationId + ".";
            RaiseStateChanged();
            return LastEvent;
        }

        public string ContaminateDemo(string survivorId, string locationId) => Contaminate(survivorId, locationId);

        // ── Safe cracking production actions ──────────────────────────

        /// <summary>Register a safe definition.</summary>
        public string RegisterSafe(SafeDefinition def, string locationId)
        {
            if (def == null) return "Invalid safe definition.";
            bool ok = SafeCrack.RegisterSafe(def, locationId);
            return ok ? $"Safe {def.id} registered at {locationId}." : $"Safe {def.id} already registered.";
        }

        public string RegisterSafeDemo(string safeId = "safe_km19_oil_tin", string locationId = "loc_cut_kilometre_19", string roomId = "room_km19_oil_tin")
        {
            var def = new SafeDefinition
            {
                id = safeId,
                displayName = "Locked Safe",
                roomId = roomId,
                difficulty = 3,
                maxAttempts = 10,
                noisePerAttempt = 0.2f,
                alarmThreshold = 0.8f,
                loot = new List<SafeLootEntry>
                {
                    new SafeLootEntry { itemId = "scrap_metal", minQuantity = 2, maxQuantity = 5, weightKg = 1f },
                    new SafeLootEntry { itemId = "clean_water", minQuantity = 1, maxQuantity = 3, weightKg = 1.5f },
                    new SafeLootEntry { itemId = "bandages", minQuantity = 1, maxQuantity = 2, weightKg = 0.5f }
                }
            };
            return RegisterSafe(def, locationId);
        }

        /// <summary>Inspect a safe.</summary>
        public string InspectSafe(string safeId)
        {
            var safe = SafeCrack.InspectSafe(safeId);
            if (safe == null) return $"Unknown safe: {safeId}";
            return $"Safe {safeId}: difficulty={safe.difficulty}, attempts={safe.attemptsUsed}/{safe.maxAttempts}, " +
                   $"noise={safe.cumulativeNoise:F2}, opened={safe.isOpened}, jammed={safe.isJammed}";
        }

        public string InspectSafeDemo(string safeId) => InspectSafe(safeId);

        /// <summary>Attempt to open a safe with a guess.</summary>
        public string AttemptSafe(string safeId, int[] guess, float toolCondition = 1.0f)
        {
            var rng = new CoreSeededRng(SafeCrack.State.safes.Count * 31 + StableHash.Of(safeId));
            var feedback = SafeCrack.Attempt(safeId, guess, toolCondition, rng);
            LastEvent = $"Safe attempt: {feedback.Message} (correct: {feedback.CorrectTumblers}/{feedback.TotalTumblers}, noise: {feedback.NoiseLevel:F2})";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AttemptSafeDemo(string safeId, int[] guess, float toolCondition = 1.0f)
            => AttemptSafe(safeId, guess, toolCondition);

        /// <summary>Attempt accessible mode.</summary>
        public string AttemptSafeAccessible(string safeId, float confidence = 0.5f, float toolCondition = 1.0f, float skill = 0.3f)
        {
            var rng = new CoreSeededRng(SafeCrack.State.safes.Count * 31 + StableHash.Of(safeId));
            var feedback = SafeCrack.AttemptAccessible(safeId, confidence, toolCondition, skill, rng);
            LastEvent = $"Safe attempt (accessible): {feedback.Message} (noise: {feedback.NoiseLevel:F2})";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AttemptSafeAccessibleDemo(string safeId, float confidence = 0.5f, float toolCondition = 1.0f, float skill = 0.3f)
            => AttemptSafeAccessible(safeId, confidence, toolCondition, skill);

        /// <summary>Transfer loot from an opened safe.</summary>
        public string TransferSafeLoot(string safeId)
        {
            var rng = new CoreSeededRng(SafeCrack.State.safes.Count * 31 + StableHash.Of(safeId));
            var loot = SafeCrack.TransferLoot(safeId, rng);
            if (loot == null) return $"Cannot transfer loot from {safeId} (not opened or already transferred).";
            var sb = new System.Text.StringBuilder($"Loot from {safeId}:");
            foreach (var entry in loot)
                sb.Append($"\n  {entry.minQuantity} × {entry.itemId}");
            LastEvent = sb.ToString();
            RaiseStateChanged();
            return LastEvent;
        }

        public string StatusLine()
        {
            return
                $"Maritime: dive {(Dive.IsActive ? $"active (room {Dive.CurrentRoomIndex + 1}/4, air {Dive.AirSupplySeconds:F0}s)" : "idle")} · " +
                $"scavenge visits {Scavenge.GetVisitCount("stadium")} · " +
                $"loot nodes {LootNodes.Count}";
        }

        public MaritimeHostSave CaptureSave()
        {
            return new MaritimeHostSave
            {
                Dive = Dive.CaptureState(),
                Scavenge = Scavenge.CaptureState(),
                Psychology = Psychology.CaptureState(),
                SafeCrack = SafeCrack.CaptureState()
            };
        }

        public void RestoreSave(MaritimeHostSave save)
        {
            if (save == null) return;
            Dive.RestoreState(save.Dive);
            Scavenge.RestoreState(save.Scavenge);
            Psychology.RestoreState(save.Psychology);
            if (save.SafeCrack != null) SafeCrack.RestoreState(save.SafeCrack);
        }
    }

    /// <summary>Maritime host save envelope (four engine states + checksum).</summary>
    public class MaritimeHostSave
    {
        public StealthDiveSaveState Dive;
        public ProceduralScavengeSave Scavenge;
        public PsychContaminationSave Psychology;
        public SafeCrackingState SafeCrack;
        public string Checksum = string.Empty;
    }
}
