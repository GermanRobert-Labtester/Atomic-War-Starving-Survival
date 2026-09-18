// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Ports
{
    /// <summary>
    /// Requirement classification for a Core integration seam.
    /// Matches the taxonomy in docs/ci/port_contract_policy.json (Plan 36A).
    /// </summary>
    public enum PortRequirement
    {
        /// <summary>Production-required: must be bound and called by the host application (src/).</summary>
        RequiredHost,

        /// <summary>Optional host capability: can be bound by host if supported, otherwise null-safe.</summary>
        OptionalHost,

        /// <summary>Internal Core call chain: invoked internally within Assets/Ashfall.Core.</summary>
        LiveViaCore,

        /// <summary>Test or diagnostic only: invoked only by unit/integration tests or diagnostic harnesses.</summary>
        TestOnly,

        /// <summary>Pure library method: mathematical, geometric, or algorithmic utility with no stateful host wiring needed.</summary>
        PureLibrary,

        /// <summary>Exemption / planned future seam: tracked with dated owner and activation condition.</summary>
        Deferred
    }

    /// <summary>
    /// Lifecycle stage at which the integration port is expected to be bound.
    /// Aligns with Plan 28C / Plan 36B lifecycle ordering.
    /// </summary>
    public enum PortLifecycleStage
    {
        /// <summary>Subsystem construction or session composition time.</summary>
        Setup,

        /// <summary>Campaign day-tick or simulation advance step.</summary>
        DayTick,

        /// <summary>Interactive player command or discrete simulated action.</summary>
        Action,

        /// <summary>Triggered reactively by domain events.</summary>
        Event
    }

    /// <summary>
    /// Declarative attribute for tagging Core integration seams machine-readably.
    /// Used by static gates and documentation generators to verify host wiring.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class PortContractAttribute : Attribute
    {
        /// <summary>Unique identifier for the port (e.g., "combat.damage_survivor").</summary>
        public string PortId { get; set; } = string.Empty;

        /// <summary>Owner subsystem or architectural domain (e.g., "combat", "medical", "economy").</summary>
        public string Owner { get; set; } = string.Empty;

        /// <summary>Classification requirement.</summary>
        public PortRequirement Requirement { get; set; } = PortRequirement.RequiredHost;

        /// <summary>Expected caller ID or host session (e.g., "CombatHostSession").</summary>
        public string RequiredCaller { get; set; } = string.Empty;

        /// <summary>Identifies the underlying gameplay effect (e.g., "survivor_damage_applied").</summary>
        public string EffectId { get; set; } = string.Empty;

        /// <summary>Lifecycle phase when binding occurs.</summary>
        public PortLifecycleStage LifecycleStage { get; set; } = PortLifecycleStage.Setup;

        /// <summary>Reason if port is optional, test-only, or pure library.</summary>
        public string? OptionalReason { get; set; }

        /// <summary>Activation condition if classification is Deferred.</summary>
        public string? ActivationCondition { get; set; }

        /// <summary>Expiry date (YYYY-MM-DD) if classification is Deferred.</summary>
        public string? Expiry { get; set; }

        public PortContractAttribute() { }

        public PortContractAttribute(string portId, string owner, PortRequirement requirement = PortRequirement.RequiredHost)
        {
            PortId = portId;
            Owner = owner;
            Requirement = requirement;
        }
    }

    /// <summary>
    /// Structured representation of an integration port contract for metadata serialization and policy reporting.
    /// </summary>
    public sealed class PortContractDefinition
    {
        public string PortId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public bool Diagnostic { get; set; }
        public string? RequiredCaller { get; set; }
        public string? EffectId { get; set; }
        public string? LifecycleStage { get; set; }
        public string? ActivationCondition { get; set; }
        public string? Expiry { get; set; }
    }

    /// <summary>
    /// Result of a runtime port validation check.
    /// </summary>
    public sealed class PortValidationResult
    {
        public string SubsystemId { get; set; } = string.Empty;
        public IReadOnlyList<string> RequiredPorts { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> BoundPorts { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> MissingPorts { get; set; } = Array.Empty<string>();
        public bool IsValid => MissingPorts.Count == 0;
    }

    /// <summary>
    /// Contract for systems or host sessions that expose verifiable effect ports.
    /// </summary>
    public interface IPortValidator
    {
        PortValidationResult ValidatePorts();
    }
}
