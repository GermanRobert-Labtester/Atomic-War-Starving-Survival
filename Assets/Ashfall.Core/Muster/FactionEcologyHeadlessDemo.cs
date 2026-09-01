using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Ashfall.Core;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    public sealed class FactionEcologyHeadlessReport : HeadlessReport
    {
        public FactionActionBoardState Board;
        public string MusterPath;
    }

    /// <summary>
    /// Plan 25 vertical-slice gate: one peacetime faction action (A1, the Guild
    /// salvage claim) resolved against the Scavenger Guild's own trust, the
    /// grievance it produces gating the E-P1 escalation chain on the faction-war
    /// runner, the claimant witness testifying from that flag, and the camp
    /// arrivals scene reacting to the derived muster path. Loads the REAL data
    /// authority (muster_faction_actions.json, muster_witnesses.json,
    /// muster_camp_scenes.json, faction_war_events.json) — no fixtures.
    /// Invoked by `dotnet test` and by Godot `-- --faction-ecology-selftest`.
    /// </summary>
    public static class FactionEcologyHeadlessDemo
    {
        public static FactionEcologyHeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new FactionEcologyHeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[FactionEcologyHeadlessDemo] begin");

            if (string.IsNullOrEmpty(dataDirectory))
                CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDirectory);
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // ── Authored action A1 loads and resolves through the guild's trust ──
            var actions = FactionActionCatalogLoader.LoadActions(dataDirectory, fileIO, json);
            var claim = actions.Find(a => a.id == "act_salvage_rights_offer");
            Check(claim != null, "action catalog loads A1 (salvage rights offer)");
            Check(claim != null && claim.variants.Count == 5, "A1 authors all five standing bands");

            var guild = new ScavengerGuildSystem();
            guild.AdjustTrust(5f); // neutral band
            var board = new FactionActionBoard(guild: guild);
            board.SetCatalog(actions);
            var offers = board.AvailableActions(60);
            var claimOffer = offers.Find(o => o.Definition.id == "act_salvage_rights_offer");
            Check(claimOffer != null && claimOffer.Band == "neutral",
                "A1 offered at day 60 in the neutral band");
            Check(board.Resolve("act_salvage_rights_offer", "dispute_claim", 60),
                "A1 dispute choice resolves (grievance authored)");
            Check(board.IsFlagSet("flag_grievance_scavenger_claim_disputed"),
                "dispute produced the grievance flag");
            Check(!board.Resolve("act_salvage_rights_offer", "pay_standard_fee", 61),
                "once-only action refuses re-resolution");

            // ── E-P1 escalation chain consumes the grievance on the war runner ──
            var warCatalog = new YearOfAsh.FactionWarContentCatalogLoader(fileIO, json, log)
                .Load(dataDirectory);
            var warRunner = new YearOfAsh.FactionWarChainRunner(warCatalog);
            warRunner.ExternalFlagProbe = board.IsFlagSet;
            int standingCalls = 0;
            warRunner.StandingDeltaApplier = (_, _) => standingCalls++;
            Check(System.Linq.Enumerable.Any(warCatalog.EventChains, c => c.chainId == "evt_p25_marked_ruin"),
                "war catalog loads the E-P1 escalation chain");

            var surfaced = warRunner.GetSurfacedStage("evt_p25_marked_ruin", 201);
            Check(surfaced != null && surfaced.stageId == "evt_p25_marked_ruin_s1",
                "E-P1 s1 surfaces once the grievance flag is set (day 200+)");
            warRunner.ResolveChoice("evt_p25_marked_ruin", "evt_p25_marked_ruin_s1",
                "evt_p25_marked_ruin_s1_c1", 201);
            Check(warRunner.IsFlagSet("flag_escalation_marked_ruin"),
                "E-P1 s1 produced the escalation flag");
            Check(warRunner.IsFlagSet("flag_escalation_marked_ruin_mediated"),
                "mediation choice produced its flag and routed standing");
            Check(standingCalls == 1, "standing delta routed to the host applier exactly once");

            // Without the grievance the chain never fires (fresh runner, no probe).
            var coldRunner = new YearOfAsh.FactionWarChainRunner(warCatalog);
            Check(coldRunner.GetSurfacedStage("evt_p25_marked_ruin", 300) == null,
                "E-P1 never surfaces without the grievance");

            // ── Witness W3 testifies from the same flag ─────────────────────
            var witnesses = WitnessCatalogLoader.LoadWitnesses(dataDirectory, fileIO, json);
            var claimant = witnesses.Find(w => w.id == "witness_scavenger_claimant");
            Check(claimant != null && claimant.testimonies.Count == 3,
                "witness catalog loads the claimant with helped/failed/absent variants");
            var eligibility = new GrievanceEligibility(board.IsFlagSet);
            var deliveries = WitnessSelector.Select(witnesses, 300, eligibility);
            var claimantDelivery = deliveries.Find(d => d.Witness.id == "witness_scavenger_claimant");
            Check(claimantDelivery != null, "claimant is selected at day 300 with the grievance set");
            Check(claimantDelivery != null && claimantDelivery.VariantId == "failed",
                "claimant testifies the failed variant from the grievance flag");

            var cleanEligibility = new GrievanceEligibility(_ => false);
            var cleanDeliveries = WitnessSelector.Select(witnesses, 300, cleanEligibility);
            var cleanClaimant = cleanDeliveries.Find(d => d.Witness.id == "witness_scavenger_claimant");
            Check(cleanClaimant != null && cleanClaimant.VariantId == "absent",
                "without any flags the claimant falls back to the absent variant");

            // ── Camp arrivals react to the derived muster path ──────────────
            var scenes = CampSceneCatalogLoader.LoadScenes(dataDirectory, fileIO, json);
            Check(scenes.Exists(s => s.id == "camp_scene_arrivals"),
                "camp scene catalog loads the arrivals scene");

            var negotiatedInput = new MusterPathInput
            {
                SurvivingMajorFactions = 3,
                ActiveTreatyCount = 1,
                CampFormed = true,
                GrievanceUnresolved = true
            };
            string path = MusterPathEvaluator.Evaluate(negotiatedInput);
            var muster = new MusterSystem();
            Check(path == MusterPaths.Negotiated && muster.SetMusterPath(path),
                "negotiated path derived and stored on the muster system");
            var musterRestored = new MusterSystem();
            musterRestored.RestoreState(muster.CaptureState());
            Check(musterRestored.MusterPath == MusterPaths.Negotiated,
                "muster path survives save/restore");

            var arrivalsNegotiated = CampSceneDirector.Select(
                scenes, "camp_scene_arrivals", 260, muster.MusterPath, board.IsFlagSet);
            Check(arrivalsNegotiated != null && arrivalsNegotiated.VariantId == "negotiated",
                "arrivals stage the negotiated variant under a negotiated path");
            var arrivalsVictors = CampSceneDirector.Select(
                scenes, "camp_scene_arrivals", 260, MusterPaths.Victors, board.IsFlagSet);
            Check(arrivalsVictors != null && arrivalsVictors.VariantId == "victors",
                "arrivals stage the victor's variant under a victor's path");
            var arrivalsEarly = CampSceneDirector.Select(
                scenes, "camp_scene_arrivals", 259, muster.MusterPath, board.IsFlagSet);
            Check(arrivalsEarly == null, "arrivals stay dark before the Muster opens");

            // ── Board state round-trips ─────────────────────────────────────
            string before = SaveChecksum.Compute(board.CaptureState());
            var restoredBoard = new FactionActionBoard(guild: new ScavengerGuildSystem());
            restoredBoard.RestoreState(board.CaptureState());
            string after = SaveChecksum.Compute(restoredBoard.CaptureState());
            Check(before == after, "board state checksum-stable across save/restore");
            Check(!restoredBoard.Resolve("act_salvage_rights_offer", "pay_standard_fee", 100),
                "reloaded board still honors the once-only resolution");

            report.Board = board.CaptureState();
            report.MusterPath = muster.MusterPath;

            log.Info("[FactionEcologyHeadlessDemo] done: " + report.PassedCount + " passed, " +
                     report.FailedCount + " failed");
            return report;
        }

        /// <summary>Minimal campaign-bound eligibility: real flags, no census
        /// constraints (the slice's witness carries no subject id).</summary>
        private sealed class GrievanceEligibility : IWitnessEligibility
        {
            private readonly Func<string, bool> _isFlagSet;
            public GrievanceEligibility(Func<string, bool> isFlagSet) => _isFlagSet = isFlagSet;
            public bool IsFlagSet(string flagId) => _isFlagSet(flagId);
            public bool IsSubjectAlive(string subjectId) => true;
            public bool IsFactionPresent(string factionId) => true;
        }
    }
}
