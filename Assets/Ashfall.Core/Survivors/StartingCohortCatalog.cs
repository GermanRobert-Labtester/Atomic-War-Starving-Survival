// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// One authored fresh-campaign cohort. Members are copied from the
    /// existing starting-survivor shape; this is not a survivor entity or
    /// continuing campaign state.
    /// </summary>
    [Serializable]
    public sealed class StartingCohortProfile
    {
        public string profile_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<StartingSurvivorDefinition> members = new List<StartingSurvivorDefinition>();

        public StartingCohortProfile Clone()
        {
            return new StartingCohortProfile
            {
                profile_id = profile_id,
                display_name = display_name,
                description = description,
                members = members
                    .Select(CloneMember)
                    .ToList()
            };
        }

        private static StartingSurvivorDefinition CloneMember(StartingSurvivorDefinition member)
        {
            return new StartingSurvivorDefinition
            {
                id = member.id,
                displayName = member.displayName,
                health = member.health,
                hunger = member.hunger,
                thirst = member.thirst,
                warmth = member.warmth,
                morale = member.morale,
                lifetimeDose = member.lifetimeDose,
                acuteRad = member.acuteRad,
                joinedDay = member.joinedDay
            };
        }
    }

    /// <summary>
    /// Validated, read-only starting-cohort catalog. The Standard profile is
    /// always retained from the legacy starting_survivors.json authority.
    /// </summary>
    public sealed class StartingCohortCatalog
    {
        public const string StandardProfileId = "cohort_standard_holdfast";

        private readonly List<StartingCohortProfile> _profiles;
        private readonly Dictionary<string, StartingCohortProfile> _byId;

        public StartingCohortCatalog(
            IEnumerable<StartingCohortProfile> profiles,
            string defaultProfileId)
        {
            _profiles = profiles?.Select(p => p.Clone()).ToList()
                ?? throw new ArgumentNullException(nameof(profiles));
            _byId = new Dictionary<string, StartingCohortProfile>(StringComparer.Ordinal);
            foreach (var profile in _profiles)
            {
                if (profile == null || string.IsNullOrEmpty(profile.profile_id))
                    continue;
                if (!_byId.ContainsKey(profile.profile_id))
                    _byId.Add(profile.profile_id, profile);
            }

            DefaultProfileId = !string.IsNullOrEmpty(defaultProfileId)
                ? defaultProfileId
                : StandardProfileId;
        }

        public IReadOnlyList<StartingCohortProfile> Profiles => _profiles;
        public string DefaultProfileId { get; }

        public StartingCohortProfile DefaultProfile =>
            TryGet(DefaultProfileId, out var profile)
                ? profile
                : TryGet(StandardProfileId, out profile)
                    ? profile
                    : throw new InvalidOperationException("Starting cohort catalog has no Standard profile.");

        public bool TryGet(string profileId, out StartingCohortProfile profile)
        {
            if (!string.IsNullOrEmpty(profileId) &&
                _byId.TryGetValue(profileId, out profile!))
                return true;

            profile = null!;
            return false;
        }
    }

    public sealed class StartingCohortCatalogLoadResult
    {
        public StartingCohortCatalog Catalog { get; internal set; } =
            new StartingCohortCatalog(Array.Empty<StartingCohortProfile>(), StartingCohortCatalog.StandardProfileId);
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }

    /// <summary>
    /// Loads the legacy starting roster first, then overlays validated
    /// alternate profiles from starting_survivor_cohorts.json. A malformed
    /// alternate file cannot replace the valid Standard profile.
    /// </summary>
    public static class StartingCohortCatalogLoader
    {
        public const string FileName = "starting_survivor_cohorts.json";
        private const int CurrentSchemaVersion = 1;

        public static StartingCohortCatalog Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            IEnumerable<SurvivorDefinition>? canonicalDefinitions = null)
        {
            return LoadDetailed(dataDir, fileIO, serializer, canonicalDefinitions).Catalog;
        }

        public static StartingCohortCatalogLoadResult LoadDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            IEnumerable<SurvivorDefinition>? canonicalDefinitions = null)
        {
            var result = new StartingCohortCatalogLoadResult();
            var canonical = (canonicalDefinitions ?? Array.Empty<SurvivorDefinition>())
                .Where(d => d != null && !string.IsNullOrEmpty(d.id))
                .ToDictionary(d => d.id, d => d, StringComparer.Ordinal);

            var legacy = SurvivorStartingStateLoader.LoadDetailed(
                dataDir,
                fileIO,
                serializer);

            if (!legacy.IsSuccess)
            {
                result.Errors.Add("Legacy starting roster is invalid: " + legacy.ErrorMessage);
                return result;
            }

            var standard = new StartingCohortProfile
            {
                profile_id = StartingCohortCatalog.StandardProfileId,
                display_name = "Standard Holdfast",
                description = "The unchanged legacy opening.",
                members = legacy.Survivors.Select(CloneMember).ToList()
            };

            ValidateMembers(
                standard,
                canonical,
                result.Errors,
                requireCanonical: canonical.Count > 0);

            var profiles = new List<StartingCohortProfile> { standard };
            string defaultProfileId = StartingCohortCatalog.StandardProfileId;
            string path = fileIO.Combine(dataDir, FileName);

            if (!fileIO.FileExists(path))
            {
                result.Catalog = new StartingCohortCatalog(profiles, defaultProfileId);
                return result;
            }

            StartingCohortRoot? root;
            try
            {
                root = serializer.Deserialize<StartingCohortRoot>(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Could not parse {FileName}: {ex.Message}");
                result.Catalog = new StartingCohortCatalog(profiles, defaultProfileId);
                return result;
            }

            if (root == null || root.profiles == null)
            {
                result.Errors.Add($"{FileName} has no profiles array.");
                result.Catalog = new StartingCohortCatalog(profiles, defaultProfileId);
                return result;
            }

            if (root.schema_version != CurrentSchemaVersion)
                result.Errors.Add($"{FileName} schema_version {root.schema_version} is not supported.");

            if (string.IsNullOrEmpty(root.default_profile_id))
                result.Errors.Add($"{FileName} has no default_profile_id.");
            else if (!string.Equals(
                root.default_profile_id,
                StartingCohortCatalog.StandardProfileId,
                StringComparison.Ordinal))
                result.Errors.Add(
                    $"{FileName} default_profile_id must remain '{StartingCohortCatalog.StandardProfileId}'.");
            else
                defaultProfileId = root.default_profile_id;

            var seenProfileIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.profiles.Count; i++)
            {
                var profile = root.profiles[i];
                if (profile == null || string.IsNullOrEmpty(profile.profile_id))
                {
                    result.Errors.Add($"{FileName} profile[{i}] has no profile_id.");
                    continue;
                }

                if (!seenProfileIds.Add(profile.profile_id))
                {
                    result.Errors.Add($"{FileName} has duplicate profile_id '{profile.profile_id}'.");
                    continue;
                }

                if (string.Equals(
                    profile.profile_id,
                    StartingCohortCatalog.StandardProfileId,
                    StringComparison.Ordinal))
                {
                    if (!ProfilesMatch(standard, profile))
                        result.Errors.Add("Standard Holdfast does not match starting_survivors.json.");
                    else
                        profiles[0] = profile.Clone();
                    continue;
                }

                var profileErrors = new List<string>();
                ValidateMembers(profile, canonical, profileErrors, requireCanonical: canonical.Count > 0);
                if (profileErrors.Count > 0)
                {
                    result.Errors.AddRange(profileErrors);
                    continue;
                }

                profiles.Add(profile.Clone());
            }

            result.Catalog = new StartingCohortCatalog(profiles, defaultProfileId);
            return result;
        }

        private static void ValidateMembers(
            StartingCohortProfile profile,
            IReadOnlyDictionary<string, SurvivorDefinition> canonical,
            ICollection<string> errors,
            bool requireCanonical)
        {
            if (profile.members == null || profile.members.Count != 3)
            {
                errors.Add($"Cohort '{profile.profile_id}' must contain exactly three members.");
                return;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < profile.members.Count; i++)
            {
                var member = profile.members[i];
                if (member == null || string.IsNullOrEmpty(member.id))
                {
                    errors.Add($"Cohort '{profile.profile_id}' member[{i}] has no survivor id.");
                    continue;
                }

                if (!seen.Add(member.id))
                    errors.Add($"Cohort '{profile.profile_id}' repeats survivor '{member.id}'.");

                SurvivorDefinition? definition = null;
                bool foundCanonical = !requireCanonical ||
                    canonical.TryGetValue(member.id, out definition);
                if (!foundCanonical)
                {
                    errors.Add($"Cohort '{profile.profile_id}' references unknown survivor '{member.id}'.");
                }
                else if (definition != null &&
                         !string.IsNullOrEmpty(definition.activeQuestlineId))
                {
                    errors.Add(
                        $"Cohort '{profile.profile_id}' survivor '{member.id}' owns later questline '{definition.activeQuestlineId}'.");
                }

                if (!ValidRange(member.health) ||
                    !ValidRange(member.hunger) ||
                    !ValidRange(member.thirst) ||
                    !ValidRange(member.warmth) ||
                    !ValidRange(member.morale) ||
                    !ValidNonNegative(member.lifetimeDose) ||
                    member.joinedDay < 0)
                {
                    errors.Add($"Cohort '{profile.profile_id}' survivor '{member.id}' has invalid initial values.");
                }
            }
        }

        private static bool ProfilesMatch(
            StartingCohortProfile standard,
            StartingCohortProfile authored)
        {
            if (authored.members == null || standard.members.Count != authored.members.Count)
                return false;

            for (int i = 0; i < standard.members.Count; i++)
            {
                var a = standard.members[i];
                var b = authored.members[i];
                if (!string.Equals(a.id, b.id, StringComparison.Ordinal) ||
                    !string.Equals(a.displayName, b.displayName, StringComparison.Ordinal) ||
                    a.health != b.health ||
                    a.hunger != b.hunger ||
                    a.thirst != b.thirst ||
                    a.warmth != b.warmth ||
                    a.morale != b.morale ||
                    a.lifetimeDose != b.lifetimeDose ||
                    a.acuteRad != b.acuteRad ||
                    a.joinedDay != b.joinedDay)
                    return false;
            }

            return true;
        }

        private static bool ValidRange(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value) && value >= 0f && value <= 100f;

        private static bool ValidNonNegative(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value) && value >= 0f;

        private static StartingSurvivorDefinition CloneMember(StartingSurvivorDefinition member)
        {
            return new StartingSurvivorDefinition
            {
                id = member.id,
                displayName = member.displayName,
                health = member.health,
                hunger = member.hunger,
                thirst = member.thirst,
                warmth = member.warmth,
                morale = member.morale,
                lifetimeDose = member.lifetimeDose,
                acuteRad = member.acuteRad,
                joinedDay = member.joinedDay
            };
        }

        [Serializable]
        private sealed class StartingCohortRoot
        {
            public int schema_version;
            public string default_profile_id = string.Empty;
            public List<StartingCohortProfile> profiles = new List<StartingCohortProfile>();
        }
    }
}
