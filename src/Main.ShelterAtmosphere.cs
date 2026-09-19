// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ShelterAtmosphereHostSession _shelterAtmosphere = null!;
        private ShelterAtmospherePanel _shelterAtmospherePanel = null!;
        private bool _shelterAtmosphereDirty;

        public ShelterAtmosphereHostSession ShelterAtmosphere => EnsureShelterAtmosphere();

        public ShelterAtmosphereHostSession EnsureShelterAtmosphere()
        {
            if (_shelterAtmosphere != null) return _shelterAtmosphere;

            SetupPowerGrid();
            SetupStartingLevel();

            var atmoState = ShelterAtmosphereSaveStore.TryLoad() ?? new AtmosphereState();
            var noiseState = ShelterNoiseSaveStore.TryLoad() ?? new ShelterNoiseState();

            var atmoSys = new ShelterAtmosphereSystem(atmoState);
            var noiseSys = new ShelterNoiseSystem(noiseState);

            // If new game / pristine state, register standard baseline shelter acoustic profile
            if (noiseState.Sources.Count == 0)
            {
                noiseSys.AddNoiseSource(NoiseSourceType.Generator, "power_room", 60f, NoiseFrequency.Low);
                noiseSys.AddNoiseSource(NoiseSourceType.Ventilation, "hvac_room", 40f, NoiseFrequency.Medium);
                noiseSys.AddNoiseSource(NoiseSourceType.Machinery, "workshop", 50f, NoiseFrequency.Medium);
                noiseSys.SoundproofRoom("sleeping_quarters", 35f, 25f);
                noiseSys.SoundproofRoom("rec_room", 30f, 20f);
            }

            _shelterAtmosphere = new ShelterAtmosphereHostSession(atmoSys, noiseSys);
            _shelterAtmosphere.StateChanged += OnShelterAtmosphereStateChanged;
            return _shelterAtmosphere;
        }

        private void OnShelterAtmosphereStateChanged()
        {
            _shelterAtmosphereDirty = true;
        }

        public void TickShelterAtmosphere(int day)
        {
            EnsureShelterAtmosphere();

            // 1. Lighting: derived from PowerGrid state
            float lighting = 70f;
            if (_powerGrid?.System != null)
            {
                if (_powerGrid.System.IsRoomServed("room_lighting"))
                {
                    lighting = 85f;
                }
                else if (_powerGrid.System.GenerationWatts > 0)
                {
                    lighting = 45f;
                }
                else
                {
                    lighting = 15f;
                }
            }

            // 2. Air Purity: derived from ventilation
            float airPurity = 75f;
            if (_ventilation != null)
            {
                airPurity = 80f;
            }

            // 3. Thermal Comfort: derived from shelter thermal system
            float thermalComfort = 70f;
            if (_shelterThermal?.System != null)
            {
                thermalComfort = _shelterThermal.System.BoilerFuelLevel > 0 ? 80f : 50f;
            }

            // 4. Cleanliness
            float cleanliness = 75f;

            // 5. Social Warmth: derived from living survivor average morale
            float socialWarmth = 65f;
            if (_survivors?.RosterState != null && _survivors.Needs != null)
            {
                float totalMorale = 0f;
                int count = 0;
                var roster = _survivors.RosterState;
                for (int i = 0; i < roster.Count; i++)
                {
                    var s = roster[i];
                    if (s != null && s.IsAliveState)
                    {
                        var need = _survivors.Needs.Get(s.Id);
                        if (need != null)
                        {
                            totalMorale += need.Morale;
                            count++;
                        }
                    }
                }
                if (count > 0)
                {
                    socialWarmth = Math.Clamp(totalMorale / count, 10f, 95f);
                }
            }

            // 6. Decoration Level: derived from shelter decor placements
            float decorationLevel = 30f;
            if (_shelterDecor?.System != null)
            {
                int count = _shelterDecor.System.State?.Placements?.Count ?? 0;
                decorationLevel = Math.Clamp(count * 10f + 25f, 10f, 95f);
            }

            _shelterAtmosphere.UpdateEnvironmentalAtmosphere(
                day,
                lighting,
                airPurity,
                thermalComfort,
                cleanliness,
                socialWarmth,
                decorationLevel);

            // Apply active atmospheric morale delta across living survivors
            var mods = _shelterAtmosphere.Modifiers;
            if (Math.Abs(mods.MoraleModifier) > 0.01f && _survivors?.RosterState != null && _survivors.Needs != null)
            {
                var roster = _survivors.RosterState;
                for (int i = 0; i < roster.Count; i++)
                {
                    var s = roster[i];
                    if (s != null && s.IsAliveState)
                    {
                        _survivors.Needs.ApplyAttributedDelta(
                            s.Id,
                            NeedKind.Morale,
                            mods.MoraleModifier,
                            "shelter.atmosphere");
                    }
                }
            }

            _shelterAtmosphereDirty = true;
        }

        private void SetupShelterAtmosphere()
        {
            EnsureShelterAtmosphere();
        }

        private void SaveShelterAtmosphere()
        {
            var session = EnsureShelterAtmosphere();
            if (session != null)
            {
                CaptureSection("shelter_atmosphere", ShelterAtmosphereSaveStore.TryCapturePersisted(session.Atmosphere.CaptureState()));
                CaptureSection("shelter_noise", ShelterNoiseSaveStore.TryCapturePersisted(session.Noise.CaptureState()));
                _shelterAtmosphereDirty = false;
            }
        }

        private void SetupShelterAtmospherePanel()
        {
            if (_shelterAtmospherePanel != null && _shelterAtmospherePanel.IsInsideTree())
                return;

            EnsureShelterAtmosphere();
            _shelterAtmospherePanel = new ShelterAtmospherePanel();
            _shelterAtmospherePanel.Bind(_shelterAtmosphere);
            _shelterAtmospherePanel.OnClose += () => _shelterAtmospherePanel.Visible = false;
            _shelterAtmospherePanel.Visible = false;
            AddChild(_shelterAtmospherePanel);
        }

        public void ShowShelterAtmospherePanel()
        {
            SetupShelterAtmospherePanel();
            _shelterAtmospherePanel.Visible = true;
            _shelterAtmospherePanel.RefreshView();
        }
    }
}
