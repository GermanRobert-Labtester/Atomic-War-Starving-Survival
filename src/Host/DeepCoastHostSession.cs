using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.Economy;
using Ashfall.Core.Journal;
using Ashfall.Core.Maritime;
using AtomicWar.Journal;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the District 8 deep-coast route (Exp 01
    /// sibling layer). No gameplay rules here: every decision, gate, roll and
    /// consequence lives in Ashfall.Core.District8DeepCoastSystem. This host
    /// only wires the canonical authorities — the real JournalSystem (once-only
    /// keys), the FactionStanceEngine (fleet/office standing), the Holdfast
    /// trade inventory (consumption + rewards), and the existing maritime dive
    /// session (StealthDiveInstance + MaritimeSaveStore).
    /// </summary>
    public sealed class DeepCoastHostSession
    : HostSessionBase{
        public const int DemoSeed = 4048;

        public District8DeepCoastSystem DeepCoast { get; }
        public JournalSystem Journal { get; }
        public FactionStanceEngine Stances { get; }
        public HoldfastTradeInventory Inventory { get; }
        public MaritimeHostSession Maritime { get; }

        private readonly ISeededRng _rng;
        private readonly ProceduralScavengeSystem _dockScavenge;
        private readonly List<VariableLootNode> _dockLoot;
        private readonly DemoSurvivor _author;
        public string LastEvent { get; private set; } = string.Empty;
        public DeepCoastHostSession(
            District8DeepCoastSystem deepCoast = null!,
            JournalSystem journal = null!,
            FactionStanceEngine stances = null!,
            HoldfastTradeInventory inventory = null!,
            MaritimeHostSession maritime = null!)
        {
            DeepCoast = deepCoast ?? new District8DeepCoastSystem(DemoSeed);
            Journal = journal ?? new JournalSystem();
            Stances = stances ?? new FactionStanceEngine();
            Inventory = inventory ?? new HoldfastTradeInventory();
            // Determinism: the dock salvage stream shares the same seeded rng as
            // the route decisions, so same seed + same actions ⇒ same rewards.
            _rng = new SeededRng(DemoSeed);
            _dockScavenge = new ProceduralScavengeSystem(_rng);
            _dockLoot = BuildDockLoot();
            Maritime = maritime ?? new MaritimeHostSession();
            _author = new DemoSurvivor("dc8_survey_party", "The Survey Party", RiskBiasTrait.Realist);

            DeepCoast.OnStateChanged += () => RaiseStateChanged();
            Maritime.StateChanged += () => RaiseStateChanged();
        }

        public static DeepCoastHostSession Create(
            District8DeepCoastSystem deepCoast = null!,
            JournalSystem journal = null!,
            FactionStanceEngine stances = null!,
            HoldfastTradeInventory inventory = null!,
            MaritimeHostSession maritime = null!)
        {
            return new DeepCoastHostSession(deepCoast, journal, stances, inventory, maritime);
        }

        // ── Actions (each one a single, idempotent stage step) ─────────

        public string Survey(int day)
        {
            if (DeepCoast.SurveyPerimeter(day))
            {
                LastEvent = "The breakwater is surveyed and logged. The yard is sealed but read.";
                Note(District8DeepCoastSystem.JournalSurvey, "We walked the boom with a clipboard and a crowbar. The padlock has rusted into the hasp like it was welded there. Below the Harbour Commission sign someone has written, in fresher paint, THE COMMISSION LEFT. We logged the chain and came back with coolant in our lungs and the smell of old fuel on our coats. The deep coast is a yard again, on paper.");
                return LastEvent;
            }
            return "Survey refused: the perimeter has already been surveyed (or the route is not at that stage).";
        }

        public string Decide(string decisionId, int day)
        {
            var decision = ParseDecision(decisionId);
            if (decision == DeepCoastAccessDecision.None)
                return "Unknown decision: " + (decisionId ?? "null") + " (stabilize | salvage | fleet | municipal).";

            var outcome = DeepCoast.MakeReopeningDecision(decision, day, _rng);
            if (outcome == null)
                return "Decision refused: the route has not been surveyed (or a decision is already recorded).";

            // Immediate consequences: faction standing through the canonical engine.
            if (outcome.FleetTrustDelta != 0f)
                Stances.ModifyTrust(District8DeepCoastSystem.FactionFleet, outcome.FleetTrustDelta);
            if (outcome.OfficeTrustDelta != 0f)
                Stances.ModifyTrust(District8DeepCoastSystem.FactionOffice, outcome.OfficeTrustDelta);

            // Immediate salvage (salvage-immediate path) through the canonical inventory.
            if (outcome.Salvage.Count > 0)
            {
                for (int i = 0; i < outcome.Salvage.Count; i++)
                    Inventory.AddItem(outcome.Salvage[i].ItemId, outcome.Salvage[i].Quantity);
            }

            Note(outcome.NarrativeKey, DecisionText(outcome));
            LastEvent = DecisionSummary(outcome);
            RaiseStateChanged();
            return LastEvent;
        }

        public string ClearPerimeter(int day)
        {
            if (DeepCoast.TryClearPerimeter(day, TryConsumeBillAtomic))
            {
                LastEvent = "The perimeter boom is down. The channel mouth is open.";
                return LastEvent;
            }
            var bill = DeepCoast.NextStepBill();
            return "Perimeter clearing refused — missing materials: " + BillText(bill)
                + " (or the route is not at the surveyed stage).";
        }

        public string ClearChannel(int day)
        {
            if (DeepCoast.TryClearServiceChannel(day, TryConsumeBillAtomic))
            {
                LastEvent = "The service channel is cut and winched clear. The deep berth is reachable.";
                Note(District8DeepCoastSystem.JournalDockOpen, "We cut the slip open. The water came up black and still and the ice came away in blue sheets that rang when they broke. A pallet of coiled rope still hangs on the gantry trolley, five years dry and still greased. The yard is open past the berths now. The Northern Sound dock is hull-down beyond the quay, answering a schedule nobody reads.");
                return LastEvent;
            }
            var bill = DeepCoast.NextStepBill();
            return "Channel clearing refused — missing materials: " + BillText(bill)
                + " (or the perimeter is not open).";
        }

        public CommandResult RepairBerth(int day)
        {
            if (DeepCoast.TryRepairDeepBerth(day, TryConsumeBillAtomic))
            {
                LastEvent = "Berth 9 is operational: winch, hose reels, mooring cable. Dock work can begin.";
                Note(District8DeepCoastSystem.JournalBerthOperational, "The winch housing opened with the square key — the Commission kept their keys, and someone left a copy under the cable drum. We greased the bitts, re-reeled the hose, and ran the mooring cable end to end. The brass plate says BERTH 9 — ICEBREAKER MAINTENANCE. It says so again now, for the first time in five years, in a way that means work.");
                return CommandResult.FromSuccess(
                    PlayerCommandCode.RepairBerth,
                    ActionResult.Success("deepcoast.berth_repaired"),
                    StateVersion, StateVersion + 1);
            }
            var bill = DeepCoast.NextStepBill();
            LastEvent = "Berth repair refused — missing materials: " + BillText(bill)
                + " (or the dock is not accessible / the structure is too damaged).";
            return new CommandResult(
                PlayerCommandCode.RepairBerth,
                ActionResult.Failed("missing_materials", "deepcoast.missing_materials"),
                StateVersion, StateVersion);
        }

        // ── Dock operation: expedition handoff → existing dive system ─

        public string StartDockDive(string diverId, string operatorId, int day)
        {
            string opId = "dc8_dock_op_" + diverId;
            if (!DeepCoast.TryStartDockOperation(opId, diverId, day))
                return "Dock operation refused: the berth is not operational, or an operation is already active.";

            // The dive itself is the existing StealthDiveInstance (MaritimeSaveStore owns it).
            Maritime.Dive.StartDive(diverId, operatorId, 120f);
            Note(District8DeepCoastSystem.JournalDiveLaunched, "First dive from Berth 9. The water under the quay is black and cold and the ice does not want us in it. The winch clicks at the top of every crank. Whatever is down there has been waiting five years. It can wait through one more day of us being careful.");
            LastEvent = $"Dock dive launched from Berth 9: {diverId} down, {operatorId} on the compressor (site site_exp09_naval_patrol).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string TickDockDive(float seconds)
        {
            if (!Maritime.Dive.IsActive) return "No active dock dive.";
            Maritime.Dive.Tick(seconds);
            LastEvent = $"Dock dive: air {Maritime.Dive.AirSupplySeconds:F0}s · room {Maritime.Dive.CurrentRoomIndex + 1}/4.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string CrankDockDive()
        {
            if (!Maritime.Dive.IsActive) return "No active dock dive.";
            Maritime.Dive.CrankCompressor();
            LastEvent = $"Compressor cranked. Air {Maritime.Dive.AirSupplySeconds:F0}s.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AdvanceDockDive(int noise)
        {
            if (!Maritime.Dive.IsActive) return "No active dock dive.";
            bool ok = Maritime.Dive.AdvanceToNextRoom(noise);
            LastEvent = ok
                ? $"Diver advanced to room {Maritime.Dive.CurrentRoomIndex + 1} (noise {Maritime.Dive.NoiseLevel})."
                : "Cannot advance further.";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Completes the dock operation. When <paramref name="rewards"/> is null,
        /// the reward content is rolled through the existing
        /// ProceduralScavengeSystem.RollLootTable (canonical dock loot, degraded
        /// items swap to canonical degraded ids) — same engine, same rng stream.
        /// The Fleet levy (when fleet-controlled) is deducted here and the rest
        /// goes to the canonical inventory.
        /// </summary>
        public string CompleteDockDive(bool success, List<SalvageEntry> rewards = null!, int day = 1)
        {
            if (!Maritime.Dive.IsActive) return "No active dock dive to complete.";
            Maritime.Dive.EndDive(success);

            float levy = 0f;
            DeepCoast.TryEndDockOperation(success, out levy);

            if (success)
            {
                var resolved = rewards ?? RollDockScavenge(day);
                for (int i = 0; i < resolved.Count; i++)
                {
                    var r = resolved[i];
                    if (r == null || string.IsNullOrEmpty(r.ItemId) || r.Quantity <= 0) continue;
                    int afterLevy = levy > 0f
                        ? Math.Max(0, r.Quantity - (int)(r.Quantity * levy))
                        : r.Quantity;
                    if (afterLevy > 0) Inventory.AddItem(r.ItemId, afterLevy);
                }
                LastEvent = levy > 0f
                    ? "Dive recovered. The Fleet's levy takes its share of the salvage; the rest is ashore."
                    : "Dive recovered. Salvage is ashore and logged.";
            }
            else
            {
                LastEvent = "Dive aborted. The water keeps what it keeps.";
            }
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Rolls the dock dive rewards through the existing procedural scavenge
        /// engine. Degraded rolls swap to canonical degraded ids
        /// (spoiled_canned_food, irradiated_water); contaminated rolls are
        /// flagged on the result the same way the maritime host does.
        /// </summary>
        private List<SalvageEntry> RollDockScavenge(int day)
        {
            _dockScavenge.SetCurrentDay(day);
            var rolls = _dockScavenge.RollLootTable(
                District8DeepCoastSystem.DockId,
                _dockLoot,
                DeepCoast.RadsPerHour(District8DeepCoastSystem.DockId),
                hasBioHazard: DeepCoast.ContaminationLevel > 0.5f);
            var rewards = new List<SalvageEntry>();
            for (int i = 0; i < rolls.Count; i++)
            {
                var r = rolls[i];
                if (r == null || r.Quantity <= 0) continue;
                rewards.Add(new SalvageEntry(
                    r.IsDegraded && !string.IsNullOrEmpty(r.DegradedItemId) ? r.DegradedItemId : r.ItemId,
                    r.Quantity));
            }
            return rewards;
        }

        private static List<VariableLootNode> BuildDockLoot()
        {
            // Canonical items only; degraded swaps resolve to real catalog ids.
            return new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap_metal", MinQty = 2, MaxQty = 5, SpawnChance = 0.7f, DegradationChance = 0.15f, DegradedItemId = "scrap_metal", Description = "Hull plate and rigging scrap, frozen into the quay ice." },
                new VariableLootNode { ItemId = "brass_fittings", MinQty = 1, MaxQty = 3, SpawnChance = 0.45f, DegradationChance = 0.1f, DegradedItemId = "scrap_metal", Description = "Fittings the crane crew never logged." },
                new VariableLootNode { ItemId = "canned_food", MinQty = 2, MaxQty = 4, SpawnChance = 0.4f, DegradationChance = 0.35f, DegradedItemId = "spoiled_canned_food", Description = "Galley stores, labels bleached by salt air." },
                new VariableLootNode { ItemId = "clean_water", MinQty = 2, MaxQty = 4, SpawnChance = 0.35f, DegradationChance = 0.2f, DegradedItemId = "irradiated_water", Description = "Jugs on a pallet, frozen into place." },
                new VariableLootNode { ItemId = "item_ro_resin", MinQty = 1, MaxQty = 2, SpawnChance = 0.2f, DegradationChance = 0f, DegradedItemId = string.Empty, Description = "Resin drums off the berth hose reels." }
            };
        }

        // ── Gating for the expedition/map flow ────────────────────────

        /// <summary>
        /// Route gating on top of IceRoadSystem's seasonal gate. The ice road
        /// owns seasonal access (loc_shelf_ prefix); this owns the reopening
        /// stage. Expedition dispatch consults both.
        /// </summary>
        public bool IsRouteNodeBlocked(string nodeId)
        {
            if (!DeepCoast.IsDeepCoastNode(nodeId)) return false;
            return !DeepCoast.IsNodeAccessible(nodeId);
        }

        /// <summary>True once the existing Northern Sound dock is a legal expedition target.</summary>
        public bool DockExpeditionAvailable => DeepCoast.IsNodeAccessible(District8DeepCoastSystem.DockId);

        /// <summary>True after the fleet-controlled decision: the Fleet came ashore.</summary>
        public bool IsFleetActive => DeepCoast.IsFleetStoodUp;

        /// <summary>
        /// Daily host tick: advances the deep-coast degradation (idempotent per
        /// calendar day in Core) and syncs the scavenge engine's world day so
        /// dock salvage degradation tracks the real timeline. The weather only
        /// shapes the brine-decay rate (Rain/FalseSpring recede faster).
        /// </summary>
        public void TickDaily(int day, WeatherKind weather = WeatherKind.Clear)
        {
            _lastDay = day > 0 ? day : 1;
            DeepCoast.TickDaily(_lastDay, weather);
            _dockScavenge.SetCurrentDay(_lastDay);
        }

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("Deep Coast: ").Append(DeepCoast.Stage).Append(" · decision ")
                .Append(DeepCoast.AccessDecision);
            if (DeepCoast.StructuralIntegrity < 100f)
                sb.Append(" · structure ").Append(DeepCoast.StructuralIntegrity.ToString("F0")).Append("%");
            if (DeepCoast.ContaminationLevel > 0f)
                sb.Append(" · contamination ").Append(DeepCoast.ContaminationLevel.ToString("P0"));
            if (DeepCoast.IsFleetLevyActive) sb.Append(" · FLEET LEVY");
            if (DeepCoast.IsDockOperationActive)
                sb.Append(" · dive active (").Append(DeepCoast.ActiveDockOperationDiverId).Append(")");
            sb.Append(" · fleet trust ").Append(Stances.GetTrust(District8DeepCoastSystem.FactionFleet).ToString("F0"))
                .Append(" · office trust ").Append(Stances.GetTrust(District8DeepCoastSystem.FactionOffice).ToString("F0"));
            return sb.ToString();
        }

        // ── Save / Load ───────────────────────────────────────────────

        public District8DeepCoastState CaptureDeepCoast() => DeepCoast.CaptureState();
        public void RestoreDeepCoast(District8DeepCoastState state) => DeepCoast.RestoreState(state);

        // ── Helpers ───────────────────────────────────────────────────

        private bool TryConsumeBillAtomic(IReadOnlyDictionary<string, int> bill)
        {
            return Inventory.TryConsumeBill(bill);
        }

        private bool TryConsumeAtomic(string itemId, int qty)
        {
            return Inventory.TryConsumeBill(new Dictionary<string, int>(StringComparer.Ordinal) { { itemId, qty } });
        }

        private void Note(string key, string text)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(text)) return;
            Journal.TryAddRawEntry(key, text, _author, DayNow);
        }

        private int DayNow
        {
            get
            {
                // The host passes the sim day for actions; journal day is informational.
                return _lastDay > 0 ? _lastDay : 1;
            }
        }
        private int _lastDay = 1;

        public void SetCurrentDay(int day) => _lastDay = day > 0 ? day : 1;

        private static DeepCoastAccessDecision ParseDecision(string id)
        {
            switch (id)
            {
                case "stabilize": return DeepCoastAccessDecision.StabilizeRepair;
                case "salvage": return DeepCoastAccessDecision.SalvageImmediate;
                case "fleet": return DeepCoastAccessDecision.FleetControlled;
                case "municipal": return DeepCoastAccessDecision.MunicipalControlled;
                default: return DeepCoastAccessDecision.None;
            }
        }

        private static string DecisionText(DeepCoastDecisionOutcome outcome)
        {
            switch (outcome.Decision)
            {
                case DeepCoastAccessDecision.StabilizeRepair:
                    return "We chose the slow work: shore the boom, log the chain, open the yard on paper before we open it in fact. The Office will have its audit trail. It always does.";
                case DeepCoastAccessDecision.SalvageImmediate:
                    return "We cut the boom fast and took what came up before the ice knew we were there. The water is angrier now. The concrete is not what it was.";
                case DeepCoastAccessDecision.FleetControlled:
                    return "We gave the Fleet the yard. They stood up the way a schedule stands up — all at once, and already counting. The levy is the price of not doing the work ourselves.";
                case DeepCoastAccessDecision.MunicipalControlled:
                    return "We kept the yard municipal. The Office approves. The yard stays ours, which means the repairs stay ours, and the berth stays ours, and the Fleet stays out on the sound.";
                default:
                    return "The yard is open.";
            }
        }

        private static string DecisionSummary(DeepCoastDecisionOutcome outcome)
        {
            var sb = new System.Text.StringBuilder("Decision recorded: ").Append(outcome.Decision).Append('.');
            if (outcome.FleetTrustDelta != 0f)
                sb.Append(" Fleet ").Append(Signed(outcome.FleetTrustDelta)).Append('.');
            if (outcome.OfficeTrustDelta != 0f)
                sb.Append(" Office ").Append(Signed(outcome.OfficeTrustDelta)).Append('.');
            if (outcome.Salvage.Count > 0)
            {
                sb.Append(" Salvage:");
                for (int i = 0; i < outcome.Salvage.Count; i++)
                    sb.Append(' ').Append(outcome.Salvage[i].Quantity).Append("×")
                        .Append(outcome.Salvage[i].ItemId).Append(',');
            }
            return sb.ToString().TrimEnd(',');
        }

        private static string BillText(Dictionary<string, int> bill)
        {
            if (bill == null || bill.Count == 0) return "none";
            var sb = new System.Text.StringBuilder();
            foreach (var kv in bill)
                sb.Append(kv.Value).Append("×").Append(kv.Key).Append(' ');
            return sb.ToString().TrimEnd();
        }

        private static string Signed(float v) => v > 0f ? "+" + v.ToString("F0") : v.ToString("F0");
    }
}
