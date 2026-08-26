using System;
using System.Collections.Generic;

namespace Ashfall.Core.Foundry
{
    public sealed partial class SilentFoundrySystem
    {
        // Production
        // -----------------------------------------------------------------

        /// <summary>
        /// Start a heat for the given product. Consumes the full charge
        /// (ingredients + fuel + water) immediately; failure to hold the charge
        /// later wastes it. Validates every prerequisite with a visible reason.
        /// </summary>
        public string StartProduction(string productId, int workers, float workerSkill, int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (_state.laborDispute == FoundryLaborDispute.StrikeActive)
                return "The strike has shut the charging floor; no heat can start.";
            if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
                return "A heat is already in progress (" + HeatStage + ").";

            var product = _catalog.GetProduct(productId);
            if (product == null) return "Unknown product: " + productId;

            workers = MathfCompat.Clamp(workers, 1, MaxWorkers);
            if (workers < 1) return "At least one worker is required.";

            // Charge check (ingredients).
            for (int i = 0; i < product.ingredients.Count; i++)
            {
                var ing = product.ingredients[i];
                if (ing == null || string.IsNullOrEmpty(ing.item_id)) continue;
                int held = _getCount(ing.item_id);
                if (held < ing.amount)
                    return "Missing charge material " + ing.item_id + " (need " + ing.amount + ", have " + held + ").";
            }

            // Fuel check — coal or charcoal.
            int coal = _getCount(SilentFoundryIds.ItemCoal);
            int charcoal = _getCount(SilentFoundryIds.ItemCharcoal);
            if (coal + charcoal < product.fuel_units)
                return "Not enough fuel (" + product.fuel_units + " units of coal/charcoal required).";

            // Water check.
            int water = _getCount(SilentFoundryIds.ItemCleanWater);
            if (water < product.water_litres)
                return "Not enough clean water for the heat (" + product.water_litres + " required, " + water + " held).";

            // Consume charge deterministically: coal first, then charcoal.
            ConsumeFuel(product.fuel_units, coal, charcoal);
            _consume(SilentFoundryIds.ItemCleanWater, product.water_litres);
            for (int i = 0; i < product.ingredients.Count; i++)
            {
                var ing = product.ingredients[i];
                if (ing == null || string.IsNullOrEmpty(ing.item_id)) continue;
                _consume(ing.item_id, ing.amount);
            }

            int materialsConsumed = product.fuel_units + product.water_litres;
            for (int i = 0; i < product.ingredients.Count; i++)
                if (product.ingredients[i] != null)
                    materialsConsumed += product.ingredients[i].amount;

            _state.activeProductId = product.product_id;
            _state.assignedWorkers = workers;
            _state.workerSkill = MathfCompat.Clamp(workerSkill, 0f, 1f);
            _state.laborAccumulated = 0f;
            _state.materialsConsumed = materialsConsumed;
            _state.heatStartedDay = day;
            _state.stageElapsedDays = 0;
            _state.heatStage = FoundryHeatStage.ChargeLoaded;
            // A hot charge with low-grade ingredients sours the sand.
            _state.contamination = Math.Min(100f, _state.contamination + 2f);

            Raise(EventHeatPrepared, product.display_name + " charge loaded (" + materialsConsumed + " units of material)");
            Raise(EventHeatStarted, "heat started day " + day + " · " + workers + " workers");
            RaiseStateChanged();
            return "Heat started: " + product.display_name + " · " + workers + " workers · fuel " + product.fuel_units
                + " · water " + product.water_litres + "L.";
        }

        private void ConsumeFuel(int units, int coalHeld, int charcoalHeld)
        {
            int fromCoal = Math.Min(units, coalHeld);
            if (fromCoal > 0) _consume(SilentFoundryIds.ItemCoal, fromCoal);
            int fromCharcoal = units - fromCoal;
            if (fromCharcoal > 0) _consume(SilentFoundryIds.ItemCharcoal, fromCharcoal);
        }

