using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp.Audio
{
    public interface IExpansionAudioProvider
    {
        // Expansion cue producers pending: Plans 178-201 system events are not
        // yet mapped to AudioCueCatalog entries (tracked as expansion-audio
        // backlog). This interface intentionally declares no members so the
        // bridge compiles without faking a subscription path.
    }

    public sealed class ExpansionAudioBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private bool _disposed;

        public ExpansionAudioBridge(AudioManager audio)
            : this(RequireAudio(audio))
        {
        }

        internal ExpansionAudioBridge(Action<string> playCue)
        {
            _playCue = playCue;
        }

        private static Action<string> RequireAudio(AudioManager audio)
        {
            if (audio == null) throw new ArgumentNullException(nameof(audio));
            return audio.PlayCue;
        }

        public void SubscribeAll(IExpansionAudioProvider provider)
        {
            // No providers yet — see IExpansionAudioProvider.
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}
