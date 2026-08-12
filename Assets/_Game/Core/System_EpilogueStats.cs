using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class EpilogueStatsState
    {
        public string systemId = "system_epilogue_stats";
        public int mealsCooked;
        public int bulletsFired;
        public int daysSurvived;
        public List<string> survivorsDead = new List<string>();
        public List<string> deathLocations = new List<string>();
        public List<string> journalEntries = new List<string>();
        public bool hasRecord;
    }

    [Serializable]
    public class EpilogueRecord
    {
        public int mealsCooked;
        public int bulletsFired;
        public int daysSurvived;
        public List<string> survivorsDead = new List<string>();
        public List<string> deathLocations = new List<string>();
        public List<string> journalEntries = new List<string>();

        // ── Expansion IV: The Logistics of Ruin ───────────────────────
        public int totalCaloriesConsumed;
        public int caloriesDeniedForGenerator;
        public int starvationDeathsWhilePantryHeld;
        public int raidersRepelled;
        public int raidersExecutedAfterSurrender;
        public int bulletsIntoUnarmedScavengers;
        public int corpsesProcessedForMaterials;
        public int goldFillingsExtracted;
        public int clothingStrippedArticles;
        public int decoyBroadcastsSent;
        public int factionPatrolsDiverted;
        public int estimatedDeceptionCasualties;
        public int crematoriumVisits;
        public int lightlessTrades;
        public int lightlessKilled;
        public string victoryType;
    }

    /// <summary>
    /// Prompt #768: The Empty Bunker (Epilogue).
    /// Tracks meals/bullets during the run; on game end generates a top-down
    /// highlight record (where people died, meals cooked, bullets fired, final journals).
    /// </summary>
    public class System_EpilogueStats
    {
        private EpilogueStatsState _state = new EpilogueStatsState();
        private EpilogueRecord _lastRecord;

        public event Action<EpilogueRecord> OnEpilogueGenerated;
        public event Action<int> OnMealRecorded;
        public event Action<int> OnBulletRecorded;

        public EpilogueStatsState State => _state;
        public EpilogueRecord LastRecord => _lastRecord;
        public int MealsCooked => _state.mealsCooked;
        public int BulletsFired => _state.bulletsFired;
        public bool HasRecord => _lastRecord != null || _state.hasRecord;

        /// <summary>Running tally — call when a meal is cooked this run.</summary>
        public void RecordMealCooked(int count = 1)
        {
            if (count <= 0) return;
            _state.mealsCooked = Math.Max(0, _state.mealsCooked + count);
            OnMealRecorded?.Invoke(_state.mealsCooked);
        }

        /// <summary>Running tally — call when ammo is expended in combat/defense.</summary>
        public void RecordBulletsFired(int count = 1)
        {
            if (count <= 0) return;
            _state.bulletsFired = Math.Max(0, _state.bulletsFired + count);
            OnBulletRecorded?.Invoke(_state.bulletsFired);
        }

        /// <summary>
        /// Generate the epilogue record from end-of-game statistics.
        /// Uses live counters when meal/bullet args are negative.
        /// </summary>
        public EpilogueRecord GenerateEpilogue(
            int mealsCooked,
            int bulletsFired,
            int daysSurvived,
            List<string> deadSurvivors,
            List<string> deathRoomIds,
            List<string> finalJournalEntries)
        {
            int meals = mealsCooked >= 0 ? mealsCooked : _state.mealsCooked;
            int bullets = bulletsFired >= 0 ? bulletsFired : _state.bulletsFired;

            var record = new EpilogueRecord
            {
                mealsCooked = Math.Max(0, meals),
                bulletsFired = Math.Max(0, bullets),
                daysSurvived = Math.Max(0, daysSurvived),
                survivorsDead = deadSurvivors != null
                    ? new List<string>(deadSurvivors)
                    : new List<string>(),
                deathLocations = deathRoomIds != null
                    ? new List<string>(deathRoomIds)
                    : new List<string>(),
                journalEntries = finalJournalEntries != null
                    ? new List<string>(finalJournalEntries)
                    : new List<string>()
            };

            _lastRecord = record;
            // Keep counters / snapshot in state for save.
            _state.mealsCooked = record.mealsCooked;
            _state.bulletsFired = record.bulletsFired;
            _state.daysSurvived = record.daysSurvived;
            _state.survivorsDead = new List<string>(record.survivorsDead);
            _state.deathLocations = new List<string>(record.deathLocations);
            _state.journalEntries = new List<string>(record.journalEntries);
            _state.hasRecord = true;

            OnEpilogueGenerated?.Invoke(record);
            return record;
        }

        /// <summary>
        /// Build a haunting narrative summary from the epilogue record.
        /// </summary>
        public string GetNarrativeSummary(EpilogueRecord record)
        {
            if (record == null) return "The bunker is empty. No one remembers.";

            int dead = record.survivorsDead != null ? record.survivorsDead.Count : 0;
            int meals = record.mealsCooked;
            int bullets = record.bulletsFired;
            int days = record.daysSurvived;

            string dayWord = days == 1 ? "day" : "days";
            string mealWord = meals == 1 ? "meal" : "meals";
            string bulletWord = bullets == 1 ? "bullet" : "bullets";

            var sb = new System.Text.StringBuilder();
            sb.Append($"They lasted {days} {dayWord}. ");
            sb.Append($"{meals} {mealWord} cooked over charcoal. ");
            sb.Append($"{bullets} {bulletWord} fired into the dark.");

            if (dead > 0)
            {
                sb.Append(" ");
                if (dead == 1)
                {
                    string name = !string.IsNullOrEmpty(record.survivorsDead[0])
                        ? record.survivorsDead[0]
                        : "someone";
                    string room = (record.deathLocations != null && record.deathLocations.Count > 0
                                   && !string.IsNullOrEmpty(record.deathLocations[0]))
                        ? $" in {record.deathLocations[0]}"
                        : "";
                    sb.Append($"{name} died{room}. No one was left to bury them.");
                }
                else
                {
                    sb.Append($"{dead} people died under concrete. ");
                    sb.Append("The rooms remember what the survivors could not.");
                }
            }

            if (record.journalEntries != null && record.journalEntries.Count > 0)
            {
                sb.Append(" The last journal entry read: ");
                sb.Append('"');
                sb.Append(record.journalEntries[record.journalEntries.Count - 1]);
                sb.Append('"');
            }

            sb.Append(" The bunker is empty now. The wind writes the rest.");
            return sb.ToString();
        }

        /// <summary>
        /// Expansion IV — The Receipt of Survival. A brutal administrative audit
        /// printed on the MoralChronicleUI. The screen is black. A dot-matrix
        /// printer sound plays. Text types out, line by line.
        /// </summary>
        public string GenerateReceiptOfSurvival(EpilogueRecord record)
        {
            if (record == null) return "NO RECORD FOUND.";

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("  RECEIPT OF SURVIVAL — ASHFALL");
            sb.AppendLine("  TESSARAT CIVIL DEFENSE DISTRICT");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine();

            // ── On Resource Management ────────────────────────────────
            sb.AppendLine("── ON RESOURCE MANAGEMENT ──────────────");
            sb.AppendLine($"  Total Calories Consumed: {record.totalCaloriesConsumed:N0}.");
            sb.AppendLine($"  Total Calories Denied to Keep the Generator Running: {record.caloriesDeniedForGenerator:N0}.");
            sb.AppendLine($"  Survivors Who Died of Starvation While the Pantry Held >10 Items: {record.starvationDeathsWhilePantryHeld}.");
            sb.AppendLine();
            sb.AppendLine("  The Math: You chose the heater over the hungry.");
            sb.AppendLine("  The heater kept the pipes from freezing. The pipes");
            sb.AppendLine("  kept the water flowing. The water kept the rest of");
            sb.AppendLine("  you alive. The math is correct. The ghosts do not");
            sb.AppendLine("  care about the math.");
            sb.AppendLine();

            // ── On Violence & The Hatch ───────────────────────────────
            sb.AppendLine("── ON VIOLENCE & THE HATCH ─────────────");
            sb.AppendLine($"  Raiders Repelled: {record.raidersRepelled}.");
            sb.AppendLine($"  Raiders Executed After Surrendering: {record.raidersExecutedAfterSurrender}.");
            sb.AppendLine($"  Bullets Fired into Unarmed Scavengers: {record.bulletsIntoUnarmedScavengers}.");
            sb.AppendLine();
            sb.AppendLine("  The Math: Ammunition is finite. Mercy is a luxury");
            sb.AppendLine("  of the well-fed. You saved the bunker. You lost");
            sb.AppendLine("  the ability to look at the children without seeing");
            sb.AppendLine("  the boy's face.");
            sb.AppendLine();

            // ── On The Dead & The Bureaucracy ─────────────────────────
            sb.AppendLine("── ON THE DEAD & THE BUREAUCRACY ───────");
            sb.AppendLine($"  Corpses Processed for Materials: {record.corpsesProcessedForMaterials}.");
            sb.AppendLine($"  Gold Fillings Extracted and Traded for Iodine: {record.goldFillingsExtracted}.");
            sb.AppendLine($"  Clothing Stripped and Worn by Survivors: {record.clothingStrippedArticles} articles.");
            sb.AppendLine();
            sb.AppendLine("  The Math: The dead do not need coats. The living");
            sb.AppendLine("  freeze without them. You became an undertaker who");
            sb.AppendLine("  bills the clients.");
            sb.AppendLine();

            // ── On The Radio & Lies ───────────────────────────────────
            sb.AppendLine("── ON THE RADIO & LIES ─────────────────");
            sb.AppendLine($"  Decoy Broadcasts Transmitted: {record.decoyBroadcastsSent}.");
            sb.AppendLine($"  Faction Patrols Diverted into Hazard Zones: {record.factionPatrolsDiverted}.");
            sb.AppendLine($"  Estimated Casualties Caused by Your Deception: {record.estimatedDeceptionCasualties}.");
            sb.AppendLine();
            sb.AppendLine("  The Math: You did not pull the trigger. You just");
            sb.AppendLine("  moved the target. The ash buried them just the same.");
            sb.AppendLine();

            // ── On The Undercity ──────────────────────────────────────
            if (record.lightlessTrades > 0 || record.lightlessKilled > 0 || record.crematoriumVisits > 0)
            {
                sb.AppendLine("── ON THE UNDERCITY ────────────────────");
                if (record.crematoriumVisits > 0)
                    sb.AppendLine($"  Crematorium Visits: {record.crematoriumVisits}.");
                if (record.lightlessTrades > 0)
                    sb.AppendLine($"  Trades Completed with the Lightless: {record.lightlessTrades}.");
                if (record.lightlessKilled > 0)
                    sb.AppendLine($"  Lightless Killed: {record.lightlessKilled}.");
                sb.AppendLine();
                sb.AppendLine("  The Math: They were not monsters. They were people");
                sb.AppendLine("  who went underground and never came back up. You");
                sb.AppendLine("  traded candle tallow for water filters. The math");
                sb.AppendLine("  was fair. The dark does not care about fairness.");
                sb.AppendLine();
            }

            // ── Final Frame ───────────────────────────────────────────
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($"  DAYS SURVIVED: {record.daysSurvived}");
            sb.AppendLine($"  SOULS LOST: {record.survivorsDead?.Count ?? 0}");
            sb.AppendLine($"  VICTORY TYPE: {record.victoryType ?? "NONE"}");
            sb.AppendLine("═══════════════════════════════════════");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the room IDs where survivors died, for top-down highlight rendering.
        /// </summary>
        public List<string> GetTopDownHighlights(EpilogueRecord record)
        {
            if (record == null || record.deathLocations == null)
                return new List<string>();

            return new List<string>(record.deathLocations);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public EpilogueStatsState CaptureState()
        {
            var copy = new EpilogueStatsState
            {
                systemId = "system_epilogue_stats",
                mealsCooked = _state.mealsCooked,
                bulletsFired = _state.bulletsFired,
                daysSurvived = _state.daysSurvived,
                hasRecord = _state.hasRecord || _lastRecord != null,
                survivorsDead = new List<string>(),
                deathLocations = new List<string>(),
                journalEntries = new List<string>()
            };

            if (_lastRecord != null)
            {
                copy.mealsCooked = _lastRecord.mealsCooked;
                copy.bulletsFired = _lastRecord.bulletsFired;
                copy.daysSurvived = _lastRecord.daysSurvived;
                CopyList(_lastRecord.survivorsDead, copy.survivorsDead);
                CopyList(_lastRecord.deathLocations, copy.deathLocations);
                CopyList(_lastRecord.journalEntries, copy.journalEntries);
                copy.hasRecord = true;
            }
            else
            {
                CopyList(_state.survivorsDead, copy.survivorsDead);
                CopyList(_state.deathLocations, copy.deathLocations);
                CopyList(_state.journalEntries, copy.journalEntries);
            }

            return copy;
        }

        public void RestoreState(EpilogueStatsState saved)
        {
            if (saved == null)
            {
                _state = new EpilogueStatsState();
                _lastRecord = null;
                return;
            }

            _state = new EpilogueStatsState
            {
                systemId = "system_epilogue_stats",
                mealsCooked = Math.Max(0, saved.mealsCooked),
                bulletsFired = Math.Max(0, saved.bulletsFired),
                daysSurvived = Math.Max(0, saved.daysSurvived),
                hasRecord = saved.hasRecord,
                survivorsDead = new List<string>(),
                deathLocations = new List<string>(),
                journalEntries = new List<string>()
            };
            CopyList(saved.survivorsDead, _state.survivorsDead);
            CopyList(saved.deathLocations, _state.deathLocations);
            CopyList(saved.journalEntries, _state.journalEntries);

            if (saved.hasRecord)
            {
                _lastRecord = new EpilogueRecord
                {
                    mealsCooked = _state.mealsCooked,
                    bulletsFired = _state.bulletsFired,
                    daysSurvived = _state.daysSurvived,
                    survivorsDead = new List<string>(_state.survivorsDead),
                    deathLocations = new List<string>(_state.deathLocations),
                    journalEntries = new List<string>(_state.journalEntries)
                };
            }
            else
            {
                _lastRecord = null;
            }
        }

        private static void CopyList(List<string> src, List<string> dst)
        {
            if (src == null || dst == null) return;
            for (int i = 0; i < src.Count; i++)
            {
                if (!string.IsNullOrEmpty(src[i]))
                    dst.Add(src[i]);
            }
        }
    }
}
