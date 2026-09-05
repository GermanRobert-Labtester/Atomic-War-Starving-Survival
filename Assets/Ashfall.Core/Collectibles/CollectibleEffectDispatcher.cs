using System;
using System.Collections.Generic;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace Ashfall.Core
{
    /// <summary>
    /// Outcome of one collectible acquisition dispatch. <see cref="FailureReason"/>
    /// is empty exactly when the dispatch succeeded; failures are explicit and
    /// retryable (discovery is only registered on success, so a later fresh
    /// acquisition of the same collectible retries the effect).
    /// </summary>
    public class CollectibleDispatchResult
    {
        public bool IsCollectible;
        public bool AlreadyDiscovered;
        public string EffectType = string.Empty;
        public bool EffectApplied;
        public bool DiscoveryRegistered;
        public string FailureReason = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic adapter routing a FIRST collectible acquisition into exactly
    /// one call to the appropriate existing campaign authority. This is a
    /// router, not a system: it owns no morale/research/journal/map state of
    /// its own and introduces no RNG.
    ///
    /// Wired into the general inventory acquisition path
    /// (<c>Inventory.OnItemAdded</c>), so collectibles dispatch identically
    /// whether obtained from scavenging, purchase, quest reward, or scripted
    /// grant. Save restore and inventory reconstruction never fire
    /// <c>OnItemAdded</c> and therefore never dispatch; discovery idempotence
    /// remains defense-in-depth on top of that.
    ///
    /// Atomic ordering per acquisition:
    /// 1. look up the collectible; 2. non-collectibles no-op;
    /// 3. already-discovered no-ops; 4. dispatch the external effect;
    /// 5. mark discovered ONLY on success (a failed dispatch stays
    /// undiscovered and retries on the next acquisition);
    /// 6. <c>none</c> effects still register discovery.
    ///
    /// Effect routing (existing authorities only):
    /// - morale        → NeedsSystem.Modify(Morale, bounded value) to living roster
    /// - knowledge     → ResearchSystem.UnlockManual(target)
    /// - journal_unlock→ JournalSystem.TryDiscoverKnowledge(target) (entry + codex)
    /// - faction_info  → JournalSystem.TryDiscoverKnowledge(target) (codex authority;
    ///                   no separate faction-intel system exists in the campaign)
    /// - location_clue → WastelandMapSystem.Discover(target) (strict node resolution)
    /// - none          → no authority; discovery still registered
    ///
    /// Authorities are injected as lazy providers because hosts construct
    /// their systems in phased order; a provider returning null while its system
    /// is genuinely absent yields an explicit, retryable failure — never a
    /// silently swallowed effect.
    /// </summary>
    public class CollectibleEffectDispatcher
    {
        /// <summary>Morale effect values are authored at 1–2; hard bound for safety.</summary>
        public const float MaxMoraleEffectValue = 10f;

        private readonly CollectibleCatalog _catalog;
        private readonly CollectibleDiscoveryState _discovery;
        private readonly Func<NeedsSystem?>? _needsProvider;
        private readonly Func<ResearchSystem?>? _researchProvider;
        private readonly Func<JournalSystem?>? _journalProvider;
        private readonly Func<WastelandMapSystem?>? _mapProvider;
        private readonly Func<int>? _dayProvider;
        private readonly ILog _log;

        public CollectibleEffectDispatcher(
            CollectibleCatalog catalog,
            CollectibleDiscoveryState discovery,
            Func<NeedsSystem?>? needsProvider = null,
            Func<ResearchSystem?>? researchProvider = null,
            Func<JournalSystem?>? journalProvider = null,
            Func<WastelandMapSystem?>? mapProvider = null,
            Func<int>? dayProvider = null,
            ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _discovery = discovery ?? throw new ArgumentNullException(nameof(discovery));
            _needsProvider = needsProvider;
            _researchProvider = researchProvider;
            _journalProvider = journalProvider;
            _mapProvider = mapProvider;
            _dayProvider = dayProvider;
            _log = log ?? NullLog.Instance;
        }

        /// <summary>Campaign discovery ledger shared with the save store.</summary>
        public CollectibleDiscoveryState Discovery => _discovery;

        /// <summary>
        /// Route one inventory acquisition. Safe to call for every item that
        /// enters the shelter inventory: non-collectibles and repeat
        /// acquisitions are no-ops.
        /// </summary>
        public CollectibleDispatchResult DispatchOnAcquire(string itemId)
        {
            var result = new CollectibleDispatchResult();

            CollectibleDefinition? def = _catalog.GetByItemId(itemId);
            if (def == null)
            {
                result.IsCollectible = false;
                return result; // ordinary item — no-op
            }

            result.IsCollectible = true;
            result.EffectType = string.IsNullOrEmpty(def.effect_type) ? "none" : def.effect_type;

            if (_discovery.IsDiscovered(itemId))
            {
                result.AlreadyDiscovered = true;
                return result; // one-time effect already handled — never replay
            }

            bool applied;
            switch (result.EffectType)
            {
                case "none":
                    applied = true;
                    break;
                case "morale":
                    applied = ApplyMorale(def, result);
                    break;
                case "knowledge":
                    applied = ApplyKnowledge(def, result);
                    break;
                case "journal_unlock":
                case "faction_info":
                    applied = ApplyJournalUnlock(def, result);
                    break;
                case "location_clue":
                    applied = ApplyLocationClue(def, result);
                    break;
                default:
                    result.FailureReason = $"unknown_effect_type:{result.EffectType}";
                    _log.Warn($"[Collectibles] {itemId}: {result.FailureReason} — effect deferred, discovery not registered.");
                    return result;
            }

            if (!applied)
            {
                _log.Warn($"[Collectibles] {itemId}: {result.FailureReason} — effect deferred, discovery not registered.");
                return result;
            }

            result.EffectApplied = true;
            result.DiscoveryRegistered = _discovery.MarkDiscovered(itemId);
            return result;
        }

        private bool ApplyMorale(CollectibleDefinition def, CollectibleDispatchResult result)
        {
            NeedsSystem? needs = _needsProvider?.Invoke();
            if (needs == null)
            {
                result.FailureReason = "morale_authority_unavailable";
                return false;
            }

            float value = Math.Clamp(def.effect_value, 0f, MaxMoraleEffectValue);
            IReadOnlyList<SurvivorNeedsState> roster = needs.Registered;
            int touched = 0;
            for (int i = 0; i < roster.Count; i++)
            {
                // Modify() skips dead survivors itself; call uniformly so the
                // effect is a single bounded grant to every living member.
                needs.Modify(roster[i], NeedKind.Morale, value);
                touched++;
            }

            _log.Info($"[Collectibles] {def.item_id}: morale +{value.ToString(System.Globalization.CultureInfo.InvariantCulture)} to {touched} survivor(s).");
            return true;
        }

        private bool ApplyKnowledge(CollectibleDefinition def, CollectibleDispatchResult result)
        {
            ResearchSystem? research = _researchProvider?.Invoke();
            if (research == null)
            {
                result.FailureReason = "research_authority_unavailable";
                return false;
            }
            if (!ValidateTarget(def, result)) return false;

            // UnlockManual is deliberately free-form: the reveal is recorded in
            // campaign research state even when the node is not (yet) in the
            // research catalog, so authored knowledge is never silently swallowed.
            research.UnlockManual(def.effect_target);
            _log.Info($"[Collectibles] {def.item_id}: knowledge '{def.effect_target}' unlocked.");
            return true;
        }

        private bool ApplyJournalUnlock(CollectibleDefinition def, CollectibleDispatchResult result)
        {
            JournalSystem? journal = _journalProvider?.Invoke();
            if (journal == null)
            {
                result.FailureReason = "journal_authority_unavailable";
                return false;
            }
            if (!ValidateTarget(def, result)) return false;

            int day = _dayProvider?.Invoke() ?? 1;
            var entry = journal.TryDiscoverKnowledge(def.effect_target, author: null, day);
            // null means the journal/codex already knows this key — the unlock
            // content exists, so the discovery still counts as handled.
            _log.Info(entry != null
                ? $"[Collectibles] {def.item_id}: journal entry + codex unlock '{def.effect_target}'."
                : $"[Collectibles] {def.item_id}: '{def.effect_target}' already known — no duplicate entry.");
            return true;
        }

        private bool ApplyLocationClue(CollectibleDefinition def, CollectibleDispatchResult result)
        {
            WastelandMapSystem? map = _mapProvider?.Invoke();
            if (map == null)
            {
                result.FailureReason = "map_authority_unavailable";
                return false;
            }
            if (!ValidateTarget(def, result)) return false;

            // Strict node resolution: a target that is not a real map node is an
            // explicit deferred failure, not a silent swallow — the clue keeps
            // its value for when the authority gains the node.
            if (!map.Discover(def.effect_target))
            {
                result.FailureReason = $"map_node_not_found:{def.effect_target}";
                return false;
            }
            _log.Info($"[Collectibles] {def.item_id}: map location '{def.effect_target}' revealed.");
            return true;
        }

        private static bool ValidateTarget(CollectibleDefinition def, CollectibleDispatchResult result)
        {
            if (string.IsNullOrEmpty(def.effect_target))
            {
                result.FailureReason = "effect_target_missing";
                return false;
            }
            return true;
        }
    }
}
