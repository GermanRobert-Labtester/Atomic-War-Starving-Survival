using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core
{
    /// <summary>
    /// One-time adoption of Dose quest progress out of the Year of Ash envelope.
    /// Pre-v2 Dose saves persisted their quest progress inside
    /// <c>YearOfAshSave.quests</c> (the QuestlineSystem was shared). Going
    /// forward Dose owns its quests in <see cref="DoseLedgerSave.quests"/>;
    /// this helper folds the canonical Dose questline records found in an older
    /// Year of Ash save into the Dose envelope so no progress is lost on
    /// upgrade. Deterministic: on conflict the Dose state wins.
    ///
    /// The Dose questlines are identified by an explicit allowlist (they do not
    /// share a uniform id prefix like <c>quest_verdict_</c>): the four register
    /// quest lines authored in <c>dose_quests.json</c>.
    /// </summary>
    public static class DoseQuestMigration
    {
        public static readonly string[] CanonicalQuestlineIds =
        {
            "quest_the_dose_the_first_reading",
            "quest_the_sick_of_room_seven",
            "quest_the_childs_number",
            "quest_the_signed_hour",
            "quest_the_falsified_reading",
            "quest_the_stolen_dosimeter",
            "quest_child_over_the_limit",
            "quest_the_register_audit",
            "quest_black_market_clean_bill",
            "quest_the_broken_calibration_chain",
            "quest_exposure_for_the_essential_worker",
            "quest_the_missing_page"
        };

        public static bool IsDoseQuestline(string questlineId)
        {
            if (string.IsNullOrEmpty(questlineId)) return false;
            for (int i = 0; i < CanonicalQuestlineIds.Length; i++)
                if (CanonicalQuestlineIds[i] == questlineId) return true;
            return false;
        }

        /// <summary>
        /// Copies Dose quest records from <paramref name="yearOfAshState"/> into
        /// <paramref name="doseState"/> where the Dose state does not already hold
        /// them. Returns the number of records adopted.
        ///
        /// Note: the aggregate <c>totalMoraleDeltaFromQuests</c> /
        /// <c>totalGuiltDeltaFromQuests</c> counters are global to the shared
        /// QuestlineSystem (they span Year of Ash + Verdict + Dose questlines) and
        /// cannot be attributed to Dose alone, so they are not migrated — the Dose
        /// envelope starts those aggregates at 0. Per-record choice history and
        /// status are fully preserved.
        /// </summary>
        public static int AdoptFromYearOfAsh(
            QuestlineSystemState doseState,
            QuestlineSystemState yearOfAshState)
        {
            int adopted = 0;
            if (doseState == null || yearOfAshState == null) return 0;

            if (yearOfAshState.active != null)
            {
                foreach (var rec in yearOfAshState.active)
                {
                    if (rec == null || !IsDoseQuestline(rec.questlineId)) continue;
                    if (doseState.active.Exists(a => a.questlineId == rec.questlineId)) continue;
                    doseState.active.Add(new ActiveQuestlineRecord
                    {
                        questlineId = rec.questlineId,
                        currentStageId = rec.currentStageId ?? string.Empty,
                        status = rec.status,
                        dayStarted = rec.dayStarted,
                        dayResolved = rec.dayResolved,
                        choiceHistory = rec.choiceHistory != null
                            ? new List<string>(rec.choiceHistory)
                            : new List<string>()
                    });
                    adopted++;
                }
            }

            if (yearOfAshState.completedQuestlineIds != null)
            {
                foreach (var id in yearOfAshState.completedQuestlineIds)
                {
                    if (!IsDoseQuestline(id)) continue;
                    if (doseState.completedQuestlineIds.Contains(id)) continue;
                    doseState.completedQuestlineIds.Add(id);
                    adopted++;
                }
            }

            if (yearOfAshState.failedQuestlineIds != null)
            {
                foreach (var id in yearOfAshState.failedQuestlineIds)
                {
                    if (!IsDoseQuestline(id)) continue;
                    if (doseState.failedQuestlineIds.Contains(id)) continue;
                    doseState.failedQuestlineIds.Add(id);
                    adopted++;
                }
            }

            return adopted;
        }

        /// <summary>
        /// Removes Dose quest records from a Year of Ash quest state. Called after
        /// adoption so the Year of Ash envelope stops re-serializing Dose quest
        /// progress (one persisted owner, not two). Returns the number removed.
        /// </summary>
        public static int StripFromYearOfAsh(QuestlineSystemState yearOfAshState)
        {
            int removed = 0;
            if (yearOfAshState == null) return 0;

            if (yearOfAshState.active != null)
            {
                for (int i = yearOfAshState.active.Count - 1; i >= 0; i--)
                {
                    if (yearOfAshState.active[i] != null &&
                        IsDoseQuestline(yearOfAshState.active[i].questlineId))
                    {
                        yearOfAshState.active.RemoveAt(i);
                        removed++;
                    }
                }
            }

            if (yearOfAshState.completedQuestlineIds != null)
            {
                for (int i = yearOfAshState.completedQuestlineIds.Count - 1; i >= 0; i--)
                {
                    if (IsDoseQuestline(yearOfAshState.completedQuestlineIds[i]))
                    {
                        yearOfAshState.completedQuestlineIds.RemoveAt(i);
                        removed++;
                    }
                }
            }

            if (yearOfAshState.failedQuestlineIds != null)
            {
                for (int i = yearOfAshState.failedQuestlineIds.Count - 1; i >= 0; i--)
                {
                    if (IsDoseQuestline(yearOfAshState.failedQuestlineIds[i]))
                    {
                        yearOfAshState.failedQuestlineIds.RemoveAt(i);
                        removed++;
                    }
                }
            }

            return removed;
        }
    }
}
