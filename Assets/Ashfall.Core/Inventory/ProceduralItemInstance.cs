using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Procedural Item Instance — runtime wrapper around ItemDefinition that adds
    /// dynamic condition, contamination, purity/calorie variance, and expiration.
    /// Ported engine-agnostic from Unity's AtomicWar._Game.Inventory.ProceduralItemInstance.
    /// </summary>
    [Serializable]
    public class ProceduralItemInstance
    {
        public string InstanceId;
        public string ItemId;
        public float ConditionPct = 1f;
        public float ContaminationPct;
        public int Quantity = 1;
        public float CustomValueMultiplier = 1f;
        public ExpirationState Expiration = ExpirationState.Fresh;
        public float ContainerVolumeLitres;
        public float ContainerIntegrityPct = 1f;
        public float ScrapPurityGrade = 1f;
        public float FiltrationEfficiencyMicrons;
        public float CaloricValueKcal;
        public float MoldRiskPct;
        public float FoodRadiationAccumulation;
        public float ScrapWeightKg;
        public int DosesRemaining = 1;

        public ProceduralItemInstance() { }

        /// <summary>Stable per-process counter feeding the deterministic instance id.</summary>
        private static int _instanceCounter;

        public ProceduralItemInstance(string itemId, int quantity = 1,
            float condition = 1f, float contamination = 0f)
        {
            InstanceId = MakeInstanceId(itemId ?? string.Empty);
            ItemId = itemId!;
            Quantity = quantity;
            ConditionPct = MathfCompat.Clamp01(condition);
            ContaminationPct = MathfCompat.Clamp01(contamination);
        }

        /// <summary>
        /// Deterministic 8-char hex instance id (FNV-1a over itemId + counter).
        /// No Guid.NewGuid, no string.GetHashCode: both are runtime-dependent and
        /// would violate the cross-host determinism invariant.
        /// </summary>
        private static string MakeInstanceId(string itemId)
        {
            int n = System.Threading.Interlocked.Increment(ref _instanceCounter);
            ulong h = 1469598103934665603UL;
            for (int i = 0; i < itemId.Length; i++)
            {
                h ^= itemId[i];
                h *= 1099511628211UL;
            }
            h ^= (uint)n;
            h *= 1099511628211UL;
            return (h & 0xFFFFFFFFUL).ToString("x8");
        }

        public float EffectiveValueMultiplier
        {
            get
            {
                float mult = CustomValueMultiplier;
                mult *= MathfCompat.Lerp(0.3f, 1.0f, ConditionPct);
                mult *= 1f - (ContaminationPct * 0.5f);
                if (Expiration == ExpirationState.Degraded) mult *= 0.3f;
                else if (Expiration == ExpirationState.Expired) mult *= 0.6f;
                return MathfCompat.Max(0.1f, mult);
            }
        }

        public string ConditionLabel
        {
            get
            {
                if (ConditionPct > 0.90f) return "Pristine";
                if (ConditionPct > 0.70f) return "Good";
                if (ConditionPct > 0.40f) return "Worn";
                if (ConditionPct > 0.15f) return "Damaged";
                return "Ruined";
            }
        }

        public string ContaminationLabel
        {
            get
            {
                if (ContaminationPct < 0.05f) return "Clean";
                if (ContaminationPct < 0.20f) return "Trace";
                if (ContaminationPct < 0.50f) return "Contaminated";
                return "Hot";
            }
        }
    }

    /// <summary>
    /// Engine-agnostic catalog of ItemDefinitions, keyed by id. Feeds the inventory
    /// system and resolves save/load item id lookups.
    /// </summary>
    public class ItemCatalog
    {
        private readonly Dictionary<string, ItemDefinition> _byId = new Dictionary<string, ItemDefinition>(StringComparer.Ordinal);

        public int Count => _byId.Count;

        public void Register(ItemDefinition def)
        {
            if (def != null && !string.IsNullOrEmpty(def.id) && !_byId.ContainsKey(def.id))
                _byId[def.id] = def;
        }

        public ItemDefinition? Get(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            return _byId.TryGetValue(itemId, out var def) ? def : null;
        }

        public bool Contains(string itemId) => Get(itemId) != null;

        public IReadOnlyCollection<string> Ids => _byId.Keys;

        public void RegisterRange(IEnumerable<ItemDefinition> defs)
        {
            if (defs == null) return;
            foreach (var d in defs) Register(d);
        }
    }
}
