using System;
using System.Collections.Generic;

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

        public ProceduralItemInstance(string itemId, int quantity = 1,
            float condition = 1f, float contamination = 0f)
        {
            InstanceId = Guid.NewGuid().ToString("N").Substring(0, 8);
            ItemId = itemId;
            Quantity = quantity;
            ConditionPct = MathfCompat.Clamp01(condition);
            ContaminationPct = MathfCompat.Clamp01(contamination);
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

        public ItemDefinition Get(string itemId)
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
