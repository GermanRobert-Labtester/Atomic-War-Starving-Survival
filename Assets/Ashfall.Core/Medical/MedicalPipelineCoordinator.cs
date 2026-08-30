// SPDX-License-Identifier: MIT
// Task #133 — The unified medical pipeline coordinator.
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.PlayerCommand;

namespace Ashfall.Core.Medical
{
    /// <summary>Patient availability reported by the host (canonical survivor lifecycle).</summary>
    public sealed class PatientAvailability
    {
        public bool Available;
        /// <summary>Stable snake_case reason when unavailable: "patient_unknown", "patient_dead", "patient_away", "patient_memorialized".</summary>
        public string ReasonCode = "patient_unknown";

        public static PatientAvailability Ok() => new PatientAvailability { Available = true, ReasonCode = "ok" };
        public static PatientAvailability Blocked(string reason) => new PatientAvailability { Available = false, ReasonCode = reason };
    }

    /// <summary>Result of one committed or blocked treatment operation.</summary>
    public sealed class MedicalOperationResult
    {
        public bool Success;
        public string ReasonCode = string.Empty;
        public long StateVersion;
        public int ProcedureId = -1;
        public DiagnosisStatus DiagnosisAfter = DiagnosisStatus.Unknown;

        public static MedicalOperationResult Fail(string reason) =>
            new MedicalOperationResult { Success = false, ReasonCode = reason };
    }

    /// <summary>
    /// Coordinates the unified affliction → diagnosis → treatment pipeline
    /// (Task #133). The coordinator owns <b>coordination only</b>: identity,
    /// knowledge, reservations, scheduling, and transaction validation. Every
    /// clinical rule stays in its domain handler; every item quantity stays in
    /// the authoritative inventory; every hour stays on the campaign clock.
    ///
    /// <para>Transaction discipline: validate everything → reserve → consume →
    /// mutate domain → release (as consumed) → publish events. Any failed step
    /// releases reservations and mutates nothing.</para>
    /// </summary>
    public sealed class MedicalPipelineCoordinator
    {
        private readonly IPlayerInventoryPort _inventory;
        private readonly DiagnosisKnowledgeStore _diagnosis;
        private readonly MedicalReservationLedger _reservations;
        private readonly MedicalProcedureSchedule _schedule;
        private readonly Dictionary<string, IAfflictionHandler> _handlers =
            new Dictionary<string, IAfflictionHandler>(StringComparer.Ordinal);
        private readonly Dictionary<string, IMedicalProtocolHandler> _protocols =
            new Dictionary<string, IMedicalProtocolHandler>(StringComparer.Ordinal);
        private readonly Func<Survivors.SurvivorId, PatientAvailability> _availability;
        private readonly Func<int> _currentDay;

        /// <summary>Monotonic version for stale-preview rejection; persists with the pipeline.</summary>
        public long StateVersion { get; private set; }

        public DiagnosisKnowledgeStore Diagnosis => _diagnosis;
        public MedicalReservationLedger Reservations => _reservations;
        public MedicalProcedureSchedule Schedule => _schedule;

        /// <summary>Raised after any committed pipeline mutation (save/UI refresh hook).</summary>
        public event Action? StateChanged;

        /// <summary>Raised only after a committed state change (never before).</summary>
        public event Action<string, Survivors.SurvivorId>? OnDiagnosisConfirmed;
        public event Action<string, Survivors.SurvivorId>? OnDiagnosisSuspected;
        public event Action<string, Survivors.SurvivorId>? OnPatientStabilized;
        public event Action<string, Survivors.SurvivorId>? OnPatientRecovered;
        public event Action<string, Survivors.SurvivorId>? OnTreatmentScheduled;
        public event Action<string, Survivors.SurvivorId>? OnTreatmentCompleted;
        public event Action<string, Survivors.SurvivorId, string>? OnTreatmentRefused;

        /// <summary>Raised after a camp-wide protocol commits (Task #133 P1).</summary>
        public event Action<string>? OnProtocolExecuted;

