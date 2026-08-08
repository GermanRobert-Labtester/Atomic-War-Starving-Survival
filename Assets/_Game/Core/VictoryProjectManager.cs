using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Long-term campaign escape goals + win/loss evaluation.
    /// Radio path: decrypt military intel → extraction coords → survive to day 100.
    /// Vehicle path: massive crafting sink → drive out.
    /// Loss: all survivors dead (death screen by cause).
    /// </summary>
    public class VictoryProjectManager
    {
        public const int IntelRequiredForExtraction = 10;
        public const int ChopperArrivalDay = 100;
        public const int VehiclePartsRequired = 50;
        public const int VehicleFuelRequired = 10;

        public const string EngineItemId = "engine";
        public const string MechanicalPartsId = ScrapMaterialIds.MechanicalParts;
        public const string FuelItemId = "fuel";
        public const string MilitaryFrequencyId = RadioFrequencySO.Ids.Military;

        private EndgameState _state = EndgameState.Ongoing;
        private DeathScreenKind _deathScreen = DeathScreenKind.None;
        private EndgameSummaryData _lastSummary;

        public EndgameState State => _state;
        public DeathScreenKind DeathScreen => _deathScreen;
        public bool IsTerminal => _state != EndgameState.Ongoing;

        public int MilitaryIntelDecrypted { get; private set; }
        public bool ExtractionUnlocked { get; private set; }
        public int ExtractionUnlockedDay { get; private set; }
        public bool EngineConsumed { get; private set; }
        public int MoralChoicesMade { get; private set; }
        public string TerminalReason { get; private set; } = string.Empty;
        public EndgameSummaryData LastSummary => _lastSummary;

        public event Action<EndgameState> OnStateChanged;
        public event Action OnExtractionUnlocked;
        public event Action<EndgameSummaryData> OnEndgameTriggered;

        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>Prompt #228 — Grease Monkey vehicle escape half cost + unlock.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        public int GetVehiclePartsRequired()
        {
            float mult = _personalQuests != null
                ? _personalQuests.GetVehicleEscapeCostMultiplier(_getSurvivors?.Invoke())
                : 1f;
            return Mathf.Max(1, Mathf.RoundToInt(VehiclePartsRequired * mult));
        }

        public int GetVehicleFuelRequired()
        {
            float mult = _personalQuests != null
                ? _personalQuests.GetVehicleEscapeCostMultiplier(_getSurvivors?.Invoke())
                : 1f;
            return Mathf.Max(1, Mathf.RoundToInt(VehicleFuelRequired * mult));
        }

        /// <summary>
        /// Record a decrypted intel node. Military-channel nodes count toward extraction.
        /// </summary>
        public bool NotifyIntel(IntelNode intel)
        {
            if (IsTerminal || intel == null) return false;
            if (!IsMilitaryIntel(intel)) return false;

            MilitaryIntelDecrypted++;
            if (!ExtractionUnlocked && MilitaryIntelDecrypted >= IntelRequiredForExtraction)
            {
                ExtractionUnlocked = true;
                ExtractionUnlockedDay = Math.Max(1, intel.ExtractedDay);
                OnExtractionUnlocked?.Invoke();
            }
            return true;
        }

        /// <summary>Test/helper: grant N military decrypts at once.</summary>
        public void GrantMilitaryIntel(int count, int day = 1)
        {
            if (IsTerminal || count <= 0) return;
            for (int i = 0; i < count; i++)
            {
                NotifyIntel(new IntelNode
                {
                    Id = $"mil_intel_{MilitaryIntelDecrypted + 1}",
                    Type = IntelType.TroopMovement,
                    SourceFrequencyId = MilitaryFrequencyId,
                    ExtractedDay = day,
                    ExpirationDay = day + 5,
                    Confidence = 0.7f,
                    Text = "Military channel fragment."
                });
            }
        }

        public void RecordMoralChoice()
        {
            if (IsTerminal) return;
            MoralChoicesMade++;
        }

        /// <summary>
        /// Day tick: if extraction coords are known and the bunker held until day 100, Rescued.
        /// </summary>
        public EndgameSummaryData TickDay(
            int day,
            IReadOnlyList<Survivor> survivors,
            Func<EndgameSummaryData, EndgameSummaryData> finalizeSummary = null)
        {
            if (IsTerminal) return _lastSummary;
            if (!ExtractionUnlocked || day < ChopperArrivalDay) return null;
            if (survivors == null || !AnyAlive(survivors)) return null;

            return Trigger(
                EndgameState.Rescued,
                DeathScreenKind.None,
                "The coordinates held. The chopper found the ash and the people under it.",
                day,
                survivors,
                finalizeSummary);
        }

        /// <summary>
        /// Consume vehicle project materials and escape if requirements are met.
        /// Requires 50 mechanical_parts, 10 fuel, and one repaired engine.
        /// </summary>
        public EndgameSummaryData TryEscapeByVehicle(
            Inventory.Inventory inventory,
            Func<string, ItemDefinition> itemLookup,
            int day,
            IReadOnlyList<Survivor> survivors,
            Func<EndgameSummaryData, EndgameSummaryData> finalizeSummary = null)
        {
            if (IsTerminal) return _lastSummary;
            if (inventory == null || itemLookup == null) return null;
            if (!CanEscapeByVehicle(inventory)) return null;

            var parts = itemLookup(MechanicalPartsId);
            var fuel = itemLookup(FuelItemId);
            var engine = itemLookup(EngineItemId);
            if (parts == null || fuel == null || engine == null) return null;

            int partsNeed = GetVehiclePartsRequired();
            int fuelNeed = GetVehicleFuelRequired();
            if (!inventory.Remove(parts, partsNeed)) return null;
            if (!inventory.Remove(fuel, fuelNeed))
            {
                inventory.Add(parts, partsNeed); // rollback
                return null;
            }
            if (!inventory.Remove(engine, 1))
            {
                inventory.Add(parts, partsNeed);
                inventory.Add(fuel, fuelNeed);
                return null;
            }

            EngineConsumed = true;
            return Trigger(
                EndgameState.Escaped,
                DeathScreenKind.None,
                "The engine caught. Wheels on ash. The wasteland fell behind.",
                day,
                survivors,
                finalizeSummary,
                vehicleEscape: true);
        }

        public bool CanEscapeByVehicle(Inventory.Inventory inventory)
        {
            if (inventory == null || IsTerminal) return false;
            if (inventory.CountById(MechanicalPartsId) < GetVehiclePartsRequired()) return false;
            if (inventory.CountById(FuelItemId) < GetVehicleFuelRequired()) return false;
            // Prompt #228 — Grease Monkey instantly unlocks vehicle escape path.
            if (_personalQuests != null
                && _personalQuests.UnlocksVehicleEscape(_getSurvivors?.Invoke())
                && HasRepairedEngine(inventory))
                return true;
            return HasRepairedEngine(inventory);
        }

        /// <summary>
        /// True when inventory holds an engine that is not broken/degraded.
        /// </summary>
        public static bool HasRepairedEngine(Inventory.Inventory inventory)
        {
            if (inventory?.Slots == null) return false;
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (slot?.Item == null || slot.Item.id != EngineItemId) continue;
                if (slot.IsBrokenOrDegraded()) continue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// If every survivor is dead, classify loss and freeze the campaign.
        /// </summary>
        public EndgameSummaryData EvaluateLoss(
            IReadOnlyList<Survivor> survivors,
            int day,
            Func<EndgameSummaryData, EndgameSummaryData> finalizeSummary = null)
        {
            if (IsTerminal) return _lastSummary;
            if (survivors == null || survivors.Count == 0) return null;
            if (AnyAlive(survivors)) return null;

            ClassifyDeaths(survivors, out var state, out var screen, out var reason);
            return Trigger(state, screen, reason, day, survivors, finalizeSummary);
        }

        /// <summary>
        /// Prompt #20 — Lifeboat Transmission resolved: one extracted, rest already dead.
        /// Bittersweet victory; mutually exclusive with full Rescued / Escaped for this run.
        /// </summary>
        public EndgameSummaryData ApplyLifeboat(
            string extractedName,
            int leftBehindCount,
            int day,
            IReadOnlyList<Survivor> survivors,
            Func<EndgameSummaryData, EndgameSummaryData> finalizeSummary = null)
        {
            if (IsTerminal) return _lastSummary;
            string who = string.IsNullOrEmpty(extractedName) ? "One of us" : extractedName;
            string reason = leftBehindCount <= 0
                ? $"{who} walked into the ash toward the contact. The hatch closed behind them."
                : $"{who} walked into the ash toward the contact. {leftBehindCount} stayed. The hatch closed.";
            return Trigger(
                EndgameState.Lifeboat,
                DeathScreenKind.None,
                reason,
                day,
                survivors,
                finalizeSummary);
        }

        /// <summary>Build a summary DTO without changing state (for UI / tests).</summary>
        public EndgameSummaryData BuildSummary(
            int day,
            IReadOnlyList<Survivor> survivors,
            string reasonOverride = null)
        {
            float rad = 0f;
            int living = 0, dead = 0;
            string primary = string.Empty;
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var s = survivors[i];
                    if (s == null) continue;
                    rad += s.LifetimeRadiationExposure;
                    if (s.IsAlive) living++;
                    else dead++;
                    if (string.IsNullOrEmpty(primary) && !string.IsNullOrEmpty(s.DisplayName))
                        primary = s.DisplayName;
                }
            }

            var data = new EndgameSummaryData
            {
                State = _state,
                DeathScreen = _deathScreen,
                DaysSurvived = Math.Max(1, day),
                TotalRadiationAbsorbed = rad,
                MoralChoicesMade = MoralChoicesMade,
                MilitaryIntelDecrypted = MilitaryIntelDecrypted,
                ExtractionUnlocked = ExtractionUnlocked,
                VehicleEscapeUsed = EngineConsumed || _state == EndgameState.Escaped,
                LivingCount = living,
                DeadCount = dead,
                PrimaryAuthorName = primary ?? string.Empty,
                Reason = reasonOverride ?? TerminalReason ?? string.Empty
            };
            ApplyOutcomeCopy(data);
            return data;
        }

        /// <summary>
        /// Rebuild summary stats from a save snapshot (acceptance: post-game from SaveSystem data).
        /// Does not mutate campaign state unless <paramref name="applyTerminalState"/> is true.
        /// </summary>
        public static EndgameSummaryData FromSaveData(SaveData save, VictoryProjectSave victory = null)
        {
            var data = new EndgameSummaryData();
            if (save == null) return data;

            data.DaysSurvived = save.GameState != null ? Math.Max(1, save.GameState.Day) : 1;
            if (save.Survivors != null)
            {
                for (int i = 0; i < save.Survivors.Count; i++)
                {
                    var s = save.Survivors[i];
                    if (s == null) continue;
                    data.TotalRadiationAbsorbed += s.LifetimeRadiationExposure;
                    if (s.State == SurvivorState.Dead) data.DeadCount++;
                    else data.LivingCount++;
                    if (string.IsNullOrEmpty(data.PrimaryAuthorName) && !string.IsNullOrEmpty(s.DisplayName))
                        data.PrimaryAuthorName = s.DisplayName;
                }
            }

            var v = victory ?? save.VictoryProject;
            if (v != null)
            {
                data.State = v.State;
                data.DeathScreen = v.DeathScreen;
                data.MoralChoicesMade = v.MoralChoicesMade;
                data.MilitaryIntelDecrypted = v.MilitaryIntelDecrypted;
                data.ExtractionUnlocked = v.ExtractionUnlocked;
                data.VehicleEscapeUsed = v.EngineConsumed || v.State == EndgameState.Escaped;
                data.Reason = v.TerminalReason ?? string.Empty;
                if (v.DaysSurvived > 0) data.DaysSurvived = v.DaysSurvived;
                if (v.TotalRadiationAbsorbed > 0f) data.TotalRadiationAbsorbed = v.TotalRadiationAbsorbed;
            }

            ApplyOutcomeCopy(data);
            return data;
        }

        public VictoryProjectSave CaptureState()
        {
            return new VictoryProjectSave
            {
                State = _state,
                DeathScreen = _deathScreen,
                MilitaryIntelDecrypted = MilitaryIntelDecrypted,
                ExtractionUnlocked = ExtractionUnlocked,
                ExtractionUnlockedDay = ExtractionUnlockedDay,
                EngineConsumed = EngineConsumed,
                MoralChoicesMade = MoralChoicesMade,
                TerminalReason = TerminalReason,
                DaysSurvived = _lastSummary != null ? _lastSummary.DaysSurvived : 0,
                TotalRadiationAbsorbed = _lastSummary != null ? _lastSummary.TotalRadiationAbsorbed : 0f
            };
        }

        public void RestoreState(VictoryProjectSave save)
        {
            if (save == null)
            {
                Clear();
                return;
            }

            _state = save.State;
            _deathScreen = save.DeathScreen;
            MilitaryIntelDecrypted = Math.Max(0, save.MilitaryIntelDecrypted);
            ExtractionUnlocked = save.ExtractionUnlocked;
            ExtractionUnlockedDay = Math.Max(0, save.ExtractionUnlockedDay);
            EngineConsumed = save.EngineConsumed;
            MoralChoicesMade = Math.Max(0, save.MoralChoicesMade);
            TerminalReason = save.TerminalReason ?? string.Empty;

            if (IsTerminal)
            {
                _lastSummary = new EndgameSummaryData
                {
                    State = _state,
                    DeathScreen = _deathScreen,
                    DaysSurvived = save.DaysSurvived > 0 ? save.DaysSurvived : 1,
                    TotalRadiationAbsorbed = save.TotalRadiationAbsorbed,
                    MoralChoicesMade = MoralChoicesMade,
                    MilitaryIntelDecrypted = MilitaryIntelDecrypted,
                    ExtractionUnlocked = ExtractionUnlocked,
                    VehicleEscapeUsed = EngineConsumed || _state == EndgameState.Escaped,
                    Reason = TerminalReason
                };
                ApplyOutcomeCopy(_lastSummary);
            }
            else
            {
                _lastSummary = null;
            }
        }

        public void Clear()
        {
            _state = EndgameState.Ongoing;
            _deathScreen = DeathScreenKind.None;
            MilitaryIntelDecrypted = 0;
            ExtractionUnlocked = false;
            ExtractionUnlockedDay = 0;
            EngineConsumed = false;
            MoralChoicesMade = 0;
            TerminalReason = string.Empty;
            _lastSummary = null;
        }

        public static bool IsMilitaryIntel(IntelNode intel)
        {
            if (intel == null) return false;
            // Prompt #19 — ghost loops never count toward extraction.
            if (intel.Type == IntelType.GhostLoop) return false;
            if (!string.IsNullOrEmpty(intel.SourceFrequencyId)
                && intel.SourceFrequencyId.StartsWith(GhostStationSystem.IdPrefix, StringComparison.Ordinal))
                return false;
            if (!string.IsNullOrEmpty(intel.SourceFrequencyId)
                && string.Equals(intel.SourceFrequencyId, MilitaryFrequencyId, StringComparison.Ordinal))
                return true;
            // Pre-war military payload types even if source id missing
            return intel.Type == IntelType.TroopMovement || intel.Type == IntelType.MortarWarning;
        }

        // -----------------------------------------------------------------

        private EndgameSummaryData Trigger(
            EndgameState state,
            DeathScreenKind deathScreen,
            string reason,
            int day,
            IReadOnlyList<Survivor> survivors,
            Func<EndgameSummaryData, EndgameSummaryData> finalizeSummary,
            bool vehicleEscape = false)
        {
            if (IsTerminal) return _lastSummary;

            _state = state;
            _deathScreen = deathScreen;
            TerminalReason = reason ?? string.Empty;
            if (vehicleEscape) EngineConsumed = true;

            var summary = BuildSummary(day, survivors, TerminalReason);
            if (finalizeSummary != null)
                summary = finalizeSummary(summary) ?? summary;

            _lastSummary = summary;
            OnStateChanged?.Invoke(_state);
            OnEndgameTriggered?.Invoke(summary);
            return summary;
        }

        private static bool AnyAlive(IReadOnlyList<Survivor> survivors)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive) return true;
            }
            return false;
        }

        private static void ClassifyDeaths(
            IReadOnlyList<Survivor> survivors,
            out EndgameState state,
            out DeathScreenKind screen,
            out string reason)
        {
            float hunger = 0f, rad = 0f, breakdown = 0f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null) continue;

                if (s.Needs != null)
                {
                    if (s.Needs.WasHungerCritical || s.Needs.Hunger >= 90f) hunger += 2f;
                    if (s.Needs.WasThirstCritical || s.Needs.Thirst >= 90f) breakdown += 1f;
                    if (s.Needs.Morale <= 10f) breakdown += 1.5f;
                }

                if (s.LifetimeRadiationExposure >= 80f || s.RadiationDose >= 80f
                    || s.HasAcuteRadiationSickness || s.HasAcuteRadiationSyndrome
                    || s.HasChronicIllness || s.LatentDamage >= 50f)
                    rad += 2.5f;

                if (s.HasMentalBreak || !string.IsNullOrEmpty(s.currentMentalBreakId))
                    breakdown += 2.5f;
            }

            // Pick dominant cause
            if (rad >= hunger && rad >= breakdown && rad > 0f)
            {
                state = EndgameState.Irradiated;
                screen = DeathScreenKind.Radiation;
                reason = "The dose wrote the last line. Nothing left to shield.";
            }
            else if (breakdown > hunger && breakdown > rad)
            {
                state = EndgameState.Starved;
                screen = DeathScreenKind.Breakdowns;
                reason = "The walls held. The people inside did not.";
            }
            else if (hunger > 0f || breakdown > 0f)
            {
                state = EndgameState.Starved;
                screen = hunger >= breakdown ? DeathScreenKind.Hunger : DeathScreenKind.Breakdowns;
                reason = screen == DeathScreenKind.Hunger
                    ? "The stores ran out. The body followed."
                    : "The walls held. The people inside did not.";
            }
            else
            {
                state = EndgameState.Starved;
                screen = DeathScreenKind.Mixed;
                reason = "No one left to write the log.";
            }
        }

        private static void ApplyOutcomeCopy(EndgameSummaryData data)
        {
            if (data == null) return;
            switch (data.State)
            {
                case EndgameState.Rescued:
                    data.OutcomeTitle = "RESCUED";
                    data.OutcomeBody = string.IsNullOrEmpty(data.Reason)
                        ? "Extraction coordinates held. The chopper came."
                        : data.Reason;
                    break;
                case EndgameState.Lifeboat:
                    data.OutcomeTitle = "LIFEBOAT";
                    data.OutcomeBody = string.IsNullOrEmpty(data.Reason)
                        ? "One seat. One name. The rest stayed under concrete."
                        : data.Reason;
                    break;
                case EndgameState.Escaped:
                    data.OutcomeTitle = "ESCAPED";
                    data.OutcomeBody = string.IsNullOrEmpty(data.Reason)
                        ? "The vehicle took them out of the ash."
                        : data.Reason;
                    break;
                case EndgameState.Irradiated:
                    data.OutcomeTitle = "IRRADIATED";
                    data.OutcomeBody = FormatDeathBody(data);
                    break;
                case EndgameState.Starved:
                    data.OutcomeTitle = data.DeathScreen == DeathScreenKind.Breakdowns
                        ? "BROKEN"
                        : "STARVED";
                    data.OutcomeBody = FormatDeathBody(data);
                    break;
                default:
                    data.OutcomeTitle = "ONGOING";
                    data.OutcomeBody = "The campaign continues.";
                    break;
            }
        }

        private static string FormatDeathBody(EndgameSummaryData data)
        {
            if (!string.IsNullOrEmpty(data.Reason)) return data.Reason;
            switch (data.DeathScreen)
            {
                case DeathScreenKind.Radiation:
                    return "Radiation took the last of them.";
                case DeathScreenKind.Hunger:
                    return "Hunger closed the hatch from the inside.";
                case DeathScreenKind.Breakdowns:
                    return "Minds failed before the bunker did.";
                default:
                    return "The shelter went quiet.";
            }
        }
    }

    [Serializable]
    public class VictoryProjectSave
    {
        public EndgameState State;
        public DeathScreenKind DeathScreen;
        public int MilitaryIntelDecrypted;
        public bool ExtractionUnlocked;
        public int ExtractionUnlockedDay;
        public bool EngineConsumed;
        public int MoralChoicesMade;
        public string TerminalReason;
        public int DaysSurvived;
        public float TotalRadiationAbsorbed;
    }
}
