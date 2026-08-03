using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Procedural short radio beeps / hiss used as stand-in VO until real
    /// voice assets replace the WAV stubs under Assets/_Game/Audio/Radio/.
    /// Safe in EditMode tests (no disk I/O).
    /// </summary>
    public static class RadioVoStubFactory
    {
        public const int SampleRate = 22050;

        public static AudioClip CreateTone(
            string name,
            float frequencyHz,
            float durationSeconds = 0.3f,
            float volume = 0.22f)
        {
            int n = Mathf.Max(1, Mathf.RoundToInt(SampleRate * durationSeconds));
            var data = new float[n];
            float attack = 0.02f;
            float release = 0.06f;
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SampleRate;
                float env = 1f;
                if (t < attack) env = t / attack;
                float remain = durationSeconds - t;
                if (remain < release) env = Mathf.Max(0f, remain / release);
                float s = Mathf.Sin(2f * Mathf.PI * frequencyHz * t);
                s += 0.3f * Mathf.Sin(2f * Mathf.PI * frequencyHz * 2f * t);
                data[i] = s * volume * env;
            }
            var clip = AudioClip.Create(name, n, 1, SampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        public static AudioClip CreateHiss(string name, float durationSeconds = 0.5f, float volume = 0.12f)
        {
            int n = Mathf.Max(1, Mathf.RoundToInt(SampleRate * durationSeconds));
            var data = new float[n];
            // Deterministic pseudo-noise (no System.Random allocation needed)
            uint state = 0xA5F07u;
            float prev = 0f;
            float attack = 0.03f;
            float release = 0.08f;
            for (int i = 0; i < n; i++)
            {
                float t = i / (float)SampleRate;
                state = state * 1664525u + 1013904223u;
                float white = ((state >> 8) / 16777215f) * 2f - 1f;
                prev = 0.9f * prev + 0.1f * white;
                float env = 1f;
                if (t < attack) env = t / attack;
                float remain = durationSeconds - t;
                if (remain < release) env = Mathf.Max(0f, remain / release);
                float flutter = 0.75f + 0.25f * Mathf.Sin(2f * Mathf.PI * 17f * t);
                data[i] = prev * volume * env * flutter;
            }
            var clip = AudioClip.Create(name, n, 1, SampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        public static AudioClip CreateDualTone(
            string name,
            float f1,
            float f2,
            float durationEach = 0.18f,
            float volume = 0.2f)
        {
            int n1 = Mathf.Max(1, Mathf.RoundToInt(SampleRate * durationEach));
            int gap = Mathf.RoundToInt(SampleRate * 0.04f);
            int n2 = n1;
            int total = n1 + gap + n2;
            var data = new float[total];
            FillTone(data, 0, n1, f1, durationEach, volume);
            FillTone(data, n1 + gap, n2, f2, durationEach, volume * 0.9f);
            var clip = AudioClip.Create(name, total, 1, SampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static void FillTone(float[] data, int offset, int count, float freq, float dur, float vol)
        {
            for (int i = 0; i < count && offset + i < data.Length; i++)
            {
                float t = i / (float)SampleRate;
                float env = Mathf.Sin(Mathf.PI * Mathf.Clamp01(t / Mathf.Max(0.001f, dur)));
                data[offset + i] = Mathf.Sin(2f * Mathf.PI * freq * t) * vol * env;
            }
        }
    }
}