        /// <summary>
        /// Tap and cast the furnace. This is the risk window: molten iron
        /// breakout through hearth brick / water-slag steam vapor explosion.
        /// Safety warnings surface first; a cast here can be lost or cause an
        /// incident. Deterministic given the seeded RNG.
        /// </summary>
        public string TapAndCast(int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (HeatStage != FoundryHeatStage.AtHeat)
                return "The furnace is not at heat. Stage: " + HeatStage + ".";

            var product = _catalog.GetProduct(_state.activeProductId);
            if (product == null)
            {
                _state.heatStage = FoundryHeatStage.Idle;
                RaiseStateChanged();
                return "No product bound to the current heat; furnace dumped.";
            }

            var warnings = GetSafetyWarnings();
            if (warnings.Count > 0)
            {
                for (int i = 0; i < warnings.Count; i++)
                {
                    Raise(EventSafetyWarning, warnings[i]);
                    _log.Warn("[SilentFoundry] " + warnings[i]);
                }
            }

            // Incident roll — only when the furnace is genuinely unsafe, never hidden.
            int incidentChance = ComputeIncidentChance();
            bool incident = incidentChance > 0 && _rng.Next(0, 100) < incidentChance;
            if (incident)
            {
                return ResolveIncident(product, day);
            }

            _state.heatStage = FoundryHeatStage.Tapped;
            _state.stageElapsedDays = 0;
            RaiseStateChanged();
            return "Tap successful. Molten " + product.display_name + " is in the ladle.";
        }

        /// <summary>Player-facing safety readout before the irreversible tap.</summary>
        public List<string> GetSafetyWarnings()
        {
            var warnings = new List<string>();
            if (_state.hearthTuyeres < 35f)
                warnings.Add("Hearth brick and tuyeres are badly worn (" + _state.hearthTuyeres.ToString("F0")
                    + "/100). Molten iron breakout risk.");
            if (_state.refractoryLining < 25f)
                warnings.Add("Refractory lining is critically spalled. The shell could fail under heat.");
            if (OverdueCycles >= 1)
                warnings.Add("Maintenance is overdue (" + DaysOverdue + " days). Furnace controls drift and fuel cost rises.");
            if (_state.safetyExhaust < 30f)
                warnings.Add("Exhaust/heat management is degraded. Fumes will concentrate on the charging floor.");
            if (_state.sandBeds < 25f)
                warnings.Add("Sand beds are damaged; castings will come out cracked or slagged.");
            return warnings;
        }

        /// <summary>Chance (0..100) of a catastrophic incident at the tap.</summary>
        public int ComputeIncidentChance()
        {
            int chance = 0;
            if (_state.hearthTuyeres < 20f && _state.refractoryLining < 25f) chance += 20;
            else if (_state.hearthTuyeres < 25f) chance += 8;
            if (OverdueCycles >= 2) chance += 12;
            if (OverdueCycles >= 1) chance += 6;
            if (_state.safetyExhaust < 25f) chance += 8;
            if (_state.sandMoisture > 90f && _state.hearthTuyeres < 40f) chance += 6; // steam pocket risk
            return Math.Min(60, chance);
        }

        private string ResolveIncident(FoundryProductEntry product, int day)
        {
            bool severe = _state.hearthTuyeres < 10f || OverdueCycles >= 3;
            var severity = severe ? FoundryIncidentSeverity.Severe : FoundryIncidentSeverity.Contained;
            int downtime = severe ? 7 : 3;
            int injured = severe ? _rng.Next(1, 3) : (_rng.Next(0, 100) < 40 ? 1 : 0);

            // Damage: the furnace takes the hit, not the whole shelter.
            _state.hearthTuyeres = Math.Max(5f, _state.hearthTuyeres - (severe ? 30f : 15f));
            _state.refractoryLining = Math.Max(5f, _state.refractoryLining - (severe ? 35f : 15f));
            _state.safetyExhaust = Math.Max(5f, _state.safetyExhaust - (severe ? 25f : 10f));

            var record = new FoundryIncidentRecord
            {
                severity = severity,
                day = day,
                summary = severe
                    ? "Molten iron broke through the hearth brick; a water-slag steam vapor explosion followed. "
                      + "The floor is shut for " + downtime + " days."
                    : "A splash and steam event on the charging floor. " + downtime + " days of lost heat.",
                workersInjured = injured,
                downtimeDays = downtime
            };
            _state.incidents.Add(record);

            // Worker exposure/fatigue consequence.
            _state.workerExposure += (severe ? 40f : 18f);

            _state.heatStage = FoundryHeatStage.Idle;
            _state.activeProductId = string.Empty;
            _state.assignedWorkers = 0;
            _state.laborAccumulated = 0f;
            _state.materialsConsumed = 0;

            Raise(EventIncident, severity + " incident day " + day + ": " + record.summary);
            OnIncident?.Invoke(record);
            RaiseStateChanged();
            return "INCIDENT: " + record.summary + " (downtime " + downtime + "d, injured " + injured + ").";
        }

