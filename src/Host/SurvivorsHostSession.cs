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
        public bool hasAcuteSickness;
        public bool hasChronicIllness;
        public bool isAlive = true;
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

        /// <summary>Demo geiger exposure context: one survivor outside, rest sheltered.</summary>
        private readonly System.Collections.Generic.Dictionary<string, RadSurvivorWrapper> _radStates;

        public string LastEvent { get; private set; } = string.Empty;
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

            // Default Holdfast room ceiling shielding
            Shelter.UpgradeCeiling("room_bunker_corridor", MaterialShieldingSystem.WallMaterial.Concrete);
            Shelter.UpgradeCeiling("room_filtration_stack", MaterialShieldingSystem.WallMaterial.Lead);
            Shelter.UpgradeCeiling("room_storage_bay", MaterialShieldingSystem.WallMaterial.Concrete);
            Shelter.UpgradeCeiling("room_bunks_living", MaterialShieldingSystem.WallMaterial.Wood);
            Shelter.UpgradeCeiling("room_radio_tuner", MaterialShieldingSystem.WallMaterial.Concrete);

            Needs.OnNeedChanged += (s, kind, v) => RaiseStateChanged();
            Needs.OnDied += s =>
            {
                LastEvent = $"{s.Id} has died.";
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
        public void LoadStartingRoster(string dataDir)
        {
            if (RosterState.Count > 0) return;
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                var starting = SurvivorStartingStateLoader.Load(dataDir, fileIO, serializer);
                if (starting != null && starting.Count > 0)
                {
                    for (int i = 0; i < starting.Count; i++)
                    {
                        var s = starting[i];
                        AddSurvivor(s.id, s.displayName, s.health, s.hunger, s.thirst, s.warmth, s.morale, s.lifetimeDose, s.acuteRad);
                    }
                    return;
                }
            }
            SeedDemoRoster();
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

        public void AddSurvivor(
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
            if (Find(id) != null) return;
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
            // Mikhail is outside in the zone; others are in the shelter, so the
            // shelter's weakest ceiling attenuates their ambient dose. Unity's
            // ExposureContext.ShelterShielding is a flat subtraction from the zone
            // rate (max(0, zone - gear - shielding)); we feed rads blocked.
            float zone = state.Id == "survivor_gunner_mikhail" ? 40f : 2f;
            float shielding = state.Id == "survivor_gunner_mikhail"
                ? 0f
                : 2f * Shelter.GetWeakestCeilingAttenuation();
            return new ExposureContext
            {
                ZoneRadLevel = zone,
                ShelterShielding = shielding,
                WornGear = CollectWornGear()
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
            LastEvent = $"{survivorId}: iodine administered — {rad.RadResistanceHoursRemaining:F0}h rad resistance.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AdministerAntiRad(string survivorId, float rads)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.AdministerAntiRad(rad, rads);
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
            var save = new SurvivorsSaveState();
            for (int i = 0; i < RosterState.Count; i++)
            {
                var s = RosterState[i];
                if (s == null) continue;
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
                    isAlive = s.IsAliveState
                };
                var rad = RadStateFor(s.Id);
                if (rad != null)
                {
                    slice.radiationDose = rad.RadiationDose;
                    slice.lifetimeRadiationExposure = rad.LifetimeRadiationExposure;
                    slice.hasRadResistance = rad.HasRadResistance;
                    slice.radResistanceHoursRemaining = rad.RadResistanceHoursRemaining;
                    slice.hasAcuteSickness = rad.HasAcuteRadiationSickness;
                    slice.hasChronicIllness = rad.HasChronicIllness;
                }
                save.survivors.Add(slice);
            }
            return save;
        }

        public void RestoreSave(SurvivorsSaveState save)
        {
            if (save == null || save.survivors == null) return;
            RosterState.Clear();
            _radStates.Clear();
            foreach (var slice in save.survivors)
            {
                if (slice == null || string.IsNullOrEmpty(slice.id)) continue;
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
                    IsAlive = slice.isAlive,
                    IsDead = !slice.isAlive
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
                    HasAcuteRadiationSickness = slice.hasAcuteSickness,
                    HasChronicIllness = slice.hasChronicIllness,
                    IsAlive = slice.isAlive
                };
                _radStates[slice.id] = rad;
                Radiation.Register(rad);
            }
            RaiseStateChanged();
        }
    }

    /// <summary>Serialized survivors envelope (needs + radiation slices).</summary>
    public class SurvivorsSaveState
    {
        public System.Collections.Generic.List<SurvivorSliceState> survivors =
            new System.Collections.Generic.List<SurvivorSliceState>();
    }
}
