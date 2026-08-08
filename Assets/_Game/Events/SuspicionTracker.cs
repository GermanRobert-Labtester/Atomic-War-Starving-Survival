using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Internal mysteries: when food or water fall below 10% of inventory capacity,
    /// the threat turns inward. Tracks resource pressure, picks a weighted suspect,
    /// and drives the Missing Rations chain (interrogate / lock pantry / ignore).
    /// </summary>
    public class SuspicionTracker
    {
        public const float ResourceStarvedThreshold = 0.10f;
        public const float HoursUntilMystery = 24f;
        public const float IgnoreVanishHours = 48f;
        public const float InnocentMoralePenalty = -30f;
        public const float GroupTraumaMorale = -20f;
        public const float AffinityInterrogateHit = -25f;
        public const string MechanicalPartsId = "mechanical_parts";
        public const string DefaultFoodId = "canned_food";

        public const string MissingRationsEventId = "missing_rations";
        public const string MissingRationsAgainEventId = "missing_rations_again";
        public const string MissingRationsCaughtEventId = "missing_rations_caught";

        public const string ChoiceInterrogate = "interrogate";
        public const string ChoiceLockPantry = "lock_pantry";
        public const string ChoiceIgnore = "ignore_theft";
        public const string ChoiceBanish = "banish_to_ash";
        public const string ChoiceForgive = "forgive_thief";

        public const string FlagMysteryActive = "missing_rations_active";
        public const string FlagPantryLocked = "pantry_locked";
        public const string FlagThiefBanished = "thief_banished";
        public const string FlagThiefForgiven = "thief_forgiven";
        public const string FlagBunkerFractured = "bunker_fractured";
        public const string DisabilityFractured = "fractured";

        // ── Live state ───────────────────────────────────────────────────

        /// <summary>Pressure active: food or water under threshold.</summary>
        public bool IsResourceStarved { get; private set; }

        /// <summary>Hours spent continuously resource-starved (resets when stock recovers).</summary>
        public float StarvedHours;

        /// <summary>A mystery event has been presented and is unresolved.</summary>
        public bool MysteryOpen;

        /// <summary>Player chose Ignore — rations keep vanishing on a 48h cadence.</summary>
        public bool IgnoreActive;

        public float IgnoreHoursAccum;
        public string TrueThiefId = string.Empty;
        public string AccusedId = string.Empty;
        public bool ThiefCaught;
        public bool BunkerFractured;
        public int VanishCount;
        public bool PantryLocked;

        public event Action<GameEvent, EventContext> OnMysteryEventReady;
        public event Action<string, int> OnRationVanished; // itemId hint, amount

        private EventRunner _boundRunner;

        // ── Evaluation ───────────────────────────────────────────────────

        public static bool EvaluateResourceStarved(Inventory.Inventory inventory)
        {
            if (inventory == null) return false;
            return inventory.FoodFillRatio() < ResourceStarvedThreshold
                || inventory.WaterFillRatio() < ResourceStarvedThreshold;
        }

        public void RefreshStarved(Inventory.Inventory inventory)
        {
            IsResourceStarved = EvaluateResourceStarved(inventory);
        }

        /// <summary>
        /// Hourly tick. Accumulates starved pressure; fires Missing Rations after 24h.
        /// While Ignore is active, deletes 1 food every 48h and fires the follow-up.
        /// </summary>
        public void Tick(float gameHours, EventContext context, EventRunner runner)
        {
            if (gameHours <= 0f || context == null) return;

            RefreshStarved(context.Inventory);
            context.IsResourceStarved = IsResourceStarved;
            context.Suspicion = this;

            if (PantryLocked)
            {
                // Locked stores stop the vanish loop and new missing-ration pressure.
                IgnoreActive = false;
                StarvedHours = 0f;
                return;
            }

            if (IgnoreActive)
            {
                IgnoreHoursAccum += gameHours;
                while (IgnoreHoursAccum >= IgnoreVanishHours)
                {
                    IgnoreHoursAccum -= IgnoreVanishHours;
                    PerformVanish(context, runner);
                }
                return;
            }

            if (!IsResourceStarved)
            {
                StarvedHours = 0f;
                return;
            }

            if (MysteryOpen) return;

            StarvedHours += gameHours;
            if (StarvedHours < HoursUntilMystery) return;

            StarvedHours = 0f;
            BeginMissingRations(context, runner);
        }

        // ── Suspect selection ────────────────────────────────────────────

        /// <summary>
        /// Weighted pick among living crew, excluding the POV/player character.
        /// High weight: low morale, Fatalist, BingeEater (trait or active break), high hunger.
        /// </summary>
        public static Survivor PickSuspect(
            IList<Survivor> crew,
            string playerSurvivorId,
            System.Random rng)
        {
            if (crew == null || crew.Count == 0) return null;

            var candidates = new List<Survivor>();
            var weights = new List<float>();
            float total = 0f;

            for (int i = 0; i < crew.Count; i++)
            {
                var s = crew[i];
                if (s == null || !s.IsAlive) continue;
                if (!string.IsNullOrEmpty(playerSurvivorId)
                    && string.Equals(s.Id, playerSurvivorId, StringComparison.Ordinal))
                    continue;

                float w = WeightFor(s);
                if (w <= 0f) continue;
                candidates.Add(s);
                weights.Add(w);
                total += w;
            }

            // Fallback: if player exclusion emptied the pool, include everyone alive.
            if (candidates.Count == 0)
            {
                for (int i = 0; i < crew.Count; i++)
                {
                    var s = crew[i];
                    if (s == null || !s.IsAlive) continue;
                    float w = WeightFor(s);
                    candidates.Add(s);
                    weights.Add(Mathf.Max(0.01f, w));
                    total += Mathf.Max(0.01f, w);
                }
            }

            if (candidates.Count == 0) return null;
            if (total <= 0f) return candidates[0];

            double roll = rng != null ? rng.NextDouble() * total : UnityEngine.Random.Range(0f, total);
            float accum = 0f;
            for (int i = 0; i < candidates.Count; i++)
            {
                accum += weights[i];
                if (roll <= accum) return candidates[i];
            }
            return candidates[candidates.Count - 1];
        }

        public static float WeightFor(Survivor s)
        {
            if (s == null || !s.IsAlive) return 0f;

            float w = 1f;

            // Low morale → desperation
            float morale = s.Needs != null ? s.Needs.Morale : 50f;
            w += Mathf.Clamp((50f - morale) / 25f, 0f, 2.5f);

            // Hunger
            float hunger = s.Needs != null ? s.Needs.Hunger : 0f;
            w += Mathf.Clamp((hunger - 50f) / 40f, 0f, 1.5f);

            // Traits / bias
            if (s.RiskBias == RiskBiasTrait.Fatalist) w += 2.0f;
            if (s.RiskBias == RiskBiasTrait.Reckless) w += 0.8f;
            if (s.RiskBias == RiskBiasTrait.Denialist) w += 0.6f;
            if (s.RiskBias == RiskBiasTrait.Paranoid) w += 0.3f;

            // Binge eater: personality trait or active mental break
            if (s.HasTrait("binge_eater")
                || string.Equals(s.currentMentalBreakId, MentalBreakSO.Ids.BingeEater, StringComparison.OrdinalIgnoreCase))
            {
                w += 3.0f;
            }

            return Mathf.Max(0.05f, w);
        }

        // ── Mystery lifecycle ────────────────────────────────────────────

        public void BeginMissingRations(EventContext context, EventRunner runner)
        {
            if (context == null) return;

            var crew = context.AllSurvivors;
            var rng = context.Random ?? AtomicWar._Game.Utilities.SeededRandom.CreateFixed("suspiciontracker");

            var thief = PickSuspect(crew, context.PlayerSurvivorId, rng);
            if (thief == null)
            {
                // Solo bunker — nothing to accuse.
                return;
            }

            TrueThiefId = thief.Id;
            // Accused is usually the thief; 25% chance eyes land on someone else (if any).
            AccusedId = TrueThiefId;
            if (rng.NextDouble() < 0.25 && crew != null && crew.Count > 2)
            {
                var alt = PickSuspect(crew, context.PlayerSurvivorId, rng);
                if (alt != null && alt.Id != TrueThiefId)
                    AccusedId = alt.Id;
            }

            MysteryOpen = true;
            ThiefCaught = false;
            context.SetEventFlag(FlagMysteryActive, true);

            // Diegetic discovery of a shortfall (stock already critical).
            // Mechanical deletion continues on the Ignore path every 48h.

            var accused = FindSurvivor(crew, AccusedId);
            var ev = CreateMissingRationsEvent(accused);
            EnsureInPool(runner, ev);

            OnMysteryEventReady?.Invoke(ev, context);
            runner?.Run(ev, context);
        }

        public void PerformVanish(EventContext context, EventRunner runner)
        {
            if (context == null) return;

            int removed = StealOneFood(context.Inventory);
            if (removed > 0)
            {
                VanishCount++;
                OnRationVanished?.Invoke(DefaultFoodId, removed);
            }

            // Follow-up: another missing-rations beat while Ignore continues.
            var accused = FindSurvivor(context.AllSurvivors, AccusedId)
                          ?? FindSurvivor(context.AllSurvivors, TrueThiefId);
            var ev = CreateMissingRationsAgainEvent(accused, VanishCount);
            EnsureInPool(runner, ev);
            MysteryOpen = true;
            OnMysteryEventReady?.Invoke(ev, context);
            runner?.Run(ev, context);
        }

        public static int StealOneFood(Inventory.Inventory inventory)
        {
            if (inventory == null) return 0;
            return inventory.RemoveByType(ItemType.Food, 1);
        }

        // ── Choice resolution ────────────────────────────────────────────

        /// <summary>Bind to EventRunner.OnChoiceApplied to resolve mystery choices.</summary>
        public void Bind(EventRunner runner)
        {
            if (runner == null) return;
            if (_boundRunner != null)
                Unbind(_boundRunner);
            _boundRunner = runner;
            runner.OnChoiceApplied += HandleChoiceApplied;
        }

        public void Unbind(EventRunner runner)
        {
            if (runner == null) return;
            runner.OnChoiceApplied -= HandleChoiceApplied;
            if (_boundRunner == runner)
                _boundRunner = null;
        }

        private void HandleChoiceApplied(GameEvent gameEvent, EventChoice choice, EventContext context)
        {
            if (choice == null || context == null) return;
            string id = choice.ChoiceId ?? string.Empty;
            string eventId = gameEvent != null ? gameEvent.id : string.Empty;

            if (eventId == MissingRationsEventId || eventId == MissingRationsAgainEventId)
            {
                if (id == ChoiceInterrogate) ResolveInterrogate(context);
                else if (id == ChoiceLockPantry) ResolveLockPantry(context);
                else if (id == ChoiceIgnore) ResolveIgnore(context);
            }
            else if (eventId == MissingRationsCaughtEventId)
            {
                if (id == ChoiceBanish) ResolveBanish(context);
                else if (id == ChoiceForgive) ResolveForgive(context);
            }
        }

        public void ResolveInterrogate(EventContext context)
        {
            if (context == null) return;
            IgnoreActive = false;
            IgnoreHoursAccum = 0f;

            bool guilty = !string.IsNullOrEmpty(AccusedId)
                          && string.Equals(AccusedId, TrueThiefId, StringComparison.Ordinal);

            // Affinity hit: primary (interrogator) vs accused
            ApplyAffinityHit(context, AccusedId, AffinityInterrogateHit);

            if (!guilty)
            {
                // Innocent — massive morale penalty on the accused + group unease
                var accused = FindSurvivor(context.AllSurvivors, AccusedId);
                if (accused != null && accused.Needs != null)
                    accused.Needs.Morale = Mathf.Clamp(accused.Needs.Morale + InnocentMoralePenalty, 0f, 100f);
                ApplyGroupMorale(context, -8f, excludeId: AccusedId);
                MysteryOpen = false;
                context.SetEventFlag(FlagMysteryActive, false);
                return;
            }

            // Caught — present banish / forgive
            ThiefCaught = true;
            MysteryOpen = true;
            var thief = FindSurvivor(context.AllSurvivors, TrueThiefId);
            var caught = CreateCaughtEvent(thief);
            PendingCaughtEvent = caught;
            EnsureInPool(_boundRunner, caught);
            OnMysteryEventReady?.Invoke(caught, context);
            _boundRunner?.Run(caught, context);
        }

        /// <summary>Set when interrogate catches the thief (also auto-Run via bound runner).</summary>
        public GameEvent PendingCaughtEvent { get; private set; }

        public GameEvent ConsumePendingCaughtEvent()
        {
            var ev = PendingCaughtEvent;
            PendingCaughtEvent = null;
            return ev;
        }

        public void ResolveLockPantry(EventContext context)
        {
            if (context == null) return;

            // Requires mechanical_parts — remove from live stock by slot ref
            // (never `new ItemDefinition`; SO instances must come from inventory).
            var slot = context.Inventory?.FindSlot(MechanicalPartsId);
            if (slot?.Item == null || context.Inventory.CountById(MechanicalPartsId) < 1)
            {
                // Soft fail — no lock, mystery stays open.
                return;
            }

            context.Inventory.Remove(slot.Item, 1);

            PantryLocked = true;
            IgnoreActive = false;
            IgnoreHoursAccum = 0f;
            MysteryOpen = false;
            context.SetEventFlag(FlagPantryLocked, true);
            context.SetEventFlag(FlagMysteryActive, false);
            ApplyGroupMorale(context, -10f);
        }

        public void ResolveIgnore(EventContext context)
        {
            if (context == null) return;
            IgnoreActive = true;
            IgnoreHoursAccum = 0f;
            MysteryOpen = false; // closed until next vanish follow-up
            context.SetEventFlag(FlagMysteryActive, true);
        }

        public void ResolveBanish(EventContext context)
        {
            if (context == null) return;
            var thief = FindSurvivor(context.AllSurvivors, TrueThiefId);
            if (thief != null)
                thief.State = SurvivorState.Dead;

            ApplyGroupMorale(context, GroupTraumaMorale);
            // Group trauma flag
            context.SetEventFlag(FlagThiefBanished, true);
            context.SetEventFlag(FlagMysteryActive, false);
            MysteryOpen = false;
            IgnoreActive = false;
            ThiefCaught = true;
        }

        public void ResolveForgive(EventContext context)
        {
            if (context == null) return;
            var thief = FindSurvivor(context.AllSurvivors, TrueThiefId);
            if (thief != null)
            {
                thief.IsFractured = true;
                if (thief.DisabilityIds == null)
                    thief.DisabilityIds = new List<string>();
                if (!thief.HasDisability(DisabilityFractured))
                    thief.DisabilityIds.Add(DisabilityFractured);
            }

            BunkerFractured = true;
            context.SetEventFlag(FlagThiefForgiven, true);
            context.SetEventFlag(FlagBunkerFractured, true);
            context.SetEventFlag(FlagMysteryActive, false);
            ApplyGroupMorale(context, -5f);
            MysteryOpen = false;
            IgnoreActive = false;
            ThiefCaught = true;
        }

        // ── Event factories ──────────────────────────────────────────────

        public static GameEvent CreateMissingRationsEvent(Survivor accused)
        {
            string name = accused != null && !string.IsNullOrEmpty(accused.DisplayName)
                ? accused.DisplayName
                : "Someone";
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = MissingRationsEventId;
            ev.title = "Missing Rations";
            ev.bodyText =
                $"A can of beans is missing. The stack is short by one. " +
                $"{name} was near the stores after lights-out. Nobody admits it.";
            ev.weight = 0f; // tracker-fired, not random pool
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequireResourceStarved = true
            };
            ev.choices = BuildInvestigationChoices();
            return ev;
        }

        public static GameEvent CreateMissingRationsAgainEvent(Survivor accused, int vanishCount)
        {
            string name = accused != null && !string.IsNullOrEmpty(accused.DisplayName)
                ? accused.DisplayName
                : "Someone";
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = MissingRationsAgainEventId;
            ev.title = "Missing Rations — Again";
            ev.bodyText =
                $"Another ration is gone. That makes {Mathf.Max(1, vanishCount)}. " +
                $"The silence around {name} is getting louder.";
            ev.weight = 0f;
            ev.conditions = new EventConditions { MinDay = 1, RequireResourceStarved = true };
            ev.choices = BuildInvestigationChoices();
            return ev;
        }

        public static GameEvent CreateCaughtEvent(Survivor thief)
        {
            string name = thief != null && !string.IsNullOrEmpty(thief.DisplayName)
                ? thief.DisplayName
                : "They";
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = MissingRationsCaughtEventId;
            ev.title = "Caught";
            ev.bodyText =
                $"{name} breaks. The empty tin is still in their pocket. " +
                "The bunker is too small for this. There is no court. Only the hatch.";
            ev.weight = 0f;
            ev.conditions = new EventConditions { MinDay = 1 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = ChoiceBanish,
                    Text = "Banish them to the ash.",
                    MoraleDelta = GroupTraumaMorale,
                    SetEventFlags = new List<string> { FlagThiefBanished }
                },
                new EventChoice
                {
                    ChoiceId = ChoiceForgive,
                    Text = "Forgive them. Keep them. Live with it.",
                    MoraleDelta = -5f,
                    SetEventFlags = new List<string> { FlagThiefForgiven, FlagBunkerFractured }
                }
            };
            return ev;
        }

        public static List<GameEvent> CreateMissingRationsChain()
        {
            return new List<GameEvent>
            {
                CreateMissingRationsEvent(null),
                CreateMissingRationsAgainEvent(null, 1),
                CreateCaughtEvent(null)
            };
        }

        private static List<EventChoice> BuildInvestigationChoices()
        {
            return new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = ChoiceInterrogate,
                    Text = "Interrogate them. Make them answer.",
                    MoraleDelta = -5f
                },
                new EventChoice
                {
                    ChoiceId = ChoiceLockPantry,
                    Text = "Lock the pantry. Spend parts. Spend the day.",
                    MoraleDelta = -8f
                    // mechanical_parts consumed in ResolveLockPantry
                },
                new EventChoice
                {
                    ChoiceId = ChoiceIgnore,
                    Text = "Ignore it. We can't afford a witch hunt.",
                    MoraleDelta = -2f
                }
            };
        }

        // ── Save ─────────────────────────────────────────────────────────

        public SuspicionTrackerSave CaptureState()
        {
            return new SuspicionTrackerSave
            {
                StarvedHours = StarvedHours,
                MysteryOpen = MysteryOpen,
                IgnoreActive = IgnoreActive,
                IgnoreHoursAccum = IgnoreHoursAccum,
                TrueThiefId = TrueThiefId ?? string.Empty,
                AccusedId = AccusedId ?? string.Empty,
                ThiefCaught = ThiefCaught,
                BunkerFractured = BunkerFractured,
                VanishCount = VanishCount,
                PantryLocked = PantryLocked
            };
        }

        public void RestoreState(SuspicionTrackerSave save)
        {
            if (save == null)
            {
                StarvedHours = 0f;
                MysteryOpen = false;
                IgnoreActive = false;
                IgnoreHoursAccum = 0f;
                TrueThiefId = string.Empty;
                AccusedId = string.Empty;
                ThiefCaught = false;
                BunkerFractured = false;
                VanishCount = 0;
                PantryLocked = false;
                return;
            }

            StarvedHours = save.StarvedHours;
            MysteryOpen = save.MysteryOpen;
            IgnoreActive = save.IgnoreActive;
            IgnoreHoursAccum = save.IgnoreHoursAccum;
            TrueThiefId = save.TrueThiefId ?? string.Empty;
            AccusedId = save.AccusedId ?? string.Empty;
            ThiefCaught = save.ThiefCaught;
            BunkerFractured = save.BunkerFractured;
            VanishCount = save.VanishCount;
            PantryLocked = save.PantryLocked;
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private static void EnsureInPool(EventRunner runner, GameEvent ev)
        {
            if (runner == null || ev == null) return;
            if (runner.FindInPool(ev.id) != null) return;
            var list = new List<GameEvent>();
            if (runner.Pool != null)
            {
                for (int i = 0; i < runner.Pool.Count; i++)
                    list.Add(runner.Pool[i]);
            }
            list.Add(ev);
            runner.SetPool(list);
        }

        private static Survivor FindSurvivor(IList<Survivor> crew, string id)
        {
            if (crew == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < crew.Count; i++)
            {
                if (crew[i] != null && crew[i].Id == id) return crew[i];
            }
            return null;
        }

        private static void ApplyGroupMorale(EventContext context, float delta, string excludeId = null)
        {
            if (context?.AllSurvivors == null) return;
            for (int i = 0; i < context.AllSurvivors.Count; i++)
            {
                var s = context.AllSurvivors[i];
                if (s == null || !s.IsAlive || s.Needs == null) continue;
                if (!string.IsNullOrEmpty(excludeId) && s.Id == excludeId) continue;
                s.Needs.Morale = Mathf.Clamp(s.Needs.Morale + delta, 0f, 100f);
            }
        }

        private static void ApplyAffinityHit(EventContext context, string otherId, float delta)
        {
            if (context?.MentalBreak == null || context.PrimarySurvivor == null) return;
            if (string.IsNullOrEmpty(otherId)) return;
            string a = context.PrimarySurvivor.Id;
            if (string.IsNullOrEmpty(a) || a == otherId) return;
            context.MentalBreak.Affinity.Adjust(a, otherId, delta);
        }
    }

    [Serializable]
    public class SuspicionTrackerSave
    {
        public float StarvedHours;
        public bool MysteryOpen;
        public bool IgnoreActive;
        public float IgnoreHoursAccum;
        public string TrueThiefId;
        public string AccusedId;
        public bool ThiefCaught;
        public bool BunkerFractured;
        public int VanishCount;
        public bool PantryLocked;
    }
}
