using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Shelter
{
    // ── Catalog (authored data authority) ───────────────────────────

    /// <summary>
    /// One preserved genetic line. Recovery releases an existing canonical
    /// item (seed packet / ampoule) — greenhouse and pharma keep ownership
    /// of cultivation and medicine; the vault only stores and preserves.
    /// </summary>
    [Serializable]
    public sealed class CryoCultivarDef
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string sample_type = "seed";           // seed | culture | tissue
        public string source_item_id = string.Empty;  // item consumed at registration
        public string recovery_item_id = string.Empty; // canonical item released on recovery
        public int recovery_amount = 1;
        public float stable_decay_per_day = 0.2f;     // viability permille/day, coolant healthy
        public float unstable_decay_per_day = 6f;     // viability permille/day, warning/critical
        public float breach_decay_per_day = 12f;      // viability permille/day during breach
        public float radiation_sensitivity = 0.5f;    // 0..1 exposure multiplier
        public int storage_tier = 1;                  // 1..3 (vault section quality)
        public int recovery_difficulty = 1;           // 1..3
        public int recovery_days = 1;
        public string[] traits = Array.Empty<string>();
    }

    [Serializable]
    public sealed class CryoCultivarsFile
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<CryoCultivarDef> cultivars = new List<CryoCultivarDef>();
    }

    public sealed class CryoCultivarCatalog
    {
        private readonly Dictionary<string, CryoCultivarDef> _byId =
            new Dictionary<string, CryoCultivarDef>(StringComparer.Ordinal);

        public List<string> Errors { get; } = new List<string>();
        public IReadOnlyCollection<CryoCultivarDef> Cultivars => _byId.Values;

        public void Load(CryoCultivarsFile file)
        {
            if (file?.cultivars == null) return;
            for (int i = 0; i < file.cultivars.Count; i++)
            {
                var c = file.cultivars[i];
                if (c == null || string.IsNullOrEmpty(c.id))
                {
                    Errors.Add("cultivars[" + i + "]: missing id");
                    continue;
                }
                if (string.IsNullOrEmpty(c.recovery_item_id)) Errors.Add(c.id + ": missing recovery_item_id");
                if (string.IsNullOrEmpty(c.source_item_id)) Errors.Add(c.id + ": missing source_item_id");
                if (c.recovery_amount < 1) Errors.Add(c.id + ": recovery_amount must be >= 1");
                if (c.radiation_sensitivity < 0f || c.radiation_sensitivity > 1f)
                    Errors.Add(c.id + ": radiation_sensitivity out of range 0..1");
                if (_byId.ContainsKey(c.id))
                {
                    Errors.Add(c.id + ": duplicate cultivar id");
                    continue;
                }
                _byId[c.id] = c;
            }
        }

        public CryoCultivarDef? GetCultivar(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _byId.TryGetValue(id, out var def) ? def : null;
        }
    }

    public static class CryoCultivarCatalogLoader
    {
        public const string FileName = "cryo_cultivars.json";

        public static CryoCultivarCatalog Load(
            string dataDirectory,
            IFileIO? files = null,
            IJsonSerializer? serializer = null)
        {
            files = files ?? new FileSystemIO();
            serializer = serializer ?? new SystemTextJsonSerializer();
            var catalog = new CryoCultivarCatalog();
            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path)) return catalog;
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return catalog;
            try
            {
                var file = serializer.Deserialize<CryoCultivarsFile>(text);
                catalog.Load(file);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "CryoCultivarsFile", ex_CATDIAG);
            }
            return catalog;
        }
    }

    // ── Runtime state ───────────────────────────────────────────────

    public enum CryoCanisterPhase
    {
        Empty = 0,
        Loaded = 1,        // registered, awaiting first tick classification
        Stable = 2,
        Warning = 3,
        Critical = 4,
        RecoveryQueued = 5,
        Thawing = 6,
        Released = 7,
        Failed = 8
    }

    [Serializable]
    public sealed class CryoCanisterState
    {
        public string canister_id = string.Empty;
        public string cultivar_id = string.Empty;
        public int viability_permille = 1000;   // 0..1000, persisted
        public int phase = (int)CryoCanisterPhase.Loaded;
        public int thaw_progress_days;
        public bool triage_protected;           // player ranking for breach triage
    }

    [Serializable]
    public sealed class CryoVaultSaveState
    {
        public int current_day;
        public int next_canister_number = 1;
        public List<CryoCanisterState> canisters = new List<CryoCanisterState>();
        public float coolant_reserve = 60f;     // 0..100
        public int insulation_level;            // 0..3 (B66 shielding plates)
        public bool breach_active;
        public string breach_reason = string.Empty;
        public int breach_started_day = -1;
        public List<string> released_log = new List<string>();
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// Plan B69 — Cryogenic Sample Preservation &amp; Genetic Cultivar Seed Vault.
    ///
    /// Stores rare biological lines and preserves viability. Ownership rules:
    /// - Coolant production is the CryogenicAirSeparationSystem's job
    ///   (item_nitrogen_supply); this system only consumes it.
    /// - Insulation upgrades consume B66 metallurgy shielding plates.
    /// - Greenhouse owns cultivation; pharma owns medicine. Recovery releases
    ///   existing canonical items through inventory — no parallel registry.
    /// - Radiation exposure comes from a provider port, never computed here.
    /// - Deterministic: viability loss is persisted, never rerolled after
    ///   restore; recovery outcomes are viability-gated thresholds, not RNG.
    /// </summary>
    public sealed class CryoVaultSystem
    {
        public const string SystemId = "cryo_vault";
        public const float MaxCoolant = 100f;
        public const float CoolantPerNitrogenUnit = 35f;
        public const int MinViabilityForRecovery = 200;
        public const int MaxInsulationLevel = 3;
        public const float BreachProtectedDrainFactor = 0.4f;

        public const string CoolantItemId = "item_nitrogen_supply";
        public const string InsulationPlateItemId = "item_metallurgy_shielding_plate";

        private readonly ISeededRng _rng;
        private readonly InventoryContainer? _inventory;
        private readonly ILog _log;
        private readonly Func<float>? _radiationExposureProvider;  // 0..1 daily normalized
        private readonly Func<bool>? _powerAvailableProvider;

        private readonly Dictionary<string, CryoCultivarDef> _catalog =
            new Dictionary<string, CryoCultivarDef>(StringComparer.Ordinal);
        private CryoVaultSaveState _state = new CryoVaultSaveState();

        public CryoVaultSaveState State => _state;
        public IReadOnlyDictionary<string, CryoCultivarDef> Catalog => _catalog;

        public event Action<string>? OnVaultWarning;                       // message
        public event Action<string, int, int>? OnSampleViabilityChanged;   // canisterId, old, new
        public event Action<string>? OnBreachStarted;                      // reason
        public event Action<string, string, int, int>? OnSampleReleased;   // canisterId, itemId, amount, viability
        public event Action<string>? OnSampleFailed;                       // canisterId
        public event Action? OnStateChanged;

        public CryoVaultSystem(
            ISeededRng rng,
            InventoryContainer? inventory = null,
            Func<float>? radiationExposureProvider = null,
            Func<bool>? powerAvailableProvider = null,
            CryoVaultSaveState? state = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory;
            _radiationExposureProvider = radiationExposureProvider;
            _powerAvailableProvider = powerAvailableProvider;
            _log = log ?? NullLog.Instance;
            if (state != null) RestoreState(state);
        }

        public void LoadCatalog(CryoCultivarsFile file)
        {
            var catalog = new CryoCultivarCatalog();
            catalog.Load(file);
            foreach (var err in catalog.Errors) _log.Warn("[CryoVault] " + err);
            foreach (var c in catalog.Cultivars) _catalog[c.id] = c;
        }

        public void LoadCatalogContent(CryoCultivarCatalog catalog)
        {
            if (catalog == null) return;
            foreach (var err in catalog.Errors) _log.Warn("[CryoVault] " + err);
            foreach (var c in catalog.Cultivars) _catalog[c.id] = c;
        }

        // ── Queries ─────────────────────────────────────────────────

        public bool IsBreachActive => _state.breach_active;
        public float CoolantReserve => _state.coolant_reserve;

        /// <summary>Vault-level thermal classification. Derived, never persisted.</summary>
        public CryoCanisterPhase ThermalStage
        {
            get
            {
                bool power = _powerAvailableProvider?.Invoke() ?? true;
                if (_state.breach_active) return CryoCanisterPhase.Critical;
                if (!power || _state.coolant_reserve <= 10f) return CryoCanisterPhase.Critical;
                if (_state.coolant_reserve <= 25f) return CryoCanisterPhase.Warning;
                return CryoCanisterPhase.Stable;
            }
        }

        // ── Actions ─────────────────────────────────────────────────

        /// <summary>
        /// Register a rare sample. Atomic: the source item is consumed exactly
        /// once and the line exists in exactly one canister — never duplicated
        /// between canister and inventory.
        /// </summary>
        public string RegisterSample(string cultivarId)
        {
            if (!_catalog.TryGetValue(cultivarId, out var def))
                return "Unknown cultivar: " + cultivarId;
            if (_state.breach_active)
                return "The vault is in breach; the seal will not open until it is resolved.";

            var inv = _inventory;
            if (inv != null && !inv.HasSufficient(def.source_item_id, 1))
                return "Missing sample source " + def.source_item_id + ".";

            // Atomic commit: consume the source, create the canister.
            inv?.TryConsume(def.source_item_id, 1);
            var canister = new CryoCanisterState
            {
                canister_id = "cryo_canister_" + _state.next_canister_number,
                cultivar_id = cultivarId,
                viability_permille = 1000,
                phase = (int)CryoCanisterPhase.Loaded
            };
            _state.next_canister_number++;
            _state.canisters.Add(canister);

            _log.Info($"[CryoVault] registered {cultivarId} as {canister.canister_id}");
            OnStateChanged?.Invoke();
            return "Sample registered: " + def.display_name + " (" + canister.canister_id + ").";
        }

        /// <summary>Replenish coolant from canonical nitrogen supply.</summary>
        public string ReplenishCoolant()
        {
            var inv = _inventory;
            if (inv != null && !inv.HasSufficient(CoolantItemId, 1))
                return "No nitrogen supply units in stock.";

            if (_state.coolant_reserve >= MaxCoolant - 0.5f)
                return "Coolant reserve is already full.";

            inv?.TryConsume(CoolantItemId, 1);
            _state.coolant_reserve = Math.Min(MaxCoolant, _state.coolant_reserve + CoolantPerNitrogenUnit);
            OnStateChanged?.Invoke();
            return "Coolant replenished to " + _state.coolant_reserve.ToString("F0") + ".";
        }

        /// <summary>Vault insulation upgrade — consumes B66 heavy shielding plate.</summary>
        public string UpgradeInsulation()
        {
            if (_state.insulation_level >= MaxInsulationLevel)
                return "Vault insulation is already at its rated maximum.";

            var inv = _inventory;
            if (inv != null && !inv.HasSufficient(InsulationPlateItemId, 1))
                return "Missing " + InsulationPlateItemId + " for the insulation upgrade.";

            inv?.TryConsume(InsulationPlateItemId, 1);
            _state.insulation_level++;
            OnStateChanged?.Invoke();
            return "Insulation upgraded to level " + _state.insulation_level + ".";
        }

        /// <summary>Queue a sample for controlled recovery/thaw.</summary>
        public string QueueRecovery(string canisterId)
        {
            var canister = FindCanister(canisterId);
            if (canister == null) return "Unknown canister: " + canisterId;
            if (canister.phase == (int)CryoCanisterPhase.RecoveryQueued
                || canister.phase == (int)CryoCanisterPhase.Thawing)
                return "Recovery already in progress for " + canisterId + ".";
            if (canister.phase != (int)CryoCanisterPhase.Loaded
                && canister.phase != (int)CryoCanisterPhase.Stable
                && canister.phase != (int)CryoCanisterPhase.Warning
                && canister.phase != (int)CryoCanisterPhase.Critical)
                return "Canister " + canisterId + " is not in a recoverable state.";

            if ((_powerAvailableProvider?.Invoke() ?? true) == false)
                return "No power for the recovery heater.";

            canister.phase = (int)CryoCanisterPhase.RecoveryQueued;
            canister.thaw_progress_days = 0;
            OnStateChanged?.Invoke();
            return "Recovery queued for " + canisterId + ".";
        }

        /// <summary>Breach triage: mark a canister protected (drains slower).</summary>
        public string SetTriageProtection(string canisterId, bool protected_)
        {
            var canister = FindCanister(canisterId);
            if (canister == null) return "Unknown canister: " + canisterId;
            canister.triage_protected = protected_;
            OnStateChanged?.Invoke();
            return protected_ ? canisterId + " marked protected." : canisterId + " protection cleared.";
        }

        /// <summary>
        /// Start a vault breach (power loss, room damage, equipment failure,
        /// seismic event). Bounded degradation with time to triage — never an
        /// instant wipe.
        /// </summary>
        public void TriggerBreach(string reason)
        {
            if (_state.breach_active) return;
            _state.breach_active = true;
            _state.breach_reason = reason ?? string.Empty;
            _state.breach_started_day = _state.current_day;
            _log.Warn("[CryoVault] BREACH: " + reason);
            OnBreachStarted?.Invoke(reason ?? string.Empty);
            OnStateChanged?.Invoke();
        }

        /// <summary>End a breach once the cause is repaired.</summary>
        public void ResolveBreach()
        {
            if (!_state.breach_active) return;
            _state.breach_active = false;
            _state.breach_reason = string.Empty;
            OnStateChanged?.Invoke();
        }

        // ── Daily tick ──────────────────────────────────────────────

        public void TickDay(int day)
        {
            _state.current_day = day;
            bool power = _powerAvailableProvider?.Invoke() ?? true;

            // Coolant burn: populated vault only; breach boils it off.
            if (_state.canisters.Count > 0 || _state.breach_active)
            {
                float burn = _state.breach_active ? 12f : Math.Max(1f, 4f - _state.insulation_level * 0.8f);
                _state.coolant_reserve = Math.Max(0f, _state.coolant_reserve - burn);
            }

            float radiation = _radiationExposureProvider?.Invoke() ?? 0f;

            // Snapshot: recovery completion removes canisters mid-loop.
            var snapshot = new List<CryoCanisterState>(_state.canisters);
            foreach (var canister in snapshot)
            {
                if (canister.phase == (int)CryoCanisterPhase.Released
                    || canister.phase == (int)CryoCanisterPhase.Failed)
                    continue;

                // Recovery pipeline.
                if (canister.phase == (int)CryoCanisterPhase.RecoveryQueued
                    || canister.phase == (int)CryoCanisterPhase.Thawing)
                {
                    if (!power)
                    {
                        canister.phase = (int)CryoCanisterPhase.Thawing; // stalled, not lost
                    }
                    else
                    {
                        canister.phase = (int)CryoCanisterPhase.Thawing;
                        canister.thaw_progress_days++;
                        var def = _catalog.TryGetValue(canister.cultivar_id, out var d) ? d : null;
                        int required = Math.Max(1, def?.recovery_days ?? 1);
                        if (canister.thaw_progress_days >= required)
                        {
                            CompleteRecovery(canister);
                            continue;
                        }
                    }
                }
                else
                {
                    // Thermal classification drives the decay profile.
                    var thermal = ThermalStage;
                    bool stable = thermal == CryoCanisterPhase.Stable;
                    var decay = stable
                        ? (_catalog.TryGetValue(canister.cultivar_id, out var d1) ? d1.stable_decay_per_day : 0.2f)
                        : (_catalog.TryGetValue(canister.cultivar_id, out var d2) ? d2.unstable_decay_per_day : 6f);
                    if (_state.breach_active)
                        decay = _catalog.TryGetValue(canister.cultivar_id, out var d3) ? d3.breach_decay_per_day : 12f;

                    // Radiation exposure scales decay by cultivar sensitivity.
                    decay *= 1f + radiation * (_catalog.TryGetValue(canister.cultivar_id, out var d4) ? d4.radiation_sensitivity : 0.5f);
                    if (_state.breach_active && canister.triage_protected)
                        decay *= BreachProtectedDrainFactor;

                    ApplyDecay(canister, decay, stable);
                }
            }

            RaiseWarnings();
            OnStateChanged?.Invoke();
        }

        private void ApplyDecay(CryoCanisterState canister, float decay, bool wasStable)
        {
            int old = canister.viability_permille;
            int newV = Math.Max(0, canister.viability_permille - (int)Math.Ceiling(decay));
            canister.viability_permille = newV;

            if (wasStable)
            {
                if (ThermalStage == CryoCanisterPhase.Warning) canister.phase = (int)CryoCanisterPhase.Warning;
                else if (ThermalStage == CryoCanisterPhase.Critical) canister.phase = (int)CryoCanisterPhase.Critical;
                else canister.phase = (int)CryoCanisterPhase.Stable;
            }
            else if (ThermalStage == CryoCanisterPhase.Stable)
            {
                canister.phase = (int)CryoCanisterPhase.Stable;
            }
            else
            {
                canister.phase = (int)ThermalStage;
            }

            if (old != newV) OnSampleViabilityChanged?.Invoke(canister.canister_id, old, newV);
        }

        private void CompleteRecovery(CryoCanisterState canister)
        {
            var def = _catalog.TryGetValue(canister.cultivar_id, out var d) ? d : null;
            if (def == null)
            {
                canister.phase = (int)CryoCanisterPhase.Failed;
                OnSampleFailed?.Invoke(canister.canister_id);
                return;
            }

            // Viability-gated deterministic outcome — resolved once, persisted.
            if (canister.viability_permille < MinViabilityForRecovery)
            {
                canister.phase = (int)CryoCanisterPhase.Failed;
                RemoveCanister(canister.canister_id);
                _log.Warn($"[CryoVault] recovery failed for {canister.canister_id} (viability {canister.viability_permille})");
                OnSampleFailed?.Invoke(canister.canister_id);
                return;
            }

            var inv = _inventory;
            bool delivered = inv == null || inv.CanAddById(def.recovery_item_id, def.recovery_amount);
            if (!delivered)
            {
                // Transactional: canister is NOT cleared; the host frees space
                // and the recovery completes on a later tick.
                return;
            }

            inv?.AddById(def.recovery_item_id, def.recovery_amount);
            _state.released_log.Add(canister.canister_id + ":" + def.recovery_item_id);
            RemoveCanister(canister.canister_id);
            OnSampleReleased?.Invoke(canister.canister_id, def.recovery_item_id, def.recovery_amount, canister.viability_permille);
        }

        private void RaiseWarnings()
        {
            var stage = ThermalStage;
            if (stage == CryoCanisterPhase.Warning)
                OnVaultWarning?.Invoke("Coolant reserve low (" + _state.coolant_reserve.ToString("F0") + "). Samples are degrading.");
            else if (stage == CryoCanisterPhase.Critical)
                OnVaultWarning?.Invoke(_state.breach_active
                    ? "VAULT BREACH: " + _state.breach_reason
                    : "Coolant critical. Samples are losing viability rapidly.");
        }

        private CryoCanisterState? FindCanister(string canisterId)
        {
            foreach (var c in _state.canisters)
                if (c.canister_id == canisterId) return c;
            return null;
        }

        private void RemoveCanister(string canisterId)
        {
            for (int i = _state.canisters.Count - 1; i >= 0; i--)
            {
                if (_state.canisters[i].canister_id == canisterId)
                {
                    _state.canisters.RemoveAt(i);
                    return;
                }
            }
        }

        // ── Save / Load ─────────────────────────────────────────────

        public CryoVaultSaveState CaptureState() => CloneState(_state);

        public void RestoreState(CryoVaultSaveState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            if (_state.canisters == null) _state.canisters = new List<CryoCanisterState>();
            if (_state.released_log == null) _state.released_log = new List<string>();
        }

        private static CryoVaultSaveState CloneState(CryoVaultSaveState src)
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<CryoVaultSaveState>(json) ?? new CryoVaultSaveState();
        }
    }
}
