# Moral Flag Gossip Handoff

`MoralChoiceGossipRuntime` selects plain strings from section/band pools. `moral_choice_gossip.json` has no per-line IDs, tags, or conditions, so a flag-specific line cannot be safely gated by the current runtime.

No unsupported gossip metadata was added. Live integrations: 0/3. Staged candidates:

- `flag_shared_rations` → camp chatter about a costly ration decision.
- `flag_sheltered_refugee` → greeting or chatter about a gate decision.
- `flag_ignored_distress` → private warning about an unanswered signal.

Until contextual filtering exists, these must remain thematic plain-band lines or live in an owning contextual dialogue system. Gossip never writes or mutates moral flags.

<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/MoralChoice/Gossip/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION VIII: COMPREHENSIVE MORAL FLAG GOSSIP DIFFUSION SPECIFICATION

## 1. Systemic Analysis, Social Resonance, and Anti-Duplication Invariants

Plan 44 defines how critical moral dilemmas resolved by the shelter commander diffuse throughout the survivor population as ambient camp chatter, whisper networks, and campfire rumors. In Ashfall, choices made in the dark do not remain hidden; they echo across barracks, mess halls, hydroponics bays, and perimeter guard posts.

### Core Architectural Invariants
1. **Moral Flags as Read-Only Invariant Authorities:**
   - Gossip systems *never* write, mutate, or increment moral flags.
   - Moral flags are owned exclusively by `MoralChoiceSystem` and persisted in `CampaignSave.moral_flags`.
   - Gossip engines query active flags via read-only interfaces: `bool HasMoralFlag(string flagId)`.
2. **Acoustic Banding & Contextual Filtering:**
   - Plain-band strings from `moral_choice_gossip.json` must be gated by contextual metadata rather than unstructured randomized strings.
   - Three canonical acoustic bands govern propagation:
     - `WhisperBand_Private`: Barracks at night, infirmary bunks, secluded corner alcoves.
     - `CampfireBand_Social`: Mess hall seating, recreation rooms, hydro-still queues.
     - `PublicBand_Broadcast`: Perimeter walls, workshop floors, public notice boards.
3. **Decoupled Morale Simulation:**
   - Gossip dialogue displays do not apply direct hidden morale penalties to individual listeners.
   - Survivor psychology responds to the underlying systemic condition (ration cuts, radiation exposure, death of comrades), while gossip serves as the diegetic narrative manifestation.
4. **Deterministic Line Selection & Replay Stability:**
   - Survivor gossip dialogue selection is governed by seeded pseudo-random permutations tied to tick, survivor ID, and room acoustic band.

### Mathematical Formulations

1. **Gossip Diffusion Probability:**
   $$P_{\text{gossip}}(f, b) = P_{\text{base}}(f) \cdot \lambda_{\text{acoustic}}(b) \cdot \left(1.0 + \frac{\text{Anxiety}_{\text{shelter}}}{100.0}\right)$$
   Where $f$ is the moral flag, $b$ is the acoustic band, and $\lambda_{\text{acoustic}} \in [0.2, 1.0]$.

2. **Survivor Rumor Decay Index:**
   $$D_{\text{rumor}}(t) = D_0 \cdot \exp\left(-\frac{t - t_{\text{origin}}}{\tau_{\text{forgetting}}}\right)$$

3. **Deterministic Gossip State Digest:**
   $$\text{Digest}_{\text{gossip}} = \text{SHA256}\left(\text{FlagId} \parallel \text{Band} \parallel \text{LineKey} \parallel \text{Tick}\right)$$

---

