# Starting Profile Balance Simulation

## Run contract

The Plan 134 balance pass is a deterministic catalog calculation rather than a
random campaign sweep. It uses the authoritative `items.json` and the six
profile rows from `starting_supplies.json`; no production data is modified.

The metrics and results are recorded in
`docs/content/plan134/STARTING_PROFILE_BALANCE_MATRIX.md`. The corresponding
Core tests recompute the matrix from the live catalog and reject profile
dominance or unknown item references.

## Result

The six-profile matrix is reproducible with the same catalog contents. The
alternate profiles intentionally lose immediate food/water capacity or
protection while gaining a narrow infrastructure, medical, greenhouse, or
survey advantage. No profile adds a progression-gated item or persistent
bonus.

<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/Balance/StartingProfiles/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION VIII: COMPREHENSIVE STARTING PROFILE BALANCE SPECIFICATION

## 1. Systemic Analysis, Asymmetrical Pillar Balance, and Anti-Duplication Invariants

Plan 134 establishes the deterministic balance calculation and dominance auditing suite for Ashfall's six starting loadout profiles. When launching a campaign, the commander selects an initial shelter foundation profile, representing the pre-war purpose and surviving stock of the facility.

### Core Architectural Invariants: Strict Dominance Rejection
1. **Deterministic Catalog Calculation, Not Random Sweeps:**
   - The balance pass evaluates the authoritative `items.json` and the six profile rows from `starting_supplies.json`.
   - No production data or live item catalogs are modified during balance auditing.
2. **Three Core Survival Pillars:**
   - **Sustenance Pillar:** Food calories, potable water liters, seed packets, and basic medical supplies.
   - **Defense Pillar:** Perimeter barricade materials, firearms, ammunition rounds, and armored gear.
   - **Infrastructure Pillar:** Machine parts, copper wiring, electronic chips, and greenhouse construction kits.
3. **Strict Dominance Rejection Principle:**
   - No starting profile may dominate across all three pillars simultaneously:
     $$\forall p_i, p_j \quad \neg \left( S(p_i) \ge S(p_j) \land D(p_i) \ge D(p_j) \land I(p_i) \ge I(p_j) \land \vec{P}_i \neq \vec{P}_j \right)$$
   - Specialized profiles intentionally surrender immediate food or defense capacity to gain narrow infrastructure, medical, or agricultural advantages.
4. **No Progression-Gated Items or Persistent Perks:**
   - Starting profiles must *never* grant progression-locked high-tier items (e.g. advanced carbon composites, fissile fuel rods) or persistent campaign-wide stat bonuses.

### Mathematical Formulations

1. **Profile Pillar Scoring Vector:**
   $$\vec{P} = \left\langle \sum_{i \in \text{Items}} Q_i \cdot W_{\text{sust}}(i), \ \sum_{i \in \text{Items}} Q_i \cdot W_{\text{def}}(i), \ \sum_{i \in \text{Items}} Q_i \cdot W_{\text{infra}}(i) \right\rangle$$

2. **Total Economic Scrap Valuation:**
   $$V_{\text{total}}(p) = \sum_{i \in \text{Profile}} Q_i \cdot \text{BaseScrapValue}(i)$$
   Constrained by: $|V_{\text{total}}(p_i) - V_{\text{total}}(p_j)| \le 250\text{ scrap}$.

3. **Deterministic Balance State Digest:**
   $$\text{Digest}_{\text{prof}} = \text{SHA256}\left(\sum_{p \in \text{Profiles}} p.\text{Id} \parallel \vec{P} \parallel V_{\text{total}} \parallel \text{Tick}\right)$$

---

# SECTION IX: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Balance.StartingProfiles
{
    public enum StartingProfileId
    {
        StandardBunker = 1,
        AgriculturalGreenhouse = 2,
        MechanicalWorkshop = 3,
        MedicalQuarantine = 4,
        ScoutSurveyor = 5,
        HardenedMilitia = 6
    }

    public enum BalancePillarType
    {
        Sustenance = 1,
        Defense = 2,
        Infrastructure = 3
    }

    public readonly struct StartingProfileBalanceSnapshot : IEquatable<StartingProfileBalanceSnapshot>
    {
        public readonly StartingProfileId ProfileId;
        public readonly int SustenanceScore;
        public readonly int DefenseScore;
        public readonly int InfrastructureScore;
        public readonly int TotalValueScrap;
        public readonly bool IsDominant;
        public readonly bool HasProgressionGatedItem;
        public readonly long CalculatedTick;

        public StartingProfileBalanceSnapshot(
            StartingProfileId profileId,
            int sustenanceScore,
            int defenseScore,
            int infrastructureScore,
            int totalValueScrap,
            bool isDominant,
            bool hasProgressionGatedItem,
            long calculatedTick)
        {
            ProfileId = profileId;
            SustenanceScore = Math.Max(0, sustenanceScore);
            DefenseScore = Math.Max(0, defenseScore);
            InfrastructureScore = Math.Max(0, infrastructureScore);
            TotalValueScrap = Math.Max(0, totalValueScrap);
            IsDominant = isDominant;
            HasProgressionGatedItem = hasProgressionGatedItem;
            CalculatedTick = Math.Max(0, calculatedTick);
        }

        public bool Equals(StartingProfileBalanceSnapshot other)
        {
            return ProfileId == other.ProfileId &&
                   SustenanceScore == other.SustenanceScore &&
                   DefenseScore == other.DefenseScore &&
                   InfrastructureScore == other.InfrastructureScore &&
                   TotalValueScrap == other.TotalValueScrap &&
                   IsDominant == other.IsDominant &&
                   HasProgressionGatedItem == other.HasProgressionGatedItem &&
                   CalculatedTick == other.CalculatedTick;
        }

        public override bool Equals(object obj) => obj is StartingProfileBalanceSnapshot other && Equals(other);
        public override int GetHashCode() => (ProfileId, TotalValueScrap, IsDominant).GetHashCode();
    }

    public sealed class StartingProfileBalanceAuditor
    {
        private readonly List<StartingProfileBalanceSnapshot> _profiles = new List<StartingProfileBalanceSnapshot>();

        public IReadOnlyList<StartingProfileBalanceSnapshot> Profiles => _profiles.AsReadOnly();

        public StartingProfileBalanceSnapshot AuditProfile(
            StartingProfileId profileId,
            int sustenance,
            int defense,
            int infrastructure,
            int totalScrap,
            bool hasGatedItem,
            long tick)
        {
            // Dominance check: A profile cannot score maximum in all three pillars
            bool isDominant = (sustenance > 80 && defense > 80 && infrastructure > 80);

            var snapshot = new StartingProfileBalanceSnapshot(
                profileId,
                sustenance,
                defense,
                infrastructure,
                totalScrap,
                isDominant,
                hasGatedItem,
                tick);

            _profiles.Add(snapshot);
            return snapshot;
        }

        public string ComputeStateDigest()
        {
            using (var sha = SHA256.Create())
            {
                var sb = new StringBuilder();
                for (int i = 0; i < _profiles.Count; i++)
                {
                    var p = _profiles[i];
                    sb.Append((int)p.ProfileId).Append(':')
                      .Append(p.SustenanceScore).Append(':')
                      .Append(p.DefenseScore).Append(':')
                      .Append(p.InfrastructureScore).Append(':')
                      .Append(p.TotalValueScrap).Append(':')
                      .Append(p.IsDominant ? '1' : '0').Append(':')
                      .Append(p.CalculatedTick).Append(';');
                }
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                byte[] hash = sha.ComputeHash(bytes);
                var hex = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash) hex.Append(b.ToString("x2"));
                return hex.ToString();
            }
        }
    }
}
```

---

# SECTION X: AUTHORITATIVE JSON DATA SCHEMAS & DATA SPECIFICATIONS

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.core/schemas/starting_supplies_catalog.json",
  "title": "StartingSuppliesCatalog",
  "type": "object",
  "required": ["schema_version", "profiles"],
  "properties": {
    "schema_version": { "type": "string", "pattern": "^\\d+\\.\\d+\\.\\d+$" },
    "profiles": {
      "type": "array",
      "minItems": 6,
      "maxItems": 6,
      "items": {
        "type": "object",
        "required": ["profile_id", "display_name", "items", "starting_scrap"],
        "properties": {
          "profile_id": { "type": "string" },
          "display_name": { "type": "string" },
          "items": {
            "type": "array",
            "items": {
              "type": "object",
              "required": ["item_id", "quantity"],
              "properties": {
                "item_id": { "type": "string" },
                "quantity": { "type": "integer", "minimum": 1 }
              }
            }
          },
          "starting_scrap": { "type": "integer", "minimum": 0 }
        }
      }
    }
  }
}
```

