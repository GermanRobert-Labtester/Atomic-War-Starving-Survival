using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// One-time adoption of Verdict quest progress out of the Year of Ash
    /// envelope. Pre-v3 Verdict saves persisted their quest progress inside
    /// <c>YearOfAshSave.quests</c> (the QuestlineSystem was shared). Going
    /// forward Verdict owns its quests in <see cref="VerdictSave.quests"/>;
    /// this helper folds any <c>quest_verdict_*</c> records found in an older
    /// Year of Ash save into the Verdict envelope so no progress is lost on
    /// upgrade. Deterministic: on conflict the Verdict state wins.
    /// </summary>
    public static class VerdictQuestMigration
    {
        public const string VerdictQuestPrefix = "quest_verdict_";

        public static bool IsVerdictQuestline(string questlineId)
        {
            return questlineId != null &&
                questlineId.StartsWith(VerdictQuestPrefix, StringComparison.Ordinal);
        }

        /// <summary>
        /// Copies <c>quest_verdict_*</c> records from <paramref name="yearOfAshState"/>
        /// into <paramref name="verdictState"/> where the Verdict state does not
        /// already hold them. Returns the number of records adopted.
        /// </summary>
        public static int AdoptFromYearOfAsh(
            QuestlineSystemState verdictState,
            QuestlineSystemState yearOfAshState)
        {
            int adopted = 0;
            if (verdictState == null || yearOfAshState == null) return 0;

            if (yearOfAshState.active != null)
            {
                foreach (var rec in yearOfAshState.active)
                {
                    if (rec == null || !IsVerdictQuestline(rec.questlineId)) continue;
                    if (verdictState.active.Exists(a => a.questlineId == rec.questlineId)) continue;
                    verdictState.active.Add(new ActiveQuestlineRecord
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
                    if (!IsVerdictQuestline(id)) continue;
                    if (verdictState.completedQuestlineIds.Contains(id)) continue;
                    verdictState.completedQuestlineIds.Add(id);
                    adopted++;
                }
            }

            if (yearOfAshState.failedQuestlineIds != null)
            {
                foreach (var id in yearOfAshState.failedQuestlineIds)
                {
                    if (!IsVerdictQuestline(id)) continue;
                    if (verdictState.failedQuestlineIds.Contains(id)) continue;
                    verdictState.failedQuestlineIds.Add(id);
                    adopted++;
                }
            }

            return adopted;
        }

        /// <summary>
        /// Removes <c>quest_verdict_*</c> records from a Year of Ash quest state.
        /// Called after adoption so the Year of Ash envelope stops re-serializing
        /// Verdict quest progress (one persisted owner, not two). Returns the
        /// number of records removed.
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
                        IsVerdictQuestline(yearOfAshState.active[i].questlineId))
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
                    if (IsVerdictQuestline(yearOfAshState.completedQuestlineIds[i]))
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
                    if (IsVerdictQuestline(yearOfAshState.failedQuestlineIds[i]))
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
