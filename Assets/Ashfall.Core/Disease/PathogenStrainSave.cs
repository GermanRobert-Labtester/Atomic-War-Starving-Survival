using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Disease
{
    [Serializable]
    public class PathogenCureProjectSaveEntry
    {
        public string strainId = string.Empty;
        public int startedDay;
        public int daysInvested;
        public int requiredDays = 10;
        public bool complete;
    }

    /// <summary>
    /// ASHFALL — pathogen strain save state (Version 1). Owns the strain layer's
    /// authoritative state: fictional cure projects and completed cures. Mutation
    /// results persist inside the canonical disease engine's own infection state,
    /// not here. Versioned + checksummed via <see cref="PathogenStrainSaveCodec"/>.
    /// </summary>
    [Serializable]
    public class PathogenStrainSaveState
    {
        public int saveVersion = PathogenStrainSaveCodec.CurrentSaveVersion;
        public List<PathogenCureProjectSaveEntry> cureProjects = new List<PathogenCureProjectSaveEntry>();
        public List<string> curedStrainIds = new List<string>();

        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Pathogen strain save codec: checksum recomputed on encode, hard-reject on
    /// decode for tamper / checksumless / newer-version payloads (mirrors
    /// <see cref="MoraleContagionSaveCodec"/>). Old saves without this section
    /// load as "no cure projects, no unlocked cures".
    /// </summary>
    public static class PathogenStrainSaveCodec
    {
        public const int CurrentSaveVersion = 1;

        public static string Encode(PathogenStrainSaveState state, IJsonSerializer json)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (json == null) throw new ArgumentNullException(nameof(json));
            state.saveVersion = CurrentSaveVersion;
            state.Checksum = SaveChecksum.Compute(state);
            return json.Serialize(state);
        }

        public static bool TryDecode(string json, IJsonSerializer serializer, out PathogenStrainSaveState state)
        {
            state = null!;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<PathogenStrainSaveState>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > CurrentSaveVersion) return false;  // future — reject
                if (decoded.saveVersion < CurrentSaveVersion) return false;  // no older format exists

                if (string.IsNullOrEmpty(decoded.Checksum)) return false;    // malformed new format — reject
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;                                            // tampered

                if (decoded.cureProjects == null) decoded.cureProjects = new List<PathogenCureProjectSaveEntry>();
                if (decoded.curedStrainIds == null) decoded.curedStrainIds = new List<string>();
                state = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<decode>", "PathogenStrainSaveState", ex_CATDIAG);
                return false;
            }
        }

        public static PathogenStrainSaveState ToSaveState(PathogenStrainSystemState state)
        {
            var save = new PathogenStrainSaveState();
            foreach (var project in state.cureProjects)
                save.cureProjects.Add(new PathogenCureProjectSaveEntry
                {
                    strainId = project.strainId,
                    startedDay = project.startedDay,
                    daysInvested = project.daysInvested,
                    requiredDays = project.requiredDays,
                    complete = project.complete
                });
            save.curedStrainIds.AddRange(state.curedStrainIds);
            return save;
        }

        public static PathogenStrainSystemState FromSaveState(PathogenStrainSaveState save)
        {
            var state = new PathogenStrainSystemState();
            foreach (var project in save.cureProjects)
                state.cureProjects.Add(new PathogenCureProjectState
                {
                    strainId = project.strainId,
                    startedDay = project.startedDay,
                    daysInvested = project.daysInvested,
                    requiredDays = project.requiredDays,
                    complete = project.complete
                });
            state.curedStrainIds.AddRange(save.curedStrainIds);
            return state;
        }
    }
}
