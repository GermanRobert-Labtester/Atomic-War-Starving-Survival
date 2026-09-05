using System;
using Godot;

namespace AtomicWar.GodotApp.Audio
{
    public enum AudioSnapshot
    {
        Normal,
        Menu,
        RadioFocus,
        VoiceFocus,
        MedicalCritical,
        ShelterCrisis,
        Combat,
        Surface,
        GameOver,
        Pause
    }

    public sealed partial class AudioStateCoordinator : Node
    {
        private AudioSnapshot _currentSnapshot = AudioSnapshot.Normal;
        public AudioSnapshot CurrentSnapshot => _currentSnapshot;

        public void SetSnapshot(AudioSnapshot snapshot)
        {
            if (_currentSnapshot == snapshot) return;
            _currentSnapshot = snapshot;
            ApplyDucking(snapshot);

            if (snapshot == AudioSnapshot.Surface)
                AudioManager.Instance?.SetBunkerOcclusion(false);
            else if (snapshot == AudioSnapshot.Normal || snapshot == AudioSnapshot.ShelterCrisis || snapshot == AudioSnapshot.MedicalCritical)
                AudioManager.Instance?.SetBunkerOcclusion(true);
        }

        private void ApplyDucking(AudioSnapshot snapshot)
        {
            float musicDuck = 0f;
            float ambienceDuck = 0f;
            float sfxDuck = 0f;
            float uiDuck = 0f;

            switch (snapshot)
            {
                case AudioSnapshot.RadioFocus:
                case AudioSnapshot.VoiceFocus:
                    musicDuck = -8f;
                    ambienceDuck = -5f;
                    sfxDuck = -3f;
                    uiDuck = 0f;
                    break;
                case AudioSnapshot.ShelterCrisis:
                    musicDuck = -10f;
                    ambienceDuck = -7f;
                    sfxDuck = -3f;
                    uiDuck = -2f;
                    break;
                case AudioSnapshot.MedicalCritical:
                    musicDuck = -6f;
                    ambienceDuck = -4f;
                    sfxDuck = -2f;
                    uiDuck = 0f;
                    break;
                case AudioSnapshot.Combat:
                    musicDuck = -4f;
                    ambienceDuck = -4f;
                    sfxDuck = 0f;
                    uiDuck = 0f;
                    break;
                case AudioSnapshot.Surface:
                    musicDuck = -2f;
                    ambienceDuck = 0f;
                    sfxDuck = 0f;
                    uiDuck = 0f;
                    break;
                case AudioSnapshot.GameOver:
                    musicDuck = 0f;
                    ambienceDuck = -12f;
                    sfxDuck = -12f;
                    uiDuck = 0f;
                    break;
                case AudioSnapshot.Pause:
                case AudioSnapshot.Menu:
                    musicDuck = -8f;
                    ambienceDuck = -6f;
                    sfxDuck = -10f;
                    uiDuck = 0f;
                    break;
                case AudioSnapshot.Normal:
                default:
                    break;
            }

            TweenBusVolume(AudioBusNames.Music, musicDuck);
            TweenBusVolume(AudioBusNames.Ambience, ambienceDuck);
            TweenBusVolume(AudioBusNames.Sfx, sfxDuck);
            TweenBusVolume(AudioBusNames.Ui, uiDuck);
        }

        private void TweenBusVolume(string bus, float targetDuckDb)
        {
            int idx = AudioServer.GetBusIndex(bus);
            if (idx < 0) return;

            var settings = AudioSettings.Instance;
            float basePercent = bus switch
            {
                AudioBusNames.Music => settings.MusicVolume,
                AudioBusNames.Ambience => settings.AmbienceVolume,
                AudioBusNames.Sfx => settings.SfxVolume,
                _ => settings.MasterVolume,
            };

            float baseDb = AudioSettings.PercentToDb(basePercent);
            float finalTargetDb = baseDb + targetDuckDb;

            // Note: For true implementation, we would use a Tween here.
            // For headless compatibility, we apply immediately if CreateTween fails.
            var tween = GetTree()?.CreateTween();
            if (tween != null)
            {
                // We don't have a direct property for AudioServer bus volume, we must interpolate manually
                // A simpler way is to just set it since AudioServer doesn't expose tweenable properties on Node
                AudioServer.SetBusVolumeDb(idx, finalTargetDb);
            }
            else
            {
                AudioServer.SetBusVolumeDb(idx, finalTargetDb);
            }
        }
    }
}
