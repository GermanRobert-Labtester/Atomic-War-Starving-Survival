// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 220 & 205 Host Session — Shelter Atmosphere, Ambiance & Acoustic Discipline.
    /// Composes ShelterAtmosphereSystem (interpretive ambiance read-model) and
    /// ShelterNoiseSystem (acoustic output, soundproofing, and quiet hours enforcement)
    /// into a unified host facade for Godot presentation and save orchestration.
    /// </summary>
    public sealed class ShelterAtmosphereHostSession : HostSessionBase
    {
        public ShelterAtmosphereSystem Atmosphere { get; }
        public ShelterNoiseSystem Noise { get; }

        public int CurrentDay { get; private set; } = 1;
        public string LastEvent { get; private set; } = string.Empty;
        public AtmosphereModifiers Modifiers => Atmosphere.GetActiveModifiers();

        public ShelterAtmosphereHostSession(ShelterAtmosphereSystem? atmosphere = null, ShelterNoiseSystem? noise = null)
        {
            Atmosphere = atmosphere ?? new ShelterAtmosphereSystem();
            Noise = noise ?? new ShelterNoiseSystem();

            Atmosphere.OnMoodShifted += OnAtmosphereMoodShifted;
            Atmosphere.OnProfileChanged += OnAtmosphereProfileChanged;
            Noise.OnNoiseSpike += OnNoiseSpikeOccurred;
            Noise.OnThreatDetectionRiskIncreased += OnThreatDetectionRiskIncreased;
        }

        public void UpdateEnvironmentalAtmosphere(
            int day,
            float lighting,
            float airPurity,
            float thermalComfort,
            float cleanliness,
            float socialWarmth,
            float decorationLevel)
        {
            CurrentDay = day;

            // Tick acoustic simulation for midday
            Noise.TickDay(day, 12);
            float acousticComfort = Math.Clamp(100f - Noise.OverallNoiseLevel, 0f, 100f);

            Atmosphere.UpdateEnvironmentalInputs(
                lighting,
                acousticComfort,
                airPurity,
                thermalComfort,
                cleanliness,
                socialWarmth,
                decorationLevel,
                day);

            RaiseStateChanged();
        }

        public void SetQuietHours(bool enabled, int startHour = 22, int endHour = 6)
        {
            Noise.SetQuietHours(enabled, startHour, endHour);
            LastEvent = enabled
                ? $"Quiet hours active ({startHour:D2}:00 - {endHour:D2}:00)."
                : "Quiet hours lifted.";
            RaiseStateChanged();
        }

        public void SoundproofRoom(string roomId, float wallAdd, float doorAdd)
        {
            if (string.IsNullOrWhiteSpace(roomId)) return;
            Noise.SoundproofRoom(roomId.Trim(), wallAdd, doorAdd);
            LastEvent = $"Installed acoustic insulation in {roomId.Trim()} (+{wallAdd:F0}% wall, +{doorAdd:F0}% door).";
            RaiseStateChanged();
        }

        public NoiseSource AddNoiseSource(NoiseSourceType type, string roomId, float output, NoiseFrequency freq = NoiseFrequency.Medium)
        {
            var src = Noise.AddNoiseSource(type, roomId, output, freq);
            LastEvent = $"Registered noise source {type} in {roomId} ({output:F0} dB).";
            RaiseStateChanged();
            return src;
        }

        public void SetSourceActive(string sourceId, bool active)
        {
            Noise.SetSourceActive(sourceId, active);
            LastEvent = $"Noise source {sourceId} {(active ? "engaged" : "silenced")}.";
            RaiseStateChanged();
        }

        public float GetRoomNoise(string roomId) => Noise.GetRoomNoise(roomId);

        public void AttenuateDetectionRisk(float amount)
        {
            Noise.AttenuateDetectionRisk(amount);
            RaiseStateChanged();
        }

        protected override void UnsubscribeSystemEvents()
        {
            Atmosphere.OnMoodShifted -= OnAtmosphereMoodShifted;
            Atmosphere.OnProfileChanged -= OnAtmosphereProfileChanged;
            Noise.OnNoiseSpike -= OnNoiseSpikeOccurred;
            Noise.OnThreatDetectionRiskIncreased -= OnThreatDetectionRiskIncreased;
        }

        private void OnAtmosphereMoodShifted(AtmosphereMoodCategory mood, float score)
        {
            LastEvent = $"Atmospheric mood transitioned to {mood} (Composite: {score:F1}/100).";
            RequestPresentationRefresh();
        }

        private void OnAtmosphereProfileChanged(AtmosphereProfileType profile)
        {
            LastEvent = $"Shelter ambiance characterized as {profile}.";
            RequestPresentationRefresh();
        }

        private void OnNoiseSpikeOccurred(NoiseEvent ev)
        {
            LastEvent = $"[Acoustic Alert] {ev.Description} (Noise: {ev.NoiseLevel:F1} dB, Risk: +{ev.DetectionRiskAdded:F1}%).";
            RequestPresentationRefresh();
        }

        private void OnThreatDetectionRiskIncreased(float risk)
        {
            RequestPresentationRefresh();
        }
    }
}