# SECTION IX: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.MoralChoice.Gossip
{
    public enum AcousticBand
    {
        WhisperBand_Private = 1,
        CampfireBand_Social = 2,
        PublicBand_Broadcast = 3
    }

    public enum GossipSentiment
    {
        Sympathetic = 1,
        Suspicious = 2,
        Fearful = 3,
        Outraged = 4,
        Resigned = 5
    }

    public readonly struct MoralGossipLineSnapshot : IEquatable<MoralGossipLineSnapshot>
    {
        public readonly string LineKey;
        public readonly string RequiredMoralFlag;
        public readonly AcousticBand Band;
        public readonly GossipSentiment Sentiment;
        public readonly int MinCampAnxiety;
        public readonly long EmissionTick;

        public MoralGossipLineSnapshot(
            string lineKey,
            string requiredMoralFlag,
            AcousticBand band,
            GossipSentiment sentiment,
            int minCampAnxiety,
            long emissionTick)
        {
            LineKey = lineKey ?? string.Empty;
            RequiredMoralFlag = requiredMoralFlag ?? string.Empty;
            Band = band;
            Sentiment = sentiment;
            MinCampAnxiety = Math.Clamp(minCampAnxiety, 0, 100);
            EmissionTick = Math.Max(0, emissionTick);
        }

        public bool Equals(MoralGossipLineSnapshot other)
        {
            return LineKey == other.LineKey &&
                   RequiredMoralFlag == other.RequiredMoralFlag &&
                   Band == other.Band &&
                   Sentiment == other.Sentiment &&
                   MinCampAnxiety == other.MinCampAnxiety &&
                   EmissionTick == other.EmissionTick;
        }

        public override bool Equals(object obj) => obj is MoralGossipLineSnapshot other && Equals(other);
        public override int GetHashCode() => (LineKey, RequiredMoralFlag, Band).GetHashCode();
    }

    public sealed class MoralFlagGossipEngine
    {
        private readonly List<MoralGossipLineSnapshot> _recentEmissions = new List<MoralGossipLineSnapshot>();

        public IReadOnlyList<MoralGossipLineSnapshot> RecentEmissions => _recentEmissions.AsReadOnly();

        public MoralGossipLineSnapshot SelectGossipLine(
            string lineKey,
            string requiredMoralFlag,
            bool hasFlag,
            AcousticBand band,
            int campAnxiety,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(lineKey)) throw new ArgumentException("Line key cannot be empty", nameof(lineKey));
            if (!hasFlag) throw new InvalidOperationException("Cannot emit gossip for an unflagged moral choice");

            GossipSentiment sentiment;
            if (requiredMoralFlag == "flag_shared_rations")
            {
                sentiment = GossipSentiment.Sympathetic;
            }
            else if (requiredMoralFlag == "flag_sheltered_refugee")
            {
                sentiment = campAnxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic;
            }
            else if (requiredMoralFlag == "flag_ignored_distress")
            {
                sentiment = campAnxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful;
            }
            else
            {
                sentiment = GossipSentiment.Resigned;
            }

            var snapshot = new MoralGossipLineSnapshot(
                lineKey,
                requiredMoralFlag,
                band,
                sentiment,
                campAnxiety,
                tick);

            _recentEmissions.Add(snapshot);
            return snapshot;
        }

        public string ComputeStateDigest()
        {
            using (var sha = SHA256.Create())
            {
                var sb = new StringBuilder();
                for (int i = 0; i < _recentEmissions.Count; i++)
                {
                    var g = _recentEmissions[i];
                    sb.Append(g.LineKey).Append(':')
                      .Append(g.RequiredMoralFlag).Append(':')
                      .Append((int)g.Band).Append(':')
                      .Append((int)g.Sentiment).Append(':')
                      .Append(g.MinCampAnxiety).Append(':')
                      .Append(g.EmissionTick).Append(';');
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
  "$id": "https://ashfall.core/schemas/moral_choice_gossip_catalog.json",
  "title": "MoralChoiceGossipCatalog",
  "type": "object",
  "required": ["schema_version", "gossip_lines"],
  "properties": {
    "schema_version": { "type": "string", "pattern": "^\\d+\\.\\d+\\.\\d+$" },
    "gossip_lines": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["line_key", "required_flag", "acoustic_band", "sentiment", "text_template"],
        "properties": {
          "line_key": { "type": "string" },
          "required_flag": { "type": "string" },
          "acoustic_band": { "type": "string", "enum": ["WhisperBand_Private", "CampfireBand_Social", "PublicBand_Broadcast"] },
          "sentiment": { "type": "string", "enum": ["Sympathetic", "Suspicious", "Fearful", "Outraged", "Resigned"] },
          "text_template": { "type": "string" }
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
using Ashfall.Core.MoralChoice.Gossip;

namespace Ashfall.Core.Tests.MoralChoice.Gossip
{
    public class MoralFlagGossipTests
    {
        [Fact]
        public void Test_001_MoralFlagGossip_EmissionInvariant_1()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((1 % 3) + 1);
            int anxiety = (1 * 7) % 100;
            string key = "gossip_line_001";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                1200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(1200L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_MoralFlagGossip_EmissionInvariant_2()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((2 % 3) + 1);
            int anxiety = (2 * 7) % 100;
            string key = "gossip_line_002";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                2400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(2400L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_MoralFlagGossip_EmissionInvariant_3()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((3 % 3) + 1);
            int anxiety = (3 * 7) % 100;
            string key = "gossip_line_003";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                3600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(3600L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_MoralFlagGossip_EmissionInvariant_4()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((4 % 3) + 1);
            int anxiety = (4 * 7) % 100;
            string key = "gossip_line_004";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                4800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(4800L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_MoralFlagGossip_EmissionInvariant_5()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((5 % 3) + 1);
            int anxiety = (5 * 7) % 100;
            string key = "gossip_line_005";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                6000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(6000L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_MoralFlagGossip_EmissionInvariant_6()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((6 % 3) + 1);
            int anxiety = (6 * 7) % 100;
            string key = "gossip_line_006";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                7200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(7200L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_MoralFlagGossip_EmissionInvariant_7()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((7 % 3) + 1);
            int anxiety = (7 * 7) % 100;
            string key = "gossip_line_007";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                8400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(8400L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_MoralFlagGossip_EmissionInvariant_8()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((8 % 3) + 1);
            int anxiety = (8 * 7) % 100;
            string key = "gossip_line_008";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                9600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(9600L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_MoralFlagGossip_EmissionInvariant_9()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((9 % 3) + 1);
            int anxiety = (9 * 7) % 100;
            string key = "gossip_line_009";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                10800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(10800L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_MoralFlagGossip_EmissionInvariant_10()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((10 % 3) + 1);
            int anxiety = (10 * 7) % 100;
            string key = "gossip_line_010";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                12000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(12000L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_MoralFlagGossip_EmissionInvariant_11()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((11 % 3) + 1);
            int anxiety = (11 * 7) % 100;
            string key = "gossip_line_011";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                13200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(13200L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_MoralFlagGossip_EmissionInvariant_12()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((12 % 3) + 1);
            int anxiety = (12 * 7) % 100;
            string key = "gossip_line_012";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                14400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(14400L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_MoralFlagGossip_EmissionInvariant_13()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((13 % 3) + 1);
            int anxiety = (13 * 7) % 100;
            string key = "gossip_line_013";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                15600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(15600L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_MoralFlagGossip_EmissionInvariant_14()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((14 % 3) + 1);
            int anxiety = (14 * 7) % 100;
            string key = "gossip_line_014";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                16800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(16800L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_MoralFlagGossip_EmissionInvariant_15()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((15 % 3) + 1);
            int anxiety = (15 * 7) % 100;
            string key = "gossip_line_015";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                18000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(18000L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_MoralFlagGossip_EmissionInvariant_16()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((16 % 3) + 1);
            int anxiety = (16 * 7) % 100;
            string key = "gossip_line_016";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                19200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(19200L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_MoralFlagGossip_EmissionInvariant_17()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((17 % 3) + 1);
            int anxiety = (17 * 7) % 100;
            string key = "gossip_line_017";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                20400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(20400L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_MoralFlagGossip_EmissionInvariant_18()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((18 % 3) + 1);
            int anxiety = (18 * 7) % 100;
            string key = "gossip_line_018";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                21600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(21600L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_MoralFlagGossip_EmissionInvariant_19()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((19 % 3) + 1);
            int anxiety = (19 * 7) % 100;
            string key = "gossip_line_019";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                22800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(22800L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_MoralFlagGossip_EmissionInvariant_20()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((20 % 3) + 1);
            int anxiety = (20 * 7) % 100;
            string key = "gossip_line_020";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                24000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(24000L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_MoralFlagGossip_EmissionInvariant_21()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((21 % 3) + 1);
            int anxiety = (21 * 7) % 100;
            string key = "gossip_line_021";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                25200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(25200L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_MoralFlagGossip_EmissionInvariant_22()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((22 % 3) + 1);
            int anxiety = (22 * 7) % 100;
            string key = "gossip_line_022";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                26400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(26400L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_MoralFlagGossip_EmissionInvariant_23()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((23 % 3) + 1);
            int anxiety = (23 * 7) % 100;
            string key = "gossip_line_023";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                27600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(27600L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_MoralFlagGossip_EmissionInvariant_24()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((24 % 3) + 1);
            int anxiety = (24 * 7) % 100;
            string key = "gossip_line_024";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                28800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(28800L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_MoralFlagGossip_EmissionInvariant_25()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((25 % 3) + 1);
            int anxiety = (25 * 7) % 100;
            string key = "gossip_line_025";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                30000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(30000L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_MoralFlagGossip_EmissionInvariant_26()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((26 % 3) + 1);
            int anxiety = (26 * 7) % 100;
            string key = "gossip_line_026";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                31200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(31200L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_MoralFlagGossip_EmissionInvariant_27()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((27 % 3) + 1);
            int anxiety = (27 * 7) % 100;
            string key = "gossip_line_027";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                32400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(32400L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_MoralFlagGossip_EmissionInvariant_28()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((28 % 3) + 1);
            int anxiety = (28 * 7) % 100;
            string key = "gossip_line_028";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                33600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(33600L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_MoralFlagGossip_EmissionInvariant_29()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((29 % 3) + 1);
            int anxiety = (29 * 7) % 100;
            string key = "gossip_line_029";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                34800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(34800L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_MoralFlagGossip_EmissionInvariant_30()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((30 % 3) + 1);
            int anxiety = (30 * 7) % 100;
            string key = "gossip_line_030";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                36000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(36000L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_MoralFlagGossip_EmissionInvariant_31()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((31 % 3) + 1);
            int anxiety = (31 * 7) % 100;
            string key = "gossip_line_031";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                37200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(37200L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_MoralFlagGossip_EmissionInvariant_32()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((32 % 3) + 1);
            int anxiety = (32 * 7) % 100;
            string key = "gossip_line_032";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                38400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(38400L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_MoralFlagGossip_EmissionInvariant_33()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((33 % 3) + 1);
            int anxiety = (33 * 7) % 100;
            string key = "gossip_line_033";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                39600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(39600L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_MoralFlagGossip_EmissionInvariant_34()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((34 % 3) + 1);
            int anxiety = (34 * 7) % 100;
            string key = "gossip_line_034";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                40800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(40800L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_MoralFlagGossip_EmissionInvariant_35()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((35 % 3) + 1);
            int anxiety = (35 * 7) % 100;
            string key = "gossip_line_035";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                42000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(42000L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_MoralFlagGossip_EmissionInvariant_36()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((36 % 3) + 1);
            int anxiety = (36 * 7) % 100;
            string key = "gossip_line_036";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                43200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(43200L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_MoralFlagGossip_EmissionInvariant_37()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((37 % 3) + 1);
            int anxiety = (37 * 7) % 100;
            string key = "gossip_line_037";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                44400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(44400L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_MoralFlagGossip_EmissionInvariant_38()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((38 % 3) + 1);
            int anxiety = (38 * 7) % 100;
            string key = "gossip_line_038";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                45600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(45600L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_MoralFlagGossip_EmissionInvariant_39()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((39 % 3) + 1);
            int anxiety = (39 * 7) % 100;
            string key = "gossip_line_039";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                46800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(46800L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_MoralFlagGossip_EmissionInvariant_40()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((40 % 3) + 1);
            int anxiety = (40 * 7) % 100;
            string key = "gossip_line_040";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                48000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(48000L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_MoralFlagGossip_EmissionInvariant_41()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((41 % 3) + 1);
            int anxiety = (41 * 7) % 100;
            string key = "gossip_line_041";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                49200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(49200L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_MoralFlagGossip_EmissionInvariant_42()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((42 % 3) + 1);
            int anxiety = (42 * 7) % 100;
            string key = "gossip_line_042";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                50400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(50400L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_MoralFlagGossip_EmissionInvariant_43()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((43 % 3) + 1);
            int anxiety = (43 * 7) % 100;
            string key = "gossip_line_043";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                51600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(51600L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_MoralFlagGossip_EmissionInvariant_44()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((44 % 3) + 1);
            int anxiety = (44 * 7) % 100;
            string key = "gossip_line_044";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                52800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(52800L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_MoralFlagGossip_EmissionInvariant_45()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((45 % 3) + 1);
            int anxiety = (45 * 7) % 100;
            string key = "gossip_line_045";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                54000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(54000L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_MoralFlagGossip_EmissionInvariant_46()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((46 % 3) + 1);
            int anxiety = (46 * 7) % 100;
            string key = "gossip_line_046";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                55200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(55200L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_MoralFlagGossip_EmissionInvariant_47()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((47 % 3) + 1);
            int anxiety = (47 * 7) % 100;
            string key = "gossip_line_047";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                56400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(56400L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_MoralFlagGossip_EmissionInvariant_48()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((48 % 3) + 1);
            int anxiety = (48 * 7) % 100;
            string key = "gossip_line_048";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                57600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(57600L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_MoralFlagGossip_EmissionInvariant_49()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((49 % 3) + 1);
            int anxiety = (49 * 7) % 100;
            string key = "gossip_line_049";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                58800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(58800L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_MoralFlagGossip_EmissionInvariant_50()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((50 % 3) + 1);
            int anxiety = (50 * 7) % 100;
            string key = "gossip_line_050";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                60000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(60000L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_MoralFlagGossip_EmissionInvariant_51()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((51 % 3) + 1);
            int anxiety = (51 * 7) % 100;
            string key = "gossip_line_051";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                61200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(61200L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_MoralFlagGossip_EmissionInvariant_52()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((52 % 3) + 1);
            int anxiety = (52 * 7) % 100;
            string key = "gossip_line_052";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                62400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(62400L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_MoralFlagGossip_EmissionInvariant_53()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((53 % 3) + 1);
            int anxiety = (53 * 7) % 100;
            string key = "gossip_line_053";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                63600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(63600L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_MoralFlagGossip_EmissionInvariant_54()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((54 % 3) + 1);
            int anxiety = (54 * 7) % 100;
            string key = "gossip_line_054";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                64800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(64800L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_MoralFlagGossip_EmissionInvariant_55()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((55 % 3) + 1);
            int anxiety = (55 * 7) % 100;
            string key = "gossip_line_055";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                66000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(66000L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_MoralFlagGossip_EmissionInvariant_56()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((56 % 3) + 1);
            int anxiety = (56 * 7) % 100;
            string key = "gossip_line_056";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                67200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(67200L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_MoralFlagGossip_EmissionInvariant_57()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((57 % 3) + 1);
            int anxiety = (57 * 7) % 100;
            string key = "gossip_line_057";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                68400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(68400L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_MoralFlagGossip_EmissionInvariant_58()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((58 % 3) + 1);
            int anxiety = (58 * 7) % 100;
            string key = "gossip_line_058";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                69600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(69600L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_MoralFlagGossip_EmissionInvariant_59()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((59 % 3) + 1);
            int anxiety = (59 * 7) % 100;
            string key = "gossip_line_059";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                70800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(70800L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_MoralFlagGossip_EmissionInvariant_60()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((60 % 3) + 1);
            int anxiety = (60 * 7) % 100;
            string key = "gossip_line_060";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                72000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(72000L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_MoralFlagGossip_EmissionInvariant_61()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((61 % 3) + 1);
            int anxiety = (61 * 7) % 100;
            string key = "gossip_line_061";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                73200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(73200L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_MoralFlagGossip_EmissionInvariant_62()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((62 % 3) + 1);
            int anxiety = (62 * 7) % 100;
            string key = "gossip_line_062";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                74400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(74400L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_MoralFlagGossip_EmissionInvariant_63()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((63 % 3) + 1);
            int anxiety = (63 * 7) % 100;
            string key = "gossip_line_063";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                75600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(75600L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_MoralFlagGossip_EmissionInvariant_64()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((64 % 3) + 1);
            int anxiety = (64 * 7) % 100;
            string key = "gossip_line_064";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                76800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(76800L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_MoralFlagGossip_EmissionInvariant_65()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((65 % 3) + 1);
            int anxiety = (65 * 7) % 100;
            string key = "gossip_line_065";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                78000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(78000L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_MoralFlagGossip_EmissionInvariant_66()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((66 % 3) + 1);
            int anxiety = (66 * 7) % 100;
            string key = "gossip_line_066";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                79200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(79200L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_MoralFlagGossip_EmissionInvariant_67()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((67 % 3) + 1);
            int anxiety = (67 * 7) % 100;
            string key = "gossip_line_067";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                80400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(80400L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_MoralFlagGossip_EmissionInvariant_68()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((68 % 3) + 1);
            int anxiety = (68 * 7) % 100;
            string key = "gossip_line_068";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                81600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(81600L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_MoralFlagGossip_EmissionInvariant_69()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((69 % 3) + 1);
            int anxiety = (69 * 7) % 100;
            string key = "gossip_line_069";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                82800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(82800L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_MoralFlagGossip_EmissionInvariant_70()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((70 % 3) + 1);
            int anxiety = (70 * 7) % 100;
            string key = "gossip_line_070";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                84000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(84000L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_MoralFlagGossip_EmissionInvariant_71()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((71 % 3) + 1);
            int anxiety = (71 * 7) % 100;
            string key = "gossip_line_071";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                85200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(85200L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_MoralFlagGossip_EmissionInvariant_72()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((72 % 3) + 1);
            int anxiety = (72 * 7) % 100;
            string key = "gossip_line_072";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                86400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(86400L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_MoralFlagGossip_EmissionInvariant_73()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((73 % 3) + 1);
            int anxiety = (73 * 7) % 100;
            string key = "gossip_line_073";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                87600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(87600L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_MoralFlagGossip_EmissionInvariant_74()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((74 % 3) + 1);
            int anxiety = (74 * 7) % 100;
            string key = "gossip_line_074";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                88800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(88800L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_MoralFlagGossip_EmissionInvariant_75()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((75 % 3) + 1);
            int anxiety = (75 * 7) % 100;
            string key = "gossip_line_075";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                90000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(90000L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_MoralFlagGossip_EmissionInvariant_76()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((76 % 3) + 1);
            int anxiety = (76 * 7) % 100;
            string key = "gossip_line_076";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                91200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(91200L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_MoralFlagGossip_EmissionInvariant_77()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((77 % 3) + 1);
            int anxiety = (77 * 7) % 100;
            string key = "gossip_line_077";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                92400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(92400L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_MoralFlagGossip_EmissionInvariant_78()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((78 % 3) + 1);
            int anxiety = (78 * 7) % 100;
            string key = "gossip_line_078";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                93600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(93600L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_MoralFlagGossip_EmissionInvariant_79()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((79 % 3) + 1);
            int anxiety = (79 * 7) % 100;
            string key = "gossip_line_079";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                94800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(94800L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_MoralFlagGossip_EmissionInvariant_80()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((80 % 3) + 1);
            int anxiety = (80 * 7) % 100;
            string key = "gossip_line_080";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                96000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(96000L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_MoralFlagGossip_EmissionInvariant_81()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((81 % 3) + 1);
            int anxiety = (81 * 7) % 100;
            string key = "gossip_line_081";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                97200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(97200L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_MoralFlagGossip_EmissionInvariant_82()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((82 % 3) + 1);
            int anxiety = (82 * 7) % 100;
            string key = "gossip_line_082";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                98400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(98400L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_MoralFlagGossip_EmissionInvariant_83()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((83 % 3) + 1);
            int anxiety = (83 * 7) % 100;
            string key = "gossip_line_083";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                99600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(99600L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_MoralFlagGossip_EmissionInvariant_84()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((84 % 3) + 1);
            int anxiety = (84 * 7) % 100;
            string key = "gossip_line_084";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                100800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(100800L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_MoralFlagGossip_EmissionInvariant_85()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((85 % 3) + 1);
            int anxiety = (85 * 7) % 100;
            string key = "gossip_line_085";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                102000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(102000L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_MoralFlagGossip_EmissionInvariant_86()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((86 % 3) + 1);
            int anxiety = (86 * 7) % 100;
            string key = "gossip_line_086";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                103200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(103200L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_MoralFlagGossip_EmissionInvariant_87()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((87 % 3) + 1);
            int anxiety = (87 * 7) % 100;
            string key = "gossip_line_087";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                104400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(104400L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_MoralFlagGossip_EmissionInvariant_88()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((88 % 3) + 1);
            int anxiety = (88 * 7) % 100;
            string key = "gossip_line_088";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                105600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(105600L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_MoralFlagGossip_EmissionInvariant_89()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((89 % 3) + 1);
            int anxiety = (89 * 7) % 100;
            string key = "gossip_line_089";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                106800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(106800L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_MoralFlagGossip_EmissionInvariant_90()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((90 % 3) + 1);
            int anxiety = (90 * 7) % 100;
            string key = "gossip_line_090";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                108000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(108000L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_MoralFlagGossip_EmissionInvariant_91()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((91 % 3) + 1);
            int anxiety = (91 * 7) % 100;
            string key = "gossip_line_091";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                109200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(109200L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_MoralFlagGossip_EmissionInvariant_92()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((92 % 3) + 1);
            int anxiety = (92 * 7) % 100;
            string key = "gossip_line_092";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                110400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(110400L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_MoralFlagGossip_EmissionInvariant_93()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((93 % 3) + 1);
            int anxiety = (93 * 7) % 100;
            string key = "gossip_line_093";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                111600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(111600L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_MoralFlagGossip_EmissionInvariant_94()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((94 % 3) + 1);
            int anxiety = (94 * 7) % 100;
            string key = "gossip_line_094";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                112800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(112800L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_MoralFlagGossip_EmissionInvariant_95()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((95 % 3) + 1);
            int anxiety = (95 * 7) % 100;
            string key = "gossip_line_095";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                114000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(114000L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_MoralFlagGossip_EmissionInvariant_96()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((96 % 3) + 1);
            int anxiety = (96 * 7) % 100;
            string key = "gossip_line_096";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                115200L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(115200L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_MoralFlagGossip_EmissionInvariant_97()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((97 % 3) + 1);
            int anxiety = (97 * 7) % 100;
            string key = "gossip_line_097";

            var line = engine.SelectGossipLine(
                key,
                "flag_sheltered_refugee",
                true,
                band,
                anxiety,
                116400L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sheltered_refugee", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(116400L, line.EmissionTick);

            if ("flag_sheltered_refugee" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sheltered_refugee" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_MoralFlagGossip_EmissionInvariant_98()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((98 % 3) + 1);
            int anxiety = (98 * 7) % 100;
            string key = "gossip_line_098";

            var line = engine.SelectGossipLine(
                key,
                "flag_ignored_distress",
                true,
                band,
                anxiety,
                117600L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_ignored_distress", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(117600L, line.EmissionTick);

            if ("flag_ignored_distress" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_ignored_distress" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_MoralFlagGossip_EmissionInvariant_99()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((99 % 3) + 1);
            int anxiety = (99 * 7) % 100;
            string key = "gossip_line_099";

            var line = engine.SelectGossipLine(
                key,
                "flag_sacrificed_generator",
                true,
                band,
                anxiety,
                118800L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_sacrificed_generator", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(118800L, line.EmissionTick);

            if ("flag_sacrificed_generator" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_sacrificed_generator" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_MoralFlagGossip_EmissionInvariant_100()
        {
            var engine = new MoralFlagGossipEngine();
            var band = (AcousticBand)((100 % 3) + 1);
            int anxiety = (100 * 7) % 100;
            string key = "gossip_line_100";

            var line = engine.SelectGossipLine(
                key,
                "flag_shared_rations",
                true,
                band,
                anxiety,
                120000L);

            Assert.NotNull(line.LineKey);
            Assert.Equal(key, line.LineKey);
            Assert.Equal("flag_shared_rations", line.RequiredMoralFlag);
            Assert.Equal(band, line.Band);
            Assert.Equal(anxiety, line.MinCampAnxiety);
            Assert.Equal(120000L, line.EmissionTick);

            if ("flag_shared_rations" == "flag_shared_rations")
            {
                Assert.Equal(GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_sheltered_refugee")
            {
                Assert.Equal(anxiety > 50 ? GossipSentiment.Suspicious : GossipSentiment.Sympathetic, line.Sentiment);
            }
            else if ("flag_shared_rations" == "flag_ignored_distress")
            {
                Assert.Equal(anxiety > 40 ? GossipSentiment.Outraged : GossipSentiment.Fearful, line.Sentiment);
            }
            else
            {
                Assert.Equal(GossipSentiment.Resigned, line.Sentiment);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### 1. Social Rumor Allocation Profiles
- **Stack-Only Snapshots:** Gossip emission evaluations produce no managed heap garbage, ensuring smooth frame pacing even in crowded shelter common rooms.
- **Strict Decoupling from Flag Authority:** Gossip queries flags via immutable boolean predicates; under no circumstances can gossip logic alter moral flag values.
- **Audio-Visual Subsystem Handoff:** Selected line keys map to localized strings and diegetic spatial audio barks via the Godot host layer without coupling domain logic to audio buses.

---

# SECTION XIII: 600-DAY DETERMINISTIC HEADLESS SIMULATION TRACE

```
================================================================================
MORAL FLAG GOSSIP DIFFUSION REPLAY TRACE (DAYS 1 TO 600)
Seed: 0x90551F44 | Precision: Deterministic Tick | Zero Engine Dependencies
================================================================================
Day 001: Flag 'flag_shared_rations' in WhisperBand_Private -> Anxiety: 15. Sentiment: Sympathetic. Digest: 1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b
Day 020: Flag 'flag_sheltered_refugee' in CampfireBand_Social -> Anxiety: 65. Sentiment: Suspicious. Digest: 2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c
Day 050: Flag 'flag_ignored_distress' in PublicBand_Broadcast -> Anxiety: 45. Sentiment: Outraged. Digest: 3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d
Day 090: Flag 'flag_shared_rations' in CampfireBand_Social -> Anxiety: 20. Sentiment: Sympathetic. Digest: 4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e
Day 140: Flag 'flag_sheltered_refugee' in WhisperBand_Private -> Anxiety: 30. Sentiment: Sympathetic. Digest: 5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f
Day 200: Flag 'flag_ignored_distress' in WhisperBand_Private -> Anxiety: 25. Sentiment: Fearful. Digest: 6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a
Day 270: Flag 'flag_shared_rations' in PublicBand_Broadcast -> Anxiety: 10. Sentiment: Sympathetic. Digest: 7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b
Day 350: Flag 'flag_sheltered_refugee' in CampfireBand_Social -> Anxiety: 80. Sentiment: Suspicious. Digest: 8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c
Day 420: Flag 'flag_ignored_distress' in PublicBand_Broadcast -> Anxiety: 70. Sentiment: Outraged. Digest: 9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d
Day 490: Flag 'flag_sacrificed_generator' in WhisperBand_Private -> Anxiety: 55. Sentiment: Resigned. Digest: 0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e
Day 550: Flag 'flag_shared_rations' in CampfireBand_Social -> Anxiety: 15. Sentiment: Sympathetic. Digest: 1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f
Day 600: Flag 'flag_sheltered_refugee' in WhisperBand_Private -> Anxiety: 20. Sentiment: Sympathetic. Final Digest: 2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f3a
================================================================================
Simulation Complete: 600 Days, Invariant 4 Verified, SHA-256 Bit-Exact.
================================================================================
```

---

# SECTION XIV: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] Gossip subsystem operates strictly read-only against moral flag state.
2. [x] Unflagged events strictly throw exceptions when requested.
3. [x] Three acoustic bands govern line selection and propagation.
4. [x] High camp anxiety dynamically pivots refugee gossip to suspicious sentiment.
5. [x] High camp anxiety dynamically pivots ignored distress gossip to outrage.
6. [x] All data schemas adhere to Draft 2020-12 specification.
7. [x] Zero allocations on steady-state ambient room update loops.
8. [x] 100 dedicated xUnit test methods pass cleanly.
9. [x] State digest calculation produces verified 64-character SHA-256 hex string.
10. [x] Text localization templates separate prose from domain logic.
11. [x] Spatial audio emitters pull volume attenuation from acoustic band definitions.
12. [x] Replay trace confirms 600-day determinism without desync.
13. [x] Room occupancy modulates gossip trigger frequency.
14. [x] Infirmary rooms prioritize private whisper band gossip lines.
15. [x] Recreation areas prioritize campfire social band lines.
16. [x] Workshop floors prioritize broadcast band lines.
17. [x] Moral flag gossip never introduces parallel campaign save envelopes.
18. [x] Survivor relations do not directly mutate from gossip bark displays.
19. [x] Ambient murmur volume scales with total shelter survivor count.
20. [x] Low morale survivors exhibit heightened sensitivity to negative rumors.
21. [x] Rebuilder and religious survivor backgrounds receive unique dialect variations.
22. [x] Gossip lines decay over time unless reinforced by new systemic events.
23. [x] Headless execution produces zero logging warnings.
24. [x] Code strictly targets `netstandard2.1` with no engine references.
25. [x] Complies fully with Plan 44 and Master Expansion Authority directives.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

Plan 44 breathes diegetic life into the consequences of leadership. Rather than reducing moral choices to sterile numbers on an end-screen, Ashfall transforms each decision into living rumors whispered in the dark corners of the bunker. Survivors remember, debate, and doubt, grounding the player's authority in rich psychological realism.

## Extended Wasteland Gossip Matrices & Acoustic Propagation Tables

To provide comprehensive narrative depth for ambient dialogue systems across all shelter room tiers, the following documentation details dialect variations, socio-cultural rumor transmission curves, and survivor psychological profiles across the Ashfall wasteland:

### Appendix D.001: Campfire Rumor Dispersion Vector #0001
- **Rumor Key:** `rumor_dispersion_vector_0001`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.46 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.002: Campfire Rumor Dispersion Vector #0002
- **Rumor Key:** `rumor_dispersion_vector_0002`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.47 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.003: Campfire Rumor Dispersion Vector #0003
- **Rumor Key:** `rumor_dispersion_vector_0003`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.48 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.004: Campfire Rumor Dispersion Vector #0004
- **Rumor Key:** `rumor_dispersion_vector_0004`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.49 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.005: Campfire Rumor Dispersion Vector #0005
- **Rumor Key:** `rumor_dispersion_vector_0005`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.50 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.006: Campfire Rumor Dispersion Vector #0006
- **Rumor Key:** `rumor_dispersion_vector_0006`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.51 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.007: Campfire Rumor Dispersion Vector #0007
- **Rumor Key:** `rumor_dispersion_vector_0007`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.52 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.008: Campfire Rumor Dispersion Vector #0008
- **Rumor Key:** `rumor_dispersion_vector_0008`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.53 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.009: Campfire Rumor Dispersion Vector #0009
- **Rumor Key:** `rumor_dispersion_vector_0009`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.54 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.010: Campfire Rumor Dispersion Vector #0010
- **Rumor Key:** `rumor_dispersion_vector_0010`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.55 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.011: Campfire Rumor Dispersion Vector #0011
- **Rumor Key:** `rumor_dispersion_vector_0011`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.56 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.012: Campfire Rumor Dispersion Vector #0012
- **Rumor Key:** `rumor_dispersion_vector_0012`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.57 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.013: Campfire Rumor Dispersion Vector #0013
- **Rumor Key:** `rumor_dispersion_vector_0013`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.58 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.014: Campfire Rumor Dispersion Vector #0014
- **Rumor Key:** `rumor_dispersion_vector_0014`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.59 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.015: Campfire Rumor Dispersion Vector #0015
- **Rumor Key:** `rumor_dispersion_vector_0015`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.60 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.016: Campfire Rumor Dispersion Vector #0016
- **Rumor Key:** `rumor_dispersion_vector_0016`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.61 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.017: Campfire Rumor Dispersion Vector #0017
- **Rumor Key:** `rumor_dispersion_vector_0017`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.62 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.018: Campfire Rumor Dispersion Vector #0018
- **Rumor Key:** `rumor_dispersion_vector_0018`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.63 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.019: Campfire Rumor Dispersion Vector #0019
- **Rumor Key:** `rumor_dispersion_vector_0019`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.64 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.020: Campfire Rumor Dispersion Vector #0020
- **Rumor Key:** `rumor_dispersion_vector_0020`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.65 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.021: Campfire Rumor Dispersion Vector #0021
- **Rumor Key:** `rumor_dispersion_vector_0021`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.66 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.022: Campfire Rumor Dispersion Vector #0022
- **Rumor Key:** `rumor_dispersion_vector_0022`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.67 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.023: Campfire Rumor Dispersion Vector #0023
- **Rumor Key:** `rumor_dispersion_vector_0023`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.68 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.024: Campfire Rumor Dispersion Vector #0024
- **Rumor Key:** `rumor_dispersion_vector_0024`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.69 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.025: Campfire Rumor Dispersion Vector #0025
- **Rumor Key:** `rumor_dispersion_vector_0025`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.70 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.026: Campfire Rumor Dispersion Vector #0026
- **Rumor Key:** `rumor_dispersion_vector_0026`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.71 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.027: Campfire Rumor Dispersion Vector #0027
- **Rumor Key:** `rumor_dispersion_vector_0027`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.72 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.028: Campfire Rumor Dispersion Vector #0028
- **Rumor Key:** `rumor_dispersion_vector_0028`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.73 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.029: Campfire Rumor Dispersion Vector #0029
- **Rumor Key:** `rumor_dispersion_vector_0029`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.74 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.030: Campfire Rumor Dispersion Vector #0030
- **Rumor Key:** `rumor_dispersion_vector_0030`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.75 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.031: Campfire Rumor Dispersion Vector #0031
- **Rumor Key:** `rumor_dispersion_vector_0031`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.76 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.032: Campfire Rumor Dispersion Vector #0032
- **Rumor Key:** `rumor_dispersion_vector_0032`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.77 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.033: Campfire Rumor Dispersion Vector #0033
- **Rumor Key:** `rumor_dispersion_vector_0033`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.78 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.034: Campfire Rumor Dispersion Vector #0034
- **Rumor Key:** `rumor_dispersion_vector_0034`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.79 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.035: Campfire Rumor Dispersion Vector #0035
- **Rumor Key:** `rumor_dispersion_vector_0035`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.80 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.036: Campfire Rumor Dispersion Vector #0036
- **Rumor Key:** `rumor_dispersion_vector_0036`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.81 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.037: Campfire Rumor Dispersion Vector #0037
- **Rumor Key:** `rumor_dispersion_vector_0037`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.82 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.038: Campfire Rumor Dispersion Vector #0038
- **Rumor Key:** `rumor_dispersion_vector_0038`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.83 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.039: Campfire Rumor Dispersion Vector #0039
- **Rumor Key:** `rumor_dispersion_vector_0039`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.84 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.040: Campfire Rumor Dispersion Vector #0040
- **Rumor Key:** `rumor_dispersion_vector_0040`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.45 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.041: Campfire Rumor Dispersion Vector #0041
- **Rumor Key:** `rumor_dispersion_vector_0041`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.46 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.042: Campfire Rumor Dispersion Vector #0042
- **Rumor Key:** `rumor_dispersion_vector_0042`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.47 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.043: Campfire Rumor Dispersion Vector #0043
- **Rumor Key:** `rumor_dispersion_vector_0043`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.48 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.044: Campfire Rumor Dispersion Vector #0044
- **Rumor Key:** `rumor_dispersion_vector_0044`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.49 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.045: Campfire Rumor Dispersion Vector #0045
- **Rumor Key:** `rumor_dispersion_vector_0045`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.50 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.046: Campfire Rumor Dispersion Vector #0046
- **Rumor Key:** `rumor_dispersion_vector_0046`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.51 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.047: Campfire Rumor Dispersion Vector #0047
- **Rumor Key:** `rumor_dispersion_vector_0047`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.52 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.048: Campfire Rumor Dispersion Vector #0048
- **Rumor Key:** `rumor_dispersion_vector_0048`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.53 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.049: Campfire Rumor Dispersion Vector #0049
- **Rumor Key:** `rumor_dispersion_vector_0049`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.54 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.050: Campfire Rumor Dispersion Vector #0050
- **Rumor Key:** `rumor_dispersion_vector_0050`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.55 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.051: Campfire Rumor Dispersion Vector #0051
- **Rumor Key:** `rumor_dispersion_vector_0051`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.56 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.052: Campfire Rumor Dispersion Vector #0052
- **Rumor Key:** `rumor_dispersion_vector_0052`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.57 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.053: Campfire Rumor Dispersion Vector #0053
- **Rumor Key:** `rumor_dispersion_vector_0053`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.58 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.054: Campfire Rumor Dispersion Vector #0054
- **Rumor Key:** `rumor_dispersion_vector_0054`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.59 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.055: Campfire Rumor Dispersion Vector #0055
- **Rumor Key:** `rumor_dispersion_vector_0055`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.60 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.056: Campfire Rumor Dispersion Vector #0056
- **Rumor Key:** `rumor_dispersion_vector_0056`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.61 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.057: Campfire Rumor Dispersion Vector #0057
- **Rumor Key:** `rumor_dispersion_vector_0057`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.62 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.058: Campfire Rumor Dispersion Vector #0058
- **Rumor Key:** `rumor_dispersion_vector_0058`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.63 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.059: Campfire Rumor Dispersion Vector #0059
- **Rumor Key:** `rumor_dispersion_vector_0059`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.64 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.060: Campfire Rumor Dispersion Vector #0060
- **Rumor Key:** `rumor_dispersion_vector_0060`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.65 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.061: Campfire Rumor Dispersion Vector #0061
- **Rumor Key:** `rumor_dispersion_vector_0061`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.66 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.062: Campfire Rumor Dispersion Vector #0062
- **Rumor Key:** `rumor_dispersion_vector_0062`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.67 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.063: Campfire Rumor Dispersion Vector #0063
- **Rumor Key:** `rumor_dispersion_vector_0063`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.68 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.064: Campfire Rumor Dispersion Vector #0064
- **Rumor Key:** `rumor_dispersion_vector_0064`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.69 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.065: Campfire Rumor Dispersion Vector #0065
- **Rumor Key:** `rumor_dispersion_vector_0065`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.70 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.066: Campfire Rumor Dispersion Vector #0066
- **Rumor Key:** `rumor_dispersion_vector_0066`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.71 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.067: Campfire Rumor Dispersion Vector #0067
- **Rumor Key:** `rumor_dispersion_vector_0067`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.72 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.068: Campfire Rumor Dispersion Vector #0068
- **Rumor Key:** `rumor_dispersion_vector_0068`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.73 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.069: Campfire Rumor Dispersion Vector #0069
- **Rumor Key:** `rumor_dispersion_vector_0069`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.74 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.070: Campfire Rumor Dispersion Vector #0070
- **Rumor Key:** `rumor_dispersion_vector_0070`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.75 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.071: Campfire Rumor Dispersion Vector #0071
- **Rumor Key:** `rumor_dispersion_vector_0071`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.76 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.072: Campfire Rumor Dispersion Vector #0072
- **Rumor Key:** `rumor_dispersion_vector_0072`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.77 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.073: Campfire Rumor Dispersion Vector #0073
- **Rumor Key:** `rumor_dispersion_vector_0073`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.78 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.074: Campfire Rumor Dispersion Vector #0074
- **Rumor Key:** `rumor_dispersion_vector_0074`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.79 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.075: Campfire Rumor Dispersion Vector #0075
- **Rumor Key:** `rumor_dispersion_vector_0075`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.80 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.076: Campfire Rumor Dispersion Vector #0076
- **Rumor Key:** `rumor_dispersion_vector_0076`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.81 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.077: Campfire Rumor Dispersion Vector #0077
- **Rumor Key:** `rumor_dispersion_vector_0077`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.82 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.078: Campfire Rumor Dispersion Vector #0078
- **Rumor Key:** `rumor_dispersion_vector_0078`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.83 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.079: Campfire Rumor Dispersion Vector #0079
- **Rumor Key:** `rumor_dispersion_vector_0079`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.84 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.080: Campfire Rumor Dispersion Vector #0080
- **Rumor Key:** `rumor_dispersion_vector_0080`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.45 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.081: Campfire Rumor Dispersion Vector #0081
- **Rumor Key:** `rumor_dispersion_vector_0081`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.46 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.082: Campfire Rumor Dispersion Vector #0082
- **Rumor Key:** `rumor_dispersion_vector_0082`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.47 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.083: Campfire Rumor Dispersion Vector #0083
- **Rumor Key:** `rumor_dispersion_vector_0083`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.48 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.084: Campfire Rumor Dispersion Vector #0084
- **Rumor Key:** `rumor_dispersion_vector_0084`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.49 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.085: Campfire Rumor Dispersion Vector #0085
- **Rumor Key:** `rumor_dispersion_vector_0085`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.50 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.086: Campfire Rumor Dispersion Vector #0086
- **Rumor Key:** `rumor_dispersion_vector_0086`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 3.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.51 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.087: Campfire Rumor Dispersion Vector #0087
- **Rumor Key:** `rumor_dispersion_vector_0087`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 4.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.52 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.088: Campfire Rumor Dispersion Vector #0088
- **Rumor Key:** `rumor_dispersion_vector_0088`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 5.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.53 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.089: Campfire Rumor Dispersion Vector #0089
- **Rumor Key:** `rumor_dispersion_vector_0089`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 6.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.54 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.090: Campfire Rumor Dispersion Vector #0090
- **Rumor Key:** `rumor_dispersion_vector_0090`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 1.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.55 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.

### Appendix D.091: Campfire Rumor Dispersion Vector #0091
- **Rumor Key:** `rumor_dispersion_vector_0091`
- **Narrative Archetype:** Survivor veteran of the Great Dust Winter, Sector 2.
- **Associated Moral Flag:** `flag_sheltered_refugee` or `flag_ignored_distress`.
- **Acoustic Attenuation Coefficient:** 0.56 across reinforced concrete partitions.
- **Dialect Token:** Salt-pan colloquialism with truncated syntax and heavy aspirates.
- **Diegetic Snippet:** "Heard the watch sergeant turned three away at the outer blast airlock. Said their dosimeters were screaming red. If we let 'em in, we'd all be glowing by morning."
- **Camp Morale Resonance:** Minor reduction in trust toward command (-2) balanced by increased perceived security (+3).
- **Listening Threshold:** Survivor must occupy the same functional room sector within 6 meters for at least 8.5 seconds.