        /// <summary>Player labor decisions that shape the strike conflict.</summary>
        public void SetOvertime(bool overtime) { _state.overtimeFlag = overtime; RaiseStateChanged(); }
        public void SetChildLaborUsed(bool used) { _state.childLaborUsed = used; RaiseStateChanged(); }

        /// <summary>
        /// Open a labor dispute. Requires a real conflict: production pressure
        /// (quota missed or an active heat under overtime/child labour) combined
        /// with education or shift grievances. Fatigue alone never triggers it.
        /// </summary>
        public string BeginLaborDispute(int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (_state.laborDispute != FoundryLaborDispute.None) return "A dispute is already open.";

            bool productionPressure = QuotaMissedRecently() || (_state.heatStage != FoundryHeatStage.Idle && _state.overtimeFlag);
            bool shiftGrievance = _state.overtimeFlag || _state.childLaborUsed;
            bool educationConflict = _state.educationConflictFlag || _state.childLaborUsed;

            if (!productionPressure || !(shiftGrievance || educationConflict))
                return "No genuine dispute conditions: production pressure=" + productionPressure
                    + " shiftGrievance=" + shiftGrievance + " educationConflict=" + educationConflict + ".";

            _state.laborDispute = FoundryLaborDispute.Tensions;
            _state.laborDisputeStartedDay = day;
            _state.educationConflictFlag = _state.educationConflictFlag || _state.childLaborUsed;
            Raise(EventLaborDispute, "labor tensions opened day " + day);
            RaiseStateChanged();
            return "Labor tensions opened. The charging floor is restless.";
        }

        /// <summary>Escalate to a full strike after unresolved tensions.</summary>
        public bool EscalateToStrike(int day)
        {
            if (_state.laborDispute != FoundryLaborDispute.Tensions) return false;
            _state.laborDispute = FoundryLaborDispute.StrikeActive;
            _state.strikeStartedDay = day;
            Raise(EventStrikeStarted, "strike active day " + day);
            OnLaborDisputeChanged?.Invoke(_state.laborDispute, day);
            // The strike is the journaled event state (jrnl_foundry_strike).
            MaybeTriggerJournal(SilentFoundryIds.JournalStrike, day);
            RaiseStateChanged();
            return true;
        }

        /// <summary>Resolve the strike with a player decision.</summary>
        public string ResolveStrike(FoundryStrikeResolution resolution, int day)
        {
            if (_state.laborDispute != FoundryLaborDispute.StrikeActive)
                return "No active strike to resolve.";

            switch (resolution)
            {
                case FoundryStrikeResolution.ConcedeShiftLimits:
                    _state.overtimeFlag = false;
                    _state.childLaborUsed = false;
                    _state.educationConflictFlag = true;
                    break;
                case FoundryStrikeResolution.UpholdQuota:
                    _state.overtimeFlag = true;
                    break;
                case FoundryStrikeResolution.Mediation:
                    _state.overtimeFlag = false;
                    _state.childLaborUsed = false;
                    break;
            }
            _state.laborDispute = FoundryLaborDispute.Resolved;
            Raise(EventStrikeResolved, resolution + " day " + day);
            OnStrikeResolved?.Invoke(resolution, day);
            RaiseStateChanged();
            return "Strike resolved via " + resolution + ".";
        }

        private bool QuotaMissedRecently()
        {
            for (int i = 0; i < _state.treatyCompliance.Count; i++)
            {
                var c = _state.treatyCompliance[i];
                if (c != null && c.missedCount > 0 && !c.currentCycleMet) return true;
            }
            return false;
        }

