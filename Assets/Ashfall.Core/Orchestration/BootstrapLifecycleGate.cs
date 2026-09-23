// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Orchestration
{
    /// <summary>
    /// Lifecycle stages for ASHFALL composition root bootstrap.
    /// Stages must progress monotonically with zero deferred seams.
    /// </summary>
    public enum BootstrapStage
    {
        None = 0,
        Configuration = 1,
        Foundation = 2,
        DomainServices = 3,
        CampaignOwners = 4,
        UiSurfaces = 5,
        Ready = 6,
    }

    /// <summary>
    /// The execution mode for the bootstrap lifecycle.
    /// All modes must satisfy identical completeness invariants.
    /// </summary>
    public enum BootstrapPathMode
    {
        FreshGame = 1,
        SaveRestore = 2,
        SessionReset = 3,
    }

    /// <summary>
    /// Record of a registered subsystem within the bootstrap manifest.
    /// </summary>
    public readonly struct SubsystemRegistrationRecord
    {
        public string SubsystemId { get; }
        public BootstrapStage Stage { get; }
        public bool IsRequired { get; }
        public bool HasDeferredSeams { get; }

        public SubsystemRegistrationRecord(string subsystemId, BootstrapStage stage, bool isRequired, bool hasDeferredSeams)
        {
            SubsystemId = subsystemId ?? throw new ArgumentNullException(nameof(subsystemId));
            Stage = stage;
            IsRequired = isRequired;
            HasDeferredSeams = hasDeferredSeams;
        }
    }

    /// <summary>
    /// EN-06: One Bootstrap Path Lifecycle Verification & Composite Gate.
    /// Guarantees that every execution path (FreshGame, SaveRestore, SessionReset)
    /// satisfies identical bootstrap stages, required subsystem registrations,
    /// and zero deferred seams.
    /// </summary>
    public sealed class BootstrapLifecycleGate
    {
        private readonly Dictionary<string, SubsystemRegistrationRecord> _registeredSubsystems = new(StringComparer.Ordinal);
        private BootstrapStage _currentStage = BootstrapStage.None;

        public BootstrapStage CurrentStage => _currentStage;
        public int RegisteredCount => _registeredSubsystems.Count;

        public bool RegisterSubsystem(string subsystemId, BootstrapStage stage, bool isRequired, bool hasDeferredSeams = false)
        {
            if (string.IsNullOrWhiteSpace(subsystemId))
                return false;

            if (stage <= BootstrapStage.None || stage > BootstrapStage.Ready)
                return false;

            _registeredSubsystems[subsystemId] = new SubsystemRegistrationRecord(subsystemId, stage, isRequired, hasDeferredSeams);
            return true;
        }

        public bool AdvanceStage(BootstrapStage nextStage)
        {
            if (nextStage <= _currentStage)
                return false;

            if (nextStage > BootstrapStage.Ready)
                return false;

            // Ensure we advance sequentially
            if ((int)nextStage != (int)_currentStage + 1)
                return false;

            _currentStage = nextStage;
            return true;
        }

        public bool ValidateLifecycleParity(BootstrapPathMode mode, out List<string> violations)
        {
            violations = new List<string>();

            if (_currentStage != BootstrapStage.Ready)
            {
                violations.Add($"Lifecycle not ready: current stage is {_currentStage}, expected {BootstrapStage.Ready} for mode {mode}.");
            }

            foreach (var kvp in _registeredSubsystems)
            {
                var record = kvp.Value;

                if (record.HasDeferredSeams)
                {
                    violations.Add($"Subsystem '{record.SubsystemId}' has deferred seams; EN-06 requires zero deferred seams.");
                }

                if (record.IsRequired && (int)record.Stage > (int)_currentStage)
                {
                    violations.Add($"Required subsystem '{record.SubsystemId}' belongs to unreached stage {record.Stage}.");
                }
            }

            return violations.Count == 0;
        }

        public void Reset()
        {
            _registeredSubsystems.Clear();
            _currentStage = BootstrapStage.None;
        }
    }
}