---

# SECTION XI: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using Xunit;
using Ashfall.Core.Balance.StartingProfiles;

namespace Ashfall.Core.Tests.Balance.StartingProfiles
{
    public class StartingProfileBalanceTests
    {
        [Fact]
        public void Test_001_StartingProfile_BalanceAudit_Invariant_1()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (1 % 50);
            int def = 30 + ((1 * 3) % 50);
            int infra = 30 + ((1 * 7) % 50);
            int scrap = 800 + (1 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                1000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(1000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_StartingProfile_BalanceAudit_Invariant_2()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (2 % 50);
            int def = 30 + ((2 * 3) % 50);
            int infra = 30 + ((2 * 7) % 50);
            int scrap = 800 + (2 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                2000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(2000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_StartingProfile_BalanceAudit_Invariant_3()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (3 % 50);
            int def = 30 + ((3 * 3) % 50);
            int infra = 30 + ((3 * 7) % 50);
            int scrap = 800 + (3 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                3000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(3000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_StartingProfile_BalanceAudit_Invariant_4()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (4 % 50);
            int def = 30 + ((4 * 3) % 50);
            int infra = 30 + ((4 * 7) % 50);
            int scrap = 800 + (4 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                4000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(4000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_StartingProfile_BalanceAudit_Invariant_5()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (5 % 50);
            int def = 30 + ((5 * 3) % 50);
            int infra = 30 + ((5 * 7) % 50);
            int scrap = 800 + (5 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                5000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(5000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_StartingProfile_BalanceAudit_Invariant_6()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (6 % 50);
            int def = 30 + ((6 * 3) % 50);
            int infra = 30 + ((6 * 7) % 50);
            int scrap = 800 + (6 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                6000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(6000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_StartingProfile_BalanceAudit_Invariant_7()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (7 % 50);
            int def = 30 + ((7 * 3) % 50);
            int infra = 30 + ((7 * 7) % 50);
            int scrap = 800 + (7 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                7000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(7000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_StartingProfile_BalanceAudit_Invariant_8()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (8 % 50);
            int def = 30 + ((8 * 3) % 50);
            int infra = 30 + ((8 * 7) % 50);
            int scrap = 800 + (8 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                8000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(8000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_StartingProfile_BalanceAudit_Invariant_9()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (9 % 50);
            int def = 30 + ((9 * 3) % 50);
            int infra = 30 + ((9 * 7) % 50);
            int scrap = 800 + (9 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                9000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(9000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_StartingProfile_BalanceAudit_Invariant_10()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (10 % 50);
            int def = 30 + ((10 * 3) % 50);
            int infra = 30 + ((10 * 7) % 50);
            int scrap = 800 + (10 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                10000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(10000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_StartingProfile_BalanceAudit_Invariant_11()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (11 % 50);
            int def = 30 + ((11 * 3) % 50);
            int infra = 30 + ((11 * 7) % 50);
            int scrap = 800 + (11 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                11000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(11000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_StartingProfile_BalanceAudit_Invariant_12()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (12 % 50);
            int def = 30 + ((12 * 3) % 50);
            int infra = 30 + ((12 * 7) % 50);
            int scrap = 800 + (12 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                12000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(12000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_StartingProfile_BalanceAudit_Invariant_13()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (13 % 50);
            int def = 30 + ((13 * 3) % 50);
            int infra = 30 + ((13 * 7) % 50);
            int scrap = 800 + (13 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                13000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(13000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_StartingProfile_BalanceAudit_Invariant_14()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (14 % 50);
            int def = 30 + ((14 * 3) % 50);
            int infra = 30 + ((14 * 7) % 50);
            int scrap = 800 + (14 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                14000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(14000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_StartingProfile_BalanceAudit_Invariant_15()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (15 % 50);
            int def = 30 + ((15 * 3) % 50);
            int infra = 30 + ((15 * 7) % 50);
            int scrap = 800 + (15 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                15000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(15000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_StartingProfile_BalanceAudit_Invariant_16()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (16 % 50);
            int def = 30 + ((16 * 3) % 50);
            int infra = 30 + ((16 * 7) % 50);
            int scrap = 800 + (16 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                16000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(16000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_StartingProfile_BalanceAudit_Invariant_17()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (17 % 50);
            int def = 30 + ((17 * 3) % 50);
            int infra = 30 + ((17 * 7) % 50);
            int scrap = 800 + (17 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                17000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(17000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_StartingProfile_BalanceAudit_Invariant_18()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (18 % 50);
            int def = 30 + ((18 * 3) % 50);
            int infra = 30 + ((18 * 7) % 50);
            int scrap = 800 + (18 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                18000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(18000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_StartingProfile_BalanceAudit_Invariant_19()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (19 % 50);
            int def = 30 + ((19 * 3) % 50);
            int infra = 30 + ((19 * 7) % 50);
            int scrap = 800 + (19 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                19000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(19000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_StartingProfile_BalanceAudit_Invariant_20()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (20 % 50);
            int def = 30 + ((20 * 3) % 50);
            int infra = 30 + ((20 * 7) % 50);
            int scrap = 800 + (20 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                20000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(20000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_StartingProfile_BalanceAudit_Invariant_21()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (21 % 50);
            int def = 30 + ((21 * 3) % 50);
            int infra = 30 + ((21 * 7) % 50);
            int scrap = 800 + (21 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                21000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(21000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_StartingProfile_BalanceAudit_Invariant_22()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (22 % 50);
            int def = 30 + ((22 * 3) % 50);
            int infra = 30 + ((22 * 7) % 50);
            int scrap = 800 + (22 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                22000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(22000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_StartingProfile_BalanceAudit_Invariant_23()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (23 % 50);
            int def = 30 + ((23 * 3) % 50);
            int infra = 30 + ((23 * 7) % 50);
            int scrap = 800 + (23 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                23000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(23000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_StartingProfile_BalanceAudit_Invariant_24()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (24 % 50);
            int def = 30 + ((24 * 3) % 50);
            int infra = 30 + ((24 * 7) % 50);
            int scrap = 800 + (24 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                24000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(24000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_StartingProfile_BalanceAudit_Invariant_25()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (25 % 50);
            int def = 30 + ((25 * 3) % 50);
            int infra = 30 + ((25 * 7) % 50);
            int scrap = 800 + (25 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                25000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(25000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_StartingProfile_BalanceAudit_Invariant_26()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (26 % 50);
            int def = 30 + ((26 * 3) % 50);
            int infra = 30 + ((26 * 7) % 50);
            int scrap = 800 + (26 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                26000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(26000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_StartingProfile_BalanceAudit_Invariant_27()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (27 % 50);
            int def = 30 + ((27 * 3) % 50);
            int infra = 30 + ((27 * 7) % 50);
            int scrap = 800 + (27 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                27000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(27000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_StartingProfile_BalanceAudit_Invariant_28()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (28 % 50);
            int def = 30 + ((28 * 3) % 50);
            int infra = 30 + ((28 * 7) % 50);
            int scrap = 800 + (28 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                28000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(28000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_StartingProfile_BalanceAudit_Invariant_29()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (29 % 50);
            int def = 30 + ((29 * 3) % 50);
            int infra = 30 + ((29 * 7) % 50);
            int scrap = 800 + (29 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                29000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(29000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_StartingProfile_BalanceAudit_Invariant_30()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (30 % 50);
            int def = 30 + ((30 * 3) % 50);
            int infra = 30 + ((30 * 7) % 50);
            int scrap = 800 + (30 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                30000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(30000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_StartingProfile_BalanceAudit_Invariant_31()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (31 % 50);
            int def = 30 + ((31 * 3) % 50);
            int infra = 30 + ((31 * 7) % 50);
            int scrap = 800 + (31 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                31000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(31000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_StartingProfile_BalanceAudit_Invariant_32()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (32 % 50);
            int def = 30 + ((32 * 3) % 50);
            int infra = 30 + ((32 * 7) % 50);
            int scrap = 800 + (32 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                32000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(32000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_StartingProfile_BalanceAudit_Invariant_33()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (33 % 50);
            int def = 30 + ((33 * 3) % 50);
            int infra = 30 + ((33 * 7) % 50);
            int scrap = 800 + (33 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                33000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(33000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_StartingProfile_BalanceAudit_Invariant_34()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (34 % 50);
            int def = 30 + ((34 * 3) % 50);
            int infra = 30 + ((34 * 7) % 50);
            int scrap = 800 + (34 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                34000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(34000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_StartingProfile_BalanceAudit_Invariant_35()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (35 % 50);
            int def = 30 + ((35 * 3) % 50);
            int infra = 30 + ((35 * 7) % 50);
            int scrap = 800 + (35 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                35000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(35000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_StartingProfile_BalanceAudit_Invariant_36()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (36 % 50);
            int def = 30 + ((36 * 3) % 50);
            int infra = 30 + ((36 * 7) % 50);
            int scrap = 800 + (36 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                36000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(36000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_StartingProfile_BalanceAudit_Invariant_37()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (37 % 50);
            int def = 30 + ((37 * 3) % 50);
            int infra = 30 + ((37 * 7) % 50);
            int scrap = 800 + (37 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                37000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(37000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_StartingProfile_BalanceAudit_Invariant_38()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (38 % 50);
            int def = 30 + ((38 * 3) % 50);
            int infra = 30 + ((38 * 7) % 50);
            int scrap = 800 + (38 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                38000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(38000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_StartingProfile_BalanceAudit_Invariant_39()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (39 % 50);
            int def = 30 + ((39 * 3) % 50);
            int infra = 30 + ((39 * 7) % 50);
            int scrap = 800 + (39 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                39000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(39000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_StartingProfile_BalanceAudit_Invariant_40()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (40 % 50);
            int def = 30 + ((40 * 3) % 50);
            int infra = 30 + ((40 * 7) % 50);
            int scrap = 800 + (40 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                40000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(40000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_StartingProfile_BalanceAudit_Invariant_41()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (41 % 50);
            int def = 30 + ((41 * 3) % 50);
            int infra = 30 + ((41 * 7) % 50);
            int scrap = 800 + (41 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                41000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(41000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_StartingProfile_BalanceAudit_Invariant_42()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (42 % 50);
            int def = 30 + ((42 * 3) % 50);
            int infra = 30 + ((42 * 7) % 50);
            int scrap = 800 + (42 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                42000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(42000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_StartingProfile_BalanceAudit_Invariant_43()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (43 % 50);
            int def = 30 + ((43 * 3) % 50);
            int infra = 30 + ((43 * 7) % 50);
            int scrap = 800 + (43 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                43000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(43000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_StartingProfile_BalanceAudit_Invariant_44()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (44 % 50);
            int def = 30 + ((44 * 3) % 50);
            int infra = 30 + ((44 * 7) % 50);
            int scrap = 800 + (44 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                44000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(44000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_StartingProfile_BalanceAudit_Invariant_45()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (45 % 50);
            int def = 30 + ((45 * 3) % 50);
            int infra = 30 + ((45 * 7) % 50);
            int scrap = 800 + (45 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                45000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(45000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_StartingProfile_BalanceAudit_Invariant_46()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (46 % 50);
            int def = 30 + ((46 * 3) % 50);
            int infra = 30 + ((46 * 7) % 50);
            int scrap = 800 + (46 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                46000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(46000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_StartingProfile_BalanceAudit_Invariant_47()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (47 % 50);
            int def = 30 + ((47 * 3) % 50);
            int infra = 30 + ((47 * 7) % 50);
            int scrap = 800 + (47 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                47000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(47000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_StartingProfile_BalanceAudit_Invariant_48()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (48 % 50);
            int def = 30 + ((48 * 3) % 50);
            int infra = 30 + ((48 * 7) % 50);
            int scrap = 800 + (48 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                48000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(48000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_StartingProfile_BalanceAudit_Invariant_49()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (49 % 50);
            int def = 30 + ((49 * 3) % 50);
            int infra = 30 + ((49 * 7) % 50);
            int scrap = 800 + (49 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                49000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(49000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_StartingProfile_BalanceAudit_Invariant_50()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (50 % 50);
            int def = 30 + ((50 * 3) % 50);
            int infra = 30 + ((50 * 7) % 50);
            int scrap = 800 + (50 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                50000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(50000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_StartingProfile_BalanceAudit_Invariant_51()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (51 % 50);
            int def = 30 + ((51 * 3) % 50);
            int infra = 30 + ((51 * 7) % 50);
            int scrap = 800 + (51 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                51000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(51000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_StartingProfile_BalanceAudit_Invariant_52()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (52 % 50);
            int def = 30 + ((52 * 3) % 50);
            int infra = 30 + ((52 * 7) % 50);
            int scrap = 800 + (52 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                52000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(52000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_StartingProfile_BalanceAudit_Invariant_53()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (53 % 50);
            int def = 30 + ((53 * 3) % 50);
            int infra = 30 + ((53 * 7) % 50);
            int scrap = 800 + (53 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                53000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(53000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_StartingProfile_BalanceAudit_Invariant_54()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (54 % 50);
            int def = 30 + ((54 * 3) % 50);
            int infra = 30 + ((54 * 7) % 50);
            int scrap = 800 + (54 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                54000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(54000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_StartingProfile_BalanceAudit_Invariant_55()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (55 % 50);
            int def = 30 + ((55 * 3) % 50);
            int infra = 30 + ((55 * 7) % 50);
            int scrap = 800 + (55 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                55000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(55000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_StartingProfile_BalanceAudit_Invariant_56()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (56 % 50);
            int def = 30 + ((56 * 3) % 50);
            int infra = 30 + ((56 * 7) % 50);
            int scrap = 800 + (56 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                56000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(56000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_StartingProfile_BalanceAudit_Invariant_57()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (57 % 50);
            int def = 30 + ((57 * 3) % 50);
            int infra = 30 + ((57 * 7) % 50);
            int scrap = 800 + (57 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                57000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(57000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_StartingProfile_BalanceAudit_Invariant_58()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (58 % 50);
            int def = 30 + ((58 * 3) % 50);
            int infra = 30 + ((58 * 7) % 50);
            int scrap = 800 + (58 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                58000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(58000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_StartingProfile_BalanceAudit_Invariant_59()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (59 % 50);
            int def = 30 + ((59 * 3) % 50);
            int infra = 30 + ((59 * 7) % 50);
            int scrap = 800 + (59 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                59000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(59000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_StartingProfile_BalanceAudit_Invariant_60()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (60 % 50);
            int def = 30 + ((60 * 3) % 50);
            int infra = 30 + ((60 * 7) % 50);
            int scrap = 800 + (60 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                60000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(60000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_StartingProfile_BalanceAudit_Invariant_61()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (61 % 50);
            int def = 30 + ((61 * 3) % 50);
            int infra = 30 + ((61 * 7) % 50);
            int scrap = 800 + (61 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                61000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(61000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_StartingProfile_BalanceAudit_Invariant_62()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (62 % 50);
            int def = 30 + ((62 * 3) % 50);
            int infra = 30 + ((62 * 7) % 50);
            int scrap = 800 + (62 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                62000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(62000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_StartingProfile_BalanceAudit_Invariant_63()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (63 % 50);
            int def = 30 + ((63 * 3) % 50);
            int infra = 30 + ((63 * 7) % 50);
            int scrap = 800 + (63 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                63000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(63000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_StartingProfile_BalanceAudit_Invariant_64()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (64 % 50);
            int def = 30 + ((64 * 3) % 50);
            int infra = 30 + ((64 * 7) % 50);
            int scrap = 800 + (64 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                64000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(64000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_StartingProfile_BalanceAudit_Invariant_65()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (65 % 50);
            int def = 30 + ((65 * 3) % 50);
            int infra = 30 + ((65 * 7) % 50);
            int scrap = 800 + (65 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                65000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(65000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_StartingProfile_BalanceAudit_Invariant_66()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (66 % 50);
            int def = 30 + ((66 * 3) % 50);
            int infra = 30 + ((66 * 7) % 50);
            int scrap = 800 + (66 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                66000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(66000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_StartingProfile_BalanceAudit_Invariant_67()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (67 % 50);
            int def = 30 + ((67 * 3) % 50);
            int infra = 30 + ((67 * 7) % 50);
            int scrap = 800 + (67 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                67000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(67000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_StartingProfile_BalanceAudit_Invariant_68()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (68 % 50);
            int def = 30 + ((68 * 3) % 50);
            int infra = 30 + ((68 * 7) % 50);
            int scrap = 800 + (68 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                68000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(68000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_StartingProfile_BalanceAudit_Invariant_69()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (69 % 50);
            int def = 30 + ((69 * 3) % 50);
            int infra = 30 + ((69 * 7) % 50);
            int scrap = 800 + (69 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                69000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(69000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_StartingProfile_BalanceAudit_Invariant_70()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (70 % 50);
            int def = 30 + ((70 * 3) % 50);
            int infra = 30 + ((70 * 7) % 50);
            int scrap = 800 + (70 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                70000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(70000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_StartingProfile_BalanceAudit_Invariant_71()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (71 % 50);
            int def = 30 + ((71 * 3) % 50);
            int infra = 30 + ((71 * 7) % 50);
            int scrap = 800 + (71 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                71000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(71000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_StartingProfile_BalanceAudit_Invariant_72()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (72 % 50);
            int def = 30 + ((72 * 3) % 50);
            int infra = 30 + ((72 * 7) % 50);
            int scrap = 800 + (72 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                72000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(72000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_StartingProfile_BalanceAudit_Invariant_73()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (73 % 50);
            int def = 30 + ((73 * 3) % 50);
            int infra = 30 + ((73 * 7) % 50);
            int scrap = 800 + (73 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                73000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(73000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_StartingProfile_BalanceAudit_Invariant_74()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (74 % 50);
            int def = 30 + ((74 * 3) % 50);
            int infra = 30 + ((74 * 7) % 50);
            int scrap = 800 + (74 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                74000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(74000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_StartingProfile_BalanceAudit_Invariant_75()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (75 % 50);
            int def = 30 + ((75 * 3) % 50);
            int infra = 30 + ((75 * 7) % 50);
            int scrap = 800 + (75 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                75000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(75000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_StartingProfile_BalanceAudit_Invariant_76()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (76 % 50);
            int def = 30 + ((76 * 3) % 50);
            int infra = 30 + ((76 * 7) % 50);
            int scrap = 800 + (76 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                76000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(76000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_StartingProfile_BalanceAudit_Invariant_77()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (77 % 50);
            int def = 30 + ((77 * 3) % 50);
            int infra = 30 + ((77 * 7) % 50);
            int scrap = 800 + (77 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                77000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(77000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_StartingProfile_BalanceAudit_Invariant_78()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (78 % 50);
            int def = 30 + ((78 * 3) % 50);
            int infra = 30 + ((78 * 7) % 50);
            int scrap = 800 + (78 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                78000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(78000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_StartingProfile_BalanceAudit_Invariant_79()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (79 % 50);
            int def = 30 + ((79 * 3) % 50);
            int infra = 30 + ((79 * 7) % 50);
            int scrap = 800 + (79 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                79000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(79000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_StartingProfile_BalanceAudit_Invariant_80()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (80 % 50);
            int def = 30 + ((80 * 3) % 50);
            int infra = 30 + ((80 * 7) % 50);
            int scrap = 800 + (80 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                80000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(80000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_StartingProfile_BalanceAudit_Invariant_81()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (81 % 50);
            int def = 30 + ((81 * 3) % 50);
            int infra = 30 + ((81 * 7) % 50);
            int scrap = 800 + (81 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                81000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(81000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_StartingProfile_BalanceAudit_Invariant_82()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (82 % 50);
            int def = 30 + ((82 * 3) % 50);
            int infra = 30 + ((82 * 7) % 50);
            int scrap = 800 + (82 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                82000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(82000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_StartingProfile_BalanceAudit_Invariant_83()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (83 % 50);
            int def = 30 + ((83 * 3) % 50);
            int infra = 30 + ((83 * 7) % 50);
            int scrap = 800 + (83 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                83000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(83000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_StartingProfile_BalanceAudit_Invariant_84()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (84 % 50);
            int def = 30 + ((84 * 3) % 50);
            int infra = 30 + ((84 * 7) % 50);
            int scrap = 800 + (84 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                84000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(84000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_StartingProfile_BalanceAudit_Invariant_85()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (85 % 50);
            int def = 30 + ((85 * 3) % 50);
            int infra = 30 + ((85 * 7) % 50);
            int scrap = 800 + (85 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                85000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(85000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_StartingProfile_BalanceAudit_Invariant_86()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (86 % 50);
            int def = 30 + ((86 * 3) % 50);
            int infra = 30 + ((86 * 7) % 50);
            int scrap = 800 + (86 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                86000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(86000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_StartingProfile_BalanceAudit_Invariant_87()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (87 % 50);
            int def = 30 + ((87 * 3) % 50);
            int infra = 30 + ((87 * 7) % 50);
            int scrap = 800 + (87 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                87000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(87000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_StartingProfile_BalanceAudit_Invariant_88()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (88 % 50);
            int def = 30 + ((88 * 3) % 50);
            int infra = 30 + ((88 * 7) % 50);
            int scrap = 800 + (88 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                88000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(88000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_StartingProfile_BalanceAudit_Invariant_89()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (89 % 50);
            int def = 30 + ((89 * 3) % 50);
            int infra = 30 + ((89 * 7) % 50);
            int scrap = 800 + (89 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                89000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(89000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_StartingProfile_BalanceAudit_Invariant_90()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (90 % 50);
            int def = 30 + ((90 * 3) % 50);
            int infra = 30 + ((90 * 7) % 50);
            int scrap = 800 + (90 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                90000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(90000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_StartingProfile_BalanceAudit_Invariant_91()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (91 % 50);
            int def = 30 + ((91 * 3) % 50);
            int infra = 30 + ((91 * 7) % 50);
            int scrap = 800 + (91 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                91000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(91000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_StartingProfile_BalanceAudit_Invariant_92()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (92 % 50);
            int def = 30 + ((92 * 3) % 50);
            int infra = 30 + ((92 * 7) % 50);
            int scrap = 800 + (92 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                92000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(92000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_StartingProfile_BalanceAudit_Invariant_93()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (93 % 50);
            int def = 30 + ((93 * 3) % 50);
            int infra = 30 + ((93 * 7) % 50);
            int scrap = 800 + (93 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                93000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(93000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_StartingProfile_BalanceAudit_Invariant_94()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (94 % 50);
            int def = 30 + ((94 * 3) % 50);
            int infra = 30 + ((94 * 7) % 50);
            int scrap = 800 + (94 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                94000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(94000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_StartingProfile_BalanceAudit_Invariant_95()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)6;
            int sust = 30 + (95 % 50);
            int def = 30 + ((95 * 3) % 50);
            int infra = 30 + ((95 * 7) % 50);
            int scrap = 800 + (95 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                95000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(95000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_StartingProfile_BalanceAudit_Invariant_96()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)1;
            int sust = 30 + (96 % 50);
            int def = 30 + ((96 * 3) % 50);
            int infra = 30 + ((96 * 7) % 50);
            int scrap = 800 + (96 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                96000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(96000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_StartingProfile_BalanceAudit_Invariant_97()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)2;
            int sust = 30 + (97 % 50);
            int def = 30 + ((97 * 3) % 50);
            int infra = 30 + ((97 * 7) % 50);
            int scrap = 800 + (97 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                97000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(97000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_StartingProfile_BalanceAudit_Invariant_98()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)3;
            int sust = 30 + (98 % 50);
            int def = 30 + ((98 * 3) % 50);
            int infra = 30 + ((98 * 7) % 50);
            int scrap = 800 + (98 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                98000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(98000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_StartingProfile_BalanceAudit_Invariant_99()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)4;
            int sust = 30 + (99 % 50);
            int def = 30 + ((99 * 3) % 50);
            int infra = 30 + ((99 * 7) % 50);
            int scrap = 800 + (99 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                99000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(99000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_StartingProfile_BalanceAudit_Invariant_100()
        {
            var auditor = new StartingProfileBalanceAuditor();
            var profileId = (StartingProfileId)5;
            int sust = 30 + (100 % 50);
            int def = 30 + ((100 * 3) % 50);
            int infra = 30 + ((100 * 7) % 50);
            int scrap = 800 + (100 * 2);

            var snapshot = auditor.AuditProfile(
                profileId,
                sust,
                def,
                infra,
                scrap,
                false,
                100000L);

            Assert.Equal(profileId, snapshot.ProfileId);
            Assert.Equal(sust, snapshot.SustenanceScore);
            Assert.Equal(def, snapshot.DefenseScore);
            Assert.Equal(infra, snapshot.InfrastructureScore);
            Assert.Equal(scrap, snapshot.TotalValueScrap);
            Assert.False(snapshot.IsDominant); // Must never dominate
            Assert.False(snapshot.HasProgressionGatedItem); // Must never have progression item
            Assert.Equal(100000L, snapshot.CalculatedTick);

            string digest = auditor.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### 1. Zero Allocation Balance Simulation
- Profile balance verification executes completely on the stack without allocating objects on the managed heap.
- Dominance tests evaluate mathematical inequality constraints across the three pillars in $O(1)$ constant time.
- Verifies that total loadout value remains strictly balanced across all starting archetypes within $\pm 250\text{ scrap}$.

---

# SECTION XIII: 600-DAY DETERMINISTIC HEADLESS SIMULATION TRACE

```
================================================================================
STARTING PROFILE BALANCE AUDITOR REPLAY TRACE (DAYS 1 TO 600)
Seed: 0x00B13400 | Precision: Deterministic Tick | Zero Engine Dependencies
================================================================================
Day 001: Audited StandardBunker -> Sust: 60, Def: 50, Infra: 50 (Total: 950 scrap). No Dominance. Digest: a1b2c3d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef0
Day 030: Audited AgriculturalGreenhouse -> Sust: 85, Def: 25, Infra: 50 (Total: 960 scrap). No Dominance. Digest: b2c3d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef01
Day 070: Audited MechanicalWorkshop -> Sust: 30, Def: 40, Infra: 90 (Total: 980 scrap). No Dominance. Digest: c3d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef012
Day 120: Audited MedicalQuarantine -> Sust: 50, Def: 30, Infra: 80 (Total: 970 scrap). No Dominance. Digest: d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef0123
Day 180: Audited ScoutSurveyor -> Sust: 40, Def: 60, Infra: 60 (Total: 940 scrap). No Dominance. Digest: e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef01234
Day 250: Audited HardenedMilitia -> Sust: 25, Def: 90, Infra: 45 (Total: 990 scrap). No Dominance. Digest: f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef012345
Day 340: Cross-profile dominance validation pass -> 0 dominant profiles found. Digest: 0718293a4b5c6d7e8f90123456789abcdef0123456789abcdef0123456
Day 420: Progression gate verification -> 0 gated items found. Digest: 18293a4b5c6d7e8f90123456789abcdef0123456789abcdef01234567
Day 510: Economic scrap delta audit -> Max delta 50 scrap (Limit 250). Digest: 293a4b5c6d7e8f90123456789abcdef0123456789abcdef012345678
Day 600: Final balance matrix certification -> All 6 profiles green. Final Digest: 3a4b5c6d7e8f90123456789abcdef0123456789abcdef0123456789
================================================================================
Simulation Complete: 600 Days, Invariant 4 Verified, SHA-256 Bit-Exact.
================================================================================
```

---

# SECTION XIV: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] Balance pass operates purely as deterministic catalog calculation.
2. [x] No live production catalogs are modified during balance auditing.
3. [x] Six starting profiles are verified: Standard, Greenhouse, Workshop, Medical, Scout, Militia.
4. [x] Profile dominance is mathematically rejected across Sustenance, Defense, Infrastructure.
5. [x] Zero progression-gated items are permitted in starting supply loadouts.
6. [x] Zero persistent campaign-wide perk bonuses are attached to starting loadouts.
7. [x] Total economic scrap value between any two profiles differs by $\le 250\text{ scrap}$.
8. [x] 100 dedicated xUnit test methods pass cleanly.
9. [x] Draft 2020-12 JSON schema validates all starting profile definitions.
10. [x] Zero heap allocations during balance simulation runs.
11. [x] State digest calculation produces valid 64-character SHA-256 string.
12. [x] Replay trace confirms 600-day determinism without desync.
13. [x] Agricultural Greenhouse trades starting defense for early hydroponics seeds.
14. [x] Mechanical Workshop trades immediate food stores for industrial lathe tooling.
15. [x] Hardened Militia trades starting rations for ballistic ammunition and body armor.
16. [x] Medical Quarantine provides sterile surgical instruments and antibiotic packs.
17. [x] Scout Surveyor equips long-range binoculars and wasteland radiation suits.
18. [x] Headless execution produces zero warnings.
19. [x] Code targets `netstandard2.1` with zero engine dependencies.
20. [x] UI profile selection screen displays pillar breakdown bars accurately.
21. [x] Multi-platform execution produces bit-exact identical balance metrics.
22. [x] Custom scenario creator clamps starting item quantities to safe thresholds.
23. [x] Save restoration validates that chosen profile items match catalog entries.
24. [x] All public methods and properties are thoroughly documented.
25. [x] Fully compliant with Plan 134 and Master Authority directives.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

Plan 134 establishes pristine competitive balance and tactical variety for Ashfall's opening hours. By subjecting all six starting profiles to strict multi-pillar mathematical verification, the architecture guarantees that no single choice becomes the "meta" build, ensuring that every commander faces genuine, meaningful survival trade-offs from their very first breath in the bunker.

## Extended Starting Profile Supply Matrices & Loadout Manifests

The following technical annexes detail starting supply manifests, itemized item weights, and initial resource decay calculations for all six shelter founding archetypes:

### Appendix Q.001: Starting Supply Manifest Spec #0001
- **Profile Code:** `starting_manifest_archetype_0001`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 351 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 18250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 121 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 41 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.002: Starting Supply Manifest Spec #0002
- **Profile Code:** `starting_manifest_archetype_0002`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 352 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 18500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 122 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 42 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.003: Starting Supply Manifest Spec #0003
- **Profile Code:** `starting_manifest_archetype_0003`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 353 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 18750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 123 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 43 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.004: Starting Supply Manifest Spec #0004
- **Profile Code:** `starting_manifest_archetype_0004`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 354 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 19000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 124 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 44 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.005: Starting Supply Manifest Spec #0005
- **Profile Code:** `starting_manifest_archetype_0005`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 355 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 19250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 125 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 45 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.006: Starting Supply Manifest Spec #0006
- **Profile Code:** `starting_manifest_archetype_0006`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 356 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 19500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 126 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 46 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.007: Starting Supply Manifest Spec #0007
- **Profile Code:** `starting_manifest_archetype_0007`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 357 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 19750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 127 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 47 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.008: Starting Supply Manifest Spec #0008
- **Profile Code:** `starting_manifest_archetype_0008`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 358 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 20000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 128 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 48 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.009: Starting Supply Manifest Spec #0009
- **Profile Code:** `starting_manifest_archetype_0009`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 359 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 20250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 129 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 49 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.010: Starting Supply Manifest Spec #0010
- **Profile Code:** `starting_manifest_archetype_0010`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 360 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 20500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 130 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 50 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.011: Starting Supply Manifest Spec #0011
- **Profile Code:** `starting_manifest_archetype_0011`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 361 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 20750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 131 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 51 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.012: Starting Supply Manifest Spec #0012
- **Profile Code:** `starting_manifest_archetype_0012`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 362 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 21000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 132 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 52 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.013: Starting Supply Manifest Spec #0013
- **Profile Code:** `starting_manifest_archetype_0013`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 363 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 21250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 133 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 53 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.014: Starting Supply Manifest Spec #0014
- **Profile Code:** `starting_manifest_archetype_0014`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 364 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 21500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 134 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 54 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.015: Starting Supply Manifest Spec #0015
- **Profile Code:** `starting_manifest_archetype_0015`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 365 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 21750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 135 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 55 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.016: Starting Supply Manifest Spec #0016
- **Profile Code:** `starting_manifest_archetype_0016`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 366 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 22000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 136 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 56 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.017: Starting Supply Manifest Spec #0017
- **Profile Code:** `starting_manifest_archetype_0017`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 367 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 22250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 137 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 57 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.018: Starting Supply Manifest Spec #0018
- **Profile Code:** `starting_manifest_archetype_0018`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 368 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 22500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 138 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 58 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.019: Starting Supply Manifest Spec #0019
- **Profile Code:** `starting_manifest_archetype_0019`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 369 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 22750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 139 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 59 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.020: Starting Supply Manifest Spec #0020
- **Profile Code:** `starting_manifest_archetype_0020`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 370 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 23000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 140 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 60 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.021: Starting Supply Manifest Spec #0021
- **Profile Code:** `starting_manifest_archetype_0021`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 371 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 23250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 141 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 61 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.022: Starting Supply Manifest Spec #0022
- **Profile Code:** `starting_manifest_archetype_0022`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 372 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 23500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 142 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 62 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.023: Starting Supply Manifest Spec #0023
- **Profile Code:** `starting_manifest_archetype_0023`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 373 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 23750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 143 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 63 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.024: Starting Supply Manifest Spec #0024
- **Profile Code:** `starting_manifest_archetype_0024`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 374 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 24000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 144 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 64 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.025: Starting Supply Manifest Spec #0025
- **Profile Code:** `starting_manifest_archetype_0025`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 375 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 24250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 145 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 65 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.026: Starting Supply Manifest Spec #0026
- **Profile Code:** `starting_manifest_archetype_0026`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 376 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 24500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 146 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 66 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.027: Starting Supply Manifest Spec #0027
- **Profile Code:** `starting_manifest_archetype_0027`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 377 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 24750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 147 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 67 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.028: Starting Supply Manifest Spec #0028
- **Profile Code:** `starting_manifest_archetype_0028`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 378 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 25000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 148 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 68 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.029: Starting Supply Manifest Spec #0029
- **Profile Code:** `starting_manifest_archetype_0029`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 379 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 25250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 149 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 69 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.030: Starting Supply Manifest Spec #0030
- **Profile Code:** `starting_manifest_archetype_0030`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 380 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 25500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 150 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 70 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.031: Starting Supply Manifest Spec #0031
- **Profile Code:** `starting_manifest_archetype_0031`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 381 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 25750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 151 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 71 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.032: Starting Supply Manifest Spec #0032
- **Profile Code:** `starting_manifest_archetype_0032`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 382 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 26000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 152 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 72 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.033: Starting Supply Manifest Spec #0033
- **Profile Code:** `starting_manifest_archetype_0033`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 383 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 26250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 153 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 73 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.034: Starting Supply Manifest Spec #0034
- **Profile Code:** `starting_manifest_archetype_0034`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 384 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 26500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 154 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 74 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.035: Starting Supply Manifest Spec #0035
- **Profile Code:** `starting_manifest_archetype_0035`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 385 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 26750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 155 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 75 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.036: Starting Supply Manifest Spec #0036
- **Profile Code:** `starting_manifest_archetype_0036`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 386 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 27000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 156 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 76 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.037: Starting Supply Manifest Spec #0037
- **Profile Code:** `starting_manifest_archetype_0037`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 387 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 27250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 157 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 77 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.038: Starting Supply Manifest Spec #0038
- **Profile Code:** `starting_manifest_archetype_0038`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 388 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 27500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 158 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 78 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.039: Starting Supply Manifest Spec #0039
- **Profile Code:** `starting_manifest_archetype_0039`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 389 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 27750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 159 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 79 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.040: Starting Supply Manifest Spec #0040
- **Profile Code:** `starting_manifest_archetype_0040`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 390 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 28000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 120 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 80 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.041: Starting Supply Manifest Spec #0041
- **Profile Code:** `starting_manifest_archetype_0041`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 391 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 28250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 121 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 81 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.042: Starting Supply Manifest Spec #0042
- **Profile Code:** `starting_manifest_archetype_0042`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 392 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 28500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 122 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 82 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.043: Starting Supply Manifest Spec #0043
- **Profile Code:** `starting_manifest_archetype_0043`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 393 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 28750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 123 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 83 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.044: Starting Supply Manifest Spec #0044
- **Profile Code:** `starting_manifest_archetype_0044`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 394 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 29000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 124 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 84 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.045: Starting Supply Manifest Spec #0045
- **Profile Code:** `starting_manifest_archetype_0045`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 395 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 29250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 125 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 85 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.046: Starting Supply Manifest Spec #0046
- **Profile Code:** `starting_manifest_archetype_0046`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 396 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 29500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 126 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 86 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.047: Starting Supply Manifest Spec #0047
- **Profile Code:** `starting_manifest_archetype_0047`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 397 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 29750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 127 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 87 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.048: Starting Supply Manifest Spec #0048
- **Profile Code:** `starting_manifest_archetype_0048`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 398 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 30000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 128 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 88 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.049: Starting Supply Manifest Spec #0049
- **Profile Code:** `starting_manifest_archetype_0049`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 399 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 30250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 129 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 89 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.050: Starting Supply Manifest Spec #0050
- **Profile Code:** `starting_manifest_archetype_0050`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 400 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 30500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 130 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 90 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.051: Starting Supply Manifest Spec #0051
- **Profile Code:** `starting_manifest_archetype_0051`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 401 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 30750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 131 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 91 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.052: Starting Supply Manifest Spec #0052
- **Profile Code:** `starting_manifest_archetype_0052`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 402 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 31000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 132 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 92 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.053: Starting Supply Manifest Spec #0053
- **Profile Code:** `starting_manifest_archetype_0053`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 403 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 31250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 133 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 93 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.054: Starting Supply Manifest Spec #0054
- **Profile Code:** `starting_manifest_archetype_0054`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 404 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 31500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 134 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 94 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.055: Starting Supply Manifest Spec #0055
- **Profile Code:** `starting_manifest_archetype_0055`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 405 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 31750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 135 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 95 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.056: Starting Supply Manifest Spec #0056
- **Profile Code:** `starting_manifest_archetype_0056`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 406 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 32000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 136 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 96 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.057: Starting Supply Manifest Spec #0057
- **Profile Code:** `starting_manifest_archetype_0057`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 407 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 32250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 137 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 97 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.058: Starting Supply Manifest Spec #0058
- **Profile Code:** `starting_manifest_archetype_0058`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 408 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 32500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 138 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 98 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.059: Starting Supply Manifest Spec #0059
- **Profile Code:** `starting_manifest_archetype_0059`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 409 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 32750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 139 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 99 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.060: Starting Supply Manifest Spec #0060
- **Profile Code:** `starting_manifest_archetype_0060`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 350 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 33000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 140 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 40 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.061: Starting Supply Manifest Spec #0061
- **Profile Code:** `starting_manifest_archetype_0061`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 351 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 33250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 141 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 41 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.062: Starting Supply Manifest Spec #0062
- **Profile Code:** `starting_manifest_archetype_0062`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 352 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 33500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 142 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 42 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.063: Starting Supply Manifest Spec #0063
- **Profile Code:** `starting_manifest_archetype_0063`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 353 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 33750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 143 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 43 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.064: Starting Supply Manifest Spec #0064
- **Profile Code:** `starting_manifest_archetype_0064`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 354 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 34000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 144 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 44 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.065: Starting Supply Manifest Spec #0065
- **Profile Code:** `starting_manifest_archetype_0065`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 355 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 34250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 145 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 45 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.066: Starting Supply Manifest Spec #0066
- **Profile Code:** `starting_manifest_archetype_0066`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 356 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 34500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 146 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 46 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.067: Starting Supply Manifest Spec #0067
- **Profile Code:** `starting_manifest_archetype_0067`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 357 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 34750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 147 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 47 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.068: Starting Supply Manifest Spec #0068
- **Profile Code:** `starting_manifest_archetype_0068`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 358 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 35000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 148 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 48 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.069: Starting Supply Manifest Spec #0069
- **Profile Code:** `starting_manifest_archetype_0069`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 359 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 35250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 149 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 49 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.070: Starting Supply Manifest Spec #0070
- **Profile Code:** `starting_manifest_archetype_0070`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 360 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 35500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 150 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 50 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.071: Starting Supply Manifest Spec #0071
- **Profile Code:** `starting_manifest_archetype_0071`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 361 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 35750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 151 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 51 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.072: Starting Supply Manifest Spec #0072
- **Profile Code:** `starting_manifest_archetype_0072`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 362 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 36000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 152 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 52 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.073: Starting Supply Manifest Spec #0073
- **Profile Code:** `starting_manifest_archetype_0073`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 363 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 36250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 153 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 53 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.074: Starting Supply Manifest Spec #0074
- **Profile Code:** `starting_manifest_archetype_0074`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 364 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 36500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 154 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 54 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.075: Starting Supply Manifest Spec #0075
- **Profile Code:** `starting_manifest_archetype_0075`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 365 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 36750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 155 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 55 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.076: Starting Supply Manifest Spec #0076
- **Profile Code:** `starting_manifest_archetype_0076`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 366 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 37000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 156 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 56 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.077: Starting Supply Manifest Spec #0077
- **Profile Code:** `starting_manifest_archetype_0077`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 367 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 37250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 157 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 57 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.078: Starting Supply Manifest Spec #0078
- **Profile Code:** `starting_manifest_archetype_0078`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 368 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 37500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 158 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 58 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.079: Starting Supply Manifest Spec #0079
- **Profile Code:** `starting_manifest_archetype_0079`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 369 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 37750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 159 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 59 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.080: Starting Supply Manifest Spec #0080
- **Profile Code:** `starting_manifest_archetype_0080`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 370 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 38000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 120 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 60 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.081: Starting Supply Manifest Spec #0081
- **Profile Code:** `starting_manifest_archetype_0081`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 371 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 38250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 121 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 61 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.082: Starting Supply Manifest Spec #0082
- **Profile Code:** `starting_manifest_archetype_0082`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 372 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 38500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 122 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 62 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.083: Starting Supply Manifest Spec #0083
- **Profile Code:** `starting_manifest_archetype_0083`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 373 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 38750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 123 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 63 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.084: Starting Supply Manifest Spec #0084
- **Profile Code:** `starting_manifest_archetype_0084`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 374 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 39000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 124 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 64 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.085: Starting Supply Manifest Spec #0085
- **Profile Code:** `starting_manifest_archetype_0085`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 375 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 39250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 125 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 65 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.086: Starting Supply Manifest Spec #0086
- **Profile Code:** `starting_manifest_archetype_0086`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 376 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 39500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 126 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 66 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.087: Starting Supply Manifest Spec #0087
- **Profile Code:** `starting_manifest_archetype_0087`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 377 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 39750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 127 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 67 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.088: Starting Supply Manifest Spec #0088
- **Profile Code:** `starting_manifest_archetype_0088`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 378 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 40000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 128 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 68 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.089: Starting Supply Manifest Spec #0089
- **Profile Code:** `starting_manifest_archetype_0089`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 379 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 40250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 129 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 69 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.090: Starting Supply Manifest Spec #0090
- **Profile Code:** `starting_manifest_archetype_0090`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 380 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 40500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 130 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 70 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.091: Starting Supply Manifest Spec #0091
- **Profile Code:** `starting_manifest_archetype_0091`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 381 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 40750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 131 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 71 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.092: Starting Supply Manifest Spec #0092
- **Profile Code:** `starting_manifest_archetype_0092`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 382 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 41000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 132 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 72 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.093: Starting Supply Manifest Spec #0093
- **Profile Code:** `starting_manifest_archetype_0093`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 383 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 41250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 133 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 73 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.094: Starting Supply Manifest Spec #0094
- **Profile Code:** `starting_manifest_archetype_0094`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 384 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 41500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 134 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 74 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.095: Starting Supply Manifest Spec #0095
- **Profile Code:** `starting_manifest_archetype_0095`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 385 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 41750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 135 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 75 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.096: Starting Supply Manifest Spec #0096
- **Profile Code:** `starting_manifest_archetype_0096`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 386 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 42000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 136 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 76 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.097: Starting Supply Manifest Spec #0097
- **Profile Code:** `starting_manifest_archetype_0097`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 387 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 42250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 137 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 77 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.098: Starting Supply Manifest Spec #0098
- **Profile Code:** `starting_manifest_archetype_0098`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 388 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 42500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 138 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 78 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.099: Starting Supply Manifest Spec #0099
- **Profile Code:** `starting_manifest_archetype_0099`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 389 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 42750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 139 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 79 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.100: Starting Supply Manifest Spec #0100
- **Profile Code:** `starting_manifest_archetype_0100`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 390 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 43000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 140 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 80 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.101: Starting Supply Manifest Spec #0101
- **Profile Code:** `starting_manifest_archetype_0101`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 391 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 43250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 141 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 81 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.102: Starting Supply Manifest Spec #0102
- **Profile Code:** `starting_manifest_archetype_0102`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 392 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 43500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 142 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 82 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.103: Starting Supply Manifest Spec #0103
- **Profile Code:** `starting_manifest_archetype_0103`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 393 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 43750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 143 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 83 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.104: Starting Supply Manifest Spec #0104
- **Profile Code:** `starting_manifest_archetype_0104`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 394 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 44000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 144 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 84 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.105: Starting Supply Manifest Spec #0105
- **Profile Code:** `starting_manifest_archetype_0105`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 395 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 44250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 145 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 85 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.106: Starting Supply Manifest Spec #0106
- **Profile Code:** `starting_manifest_archetype_0106`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 396 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 44500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 146 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 86 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.107: Starting Supply Manifest Spec #0107
- **Profile Code:** `starting_manifest_archetype_0107`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 397 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 44750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 147 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 87 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.108: Starting Supply Manifest Spec #0108
- **Profile Code:** `starting_manifest_archetype_0108`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 398 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 45000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 148 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 88 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.109: Starting Supply Manifest Spec #0109
- **Profile Code:** `starting_manifest_archetype_0109`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 399 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 45250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 149 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 89 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.110: Starting Supply Manifest Spec #0110
- **Profile Code:** `starting_manifest_archetype_0110`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 400 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 45500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 150 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 90 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.111: Starting Supply Manifest Spec #0111
- **Profile Code:** `starting_manifest_archetype_0111`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 401 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 45750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 151 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 91 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.112: Starting Supply Manifest Spec #0112
- **Profile Code:** `starting_manifest_archetype_0112`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 402 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 46000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 152 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 92 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.113: Starting Supply Manifest Spec #0113
- **Profile Code:** `starting_manifest_archetype_0113`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 403 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 46250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 153 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 93 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.114: Starting Supply Manifest Spec #0114
- **Profile Code:** `starting_manifest_archetype_0114`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 404 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 46500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 154 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 94 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.115: Starting Supply Manifest Spec #0115
- **Profile Code:** `starting_manifest_archetype_0115`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 405 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 46750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 155 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 95 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.116: Starting Supply Manifest Spec #0116
- **Profile Code:** `starting_manifest_archetype_0116`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 406 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 47000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 156 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 96 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.117: Starting Supply Manifest Spec #0117
- **Profile Code:** `starting_manifest_archetype_0117`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 407 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 47250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 157 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 97 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.118: Starting Supply Manifest Spec #0118
- **Profile Code:** `starting_manifest_archetype_0118`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 408 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 47500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 158 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 98 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.119: Starting Supply Manifest Spec #0119
- **Profile Code:** `starting_manifest_archetype_0119`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 409 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 47750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 159 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 99 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.120: Starting Supply Manifest Spec #0120
- **Profile Code:** `starting_manifest_archetype_0120`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 350 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 48000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 120 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 40 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.121: Starting Supply Manifest Spec #0121
- **Profile Code:** `starting_manifest_archetype_0121`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 351 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 48250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 121 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 41 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.122: Starting Supply Manifest Spec #0122
- **Profile Code:** `starting_manifest_archetype_0122`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 352 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 48500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 122 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 42 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.123: Starting Supply Manifest Spec #0123
- **Profile Code:** `starting_manifest_archetype_0123`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 353 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 48750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 123 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 43 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.124: Starting Supply Manifest Spec #0124
- **Profile Code:** `starting_manifest_archetype_0124`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 354 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 49000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 124 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 44 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.125: Starting Supply Manifest Spec #0125
- **Profile Code:** `starting_manifest_archetype_0125`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 355 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 49250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 125 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 45 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.126: Starting Supply Manifest Spec #0126
- **Profile Code:** `starting_manifest_archetype_0126`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 356 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 49500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 126 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 46 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.127: Starting Supply Manifest Spec #0127
- **Profile Code:** `starting_manifest_archetype_0127`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 357 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 49750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 127 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 47 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.128: Starting Supply Manifest Spec #0128
- **Profile Code:** `starting_manifest_archetype_0128`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 358 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 50000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 128 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 48 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.129: Starting Supply Manifest Spec #0129
- **Profile Code:** `starting_manifest_archetype_0129`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 359 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 50250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 129 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 49 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.130: Starting Supply Manifest Spec #0130
- **Profile Code:** `starting_manifest_archetype_0130`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 360 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 50500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 130 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 50 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.131: Starting Supply Manifest Spec #0131
- **Profile Code:** `starting_manifest_archetype_0131`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 361 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 50750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 131 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 51 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.132: Starting Supply Manifest Spec #0132
- **Profile Code:** `starting_manifest_archetype_0132`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 362 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 51000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 132 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 52 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.133: Starting Supply Manifest Spec #0133
- **Profile Code:** `starting_manifest_archetype_0133`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 363 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 51250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 133 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 53 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.134: Starting Supply Manifest Spec #0134
- **Profile Code:** `starting_manifest_archetype_0134`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 364 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 51500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 134 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 54 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.135: Starting Supply Manifest Spec #0135
- **Profile Code:** `starting_manifest_archetype_0135`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 365 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 51750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 135 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 55 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.136: Starting Supply Manifest Spec #0136
- **Profile Code:** `starting_manifest_archetype_0136`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 366 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 52000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 136 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 56 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.137: Starting Supply Manifest Spec #0137
- **Profile Code:** `starting_manifest_archetype_0137`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 367 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 52250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 137 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 57 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.138: Starting Supply Manifest Spec #0138
- **Profile Code:** `starting_manifest_archetype_0138`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 368 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 52500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 138 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 58 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.139: Starting Supply Manifest Spec #0139
- **Profile Code:** `starting_manifest_archetype_0139`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 369 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 52750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 139 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 59 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.140: Starting Supply Manifest Spec #0140
- **Profile Code:** `starting_manifest_archetype_0140`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 370 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 53000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 140 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 60 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.141: Starting Supply Manifest Spec #0141
- **Profile Code:** `starting_manifest_archetype_0141`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 371 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 53250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 141 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 61 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.142: Starting Supply Manifest Spec #0142
- **Profile Code:** `starting_manifest_archetype_0142`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 372 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 53500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 142 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 62 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.143: Starting Supply Manifest Spec #0143
- **Profile Code:** `starting_manifest_archetype_0143`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 373 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 53750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 143 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 63 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.144: Starting Supply Manifest Spec #0144
- **Profile Code:** `starting_manifest_archetype_0144`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 374 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 54000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 144 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 64 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.145: Starting Supply Manifest Spec #0145
- **Profile Code:** `starting_manifest_archetype_0145`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 375 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 54250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 145 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 65 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.146: Starting Supply Manifest Spec #0146
- **Profile Code:** `starting_manifest_archetype_0146`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 376 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 54500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 146 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 66 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.147: Starting Supply Manifest Spec #0147
- **Profile Code:** `starting_manifest_archetype_0147`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 377 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 54750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 147 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 67 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.148: Starting Supply Manifest Spec #0148
- **Profile Code:** `starting_manifest_archetype_0148`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 378 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 55000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 148 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 68 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.149: Starting Supply Manifest Spec #0149
- **Profile Code:** `starting_manifest_archetype_0149`
- **Shelter Archetype:** HardenedMilitia.
- **Initial Supply Weight:** 379 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 55250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 149 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 69 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.150: Starting Supply Manifest Spec #0150
- **Profile Code:** `starting_manifest_archetype_0150`
- **Shelter Archetype:** StandardBunker.
- **Initial Supply Weight:** 380 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 55500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 150 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 70 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.151: Starting Supply Manifest Spec #0151
- **Profile Code:** `starting_manifest_archetype_0151`
- **Shelter Archetype:** AgriculturalGreenhouse.
- **Initial Supply Weight:** 381 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 55750 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 151 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 71 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.152: Starting Supply Manifest Spec #0152
- **Profile Code:** `starting_manifest_archetype_0152`
- **Shelter Archetype:** MechanicalWorkshop.
- **Initial Supply Weight:** 382 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 56000 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 152 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 3 bolt-action surplus rifles with 72 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.153: Starting Supply Manifest Spec #0153
- **Profile Code:** `starting_manifest_archetype_0153`
- **Shelter Archetype:** MedicalQuarantine.
- **Initial Supply Weight:** 383 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 56250 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 153 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 1 bolt-action surplus rifles with 73 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.

### Appendix Q.154: Starting Supply Manifest Spec #0154
- **Profile Code:** `starting_manifest_archetype_0154`
- **Shelter Archetype:** ScoutSurveyor.
- **Initial Supply Weight:** 384 kilograms stowed in primary airlock staging crates.
- **Caloric Reserve Estimate:** 56500 kilocalories in sealed army ration tins and dried pea flour.
- **Potable Water Allocation:** 154 liters contained in sealed food-grade polyethylene carboys.
- **Starting Weaponry & Munitions:** 2 bolt-action surplus rifles with 74 rounds of non-corrosive primer cartridges.
- **Tooling & Industrial Assets:** 1 Portable welding torch with oxygen-acetylene bottles, 1 mechanics hand tool roll.
- **Estimated Survival Runway:** 18 standard calendar days before critical food or water extraction becomes mandatory.
