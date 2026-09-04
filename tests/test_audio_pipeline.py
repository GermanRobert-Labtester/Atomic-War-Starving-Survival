#!/usr/bin/env python3
"""
tests/test_audio_pipeline.py — Unit & Integration Tests for tools/audio_pipeline.py.

Verifies:
1. Preset definitions (ceilings <= -1.5 dBFS).
2. Seeded synthesis determinism.
3. Byte-reproducibility across two independent runs.
4. Prohibition of 0 dBFS peak normalization.
5. Exporter writes valid WAV and NEVER creates an .import file.
6. FFmpeg measurement integration.
7. Ledger serialization.
"""

import os
import pathlib
import sys
import tempfile
import unittest

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent
sys.path.insert(0, str(REPO_ROOT / "tools"))

from audio_pipeline import (
    DeliveryPreset,
    PRESET_UI,
    PRESET_SFX,
    PRESET_LOOP,
    PRESET_AMBIENCE,
    PRESET_MAP,
    SeededSynthesizer,
    AudioMasterer,
    AudioExporter,
    AudioMeasurer,
    ReproducibilityEngine,
    DeliveryLedger
)

class TestAudioPipeline(unittest.TestCase):

    def test_presets_ceiling(self):
        """All delivery presets must enforce a maximum peak ceiling <= -1.5 dBFS."""
        for name, preset in PRESET_MAP.items():
            self.assertLessEqual(preset.max_peak_dbfs, -1.5, f"Preset {name} exceeds -1.5 dBFS")
            self.assertLessEqual(preset.linear_ceiling, 0.85, f"Linear ceiling for {name} too high")

    def test_reproducibility(self):
        """Two identical seeded synthesis runs must produce identical byte hashes."""
        def synth_fn(seed: int):
            synth = SeededSynthesizer(seed=seed)
            tone = synth.sine(440.0, 0.5)
            noise = synth.filtered_noise(0.5, "lp", 0.2)
            return [t + n * 0.3 for t, n in zip(tone, noise)]

        is_reproducible, h1, h2 = ReproducibilityEngine.verify(synth_fn, seed=12345, preset=PRESET_SFX)
        self.assertTrue(is_reproducible, f"Hashes differ: {h1} != {h2}")
        self.assertEqual(h1, h2)

    def test_masterer_prohibits_overs(self):
        """Masterer must clamp extreme signals to the preset linear ceiling."""
        samples = [2.5, -3.0, 1.8, -4.2]
        mastered = AudioMasterer.master(samples, PRESET_SFX)
        ceiling = PRESET_SFX.linear_ceiling
        for s in mastered:
            self.assertLessEqual(s, ceiling + 1e-6)
            self.assertGreaterEqual(s, -ceiling - 1e-6)

    def test_exporter_never_creates_import_sidecar(self):
        """AudioExporter must export a clean WAV and NEVER create a .import sidecar."""
        with tempfile.TemporaryDirectory() as tmpdir:
            wav_path = pathlib.Path(tmpdir) / "test_sfx.wav"
            samples = [0.1 * i for i in range(100)]
            sha256 = AudioExporter.export_wav(wav_path, samples, PRESET_UI)

            self.assertTrue(wav_path.exists(), "WAV file was not created")
            self.assertTrue(len(sha256) == 64, "Invalid SHA256 hash returned")

            import_sidecar = pathlib.Path(tmpdir) / "test_sfx.wav.import"
            self.assertFalse(import_sidecar.exists(), "AudioExporter must NEVER write .import files!")

    def test_measurer_and_ledger(self):
        """AudioMeasurer must measure audio metrics and DeliveryLedger must record them."""
        with tempfile.TemporaryDirectory() as tmpdir:
            wav_path = pathlib.Path(tmpdir) / "tone.wav"
            synth = SeededSynthesizer(seed=999)
            samples = synth.sine(1000.0, 1.0)
            sha256 = AudioExporter.export_wav(wav_path, samples, PRESET_UI)

            metrics = AudioMeasurer.measure(wav_path)
            self.assertGreater(metrics["duration_seconds"], 0.9)
            self.assertLessEqual(metrics["true_peak_dbfs"], -1.4)  # Peak ceiling at -1.5 dBFS

            ledger = DeliveryLedger()
            ledger.record("test_tone", wav_path, PRESET_UI, sha256, metrics)
            self.assertEqual(len(ledger.entries), 1)

            json_out = pathlib.Path(tmpdir) / "ledger.json"
            md_out = pathlib.Path(tmpdir) / "ledger.md"
            ledger.save_json(json_out)
            ledger.save_markdown(md_out)
            self.assertTrue(json_out.exists())
            self.assertTrue(md_out.exists())

if __name__ == '__main__':
    unittest.main()
