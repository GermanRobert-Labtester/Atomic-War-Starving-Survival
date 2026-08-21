using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Nobody's Charter pack-minimum smoke: factions load, vouch quest exists,
    /// danger/rads schema, GateAllowsCrossing actually blocks travel.
    /// </summary>
    public static class CrossingHeadlessDemo
    {
        public static HeadlessReport Run(string dataDirectory = null, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) report.PassedCount++;
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
                if (condition) log.Info("[PASS] " + name);
            }

            log.Info("[CrossingHeadlessDemo] begin");

            var session = CrossingSession.Load(dataDirectory, log);
            report.LocationCount = session.Catalog.Locations.Count;
            report.QuestCount = session.Catalog.Quests.Count;

            Check(session.Catalog.Factions.Count == 3, "three Crossing blocs (not faction_lore.json)");
            Check(session.Catalog.GetFaction(CrossingIds.FactionScale) != null, "faction_the_scale");
            Check(session.Catalog.Quests.Count >= 12, "twelve Nobody's Charter quests");
            Check(session.Catalog.GetQuest(CrossingIds.TheVouch) != null, "quest_crossing_the_vouch exists");
            Check(session.Catalog.GetQuest(CrossingIds.FirstWeigh) != null, "quest_crossing_first_weigh");
            Check(session.Catalog.GetQuest("quest_crossing_the_marker") != null, "quest_crossing_the_marker exists");
            Check(session.Catalog.GetQuest("quest_crossing_the_forfeit") != null, "quest_crossing_the_forfeit exists");
            Check(session.Catalog.GetQuest("quest_crossing_the_vote_that_isnt") != null, "quest_crossing_the_vote_that_isnt exists");
            Check(session.Catalog.GetQuest("quest_crossing_three_dry_pages") != null, "quest_crossing_three_dry_pages exists");
            Check(session.Catalog.GetQuest("quest_crossing_who_holds_the_ledger") != null, "quest_crossing_who_holds_the_ledger exists");
            Check(session.Catalog.GetQuest("quest_crossing_companion_mattis") != null, "quest_crossing_companion_mattis exists");
            Check(session.Catalog.Items.Count >= 11, "eleven Crossing items loaded");
            Check(session.Catalog.GetItem("item_charter_three_pages") != null, "item_charter_three_pages present");
            Check(session.Catalog.GetItem("item_debt_contract_copy") != null, "item_debt_contract_copy present");
            Check(session.Catalog.Encounters.Count >= 10, "ten Crossing encounters loaded");
            Check(session.Catalog.GetEncounter("enc_nc_collector_visit") != null, "enc_nc_collector_visit present");
            Check(session.Catalog.GetEncounter("enc_nc_backer_pressure") != null, "enc_nc_backer_pressure present");
            Check(session.Catalog.GetEncounter("enc_nc_lockup_muscle") != null, "enc_nc_lockup_muscle present");
            Check(session.Catalog.GetEncounter("enc_nc_standing_ambush") != null, "enc_nc_standing_ambush present");
            Check(session.Catalog.Crises.Count >= 5, "five Crossing multi-phase crises loaded");
            Check(session.Catalog.GetCrisis("crisis_the_forfeit") != null, "crisis_the_forfeit present");
            Check(session.Catalog.GetCrisis("crisis_who_holds_the_ledger") != null, "crisis_who_holds_the_ledger present");
            var firstWeigh = session.Catalog.GetQuest(CrossingIds.FirstWeigh);
            Check(firstWeigh != null && firstWeigh.prereq_quest_id == CrossingIds.TheVouch,
                "first_weigh still prereqs the vouch");

            var deck = session.Catalog.GetLocation(CrossingIds.Weighbridge);
            Check(deck != null, "loc_crossing_weighbridge present");
            Check(deck != null && deck.displayName != "The Weighbridge",
                "Deck Scale copy is distinct from loc_weighbridge");

            bool schema = true;
            for (int i = 0; i < session.Catalog.Locations.Count; i++)
            {
                var loc = session.Catalog.Locations[i];
                if (loc == null) continue;
                if (loc.dangerLevel < CrossingCatalogLoader.MinDanger - 0.01f
                    || loc.dangerLevel > CrossingCatalogLoader.MaxDanger + 0.01f)
                    schema = false;
                if (loc.baseRadsPerHour < CrossingCatalogLoader.MinRads - 0.01f
                    || loc.baseRadsPerHour > CrossingCatalogLoader.MaxRads + 0.01f)
                    schema = false;
            }
            Check(schema, "danger 3–6 and rads 8–25 (scale fix)");

            Check(!session.GateAllowsCrossing(), "gate closed without a vouch");
            Check(session.IsTravelBlocked(CrossingIds.ViaductGate), "GateAllowsCrossing blocks the viaduct");
            Check(session.TryVouch(CrossingIds.NpcMattis), "Mattis vouch granted");
            Check(session.GateAllowsCrossing(), "gate open after vouch");
            Check(!session.IsTravelBlocked(CrossingIds.ViaductGate), "viaduct walkable after vouch");
            Check(session.IsTravelBlocked("loc_ice_road_gate") == false, "non-Crossing nodes ignored by vouch gate");

            var json = new SystemTextJsonSerializer();
            string blob = json.Serialize(session.Vouch.CaptureState());
            var restored = new VouchAccessSystem();
            restored.RestoreState(json.Deserialize<VouchAccessSystemState>(blob)!);
            Check(restored.HasAccess && restored.VouchedBy == CrossingIds.NpcMattis, "vouch save roundtrip");

            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("CrossingHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            sb.Append(" locations=").Append(report.LocationCount).Append(" quests=").Append(report.QuestCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
