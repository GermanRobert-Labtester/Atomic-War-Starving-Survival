// System_MilitiaContributionTax.cs — Upland Provincial Militia tax ledger
// (Expansion II: The Weight of Factions). Each village starts at a 10% tithe,
// escalates by 5% per missed week, capped at 50%. Refusal withdraws protection
// after a 3-day grace; the scavenger warlords move in. Two paid weeks in a row
// reinstates protection.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Militia Contribution Tax system.
    /// One MilitiaTaxEntry per village on the militia's books.
    /// </summary>
    [Serializable]
    public class MilitiaContributionTaxState
    {
        public string system_id = "system_militia_contribution_tax";
        public List<MilitiaTaxEntry> entries = new List<MilitiaTaxEntry>();
    }

    /// <summary>
    /// Per-village tax record. Tracks the current base rate, paid/refused
    /// streaks, whether protection is currently withdrawn, and the most
    /// recent collector id.
    /// </summary>
    [Serializable]
    public class MilitiaTaxEntry
    {
        public string village_id;
        public float current_tax_rate;
        public int consecutive_paid_weeks;
        public int consecutive_refused_weeks;
        public bool protection_withdrawn;
        public string last_collector_id;

        public MilitiaTaxEntry() { }

        public MilitiaTaxEntry(string id, float startRate)
        {
            village_id = id;
            current_tax_rate = startRate;
            consecutive_paid_weeks = 0;
            consecutive_refused_weeks = 0;
            protection_withdrawn = false;
            last_collector_id = string.Empty;
        }

        public MilitiaTaxEntry Clone()
        {
            return new MilitiaTaxEntry
            {
                village_id = village_id,
                current_tax_rate = current_tax_rate,
                consecutive_paid_weeks = consecutive_paid_weeks,
                consecutive_refused_weeks = consecutive_refused_weeks,
                protection_withdrawn = protection_withdrawn,
                last_collector_id = last_collector_id
            };
        }
    }

    /// <summary>
    /// Upland Militia tax book. Rates are the militia's "rent" for protection;
    /// refusal escalates them and eventually drops the village off the patrol
    /// route. No hard references to other systems; use the public events.
    /// </summary>
    public class System_MilitiaContributionTax
    {
        // Ids (snake_case)
        public const string MilitiaTaxSystemId = "system_militia_contribution_tax";
        public const string SlotId = "militia_contribution_tax";

        // Lore rules
        public const float StartingTaxRate = 0.10f;
        public const float MaxTaxRate = 0.50f;
        public const float TaxEscalationPerWeek = 0.05f;
        public const int RefusalGraceDays = 3;
        public const int ReinstatedAfterPaidWeeks = 2;

        // Events
        public event Action<string, float> OnTaxRateChanged;
        public event Action<string> OnProtectionWithdrawn;
        public event Action<string> OnProtectionReinstated;

        private MilitiaContributionTaxState _state = new MilitiaContributionTaxState();

        public IReadOnlyList<MilitiaTaxEntry> Entries => _state.entries;

        public int EntryCount => _state.entries.Count;

        public MilitiaTaxEntry GetEntry(string villageId)
        {
            if (string.IsNullOrEmpty(villageId)) return null;
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e != null && e.village_id == villageId) return e;
            }
            return null;
        }

        // Set initial tax rate for a village; if unknown, creates the entry.
        // Returns the rate that was applied.
        public float SetVillageInitialRate(string villageId, float startRate)
        {
            if (string.IsNullOrEmpty(villageId)) return StartingTaxRate;
            var entry = GetOrCreate(villageId);
            float clamped = ClampRate(startRate);
            if (entry.current_tax_rate != clamped)
            {
                entry.current_tax_rate = clamped;
                OnTaxRateChanged?.Invoke(villageId, clamped);
            }
            return entry.current_tax_rate;
        }

        // Record a tax payment. Resets refusal streak, increments paid streak,
        // and reinstates protection after ReinstatedAfterPaidWeeks good weeks.
        public void PayTax(string villageId, int weekIndex)
        {
            if (string.IsNullOrEmpty(villageId)) return;
            var entry = GetOrCreate(villageId);

            entry.consecutive_paid_weeks += 1;
            entry.consecutive_refused_weeks = 0;

            // Each paid week also slightly de-escalates the rate back to the
            // starting point (ratchet-down) — keeps the militia humane.
            float deEscalation = TaxEscalationPerWeek;
            float target = StartingTaxRate;
            float newRate = entry.current_tax_rate - deEscalation;
            if (newRate < target) newRate = target;
            if (newRate != entry.current_tax_rate)
            {
                entry.current_tax_rate = ClampRate(newRate);
                OnTaxRateChanged?.Invoke(villageId, entry.current_tax_rate);
            }

            if (entry.protection_withdrawn && entry.consecutive_paid_weeks >= ReinstatedAfterPaidWeeks)
            {
                entry.protection_withdrawn = false;
                entry.consecutive_paid_weeks = 0;
                OnProtectionReinstated?.Invoke(villageId);
            }
        }

        // Record a refusal. Escalates the rate by TaxEscalationPerWeek,
        // capped at MaxTaxRate. If the streak reaches RefusalGraceDays
        // (in days, not weeks) the system flips protection_withdrawn.
        public void RefuseTax(string villageId, int weekIndex)
        {
            if (string.IsNullOrEmpty(villageId)) return;
            var entry = GetOrCreate(villageId);

            entry.consecutive_refused_weeks += 1;
            entry.consecutive_paid_weeks = 0;

            float newRate = entry.current_tax_rate + TaxEscalationPerWeek;
            newRate = ClampRate(newRate);
            if (newRate != entry.current_tax_rate)
            {
                entry.current_tax_rate = newRate;
                OnTaxRateChanged?.Invoke(villageId, newRate);
            }

            if (!entry.protection_withdrawn && entry.consecutive_refused_weeks >= RefusalGraceDays)
            {
                entry.protection_withdrawn = true;
                entry.consecutive_paid_weeks = 0;
                OnProtectionWithdrawn?.Invoke(villageId);
            }
        }

        // Force-withdraw protection (e.g. triggered by external event).
        public void WithdrawProtection(string villageId)
        {
            if (string.IsNullOrEmpty(villageId)) return;
            var entry = GetOrCreate(villageId);
            if (entry.protection_withdrawn) return;
            entry.protection_withdrawn = true;
            entry.consecutive_paid_weeks = 0;
            OnProtectionWithdrawn?.Invoke(villageId);
        }

        // Force-reinstate protection (e.g. a collector was bribed).
        public void ReinstateProtection(string villageId)
        {
            if (string.IsNullOrEmpty(villageId)) return;
            var entry = GetOrCreate(villageId);
            if (!entry.protection_withdrawn) return;
            entry.protection_withdrawn = false;
            entry.consecutive_paid_weeks = 0;
            OnProtectionReinstated?.Invoke(villageId);
        }

        // Set the id of the last collector who visited the village.
        public void SetLastCollector(string villageId, string collectorId)
        {
            if (string.IsNullOrEmpty(villageId)) return;
            var entry = GetOrCreate(villageId);
            entry.last_collector_id = collectorId ?? string.Empty;
        }

        // The "effective" rate the village actually owes this week. Equals
        // the base rate — escalation is folded in directly via RefuseTax.
        // Public helper exposed per spec.
        public float GetEffectiveTaxRate(string villageId)
        {
            var e = GetEntry(villageId);
            if (e == null) return StartingTaxRate;
            return e.current_tax_rate;
        }

        public bool IsProtectionWithdrawn(string villageId)
        {
            var e = GetEntry(villageId);
            return e != null && e.protection_withdrawn;
        }

        // Save/Load (deep copy)
        public MilitiaContributionTaxState CaptureState()
        {
            var copy = new MilitiaContributionTaxState
            {
                system_id = "system_militia_contribution_tax",
                entries = new List<MilitiaTaxEntry>()
            };
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e == null || string.IsNullOrEmpty(e.village_id)) continue;
                copy.entries.Add(e.Clone());
            }
            return copy;
        }

        public void RestoreState(MilitiaContributionTaxState saved)
        {
            if (saved == null)
            {
                _state = new MilitiaContributionTaxState();
                return;
            }
            _state = new MilitiaContributionTaxState
            {
                system_id = "system_militia_contribution_tax",
                entries = new List<MilitiaTaxEntry>()
            };
            for (int i = 0; i < saved.entries.Count; i++)
            {
                var e = saved.entries[i];
                if (e == null || string.IsNullOrEmpty(e.village_id)) continue;
                _state.entries.Add(e.Clone());
            }
        }

        private static float ClampRate(float r)
        {
            if (r < StartingTaxRate) return StartingTaxRate;
            if (r > MaxTaxRate) return MaxTaxRate;
            return r;
        }

        private MilitiaTaxEntry GetOrCreate(string villageId)
        {
            var existing = GetEntry(villageId);
            if (existing != null) return existing;
            var fresh = new MilitiaTaxEntry(villageId, StartingTaxRate);
            _state.entries.Add(fresh);
            return fresh;
        }
    }
}
