using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// PLAN 147 — one activation of a contraband record as a discoverable bunker
    /// stash. A contraband record is content/profile data; this mapping is the
    /// ONLY executable bridge: it links the record to a canonical inventory item
    /// id granted once, after a deterministic campaign-day gate.
    ///
    /// The canonical item (items.json / greenhouse_items.json) remains the sole
    /// authority for weight, stack, trade value and use effects. No contraband
    /// field is executed directly by this mapping.
    /// </summary>
    [Serializable]
    public sealed class ContrabandStashActivation
    {
        public string entryId = string.Empty;
        public string canonicalItemId = string.Empty;
        public int grantQuantity = 1;
        public int minDay = 1;

        public override string ToString()
            => $"{entryId} -> {canonicalItemId} x{grantQuantity} (day >= {minDay})";
    }

    /// <summary>Serialized claim bookkeeping: which stash was claimed on which day. Once-only by construction.</summary>
    [Serializable]
    public sealed class ContrabandStashState
    {
        public string systemId = ContrabandStashSystem.SystemId;
        public Dictionary<string, int> claimedDayByEntry = new Dictionary<string, int>(StringComparer.Ordinal);
    }

    /// <summary>
    /// PLAN 147 — illicit stash discovery runtime over the BunkerContrabandCatalog.
    ///
    /// Contract (see docs/plans/CONTRABAND_MECHANICS_AUTHORITY_MATRIX.md):
    ///   - Catalog queries and availability checks are pure reads: no inventory,
    ///     morale, flag or economy mutation happens on load, query or open.
    ///   - Acquisition is once-only and deterministic: a stash becomes
    ///     discoverable from a fixed campaign day (real campaign state), and a
    ///     claim consumes the stash permanently. There is no RNG here — a
    ///     blocked claim cannot reroll by reopening a screen, and save/load
    ///     round-trips preserve claims exactly.
    ///   - The grant is a single atomic InventoryBill over the canonical item id;
    ///     capacity/weight validation belongs to the existing Inventory
    ///     authority. A blocked-at-capacity claim is NOT recorded and may be
    ///     retried after freeing space.
    ///   - market_price_scrip / hidden_stash_location / all mechanics fields of
    ///     the entry stay descriptive at this layer; they are not executed.
    /// </summary>
    public sealed class ContrabandStashSystem
    {
        public const string SystemId = "contraband_stash";

        private readonly BunkerContrabandCatalog _catalog;
        private readonly InventoryContainer _inventory;
        private readonly Func<string, ItemDefinition?>? _definitionLookup;
        private readonly ILog _log;

        private readonly Dictionary<string, ContrabandStashActivation> _activationsByEntry =
            new Dictionary<string, ContrabandStashActivation>(StringComparer.Ordinal);
        private ContrabandStashState _state = new ContrabandStashState();

        /// <summary>Raised exactly once per successful stash claim, after the inventory commit.</summary>
        public event Action<string, int>? OnStashClaimed; // entryId, day
        public event Action? OnStateChanged;

        public ContrabandStashSystem(
            BunkerContrabandCatalog catalog,
            InventoryContainer inventory,
            Func<string, ItemDefinition?>? definitionLookup = null,
            ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _definitionLookup = definitionLookup;
            _log = log ?? NullLog.Instance;
        }

        public IReadOnlyCollection<ContrabandStashActivation> Activations => _activationsByEntry.Values;
        public ContrabandStashState State => _state;

        // ── Registration ───────────────────────────────────────────────

        /// <summary>
        /// Registers one executable stash activation. Fails closed (returns false)
        /// for unknown entry ids, empty canonical item ids, non-positive grants,
        /// negative day gates or duplicate registrations.
        /// </summary>
        public bool TryRegisterActivation(ContrabandStashActivation activation)
        {
            if (activation == null
                || string.IsNullOrWhiteSpace(activation.entryId)
                || _catalog.GetById(activation.entryId) == null)
            {
                _log.Warn("[ContrabandStash] activation rejected: unknown or missing contraband entry id");
                return false;
            }
            if (string.IsNullOrWhiteSpace(activation.canonicalItemId)
                || activation.grantQuantity <= 0
                || activation.minDay < 0)
            {
                _log.Warn($"[ContrabandStash] activation rejected for '{activation.entryId}': invalid canonical item, quantity or day gate");
                return false;
            }
            if (_activationsByEntry.ContainsKey(activation.entryId))
            {
                _log.Warn($"[ContrabandStash] activation rejected for '{activation.entryId}': already registered");
                return false;
            }

            _activationsByEntry[activation.entryId] = activation;
            return true;
        }

        /// <summary>
        /// Plan 147 §13 vertical slices — the reviewed activation set proving
        /// one low-, one mid- and two high-tier contraband records end-to-end:
        ///   - contraband_card_deck_pinned_kings  -> item_playing_cards (tier 1, comfort/trade item)
        ///   - contraband_unrationed_sugar_brick  -> sugar             (tier 2, canonical moraleEffect through the item-use pipeline)
        ///   - contraband_century_seed_grain_vial -> item_seed_wheat   (tier 3, plantable through GreenhouseExpansionCatalog.CropCatalog)
        ///   - contraband_bootleg_morphine_ampoules -> morphine        (tier 3, canonical medical item; dependency via ChemicalDependencySystem)
        /// Every further activation requires a recorded owner in the authority matrix.
        /// </summary>
        public static IReadOnlyList<ContrabandStashActivation> DefaultActivations() => new List<ContrabandStashActivation>
        {
            new ContrabandStashActivation
            {
                entryId = "contraband_card_deck_pinned_kings",
                canonicalItemId = "item_playing_cards",
                grantQuantity = 1,
                minDay = 3
            },
            new ContrabandStashActivation
            {
                entryId = "contraband_unrationed_sugar_brick",
                canonicalItemId = "sugar",
                grantQuantity = 8,   // 800g brick = 8 canonical 100g sugar packets
                minDay = 8
            },
            new ContrabandStashActivation
            {
                entryId = "contraband_century_seed_grain_vial",
                canonicalItemId = "item_seed_wheat",
                grantQuantity = 1,
                minDay = 20
            },
            new ContrabandStashActivation
            {
                // Plan 147 follow-up: narcotics slice. Canonical `morphine` item
                // (items.json, healthEffect/moraleEffect) is the sole effect
                // authority; dependency risk routes exclusively through
                // ChemicalDependencySystem.OnSubstanceConsumed, fired by the
                // host once per committed consumption (OnConsumed). The JSON
                // instant_pain_relief_hp=40 / chemical_dependency_risk=0.35
                // remain non-executed.
                entryId = "contraband_bootleg_morphine_ampoules",
                canonicalItemId = "morphine",
                grantQuantity = 4,
                minDay = 25
            }
        };

        // ── Pure availability queries (no mutation) ────────────────────

        public bool IsActivated(string entryId)
            => !string.IsNullOrEmpty(entryId) && _activationsByEntry.ContainsKey(entryId);

        public bool IsClaimed(string entryId)
            => !string.IsNullOrEmpty(entryId) && _state.claimedDayByEntry.ContainsKey(entryId);

        /// <summary>A stash is discoverable when activated, unclaimed and the campaign day gate has passed.</summary>
        public bool IsDiscoverable(string entryId, int day)
        {
            if (!IsActivated(entryId)) return false;
            if (IsClaimed(entryId)) return false;
            return day >= _activationsByEntry[entryId].minDay;
        }

        /// <summary>All currently discoverable stash entries for the given day. Pure read.</summary>
        public List<ContrabandEntry> ListDiscoverable(int day)
        {
            var result = new List<ContrabandEntry>();
            foreach (var activation in _activationsByEntry.Values)
            {
                if (!IsClaimed(activation.entryId) && day >= activation.minDay)
                {
                    var entry = _catalog.GetById(activation.entryId);
                    if (entry != null) result.Add(entry);
                }
            }
            result.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            return result;
        }

        /// <summary>The canonical grant an activation would award. Pure read — no inventory contact.</summary>
        public ContrabandStashActivation? GetActivation(string entryId)
            => IsActivated(entryId) ? _activationsByEntry[entryId] : null;

        // ── Acquisition ─────────────────────────────────────────────────

        /// <summary>
        /// Claims a stash: validates the once-only bookkeeping first, then commits
        /// a single atomic InventoryBill of the canonical item. The claim is
        /// recorded only after the inventory commit succeeds, so a capacity-blocked
        /// claim can be retried and can never destroy the stash.
        /// </summary>
        public ActionResult TryClaimStash(string entryId, int day)
        {
            if (string.IsNullOrEmpty(entryId) || !_activationsByEntry.TryGetValue(entryId, out var activation))
                return ActionResult.Failed("contraband_not_activated", "contraband.unknown_entry");

            if (IsClaimed(entryId))
                return ActionResult.Blocked("contraband_already_claimed", "contraband.stash_already_emptied");

            if (day < activation.minDay)
                return ActionResult.Blocked("contraband_not_yet_discoverable", "contraband.stash_not_found_yet");

            var bill = new InventoryBill().AddGrant(activation.canonicalItemId, activation.grantQuantity);
            var validation = _inventory.ValidateTransaction(bill, _definitionLookup);
            if (!validation.IsValid)
            {
                return ActionResult.Blocked(
                    validation.FailureReason ?? "storage_capacity_exceeded",
                    "contraband.no_room_for_stash");
            }

            bool committed = _inventory.TryExecuteTransaction(bill, null, _definitionLookup);
            if (!committed)
            {
                // Defensive: ValidateTransaction passed but commit failed — do not
                // record the claim; the stash stays available.
                _log.Warn($"[ContrabandStash] commit failed for '{entryId}' — stash left in place");
                return ActionResult.Failed("transaction_commit_failed", "contraband.stash_commit_failed");
            }

            _state.claimedDayByEntry[entryId] = day;
            OnStashClaimed?.Invoke(entryId, day);
            OnStateChanged?.Invoke();

            return ActionResult.Success(
                "contraband.stash_claimed",
                new Dictionary<string, double>
                {
                    { "grant_quantity", activation.grantQuantity }
                });
        }

        // ── Save / load (house pattern) ─────────────────────────────────

        public ContrabandStashState CaptureState() => CloneState(_state);

        public void RestoreState(ContrabandStashState? saved)
        {
            // Old saves have no contraband section: null/empty restores to a fresh
            // (all stashes available) state and cannot grant or remove wealth.
            _state = saved == null ? new ContrabandStashState() : CloneState(saved);
            OnStateChanged?.Invoke();
        }

        private static ContrabandStashState CloneState(ContrabandStashState src)
        {
            var serializer = new SystemTextJsonSerializer();
            var json = serializer.Serialize(src);
            return serializer.Deserialize<ContrabandStashState>(json) ?? new ContrabandStashState();
        }
    }
}
