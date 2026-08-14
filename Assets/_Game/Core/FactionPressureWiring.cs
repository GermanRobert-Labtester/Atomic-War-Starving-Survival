// FactionPressureWiring.cs — Wires the four Expansion II faction-pressure
// systems (GarrisonComplianceLedger, MilitiaContributionTax, CultLeash,
// WarlordTribute) into the host's event runner, raid pipeline, cult quest,
// and FactionRadioInterceptSystem. Static class with Func<>/Action<>
// injection so GameBootstrap can stay decoupled from the systems.
using System;
using Ashfall.Core.Economy;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Quests;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Canonical hook points for the four Expansion II faction-pressure
    /// systems. The host (GameBootstrap) injects provider funcs and invokes
    /// <see cref="WireIntoHost"/> once at boot. <see cref="HandleRaidResolved"/>
    /// is the per-raid entry point — the only place that routes a raid
    /// outcome into the right system.
    /// </summary>
    public static class FactionPressureWiring
    {
        public const string WiringSystemId = "system_faction_pressure_wiring";
        public const string LedgerSystemId = System_GarrisonComplianceLedger.LedgerSystemId;
        public const string MilitiaSystemId = System_MilitiaContributionTax.MilitiaTaxSystemId;
        public const string CultSystemId = System_CultLeash.CultLeashSystemId;
        public const string WarlordSystemId = System_WarlordTribute.WarlordTributeSystemId;

        public const string FactionMilitary = FactionSO.Ids.MilitaryRemnants;
        public const string FactionMilitia  = FactionSO.Ids.UplandMilitia;
        public const string FactionCult    = FactionSO.Ids.CultOfTheGlow;
        public const string FactionScav    = FactionSO.Ids.ScavengerCamp;

        // ── Canonical hook points (host-injected) ────────────────────────
        public static System_GarrisonComplianceLedger GarrisonLedger;
        public static System_MilitiaContributionTax MilitiaTax;
        public static System_CultLeash CultLeash;
        public static System_WarlordTribute WarlordTribute;

        // ── Host-injected providers (Func<>/Action<>) ────────────────────
        public static Func<string> ShelterIdProvider = () => "shelter_player";
        public static Func<int> DayProvider = () => 0;
        public static Func<float, float> TaxRateClamp = Mathf.Clamp01;
        public static Func<int> RadioDayProvider;

        public static FactionRadioInterceptSystem RadioIntercepts;

        private static bool _wiredGarrison;
        private static bool _wiredMilitia;
        private static bool _wiredCult;
        private static bool _wiredWarlord;
        private static bool _wiredHost;
        private static DynamicEconomySystem _wiredEconomy;

        // ── WireIntoHost: called once at boot by GameBootstrap ───────────
        public static void WireIntoHost(GameBootstrap host)
        {
            if (host == null)
            {
                Debug.LogWarning("[FactionPressureWiring] WireIntoHost called with null host; providers must be set manually.");
            }

            if (host != null && !_wiredHost)
            {
                var economy = TryGetEconomyFromHost(host);
                if (economy != null)
                {
                    economy.OnRaidResolved -= HandleRaidResolved;
                    economy.OnRaidResolved += HandleRaidResolved;
                    _wiredEconomy = economy;
                }
                _wiredHost = true;
            }

            if (RadioIntercepts != null)
            {
                if (GarrisonLedger != null && !_wiredGarrison)
                {
                    GarrisonLedger.OnStrikeRecorded += (id, n) =>
                        PushPressureLine(FactionMilitary, "STRIKE " + n + "/3 at " + id);
                    GarrisonLedger.OnNonCompliant += id =>
                        PushPressureLine(FactionMilitary, "NON-COMPLIANT: " + id);
                    GarrisonLedger.OnReinstated += id =>
                        PushPressureLine(FactionMilitary, "REINSTATED: " + id);
                    _wiredGarrison = true;
                }

                if (MilitiaTax != null && !_wiredMilitia)
                {
                    MilitiaTax.OnTaxRateChanged += (id, r) =>
                        PushPressureLine(FactionMilitia, "TITHE " + Mathf.RoundToInt(r * 100f) + "%");
                    MilitiaTax.OnProtectionWithdrawn += id =>
                        PushPressureLine(FactionMilitia, "PROTECTION WITHDRAWN: " + id);
                    MilitiaTax.OnProtectionReinstated += id =>
                        PushPressureLine(FactionMilitia, "PROTECTION REINSTATED: " + id);
                    _wiredMilitia = true;
                }

                if (CultLeash != null && !_wiredCult)
                {
                    CultLeash.OnVisitRecorded += (id, n) =>
                        PushPressureLine(FactionCult, "VISIT " + n + " at " + id);
                    CultLeash.OnBlessed += id =>
                        PushPressureLine(FactionCult, "BLESSED: " + id);
                    CultLeash.OnCommunionMissed += (id, w) =>
                        PushPressureLine(FactionCult, "COMMUNION MISSED " + w + "w: " + id);
                    CultLeash.OnLeaveAttempted += id =>
                        PushPressureLine(FactionCult, "LEAVE ATTEMPTED: " + id);
                    _wiredCult = true;
                }

                if (WarlordTribute != null && !_wiredWarlord)
                {
                    WarlordTribute.OnTributeSet += (id, amt) =>
                        PushPressureLine(FactionScav, "TRIBUTE SET " + amt.ToString("0.0") + " for " + id);
                    WarlordTribute.OnShortPaymentEscalated += (id, w) =>
                        PushPressureLine(FactionScav, "SHORT PAY x" + w + " wks: " + id);
                    WarlordTribute.OnLeaveOneThingGiven += id =>
                        PushPressureLine(FactionScav, "LEAVE-ONE-THING GIVEN: " + id);
                    WarlordTribute.OnShelterBurned += id =>
                        PushPressureLine(FactionScav, "SHELTER BURNED: " + id);
                    _wiredWarlord = true;
                }
            }
        }

        public static void Unwire()
        {
            if (_wiredEconomy != null)
            {
                _wiredEconomy.OnRaidResolved -= HandleRaidResolved;
                _wiredEconomy = null;
            }
            if (_attachedCultQuest != null && _cultQuestHandler != null)
            {
                _attachedCultQuest.OnCommunionMissed -= _cultQuestHandler;
                _attachedCultQuest = null;
                _cultQuestHandler = null;
            }
            _wiredHost = false;
            _wiredGarrison = false;
            _wiredMilitia = false;
            _wiredCult = false;
            _wiredWarlord = false;
            GarrisonLedger = null;
            MilitiaTax = null;
            CultLeash = null;
            WarlordTribute = null;
            RadioIntercepts = null;
        }

        // ── HandleRaidResolved: the per-raid router ──────────────────────
        public static void HandleRaidResolved(FactionRaidResult result)
        {
            if (result == null) return;
            if (!result.Launched) return;

            string fid = result.FactionId ?? string.Empty;
            string shelterId = SafeShelterId();

            if (fid == FactionMilitary)
            {
                if (GarrisonLedger == null) return;
                if (result.Repelled)
                {
                    GarrisonLedger.RecordRequisition(shelterId, "raid_" + SafeDay());
                }
                if (result.Breached)
                {
                    GarrisonLedger.FileNonCompliance(shelterId, "breach_" + SafeDay());
                }
            }
            else if (fid == FactionMilitia)
            {
                if (MilitiaTax == null) return;
                if (result.Breached)
                {
                    MilitiaTax.RefuseTax(shelterId, SafeDay());
                }
                else if (result.Repelled)
                {
                    // Repelled militia = no tax owed this week; paying the
                    // base rate ticks the "consecutive_paid_weeks" counter
                    // toward reinstatement if it was withdrawn.
                    MilitiaTax.PayTax(shelterId, SafeDay());
                }
            }
            else if (fid == FactionScav)
            {
                if (WarlordTribute == null) return;
                if (result.Repelled)
                {
                    if (result.StolenItemCount == 0)
                    {
                        WarlordTribute.FulfillLeaveOneThing(shelterId, "loot_cached");
                    }
                    else
                    {
                        float required = WarlordTribute.GetRequiredTribute(shelterId);
                        float paid = Mathf.Max(0f, required - result.StolenItemCount);
                        WarlordTribute.PayShort(shelterId, paid, SafeDay());
                    }
                }
                if (result.Breached)
                {
                    WarlordTribute.PayShort(shelterId, 0f, SafeDay());
                }
            }
            else if (fid == FactionCult)
            {
                if (CultLeash == null) return;
                if (result.Repelled || result.Breached)
                {
                    // Cult raid repelled = shelter didn't honor communion.
                    // Use RecordMissedCommunion (the system's canonical
                    // "miss" entry point); this is what the cult quest's
                    // OnCommunionMissed event also drives. One missed week
                    // per raid resolved.
                    CultLeash.RecordMissedCommunion(shelterId);
                }
            }
        }

        // ── HandleChoiceApplied: optional EventRunner hook ──────────────
        public static void HandleChoiceApplied(object ev, object choice, object ctx)
        {
            if (ev == null) return;
            string evId = TryGetStringProp(ev, "Id") ?? TryGetStringProp(ev, "QuestId") ?? string.Empty;
            if (string.IsNullOrEmpty(evId)) return;
            if (evId != "quest_cult_glow_communion") return;
            string choiceId = TryGetStringProp(choice, "Id") ?? string.Empty;
            string shelterId = SafeShelterId();
            if (CultLeash == null) return;

            if (choiceId == "refuse_invitation" || choiceId == "refuse_convert")
            {
                CultLeash.RecordMissedCommunion(shelterId);
            }
            else if (choiceId == "give_convert")
            {
                CultLeash.RecordVisit(shelterId);
            }
        }

        // ── AttachCultQuest: host wires the quest's new OnCommunionMissed
        //    event directly to the cult leash system. Idempotent.
        private static Quest_CultGlowCommunion _attachedCultQuest;
        private static Action<string, int> _cultQuestHandler;

        public static void AttachCultQuest(Quest_CultGlowCommunion quest)
        {
            if (quest == null) return;
            if (ReferenceEquals(_attachedCultQuest, quest)) return;
            // Unsubscribe old if any.
            if (_attachedCultQuest != null && _cultQuestHandler != null)
            {
                _attachedCultQuest.OnCommunionMissed -= _cultQuestHandler;
                _attachedCultQuest = null;
                _cultQuestHandler = null;
            }
            _attachedCultQuest = quest;
            _cultQuestHandler = (shelterId, missedWeeks) =>
            {
                if (CultLeash == null) return;
                if (string.IsNullOrEmpty(shelterId)) shelterId = SafeShelterId();
                if (string.IsNullOrEmpty(shelterId)) return;
                CultLeash.RecordMissedCommunion(shelterId, missedWeeks);
            };
            quest.OnCommunionMissed += _cultQuestHandler;
        }

        // ── Small helpers ───────────────────────────────────────────────
        private static void PushPressureLine(string factionId, string msg)
        {
            if (RadioIntercepts == null || string.IsNullOrEmpty(msg)) return;
            int day = SafeRadioDay();
            // Reuse HatchRepel for any pressure one-liner (per spec: do not
            // add new InterceptKind values; the lore line carries the
            // signal). The faction_id is the discriminator.
            RadioIntercepts.Push(
                factionId,
                FactionRadioInterceptSystem.InterceptKind.HatchRepel,
                msg,
                day);
        }

        private static string SafeShelterId()
        {
            try
            {
                string s = ShelterIdProvider != null ? ShelterIdProvider() : null;
                return string.IsNullOrEmpty(s) ? "shelter_player" : s;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[FactionPressureWiring] SafeShelterId failed: {ex.Message}");
                return "shelter_player";
            }
        }

        private static int SafeDay()
        {
            try { return DayProvider != null ? DayProvider() : 0; }
            catch (Exception ex)
            {
                Debug.LogWarning($"[FactionPressureWiring] SafeDay failed: {ex.Message}");
                return 0;
            }
        }

        private static int SafeRadioDay()
        {
            try
            {
                if (RadioDayProvider != null) return RadioDayProvider();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[FactionPressureWiring] SafeRadioDay failed: {ex.Message}");
            }
            return SafeDay();
        }

        private static DynamicEconomySystem TryGetEconomyFromHost(GameBootstrap host)
        {
            if (host == null) return null;
            try
            {
                var prop = host.GetType().GetProperty("EconomySystem",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);
                if (prop != null) return prop.GetValue(host) as DynamicEconomySystem;
                var fld = host.GetType().GetField("EconomySystem",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
                if (fld != null) return fld.GetValue(host) as DynamicEconomySystem;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[FactionPressureWiring] Could not reflect host.EconomySystem: " + ex.Message);
            }
            return null;
        }

        private static string TryGetStringProp(object o, string name)
        {
            if (o == null || string.IsNullOrEmpty(name)) return null;
            try
            {
                var p = o.GetType().GetProperty(name);
                if (p != null && p.PropertyType == typeof(string))
                    return p.GetValue(o) as string;
                var f = o.GetType().GetField(name);
                if (f != null && f.FieldType == typeof(string))
                    return f.GetValue(o) as string;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[FactionPressureWiring] TryGetStringProp '{name}' failed: {ex.Message}");
            }
            return null;
        }
    }
}
