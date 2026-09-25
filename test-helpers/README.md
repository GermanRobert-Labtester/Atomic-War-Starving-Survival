# Test Helpers & Pytest Infrastructure (Python)

This directory hosts Python-specific test infrastructure:
- Pytest plugins and custom test runner hooks
- Shared test fixtures for Python test suites (e.g. `tests/test_audio_pipeline.py`)
- Custom domain assertions, mock factories, and coverage helpers
- Audio DSP test probes and waveform fixtures

Hot-path test selection and parallel subprocess orchestration are executed via Go (`bin/ashfall-dev select-tests` and `ashfall-dev run-tasks`).
