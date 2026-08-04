using System;
using System.Collections.Generic;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #18 — Debt Collector: after a faction digs the hatch open, they
    /// return in <see cref="CollectionDelayDays"/> days demanding half the
    /// bunker's fuel and half its clean water. Pay clears the debt; refuse
    /// sabotages the catchment and cuts the antenna — no bunker combat.
    /// Distinct from the immediate trust slam on dig-out accept.
    /// </summary>
    public class DebtCollectorSystem
    {
        public const int CollectionDelayDays = 20;
        public const float ResourceDemandFraction = 0.5f;
        public const float PayTrustRecover = 8f;

        /// <summary>EMP damage applied to radio state on refuse (destroys antenna).</summary>
        public const float AntennaCutEmpDamage = 100f;

        public const string ChoicePay = "pay_debt";
        public const string ChoiceRefuse = "refuse_debt";
        public const string EventIdPrefix = "evt_debt_collector_";

        public const string FlagCatchmentSabotaged = "catchment_sabotaged";
        public const string FlagAntennaCut = "antenna_cut";
        public const string CatchmentModuleId = "catchment_surface";
        public const string RadioModuleId = "radio";
        public const string HeaterModuleId = "heater";

        private readonly List<DebtEntry> _debts = new List<DebtEntry>();
        private int _seq;

        private DynamicEconomySystem _economy;
        private FactionRadioInterceptSystem _intercepts;
        private Func<int> _getDay;
        private Shelter.Shelter _shelter;
        private WaterStorage _water;
        private Inventory.Inventory _inventory;
        private RadioState _radioState;

        public IReadOnlyList<DebtEntry> Debts => _debts;

        /// <summary>First pending debt whose collector day has arrived and is not resolved.</summary>
        public DebtEntry ActiveCollection
        {
            get
            {
                int day = CurrentDay;
                for (int i = 0; i < _debts.Count; i++)
                {
                    var d = _debts[i];
                    if (d != null && !d.Resolved && d.CollectorDay <= day && d.Presented)
                        return d;
                }
                return null;
            }
        }

        public event Action<DebtEntry> OnDebtScheduled;
        public event Action<DebtEntry, GameEvent> OnCollectorArrived;
        public event Action<DebtEntry> OnDebtResolved;
        public event Action OnStateChanged;

        public void Bind(
            DynamicEconomySystem economy = null,
            FactionRadioInterceptSystem intercepts = null,
            Func<int> getDay = null,
            Shelter.Shelter shelter = null,
            WaterStorage water = null,
            Inventory.Inventory inventory = null,
            RadioState radioState = null)
        {
            _economy = economy;
            _intercepts = intercepts;
            _getDay = getDay ?? (() => 0);
            _shelter = shelter;
            _water = water;
            _inventory = inventory;
            _radioState = radioState;
        }

        public void SetShelter(Shelter.Shelter shelter) => _shelter = shelter;
        public void SetWaterStorage(WaterStorage water) => _water = water;
        public void SetInventory(Inventory.Inventory inventory) => _inventory = inventory;
        public void SetRadioState(RadioState radioState) => _radioState = radioState;

        public int CurrentDay => _getDay != null ? _getDay() : 0;

        /// <summary>
        /// Schedule a collector visit for <paramref name="factionId"/> after
        /// <see cref="CollectionDelayDays"/>. Called when faction dig-out is accepted.
        /// </summary>
        public DebtEntry ScheduleDebt(string factionId, int scheduleDay = -1)
        {
            if (string.IsNullOrEmpty(factionId)) return null;

            int day = scheduleDay >= 0 ? scheduleDay : CurrentDay;
            var entry = new DebtEntry
            {
                Id = $"debt_{++_seq}",
                FactionId = factionId,
                ScheduledDay = day,
                CollectorDay = day + CollectionDelayDays,
                Resolved = false,
                Presented = false,
                Paid = false,
                Refused = false,
                FuelDemanded = 0f,
                WaterDemanded = 0f
            };
            _debts.Add(entry);
            OnDebtScheduled?.Invoke(entry);
            OnStateChanged?.Invoke();
            return entry;
        }

        /// <summary>
        /// Daily tick: present any debt whose collector day has arrived.
        /// </summary>
        public void TickDay(int day)
        {
            for (int i = 0; i < _debts.Count; i++)
            {
                var d = _debts[i];
                if (d == null || d.Resolved || d.Presented) continue;
                if (day < d.CollectorDay) continue;
                PresentCollector(d);
            }
        }

        /// <summary>Force-present a debt (tests / scripted).</summary>
        public GameEvent PresentCollector(DebtEntry debt)
        {
            if (debt == null || debt.Resolved) return null;

            // Snapshot demand from current stocks at arrival.
            float fuel = MeasureTotalFuel(_shelter, _inventory);
            float water = _water != null ? Mathf.Max(0f, _water.CleanWater) : 0f;
            debt.FuelDemanded = Mathf.Max(0f, fuel * ResourceDemandFraction);
            debt.WaterDemanded = Mathf.Max(0f, water * ResourceDemandFraction);
            debt.Presented = true;

            string fac = DisplayName(debt.FactionId);
            _intercepts?.Push(
                debt.FactionId,
                FactionRadioInterceptSystem.InterceptKind.Parley,
                $"Cold knock. {fac} is back for the dig-out debt — half the fuel, half the clean water.",
                CurrentDay);

            var ev = CreateCollectorEvent(debt);
            OnCollectorArrived?.Invoke(debt, ev);
            OnStateChanged?.Invoke();
            return ev;
        }

        public GameEvent CreateCollectorEvent(DebtEntry debt)
        {
            if (debt == null) return null;

            string fac = DisplayName(debt.FactionId);
            float fuel = debt.FuelDemanded;
            float water = debt.WaterDemanded;

            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EventIdPrefix + debt.Id;
            ev.title = "The Debt";
            ev.bodyText =
                $"{fac} stands at the hatch with empty cans and a ledger. " +
                $"They want half — {fuel:0.#} fuel and {water:0.#} clean water. " +
                "Pay, or they walk the roof. They do not need to open the hatch to hurt you.";
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = 1 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = ChoicePay,
                    Text = "Pay. Hand over the cans.",
                    MoraleDelta = -2f,
                    FactionId = debt.FactionId
                },
                new EventChoice
                {
                    ChoiceId = ChoiceRefuse,
                    Text = "Refuse. Close the wheel.",
                    MoraleDelta = -6f,
                    FactionId = debt.FactionId
                }
            };
            return ev;
        }

        public DebtEntry FindDebt(string debtId)
        {
            if (string.IsNullOrEmpty(debtId)) return null;
            for (int i = 0; i < _debts.Count; i++)
            {
                if (_debts[i] != null && _debts[i].Id == debtId)
                    return _debts[i];
            }
            return null;
        }

        public bool ApplyChoice(string debtId, string choiceId, EventContext eventFlags = null)
        {
            var debt = FindDebt(debtId);
            if (debt == null || debt.Resolved) return false;

            if (string.Equals(choiceId, ChoicePay, StringComparison.Ordinal))
                return TryPay(debt, eventFlags);
            if (string.Equals(choiceId, ChoiceRefuse, StringComparison.Ordinal))
                return TryRefuse(debt, eventFlags);
            return false;
        }

        public bool ApplyChoiceFromEvent(GameEvent gameEvent, EventChoice choice, EventContext eventFlags = null)
        {
            if (gameEvent == null || choice == null) return false;
            string debtId = ExtractDebtIdFromEventId(gameEvent.id);
            if (string.IsNullOrEmpty(debtId)) return false;
            return ApplyChoice(debtId, choice.ChoiceId, eventFlags);
        }

        /// <summary>Pay half fuel + half clean water; clear debt; small trust recover.</summary>
        public bool TryPay(DebtEntry debt, EventContext eventFlags = null)
        {
            if (debt == null || debt.Resolved) return false;

            // Recompute demand from current stocks if not yet presented, else use snapshot.
            if (!debt.Presented)
            {
                debt.FuelDemanded = MeasureTotalFuel(_shelter, _inventory) * ResourceDemandFraction;
                debt.WaterDemanded = (_water != null ? _water.CleanWater : 0f) * ResourceDemandFraction;
                debt.Presented = true;
            }

            ConsumeFuel(debt.FuelDemanded, _shelter, _inventory);
            if (_water != null && debt.WaterDemanded > 0f)
                _water.ConsumeClean(debt.WaterDemanded);

            if (_economy != null)
                _economy.ModifyTrust(debt.FactionId, PayTrustRecover);

            debt.Resolved = true;
            debt.Paid = true;
            debt.Refused = false;

            // Short-term dig-out debt flag can clear once paid.
            eventFlags?.SetEventFlag(HatchEntrapmentSystem.FlagFactionDigOutDebt, false);

            _intercepts?.Push(
                debt.FactionId,
                FactionRadioInterceptSystem.InterceptKind.Parley,
                $"Cans change hands. {DisplayName(debt.FactionId)} marks the ledger closed. They leave without looking back.",
                CurrentDay);

            OnDebtResolved?.Invoke(debt);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Refuse payment: sabotage catchment + cut antenna. No combat.
        /// </summary>
        public bool TryRefuse(DebtEntry debt, EventContext eventFlags = null)
        {
            if (debt == null || debt.Resolved) return false;

            SabotageCatchment(_shelter);
            CutAntenna(_shelter, _radioState);

            eventFlags?.SetEventFlag(FlagCatchmentSabotaged, true);
            eventFlags?.SetEventFlag(FlagAntennaCut, true);

            debt.Resolved = true;
            debt.Paid = false;
            debt.Refused = true;
            debt.Presented = true;

            _intercepts?.Push(
                debt.FactionId,
                FactionRadioInterceptSystem.InterceptKind.HatchRepel,
                $"They do not kick the hatch. Footsteps on the roof. Catchment dead. Antenna cut. " +
                $"{DisplayName(debt.FactionId)} walks off. The debt is paid in silence.",
                CurrentDay);

            OnDebtResolved?.Invoke(debt);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool HasPendingDebtFor(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            for (int i = 0; i < _debts.Count; i++)
            {
                var d = _debts[i];
                if (d != null && !d.Resolved
                    && string.Equals(d.FactionId, factionId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public DebtCollectorSave CaptureState()
        {
            var arr = new DebtEntrySave[_debts.Count];
            for (int i = 0; i < _debts.Count; i++)
            {
                var d = _debts[i];
                if (d == null) continue;
                arr[i] = new DebtEntrySave
                {
                    Id = d.Id,
                    FactionId = d.FactionId,
                    ScheduledDay = d.ScheduledDay,
                    CollectorDay = d.CollectorDay,
                    Resolved = d.Resolved,
                    Presented = d.Presented,
                    Paid = d.Paid,
                    Refused = d.Refused,
                    FuelDemanded = d.FuelDemanded,
                    WaterDemanded = d.WaterDemanded
                };
            }
            return new DebtCollectorSave { NextSeq = _seq, Debts = arr };
        }

        public void RestoreState(DebtCollectorSave save)
        {
            _debts.Clear();
            _seq = 0;
            if (save == null) return;
            _seq = Math.Max(0, save.NextSeq);
            if (save.Debts == null) return;
            for (int i = 0; i < save.Debts.Length; i++)
            {
                var e = save.Debts[i];
                if (e == null || string.IsNullOrEmpty(e.Id)) continue;
                _debts.Add(new DebtEntry
                {
                    Id = e.Id,
                    FactionId = e.FactionId ?? string.Empty,
                    ScheduledDay = e.ScheduledDay,
                    CollectorDay = e.CollectorDay,
                    Resolved = e.Resolved,
                    Presented = e.Presented,
                    Paid = e.Paid,
                    Refused = e.Refused,
                    FuelDemanded = e.FuelDemanded,
                    WaterDemanded = e.WaterDemanded
                });
            }
        }

        public void Clear()
        {
            _debts.Clear();
            _seq = 0;
        }

        // -----------------------------------------------------------------
        // Resource helpers
        // -----------------------------------------------------------------

        /// <summary>
        /// Total fuel units: heater tank + radio module fuel + inventory Fuel items.
        /// </summary>
        public static float MeasureTotalFuel(Shelter.Shelter shelter, Inventory.Inventory inventory)
        {
            float total = 0f;
            if (shelter != null)
            {
                var heater = shelter.GetModule(HeaterModuleId);
                if (heater != null) total += Mathf.Max(0f, heater.Fuel);
                var radio = shelter.GetModule(RadioModuleId);
                if (radio != null) total += Mathf.Max(0f, radio.Fuel);
            }
            if (inventory?.Slots != null)
            {
                for (int i = 0; i < inventory.Slots.Count; i++)
                {
                    var slot = inventory.Slots[i];
                    if (slot?.Item == null || slot.Amount <= 0) continue;
                    if (slot.Item.type == ItemType.Fuel)
                        total += slot.Amount;
                }
            }
            return total;
        }

        /// <summary>Remove up to <paramref name="amount"/> fuel from heater, then radio, then inventory.</summary>
        public static float ConsumeFuel(float amount, Shelter.Shelter shelter, Inventory.Inventory inventory)
        {
            if (amount <= 0f) return 0f;
            float remaining = amount;

            if (shelter != null)
            {
                remaining -= DrainModuleFuel(shelter.GetModule(HeaterModuleId), remaining);
                if (remaining > 0f)
                    remaining -= DrainModuleFuel(shelter.GetModule(RadioModuleId), remaining);
            }

            if (remaining > 0f && inventory != null)
            {
                int take = Mathf.CeilToInt(remaining);
                int removed = inventory.RemoveByType(ItemType.Fuel, take);
                remaining -= removed;
            }

            return amount - remaining;
        }

        private static float DrainModuleFuel(ShelterModuleInstance mod, float amount)
        {
            if (mod == null || amount <= 0f || mod.Fuel <= 0f) return 0f;
            float take = Mathf.Min(mod.Fuel, amount);
            mod.Fuel -= take;
            return take;
        }

        public static void SabotageCatchment(Shelter.Shelter shelter)
        {
            if (shelter == null) return;
            var catchment = shelter.GetModule(CatchmentModuleId);
            if (catchment == null)
            {
                // Ensure a sabotaged module exists so the flag has a mechanical home.
                catchment = new ShelterModuleInstance(CatchmentModuleId, 1);
                shelter.AddModule(catchment);
            }
            catchment.IsEnabled = false;
            catchment.FilterHealth = 0f;
        }

        public static void CutAntenna(Shelter.Shelter shelter, RadioState radioState)
        {
            if (shelter != null)
            {
                var radio = shelter.GetModule(RadioModuleId);
                if (radio != null)
                {
                    radio.IsEnabled = false;
                    radio.Fuel = 0f;
                }
            }
            if (radioState != null)
            {
                radioState.ApplyEmpDamage(AntennaCutEmpDamage);
                radioState.AvailableFuel = 0f;
            }
        }

        public static bool IsCatchmentSabotaged(Shelter.Shelter shelter)
        {
            if (shelter == null) return false;
            var catchment = shelter.GetModule(CatchmentModuleId);
            if (catchment == null) return false;
            return !catchment.IsOperational || catchment.FilterHealth <= 0f;
        }

        public static bool IsAntennaCut(Shelter.Shelter shelter, RadioState radioState)
        {
            if (radioState != null && !radioState.IsOperational) return true;
            if (shelter == null) return false;
            var radio = shelter.GetModule(RadioModuleId);
            return radio != null && !radio.IsOperational;
        }

        public static string ExtractDebtIdFromEventId(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return string.Empty;
            if (!eventId.StartsWith(EventIdPrefix, StringComparison.Ordinal)) return string.Empty;
            return eventId.Substring(EventIdPrefix.Length);
        }

        private string DisplayName(string factionId)
        {
            if (_economy == null || string.IsNullOrEmpty(factionId)) return "They";
            var fac = _economy.GetFaction(factionId);
            return fac != null && !string.IsNullOrEmpty(fac.displayName) ? fac.displayName : factionId;
        }
    }

    [Serializable]
    public class DebtEntry
    {
        public string Id;
        public string FactionId;
        public int ScheduledDay;
        public int CollectorDay;
        public bool Resolved;
        public bool Presented;
        public bool Paid;
        public bool Refused;
        public float FuelDemanded;
        public float WaterDemanded;
    }

    [Serializable]
    public class DebtCollectorSave
    {
        public int NextSeq;
        public DebtEntrySave[] Debts;
    }

    [Serializable]
    public class DebtEntrySave
    {
        public string Id;
        public string FactionId;
        public int ScheduledDay;
        public int CollectorDay;
        public bool Resolved;
        public bool Presented;
        public bool Paid;
        public bool Refused;
        public float FuelDemanded;
        public float WaterDemanded;
    }
}