        // -----------------------------------------------------------------
        // Daily simulation
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance one simulation day. Drives maintenance accounting, heat
        /// stage progression, labour escalation and treaty assessment.
        /// Deterministic; uses no wall-clock time.
        /// </summary>
        public void TickDaily(int day)
        {
            if (!_state.unlocked) return;

            // Maintenance accounting.
            if (_state.maintenanceDueDay > 0 && day > _state.maintenanceDueDay)
            {
                _state.daysSinceMaintenance++;
                if (_state.daysSinceMaintenance == _state.maintenanceCycleDays + 1)
                {
                    Raise(EventMaintenanceDue, "maintenance overdue since day " + _state.maintenanceDueDay);
                    OnSafetyWarning?.Invoke("Maintenance is overdue. Fuel cost and cast risk climb every day.");
                }
            }
            else if (_state.maintenanceDueDay == 0)
            {
                // Not yet commissioned — count from unlock.
                if (_state.unlockDay > 0)
                {
                    _state.daysSinceMaintenance = Math.Max(0, day - _state.unlockDay);
                    if (_state.daysSinceMaintenance == _state.maintenanceCycleDays + 1)
                        Raise(EventMaintenanceDue, "first maintenance window passed day " + day);
                }
            }

            // Gradual wear while the facility is in use (any heat or repair activity).
            float wear = HeatStage == FoundryHeatStage.Idle ? 0.15f : 0.9f;
            _state.refractoryLining = Math.Max(0f, _state.refractoryLining - wear * 0.5f);
            _state.hearthTuyeres = Math.Max(0f, _state.hearthTuyeres - wear * 0.7f);
            _state.safetyExhaust = Math.Max(0f, _state.safetyExhaust - wear * 0.4f);

            // Heat stage machine.
            AdvanceHeatStage(day);

            // Labor escalation: unresolved tensions escalate after one day.
            if (_state.laborDispute == FoundryLaborDispute.Tensions
                && day - _state.laborDisputeStartedDay >= 1)
            {
                EscalateToStrike(day);
            }

            // Treaty assessment at ratification/deadline days.
            AssessTreatyCompliance(day);

            RaiseStateChanged();
        }

        private void AdvanceHeatStage(int day)
        {
            if (HeatStage == FoundryHeatStage.Idle || HeatStage == FoundryHeatStage.Complete) return;

            _state.stageElapsedDays++;

            switch (HeatStage)
            {
                case FoundryHeatStage.ChargeLoaded:
                    // Wait a full day with the charge in the cupola.
                    if (_state.stageElapsedDays >= 1) SetStage(FoundryHeatStage.Preheat, day);
                    break;

                case FoundryHeatStage.Preheat:
                    // Overdue maintenance lengthens preheat (more fuel, slower climb).
                    int preheatDays = 1 + Math.Min(2, OverdueCycles);
                    if (_state.stageElapsedDays >= preheatDays) SetStage(FoundryHeatStage.AtHeat, day);
                    break;

                case FoundryHeatStage.AtHeat:
                    // The furnace holds. The player must tap; an un-tapped heat
                    // burns out after 3 days and wastes the charge (visible cost).
                    if (_state.stageElapsedDays >= 3)
                    {
                        _state.heatStage = FoundryHeatStage.Idle;
                        var product = _catalog.GetProduct(_state.activeProductId);
                        _state.failed.Add(new FoundryFailedCastRecord
                        {
                            productId = _state.activeProductId,
                            displayName = product?.display_name ?? _state.activeProductId,
                            reason = "Heat burned out untapped (furnace held too long).",
                            failedDay = day,
                            materialsLost = _state.materialsConsumed
                        });
                        _state.activeProductId = string.Empty;
                        _state.assignedWorkers = 0;
                        _state.laborAccumulated = 0f;
                        Raise(EventCastFailed, "heat burned out untapped day " + day);
                        OnCastFailed?.Invoke(_state.failed[_state.failed.Count - 1]);
                    }
                    break;

                case FoundryHeatStage.Tapped:
                    SetStage(FoundryHeatStage.Casting, day);
                    break;

                case FoundryHeatStage.Casting:
                    {
                        var product = _catalog.GetProduct(_state.activeProductId);
                        int castingDays = Math.Max(1, (int)Math.Ceiling((product?.cast_hours ?? 4f) / 24f));
                        if (_state.stageElapsedDays >= castingDays)
                        {
                            SetStage(FoundryHeatStage.Cooling, day);
                        }
                        else
                        {
                            // Labour accrues per day across assigned workers.
                            float labourPerDay = _state.assignedWorkers * 8f * (0.75f + 0.25f * _state.workerSkill);
                            _state.laborAccumulated += labourPerDay;
                            // Heat and fumes exact a cost on the crew.
                            _state.workerExposure += 4f;
                        }
                    }
                    break;

                case FoundryHeatStage.Cooling:
                    if (_state.stageElapsedDays >= 1) CompleteCast(day);
                    break;
            }
        }

