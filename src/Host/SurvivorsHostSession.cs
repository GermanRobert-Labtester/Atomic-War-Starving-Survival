using System;
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
    {
        public NeedsSystem Needs { get; }
        public RadiationSystem Radiation { get; }
        public MaterialShieldingSystem Shelter { get; } = new MaterialShieldingSystem();
        public System.Collections.Generic.List<SurvivorNeedsState> Roster { get; } =
            new System.Collections.Generic.List<SurvivorNeedsState>();

        /// <summary>Demo geiger exposure context: one survivor outside, rest sheltered.</summary>
        private readonly System.Collections.Generic.Dictionary<string, RadSurvivorWrapper> _radStates;

        public string LastEvent { get; private set; } = string.Empty;
        public event Action StateChanged;

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

            Needs.OnNeedChanged += (s, kind, v) => StateChanged?.Invoke();
            Needs.OnDied += s =>
            {
                LastEvent = $"{s.Id} has died.";
                StateChanged?.Invoke();
            };
        }

        /// <summary>Seed the demo roster with canonical survivor ids from the master list.</summary>
        public void SeedDemoRoster()
        {
            if (Roster.Count > 0) return;
            AddSurvivor("survivor_dr_sarah_chen", "Dr. Sarah Chen (Trauma Surgeon)");
            AddSurvivor("survivor_gunner_mikhail", "Gunner Mikhail (Heavy Artillery Loader)");
            AddSurvivor("elena_vasquez", "Elena Vasquez (Aridoculture Engineer)");
        }

        public void AddSurvivor(string id, string displayName)
        {
            if (Find(id) != null) return;
            var state = new SurvivorNeedsState { Id = id };
            Roster.Add(state);
            Needs.Register(state);
            var rad = new RadSurvivorWrapper { Id = id };
            _radStates[id] = rad;
            Radiation.Register(rad);
        }

        public SurvivorNeedsState Find(string id)
        {
            for (int i = 0; i < Roster.Count; i++)
                if (Roster[i] != null && Roster[i].Id == id) return Roster[i];
            return null;
        }

        private SurvivorRadState RadStateFor(string id)
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
                ShelterShielding = shielding
            };
        }

        // ── Hourly tick ────────────────────────────────────────────────

        public string TickHour(float gameHours = 1f)
        {
            Needs.Tick(gameHours);
            Radiation.Tick(gameHours);
            LastEvent = $"Advanced {gameHours:F0} hour(s).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Radiation ops ──────────────────────────────────────────────

        public string AdministerIodine(string survivorId)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.AdministerIodine(rad);
            LastEvent = $"{survivorId}: iodine administered — {rad.RadResistanceHoursRemaining:F0}h rad resistance.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string AdministerAntiRad(string survivorId, float rads)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.AdministerAntiRad(rad, rads);
            LastEvent = $"{survivorId}: anti-rad cleared {rads:F0} mSv (dose now {rad.RadiationDose:F0}).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string ExposeToZone(string survivorId, float radsPerHour)
        {
            var rad = RadStateFor(survivorId);
            if (rad == null) return $"Unknown survivor: {survivorId}.";
            Radiation.Expose(rad, radsPerHour, 1f);
            LastEvent = $"{survivorId}: exposed to {radsPerHour} mSv/hr for 1h (dose {rad.RadiationDose:F0}/100).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Status ─────────────────────────────────────────────────────

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("SURVIVORS — NEEDS & RADIATION\n");
            for (int i = 0; i < Roster.Count; i++)
            {
                var s = Roster[i];
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
            for (int i = 0; i < Roster.Count; i++)
            {
                var s = Roster[i];
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
            Roster.Clear();
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
                Roster.Add(s);
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
            StateChanged?.Invoke();
        }
    }

    /// <summary>Serialized survivors envelope (needs + radiation slices).</summary>
    public class SurvivorsSaveState
    {
        public System.Collections.Generic.List<SurvivorSliceState> survivors =
            new System.Collections.Generic.List<SurvivorSliceState>();
    }
}
