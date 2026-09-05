using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Serialized per-survivor slice the host persists: needs + radiation.
    /// Stored as a list keyed by survivor id.
    /// </summary>
    public class SurvivorSliceState
    {
        public string id = string.Empty;
        public float hunger;
        public float thirst;
        public float fatigue;
        public float warmth = 100f;
        public float morale = 50f;
        public float health = 100f;
        public float hygiene = 100f;
        public float radiationDose;
        public float lifetimeRadiationExposure;
        public bool hasRadResistance;
        public float radResistanceHoursRemaining;
        public float iodineProtectionTimer;
        public bool hasAcuteSickness;
        public bool hasChronicIllness;
        public bool hasAcuteRadiationSyndrome;
        public bool radiationIsAlive = true;
        public bool wasHungerCritical;
        public bool wasThirstCritical;
        public bool wasWarmthCritical;
        public float maxHealthCap = 100f;
        public bool isDead;
        public bool isAlive = true;
        public string locationKind = "ShelterInterior";
        public string locationId = string.Empty;
    }

    /// <summary>
    /// Thin Godot-host session for the survival loop: NeedsSystem + RadiationSystem
    /// (ported from Unity's Survivors/NeedsSystem and Radiation/RadiationSystem).
    /// Owns a small demo roster of SurvivorNeedsState, ticks needs + radiation on
    /// the hour, and persists per-survivor slices to user:// via SurvivorsSaveStore.
    /// </summary>
    public sealed class SurvivorsHostSession
    : HostSessionBase{
        public NeedsSystem Needs { get; }
        public RadiationSystem Radiation { get; }
        public MaterialShieldingSystem Shelter { get; } = new MaterialShieldingSystem();
        public System.Collections.Generic.List<SurvivorNeedsState> RosterState { get; } =
            new System.Collections.Generic.List<SurvivorNeedsState>();

        /// <summary>
        /// Optional equipped-gear source. When bound, BuildExposure assembles the
        /// survivor's equipped inventory into ExposureContext.WornGear so a worn
        /// gas mask / hazmat suit actually reduces ambient dose (AGENTS Loop 9 gap).
        /// </summary>
        public InventoryHostSession Inventory { get; set; }

        /// <summary>Environmental exposure resolver calculating doses from position, weather, and shelter shielding.</summary>
        public ExposureEnvironmentResolver ExposureResolver { get; } = new ExposureEnvironmentResolver();
        private readonly System.Collections.Generic.Dictionary<string, ExposureEnvironment> _lastExposureEnvironments =
            new System.Collections.Generic.Dictionary<string, ExposureEnvironment>(StringComparer.Ordinal);

        /// <summary>Demo geiger exposure context: one survivor outside, rest sheltered.</summary>
        private readonly System.Collections.Generic.Dictionary<string, RadSurvivorWrapper> _radStates;

        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>
        /// Raised when a survivor dies through the needs/radiation survival
        /// loop. Carries the survivor id, the normalized cause, and a detail
        /// string. The host's SurvivorFateSystem is the subscriber — this is
        /// the single needs/radiation death feed into the unified pipeline.
        /// </summary>
        public event System.Action<string, Ashfall.Core.Survivors.SurvivorDeathCause, string> OnSurvivorDied;

        /// <summary>
        /// True while the survivor is dying of acute radiation sickness — used
        /// to attribute a needs death at 0 HP to radiation rather than to
        /// generic privation. Checked when OnDied fires.
        /// </summary>
        private bool IsDyingOfRadiation(string id)
        {
            var rad = RadStateFor(id);
            return rad != null && rad.HasAcuteRadiationSickness;
        }

        private sealed class RadSurvivorWrapper : SurvivorRadState { }

        public SurvivorsHostSession()
        {
            Needs = new NeedsSystem();
            Radiation = new RadiationSystem(
                exposureContext: s => BuildExposure(s),
                applyNeed: (s, needId, delta) =>
                {
                    var survivor = Find(s.Id);
                    if (survivor == null || needId != "health") return;
                    Needs.Modify(survivor, NeedKind.Health, delta);
                });
            _radStates = new System.Collections.Generic.Dictionary<string, RadSurvivorWrapper>();
            ExposureResolver.ShelterAttenuationProvider = () => Shelter.GetWeakestCeilingAttenuation();

            // Default Holdfast room ceiling shielding
            Shelter.UpgradeCeiling("room_bunker_corridor", MaterialShieldingSystem.WallMaterial.Concrete);
            Shelter.UpgradeCeiling("room_filtration_stack", MaterialShieldingSystem.WallMaterial.Lead);
            Shelter.UpgradeCeiling("room_storage_bay", MaterialShieldingSystem.WallMaterial.Concrete);
            Shelter.UpgradeCeiling("room_bunks_living", MaterialShieldingSystem.WallMaterial.Wood);
            Shelter.UpgradeCeiling("room_radio_tuner", MaterialShieldingSystem.WallMaterial.Concrete);

            Needs.OnNeedChanged += (s, kind, v) => RaiseStateChanged();
            Needs.OnDied += s =>
            {
                // Normalize the cause: a 0-HP death while in acute radiation
                // sickness is a radiation death; everything else is privation.
                var cause = IsDyingOfRadiation(s.Id)
                    ? Ashfall.Core.Survivors.SurvivorDeathCause.Radiation
                    : Ashfall.Core.Survivors.SurvivorDeathCause.Needs;
                LastEvent = $"{s.Id} has died.";
                OnSurvivorDied?.Invoke(s.Id, cause, LastEvent);
                RaiseStateChanged();
            };

            Radiation.OnStatusGained += (radState, status) =>
            {
                if (status == SurvivorStatus.AcuteRadiationSickness)
                {
                    LastEvent = $"RADIATION ALERT: {radState.Id} entered acute radiation sickness.";
                    AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayRadiationAlert();
                    RaiseStateChanged();
                }
                else if (status == SurvivorStatus.ChronicIllness)
                {
                    LastEvent = $"RADIATION ALERT: {radState.Id} developed chronic illness.";
                    RaiseStateChanged();
                }
            };
        }

        public SurvivorRosterSystem Roster { get; } = new SurvivorRosterSystem();

        /// <summary>Load starting roster and initial conditions from starting_survivors.json (the authority).</summary>
        public void LoadStartingRoster(string dataDir, bool failClosed = true)
        {
            if (RosterState.Count > 0) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var detailed = SurvivorStartingStateLoader.LoadDetailed(dataDir, fileIO, serializer);
            if (!detailed.IsSuccess)
            {
                if (failClosed)
                {
                    LastEvent = $"ERROR: Failed to load authoritative starting survivors: {detailed.ErrorMessage}";
                    GD.PrintErr($"[SurvivorsHostSession] {LastEvent}");
                    throw new InvalidOperationException(LastEvent);
                }
                else
                {
                    SeedDemoRoster();
                    return;
                }
            }

            for (int i = 0; i < detailed.Survivors.Count; i++)
            {
                var s = detailed.Survivors[i];
                if (!AddSurvivor(s.id, s.displayName, s.health, s.hunger, s.thirst, s.warmth, s.morale, s.lifetimeDose, s.acuteRad))
                {
                    if (failClosed)
                    {
                        throw new InvalidOperationException($"Failed to register starting survivor '{s.id}': duplicate survivor ID.");
                    }
                }
            }
        }

        /// <summary>Seed the demo roster with canonical survivor ids from the master list.</summary>
        public void SeedDemoRoster()
        {
            if (RosterState.Count > 0) return;
            AddSurvivor("survivor_dr_sarah_chen", "Dr. Sarah Chen (Trauma Surgeon)", health: 90f, hunger: 20f, thirst: 25f, warmth: 85f, morale: 70f, lifetimeDose: 14f);
            AddSurvivor("survivor_gunner_mikhail", "Gunner Mikhail (Heavy Artillery Loader)", health: 80f, hunger: 35f, thirst: 30f, warmth: 75f, morale: 55f, lifetimeDose: 38f, acuteRad: true);
            AddSurvivor("elena_vasquez", "Elena Vasquez (Aridoculture Engineer)", health: 95f, hunger: 15f, thirst: 20f, warmth: 90f, morale: 65f, lifetimeDose: 8f);
        }

        /// <summary>Load the survivors.json catalog into the roster system (the authority).</summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            Roster.RegisterRange(SurvivorCatalogLoader.Load(dataDir, fileIO, serializer));
        }

        public bool AddSurvivor(
            string id,
            string displayName,
            float health = 100f,
            float hunger = 0f,
            float thirst = 0f,
            float warmth = 100f,
            float morale = 50f,
            float lifetimeDose = 0f,
            bool acuteRad = false)
        {
            if (Find(id) != null) return false;
            Roster.RegisterDefinition(new SurvivorDefinition
            {
                id = id,
                displayName = displayName,
                baseHealth = 100f
            });
            Roster.Join(id, 0);
            var state = new SurvivorNeedsState
            {
                Id = id,
                Health = health,
                Hunger = hunger,
                Thirst = thirst,
                Warmth = warmth,
                Morale = morale
            };
            RosterState.Add(state);
            Needs.Register(state);
            var rad = new RadSurvivorWrapper
            {
                Id = id,
                RadiationDose = acuteRad ? 15f : 0f,
                LifetimeRadiationExposure = lifetimeDose,
                HasAcuteRadiationSickness = acuteRad
            };
            _radStates[id] = rad;
            Radiation.Register(rad);
            return true;
        }

        public SurvivorNeedsState? Find(string id)
        {
            for (int i = 0; i < RosterState.Count; i++)
                if (RosterState[i] != null && RosterState[i].Id == id) return RosterState[i];
            return null;
        }

        public SurvivorRadState? RadStateFor(string id)
        {
            return _radStates.TryGetValue(id, out var r) ? r : null;
        }

        private ExposureContext BuildExposure(SurvivorRadState state)
        {
            var env = ExposureResolver.Resolve(state.Id);
            _lastExposureEnvironments[state.Id] = env;
            return env.ToExposureContext(CollectWornGear());
        }

        /// <summary>Get the last calculated exposure environment snapshot for UI and debug traces.</summary>
        public ExposureEnvironment? GetLastExposureEnvironment(string survivorId)
        {
            return _lastExposureEnvironments.TryGetValue(survivorId, out var env) ? env : null;
        }

        /// <summary>Set survivor location explicitly (e.g. ShelterInterior, ShelterPerimeter, WastelandOutdoors).</summary>
        public void SetSurvivorLocation(string survivorId, SurvivorExposureLocation kind, string locationId = "")
        {
            ExposureResolver.SetSurvivorLocation(survivorId, kind, locationId);
        }

        /// <summary>Get current resolved survivor location.</summary>
        public (SurvivorExposureLocation Kind, string LocationId) GetSurvivorLocation(string survivorId)
        {
            return ExposureResolver.GetSurvivorLocation(survivorId);
        }

        /// <summary>Bind weather provider supplying outdoor rad modifier (from WeatherSystem).</summary>
        public void BindWeatherProvider(Func<float> weatherRadModifierProvider)
        {
            ExposureResolver.WeatherRadModifierProvider = weatherRadModifierProvider;
        }

        /// <summary>Bind location provider supplying base rads per hour for expedition nodes.</summary>
        public void BindLocationRadRateProvider(Func<string, float> locationRadRateProvider)
        {
            ExposureResolver.LocationRadRateProvider = locationRadRateProvider;
        }

        /// <summary>Bind fallout contamination provider (rad add-on by location id).</summary>
        public void BindFalloutContaminationProvider(Func<string, float> falloutContaminationProvider)
        {
            ExposureResolver.FalloutContaminationProvider = falloutContaminationProvider;
        }

        /// <summary>Bind expedition session so deployed survivors automatically resolve to expedition location.</summary>
        public void BindExpeditionSession(ExpeditionHostSession expeditionSession)
        {
            if (expeditionSession == null) return;
            ExposureResolver.SurvivorLocationQuery = id =>
            {
                if (expeditionSession.Engine != null &&
                    expeditionSession.Engine.Active.TryGetValue(id, out var exp))
                {
                    return (SurvivorExposureLocation.Expedition, exp.locationId);
                }
                return GetSurvivorLocation(id);
            };
        }

        /// <summary>
        /// Assemble the shared inventory's equipped protective gear into a list
        /// of Inventory.WornGear records. RadiationSystem subtracts this from the zone rate.
        /// </summary>
        private System.Collections.Generic.List<Ashfall.Core.Inventory.WornGear> CollectWornGear()
        {
            var result = new System.Collections.Generic.List<Ashfall.Core.Inventory.WornGear>();
            var inventory = Inventory?.Inventory;
            if (inventory == null) return result;
            inventory.FillWornGear(result);
            return result;
        }

        // ── Hourly tick ────────────────────────────────────────────────

        public string TickHour(float gameHours = 1f)
        {
            Needs.Tick(gameHours);
            Radiation.Tick(gameHours);
            LastEvent = $"Advanced {gameHours:F0} hour(s).";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Radiation ops ──────────────────────────────────────────────

        public string AdministerIodine(string survivorId)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.AdministerIodine(rad);
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.ActionPillBottle);
            LastEvent = $"{survivorId}: iodine administered — {rad.RadResistanceHoursRemaining:F0}h rad resistance.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AdministerAntiRad(string survivorId, float rads)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.AdministerAntiRad(rad, rads);
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.ActionInjection);
            LastEvent = $"{survivorId}: anti-rad cleared {rads:F0} mSv (dose now {rad.RadiationDose:F0}).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string HealSurvivor(string survivorId, float amount = 25f)
        {
            var survivor = Find(survivorId);
            if (survivor == null) return $"Unknown survivor: {survivorId}.";
            Needs.Modify(survivor, NeedKind.Health, amount);
            LastEvent = $"{survivorId}: treated with bandage (+{amount:F0} HP, health now {survivor.Health:F0}).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string ExposeToZone(string survivorId, float radsPerHour)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.Expose(rad, radsPerHour, 1f);
            if (radsPerHour > 0f)
                AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadGeigerBurst);
            LastEvent = $"{survivorId}: exposed to {radsPerHour} mSv/hr for 1h (dose {rad.RadiationDose:F0}/100).";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Status ─────────────────────────────────────────────────────

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("SURVIVORS — NEEDS & RADIATION\n");
            for (int i = 0; i < RosterState.Count; i++)
            {
                var s = RosterState[i];
                if (s == null) continue;
                var rad = RadStateFor(s.Id);
                sb.Append(s.Id).Append(": H ").Append(s.Hunger.ToString("F0"))
                  .Append(" T ").Append(s.Thirst.ToString("F0"))
                  .Append(" W ").Append(s.Warmth.ToString("F0"))
                  .Append(" M ").Append(s.Morale.ToString("F0"))
                  .Append(" HP ").Append(s.Health.ToString("F0"))
                  .Append(rad != null ? " | dose " + rad.RadiationDose.ToString("F0") + "/100" : "");
                if (rad != null && rad.HasRadResistance)
                    sb.Append(" ⚡rad-res");
                if (rad != null && rad.HasAcuteRadiationSickness)
                    sb.Append(" ☢ACUTE");
                if (rad != null && rad.HasChronicIllness)
                    sb.Append(" ☢CHRONIC");
                if (!s.IsAliveState)
                    sb.Append(" ✝DEAD");
                sb.Append('\n');
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public SurvivorsSaveState CaptureSave()
        {
            var save = new SurvivorsSaveState
            {
                roster = Roster.CaptureState()
            };
            var ordered = new System.Collections.Generic.List<SurvivorNeedsState>(RosterState);
            ordered.Sort((a, b) => string.CompareOrdinal(a?.Id, b?.Id));
            for (int i = 0; i < ordered.Count; i++)
            {
                var s = ordered[i];
                if (s == null) continue;
                if (!SurvivorId.TryParse(s.Id, out _, out string identityError))
                    throw new InvalidOperationException("Cannot persist survivor needs for invalid id '" + s.Id + "': " + identityError);

                var slice = new SurvivorSliceState
                {
                    id = s.Id,
                    hunger = s.Hunger,
                    thirst = s.Thirst,
                    fatigue = s.Fatigue,
                    warmth = s.Warmth,
                    morale = s.Morale,
                    health = s.Health,
                    hygiene = s.Hygiene,
                    wasHungerCritical = s.WasHungerCritical,
                    wasThirstCritical = s.WasThirstCritical,
                    wasWarmthCritical = s.WasWarmthCritical,
                    maxHealthCap = s.MaxHealthCap,
                    isDead = s.IsDead,
                    isAlive = s.IsAlive
                };
                var rad = RadStateFor(s.Id);
                if (rad != null)
                {
                    slice.radiationDose = rad.RadiationDose;
                    slice.lifetimeRadiationExposure = rad.LifetimeRadiationExposure;
                    slice.hasRadResistance = rad.HasRadResistance;
                    slice.radResistanceHoursRemaining = rad.RadResistanceHoursRemaining;
                    slice.iodineProtectionTimer = rad.IodineProtectionTimer;
                    slice.hasAcuteSickness = rad.HasAcuteRadiationSickness;
                    slice.hasChronicIllness = rad.HasChronicIllness;
                    slice.hasAcuteRadiationSyndrome = rad.HasAcuteRadiationSyndrome;
                    slice.radiationIsAlive = rad.IsAlive;
                }
                var loc = GetSurvivorLocation(s.Id);
                slice.locationKind = loc.Kind.ToString();
                slice.locationId = loc.LocationId;
                save.survivors.Add(slice);
            }
            return save;
        }

        public void RestoreSave(SurvivorsSaveState save)
        {
            if (save == null || save.survivors == null) return;
            var roster = ValidateSaveIdentity(save);

            // Remove every old component before dropping host references. Both
            // systems key simulation state by survivor identity; leaving either
            // component registered would let a prior campaign tick after load.
            for (int i = 0; i < RosterState.Count; i++)
            {
                if (RosterState[i] != null) Needs.Unregister(RosterState[i]);
            }
            for (int i = Radiation.RegisteredCount - 1; i >= 0; i--)
            {
                Radiation.Unregister(Radiation.Registered[i]);
            }

            Roster.RestoreState(roster);
            RosterState.Clear();
            _radStates.Clear();
            var ordered = new System.Collections.Generic.List<SurvivorSliceState>(save.survivors);
            ordered.Sort((a, b) => string.CompareOrdinal(a?.id, b?.id));
            foreach (var slice in ordered)
            {
                var s = new SurvivorNeedsState
                {
                    Id = slice.id,
                    Hunger = slice.hunger,
                    Thirst = slice.thirst,
                    Fatigue = slice.fatigue,
                    Warmth = slice.warmth,
                    Morale = slice.morale,
                    Health = slice.health,
                    Hygiene = slice.hygiene,
                    WasHungerCritical = slice.wasHungerCritical,
                    WasThirstCritical = slice.wasThirstCritical,
                    WasWarmthCritical = slice.wasWarmthCritical,
                    MaxHealthCap = slice.maxHealthCap,
                    IsAlive = slice.isAlive,
                    IsDead = slice.isDead || !slice.isAlive
                };
                RosterState.Add(s);
                Needs.Register(s);
                var rad = new RadSurvivorWrapper
                {
                    Id = slice.id,
                    RadiationDose = slice.radiationDose,
                    LifetimeRadiationExposure = slice.lifetimeRadiationExposure,
                    HasRadResistance = slice.hasRadResistance,
                    RadResistanceHoursRemaining = slice.radResistanceHoursRemaining,
                    IodineProtectionTimer = slice.iodineProtectionTimer,
                    HasAcuteRadiationSickness = slice.hasAcuteSickness,
                    HasChronicIllness = slice.hasChronicIllness,
                    HasAcuteRadiationSyndrome = slice.hasAcuteRadiationSyndrome,
                    IsAlive = slice.radiationIsAlive && slice.isAlive
                };
                _radStates[slice.id] = rad;
                Radiation.Register(rad);
                if (!string.IsNullOrEmpty(slice.locationKind) &&
                    Enum.TryParse<SurvivorExposureLocation>(slice.locationKind, out var parsedKind))
                {
                    SetSurvivorLocation(slice.id, parsedKind, slice.locationId ?? string.Empty);
                }
            }
            RaiseStateChanged();
        }

        private SurvivorRosterState ValidateSaveIdentity(SurvivorsSaveState save)
        {
            var sliceIds = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < save.survivors.Count; i++)
            {
                var slice = save.survivors[i];
                if (slice == null || string.IsNullOrEmpty(slice.id))
                    throw new InvalidOperationException("Survivors save contains a null or empty survivor slice at index " + i + ".");
                if (!SurvivorId.TryParse(slice.id, out _, out string identityError))
                    throw new InvalidOperationException("Survivors save contains invalid survivor id '" + slice.id + "': " + identityError);
                if (!sliceIds.Add(slice.id))
                    throw new InvalidOperationException("Survivors save contains duplicate survivor slice id '" + slice.id + "'.");
                if (slice.isDead && slice.isAlive)
                    throw new InvalidOperationException("Survivors save contains contradictory alive/dead flags for survivor '" + slice.id + "'.");
            }

            if (save.roster != null)
            {
                if (save.roster.entries == null)
                    throw new InvalidOperationException("Survivors save roster entries are null.");
                var rosterIds = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < save.roster.entries.Count; i++)
                {
                    var entry = save.roster.entries[i];
                    if (entry == null || string.IsNullOrEmpty(entry.survivorId))
                        throw new InvalidOperationException("Survivors save contains a null or empty roster identity at index " + i + ".");
                    if (!SurvivorId.TryParse(entry.survivorId, out _, out string identityError))
                        throw new InvalidOperationException("Survivors save roster contains invalid survivor id '" + entry.survivorId + "': " + identityError);
                    if (!rosterIds.Add(entry.survivorId))
                        throw new InvalidOperationException("Survivors save contains duplicate roster survivor id '" + entry.survivorId + "'.");
                    if (!sliceIds.Contains(entry.survivorId))
                        throw new InvalidOperationException("Survivors save roster identity '" + entry.survivorId + "' has no needs/radiation slice.");
                }
                foreach (string id in sliceIds)
                {
                    if (!rosterIds.Contains(id))
                        throw new InvalidOperationException("Survivors save slice identity '" + id + "' has no roster entry.");
                }
                return save.roster;
            }

            // Pre-roster-envelope saves remain loadable. Rebuild their roster
            // from the saved slice identities, retaining catalog metadata when
            // the current host already knows that survivor.
            var legacy = new SurvivorRosterState();
            foreach (var slice in save.survivors)
            {
                var existing = Roster.Find(slice.id);
                legacy.entries.Add(new SurvivorRosterEntry
                {
                    survivorId = slice.id,
                    definitionId = existing?.definitionId ?? slice.id,
                    joinedDay = existing?.joinedDay ?? 0,
                    isAlive = slice.isAlive,
                    deathReason = existing?.deathReason ?? string.Empty
                });
            }
            return legacy;
        }
    }

    /// <summary>Serialized survivors envelope (needs + radiation slices).</summary>
    public class SurvivorsSaveState
    {
        public System.Collections.Generic.List<SurvivorSliceState> survivors =
            new System.Collections.Generic.List<SurvivorSliceState>();
        public SurvivorRosterState? roster;
    }
}