        private void SetStage(FoundryHeatStage stage, int day)
        {
            _state.heatStage = stage;
            _state.stageElapsedDays = 0;
        }

        private void CompleteCast(int day)
        {
            var product = _catalog.GetProduct(_state.activeProductId);
            if (product == null)
            {
                _state.heatStage = FoundryHeatStage.Idle;
                _state.activeProductId = string.Empty;
                return;
            }

            float quality = RollQuality(product);
            _state.pendingQuality = quality;
            var tier = QualityTier(quality);

            if (tier == FoundryQualityTier.Scrap || quality <= 0f)
            {
                _state.failed.Add(new FoundryFailedCastRecord
                {
                    productId = product.product_id,
                    displayName = product.display_name,
                    reason = "Cast cracked or slagged (quality " + quality.ToString("F0") + ").",
                    failedDay = day,
                    materialsLost = _state.materialsConsumed
                });
                _state.heatStage = FoundryHeatStage.Idle;
                _state.activeProductId = string.Empty;
                _state.assignedWorkers = 0;
                _state.laborAccumulated = 0f;
                Raise(EventCastFailed, product.display_name + " cast failed day " + day + " (quality " + quality.ToString("F0") + ")");
                OnCastFailed?.Invoke(_state.failed[_state.failed.Count - 1]);
                RaiseStateChanged();
                return;
            }

            // Output lands in inventory when the host wired an inventory.
            if (_canAdd(product.result_item_id, product.result_amount))
            {
                _addItem(product.result_item_id, product.result_amount);
            }

            var record = new FoundryProductionRecord
            {
                productId = product.product_id,
                displayName = product.display_name,
                amount = product.result_amount,
                tier = tier,
                completedDay = day,
                workers = _state.assignedWorkers
            };
            _state.completed.Add(record);

            // Quota fulfilment.
            ApplyQuotaFulfilment(product, record.amount);

            _state.heatStage = FoundryHeatStage.Complete;
            _state.activeProductId = string.Empty;
            _state.assignedWorkers = 0;
            _state.laborAccumulated = 0f;

            Raise(EventCastCompleted, product.display_name + " ×" + record.amount + " (" + tier + ", quality " + quality.ToString("F0") + ") day " + day);
            OnProductionCompleted?.Invoke(record);

            // First successful heat → jrnl_foundry_first_heat (once).
            MaybeTriggerJournal(SilentFoundryIds.JournalFirstHeat, day);

            Raise(EventHeatCompleted, product.display_name + " cast completed day " + day);
            RaiseStateChanged();
        }

        private float RollQuality(FoundryProductEntry product)
        {
            float q = product.quality_target;
            q += ((_state.sandQuality - 60f) / 10f) * 5f;      // sand quality ±5
            q -= Math.Abs(_state.sandMoisture - 65f) * 0.35f;  // moisture deviation (target 65)
            q += ((_state.binderQuality - 50f) / 10f) * 4f;    // binder ±4
            q -= _state.contamination / 10f;                   // contamination 0..-10
            q += (_state.patternQuality / 100f) * 4f;          // pattern +0..4
            q += ((_state.compaction - 60f) / 10f) * 3f;       // compaction ±3
            q += ((_state.hearthTuyeres - 60f) / 10f) * 3f;    // furnace condition ±3
            q += ((_state.refractoryLining - 60f) / 10f) * 3f; // lining condition ±3
            q -= Math.Min(15f, DaysOverdue * 2.5f);            // maintenance neglect
            q += (_state.workerSkill - product.skill_target) * 12f; // skill ±6
            q += _rng.Next(-5, 6);                             // seeded jitter only

            // Mold reuse degrades the bed.
            _state.moldReuseCount++;
            _state.sandQuality = Math.Max(5f, _state.sandQuality - 2.5f);
            _state.binderQuality = Math.Max(5f, _state.binderQuality - 2f);
            _state.contamination = Math.Min(100f, _state.contamination + 3f);

            return MathfCompat.Clamp(q, 0f, 100f);
        }

        public static FoundryQualityTier QualityTier(float quality)
        {
            if (quality >= 90f) return FoundryQualityTier.Fine;
            if (quality >= 75f) return FoundryQualityTier.Good;
            if (quality >= 55f) return FoundryQualityTier.Usable;
            return FoundryQualityTier.Scrap;
        }

        // -----------------------------------------------------------------
    }
}