        public MedicalPipelineCoordinator(
            IPlayerInventoryPort inventory,
            DiagnosisKnowledgeStore diagnosis,
            MedicalReservationLedger reservations,
            MedicalProcedureSchedule schedule,
            Func<Survivors.SurvivorId, PatientAvailability> availability,
            Func<int> currentDay)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _diagnosis = diagnosis ?? throw new ArgumentNullException(nameof(diagnosis));
            _reservations = reservations ?? throw new ArgumentNullException(nameof(reservations));
            _schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
            _availability = availability ?? throw new ArgumentNullException(nameof(availability));
            _currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        }

        // ── Handler registry ─────────────────────────────────────────

        public void RegisterHandler(IAfflictionHandler handler)
        {
            if (handler == null || handler.DefinitionId.IsEmpty) return;
            _handlers[handler.DefinitionId.Value] = handler;
        }

        public IAfflictionHandler? GetHandler(AfflictionId definition)
        {
            return _handlers.TryGetValue(definition.Value, out var h) ? h : null;
        }

        public IReadOnlyCollection<IAfflictionHandler> Handlers => _handlers.Values;

        // ── Protocol registry (camp-wide; no patient) ────────────────

        /// <summary>Register a camp-wide protocol handler (Task #133 P1).</summary>
        public void RegisterProtocol(IMedicalProtocolHandler protocol)
        {
            if (protocol == null || string.IsNullOrEmpty(protocol.ProtocolId)) return;
            _protocols[protocol.ProtocolId] = protocol;
        }

        public IMedicalProtocolHandler? GetProtocol(string protocolId)
        {
            return _protocols.TryGetValue(protocolId, out var p) ? p : null;
        }

        public IReadOnlyCollection<IMedicalProtocolHandler> Protocols => _protocols.Values;

        // ── Diagnosis commands ───────────────────────────────────────

        /// <summary>
        /// Side-effect-free diagnose preview. Diagnosing requires: known patient,
        /// plausible condition (<see cref="IAfflictionHandler.CouldHaveCondition"/>),
        /// not already confirmed. Diagnosis itself consumes nothing.
        /// </summary>
        public CommandPreview PreviewDiagnose(Survivors.SurvivorId survivor, AfflictionId definition, long expectedVersion = 0)
        {
            if (expectedVersion != 0 && expectedVersion != StateVersion)
                return Stale(expectedVersion);
            var fail = ValidatePatient(survivor, "treatment.diagnose");
            if (fail != null) return Unavailable("treatment.diagnose", fail, expectedVersion);
            if (!_handlers.TryGetValue(definition.Value, out var handler))
                return Unavailable("treatment.diagnose", "unknown_affliction", expectedVersion);
            if (!handler.CouldHaveCondition(survivor))
                return Unavailable("treatment.diagnose", "no_plausible_condition", expectedVersion);

            var episode = AfflictionEpisodeId.Create(survivor, definition);
            if (_diagnosis.GetStatus(episode) == DiagnosisStatus.Confirmed)
                return Unavailable("treatment.diagnose", "already_confirmed", expectedVersion);

            return CommandPreview.Available("treatment.diagnose", StateVersion);
        }

        /// <summary>Execute a diagnose: mutates only diagnosis knowledge. Atomic.</summary>
        public MedicalOperationResult ExecuteDiagnose(Survivors.SurvivorId survivor, AfflictionId definition, long expectedVersion = 0)
        {
            var preview = PreviewDiagnose(survivor, definition, expectedVersion);
            if (!preview.IsAvailable)
                return new MedicalOperationResult { Success = false, ReasonCode = preview.FailureCode, StateVersion = StateVersion };

            var episode = AfflictionEpisodeId.Create(survivor, definition);
            int day = _currentDay();
            bool changed = _diagnosis.Confirm(episode, day, "diagnose_command");
            if (changed)
            {
                StateVersion++;
                StateChanged?.Invoke();
            }
            OnDiagnosisConfirmed?.Invoke(definition.Value, survivor);
            return new MedicalOperationResult
            {
                Success = true,
                ReasonCode = "ok",
                StateVersion = StateVersion,
                DiagnosisAfter = DiagnosisStatus.Confirmed
            };
        }

        // ── Identify (clinical examination, Task #133 P1) ─────────────

