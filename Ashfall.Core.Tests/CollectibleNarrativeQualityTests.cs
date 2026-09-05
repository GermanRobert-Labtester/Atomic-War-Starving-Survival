using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship XII — narrative quality and localization-readiness gates for the
    /// 40-item collectible corpus. Machine-enforceable editorial checks only:
    /// counts, non-empty text, name length/uniqueness, sentence ceilings,
    /// cliché frequencies, brand/slang/procedural-instruction blacklists, and
    /// generic codex-target resolution. Emotional-register distribution is an
    /// editorial judgement and lives in
    /// docs/narrative/COLLECTIBLES_NARRATIVE_QUALITY_AUDIT.md, not here.
    ///
    /// Localization model: catalog text is intentionally raw default-language
    /// strings in items.json (the canonical model used by every mature catalog);
    /// LocalizationService keys are UI chrome only. These gates pin the raw
    /// strings' quality so a later key-first migration starts from a clean corpus.
    /// </summary>
    public class CollectibleNarrativeQualityTests
    {
        private static readonly string DataDir = FindDataDir();
        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private sealed record CollectibleText(
            string ItemId, string Category, string EffectType, string EffectTarget,
            string DisplayName, string Description);

        private static List<CollectibleText> LoadCorpus()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
                ?? throw new InvalidOperationException("collectibles.json must load");

            string raw = FileIO.ReadAllText(Path.Combine(DataDir, "items.json"));
            using var json = JsonDocument.Parse(raw);
            var items = json.RootElement.GetProperty("items");

            var corpus = new List<CollectibleText>();
            foreach (var it in items.EnumerateArray())
            {
                string id = it.GetProperty("id").GetString() ?? "";
                var def = catalog.GetByItemId(id);
                if (def == null) continue;
                corpus.Add(new CollectibleText(
                    id, def.category, def.effect_type, def.effect_target,
                    it.GetProperty("displayName").GetString() ?? "",
                    it.GetProperty("description").GetString() ?? ""));
            }
            Assert.Equal(40, corpus.Count);
            return corpus;
        }

        /// <summary>Simple terminator-based sentence counter (project-approved;
        /// no NLP dependency): split on . ! ? followed by whitespace/end.</summary>
        private static int CountSentences(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Regex.Split(text, @"[.!?]+(?:\s|$)").Count(s => !string.IsNullOrWhiteSpace(s));
        }

        private static int CountTerm(string haystack, string needle) => Regex.Matches(
            haystack.ToLowerInvariant(), $@"\b{Regex.Escape(needle.ToLowerInvariant())}\b").Count;

        // ── Hard text gates ─────────────────────────────────────────

        [Fact]
        public void All40Descriptions_NonEmpty_AtMostThreeSentences()
        {
            var broken = LoadCorpus()
                .Where(c => string.IsNullOrWhiteSpace(c.Description) || CountSentences(c.Description) > 3)
                .Select(c => $"{c.ItemId}: sentences={CountSentences(c.Description)}")
                .ToList();
            Assert.True(broken.Count == 0, "description empty or >3 sentences:\n" + string.Join("\n", broken));
        }

        [Fact]
        public void All40Names_NonEmpty_AtMostFiftyChars_UniqueWithinCategory()
        {
            var corpus = LoadCorpus();
            var broken = corpus
                .Where(c => string.IsNullOrWhiteSpace(c.DisplayName) || c.DisplayName.Length > 50)
                .Select(c => $"{c.ItemId}: len={c.DisplayName.Length}")
                .ToList();

            var dupes = corpus
                .GroupBy(c => $"{c.Category}|{c.DisplayName}", StringComparer.Ordinal)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Assert.True(broken.Count == 0, "name empty or >50 chars:\n" + string.Join("\n", broken));
            Assert.True(dupes.Count == 0, "duplicate display name within category:\n" + string.Join("\n", dupes));
        }

        // ── Cliché ceilings (Flagship XII §1.5) ─────────────────────

        [Theory]
        [InlineData("faded", 2)]
        [InlineData("torn", 2)]
        [InlineData("bloodstained", 2)]
        [InlineData("haunting reminder", 2)]
        public void HardClicheTerms_StayWithinCeilings(string term, int ceiling)
        {
            string all = string.Join("\n", LoadCorpus().Select(c => c.Description));
            int uses = CountTerm(all, term);
            Assert.True(uses <= ceiling, $"cliché '{term}' used {uses}x (ceiling {ceiling})");
        }

        // ── IP / brand / slang / procedural gates (§1.9, §1.10, §4.10) ──

        private static readonly string[] ForbiddenRealWorldTerms =
        {
            "coca-cola", "pepsi", "nike", "adidas", "ford", "chevrolet", "toyota",
            "sony", "nintendo", "playstation", "xbox", "harley-davidson",
            "new york times", "wall street journal", "rolling stone", "forbes",
            "yankees", "dodgers", "lakers", "manchester united", "fifa", "nasa",
            "google", "facebook", "instagram", "tiktok"
        };

        [Fact]
        public void Descriptions_ContainNoRealBrandsPublicationsOrTeams()
        {
            string all = string.Join("\n", LoadCorpus().Select(c => c.Description + "\n" + c.DisplayName));
            var offenders = ForbiddenRealWorldTerms.Where(t => all.Contains(t, StringComparison.OrdinalIgnoreCase)).ToList();
            Assert.True(offenders.Count == 0, "real-world terms found: " + string.Join(", ", offenders));
        }

        private static readonly string[] ForbiddenModernSlang =
        {
            "selfie", "meme", "livestream", "hashtag", "influencer", "podcast",
            "emoji", "vlog", "ghosting", "vibe check", "no cap", "rizz", "yeet"
        };

        [Fact]
        public void Descriptions_ContainNoModernInternetSlang()
        {
            string all = string.Join("\n", LoadCorpus().Select(c => c.Description));
            var offenders = ForbiddenModernSlang.Where(t => CountTerm(all, t) > 0).ToList();
            Assert.True(offenders.Count == 0, "modern slang found: " + string.Join(", ", offenders));
        }

        private static readonly string[] ForbiddenProceduralTerms =
        {
            "step 1", "step one", "detonator", "gunpowder", "enriched uranium",
            "nerve agent", "mustard gas", "pipe bomb", "explosive lens",
            "critical mass", "how to build", "instructions for making"
        };

        [Fact]
        public void Descriptions_ContainNoProceduralConstructionOrHazardInstructions()
        {
            var corpus = LoadCorpus();
            string all = string.Join("\n", corpus.Select(c => c.Description));
            var offenders = ForbiddenProceduralTerms.Where(t => all.Contains(t, StringComparison.OrdinalIgnoreCase)).ToList();

            // Numbered instruction sequences ("1. … 2. …") read as procedures,
            // not prose.
            var numbered = corpus
                .Where(c => Regex.IsMatch(c.Description, @"\b\d\.[\s]") && CountTerm(c.Description, "then") > 0)
                .Select(c => c.ItemId)
                .ToList();

            Assert.True(offenders.Count == 0, "procedural/hazard terms found: " + string.Join(", ", offenders));
            Assert.True(numbered.Count == 0, "numbered instruction sequences in: " + string.Join(", ", numbered));
        }

        // ── Codex content gates (§2.10/§3.11 generic; entry contract §2.3) ──

        private static JournalVoiceProseCatalog LoadProse()
        {
            var loader = new JournalVoiceProseCatalogLoader(FileIO, Serializer);
            var catalog = loader.Load(DataDir);
            Assert.True(catalog.Count > 0, "journal_voice_prose.json must load");
            return catalog;
        }

        [Fact]
        public void JournalAndFactionTargets_ResolveAgainstProseAuthority()
        {
            var prose = LoadProse();
            var broken = LoadCorpus()
                .Where(c => c.EffectType == "journal_unlock" || c.EffectType == "faction_info")
                .Where(c => string.IsNullOrWhiteSpace(c.EffectTarget) || !prose.HasKey(c.EffectTarget))
                .Select(c => $"{c.ItemId} -> {c.EffectTarget}")
                .ToList();
            Assert.True(broken.Count == 0,
                "journal_unlock/faction_info targets must resolve to authored prose keys:\n" + string.Join("\n", broken));
        }

        [Fact]
        public void CodexTargets_DefaultAndRealistProse_AreTwoToFourSentences()
        {
            // The collectible dispatcher is the only live discovery path for
            // these keys and passes author:null, which JournalSystem resolves
            // to the Realist voice. Default covers the fallback. Both must
            // satisfy the 2–4 sentence entry contract.
            var prose = LoadProse();
            var targets = LoadCorpus()
                .Where(c => c.EffectType == "journal_unlock" || c.EffectType == "faction_info")
                .Select(c => c.EffectTarget)
                .Distinct()
                .ToList();

            var broken = new List<string>();
            foreach (string target in targets)
            {
                var entry = prose.GetEntry(target);
                if (entry == null) { broken.Add($"{target}: missing"); continue; }
                int realist = CountSentences(entry.GetProseForBias(RiskBiasTrait.Realist));
                int def = CountSentences(entry.@default);
                if (realist is < 2 or > 4) broken.Add($"{target}: realist={realist} sentences");
                if (def is < 2 or > 4) broken.Add($"{target}: default={def} sentences");
            }
            Assert.True(broken.Count == 0, "entry contract violations:\n" + string.Join("\n", broken));
        }
    }
}
