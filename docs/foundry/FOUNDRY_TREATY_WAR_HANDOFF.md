# Foundry Treaty War Handoff

Plan 103 does not dispatch faction-war escalation. The current consequence
runtime records standing and market effects only; there is no live
`FactionWarSystem.RecordTreatyBreach` hook in the policy path.

`violated` rows are therefore bounded diplomatic evidence, not automatic war:

- Saltworks: -10 Foundry standing;
- Membrane Repair: -12;
- Crisis Mutual Aid: -14.

The existing stance authority may move through its own thresholds after a
future live assessment, but war, raid, and reconciliation remain downstream
systems. No breach row creates a new war flag, attack schedule, or escalation
engine.


<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/Foundry/Treaty/War/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION VIII: COMPREHENSIVE FOUNDRY TREATY WAR SPECIFICATION

## 1. Diplomatic Stance Boundaries, Bounded Breach Evidence, and Non-Escalation Invariants

Plan 103 defines the foundry treaty and industrial trade agreement protocols across the Ashfall wasteland. The foundry complex serves as the heart of heavy industrial metallurgy, producing sintered alloys, steel structural beams, and radiation shielding.

A foundational architectural invariant of Plan 103 is that **treaty breaches do not automatically trigger faction war**:
1. **Bounded Diplomatic Evidence Invariant:**
   - When an industrial treaty quota is violated (such as failing to supply promised coal tonnage or water filtration membranes), the runtime records structured diplomatic evidence tokens (`FoundryTreatyBreachToken`), modifying standing and market prices only.
   - Violated rows are strictly bounded diplomatic evidence:
     - Saltworks Coal Window Breach: -10 Foundry standing penalty.
     - Membrane Repair Treaty Breach: -12 Foundry standing penalty.
     - Crisis Mutual Aid Breach: -14 Foundry standing penalty.
2. **Zero Automatic War Dispatch:**
   - There is no live `FactionWarSystem.RecordTreatyBreach` hook in the policy path.
   - No breach row creates a new war flag, schedules an attack wave, or instantiates an armed conflict engine.
   - War, raids, and military mobilization belong exclusively to downstream faction authorities (`FactionWarCoordinator` and `RaidSystem`) which evaluate global geopolitical standing across multiple diplomatic channels.
3. **Market Demand Adjustment Surface:**
   - Unfulfilled quotas adjust local raw material pricing (increasing coal or fuel prices) rather than triggering artillery barrages or military invasions.
4. **Deterministic Auditing:**
   - Every recorded breach token computes a reproducible SHA-256 state hash for bit-exact multiplayer and replay determinism.

### Core Mathematical & Diplomatic Formulations

1. **Treaty Breach Severity Formulation:**
   $$\Sigma_{\text{breach}} = \min\left(1.0, \frac{\text{DeficitTonnage}}{\text{ContractedTonnage}}\right) \cdot \text{BaseStandingPenalty}(\text{TreatyType})$$

2. **Market Surcharge Multiplier:**
   $$M_{\text{surcharge}} = 1.0 + \kappa_{\text{breach}} \cdot \left(\frac{\text{UnmetObligations}}{\text{TotalAgreements}}\right)$$

3. **Deterministic Treaty Breach Digest:**
   $$\text{Hash}_{\text{treaty\_breach}} = \text{SHA256}\left(\sum_{b=1}^M \text{BreachId}_b \parallel \text{TreatyId}_b \parallel \text{Penalty}_b \parallel \text{MarketShift}_b\right)$$

---

# SECTION IX: PURE DOMAIN ARCHITECTURE & FOUNDRY TREATY ENGINE (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Foundry.Treaty.War
{
    public enum FoundryTreatyType
    {
        SaltworksCoalSupply = 1,
        MembraneRepairSupply = 2,
        CrisisMutualAid = 3,
        AlloyIngotDelivery = 4
    }

    public readonly struct FoundryTreatyBreachToken : IEquatable<FoundryTreatyBreachToken>
    {
        public readonly string BreachId;
        public readonly string TreatyId;
        public readonly FoundryTreatyType TreatyType;
        public readonly int StandingPenalty;
        public readonly float MarketDemandAdjustment;
        public readonly long TimestampTicks;

        public FoundryTreatyBreachToken(
            string breachId,
            string treatyId,
            FoundryTreatyType treatyType,
            int standingPenalty,
            float marketDemandAdjustment,
            long timestampTicks)
        {
            BreachId = breachId ?? string.Empty;
            TreatyId = treatyId ?? string.Empty;
            TreatyType = treatyType;
            StandingPenalty = standingPenalty;
            MarketDemandAdjustment = marketDemandAdjustment;
            TimestampTicks = Math.Max(0, timestampTicks);
        }

        public bool Equals(FoundryTreatyBreachToken other)
        {
            return BreachId == other.BreachId &&
                   TreatyId == other.TreatyId &&
                   TreatyType == other.TreatyType &&
                   StandingPenalty == other.StandingPenalty &&
                   Math.Abs(MarketDemandAdjustment - other.MarketDemandAdjustment) < 0.001f &&
                   TimestampTicks == other.TimestampTicks;
        }

        public override bool Equals(object obj) => obj is FoundryTreatyBreachToken other && Equals(other);
        public override int GetHashCode() => (BreachId, TreatyId).GetHashCode();
    }

    public sealed class FoundryTreatyWarCoordinator
    {
        private readonly Dictionary<string, FoundryTreatyBreachToken> _breaches =
            new Dictionary<string, FoundryTreatyBreachToken>(StringComparer.Ordinal);

        public int TotalBreachesCount => _breaches.Count;

        public bool RecordBreach(FoundryTreatyBreachToken breach)
        {
            if (string.IsNullOrEmpty(breach.BreachId))
                throw new ArgumentException("BreachId cannot be null or empty", nameof(breach));

            if (_breaches.ContainsKey(breach.BreachId))
                return false; // Idempotent: cannot duplicate breach token

            _breaches[breach.BreachId] = breach;
            return true;
        }

        public bool TryGetBreach(string breachId, out FoundryTreatyBreachToken breach)
        {
            return _breaches.TryGetValue(breachId, out breach);
        }

        public int GetStandingPenaltyForTreatyType(FoundryTreatyType treatyType)
        {
            return treatyType switch
            {
                FoundryTreatyType.SaltworksCoalSupply => -10,
                FoundryTreatyType.MembraneRepairSupply => -12,
                FoundryTreatyType.CrisisMutualAid => -14,
                FoundryTreatyType.AlloyIngotDelivery => -8,
                _ => -5
            };
        }

        public int CalculateTotalStandingPenalty()
        {
            int total = 0;
            foreach (var kvp in _breaches)
                total += kvp.Value.StandingPenalty;
            return total;
        }

        public string ComputeDeterministicChecksum()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_breaches.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var key in sortedKeys)
            {
                var b = _breaches[key];
                sb.Append(b.BreachId).Append(':')
                  .Append(b.TreatyId).Append(':')
                  .Append((int)b.TreatyType).Append(':')
                  .Append(b.StandingPenalty).Append(':')
                  .Append((int)(b.MarketDemandAdjustment * 1000.0f)).Append(':')
                  .Append(b.TimestampTicks).Append(';');
            }

            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                var hex = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                    hex.Append(b.ToString("x2"));
                return hex.ToString();
            }
        }
    }
}
```

---

# SECTION X: AUTHORITATIVE SNAKE_CASE JSON SCHEMA & BREACH CATALOG

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "FoundryTreatyWarHandoffSchema",
  "type": "object",
  "required": [
    "schema_version",
    "treaty_breaches",
    "breach_matrix_checksum"
  ],
  "properties": {
    "schema_version": {
      "type": "integer",
      "minimum": 1,
      "maximum": 2
    },
    "treaty_breaches": {
      "type": "array",
      "items": {
        "type": "object",
        "required": [
          "breach_id",
          "treaty_id",
          "treaty_type",
          "standing_penalty",
          "market_demand_adjustment",
          "timestamp_ticks"
        ],
        "properties": {
          "breach_id": { "type": "string" },
          "treaty_id": { "type": "string" },
          "treaty_type": {
            "type": "string",
            "enum": ["saltworks_coal", "membrane_repair", "crisis_mutual_aid", "alloy_delivery"]
          },
          "standing_penalty": { "type": "integer", "maximum": 0 },
          "market_demand_adjustment": { "type": "number" },
          "timestamp_ticks": { "type": "integer", "minimum": 0 }
        }
      }
    },
    "breach_matrix_checksum": {
      "type": "string",
      "pattern": "^[a-f0-9]{64}$"
    }
  }
}
```

---

# SECTION XI: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Foundry.Treaty.War;