        /// <summary>
        /// Side-effect-free preview of a clinical examination: available when
        /// the patient has at least one Suspected episode. The examination is
        /// untargeted — the player never names a disease, so hidden identities
        /// cannot be probed through this command.
        /// </summary>
        public CommandPreview PreviewIdentify(Survivors.SurvivorId survivor, long expectedVersion = 0)
        {
            if (expectedVersion != 0 && expectedVersion != StateVersion)
                return Stale(expectedVersion);
            var fail = ValidatePatient(survivor, PlayerCommandCode.TreatmentDiagnose);
            if (fail != null) return Unavailable(PlayerCommandCode.TreatmentDiagnose, fail, expectedVersion);
            if (!HasSuspectedEpisode(survivor))
                return Unavailable(PlayerCommandCode.TreatmentDiagnose, "no_suspected_condition", expectedVersion);
            return CommandPreview.Available(PlayerCommandCode.TreatmentDiagnose, StateVersion);
        }

        /// <summary>
        /// Execute a clinical examination: confirms every Suspected episode the
        /// patient currently has (across all registered handlers, ordinal
        /// definition order). Mutates only diagnosis knowledge. Atomic.
        /// </summary>
        public MedicalOperationResult ExecuteIdentify(Survivors.SurvivorId survivor, long expectedVersion = 0)
        {
            var preview = PreviewIdentify(survivor, expectedVersion);
            if (!preview.IsAvailable)
                return new MedicalOperationResult { Success = false, ReasonCode = preview.FailureCode, StateVersion = StateVersion };

            int day = _currentDay();
            int confirmed = 0;
            foreach (var definitionId in SortedHandlerIds())
            {
                var handler = _handlers[definitionId];
                var episode = handler.GetEpisode(survivor);
                if (episode == null) continue;
                if (_diagnosis.GetStatus(episode.EpisodeId) != DiagnosisStatus.Suspected) continue;
                if (_diagnosis.Confirm(episode.EpisodeId, day, "identify_command"))
                {
                    confirmed++;
                    OnDiagnosisConfirmed?.Invoke(definitionId, survivor);
                }
            }

            if (confirmed == 0)
                return new MedicalOperationResult { Success = false, ReasonCode = "no_suspected_condition", StateVersion = StateVersion };

            StateVersion++;
            StateChanged?.Invoke();
            return new MedicalOperationResult { Success = true, ReasonCode = "ok", StateVersion = StateVersion };
        }

