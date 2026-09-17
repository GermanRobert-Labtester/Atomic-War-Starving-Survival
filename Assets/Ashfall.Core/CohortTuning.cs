// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Plan 19B: Data-driven tuning parameters for Cohort generation, schooling,
    /// and child ration requirements. Authoritative source in JSON:
    /// Assets/StreamingAssets/Data/cohort_tuning.json.
    /// </summary>
    [Serializable]
    public sealed class CohortTuning
    {
        public int schema_version { get; set; } = 1;
        public float child_ration_fraction { get; set; } = 0.5f;
        public int schooling_age_days { get; set; } = 30;
        public int maturation_age_days { get; set; } = 180;
        public int max_active_apprentices { get; set; } = 3;

        public static CohortTuning Default { get; } = new CohortTuning();

        public static CohortTuning Load(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return Default;
            try
            {
                var ser = new SystemTextJsonSerializer();
                var loaded = ser.Deserialize<CohortTuning>(jsonContent);
                return loaded ?? Default;
            }
            catch
            {
                return Default;
            }
        }
    }
}
