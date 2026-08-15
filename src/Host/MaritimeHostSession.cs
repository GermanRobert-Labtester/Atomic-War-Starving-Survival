using System;
using System.Collections.Generic;
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
    {
        public const int DemoSeed = 9909;

        public StealthDiveInstance Dive { get; }
        public ProceduralScavengeSystem Scavenge { get; }
        public PsychologicalContaminationSystem Psychology { get; }
        public List<VariableLootNode> LootNodes { get; } = new List<VariableLootNode>();

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public MaritimeHostSession(
            StealthDiveInstance dive = null,
            ProceduralScavengeSystem scavenge = null,
            PsychologicalContaminationSystem psychology = null)
        {
            Dive = dive ?? new StealthDiveInstance();
            Scavenge = scavenge ?? new ProceduralScavengeSystem(new SeededRng(DemoSeed));
            Psychology = psychology ?? new PsychologicalContaminationSystem();
            SeedLootNodes();
            WireEvents();
        }

        private void WireEvents()
        {
            Dive.OnDiveEnded += _ => { LastEvent = "Dive ended."; StateChanged?.Invoke(); };
            Dive.OnRoomEntered += _ => { LastEvent = "Dive moved to next room."; StateChanged?.Invoke(); };
            Scavenge.OnLootRolled += (_, _, _) => StateChanged?.Invoke();
            Scavenge.OnItemDegraded += (_, _) => StateChanged?.Invoke();
            Psychology.OnContaminationApplied += (_, _) => StateChanged?.Invoke();
            Psychology.OnContaminationExpired += (_, _) => StateChanged?.Invoke();
        }

        public static MaritimeHostSession Create(string dataDir)
        {
            var session = new MaritimeHostSession();
            var save = MaritimeSaveStore.TryLoad();
            if (save != null)
            {
                session.Dive.RestoreState(save.Dive);
                session.Scavenge.RestoreState(save.Scavenge);
                session.Psychology.RestoreState(save.Psychology);
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

        // ── Demo actions (headless / dev buttons) ─────────────────────

        public string StartDiveDemo(string diverId, string operatorId)
        {
            Dive.StartDive(diverId, operatorId, 120f);
            LastEvent = $"Dive started: {diverId} (air 120s).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string TickDiveDemo(float seconds)
        {
            if (!Dive.IsActive) return "No active dive.";
            Dive.Tick(seconds);
            LastEvent = $"Dive tick {seconds}s · air {Dive.AirSupplySeconds:F0}s · room {Dive.CurrentRoomIndex + 1}/4.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string CrankDiveDemo()
        {
            if (!Dive.IsActive) return "No active dive.";
            Dive.CrankCompressor();
            LastEvent = $"Compressor cranked. Air {Dive.AirSupplySeconds:F0}s.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string AdvanceDiveDemo(int noise)
        {
            if (!Dive.IsActive) return "No active dive.";
            bool ok = Dive.AdvanceToNextRoom(noise);
            LastEvent = ok
                ? $"Advanced to room {Dive.CurrentRoomIndex + 1} (noise {Dive.NoiseLevel})."
                : "Cannot advance (at the deep hold or dive inactive).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string ScavengeDemo(string locationId)
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
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string ContaminateDemo(string survivorId, string locationId)
        {
            Psychology.ApplyContamination(survivorId, locationId, moraleAtVisit: 50f, "generic");
            LastEvent = Psychology.HasContamination(survivorId, PsychologicalContaminationSystem.Contam_ThousandYardStare)
                ? survivorId + " shows the thousand-yard stare."
                : survivorId + " visited " + locationId + ".";
            StateChanged?.Invoke();
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
                Psychology = Psychology.CaptureState()
            };
        }

        public void RestoreSave(MaritimeHostSave save)
        {
            if (save == null) return;
            Dive.RestoreState(save.Dive);
            Scavenge.RestoreState(save.Scavenge);
            Psychology.RestoreState(save.Psychology);
        }
    }

    /// <summary>Maritime host save envelope (three engine states + checksum).</summary>
    public class MaritimeHostSave
    {
        public StealthDiveSaveState Dive;
        public ProceduralScavengeSave Scavenge;
        public PsychContaminationSave Psychology;
        public string Checksum = string.Empty;
    }
}