        private bool HasSuspectedEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return false;
            foreach (var definitionId in SortedHandlerIds())
            {
                var episode = _handlers[definitionId].GetEpisode(survivor);
                if (episode == null) continue;
                if (_diagnosis.GetStatus(episode.EpisodeId) == DiagnosisStatus.Suspected) return true;
            }
            return false;
        }

        private List<string> SortedHandlerIds()
        {
            var ids = new List<string>(_handlers.Keys);
            ids.Sort(StringComparer.Ordinal);
            return ids;
        }

        /// <summary>Auto-suspect from domain evidence (e.g. threshold crossing). Never confirms. Idempotent.</summary>
        public void SuspectFromEvidence(Survivors.SurvivorId survivor, AfflictionId definition, int day, string evidenceCode)
        {
            var episode = AfflictionEpisodeId.Create(survivor, definition);
            var status = _diagnosis.GetStatus(episode);
            if (status != DiagnosisStatus.Unknown) return;
            _diagnosis.SetStatus(episode, DiagnosisStatus.Suspected, day, evidenceCode);
            StateVersion++;
            StateChanged?.Invoke();
            OnDiagnosisSuspected?.Invoke(definition.Value, survivor);
        }

        /// <summary>
        /// Legacy-save migration: the pre-pipeline game displayed these conditions
        /// openly, so restored episodes arrive Confirmed. Idempotent.
        /// </summary>
        public void ConfirmForLegacySave(Survivors.SurvivorId survivor, AfflictionId definition, int day)
        {
            var episode = AfflictionEpisodeId.Create(survivor, definition);
            if (_diagnosis.Confirm(episode, day, "legacy_save_migration"))
            {
                StateVersion++;
                StateChanged?.Invoke();
            }
        }

        // ── Treatment commands ───────────────────────────────────────

        /// <summary>
        /// Side-effect-free treatment preview: patient → handler contraindication
        /// → diagnosis → exclusivity → inventory availability (count − claims).
        /// Never mutates any store. Treatments whose catalog definition carries
        /// no fixed affliction (e.g. quarantine) require an explicit
        /// <paramref name="target"/> handler. <paramref name="targetItem"/>
        /// selects a sub-case inside one affliction definition (e.g. the
        /// substance for a chemical-dependency detox start); handlers that do
        /// not need it ignore it.
        /// </summary>
        public CommandPreview PreviewTreatment(Survivors.SurvivorId survivor, string treatmentId, long expectedVersion = 0, AfflictionId? target = null, string? targetItem = null)
        {
            if (expectedVersion != 0 && expectedVersion != StateVersion)
                return Stale(expectedVersion);
            var def = MedicalTreatmentCatalog.Get(treatmentId);
            if (def == null)
                return Unavailable("treatment.start", "unknown_treatment", expectedVersion);

            var fail = ValidatePatient(survivor, "treatment.start");
            if (fail != null) return Unavailable("treatment.start", fail, expectedVersion);

            string? afflictionId = ResolveTargetAffliction(def, target);
            if (afflictionId == null)
                return Unavailable("treatment.start", "target_affliction_required", expectedVersion);
            if (!_handlers.TryGetValue(afflictionId, out var handler))
                return Unavailable("treatment.start", "unknown_affliction", expectedVersion);

            string? contra = handler.ValidateTreatment(survivor, treatmentId, targetItem);
            if (contra != null) return Unavailable("treatment.start", contra, expectedVersion);

            var episode = AfflictionEpisodeId.Create(survivor, new AfflictionId(afflictionId));
            if (def.RequiresConfirmedDiagnosis && _diagnosis.GetStatus(episode) != DiagnosisStatus.Confirmed)
                return Unavailable("treatment.start", "diagnosis_unconfirmed", expectedVersion);

            if (def.ExclusivePerPatient && _schedule.HasActiveProcedure(survivor, treatmentId))
                return Unavailable("treatment.start", "already_in_treatment", expectedVersion);

            foreach (var kv in def.ItemCosts)
            {
                int available = _inventory.CountById(kv.Key) - _reservations.ReservedQuantity(kv.Key);
                if (available < kv.Value)
                    return Unavailable("treatment.start", "missing_medicine", expectedVersion);
            }

            var deltas = new Dictionary<string, double>();
            foreach (var kv in def.ItemCosts) deltas[kv.Key] = -kv.Value;
            return CommandPreview.Available("treatment.start", StateVersion, deltas,
                estimatedDurationHours: def.IsScheduled ? def.DurationHours : (float?)null);
        }

        /// <summary>
        /// Execute an immediate treatment atomically: reserve → consume → apply →
        /// release-as-consumed. On any failure nothing is partially consumed.
        /// </summary>
        public MedicalOperationResult ExecuteTreatment(Survivors.SurvivorId survivor, string treatmentId, long expectedVersion = 0, AfflictionId? target = null, string? targetItem = null)
        {
            var preview = PreviewTreatment(survivor, treatmentId, expectedVersion, target, targetItem);
            if (!preview.IsAvailable)
            {
                OnTreatmentRefused?.Invoke(treatmentId, survivor, preview.FailureCode);
                return new MedicalOperationResult { Success = false, ReasonCode = preview.FailureCode, StateVersion = StateVersion };
            }

            var def = MedicalTreatmentCatalog.Get(treatmentId)!;
            var handler = _handlers[ResolveTargetAffliction(def, target)!];

            if (def.IsScheduled)
                return ExecuteScheduled(survivor, def, handler);

            // 1. Reserve medicine claims.
            var reservationIds = new List<int>();
            foreach (var kv in def.ItemCosts)
            {
                int id = _reservations.Reserve(survivor, MedicalReservationKind.Medicine, kv.Key, kv.Value, treatmentId);
                if (id < 0)
                {
                    RollbackReservations(reservationIds);
                    return new MedicalOperationResult { Success = false, ReasonCode = "reservation_failed", StateVersion = StateVersion };
                }
                reservationIds.Add(id);
            }

            // 2. Consume through the authoritative inventory (atomic bill).
            var bill = new Dictionary<string, int>(def.ItemCosts);
            if (!_inventory.TryConsumeBill(bill))
            {
                RollbackReservations(reservationIds);
                OnTreatmentRefused?.Invoke(treatmentId, survivor, "missing_medicine");
                return new MedicalOperationResult { Success = false, ReasonCode = "missing_medicine", StateVersion = StateVersion };
            }

            // 3. Apply through the domain handler (domain owns the clinical rule).
            if (!handler.ApplyTreatment(survivor, treatmentId, targetItem))
            {
                RollbackReservations(reservationIds);
                OnTreatmentRefused?.Invoke(treatmentId, survivor, "treatment_rejected");
                return new MedicalOperationResult { Success = false, ReasonCode = "treatment_rejected", StateVersion = StateVersion };
            }

            // 4. Release claims as consumed.
            foreach (var id in reservationIds) _reservations.Release(id);

            StateVersion++;
            StateChanged?.Invoke();
            OnPatientStabilized?.Invoke(treatmentId, survivor);
            if (handler.HasResolved(survivor))
                OnPatientRecovered?.Invoke(ResolveTargetAffliction(def, target)!, survivor);

            return new MedicalOperationResult
            {
                Success = true,
                ReasonCode = "ok",
                StateVersion = StateVersion
            };
        }

        /// <summary>
        /// Execute a scheduled treatment: validate → reserve → create the
        /// procedure row. Costs are consumed at completion by
        /// <see cref="AdvanceScheduled"/>; reservations hold the claim meanwhile.
        /// </summary>
        private MedicalOperationResult ExecuteScheduled(Survivors.SurvivorId survivor, MedicalTreatmentDef def, IAfflictionHandler handler)
        {
            int day = _currentDay();
            var reservationIds = new List<int>();
            foreach (var kv in def.ItemCosts)
            {
                int id = _reservations.Reserve(survivor, MedicalReservationKind.Medicine, kv.Key, kv.Value, def.TreatmentId);
                if (id < 0)
                {
                    RollbackReservations(reservationIds);
                    return new MedicalOperationResult { Success = false, ReasonCode = "reservation_failed", StateVersion = StateVersion };
                }
                reservationIds.Add(id);
            }

            var episode = AfflictionEpisodeId.Create(survivor, new AfflictionId(def.AfflictionId));
            int procedureId = _schedule.Schedule(survivor, def.TreatmentId, episode, day, def.DurationHours, reservationIds);
            if (procedureId < 0)
            {
                RollbackReservations(reservationIds);
                return new MedicalOperationResult { Success = false, ReasonCode = "schedule_failed", StateVersion = StateVersion };
            }

            StateVersion++;
            StateChanged?.Invoke();
            OnTreatmentScheduled?.Invoke(def.TreatmentId, survivor);
            return new MedicalOperationResult
            {
                Success = true,
                ReasonCode = "ok",
                StateVersion = StateVersion,
                ProcedureId = procedureId
            };
        }

        // ── Camp-wide protocols (Task #133 P1) ───────────────────────

        /// <summary>
        /// Side-effect-free protocol preview: known protocol, not already
        /// applied, and supplies available (inventory − claims). Never mutates.
        /// </summary>
        public CommandPreview PreviewProtocol(string protocolId, long expectedVersion = 0)
        {
            if (expectedVersion != 0 && expectedVersion != StateVersion)
                return Stale(expectedVersion);
            if (!_protocols.TryGetValue(protocolId, out var protocol))
                return Unavailable(PlayerCommandCode.TreatmentProtocol, "unknown_protocol", expectedVersion);

            string? blocked = protocol.Validate();
            if (blocked != null)
                return Unavailable(PlayerCommandCode.TreatmentProtocol, blocked, expectedVersion);

            foreach (var kv in protocol.ItemCosts)
            {
                int available = _inventory.CountById(kv.Key) - _reservations.ReservedQuantity(kv.Key);
                if (available < kv.Value)
                    return Unavailable(PlayerCommandCode.TreatmentProtocol, "missing_medicine", expectedVersion);
            }

            var deltas = new Dictionary<string, double>();
            foreach (var kv in protocol.ItemCosts) deltas[kv.Key] = -kv.Value;
            return CommandPreview.Available(PlayerCommandCode.TreatmentProtocol, StateVersion, deltas);
        }

        /// <summary>
        /// Execute a camp-wide protocol atomically: validate → consume through
        /// the authoritative inventory → apply to domain state. Protocols are
        /// synchronous and attribute to no patient, so no reservation rows are
        /// created; a failed consume mutates nothing.
        /// </summary>
        public MedicalOperationResult ExecuteProtocol(string protocolId, long expectedVersion = 0)
        {
            var preview = PreviewProtocol(protocolId, expectedVersion);
            if (!preview.IsAvailable)
                return new MedicalOperationResult { Success = false, ReasonCode = preview.FailureCode, StateVersion = StateVersion };

            var protocol = _protocols[protocolId];
            var bill = new Dictionary<string, int>(protocol.ItemCosts);
            if (bill.Count > 0 && !_inventory.TryConsumeBill(bill))
            {
                return new MedicalOperationResult { Success = false, ReasonCode = "missing_medicine", StateVersion = StateVersion };
            }

            if (!protocol.Apply())
            {
                // Validate-first contract violation: unreachable for well-formed
                // handlers (validate and apply run in the same tick). Surfaced
                // as a failure rather than a silent success.
                return new MedicalOperationResult { Success = false, ReasonCode = "protocol_rejected", StateVersion = StateVersion };
            }

            StateVersion++;
            StateChanged?.Invoke();
            OnProtocolExecuted?.Invoke(protocolId);
            return new MedicalOperationResult { Success = true, ReasonCode = "ok", StateVersion = StateVersion };
        }

        // ── Cancellation ─────────────────────────────────────────────

        /// <summary>
        /// Cancel a scheduled procedure. Reserved-but-unconsumed medicine is
        /// released (refunded); nothing already consumed is returned.
        /// </summary>
        public MedicalOperationResult ExecuteCancel(int procedureId, long expectedVersion = 0)
        {
            if (expectedVersion != 0 && expectedVersion != StateVersion)
                return new MedicalOperationResult { Success = false, ReasonCode = "stale_preview", StateVersion = StateVersion };
            if (!_schedule.TryGetActive(procedureId, out var row))
                return new MedicalOperationResult { Success = false, ReasonCode = "unknown_procedure", StateVersion = StateVersion };

            int day = _currentDay();
            _schedule.Cancel(procedureId, day);
            foreach (var resId in row.reservationIds) _reservations.Release(resId);
            StateVersion++;
            StateChanged?.Invoke();
            return new MedicalOperationResult { Success = true, ReasonCode = "ok", StateVersion = StateVersion };
        }

        // ── Clock-driven advancement (day owner only) ─────────────────

        /// <summary>
        /// Advance scheduled procedures by <paramref name="hours"/> game-hours.
        /// Called ONLY by the campaign day owner. Completes procedures whose
        /// remaining hours reach zero: consumes reserved costs, applies the
        /// domain treatment, releases the bed. Failures (patient gone) cancel
        /// deterministically with full release.
        /// </summary>
        public IReadOnlyList<MedicalProcedureCompletion> AdvanceScheduled(float hours, int currentDay)
        {
            var completions = _schedule.Advance(hours, currentDay);
            if (completions.Count == 0) return completions;

            foreach (var completion in completions)
            {
                var def = MedicalTreatmentCatalog.Get(completion.TreatmentId);
                bool survivorOk = Survivors.SurvivorId.TryParse(completion.SurvivorId, out var survivor);
                if (def == null || !survivorOk)
                {
                    FailProcedure(completion, currentDay);
                    continue;
                }

                var availability = _availability(survivor);
                if (!availability.Available)
                {
                    // Patient died or left mid-procedure: cancel, release claims, no consumption.
                    ReleaseProcedureReservations(completion.ProcedureId);
                    OnTreatmentRefused?.Invoke(completion.TreatmentId, survivor, "patient_unavailable");
                    continue;
                }

                var handler = _handlers.TryGetValue(def.AfflictionId, out var h) ? h : null;
                if (handler == null)
                {
                    FailProcedure(completion, currentDay);
                    continue;
                }

                // Consume the reserved medicine now.
                var bill = new Dictionary<string, int>(def.ItemCosts);
                bool consumed = bill.Count == 0 || _inventory.TryConsumeBill(bill);
                ReleaseProcedureReservations(completion.ProcedureId);
                if (!consumed || !handler.ApplyTreatment(survivor, completion.TreatmentId))
                {
                    OnTreatmentRefused?.Invoke(completion.TreatmentId, survivor, consumed ? "treatment_rejected" : "missing_medicine");
                    continue;
                }

                StateVersion++;
            StateChanged?.Invoke();
                OnTreatmentCompleted?.Invoke(completion.TreatmentId, survivor);
                OnPatientStabilized?.Invoke(completion.TreatmentId, survivor);
                if (handler.HasResolved(survivor))
                    OnPatientRecovered?.Invoke(def.AfflictionId, survivor);
            }
            return completions;
        }

        private void FailProcedure(MedicalProcedureCompletion completion, int currentDay)
        {
            ReleaseProcedureReservations(completion.ProcedureId);
            _schedule.Cancel(completion.ProcedureId, currentDay, failed: true);
        }

        private void ReleaseProcedureReservations(int procedureId)
        {
            if (!_schedule.TryGetActive(procedureId, out var row))
            {
                // The row may already be in history (completed). Search history via capture.
                foreach (var r in Schedule.History)
                {
                    if (r.procedureId == procedureId)
                    {
                        foreach (var resId in r.reservationIds) _reservations.Release(resId);
                        return;
                    }
                }
                return;
            }
            foreach (var resId in row.reservationIds) _reservations.Release(resId);
        }

        /// <summary>Release every claim and cancel every procedure for a survivor (death reconciliation).</summary>
        public void ReconcilePatientDeath(Survivors.SurvivorId survivor, int day)
        {
            var cancelled = _schedule.CancelAllForSurvivor(survivor, day);
            foreach (var pid in cancelled)
            {
                foreach (var r in Schedule.History)
                {
                    if (r.procedureId == pid)
                        foreach (var resId in r.reservationIds) _reservations.Release(resId);
                }
            }
            _reservations.ReleaseAllForSurvivor(survivor);
            if (cancelled.Count > 0)
            {
                StateVersion++;
                StateChanged?.Invoke();
            }
        }

        // ── Helpers ──────────────────────────────────────────────────

        /// <summary>Availability of a patient as reported by the host lifecycle.</summary>
        public PatientAvailability AvailabilityOf(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return PatientAvailability.Blocked("survivor_id_invalid");
            return _availability(survivor);
        }

        /// <summary>
        /// Resolve the affliction a treatment targets: the explicit target when
        /// given, otherwise the catalog definition's fixed affliction. Null when
        /// neither exists (the treatment requires a target from the caller).
        /// </summary>
        private static string? ResolveTargetAffliction(MedicalTreatmentDef def, AfflictionId? target)
        {
            if (target.HasValue && !target.Value.IsEmpty) return target.Value.Value;
            return string.IsNullOrEmpty(def.AfflictionId) ? null : def.AfflictionId;
        }

        private string? ValidatePatient(Survivors.SurvivorId survivor, string commandCode)
        {
            if (survivor.IsEmpty) return "survivor_id_invalid";
            var availability = _availability(survivor);
            return availability.Available ? null : availability.ReasonCode;
        }

        private void RollbackReservations(List<int> reservationIds)
        {
            foreach (var id in reservationIds) _reservations.Release(id);
        }

        private static CommandPreview Stale(long expectedVersion) =>
            CommandPreview.Unavailable("treatment.start", "stale_preview", "medical.stale_preview", expectedVersion);

        private static CommandPreview Unavailable(string code, string failure, long version) =>
            CommandPreview.Unavailable(code, failure, "medical." + failure, version);

        // ── Save / Load ──────────────────────────────────────────────

        public MedicalPipelineSaveState CaptureState()
        {
            return new MedicalPipelineSaveState
            {
                version = MedicalPipelineSaveState.CurrentVersion,
                diagnosis = _diagnosis.CaptureState(),
                reservations = _reservations.CaptureState(),
                procedures = _schedule.CaptureState(),
                stateVersion = StateVersion
            };
        }

        public void RestoreState(MedicalPipelineSaveState? saved)
        {
            if (saved == null) return;
            if (saved.version > MedicalPipelineSaveState.CurrentVersion)
                throw new InvalidOperationException(
                    $"MedicalPipelineSaveState version {saved.version} is newer than supported {MedicalPipelineSaveState.CurrentVersion}.");
            _diagnosis.RestoreState(saved.diagnosis);
            _reservations.RestoreState(saved.reservations);
            _schedule.RestoreState(saved.procedures);
            StateVersion = saved.stateVersion;
        }
    }
}
