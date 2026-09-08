using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class StartingCohortCatalogTests
    {
        private readonly string _dataDir;
        private readonly string _legacyJson;
        private readonly string _cohortJson;
        private readonly List<SurvivorDefinition> _canonical;

        public StartingCohortCatalogTests()
        {
            _dataDir = ResolveDataDir();
            _legacyJson = File.ReadAllText(Path.Combine(_dataDir, SurvivorStartingStateLoader.FileName));
            _cohortJson = File.ReadAllText(Path.Combine(_dataDir, StartingCohortCatalogLoader.FileName));
            _canonical = SurvivorCatalogLoader.Load(
                _dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());
        }

        [Fact]
        public void AuthoritativeCatalog_HasSixProfilesAndThreeMembersEach()
        {
            var result = Load(_cohortJson, _canonical);

            Assert.True(result.IsSuccess, string.Join(Environment.NewLine, result.Errors));
            Assert.Equal(6, result.Catalog.Profiles.Count);
            Assert.Equal(
                StartingCohortCatalog.StandardProfileId,
                result.Catalog.DefaultProfileId);
            Assert.All(result.Catalog.Profiles, profile =>
            {
                Assert.Equal(3, profile.members.Count);
                Assert.Equal(
                    3,
                    profile.members.Select(member => member.id).Distinct(StringComparer.Ordinal).Count());
            });
        }

        [Fact]
        public void StandardProfile_IsExactLegacyParity()
        {
            var legacy = SurvivorStartingStateLoader.Load(
                _dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());
            var result = Load(_cohortJson, _canonical);

            Assert.True(result.IsSuccess, string.Join(Environment.NewLine, result.Errors));
            Assert.True(result.Catalog.TryGet(
                StartingCohortCatalog.StandardProfileId,
                out var standard));
            Assert.Equal(legacy.Count, standard.members.Count);
            for (int i = 0; i < legacy.Count; i++)
            {
                Assert.Equal(legacy[i].id, standard.members[i].id);
                Assert.Equal(legacy[i].displayName, standard.members[i].displayName);
                Assert.Equal(legacy[i].health, standard.members[i].health);
                Assert.Equal(legacy[i].hunger, standard.members[i].hunger);
                Assert.Equal(legacy[i].thirst, standard.members[i].thirst);
                Assert.Equal(legacy[i].warmth, standard.members[i].warmth);
                Assert.Equal(legacy[i].morale, standard.members[i].morale);
                Assert.Equal(legacy[i].lifetimeDose, standard.members[i].lifetimeDose);
                Assert.Equal(legacy[i].acuteRad, standard.members[i].acuteRad);
                Assert.Equal(legacy[i].joinedDay, standard.members[i].joinedDay);
            }
        }

        [Fact]
        public void DuplicateProfileId_IsRejectedWithoutDroppingStandard()
        {
            string invalid = _cohortJson.Replace(
                "\"cohort_repair_crew\"",
                "\"cohort_triage_ward\"");
            var result = Load(invalid, _canonical);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("duplicate profile_id"));
            Assert.True(result.Catalog.TryGet(
                StartingCohortCatalog.StandardProfileId,
                out _));
            Assert.True(result.Catalog.TryGet("cohort_triage_ward", out _));
            Assert.False(result.Catalog.TryGet("cohort_repair_crew", out _));
        }

        [Fact]
        public void DuplicateMemberId_IsRejectedAndAlternateIsolated()
        {
            string invalid = _cohortJson.Replace(
                "\"riley_cooper\"",
                "\"ariana_cruz\"");
            var result = Load(invalid, _canonical);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("repeats survivor"));
            Assert.True(result.Catalog.TryGet(
                StartingCohortCatalog.StandardProfileId,
                out _));
            Assert.False(result.Catalog.TryGet("cohort_triage_ward", out _));
            Assert.True(result.Catalog.TryGet("cohort_repair_crew", out _));
        }

        [Fact]
        public void UnknownCanonicalId_IsRejectedAndAlternateIsolated()
        {
            string invalid = _cohortJson.Replace(
                "\"riley_cooper\"",
                "\"survivor_not_in_catalog\"");
            var result = Load(invalid, _canonical);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("unknown survivor"));
            Assert.True(result.Catalog.TryGet(
                StartingCohortCatalog.StandardProfileId,
                out _));
            Assert.False(result.Catalog.TryGet("cohort_triage_ward", out _));
            Assert.True(result.Catalog.TryGet("cohort_repair_crew", out _));
        }

        [Fact]
        public void ActiveQuestlineOwner_IsRejectedAndAlternateIsolated()
        {
            var canonicalWithQuestOwner = _canonical
                .Select(definition => new SurvivorDefinition
                {
                    id = definition.id,
                    displayName = definition.displayName,
                    profession = definition.profession,
                    bio = definition.bio,
                    baseHealth = definition.baseHealth,
                    activeQuestlineId = definition.id == "riley_cooper"
                        ? "quest_later_reveal"
                        : definition.activeQuestlineId,
                    traitIds = definition.traitIds.ToList()
                })
                .ToList();
            var result = Load(_cohortJson, canonicalWithQuestOwner);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("later questline"));
            Assert.False(result.Catalog.TryGet("cohort_triage_ward", out _));
            Assert.True(result.Catalog.TryGet("cohort_repair_crew", out _));
        }

        [Fact]
        public void InvalidInitialValues_AreRejectedAndAlternateIsolated()
        {
            string invalid = _cohortJson.Replace(
                "\"health\": 86.0",
                "\"health\": -1.0");
            var result = Load(invalid, _canonical);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("invalid initial values"));
            Assert.False(result.Catalog.TryGet("cohort_triage_ward", out _));
            Assert.True(result.Catalog.TryGet("cohort_repair_crew", out _));
        }

        [Fact]
        public void MissingDefaultProfile_IsRejectedButStandardRemainsAvailable()
        {
            string invalid = _cohortJson.Replace(
                "\"default_profile_id\": \"cohort_standard_holdfast\"",
                "\"default_profile_id\": \"\"");
            var result = Load(invalid, _canonical);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("no default_profile_id"));
            Assert.True(result.Catalog.TryGet(
                StartingCohortCatalog.StandardProfileId,
                out _));
        }

        [Fact]
        public void LegacyOnlyData_FallsBackToStandardProfile()
        {
            var files = new MemoryFileIO();
            files.Put("starting_survivors.json", _legacyJson);
            var result = StartingCohortCatalogLoader.LoadDetailed(
                "memory",
                files,
                new SystemTextJsonSerializer(),
                _canonical);

            Assert.True(result.IsSuccess, string.Join(Environment.NewLine, result.Errors));
            Assert.Single(result.Catalog.Profiles);
            Assert.Equal(
                StartingCohortCatalog.StandardProfileId,
                result.Catalog.DefaultProfileId);
        }

        private StartingCohortCatalogLoadResult Load(
            string cohortJson,
            IEnumerable<SurvivorDefinition> canonical)
        {
            var files = new MemoryFileIO();
            files.Put("starting_survivors.json", _legacyJson);
            files.Put(StartingCohortCatalogLoader.FileName, cohortJson);
            return StartingCohortCatalogLoader.LoadDetailed(
                "memory",
                files,
                new SystemTextJsonSerializer(),
                canonical);
        }

        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            return Path.GetFullPath(Path.Combine(
                baseDir,
                "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data"));
        }

        private sealed class MemoryFileIO : IFileIO
        {
            private readonly Dictionary<string, string> _files =
                new Dictionary<string, string>(StringComparer.Ordinal);

            public void Put(string fileName, string contents) =>
                _files[Combine("memory", fileName)] = contents;

            public bool DirectoryExists(string path) => true;

            public bool FileExists(string path) => _files.ContainsKey(path);

            public string ReadAllText(string path) => _files[path];

            public void WriteAllText(string path, string contents) => _files[path] = contents;

            public string Combine(params string[] parts) =>
                string.Join("/", parts.Where(part => !string.IsNullOrEmpty(part)));
        }
    }
}
