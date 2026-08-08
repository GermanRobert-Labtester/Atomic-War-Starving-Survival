using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Shelter electricity grid: sums generation and draw, auto-sheds low-priority
    /// loads when demand exceeds supply, tracks diesel CO, and supports bicycle
    /// pedaling as manual generation when fuel runs out.
    /// </summary>
    [Serializable]
    public class PowerNetwork
    {
        private readonly List<PowerSourceInstance> _sources = new List<PowerSourceInstance>();
        private readonly List<PowerConsumer> _consumers = new List<PowerConsumer>();
        private readonly Dictionary<string, PowerSourceSO> _defs = new Dictionary<string, PowerSourceSO>();

        public float TotalGeneration { get; private set; }
        public float TotalDraw { get; private set; }
        /// <summary>Requested draw before load-shedding (diagnostic / UI).</summary>
        public float RequestedDraw { get; private set; }
        /// <summary>True when generation is zero while loads are still requested.</summary>
        public bool IsBlackout { get; private set; }
        /// <summary>True when at least one requested load was shed this balance.</summary>
        public bool IsLoadShedding { get; private set; }
        /// <summary>Accumulated CO ppm from diesel generation (Prompt #20 air-quality hook).</summary>
        public float CarbonMonoxidePpm { get; private set; }
        /// <summary>
        /// Prompt #380 — gasoline varnish degradation: multiplies diesel fuel burn.
        /// Set by FuelDecaySystem (1 / efficiency). Default 1 = undegraded fuel.
        /// </summary>
        public float FuelDegradationBurnMultiplier { get; set; } = 1f;

        public IReadOnlyList<PowerSourceInstance> Sources => _sources;
        public IReadOnlyList<PowerConsumer> Consumers => _consumers;

        public event Action OnPowerStateChanged;
        public event Action OnBlackout;
        public event Action OnLoadShed;
        public event Action<float> OnCoChanged;

        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>Prompt #226 — Grid Walker: no gen breakdown/fire; 150% capacity.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        /// <summary>Prompt #239 — Voice of the Wastes zeroes radio power draw.</summary>
        public float GetConsumerWatts(PowerConsumer c)
        {
            if (c == null) return 0f;
            if (c.ModuleId == "radio" && _personalQuests != null
                && _personalQuests.RadioPowerIsFree(_getSurvivors?.Invoke()))
                return 0f;
            float watts = c.Watts;
            // #283 Ruthless Exec: +20% module efficiency → same work for less draw.
            if (watts > 0f && _personalQuests != null && _getSurvivors != null)
            {
                var list = _getSurvivors();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var ex = list[i];
                        if (ex == null || !ex.IsAlive) continue;
                        float m = _personalQuests.GetRuthlessModuleEfficiencyMultiplier(ex);
                        if (m > 1f)
                        {
                            watts /= m;
                            break;
                        }
                    }
                }
            }
            return watts;
        }

        /// <summary>#280 Tech Bro offline-tablet waste (watts) when unsupervised.</summary>
        public float GetTechBroPowerWasteWatts(bool supervised = false)
        {
            if (_personalQuests == null || _getSurvivors == null) return 0f;
            var list = _getSurvivors();
            if (list == null) return 0f;
            float total = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                var sv = list[i];
                if (sv == null || !sv.IsAlive) continue;
                total += _personalQuests.GetTechBroPowerWasteWatts(sv, supervised);
            }
            return total;
        }

        /// <summary>
        /// Inject carbon monoxide from external sources (room fire, sealed smoke).
        /// Internal Horror — Fire in the Hole.
        /// </summary>
        public void AddCarbonMonoxide(float ppm)
        {
            if (ppm <= 0f) return;
            CarbonMonoxidePpm = Mathf.Max(0f, CarbonMonoxidePpm + ppm);
            OnCoChanged?.Invoke(CarbonMonoxidePpm);
        }

        // -----------------------------------------------------------------
        // Registration
        // -----------------------------------------------------------------

        public void RegisterSourceDefinition(PowerSourceSO def)
        {
            if (def == null || string.IsNullOrEmpty(def.SourceId)) return;
            _defs[def.SourceId] = def;
        }

        public void AddSource(PowerSourceInstance instance)
        {
            if (instance == null || string.IsNullOrEmpty(instance.SourceId)) return;
            if (instance.Definition == null && _defs.TryGetValue(instance.SourceId, out var def))
                instance.Definition = def;
            if (instance.Definition != null)
                RegisterSourceDefinition(instance.Definition);

            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i] != null && _sources[i].SourceId == instance.SourceId)
                {
                    _sources[i] = instance;
                    Rebalance();
                    return;
                }
            }
            _sources.Add(instance);
            Rebalance();
        }

        public void AddConsumer(PowerConsumer consumer)
        {
            if (consumer == null || string.IsNullOrEmpty(consumer.ModuleId)) return;
            consumer.Priority = Mathf.Clamp(consumer.Priority, 1, 5);
            for (int i = 0; i < _consumers.Count; i++)
            {
                if (_consumers[i] != null && _consumers[i].ModuleId == consumer.ModuleId)
                {
                    _consumers[i] = consumer;
                    Rebalance();
                    return;
                }
            }
            _consumers.Add(consumer);
            Rebalance();
        }

        public PowerConsumer GetConsumer(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return null;
            for (int i = 0; i < _consumers.Count; i++)
            {
                if (_consumers[i] != null && _consumers[i].ModuleId == moduleId)
                    return _consumers[i];
            }
            return null;
        }

        public PowerSourceInstance GetSource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId)) return null;
            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i] != null && _sources[i].SourceId == sourceId)
                    return _sources[i];
            }
            return null;
        }

        public bool IsModulePowered(string moduleId)
        {
            var c = GetConsumer(moduleId);
            return c != null && c.IsPowered;
        }

        // -----------------------------------------------------------------
        // Player controls
        // -----------------------------------------------------------------

        /// <summary>Set consumer priority (1 = critical, 5 = shed first).</summary>
        public void SetPriority(string moduleId, int priority)
        {
            var c = GetConsumer(moduleId);
            if (c == null) return;
            c.Priority = Mathf.Clamp(priority, 1, 5);
            Rebalance();
        }

        /// <summary>Player request toggle — module wants power when true.</summary>
        public void SetRequested(string moduleId, bool requested)
        {
            var c = GetConsumer(moduleId);
            if (c == null) return;
            c.IsRequested = requested;
            Rebalance();
        }

        /// <summary>Cycle priority 1→2→…→5→1 for UI priority toggles.</summary>
        public int CyclePriority(string moduleId)
        {
            var c = GetConsumer(moduleId);
            if (c == null) return 0;
            int next = c.Priority >= 5 ? 1 : c.Priority + 1;
            SetPriority(moduleId, next);
            return next;
        }

        public void SetSourceEnabled(string sourceId, bool enabled)
        {
            var s = GetSource(sourceId);
            if (s == null) return;
            s.IsEnabled = enabled;
            Rebalance();
        }

        public void AssignPedaler(string sourceId, string survivorId)
        {
            var s = GetSource(sourceId);
            if (s == null) return;
            s.AssignPedaler(survivorId);
            Rebalance();
        }

        public void ClearPedaler(string sourceId)
        {
            var s = GetSource(sourceId);
            if (s == null) return;
            s.ClearPedaler();
            Rebalance();
        }

        /// <summary>Find any bicycle source that can accept a pedaler.</summary>
        public PowerSourceInstance FindAvailableBicycle()
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                var s = _sources[i];
                if (s == null || !s.IsEnabled) continue;
                var def = s.Definition ?? (_defs.TryGetValue(s.SourceId, out var d) ? d : null);
                if (def == null) continue;
                if (def.Kind == PowerSourceKind.Bicycle && def.RequiresPedaling && !s.HasPedaler)
                    return s;
            }
            return null;
        }

        public bool HasActivePedaler()
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                var s = _sources[i];
                if (s != null && s.IsEnabled && s.HasPedaler) return true;
            }
            return false;
        }

        public float GetDieselFuelTotal()
        {
            float total = 0f;
            for (int i = 0; i < _sources.Count; i++)
            {
                var s = _sources[i];
                if (s == null) continue;
                var def = ResolveDef(s);
                if (def != null && def.Kind == PowerSourceKind.Diesel)
                    total += s.Fuel;
            }
            return total;
        }

        // -----------------------------------------------------------------
        // Tick + balance
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance generation (fuel/CO/pedaling costs) then rebalance loads.
        /// <paramref name="weatherName"/> gates wind/solar.
        /// <paramref name="tryApplyPedalCost"/> is invoked for each active bicycle
        /// pedaler as (survivorId, fatigueDelta, hungerDelta) and returns false if
        /// the rider is unavailable/exhausted (pedaler is then cleared). Kept as a
        /// callback so Shelter does not hard-depend on the Survivors assembly.
        /// </summary>
        public void Tick(
            float gameHours,
            string weatherName = null,
            Func<string, float, float, bool> tryApplyPedalCost = null)
        {
            if (gameHours <= 0f)
            {
                Rebalance(weatherName);
                return;
            }

            float coDelta = 0f;

            for (int i = 0; i < _sources.Count; i++)
            {
                var src = _sources[i];
                if (src == null || !src.IsEnabled) continue;
                var def = ResolveDef(src);
                if (def == null) continue;

                bool canGenerate = CanSourceGenerate(src, def, weatherName);
                if (!canGenerate) continue;

                switch (def.Kind)
                {
                    case PowerSourceKind.Diesel:
                        if (src.Fuel <= 0f) break;
                        // #260 Supply Chain Master: bunker-wide diesel burn mult (0.7).
                        float bunkerFuelMult = _personalQuests != null
                            ? _personalQuests.GetBunkerFuelBurnMultiplier(_getSurvivors?.Invoke())
                            : 1f;
                        float degradeMult = FuelDegradationBurnMultiplier > 0f
                            ? FuelDegradationBurnMultiplier
                            : 1f;
                        float burn = def.FuelPerHour * gameHours
                                     * src.EffectiveFuelBurnMultiplier * bunkerFuelMult * degradeMult;
                        src.Fuel = Mathf.Max(0f, src.Fuel - burn);
                        if (def.CoPpmPerHour > 0f)
                            coDelta += def.CoPpmPerHour * gameHours;
                        // Mechanical wear: overworked gens become fire risks (Internal Horror).
                        // Prompt #226 — Grid Walker: generators never break down.
                        bool immune = _personalQuests != null
                            && _personalQuests.GeneratorsImmuneToBreakdown(_getSurvivors?.Invoke());
                        if (!immune)
                        {
                            float wear = 0.15f * gameHours;
                            // #283 Ruthless Exec: modules work harder and die faster.
                            if (_personalQuests != null && _getSurvivors != null)
                            {
                                var list = _getSurvivors();
                                if (list != null)
                                {
                                    for (int wi = 0; wi < list.Count; wi++)
                                    {
                                        var ex = list[wi];
                                        if (ex == null || !ex.IsAlive) continue;
                                        float m = _personalQuests.GetRuthlessModuleWearMultiplier(ex);
                                        if (m > 1f) { wear *= m; break; }
                                    }
                                }
                            }
                            src.Durability = Mathf.Max(0f, src.Durability - wear);
                        }
                        break;

                    case PowerSourceKind.Bicycle:
                        if (!src.HasPedaler) break;
                        if (tryApplyPedalCost == null)
                        {
                            // No host callback: keep generation but skip need drain.
                            break;
                        }
                        float fat = def.FatiguePerHour * gameHours;
                        float hun = def.HungerPerHour * gameHours;
                        if (!tryApplyPedalCost(src.PedalingSurvivorId, fat, hun))
                        {
                            src.ClearPedaler();
                        }
                        break;
                }
            }

            if (coDelta > 0f)
            {
                CarbonMonoxidePpm = Mathf.Max(0f, CarbonMonoxidePpm + coDelta);
                OnCoChanged?.Invoke(CarbonMonoxidePpm);
            }

            // Natural slow CO bleed when no diesel is running (very small).
            if (coDelta <= 0f && CarbonMonoxidePpm > 0f)
            {
                CarbonMonoxidePpm = Mathf.Max(0f, CarbonMonoxidePpm - 0.5f * gameHours);
            }

            Rebalance(weatherName);
        }

        /// <summary>
        /// Recompute generation/draw and auto-shed low-priority loads so that
        /// TotalDraw &lt;= TotalGeneration when possible. Higher priority numbers
        /// shed first (e.g. priority 2 heater before priority 1 filter).
        /// </summary>
        public void Rebalance(string weatherName = null)
        {
            TotalGeneration = ComputeGeneration(weatherName);

            float requested = 0f;
            for (int i = 0; i < _consumers.Count; i++)
            {
                var c = _consumers[i];
                if (c == null) continue;
                c.IsShed = false;
                if (c.IsRequested)
                    requested += Mathf.Max(0f, GetConsumerWatts(c));
            }
            // #280 Tech Bro: unsupervised tablet wastes power.
            requested += GetTechBroPowerWasteWatts();
            RequestedDraw = requested;

            // Shed highest priority number first until under budget.
            if (requested > TotalGeneration + 0.0001f)
            {
                // Build shed order: priority desc, then watts desc (shed largest luxury first).
                var order = new List<PowerConsumer>();
                for (int i = 0; i < _consumers.Count; i++)
                {
                    var c = _consumers[i];
                    if (c != null && c.IsRequested && GetConsumerWatts(c) > 0f)
                        order.Add(c);
                }
                order.Sort((a, b) =>
                {
                    int p = b.Priority.CompareTo(a.Priority);
                    if (p != 0) return p;
                    return b.Watts.CompareTo(a.Watts);
                });

                float remaining = requested;
                for (int i = 0; i < order.Count && remaining > TotalGeneration + 0.0001f; i++)
                {
                    order[i].IsShed = true;
                    remaining -= order[i].Watts;
                }
            }

            float draw = 0f;
            bool anyShed = false;
            bool anyRequested = false;
            for (int i = 0; i < _consumers.Count; i++)
            {
                var c = _consumers[i];
                if (c == null) continue;
                if (c.IsRequested) anyRequested = true;
                if (c.IsShed) anyShed = true;
                // Prompt #239 — free radio draw when Voice of the Wastes is present.
                if (!c.IsShed && c.IsRequested)
                    draw += GetConsumerWatts(c);
            }
            TotalDraw = draw;
            IsLoadShedding = anyShed;

            bool wasBlackout = IsBlackout;
            // Full blackout: no generation while something still wants power.
            IsBlackout = TotalGeneration <= 0.0001f && anyRequested;

            OnPowerStateChanged?.Invoke();
            if (anyShed) OnLoadShed?.Invoke();
            if (IsBlackout && !wasBlackout)
            {
                OnBlackout?.Invoke();

                // #318 Core: dies the instant the power network fails (unless Omniscience).
                if (_personalQuests != null && _getSurvivors != null)
                {
                    var survivors = _getSurvivors();
                    if (survivors != null)
                    {
                        for (int i = 0; i < survivors.Count; i++)
                        {
                            var sv = survivors[i];
                            if (sv != null && sv.IsAlive)
                                _personalQuests.NotifyPowerNetworkFailure(sv);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply powered/shed state onto shelter module IsEnabled flags.
        /// Modules not registered as consumers are left untouched.
        /// </summary>
        public void ApplyToShelter(Shelter shelter)
        {
            if (shelter == null) return;
            for (int i = 0; i < _consumers.Count; i++)
            {
                var c = _consumers[i];
                if (c == null) continue;
                var mod = shelter.GetModule(c.ModuleId);
                if (mod == null) continue;
                // Powered only when requested and not shed.
                mod.IsEnabled = c.IsPowered;
            }
        }

        // -----------------------------------------------------------------
        // Internals
        // -----------------------------------------------------------------

        private PowerSourceSO ResolveDef(PowerSourceInstance src)
        {
            if (src.Definition != null) return src.Definition;
            if (_defs.TryGetValue(src.SourceId, out var d))
            {
                src.Definition = d;
                return d;
            }
            return null;
        }

        private float ComputeGeneration(string weatherName)
        {
            float gen = 0f;
            for (int i = 0; i < _sources.Count; i++)
            {
                var src = _sources[i];
                if (src == null || !src.IsEnabled) continue;
                var def = ResolveDef(src);
                if (def == null) continue;
                if (!CanSourceGenerate(src, def, weatherName)) continue;
                gen += Mathf.Max(0f, def.OutputWatts);
            }
            // Prompt #226 — Grid Walker hotwire: 150% power capacity.
            if (_personalQuests != null)
            {
                float mult = _personalQuests.GetPowerCapacityMultiplier(_getSurvivors?.Invoke());
                if (mult > 1f) gen *= mult;
            }
            return gen;
        }

        private static bool CanSourceGenerate(PowerSourceInstance src, PowerSourceSO def, string weatherName)
        {
            if (def.IsWeatherGated && !def.AllowsWeather(weatherName))
                return false;

            switch (def.Kind)
            {
                case PowerSourceKind.Diesel:
                    return src.Fuel > 0f;
                case PowerSourceKind.Bicycle:
                    return !def.RequiresPedaling || src.HasPedaler;
                default:
                    return true;
            }
        }

        // -----------------------------------------------------------------
        // Factories / save
        // -----------------------------------------------------------------

        /// <summary>
        /// Bootstrap / test factory: diesel 50 W + bicycle 20 W, standard module loads.
        /// Acceptance scenario consumers: filter 30 W P1, heater 40 W P2.
        /// </summary>
        public static PowerNetwork CreateDefault(float dieselFuel = 40f)
        {
            var net = new PowerNetwork();
            var diesel = PowerSourceSO.CreateDieselGenerator(50f);
            var bike = PowerSourceSO.CreateBicycleGenerator(20f);
            net.RegisterSourceDefinition(diesel);
            net.RegisterSourceDefinition(bike);
            net.AddSource(new PowerSourceInstance(diesel, dieselFuel));
            net.AddSource(new PowerSourceInstance(bike, 0f));

            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 30f, 1));
            net.AddConsumer(new PowerConsumer("heater", "Heater", 40f, 2));
            net.AddConsumer(new PowerConsumer("radio", "Radio", 15f, 3));
            net.AddConsumer(new PowerConsumer("grow_light", "Grow Lights", 25f, 4));
            net.AddConsumer(new PowerConsumer("water_purifier", "Water Purifier", 20f, 3));
            // Radio / grow_light / water_purifier start requested=false if not installed; bootstrap re-enables.
            net.GetConsumer("radio").IsRequested = false;
            net.GetConsumer("grow_light").IsRequested = false;
            net.GetConsumer("water_purifier").IsRequested = false;
            net.Rebalance();
            return net;
        }

        public PowerNetworkSave CaptureState()
        {
            var save = new PowerNetworkSave
            {
                CarbonMonoxidePpm = CarbonMonoxidePpm,
                Sources = new PowerSourceSave[_sources.Count],
                Consumers = new PowerConsumerSave[_consumers.Count]
            };
            for (int i = 0; i < _sources.Count; i++)
            {
                var s = _sources[i];
                save.Sources[i] = new PowerSourceSave
                {
                    SourceId = s.SourceId,
                    IsEnabled = s.IsEnabled,
                    Fuel = s.Fuel,
                    FuelBurnMultiplier = s.FuelBurnMultiplier,
                    Durability = s.Durability,
                    PedalingSurvivorId = s.PedalingSurvivorId,
                    RoomId = s.RoomId
                };
            }
            for (int i = 0; i < _consumers.Count; i++)
            {
                var c = _consumers[i];
                save.Consumers[i] = new PowerConsumerSave
                {
                    ModuleId = c.ModuleId,
                    DisplayName = c.DisplayName,
                    Watts = c.Watts,
                    Priority = c.Priority,
                    IsRequested = c.IsRequested,
                    PriorityLocked = c.PriorityLocked
                };
            }
            return save;
        }

        public void RestoreState(PowerNetworkSave save, IList<PowerSourceSO> knownDefs = null)
        {
            if (save == null) return;
            CarbonMonoxidePpm = save.CarbonMonoxidePpm;

            if (knownDefs != null)
            {
                for (int i = 0; i < knownDefs.Count; i++)
                    RegisterSourceDefinition(knownDefs[i]);
            }

            if (save.Sources != null)
            {
                _sources.Clear();
                for (int i = 0; i < save.Sources.Length; i++)
                {
                    var row = save.Sources[i];
                    if (row == null || string.IsNullOrEmpty(row.SourceId)) continue;
                    var inst = new PowerSourceInstance
                    {
                        SourceId = row.SourceId,
                        IsEnabled = row.IsEnabled,
                        Fuel = row.Fuel,
                        // Same guard as Durability: old saves have no key, so a
                        // non-positive value means "stock burn rate", not "free fuel".
                        FuelBurnMultiplier =
                            row.FuelBurnMultiplier > 0f ? row.FuelBurnMultiplier : 1f,
                        Durability = row.Durability > 0f ? row.Durability : 100f,
                        PedalingSurvivorId = row.PedalingSurvivorId,
                        RoomId = row.RoomId
                    };
                    if (_defs.TryGetValue(row.SourceId, out var def))
                        inst.Definition = def;
                    _sources.Add(inst);
                }
            }

            if (save.Consumers != null)
            {
                _consumers.Clear();
                for (int i = 0; i < save.Consumers.Length; i++)
                {
                    var row = save.Consumers[i];
                    if (row == null || string.IsNullOrEmpty(row.ModuleId)) continue;
                    _consumers.Add(new PowerConsumer
                    {
                        ModuleId = row.ModuleId,
                        DisplayName = row.DisplayName,
                        Watts = row.Watts,
                        Priority = Mathf.Clamp(row.Priority, 1, 5),
                        IsRequested = row.IsRequested,
                        PriorityLocked = row.PriorityLocked
                    });
                }
            }

            Rebalance();
        }
    }

    [Serializable]
    public class PowerNetworkSave
    {
        public float CarbonMonoxidePpm;
        public PowerSourceSave[] Sources;
        public PowerConsumerSave[] Consumers;
    }

    [Serializable]
    public class PowerSourceSave
    {
        public string SourceId;
        public bool IsEnabled;
        public float Fuel;
        /// <summary>Prompt #200 Thermodynamics burn multiplier from the last fuel
        /// loader (0.8 = 20% longer). Was not persisted, so a Thermodynamics refuel
        /// reverted to the stock rate on load.</summary>
        public float FuelBurnMultiplier = 1f;
        public float Durability = 100f;
        public string PedalingSurvivorId;
        public string RoomId;
    }

    [Serializable]
    public class PowerConsumerSave
    {
        public string ModuleId;
        public string DisplayName;
        public float Watts;
        public int Priority;
        public bool IsRequested;
        public bool PriorityLocked;
    }
}
