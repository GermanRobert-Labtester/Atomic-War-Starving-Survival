// GameBootstrap.EventTrackers.cs — boot/wire Event_* narrative systems with CaptureState.
// (Separate from GameBootstrap.Events.cs which hosts Safe Haven choice handlers.)
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct Event_* trackers that already implement Capture/Restore.
        /// Remaining events without CR land in a follow-up batch.
        /// Host hooks are offline-safe logs; event hosts fire real APIs.
        /// </summary>
        private void BootEvents()
        {
            EventBrawl = new Event_Brawl();
            EventComingOfAge = new Event_ComingOfAge();
            EventCultBlessing = new Event_CultBlessing();
            EventCultInitiation = new Event_CultInitiation();
            EventCultOfAi = new Event_CultOfAI();
            EventEmpCascade = new Event_EMPCascade();
            EventFeralRescue = new Event_FeralRescue();
            EventFoundDiary = new Event_FoundDiary();
            EventGriefCascade = new Event_GriefCascade();
            EventHungerStrike = new Event_HungerStrike();
            EventNodeCollapse = new Event_NodeCollapse();
            EventRansomNote = new Event_RansomNote();
            EventSchism = new Event_Schism();
            EventSecretSociety = new Event_SecretSociety();
            EventSiblingFeud = new Event_SiblingFeud();
            EventSpontaneousMurder = new Event_SpontaneousMurder();
            EventTeenRebellion = new Event_TeenRebellion();
            EventWitchHunt = new Event_WitchHunt();
            EventEuthanasiaPact = new Event_EuthanasiaPact();
            EventFactionMerger = new Event_FactionMerger();
            EventMudslide = new Event_Mudslide();
            EventNumbersStation = new Event_NumbersStation();
            EventProjectSabotage = new Event_ProjectSabotage();
            EventSinkhole = new FoundationSinkholeSystem("event_sinkhole");
            EventTriangulation = new Event_Triangulation();
            EventVaultCollision = new VaultCollisionSystem("event_vault_collision");
            EventWarlordSuccession = new Event_WarlordSuccession();

            WireEvents();
            Debug.Log("[GameBootstrap] Events ready (27 CaptureState trackers).");
        }

        private void WireEvents()
        {
            if (EventBrawl != null)
            {
                EventBrawl.OnBrawlStarted += (a, b) =>
                    Debug.Log($"[GameBootstrap] EVENT: brawl started '{a}' vs '{b}'");
                EventBrawl.OnBrawlBrokenUp += (inter, stopped) =>
                    Debug.Log($"[GameBootstrap] EVENT: brawl broken by '{inter}' stopped '{stopped}'");
                EventBrawl.OnInjuryInflicted += (id, injury) =>
                    Debug.Log($"[GameBootstrap] EVENT: brawl injury '{injury}' on '{id}'");
            }

            if (EventComingOfAge != null)
            {
                EventComingOfAge.OnComingOfAge += (child, trait) =>
                    Debug.Log($"[GameBootstrap] EVENT: coming of age '{child}' trait '{trait}'");
                EventComingOfAge.OnDependentTraitsRemoved += id =>
                    Debug.Log($"[GameBootstrap] EVENT: coming of age cleared dependents for '{id}'");
            }

            if (EventCultBlessing != null)
            {
                EventCultBlessing.OnSurvivorReturned += id =>
                    Debug.Log($"[GameBootstrap] EVENT: cult blessing returned '{id}'");
                EventCultBlessing.OnRadEmitted += (id, rad) =>
                    Debug.Log($"[GameBootstrap] EVENT: cult blessing rad +{rad:F0} from '{id}'");
                EventCultBlessing.OnAllyPoisoned += (src, ally) =>
                    Debug.Log($"[GameBootstrap] EVENT: cult blessing poisoned '{ally}' via '{src}'");
                EventCultBlessing.OnChelationApplied += id =>
                    Debug.Log($"[GameBootstrap] EVENT: cult blessing chelation '{id}'");
            }

            if (EventCultInitiation != null)
            {
                EventCultInitiation.OnInitiationStarted += (id, type) =>
                    Debug.Log($"[GameBootstrap] EVENT: cult initiation '{type}' for '{id}'");
                EventCultInitiation.OnDayPassed += (id, day) =>
                    Debug.Log($"[GameBootstrap] EVENT: cult initiation day {day} for '{id}'");
                EventCultInitiation.OnPlayerIntervened += () =>
                    Debug.Log("[GameBootstrap] EVENT: cult initiation player intervened");
                EventCultInitiation.OnMutinyTriggered += preacher =>
                    Debug.Log($"[GameBootstrap] EVENT: cult initiation mutiny '{preacher}'");
                EventCultInitiation.OnInitiationSurvived += id =>
                    Debug.Log($"[GameBootstrap] EVENT: cult initiation survived '{id}'");
                EventCultInitiation.OnInitiationFailed += id =>
                    Debug.Log($"[GameBootstrap] EVENT: cult initiation failed '{id}'");
            }

            if (EventCultOfAi != null)
            {
                EventCultOfAi.OnCultFormed += n =>
                    Debug.Log($"[GameBootstrap] EVENT: cult of AI formed n={n}");
                EventCultOfAi.OnFoodSacrificed += amt =>
                    Debug.Log($"[GameBootstrap] EVENT: cult of AI sacrificed food {amt}");
                EventCultOfAi.OnMutinyTriggered += () =>
                    Debug.Log("[GameBootstrap] EVENT: cult of AI mutiny");
                EventCultOfAi.OnCultDisbanded += () =>
                    Debug.Log("[GameBootstrap] EVENT: cult of AI disbanded");
            }

            if (EventEmpCascade != null)
            {
                EventEmpCascade.OnCascadeTriggered += () =>
                    Debug.Log("[GameBootstrap] EVENT: EMP cascade triggered");
                EventEmpCascade.OnExosuitCrushed += id =>
                    Debug.Log($"[GameBootstrap] EVENT: EMP exosuit crushed '{id}'");
                EventEmpCascade.OnAutodocAmputated += (patient, limb) =>
                    Debug.Log($"[GameBootstrap] EVENT: EMP autodoc amputated '{limb}' on '{patient}'");
                EventEmpCascade.OnDeviceFried += id =>
                    Debug.Log($"[GameBootstrap] EVENT: EMP fried '{id}'");
            }

            if (EventFeralRescue != null)
            {
                EventFeralRescue.OnFeralChildFound += id =>
                    Debug.Log($"[GameBootstrap] EVENT: feral child found '{id}'");
                EventFeralRescue.OnDiseaseIntroduced += id =>
                    Debug.Log($"[GameBootstrap] EVENT: feral disease '{id}'");
                EventFeralRescue.OnSocializationProgress += id =>
                    Debug.Log($"[GameBootstrap] EVENT: feral socialization '{id}'");
                EventFeralRescue.OnChildTamed += id =>
                    Debug.Log($"[GameBootstrap] EVENT: feral child tamed '{id}'");
            }

            if (EventFoundDiary != null)
            {
                EventFoundDiary.OnDiaryFound += (finder, owner) =>
                    Debug.Log($"[GameBootstrap] EVENT: diary found by '{finder}' owner '{owner}'");
                EventFoundDiary.OnBlackmailStarted += (bm, tgt) =>
                    Debug.Log($"[GameBootstrap] EVENT: diary blackmail '{bm}' → '{tgt}'");
                EventFoundDiary.OnBlackmailResolved += id =>
                    Debug.Log($"[GameBootstrap] EVENT: diary blackmail resolved '{id}'");
            }

            if (EventGriefCascade != null)
            {
                EventGriefCascade.OnBelovedDied += (id, rating) =>
                    Debug.Log($"[GameBootstrap] EVENT: grief beloved died '{id}' rating={rating:F1}");
                EventGriefCascade.OnCascadeStarted += id =>
                    Debug.Log($"[GameBootstrap] EVENT: grief cascade started '{id}'");
                EventGriefCascade.OnMoraleCrashed += amt =>
                    Debug.Log($"[GameBootstrap] EVENT: grief morale crash {amt:F1}");
                EventGriefCascade.OnMentalBreakTriggered += (id, type) =>
                    Debug.Log($"[GameBootstrap] EVENT: grief mental break '{type}' on '{id}'");
                EventGriefCascade.OnSecondaryDeath += (id, cause) =>
                    Debug.Log($"[GameBootstrap] EVENT: grief secondary death '{id}' ({cause})");
                EventGriefCascade.OnCascadeEnded += n =>
                    Debug.Log($"[GameBootstrap] EVENT: grief cascade ended deaths={n}");
            }

            if (EventHungerStrike != null)
            {
                EventHungerStrike.OnHungerStrikeStarted += id =>
                    Debug.Log($"[GameBootstrap] EVENT: hunger strike started '{id}'");
                EventHungerStrike.OnHungerStrikeEnded += id =>
                    Debug.Log($"[GameBootstrap] EVENT: hunger strike ended '{id}'");
                EventHungerStrike.OnSurvivorStarvedToDeath += id =>
                    Debug.Log($"[GameBootstrap] EVENT: hunger strike starved '{id}'");
            }

            if (EventNodeCollapse != null)
            {
                EventNodeCollapse.OnCollapseStarted += node =>
                    Debug.Log($"[GameBootstrap] EVENT: node collapse started '{node}'");
                EventNodeCollapse.OnCountdownTick += (node, turns) =>
                    Debug.Log($"[GameBootstrap] EVENT: node collapse tick '{node}' turns={turns}");
                EventNodeCollapse.OnNodeDeleted += node =>
                    Debug.Log($"[GameBootstrap] EVENT: node collapse deleted '{node}'");
                EventNodeCollapse.OnSurvivorTrapped += id =>
                    Debug.Log($"[GameBootstrap] EVENT: node collapse trapped '{id}'");
            }

            if (EventRansomNote != null)
            {
                EventRansomNote.OnNoteDelivered += (amt, res) =>
                    Debug.Log($"[GameBootstrap] EVENT: ransom note {amt:F0} {res}");
                EventRansomNote.OnRansomPaid += () =>
                    Debug.Log("[GameBootstrap] EVENT: ransom paid");
                EventRansomNote.OnRansomRefused += () =>
                    Debug.Log("[GameBootstrap] EVENT: ransom refused");
                EventRansomNote.OnWarlordReturned += demand =>
                    Debug.Log($"[GameBootstrap] EVENT: ransom warlord returned demand={demand}");
            }

            if (EventSchism != null)
            {
                EventSchism.OnSchismStarted += () =>
                    Debug.Log("[GameBootstrap] EVENT: schism started");
                EventSchism.OnSurvivorPickedSide += (id, a) =>
                    Debug.Log($"[GameBootstrap] EVENT: schism '{id}' sideA={a}");
                EventSchism.OnSchismResolved += () =>
                    Debug.Log("[GameBootstrap] EVENT: schism resolved");
            }

            if (EventSecretSociety != null)
            {
                EventSecretSociety.OnCliqueFormed += (id, members) =>
                    Debug.Log($"[GameBootstrap] EVENT: secret society clique '{id}' n={members?.Length ?? 0}");
                EventSecretSociety.OnInsiderHelped += (helper, helped) =>
                    Debug.Log($"[GameBootstrap] EVENT: secret society help '{helper}'→'{helped}'");
                EventSecretSociety.OnOutsiderIgnored += (member, outsider) =>
                    Debug.Log($"[GameBootstrap] EVENT: secret society ignore '{member}' vs '{outsider}'");
                EventSecretSociety.OnSabotage += (member, target, type) =>
                    Debug.Log($"[GameBootstrap] EVENT: secret society sabotage '{type}' '{member}'→'{target}'");
            }

            if (EventSiblingFeud != null)
            {
                EventSiblingFeud.OnJealousyTriggered += (teen, sib) =>
                    Debug.Log($"[GameBootstrap] EVENT: sibling feud jealousy '{teen}' vs '{sib}'");
                EventSiblingFeud.OnAffinityReduced += (a, b, d) =>
                    Debug.Log($"[GameBootstrap] EVENT: sibling feud affinity {d:F2} '{a}'/'{b}'");
            }

            if (EventSpontaneousMurder != null)
            {
                EventSpontaneousMurder.OnDaysMaxedUpdated += (id, anx, dep) =>
                    Debug.Log($"[GameBootstrap] EVENT: spontaneous murder days anx={anx} dep={dep} '{id}'");
                EventSpontaneousMurder.OnSnapTriggered += id =>
                    Debug.Log($"[GameBootstrap] EVENT: spontaneous murder snap '{id}'");
                EventSpontaneousMurder.OnMurderCommitted += (killer, victim, weapon) =>
                    Debug.Log($"[GameBootstrap] EVENT: spontaneous murder '{killer}' killed '{victim}' with '{weapon}'");
                EventSpontaneousMurder.OnBodyDiscovered += id =>
                    Debug.Log($"[GameBootstrap] EVENT: spontaneous murder body '{id}'");
            }

            if (EventTeenRebellion != null)
            {
                EventTeenRebellion.OnRebellionStarted += (teen, room) =>
                    Debug.Log($"[GameBootstrap] EVENT: teen rebellion '{teen}' in '{room}'");
                EventTeenRebellion.OnResourcesWasted += (teen, food, power) =>
                    Debug.Log($"[GameBootstrap] EVENT: teen rebellion waste food={food} power={power:F1} '{teen}'");
                EventTeenRebellion.OnTrustLowered += (teen, pen) =>
                    Debug.Log($"[GameBootstrap] EVENT: teen rebellion trust {pen:F2} '{teen}'");
            }

            if (EventWitchHunt != null)
            {
                EventWitchHunt.OnBadLuckTracked += (type, streak) =>
                    Debug.Log($"[GameBootstrap] EVENT: witch hunt bad luck '{type}' streak={streak}");
                EventWitchHunt.OnHuntStarted += (accused, accusers) =>
                    Debug.Log($"[GameBootstrap] EVENT: witch hunt accused '{accused}' n={accusers?.Length ?? 0}");
                EventWitchHunt.OnBanishmentDemanded += id =>
                    Debug.Log($"[GameBootstrap] EVENT: witch hunt banish demand '{id}'");
                EventWitchHunt.OnPlayerBanished += id =>
                    Debug.Log($"[GameBootstrap] EVENT: witch hunt banished '{id}'");
                EventWitchHunt.OnStrikeStarted += strikers =>
                    Debug.Log($"[GameBootstrap] EVENT: witch hunt strike n={strikers?.Length ?? 0}");
                EventWitchHunt.OnStrikeEnded += () =>
                    Debug.Log("[GameBootstrap] EVENT: witch hunt strike ended");
            }

            if (EventEuthanasiaPact != null)
            {
                EventEuthanasiaPact.OnPactFormed += (a, b, hours) =>
                    Debug.Log($"[GameBootstrap] EVENT: euthanasia pact '{a}'+'{b}' {hours:F0}h");
                EventEuthanasiaPact.OnPactTick += (a, b, left) =>
                    Debug.Log($"[GameBootstrap] EVENT: euthanasia pact tick {left:F0}h '{a}'/'{b}'");
                EventEuthanasiaPact.OnPactExecuted += (a, b) =>
                    Debug.Log($"[GameBootstrap] EVENT: euthanasia pact executed '{a}'/'{b}'");
                EventEuthanasiaPact.OnPactCancelled += (a, b) =>
                    Debug.Log($"[GameBootstrap] EVENT: euthanasia pact cancelled '{a}'/'{b}'");
            }
            if (EventFactionMerger != null)
            {
                EventFactionMerger.OnMergerTriggered += (st, f1, f2) =>
                    Debug.Log($"[GameBootstrap] EVENT: faction merger '{f1}'+'{f2}'");
                EventFactionMerger.OnSuperFactionFormed += st =>
                    Debug.Log($"[GameBootstrap] EVENT: super faction '{st.superFactionId}'");
            }
            if (EventMudslide != null)
            {
                EventMudslide.OnHatchBuried += st =>
                    Debug.Log("[GameBootstrap] EVENT: mudslide hatch buried");
                EventMudslide.OnHatchCleared += st =>
                    Debug.Log("[GameBootstrap] EVENT: mudslide hatch cleared");
                EventMudslide.OnDigProgress += (st, h) =>
                    Debug.Log($"[GameBootstrap] EVENT: mudslide dig {h:F1}h");
                EventMudslide.OnContaminationApplied += (st, c) =>
                    Debug.Log($"[GameBootstrap] EVENT: mudslide contamination +{c:F1}");
            }
            if (EventNumbersStation != null)
            {
                EventNumbersStation.OnSequenceGenerated += (st, seq) =>
                    Debug.Log($"[GameBootstrap] EVENT: numbers station seq n={seq?.Count ?? 0}");
                EventNumbersStation.OnDecodeAttempt += (st, ok) =>
                    Debug.Log($"[GameBootstrap] EVENT: numbers station decode ok={ok}");
                EventNumbersStation.OnMapNodeUnlocked += (st, node) =>
                    Debug.Log($"[GameBootstrap] EVENT: numbers station unlock '{node}'");
            }
            if (EventProjectSabotage != null)
            {
                EventProjectSabotage.OnSabotageAttempt += (st, site, n) =>
                    Debug.Log($"[GameBootstrap] EVENT: project sabotage '{site}' n={n}");
                EventProjectSabotage.OnGuardAssigned += (st, n) =>
                    Debug.Log($"[GameBootstrap] EVENT: project sabotage guards={n}");
                EventProjectSabotage.OnConstructionDamaged += (st, site, p) =>
                    Debug.Log($"[GameBootstrap] EVENT: project sabotage damage '{site}' {p:F0}%");
                EventProjectSabotage.OnSabotageRepelled += (st, site) =>
                    Debug.Log($"[GameBootstrap] EVENT: project sabotage repelled '{site}'");
            }
            if (EventSinkhole != null)
            {
                EventSinkhole.OnCollapseTriggered += (id, room) =>
                    Debug.Log($"[GameBootstrap] EVENT: sinkhole collapse '{room}' ({id})");
            }
            if (EventTriangulation != null)
            {
                EventTriangulation.OnSignalReceived += (st, km) =>
                    Debug.Log($"[GameBootstrap] EVENT: triangulation signal {km:F1}km");
                EventTriangulation.OnTriangulationAttempt += (st, ok, node) =>
                    Debug.Log($"[GameBootstrap] EVENT: triangulation attempt ok={ok} '{node}'");
                EventTriangulation.OnSupplyClaimed += st =>
                    Debug.Log("[GameBootstrap] EVENT: triangulation supply claimed");
            }
            if (EventVaultCollision != null)
            {
                EventVaultCollision.OnCollision += (id, neighbor) =>
                    Debug.Log($"[GameBootstrap] EVENT: vault collision '{neighbor}' ({id})");
                EventVaultCollision.OnLootOrThreat += (id, outcome) =>
                    Debug.Log($"[GameBootstrap] EVENT: vault collision outcome '{outcome}'");
            }
            if (EventWarlordSuccession != null)
            {
                EventWarlordSuccession.OnLeaderAssassinated += (st, faction) =>
                    Debug.Log($"[GameBootstrap] EVENT: warlord succession assassinated '{faction}'");
                EventWarlordSuccession.OnFactionFractured += st =>
                    Debug.Log("[GameBootstrap] EVENT: warlord succession fractured");
                EventWarlordSuccession.OnSubFactionsAtWar += st =>
                    Debug.Log("[GameBootstrap] EVENT: warlord succession at war");
                EventWarlordSuccession.OnFactionsPlayedOff += (st, target) =>
                    Debug.Log($"[GameBootstrap] EVENT: warlord succession played off '{target}'");
            }
        }
    }
}