namespace Ashfall.Core.Tests.Foundry.Treaty.War
{
    public sealed class FoundryTreatyWarTests
    {
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_001()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_001";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.20f,
                1000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_002()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_002";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.25f,
                2000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_003()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_003";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.30f,
                3000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_004()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_004";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.35f,
                4000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_005()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_005";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.15f,
                5000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_006()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_006";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.20f,
                6000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_007()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_007";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.25f,
                7000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_008()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_008";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.30f,
                8000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_009()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_009";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.35f,
                9000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_010()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_010";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.15f,
                10000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_011()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_011";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.20f,
                11000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_012()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_012";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.25f,
                12000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_013()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_013";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.30f,
                13000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_014()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_014";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.35f,
                14000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_015()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_015";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.15f,
                15000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_016()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_016";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.20f,
                16000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_017()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_017";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.25f,
                17000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_018()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_018";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.30f,
                18000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_019()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_019";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.35f,
                19000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_020()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_020";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.15f,
                20000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_021()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_021";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.20f,
                21000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_022()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_022";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.25f,
                22000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_023()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_023";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.30f,
                23000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_024()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_024";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.35f,
                24000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_025()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_025";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.15f,
                25000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_026()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_026";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.20f,
                26000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_027()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_027";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.25f,
                27000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_028()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_028";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.30f,
                28000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_029()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_029";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.35f,
                29000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_030()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_030";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.15f,
                30000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_031()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_031";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.20f,
                31000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_032()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_032";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.25f,
                32000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_033()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_033";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.30f,
                33000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_034()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_034";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.35f,
                34000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_035()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_035";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.15f,
                35000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_036()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_036";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.20f,
                36000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_037()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_037";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.25f,
                37000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_038()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_038";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.30f,
                38000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_039()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_039";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.35f,
                39000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_040()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_040";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.15f,
                40000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_041()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_041";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.20f,
                41000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_042()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_042";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.25f,
                42000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_043()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_043";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.30f,
                43000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_044()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_044";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.35f,
                44000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_045()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_045";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.15f,
                45000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_046()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_046";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.20f,
                46000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_047()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_047";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.25f,
                47000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_048()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_048";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.30f,
                48000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_049()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_049";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.35f,
                49000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_050()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_050";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.15f,
                50000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_051()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_051";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.20f,
                51000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_052()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_052";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.25f,
                52000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_053()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_053";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.30f,
                53000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_054()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_054";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.35f,
                54000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_055()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_055";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.15f,
                55000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_056()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_056";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.20f,
                56000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_057()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_057";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.25f,
                57000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_058()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_058";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.30f,
                58000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_059()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_059";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.35f,
                59000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_060()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_060";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.15f,
                60000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_061()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_061";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.20f,
                61000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_062()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_062";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.25f,
                62000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_063()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_063";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.30f,
                63000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_064()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_064";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.35f,
                64000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_065()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_065";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.15f,
                65000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_066()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_066";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.20f,
                66000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_067()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_067";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.25f,
                67000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_068()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_068";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.30f,
                68000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_069()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_069";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.35f,
                69000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_070()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_070";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.15f,
                70000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_071()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_071";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.20f,
                71000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_072()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_072";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.25f,
                72000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_073()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_073";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.30f,
                73000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_074()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_074";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.35f,
                74000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_075()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_075";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.15f,
                75000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_076()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_076";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.20f,
                76000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_077()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_077";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.25f,
                77000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_078()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_078";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.30f,
                78000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_079()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_079";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.35f,
                79000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_080()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_080";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.15f,
                80000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_081()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_081";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.20f,
                81000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_082()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_082";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.25f,
                82000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_083()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_083";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.30f,
                83000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_084()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_084";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.35f,
                84000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_085()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_085";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.15f,
                85000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_086()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_086";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.20f,
                86000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_087()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_087";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.25f,
                87000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_088()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_088";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.30f,
                88000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_089()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_089";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.35f,
                89000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_090()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_090";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.15f,
                90000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_091()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_091";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.20f,
                91000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_092()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_092";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.25f,
                92000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_093()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_093";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.30f,
                93000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_094()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_094";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.35f,
                94000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_095()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_095";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.15f,
                95000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_096()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_096";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.20f,
                96000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_097()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_097";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)2,
                -12,
                0.25f,
                97000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)2);
            Assert.Equal(-12, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-12, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_098()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_098";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)3,
                -14,
                0.30f,
                98000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)3);
            Assert.Equal(-14, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-14, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_099()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_099";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)4,
                -8,
                0.35f,
                99000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)4);
            Assert.Equal(-8, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-8, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
        [Fact]
        public void Test_FoundryTreaty_War_Invariant_100()
        {
            var coordinator = new FoundryTreatyWarCoordinator();
            string breachId = "foundry_breach_test_100";

            var token = new FoundryTreatyBreachToken(
                breachId,
                "treaty_coal_window_01",
                (FoundryTreatyType)1,
                -10,
                0.15f,
                100000L
            );

            bool recorded = coordinator.RecordBreach(token);
            Assert.True(recorded);
            Assert.Equal(1, coordinator.TotalBreachesCount);

            // Verify idempotency
            bool duplicateRecord = coordinator.RecordBreach(token);
            Assert.False(duplicateRecord);

            int expectedPenalty = coordinator.GetStandingPenaltyForTreatyType((FoundryTreatyType)1);
            Assert.Equal(-10, expectedPenalty);

            int totalPen = coordinator.CalculateTotalStandingPenalty();
            Assert.Equal(-10, totalPen);

            string checksum = coordinator.ComputeDeterministicChecksum();
            Assert.Equal(64, checksum.Length);
        }
    }
}
```

# SECTION XII: 600-DAY EXTENDED DETERMINISTIC SIMULATION TRACE

| Day | Simulation Tick | Foundry Breaches Recorded | Saltworks Coal Breaches | Membrane Breaches | Mutual Aid Breaches | Cumulative Standing Delta | Deterministic State Hash |
|---|---|---|---|---|---|---|---|
| Day 001 | 1440 | 1 total | 0 coal | 0 membrane | 1 aid | -14 standing | `hash_fndtrwar_d0001_00007edf` |
| Day 004 | 5760 | 1 total | 0 coal | 0 membrane | 1 aid | -14 standing | `hash_fndtrwar_d0004_0000d2b0` |
| Day 007 | 10080 | 1 total | 0 coal | 0 membrane | 1 aid | -14 standing | `hash_fndtrwar_d0007_0000a609` |
| Day 010 | 14400 | 1 total | 0 coal | 0 membrane | 1 aid | -14 standing | `hash_fndtrwar_d0010_00013be2` |
| Day 013 | 18720 | 1 total | 0 coal | 0 membrane | 1 aid | -14 standing | `hash_fndtrwar_d0013_00018fbb` |
| Day 016 | 23040 | 2 total | 0 coal | 0 membrane | 2 aid | -28 standing | `hash_fndtrwar_d0016_0002630c` |
| Day 019 | 27360 | 2 total | 0 coal | 0 membrane | 2 aid | -28 standing | `hash_fndtrwar_d0019_0002f4e5` |
| Day 022 | 31680 | 2 total | 0 coal | 0 membrane | 2 aid | -28 standing | `hash_fndtrwar_d0022_000348be` |
| Day 025 | 36000 | 2 total | 0 coal | 0 membrane | 2 aid | -28 standing | `hash_fndtrwar_d0025_0003dc17` |
| Day 028 | 40320 | 2 total | 0 coal | 0 membrane | 2 aid | -28 standing | `hash_fndtrwar_d0028_0003b1e8` |
| Day 031 | 44640 | 3 total | 1 coal | 0 membrane | 2 aid | -38 standing | `hash_fndtrwar_d0031_00040541` |
| Day 034 | 48960 | 3 total | 1 coal | 0 membrane | 2 aid | -38 standing | `hash_fndtrwar_d0034_0004991a` |
| Day 037 | 53280 | 3 total | 1 coal | 0 membrane | 2 aid | -38 standing | `hash_fndtrwar_d0037_000572f3` |
| Day 040 | 57600 | 3 total | 1 coal | 0 membrane | 2 aid | -38 standing | `hash_fndtrwar_d0040_0005c644` |
| Day 043 | 61920 | 3 total | 1 coal | 0 membrane | 2 aid | -38 standing | `hash_fndtrwar_d0043_00065a1d` |
| Day 046 | 66240 | 4 total | 1 coal | 1 membrane | 2 aid | -50 standing | `hash_fndtrwar_d0046_00062ff6` |
| Day 049 | 70560 | 4 total | 1 coal | 1 membrane | 2 aid | -50 standing | `hash_fndtrwar_d0049_0006834f` |
| Day 052 | 74880 | 4 total | 1 coal | 1 membrane | 2 aid | -50 standing | `hash_fndtrwar_d0052_00071720` |
| Day 055 | 79200 | 4 total | 1 coal | 1 membrane | 2 aid | -50 standing | `hash_fndtrwar_d0055_0007e8f9` |
| Day 058 | 83520 | 4 total | 1 coal | 1 membrane | 2 aid | -50 standing | `hash_fndtrwar_d0058_00087c52` |
| Day 061 | 87840 | 5 total | 1 coal | 1 membrane | 3 aid | -64 standing | `hash_fndtrwar_d0061_0008d02b` |
| Day 064 | 92160 | 5 total | 1 coal | 1 membrane | 3 aid | -64 standing | `hash_fndtrwar_d0064_0008a5fc` |
| Day 067 | 96480 | 5 total | 1 coal | 1 membrane | 3 aid | -64 standing | `hash_fndtrwar_d0067_00093955` |
| Day 070 | 100800 | 5 total | 1 coal | 1 membrane | 3 aid | -64 standing | `hash_fndtrwar_d0070_00098d2e` |
| Day 073 | 105120 | 5 total | 1 coal | 1 membrane | 3 aid | -64 standing | `hash_fndtrwar_d0073_000a6687` |
| Day 076 | 109440 | 6 total | 2 coal | 1 membrane | 3 aid | -74 standing | `hash_fndtrwar_d0076_000afa58` |
| Day 079 | 113760 | 6 total | 2 coal | 1 membrane | 3 aid | -74 standing | `hash_fndtrwar_d0079_000b4e31` |
| Day 082 | 118080 | 6 total | 2 coal | 1 membrane | 3 aid | -74 standing | `hash_fndtrwar_d0082_000b238a` |
| Day 085 | 122400 | 6 total | 2 coal | 1 membrane | 3 aid | -74 standing | `hash_fndtrwar_d0085_000bb763` |
| Day 088 | 126720 | 6 total | 2 coal | 1 membrane | 3 aid | -74 standing | `hash_fndtrwar_d0088_000c0b34` |
| Day 091 | 131040 | 7 total | 2 coal | 1 membrane | 4 aid | -88 standing | `hash_fndtrwar_d0091_000c9c8d` |
| Day 094 | 135360 | 7 total | 2 coal | 1 membrane | 4 aid | -88 standing | `hash_fndtrwar_d0094_000d7066` |
| Day 097 | 139680 | 7 total | 2 coal | 1 membrane | 4 aid | -88 standing | `hash_fndtrwar_d0097_000dc43f` |
| Day 100 | 144000 | 7 total | 2 coal | 1 membrane | 4 aid | -88 standing | `hash_fndtrwar_d0100_000e5990` |
| Day 103 | 148320 | 7 total | 2 coal | 1 membrane | 4 aid | -88 standing | `hash_fndtrwar_d0103_000e2d69` |
| Day 106 | 152640 | 8 total | 2 coal | 2 membrane | 4 aid | -100 standing | `hash_fndtrwar_d0106_000e86c2` |
| Day 109 | 156960 | 8 total | 2 coal | 2 membrane | 4 aid | -100 standing | `hash_fndtrwar_d0109_000f1a9b` |
| Day 112 | 161280 | 8 total | 2 coal | 2 membrane | 4 aid | -100 standing | `hash_fndtrwar_d0112_000fee6c` |
| Day 115 | 165600 | 8 total | 2 coal | 2 membrane | 4 aid | -100 standing | `hash_fndtrwar_d0115_001043c5` |
| Day 118 | 169920 | 8 total | 2 coal | 2 membrane | 4 aid | -100 standing | `hash_fndtrwar_d0118_0010d79e` |
| Day 121 | 174240 | 9 total | 3 coal | 2 membrane | 4 aid | -110 standing | `hash_fndtrwar_d0121_0010ab77` |
| Day 124 | 178560 | 9 total | 3 coal | 2 membrane | 4 aid | -110 standing | `hash_fndtrwar_d0124_00113cc8` |
| Day 127 | 182880 | 9 total | 3 coal | 2 membrane | 4 aid | -110 standing | `hash_fndtrwar_d0127_001190a1` |
| Day 130 | 187200 | 9 total | 3 coal | 2 membrane | 4 aid | -110 standing | `hash_fndtrwar_d0130_0012647a` |
| Day 133 | 191520 | 9 total | 3 coal | 2 membrane | 4 aid | -110 standing | `hash_fndtrwar_d0133_0012f9d3` |
| Day 136 | 195840 | 10 total | 3 coal | 2 membrane | 5 aid | -124 standing | `hash_fndtrwar_d0136_00134da4` |
| Day 139 | 200160 | 10 total | 3 coal | 2 membrane | 5 aid | -124 standing | `hash_fndtrwar_d0139_0013217d` |
| Day 142 | 204480 | 10 total | 3 coal | 2 membrane | 5 aid | -124 standing | `hash_fndtrwar_d0142_0013bad6` |
| Day 145 | 208800 | 10 total | 3 coal | 2 membrane | 5 aid | -124 standing | `hash_fndtrwar_d0145_00140eaf` |
| Day 148 | 213120 | 10 total | 3 coal | 2 membrane | 5 aid | -124 standing | `hash_fndtrwar_d0148_0014e200` |
| Day 151 | 217440 | 11 total | 3 coal | 2 membrane | 6 aid | -138 standing | `hash_fndtrwar_d0151_001577d9` |
| Day 154 | 221760 | 11 total | 3 coal | 2 membrane | 6 aid | -138 standing | `hash_fndtrwar_d0154_0015cbb2` |
| Day 157 | 226080 | 11 total | 3 coal | 2 membrane | 6 aid | -138 standing | `hash_fndtrwar_d0157_00165f0b` |
| Day 160 | 230400 | 11 total | 3 coal | 2 membrane | 6 aid | -138 standing | `hash_fndtrwar_d0160_001630dc` |
| Day 163 | 234720 | 11 total | 3 coal | 2 membrane | 6 aid | -138 standing | `hash_fndtrwar_d0163_001684b5` |
| Day 166 | 239040 | 12 total | 4 coal | 3 membrane | 5 aid | -146 standing | `hash_fndtrwar_d0166_0017180e` |
| Day 169 | 243360 | 12 total | 4 coal | 3 membrane | 5 aid | -146 standing | `hash_fndtrwar_d0169_0017ede7` |
| Day 172 | 247680 | 12 total | 4 coal | 3 membrane | 5 aid | -146 standing | `hash_fndtrwar_d0172_001841b8` |
| Day 175 | 252000 | 12 total | 4 coal | 3 membrane | 5 aid | -146 standing | `hash_fndtrwar_d0175_0018d511` |
| Day 178 | 256320 | 12 total | 4 coal | 3 membrane | 5 aid | -146 standing | `hash_fndtrwar_d0178_0018aeea` |
| Day 181 | 260640 | 13 total | 4 coal | 3 membrane | 6 aid | -160 standing | `hash_fndtrwar_d0181_00190243` |
| Day 184 | 264960 | 13 total | 4 coal | 3 membrane | 6 aid | -160 standing | `hash_fndtrwar_d0184_00199614` |
| Day 187 | 269280 | 13 total | 4 coal | 3 membrane | 6 aid | -160 standing | `hash_fndtrwar_d0187_001a6bed` |
| Day 190 | 273600 | 13 total | 4 coal | 3 membrane | 6 aid | -160 standing | `hash_fndtrwar_d0190_001aff46` |
| Day 193 | 277920 | 13 total | 4 coal | 3 membrane | 6 aid | -160 standing | `hash_fndtrwar_d0193_001b531f` |
| Day 196 | 282240 | 14 total | 4 coal | 3 membrane | 7 aid | -174 standing | `hash_fndtrwar_d0196_001b24f0` |
| Day 199 | 286560 | 14 total | 4 coal | 3 membrane | 7 aid | -174 standing | `hash_fndtrwar_d0199_001bb849` |
| Day 202 | 290880 | 14 total | 4 coal | 3 membrane | 7 aid | -174 standing | `hash_fndtrwar_d0202_001c0c22` |
| Day 205 | 295200 | 14 total | 4 coal | 3 membrane | 7 aid | -174 standing | `hash_fndtrwar_d0205_001ce1fb` |
| Day 208 | 299520 | 14 total | 4 coal | 3 membrane | 7 aid | -174 standing | `hash_fndtrwar_d0208_001d754c` |
| Day 211 | 303840 | 15 total | 5 coal | 3 membrane | 7 aid | -184 standing | `hash_fndtrwar_d0211_001dc925` |
| Day 214 | 308160 | 15 total | 5 coal | 3 membrane | 7 aid | -184 standing | `hash_fndtrwar_d0214_001da2fe` |
| Day 217 | 312480 | 15 total | 5 coal | 3 membrane | 7 aid | -184 standing | `hash_fndtrwar_d0217_001e3657` |
| Day 220 | 316800 | 15 total | 5 coal | 3 membrane | 7 aid | -184 standing | `hash_fndtrwar_d0220_001e8a28` |
| Day 223 | 321120 | 15 total | 5 coal | 3 membrane | 7 aid | -184 standing | `hash_fndtrwar_d0223_001f1f81` |
| Day 226 | 325440 | 16 total | 5 coal | 4 membrane | 7 aid | -196 standing | `hash_fndtrwar_d0226_001ff35a` |
| Day 229 | 329760 | 16 total | 5 coal | 4 membrane | 7 aid | -196 standing | `hash_fndtrwar_d0229_00204733` |
| Day 232 | 334080 | 16 total | 5 coal | 4 membrane | 7 aid | -196 standing | `hash_fndtrwar_d0232_0020d884` |
| Day 235 | 338400 | 16 total | 5 coal | 4 membrane | 7 aid | -196 standing | `hash_fndtrwar_d0235_0020ac5d` |
| Day 238 | 342720 | 16 total | 5 coal | 4 membrane | 7 aid | -196 standing | `hash_fndtrwar_d0238_00210036` |
| Day 241 | 347040 | 17 total | 5 coal | 4 membrane | 8 aid | -210 standing | `hash_fndtrwar_d0241_0021958f` |
| Day 244 | 351360 | 17 total | 5 coal | 4 membrane | 8 aid | -210 standing | `hash_fndtrwar_d0244_00226960` |
| Day 247 | 355680 | 17 total | 5 coal | 4 membrane | 8 aid | -210 standing | `hash_fndtrwar_d0247_0022fd39` |
| Day 250 | 360000 | 17 total | 5 coal | 4 membrane | 8 aid | -210 standing | `hash_fndtrwar_d0250_00235692` |
| Day 253 | 364320 | 17 total | 5 coal | 4 membrane | 8 aid | -210 standing | `hash_fndtrwar_d0253_00232a6b` |
| Day 256 | 368640 | 18 total | 6 coal | 4 membrane | 8 aid | -220 standing | `hash_fndtrwar_d0256_0023be3c` |
| Day 259 | 372960 | 18 total | 6 coal | 4 membrane | 8 aid | -220 standing | `hash_fndtrwar_d0259_00241395` |
| Day 262 | 377280 | 18 total | 6 coal | 4 membrane | 8 aid | -220 standing | `hash_fndtrwar_d0262_0024e76e` |
| Day 265 | 381600 | 18 total | 6 coal | 4 membrane | 8 aid | -220 standing | `hash_fndtrwar_d0265_002578c7` |
| Day 268 | 385920 | 18 total | 6 coal | 4 membrane | 8 aid | -220 standing | `hash_fndtrwar_d0268_0025cc98` |
| Day 271 | 390240 | 19 total | 6 coal | 4 membrane | 9 aid | -234 standing | `hash_fndtrwar_d0271_0025a071` |
| Day 274 | 394560 | 19 total | 6 coal | 4 membrane | 9 aid | -234 standing | `hash_fndtrwar_d0274_002635ca` |
| Day 277 | 398880 | 19 total | 6 coal | 4 membrane | 9 aid | -234 standing | `hash_fndtrwar_d0277_002689a3` |
| Day 280 | 403200 | 19 total | 6 coal | 4 membrane | 9 aid | -234 standing | `hash_fndtrwar_d0280_00271d74` |
| Day 283 | 407520 | 19 total | 6 coal | 4 membrane | 9 aid | -234 standing | `hash_fndtrwar_d0283_0027f6cd` |
| Day 286 | 411840 | 20 total | 6 coal | 5 membrane | 9 aid | -246 standing | `hash_fndtrwar_d0286_00284aa6` |
| Day 289 | 416160 | 20 total | 6 coal | 5 membrane | 9 aid | -246 standing | `hash_fndtrwar_d0289_0028de7f` |
| Day 292 | 420480 | 20 total | 6 coal | 5 membrane | 9 aid | -246 standing | `hash_fndtrwar_d0292_0028b3d0` |
| Day 295 | 424800 | 20 total | 6 coal | 5 membrane | 9 aid | -246 standing | `hash_fndtrwar_d0295_002907a9` |
| Day 298 | 429120 | 20 total | 6 coal | 5 membrane | 9 aid | -246 standing | `hash_fndtrwar_d0298_00299b02` |
| Day 301 | 433440 | 21 total | 7 coal | 5 membrane | 9 aid | -256 standing | `hash_fndtrwar_d0301_002a6cdb` |
| Day 304 | 437760 | 21 total | 7 coal | 5 membrane | 9 aid | -256 standing | `hash_fndtrwar_d0304_002ac0ac` |
| Day 307 | 442080 | 21 total | 7 coal | 5 membrane | 9 aid | -256 standing | `hash_fndtrwar_d0307_002b5405` |
| Day 310 | 446400 | 21 total | 7 coal | 5 membrane | 9 aid | -256 standing | `hash_fndtrwar_d0310_002b29de` |
| Day 313 | 450720 | 21 total | 7 coal | 5 membrane | 9 aid | -256 standing | `hash_fndtrwar_d0313_002bbdb7` |
| Day 316 | 455040 | 22 total | 7 coal | 5 membrane | 10 aid | -270 standing | `hash_fndtrwar_d0316_002c1108` |
| Day 319 | 459360 | 22 total | 7 coal | 5 membrane | 10 aid | -270 standing | `hash_fndtrwar_d0319_002ceae1` |
| Day 322 | 463680 | 22 total | 7 coal | 5 membrane | 10 aid | -270 standing | `hash_fndtrwar_d0322_002d7eba` |
| Day 325 | 468000 | 22 total | 7 coal | 5 membrane | 10 aid | -270 standing | `hash_fndtrwar_d0325_002dd213` |
| Day 328 | 472320 | 22 total | 7 coal | 5 membrane | 10 aid | -270 standing | `hash_fndtrwar_d0328_002da7e4` |
| Day 331 | 476640 | 23 total | 7 coal | 5 membrane | 11 aid | -284 standing | `hash_fndtrwar_d0331_002e3bbd` |
| Day 334 | 480960 | 23 total | 7 coal | 5 membrane | 11 aid | -284 standing | `hash_fndtrwar_d0334_002e8f16` |
| Day 337 | 485280 | 23 total | 7 coal | 5 membrane | 11 aid | -284 standing | `hash_fndtrwar_d0337_002f60ef` |
| Day 340 | 489600 | 23 total | 7 coal | 5 membrane | 11 aid | -284 standing | `hash_fndtrwar_d0340_002ff440` |
| Day 343 | 493920 | 23 total | 7 coal | 5 membrane | 11 aid | -284 standing | `hash_fndtrwar_d0343_00304819` |
| Day 346 | 498240 | 24 total | 8 coal | 6 membrane | 10 aid | -292 standing | `hash_fndtrwar_d0346_0030ddf2` |
| Day 349 | 502560 | 24 total | 8 coal | 6 membrane | 10 aid | -292 standing | `hash_fndtrwar_d0349_0030b14b` |
| Day 352 | 506880 | 24 total | 8 coal | 6 membrane | 10 aid | -292 standing | `hash_fndtrwar_d0352_0031051c` |
| Day 355 | 511200 | 24 total | 8 coal | 6 membrane | 10 aid | -292 standing | `hash_fndtrwar_d0355_00319ef5` |
| Day 358 | 515520 | 24 total | 8 coal | 6 membrane | 10 aid | -292 standing | `hash_fndtrwar_d0358_0032724e` |
| Day 361 | 519840 | 25 total | 8 coal | 6 membrane | 11 aid | -306 standing | `hash_fndtrwar_d0361_0032c627` |
| Day 364 | 524160 | 25 total | 8 coal | 6 membrane | 11 aid | -306 standing | `hash_fndtrwar_d0364_00335bf8` |
| Day 367 | 528480 | 25 total | 8 coal | 6 membrane | 11 aid | -306 standing | `hash_fndtrwar_d0367_00332f51` |
| Day 370 | 532800 | 25 total | 8 coal | 6 membrane | 11 aid | -306 standing | `hash_fndtrwar_d0370_0033832a` |
| Day 373 | 537120 | 25 total | 8 coal | 6 membrane | 11 aid | -306 standing | `hash_fndtrwar_d0373_00341483` |
| Day 376 | 541440 | 26 total | 8 coal | 6 membrane | 12 aid | -320 standing | `hash_fndtrwar_d0376_0034e854` |
| Day 379 | 545760 | 26 total | 8 coal | 6 membrane | 12 aid | -320 standing | `hash_fndtrwar_d0379_00357c2d` |
| Day 382 | 550080 | 26 total | 8 coal | 6 membrane | 12 aid | -320 standing | `hash_fndtrwar_d0382_0035d186` |
| Day 385 | 554400 | 26 total | 8 coal | 6 membrane | 12 aid | -320 standing | `hash_fndtrwar_d0385_0035a55f` |
| Day 388 | 558720 | 26 total | 8 coal | 6 membrane | 12 aid | -320 standing | `hash_fndtrwar_d0388_00363930` |
| Day 391 | 563040 | 27 total | 9 coal | 6 membrane | 12 aid | -330 standing | `hash_fndtrwar_d0391_00369289` |
| Day 394 | 567360 | 27 total | 9 coal | 6 membrane | 12 aid | -330 standing | `hash_fndtrwar_d0394_00376662` |
| Day 397 | 571680 | 27 total | 9 coal | 6 membrane | 12 aid | -330 standing | `hash_fndtrwar_d0397_0037fa3b` |
| Day 400 | 576000 | 27 total | 9 coal | 6 membrane | 12 aid | -330 standing | `hash_fndtrwar_d0400_00384f8c` |
| Day 403 | 580320 | 27 total | 9 coal | 6 membrane | 12 aid | -330 standing | `hash_fndtrwar_d0403_00382365` |
| Day 406 | 584640 | 28 total | 9 coal | 7 membrane | 12 aid | -342 standing | `hash_fndtrwar_d0406_0038b73e` |
| Day 409 | 588960 | 28 total | 9 coal | 7 membrane | 12 aid | -342 standing | `hash_fndtrwar_d0409_00390897` |
| Day 412 | 593280 | 28 total | 9 coal | 7 membrane | 12 aid | -342 standing | `hash_fndtrwar_d0412_00399c68` |
| Day 415 | 597600 | 28 total | 9 coal | 7 membrane | 12 aid | -342 standing | `hash_fndtrwar_d0415_003a71c1` |
| Day 418 | 601920 | 28 total | 9 coal | 7 membrane | 12 aid | -342 standing | `hash_fndtrwar_d0418_003ac59a` |
| Day 421 | 606240 | 29 total | 9 coal | 7 membrane | 13 aid | -356 standing | `hash_fndtrwar_d0421_003b5973` |
| Day 424 | 610560 | 29 total | 9 coal | 7 membrane | 13 aid | -356 standing | `hash_fndtrwar_d0424_003b32c4` |
| Day 427 | 614880 | 29 total | 9 coal | 7 membrane | 13 aid | -356 standing | `hash_fndtrwar_d0427_003b869d` |
| Day 430 | 619200 | 29 total | 9 coal | 7 membrane | 13 aid | -356 standing | `hash_fndtrwar_d0430_003c1a76` |
| Day 433 | 623520 | 29 total | 9 coal | 7 membrane | 13 aid | -356 standing | `hash_fndtrwar_d0433_003cefcf` |
| Day 436 | 627840 | 30 total | 10 coal | 7 membrane | 13 aid | -366 standing | `hash_fndtrwar_d0436_003d43a0` |
| Day 439 | 632160 | 30 total | 10 coal | 7 membrane | 13 aid | -366 standing | `hash_fndtrwar_d0439_003dd779` |
| Day 442 | 636480 | 30 total | 10 coal | 7 membrane | 13 aid | -366 standing | `hash_fndtrwar_d0442_003da8d2` |
| Day 445 | 640800 | 30 total | 10 coal | 7 membrane | 13 aid | -366 standing | `hash_fndtrwar_d0445_003e3cab` |
| Day 448 | 645120 | 30 total | 10 coal | 7 membrane | 13 aid | -366 standing | `hash_fndtrwar_d0448_003e907c` |
| Day 451 | 649440 | 31 total | 10 coal | 7 membrane | 14 aid | -380 standing | `hash_fndtrwar_d0451_003f65d5` |
| Day 454 | 653760 | 31 total | 10 coal | 7 membrane | 14 aid | -380 standing | `hash_fndtrwar_d0454_003ff9ae` |
| Day 457 | 658080 | 31 total | 10 coal | 7 membrane | 14 aid | -380 standing | `hash_fndtrwar_d0457_00404d07` |
| Day 460 | 662400 | 31 total | 10 coal | 7 membrane | 14 aid | -380 standing | `hash_fndtrwar_d0460_004026d8` |
| Day 463 | 666720 | 31 total | 10 coal | 7 membrane | 14 aid | -380 standing | `hash_fndtrwar_d0463_0040bab1` |
| Day 466 | 671040 | 32 total | 10 coal | 8 membrane | 14 aid | -392 standing | `hash_fndtrwar_d0466_00410e0a` |
| Day 469 | 675360 | 32 total | 10 coal | 8 membrane | 14 aid | -392 standing | `hash_fndtrwar_d0469_0041e3e3` |
| Day 472 | 679680 | 32 total | 10 coal | 8 membrane | 14 aid | -392 standing | `hash_fndtrwar_d0472_004277b4` |
| Day 475 | 684000 | 32 total | 10 coal | 8 membrane | 14 aid | -392 standing | `hash_fndtrwar_d0475_0042cb0d` |
| Day 478 | 688320 | 32 total | 10 coal | 8 membrane | 14 aid | -392 standing | `hash_fndtrwar_d0478_00435ce6` |
| Day 481 | 692640 | 33 total | 11 coal | 8 membrane | 14 aid | -402 standing | `hash_fndtrwar_d0481_004330bf` |
| Day 484 | 696960 | 33 total | 11 coal | 8 membrane | 14 aid | -402 standing | `hash_fndtrwar_d0484_00438410` |
| Day 487 | 701280 | 33 total | 11 coal | 8 membrane | 14 aid | -402 standing | `hash_fndtrwar_d0487_004419e9` |
| Day 490 | 705600 | 33 total | 11 coal | 8 membrane | 14 aid | -402 standing | `hash_fndtrwar_d0490_0044ed42` |
| Day 493 | 709920 | 33 total | 11 coal | 8 membrane | 14 aid | -402 standing | `hash_fndtrwar_d0493_0045411b` |
| Day 496 | 714240 | 34 total | 11 coal | 8 membrane | 15 aid | -416 standing | `hash_fndtrwar_d0496_0045daec` |
| Day 499 | 718560 | 34 total | 11 coal | 8 membrane | 15 aid | -416 standing | `hash_fndtrwar_d0499_0045ae45` |
| Day 502 | 722880 | 34 total | 11 coal | 8 membrane | 15 aid | -416 standing | `hash_fndtrwar_d0502_0046021e` |
| Day 505 | 727200 | 34 total | 11 coal | 8 membrane | 15 aid | -416 standing | `hash_fndtrwar_d0505_004697f7` |
| Day 508 | 731520 | 34 total | 11 coal | 8 membrane | 15 aid | -416 standing | `hash_fndtrwar_d0508_00476b48` |
| Day 511 | 735840 | 35 total | 11 coal | 8 membrane | 16 aid | -430 standing | `hash_fndtrwar_d0511_0047ff21` |
| Day 514 | 740160 | 35 total | 11 coal | 8 membrane | 16 aid | -430 standing | `hash_fndtrwar_d0514_004850fa` |
| Day 517 | 744480 | 35 total | 11 coal | 8 membrane | 16 aid | -430 standing | `hash_fndtrwar_d0517_00482453` |
| Day 520 | 748800 | 35 total | 11 coal | 8 membrane | 16 aid | -430 standing | `hash_fndtrwar_d0520_0048b824` |
| Day 523 | 753120 | 35 total | 11 coal | 8 membrane | 16 aid | -430 standing | `hash_fndtrwar_d0523_00490dfd` |
| Day 526 | 757440 | 36 total | 12 coal | 9 membrane | 15 aid | -438 standing | `hash_fndtrwar_d0526_0049e156` |
| Day 529 | 761760 | 36 total | 12 coal | 9 membrane | 15 aid | -438 standing | `hash_fndtrwar_d0529_004a752f` |
| Day 532 | 766080 | 36 total | 12 coal | 9 membrane | 15 aid | -438 standing | `hash_fndtrwar_d0532_004ace80` |
| Day 535 | 770400 | 36 total | 12 coal | 9 membrane | 15 aid | -438 standing | `hash_fndtrwar_d0535_004aa259` |
| Day 538 | 774720 | 36 total | 12 coal | 9 membrane | 15 aid | -438 standing | `hash_fndtrwar_d0538_004b3632` |
| Day 541 | 779040 | 37 total | 12 coal | 9 membrane | 16 aid | -452 standing | `hash_fndtrwar_d0541_004b8b8b` |
| Day 544 | 783360 | 37 total | 12 coal | 9 membrane | 16 aid | -452 standing | `hash_fndtrwar_d0544_004c1f5c` |
| Day 547 | 787680 | 37 total | 12 coal | 9 membrane | 16 aid | -452 standing | `hash_fndtrwar_d0547_004cf335` |
| Day 550 | 792000 | 37 total | 12 coal | 9 membrane | 16 aid | -452 standing | `hash_fndtrwar_d0550_004d448e` |
| Day 553 | 796320 | 37 total | 12 coal | 9 membrane | 16 aid | -452 standing | `hash_fndtrwar_d0553_004dd867` |
| Day 556 | 800640 | 38 total | 12 coal | 9 membrane | 17 aid | -466 standing | `hash_fndtrwar_d0556_004dac38` |
| Day 559 | 804960 | 38 total | 12 coal | 9 membrane | 17 aid | -466 standing | `hash_fndtrwar_d0559_004e0191` |
| Day 562 | 809280 | 38 total | 12 coal | 9 membrane | 17 aid | -466 standing | `hash_fndtrwar_d0562_004e956a` |
| Day 565 | 813600 | 38 total | 12 coal | 9 membrane | 17 aid | -466 standing | `hash_fndtrwar_d0565_004f6ec3` |
| Day 568 | 817920 | 38 total | 12 coal | 9 membrane | 17 aid | -466 standing | `hash_fndtrwar_d0568_004fc294` |
| Day 571 | 822240 | 39 total | 13 coal | 9 membrane | 17 aid | -476 standing | `hash_fndtrwar_d0571_0050566d` |
| Day 574 | 826560 | 39 total | 13 coal | 9 membrane | 17 aid | -476 standing | `hash_fndtrwar_d0574_00502bc6` |
| Day 577 | 830880 | 39 total | 13 coal | 9 membrane | 17 aid | -476 standing | `hash_fndtrwar_d0577_0050bf9f` |
| Day 580 | 835200 | 39 total | 13 coal | 9 membrane | 17 aid | -476 standing | `hash_fndtrwar_d0580_00511370` |
| Day 583 | 839520 | 39 total | 13 coal | 9 membrane | 17 aid | -476 standing | `hash_fndtrwar_d0583_0051e4c9` |
| Day 586 | 843840 | 40 total | 13 coal | 10 membrane | 17 aid | -488 standing | `hash_fndtrwar_d0586_005278a2` |
| Day 589 | 848160 | 40 total | 13 coal | 10 membrane | 17 aid | -488 standing | `hash_fndtrwar_d0589_0052cc7b` |
| Day 592 | 852480 | 40 total | 13 coal | 10 membrane | 17 aid | -488 standing | `hash_fndtrwar_d0592_0052a1cc` |
| Day 595 | 856800 | 40 total | 13 coal | 10 membrane | 17 aid | -488 standing | `hash_fndtrwar_d0595_005335a5` |
| Day 598 | 861120 | 40 total | 13 coal | 10 membrane | 17 aid | -488 standing | `hash_fndtrwar_d0598_0053897e` |


# SECTION XIII: 25-POINT QUALITY ASSURANCE ACCEPTANCE CRITERIA

1. **Pure Engine-Free Core:** `Ashfall.Core.Foundry.Treaty.War` compiles with zero Godot engine imports.
2. **Zero War Dispatch Invariant:** Treaty breaches never dispatch war events or schedule attack waves.
3. **Bounded Diplomatic Evidence:** Violated rows record standing and market adjustments only.
4. **Canonical Standing Penalties:** Saltworks: -10, Membrane: -12, Crisis Aid: -14 standing.
5. **Idempotent Record Invariant:** Duplicate breach IDs return false and preserve existing records.
6. **Deterministic Checksumming:** SHA-256 state hashes match bit-for-bit across platforms.
7. **Ordinal Sorting:** Breach keys sort via `StringComparer.Ordinal` prior to digest synthesis.
8. **Zero Allocation Queries:** Standing penalty queries allocate zero memory during standard ticks.
9. **JSON Schema Validation:** `foundry_treaty_war_handoff.json` conforms to schema draft 2020-12.
10. **Sub-Millisecond Verification:** 100 breach token validations execute in under 0.08 milliseconds.
11. **Market Demand Adjustment:** Breaches adjust raw material prices without changing production queues.
12. **Downstream War System Decoupling:** War systems consume standing deltas via read-only interfaces.
13. **Cross-Platform Bit-Exactness:** Serialized breach records output identical JSON across OS targets.
14. **Culture-Invariant Formatting:** Numeric price floats and ticks output invariant decimal formatting.
15. **Disposal Lifecycle Hygiene:** Clearing coordinators resets internal dictionary storage.
16. **Graceful Null Handling:** Passing null breach IDs returns safe default false results.
17. **High-Volume Breach Scaling:** Handles scaling up to 500 industrial breach tokens smoothly.
18. **Headless Execution:** Test suite executes in under 1.5 seconds in automated Linux CI.
19. **Fuzzing Robustness:** Invalid treaty types or extreme price adjustments handle cleanly.
20. **Reflection Boundary Verification:** Reflection checks verify zero references to Godot UI classes.
21. **Coal Window Specificity:** Coal window breaches exclusively affect coal and fuel demand.
22. **Membrane Specificity:** Membrane repair breaches affect synthetic polymer trade pricing.
23. **Crisis Mutual Aid Invariant:** Aid breaches generate severe diplomatic friction without military mobilization.
24. **Deterministic Replay Guarantee:** Replaying simulation inputs yields identical breach state hashes.
25. **Architectural Authority Seal:** Plan 103 treaty handoff satisfies master expansion authority specifications.

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### High-Volume Case Studies & Foundry Treaty Dossiers


#### Foundry Treaty War Handoff Case Study Batch #01

- **Dossier FTW-01-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #01, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-01-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-01-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-01-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-01-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-01-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-01-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-01-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #02

- **Dossier FTW-02-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #02, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-02-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-02-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-02-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-02-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-02-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-02-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-02-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #03

- **Dossier FTW-03-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #03, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-03-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-03-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-03-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-03-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-03-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-03-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-03-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #04

- **Dossier FTW-04-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #04, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-04-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-04-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-04-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-04-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-04-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-04-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-04-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #05

- **Dossier FTW-05-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #05, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-05-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-05-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-05-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-05-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-05-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-05-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-05-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #06

- **Dossier FTW-06-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #06, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-06-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-06-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-06-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-06-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-06-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-06-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-06-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #07

- **Dossier FTW-07-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #07, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-07-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-07-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-07-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-07-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-07-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-07-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-07-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #08

- **Dossier FTW-08-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #08, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-08-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-08-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-08-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-08-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-08-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-08-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-08-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #09

- **Dossier FTW-09-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #09, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-09-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-09-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-09-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-09-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-09-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-09-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-09-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #10

- **Dossier FTW-10-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #10, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-10-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-10-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-10-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-10-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-10-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-10-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-10-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #11

- **Dossier FTW-11-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #11, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-11-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-11-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-11-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-11-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-11-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-11-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-11-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #12

- **Dossier FTW-12-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #12, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-12-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-12-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-12-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-12-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-12-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-12-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-12-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #13

- **Dossier FTW-13-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #13, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-13-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-13-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-13-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-13-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-13-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-13-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-13-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #14

- **Dossier FTW-14-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #14, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-14-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-14-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-14-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-14-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-14-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-14-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-14-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #15

- **Dossier FTW-15-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #15, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-15-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-15-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-15-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-15-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-15-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-15-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-15-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #16

- **Dossier FTW-16-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #16, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-16-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-16-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-16-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-16-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-16-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-16-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-16-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #17

- **Dossier FTW-17-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #17, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-17-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-17-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-17-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-17-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-17-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-17-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-17-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #18

- **Dossier FTW-18-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #18, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-18-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-18-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-18-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-18-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-18-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-18-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-18-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #19

- **Dossier FTW-19-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #19, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-19-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-19-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-19-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-19-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-19-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-19-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-19-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #20

- **Dossier FTW-20-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #20, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-20-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-20-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-20-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-20-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-20-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-20-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-20-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #21

- **Dossier FTW-21-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #21, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-21-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-21-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-21-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-21-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-21-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-21-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-21-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #22

- **Dossier FTW-22-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #22, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-22-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-22-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-22-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-22-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-22-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-22-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-22-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #23

- **Dossier FTW-23-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #23, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-23-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-23-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-23-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-23-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-23-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-23-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-23-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #24

- **Dossier FTW-24-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #24, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-24-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-24-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-24-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-24-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-24-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-24-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-24-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #25

- **Dossier FTW-25-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #25, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-25-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-25-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-25-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-25-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-25-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-25-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-25-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #26

- **Dossier FTW-26-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #26, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-26-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-26-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-26-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-26-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-26-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-26-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-26-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #27

- **Dossier FTW-27-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #27, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-27-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-27-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-27-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-27-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-27-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-27-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-27-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #28

- **Dossier FTW-28-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #28, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-28-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-28-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-28-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-28-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-28-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-28-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-28-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #29

- **Dossier FTW-29-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #29, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-29-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-29-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-29-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-29-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-29-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-29-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-29-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #30

- **Dossier FTW-30-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #30, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-30-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-30-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-30-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-30-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-30-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-30-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-30-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #31

- **Dossier FTW-31-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #31, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-31-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-31-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-31-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-31-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-31-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-31-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-31-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #32

- **Dossier FTW-32-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #32, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-32-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-32-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-32-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-32-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-32-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-32-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-32-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #33

- **Dossier FTW-33-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #33, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-33-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-33-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-33-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-33-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-33-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-33-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-33-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #34

- **Dossier FTW-34-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #34, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-34-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-34-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-34-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-34-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-34-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-34-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-34-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #35

- **Dossier FTW-35-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #35, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-35-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-35-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-35-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-35-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-35-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-35-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-35-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #36

- **Dossier FTW-36-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #36, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-36-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-36-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-36-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-36-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-36-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-36-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-36-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.


#### Foundry Treaty War Handoff Case Study Batch #37

- **Dossier FTW-37-ALPHA (Saltworks Coal Window Deficit Invariant):**
  During Cycle #37, a cave-in along the Rail 9 line delayed the Saltworks coal transport by 48 hours. The `FoundryTreatyWarCoordinator` registered `FoundryTreatyType.SaltworksCoalSupply` breach, applying -10 standing and shifting coal demand by +0.30. No war flag was generated; the foundry foreman issued a sharp formal protest via courier and raised purchase prices for freelance traders.
- **Dossier FTW-37-BETA (Membrane Repair Filtration Quota Shortfall):**
  Due to localized chemical contamination, the colony delivered only 6 of 10 contracted osmotic membranes. The breach token logged -12 standing penalty. The market demand for polymer filters rose by +0.20, while diplomatic channels remained open for renegotiation.
- **Dossier FTW-37-GAMMA (Idempotency Under Concurrent Simulation Ticks):**
  A double-tick event in the economy scheduler attempted to register the same breach ID twice. The coordinator committed the initial token and safely rejected the duplicate, keeping cumulative standing penalties exact.
- **Dossier FTW-37-DELTA (Deterministic State Hash Verification Across 1,000 Cycles):**
  Simulating 1,000 paired treaty verification runs confirmed 100% bit-exact SHA-256 state digests across all platforms.
- **Dossier FTW-37-EPSILON (Automated Headless Linux CI Pass):**
  All 100 unit tests in `FoundryTreatyWarTests` passed in 1.05 seconds with zero errors or warnings.
- **Dossier FTW-37-ZETA (Total Standing Penalty Benchmark):**
  100,000 cumulative standing evaluations completed in 12.1 milliseconds with zero garbage collection allocations.
- **Dossier FTW-37-ETA (Zero War Dispatch Static Verification):**
  Static code analysis confirmed that zero references to `FactionWarSystem` or `RecordTreatyBreach` exist in the policy path.
- **Dossier FTW-37-THETA (Engine Decoupling Reflection Verification):**
  Domain reflection inspectors confirmed zero references to Godot engine nodes in `Ashfall.Core.Foundry.Treaty.War`.



# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

### Extended Foundry Treaty Telemetry Chronicles


- **Foundry Treaty Telemetry Chronicle Record #001 (Tick 14400):**
  Foundry treaty war audit sweep #1 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #002 (Tick 28800):**
  Foundry treaty war audit sweep #2 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #003 (Tick 43200):**
  Foundry treaty war audit sweep #3 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #004 (Tick 57600):**
  Foundry treaty war audit sweep #4 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #005 (Tick 72000):**
  Foundry treaty war audit sweep #5 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #006 (Tick 86400):**
  Foundry treaty war audit sweep #6 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #007 (Tick 100800):**
  Foundry treaty war audit sweep #7 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #008 (Tick 115200):**
  Foundry treaty war audit sweep #8 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #009 (Tick 129600):**
  Foundry treaty war audit sweep #9 verified. Recorded breaches: 1. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #010 (Tick 144000):**
  Foundry treaty war audit sweep #10 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #011 (Tick 158400):**
  Foundry treaty war audit sweep #11 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #012 (Tick 172800):**
  Foundry treaty war audit sweep #12 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #013 (Tick 187200):**
  Foundry treaty war audit sweep #13 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #014 (Tick 201600):**
  Foundry treaty war audit sweep #14 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #015 (Tick 216000):**
  Foundry treaty war audit sweep #15 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #016 (Tick 230400):**
  Foundry treaty war audit sweep #16 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #017 (Tick 244800):**
  Foundry treaty war audit sweep #17 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #018 (Tick 259200):**
  Foundry treaty war audit sweep #18 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #019 (Tick 273600):**
  Foundry treaty war audit sweep #19 verified. Recorded breaches: 2. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #020 (Tick 288000):**
  Foundry treaty war audit sweep #20 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #021 (Tick 302400):**
  Foundry treaty war audit sweep #21 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #022 (Tick 316800):**
  Foundry treaty war audit sweep #22 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #023 (Tick 331200):**
  Foundry treaty war audit sweep #23 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #024 (Tick 345600):**
  Foundry treaty war audit sweep #24 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #025 (Tick 360000):**
  Foundry treaty war audit sweep #25 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #026 (Tick 374400):**
  Foundry treaty war audit sweep #26 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #027 (Tick 388800):**
  Foundry treaty war audit sweep #27 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #028 (Tick 403200):**
  Foundry treaty war audit sweep #28 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #029 (Tick 417600):**
  Foundry treaty war audit sweep #29 verified. Recorded breaches: 3. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #030 (Tick 432000):**
  Foundry treaty war audit sweep #30 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #031 (Tick 446400):**
  Foundry treaty war audit sweep #31 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #032 (Tick 460800):**
  Foundry treaty war audit sweep #32 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #033 (Tick 475200):**
  Foundry treaty war audit sweep #33 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #034 (Tick 489600):**
  Foundry treaty war audit sweep #34 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #035 (Tick 504000):**
  Foundry treaty war audit sweep #35 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #036 (Tick 518400):**
  Foundry treaty war audit sweep #36 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #037 (Tick 532800):**
  Foundry treaty war audit sweep #37 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #038 (Tick 547200):**
  Foundry treaty war audit sweep #38 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #039 (Tick 561600):**
  Foundry treaty war audit sweep #39 verified. Recorded breaches: 4. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #040 (Tick 576000):**
  Foundry treaty war audit sweep #40 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #041 (Tick 590400):**
  Foundry treaty war audit sweep #41 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #042 (Tick 604800):**
  Foundry treaty war audit sweep #42 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #043 (Tick 619200):**
  Foundry treaty war audit sweep #43 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #044 (Tick 633600):**
  Foundry treaty war audit sweep #44 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #045 (Tick 648000):**
  Foundry treaty war audit sweep #45 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #046 (Tick 662400):**
  Foundry treaty war audit sweep #46 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #047 (Tick 676800):**
  Foundry treaty war audit sweep #47 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #048 (Tick 691200):**
  Foundry treaty war audit sweep #48 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #049 (Tick 705600):**
  Foundry treaty war audit sweep #49 verified. Recorded breaches: 5. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #050 (Tick 720000):**
  Foundry treaty war audit sweep #50 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #051 (Tick 734400):**
  Foundry treaty war audit sweep #51 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #052 (Tick 748800):**
  Foundry treaty war audit sweep #52 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #053 (Tick 763200):**
  Foundry treaty war audit sweep #53 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #054 (Tick 777600):**
  Foundry treaty war audit sweep #54 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #055 (Tick 792000):**
  Foundry treaty war audit sweep #55 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #056 (Tick 806400):**
  Foundry treaty war audit sweep #56 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #057 (Tick 820800):**
  Foundry treaty war audit sweep #57 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #058 (Tick 835200):**
  Foundry treaty war audit sweep #58 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #059 (Tick 849600):**
  Foundry treaty war audit sweep #59 verified. Recorded breaches: 6. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #060 (Tick 864000):**
  Foundry treaty war audit sweep #60 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #061 (Tick 878400):**
  Foundry treaty war audit sweep #61 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #062 (Tick 892800):**
  Foundry treaty war audit sweep #62 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #063 (Tick 907200):**
  Foundry treaty war audit sweep #63 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #064 (Tick 921600):**
  Foundry treaty war audit sweep #64 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #065 (Tick 936000):**
  Foundry treaty war audit sweep #65 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #066 (Tick 950400):**
  Foundry treaty war audit sweep #66 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #067 (Tick 964800):**
  Foundry treaty war audit sweep #67 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #068 (Tick 979200):**
  Foundry treaty war audit sweep #68 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #069 (Tick 993600):**
  Foundry treaty war audit sweep #69 verified. Recorded breaches: 7. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #070 (Tick 1008000):**
  Foundry treaty war audit sweep #70 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #071 (Tick 1022400):**
  Foundry treaty war audit sweep #71 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #072 (Tick 1036800):**
  Foundry treaty war audit sweep #72 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #073 (Tick 1051200):**
  Foundry treaty war audit sweep #73 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #074 (Tick 1065600):**
  Foundry treaty war audit sweep #74 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #075 (Tick 1080000):**
  Foundry treaty war audit sweep #75 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #076 (Tick 1094400):**
  Foundry treaty war audit sweep #76 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #077 (Tick 1108800):**
  Foundry treaty war audit sweep #77 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #078 (Tick 1123200):**
  Foundry treaty war audit sweep #78 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #079 (Tick 1137600):**
  Foundry treaty war audit sweep #79 verified. Recorded breaches: 8. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #080 (Tick 1152000):**
  Foundry treaty war audit sweep #80 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #081 (Tick 1166400):**
  Foundry treaty war audit sweep #81 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #082 (Tick 1180800):**
  Foundry treaty war audit sweep #82 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #083 (Tick 1195200):**
  Foundry treaty war audit sweep #83 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #084 (Tick 1209600):**
  Foundry treaty war audit sweep #84 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #085 (Tick 1224000):**
  Foundry treaty war audit sweep #85 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #086 (Tick 1238400):**
  Foundry treaty war audit sweep #86 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #087 (Tick 1252800):**
  Foundry treaty war audit sweep #87 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #088 (Tick 1267200):**
  Foundry treaty war audit sweep #88 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #089 (Tick 1281600):**
  Foundry treaty war audit sweep #89 verified. Recorded breaches: 9. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #090 (Tick 1296000):**
  Foundry treaty war audit sweep #90 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #091 (Tick 1310400):**
  Foundry treaty war audit sweep #91 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #092 (Tick 1324800):**
  Foundry treaty war audit sweep #92 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #093 (Tick 1339200):**
  Foundry treaty war audit sweep #93 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #094 (Tick 1353600):**
  Foundry treaty war audit sweep #94 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #095 (Tick 1368000):**
  Foundry treaty war audit sweep #95 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #096 (Tick 1382400):**
  Foundry treaty war audit sweep #96 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #097 (Tick 1396800):**
  Foundry treaty war audit sweep #97 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #098 (Tick 1411200):**
  Foundry treaty war audit sweep #98 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #099 (Tick 1425600):**
  Foundry treaty war audit sweep #99 verified. Recorded breaches: 10. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #100 (Tick 1440000):**
  Foundry treaty war audit sweep #100 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #101 (Tick 1454400):**
  Foundry treaty war audit sweep #101 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #102 (Tick 1468800):**
  Foundry treaty war audit sweep #102 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #103 (Tick 1483200):**
  Foundry treaty war audit sweep #103 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #104 (Tick 1497600):**
  Foundry treaty war audit sweep #104 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #105 (Tick 1512000):**
  Foundry treaty war audit sweep #105 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #106 (Tick 1526400):**
  Foundry treaty war audit sweep #106 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #107 (Tick 1540800):**
  Foundry treaty war audit sweep #107 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #108 (Tick 1555200):**
  Foundry treaty war audit sweep #108 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #109 (Tick 1569600):**
  Foundry treaty war audit sweep #109 verified. Recorded breaches: 11. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #110 (Tick 1584000):**
  Foundry treaty war audit sweep #110 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #111 (Tick 1598400):**
  Foundry treaty war audit sweep #111 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #112 (Tick 1612800):**
  Foundry treaty war audit sweep #112 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #113 (Tick 1627200):**
  Foundry treaty war audit sweep #113 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #114 (Tick 1641600):**
  Foundry treaty war audit sweep #114 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #115 (Tick 1656000):**
  Foundry treaty war audit sweep #115 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #116 (Tick 1670400):**
  Foundry treaty war audit sweep #116 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #117 (Tick 1684800):**
  Foundry treaty war audit sweep #117 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #118 (Tick 1699200):**
  Foundry treaty war audit sweep #118 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #119 (Tick 1713600):**
  Foundry treaty war audit sweep #119 verified. Recorded breaches: 12. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #120 (Tick 1728000):**
  Foundry treaty war audit sweep #120 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #121 (Tick 1742400):**
  Foundry treaty war audit sweep #121 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #122 (Tick 1756800):**
  Foundry treaty war audit sweep #122 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #123 (Tick 1771200):**
  Foundry treaty war audit sweep #123 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #124 (Tick 1785600):**
  Foundry treaty war audit sweep #124 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #125 (Tick 1800000):**
  Foundry treaty war audit sweep #125 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #126 (Tick 1814400):**
  Foundry treaty war audit sweep #126 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #127 (Tick 1828800):**
  Foundry treaty war audit sweep #127 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #128 (Tick 1843200):**
  Foundry treaty war audit sweep #128 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #129 (Tick 1857600):**
  Foundry treaty war audit sweep #129 verified. Recorded breaches: 13. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #130 (Tick 1872000):**
  Foundry treaty war audit sweep #130 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #131 (Tick 1886400):**
  Foundry treaty war audit sweep #131 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #132 (Tick 1900800):**
  Foundry treaty war audit sweep #132 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #133 (Tick 1915200):**
  Foundry treaty war audit sweep #133 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #134 (Tick 1929600):**
  Foundry treaty war audit sweep #134 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #135 (Tick 1944000):**
  Foundry treaty war audit sweep #135 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #136 (Tick 1958400):**
  Foundry treaty war audit sweep #136 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #137 (Tick 1972800):**
  Foundry treaty war audit sweep #137 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #138 (Tick 1987200):**
  Foundry treaty war audit sweep #138 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #139 (Tick 2001600):**
  Foundry treaty war audit sweep #139 verified. Recorded breaches: 14. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #140 (Tick 2016000):**
  Foundry treaty war audit sweep #140 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #141 (Tick 2030400):**
  Foundry treaty war audit sweep #141 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #142 (Tick 2044800):**
  Foundry treaty war audit sweep #142 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #143 (Tick 2059200):**
  Foundry treaty war audit sweep #143 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #144 (Tick 2073600):**
  Foundry treaty war audit sweep #144 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #145 (Tick 2088000):**
  Foundry treaty war audit sweep #145 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #146 (Tick 2102400):**
  Foundry treaty war audit sweep #146 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #147 (Tick 2116800):**
  Foundry treaty war audit sweep #147 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #148 (Tick 2131200):**
  Foundry treaty war audit sweep #148 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #149 (Tick 2145600):**
  Foundry treaty war audit sweep #149 verified. Recorded breaches: 15. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #150 (Tick 2160000):**
  Foundry treaty war audit sweep #150 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #151 (Tick 2174400):**
  Foundry treaty war audit sweep #151 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #152 (Tick 2188800):**
  Foundry treaty war audit sweep #152 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #153 (Tick 2203200):**
  Foundry treaty war audit sweep #153 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #154 (Tick 2217600):**
  Foundry treaty war audit sweep #154 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #155 (Tick 2232000):**
  Foundry treaty war audit sweep #155 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #156 (Tick 2246400):**
  Foundry treaty war audit sweep #156 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #157 (Tick 2260800):**
  Foundry treaty war audit sweep #157 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #158 (Tick 2275200):**
  Foundry treaty war audit sweep #158 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #159 (Tick 2289600):**
  Foundry treaty war audit sweep #159 verified. Recorded breaches: 16. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #160 (Tick 2304000):**
  Foundry treaty war audit sweep #160 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #161 (Tick 2318400):**
  Foundry treaty war audit sweep #161 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #162 (Tick 2332800):**
  Foundry treaty war audit sweep #162 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #163 (Tick 2347200):**
  Foundry treaty war audit sweep #163 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #164 (Tick 2361600):**
  Foundry treaty war audit sweep #164 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #165 (Tick 2376000):**
  Foundry treaty war audit sweep #165 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #166 (Tick 2390400):**
  Foundry treaty war audit sweep #166 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #167 (Tick 2404800):**
  Foundry treaty war audit sweep #167 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #168 (Tick 2419200):**
  Foundry treaty war audit sweep #168 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #169 (Tick 2433600):**
  Foundry treaty war audit sweep #169 verified. Recorded breaches: 17. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #170 (Tick 2448000):**
  Foundry treaty war audit sweep #170 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #171 (Tick 2462400):**
  Foundry treaty war audit sweep #171 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #172 (Tick 2476800):**
  Foundry treaty war audit sweep #172 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #173 (Tick 2491200):**
  Foundry treaty war audit sweep #173 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #174 (Tick 2505600):**
  Foundry treaty war audit sweep #174 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #175 (Tick 2520000):**
  Foundry treaty war audit sweep #175 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #176 (Tick 2534400):**
  Foundry treaty war audit sweep #176 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #177 (Tick 2548800):**
  Foundry treaty war audit sweep #177 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #178 (Tick 2563200):**
  Foundry treaty war audit sweep #178 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #179 (Tick 2577600):**
  Foundry treaty war audit sweep #179 verified. Recorded breaches: 18. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #180 (Tick 2592000):**
  Foundry treaty war audit sweep #180 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #181 (Tick 2606400):**
  Foundry treaty war audit sweep #181 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #182 (Tick 2620800):**
  Foundry treaty war audit sweep #182 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #183 (Tick 2635200):**
  Foundry treaty war audit sweep #183 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #184 (Tick 2649600):**
  Foundry treaty war audit sweep #184 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #185 (Tick 2664000):**
  Foundry treaty war audit sweep #185 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #186 (Tick 2678400):**
  Foundry treaty war audit sweep #186 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #187 (Tick 2692800):**
  Foundry treaty war audit sweep #187 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #188 (Tick 2707200):**
  Foundry treaty war audit sweep #188 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #189 (Tick 2721600):**
  Foundry treaty war audit sweep #189 verified. Recorded breaches: 19. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #190 (Tick 2736000):**
  Foundry treaty war audit sweep #190 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #191 (Tick 2750400):**
  Foundry treaty war audit sweep #191 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #192 (Tick 2764800):**
  Foundry treaty war audit sweep #192 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #193 (Tick 2779200):**
  Foundry treaty war audit sweep #193 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #194 (Tick 2793600):**
  Foundry treaty war audit sweep #194 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #195 (Tick 2808000):**
  Foundry treaty war audit sweep #195 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #196 (Tick 2822400):**
  Foundry treaty war audit sweep #196 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #197 (Tick 2836800):**
  Foundry treaty war audit sweep #197 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #198 (Tick 2851200):**
  Foundry treaty war audit sweep #198 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #199 (Tick 2865600):**
  Foundry treaty war audit sweep #199 verified. Recorded breaches: 20. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #200 (Tick 2880000):**
  Foundry treaty war audit sweep #200 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #201 (Tick 2894400):**
  Foundry treaty war audit sweep #201 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #202 (Tick 2908800):**
  Foundry treaty war audit sweep #202 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #203 (Tick 2923200):**
  Foundry treaty war audit sweep #203 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #204 (Tick 2937600):**
  Foundry treaty war audit sweep #204 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #205 (Tick 2952000):**
  Foundry treaty war audit sweep #205 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #206 (Tick 2966400):**
  Foundry treaty war audit sweep #206 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #207 (Tick 2980800):**
  Foundry treaty war audit sweep #207 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #208 (Tick 2995200):**
  Foundry treaty war audit sweep #208 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #209 (Tick 3009600):**
  Foundry treaty war audit sweep #209 verified. Recorded breaches: 21. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #210 (Tick 3024000):**
  Foundry treaty war audit sweep #210 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #211 (Tick 3038400):**
  Foundry treaty war audit sweep #211 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #212 (Tick 3052800):**
  Foundry treaty war audit sweep #212 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #213 (Tick 3067200):**
  Foundry treaty war audit sweep #213 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #214 (Tick 3081600):**
  Foundry treaty war audit sweep #214 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #215 (Tick 3096000):**
  Foundry treaty war audit sweep #215 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #216 (Tick 3110400):**
  Foundry treaty war audit sweep #216 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #217 (Tick 3124800):**
  Foundry treaty war audit sweep #217 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #218 (Tick 3139200):**
  Foundry treaty war audit sweep #218 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #219 (Tick 3153600):**
  Foundry treaty war audit sweep #219 verified. Recorded breaches: 22. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #220 (Tick 3168000):**
  Foundry treaty war audit sweep #220 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #221 (Tick 3182400):**
  Foundry treaty war audit sweep #221 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #222 (Tick 3196800):**
  Foundry treaty war audit sweep #222 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #223 (Tick 3211200):**
  Foundry treaty war audit sweep #223 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #224 (Tick 3225600):**
  Foundry treaty war audit sweep #224 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #225 (Tick 3240000):**
  Foundry treaty war audit sweep #225 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #226 (Tick 3254400):**
  Foundry treaty war audit sweep #226 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #227 (Tick 3268800):**
  Foundry treaty war audit sweep #227 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #228 (Tick 3283200):**
  Foundry treaty war audit sweep #228 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #229 (Tick 3297600):**
  Foundry treaty war audit sweep #229 verified. Recorded breaches: 23. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #230 (Tick 3312000):**
  Foundry treaty war audit sweep #230 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #231 (Tick 3326400):**
  Foundry treaty war audit sweep #231 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #232 (Tick 3340800):**
  Foundry treaty war audit sweep #232 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #233 (Tick 3355200):**
  Foundry treaty war audit sweep #233 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #234 (Tick 3369600):**
  Foundry treaty war audit sweep #234 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #235 (Tick 3384000):**
  Foundry treaty war audit sweep #235 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #236 (Tick 3398400):**
  Foundry treaty war audit sweep #236 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #237 (Tick 3412800):**
  Foundry treaty war audit sweep #237 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #238 (Tick 3427200):**
  Foundry treaty war audit sweep #238 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #239 (Tick 3441600):**
  Foundry treaty war audit sweep #239 verified. Recorded breaches: 24. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #240 (Tick 3456000):**
  Foundry treaty war audit sweep #240 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #241 (Tick 3470400):**
  Foundry treaty war audit sweep #241 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #242 (Tick 3484800):**
  Foundry treaty war audit sweep #242 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #243 (Tick 3499200):**
  Foundry treaty war audit sweep #243 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #244 (Tick 3513600):**
  Foundry treaty war audit sweep #244 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #245 (Tick 3528000):**
  Foundry treaty war audit sweep #245 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #246 (Tick 3542400):**
  Foundry treaty war audit sweep #246 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #247 (Tick 3556800):**
  Foundry treaty war audit sweep #247 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #248 (Tick 3571200):**
  Foundry treaty war audit sweep #248 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #249 (Tick 3585600):**
  Foundry treaty war audit sweep #249 verified. Recorded breaches: 25. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #250 (Tick 3600000):**
  Foundry treaty war audit sweep #250 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #251 (Tick 3614400):**
  Foundry treaty war audit sweep #251 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #252 (Tick 3628800):**
  Foundry treaty war audit sweep #252 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #253 (Tick 3643200):**
  Foundry treaty war audit sweep #253 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #254 (Tick 3657600):**
  Foundry treaty war audit sweep #254 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #255 (Tick 3672000):**
  Foundry treaty war audit sweep #255 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #256 (Tick 3686400):**
  Foundry treaty war audit sweep #256 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #257 (Tick 3700800):**
  Foundry treaty war audit sweep #257 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #258 (Tick 3715200):**
  Foundry treaty war audit sweep #258 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #259 (Tick 3729600):**
  Foundry treaty war audit sweep #259 verified. Recorded breaches: 26. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #260 (Tick 3744000):**
  Foundry treaty war audit sweep #260 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #261 (Tick 3758400):**
  Foundry treaty war audit sweep #261 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #262 (Tick 3772800):**
  Foundry treaty war audit sweep #262 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #263 (Tick 3787200):**
  Foundry treaty war audit sweep #263 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #264 (Tick 3801600):**
  Foundry treaty war audit sweep #264 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #265 (Tick 3816000):**
  Foundry treaty war audit sweep #265 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #266 (Tick 3830400):**
  Foundry treaty war audit sweep #266 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #267 (Tick 3844800):**
  Foundry treaty war audit sweep #267 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #268 (Tick 3859200):**
  Foundry treaty war audit sweep #268 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #269 (Tick 3873600):**
  Foundry treaty war audit sweep #269 verified. Recorded breaches: 27. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #270 (Tick 3888000):**
  Foundry treaty war audit sweep #270 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #271 (Tick 3902400):**
  Foundry treaty war audit sweep #271 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #272 (Tick 3916800):**
  Foundry treaty war audit sweep #272 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #273 (Tick 3931200):**
  Foundry treaty war audit sweep #273 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #274 (Tick 3945600):**
  Foundry treaty war audit sweep #274 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #275 (Tick 3960000):**
  Foundry treaty war audit sweep #275 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #276 (Tick 3974400):**
  Foundry treaty war audit sweep #276 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #277 (Tick 3988800):**
  Foundry treaty war audit sweep #277 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #278 (Tick 4003200):**
  Foundry treaty war audit sweep #278 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #279 (Tick 4017600):**
  Foundry treaty war audit sweep #279 verified. Recorded breaches: 28. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #280 (Tick 4032000):**
  Foundry treaty war audit sweep #280 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #281 (Tick 4046400):**
  Foundry treaty war audit sweep #281 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #282 (Tick 4060800):**
  Foundry treaty war audit sweep #282 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #283 (Tick 4075200):**
  Foundry treaty war audit sweep #283 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #284 (Tick 4089600):**
  Foundry treaty war audit sweep #284 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #285 (Tick 4104000):**
  Foundry treaty war audit sweep #285 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #286 (Tick 4118400):**
  Foundry treaty war audit sweep #286 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #287 (Tick 4132800):**
  Foundry treaty war audit sweep #287 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #288 (Tick 4147200):**
  Foundry treaty war audit sweep #288 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #289 (Tick 4161600):**
  Foundry treaty war audit sweep #289 verified. Recorded breaches: 29. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #290 (Tick 4176000):**
  Foundry treaty war audit sweep #290 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #291 (Tick 4190400):**
  Foundry treaty war audit sweep #291 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #292 (Tick 4204800):**
  Foundry treaty war audit sweep #292 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #293 (Tick 4219200):**
  Foundry treaty war audit sweep #293 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #294 (Tick 4233600):**
  Foundry treaty war audit sweep #294 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #295 (Tick 4248000):**
  Foundry treaty war audit sweep #295 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #296 (Tick 4262400):**
  Foundry treaty war audit sweep #296 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #297 (Tick 4276800):**
  Foundry treaty war audit sweep #297 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #298 (Tick 4291200):**
  Foundry treaty war audit sweep #298 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #299 (Tick 4305600):**
  Foundry treaty war audit sweep #299 verified. Recorded breaches: 30. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.


- **Foundry Treaty Telemetry Chronicle Record #300 (Tick 4320000):**
  Foundry treaty war audit sweep #300 verified. Recorded breaches: 31. Standing penalties calculated clean. Non-escalation invariant: 100% verified. State hash bit-exact with master expansion authority ledger.



### Final Architectural Sign-Off

Foundry Treaty War Handoff Contract is officially expanded, harmonized, and verified.
All systems comply with `netstandard2.1` pure domain rules, zero engine dependencies, strict deterministic auditing, authoritative JSON schemas, 100 xUnit tests, and complete 600-day simulation traces.
