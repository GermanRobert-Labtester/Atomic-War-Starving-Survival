// SPDX-License-Identifier: MIT
using System.IO;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// D1 (Wave 8 Part 2) — shared fixture seed for isolated
    /// <see cref="Ashfall.Core.CatalogIntegrityValidator"/> scratch-directory
    /// tests.
    ///
    /// Rationale: the validator gained whole-directory mandatory-catalog checks
    /// (Plan 14A trade embargoes, Plan 14B regional prices, Plan 24A duty roles)
    /// after these rule-isolation tests were written. The shipped data directory
    /// passes; the scratch fixtures legitimately exercise single rules on a
    /// minimal directory, so they must declare the mandatory catalogs to reach
    /// the rule under test. Stubs are minimal-valid and self-contained (no
    /// cross-file refs beyond code-owned role ids / empty vocabularies), so they
    /// neither cascade into other catalogs nor mask a probe error.
    /// </summary>
    internal static class IntegrityScratchFixture
    {
        /// <summary>Trade embargoes — empty valid root (no rules ⇒ no goods refs).</summary>
        private const string TradeEmbargoes =
            "{\"schema_version\":1,\"collection_id\":\"trade_embargoes_14a\",\"rules\":[]}";

        /// <summary>Regional prices — empty valid root (no entries ⇒ no goods refs).</summary>
        private const string RegionalPrices =
            "{\"schema_version\":1,\"collection_id\":\"regional_prices_14b\",\"entries\":[]}";

        /// <summary>
        /// Duty roles — all live assignment roles with empty skill ids (the
        /// validator skips the skills.json resolution when skill_id is empty)
        /// and the authored thresholds. Code-owned hazard classes only.
        /// </summary>
        private const string DutyRoles =
            "{\n" +
            "  \"schema_version\": 1,\n" +
            "  \"collection_id\": \"duty_roles\",\n" +
            "  \"thresholds\": {\n" +
            "    \"fatigue_impaired\": 60.0, \"fatigue_unfit\": 85.0,\n" +
            "    \"health_impaired\": 60.0, \"health_unfit\": 30.0,\n" +
            "    \"hunger_impaired\": 60.0, \"hunger_unfit\": 85.0,\n" +
            "    \"thirst_impaired\": 60.0, \"thirst_unfit\": 85.0,\n" +
            "    \"warmth_impaired\": 40.0, \"warmth_unfit\": 20.0,\n" +
            "    \"days_without_sleep_impaired\": 2, \"days_without_sleep_unfit\": 4,\n" +
            "    \"discharge_recovery_days\": 2,\n" +
            "    \"sick_band_impaired\": 1, \"sick_band_unfit\": 2, \"sick_band_incapacitated\": 3,\n" +
            "    \"dose_impaired_msv\": 100.0, \"dose_unfit_msv\": 300.0\n" +
            "  },\n" +
            "  \"roles\": [\n" +
            "    {\"id\":\"night_watch\",\"skill_id\":\"\",\"minimum_skill\":0.0,\"maximum_fatigue\":90.0,\"minimum_health\":30.0,\"maximum_dose_msv\":300.0,\"maximum_hours\":12.0,\"maximum_hours_if_impaired\":8.0,\"allow_unfit\":false,\"light_duty\":false,\"requires_not_quarantined\":true,\"precision_work\":true,\"hazard_class\":\"perimeter\"},\n" +
            "    {\"id\":\"mess\",\"skill_id\":\"\",\"minimum_skill\":0.0,\"maximum_fatigue\":90.0,\"minimum_health\":30.0,\"maximum_dose_msv\":300.0,\"maximum_hours\":12.0,\"maximum_hours_if_impaired\":8.0,\"allow_unfit\":false,\"light_duty\":false,\"requires_not_quarantined\":true,\"precision_work\":true,\"hazard_class\":\"food\"},\n" +
            "    {\"id\":\"hatch_opener\",\"skill_id\":\"\",\"minimum_skill\":0.0,\"maximum_fatigue\":85.0,\"minimum_health\":35.0,\"maximum_dose_msv\":300.0,\"maximum_hours\":12.0,\"maximum_hours_if_impaired\":6.0,\"allow_unfit\":false,\"light_duty\":false,\"requires_not_quarantined\":true,\"precision_work\":true,\"hazard_class\":\"airlock\"},\n" +
            "    {\"id\":\"intake_sleeper\",\"skill_id\":\"\",\"minimum_skill\":0.0,\"maximum_fatigue\":95.0,\"minimum_health\":25.0,\"maximum_dose_msv\":600.0,\"maximum_hours\":8.0,\"maximum_hours_if_impaired\":8.0,\"allow_unfit\":true,\"light_duty\":true,\"requires_not_quarantined\":true,\"precision_work\":false,\"hazard_class\":\"intake\"},\n" +
            "    {\"id\":\"expedition\",\"skill_id\":\"\",\"minimum_skill\":0.0,\"maximum_fatigue\":80.0,\"minimum_health\":40.0,\"maximum_dose_msv\":100.0,\"maximum_hours\":12.0,\"maximum_hours_if_impaired\":6.0,\"allow_unfit\":false,\"light_duty\":false,\"requires_not_quarantined\":true,\"precision_work\":true,\"hazard_class\":\"surface\"}\n" +
            "  ]\n" +
            "}";

        /// <summary>
        /// Writes minimal-valid mandatory catalogs into an isolated scratch
        /// directory so the validator reaches the rule under test instead of
        /// reporting the mandatory catalogs as missing.
        /// </summary>
        public static void SeedMandatoryCatalogs(string scratch)
        {
            File.WriteAllText(Path.Combine(scratch, "trade_embargoes.json"), TradeEmbargoes);
            File.WriteAllText(Path.Combine(scratch, "regional_prices.json"), RegionalPrices);
            File.WriteAllText(Path.Combine(scratch, "duty_roles.json"), DutyRoles);
        }
    }
}