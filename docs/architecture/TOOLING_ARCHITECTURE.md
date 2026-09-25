# ASHFALL Dual-Stack Tooling Architecture (Go & Python)

This document establishes the official operational boundary between **Go** (for high-frequency, low-RAM, concurrent services and CLIs) and **Python** (for AI research, data analytics, imaging, and audio DSP).

---

## 1. The Practical Dual-Stack Architecture

```
                    ┌────────────────────────────────────────────────────────┐
                    │                      AI AGENTS                         │
                    └───────────────────┬────────────────────────────────────┘
                                        │
             ┌──────────────────────────┴──────────────────────────┐
             ▼                                                     ▼
┌─────────────────────────────────────────┐   ┌──────────────────────────────────────────┐
│          GO PRODUCTION STACK            │   │          PYTHON RESEARCH LAB             │
│   (Low RAM, Fast Startup, Concurrency)  │   │  (Rich AI/Data Ecosystem, Rapid Trials)  │
├─────────────────────────────────────────┤   ├──────────────────────────────────────────┤
│ • game-agent-core (:8082)               │   │ • ai-experiments/                        │
│   Resident agent task orchestrator      │   │   Prompt engineering, LangGraph, CrewAI, │
│ • repo-indexer (:8081)                  │   │   Smolagents, RAG & embedding notebooks  │
│   In-memory fsnotify file watcher & API │   │ • test-helpers/                          │
│ • test-selector (`select-tests`)        │   │   Pytest plugins, custom fixtures, mocks │
│   Git diff → affected test shard map    │   │ • data-tools/                            │
│ • config-validator (`validate-json`)    │   │   Telemetry analytics, balance tuning,   │
│   Sub-second JSON/YAML schema gate      │   │   Pandas / Matplotlib / Seaborn plots    │
│ • llm-proxy (:8088)                     │   │ • Specialized Ecosystem Drivers:         │
│   Unified LLM routing, retries, keys    │   │   Pillow procedural graphics, ElevenLabs │
│ • build-orchestrator (`run-tasks`)      │   │   voice synthesis, audio DSP math        │
│   Bounded parallel task & test executor │   │                                          │
└─────────────────────────────────────────┘   └──────────────────────────────────────────┘
```

---

## 2. Decision Checklist for Tool Placement

When authoring or modifying any development or CI tool, evaluate these 5 criteria:

1. **Does it run continuously or very frequently (on every edit / git hook / CI gate)?**
   - **YES** $\rightarrow$ Strong candidate for **Go** (`bin/ashfall-dev`).
2. **Does it need heavy Python-only libraries (Pillow, NumPy, SciPy, ElevenLabs, Composio, LangChain)?**
   - **YES** $\rightarrow$ Keep in **Python**.
3. **Is it a user-facing CLI or gate that agents/CI must execute instantly with zero startup overhead?**
   - **YES** $\rightarrow$ **Go** provides a single static binary with < 10ms execution and no virtualenv dependencies.
4. **Is it a quick prototype, one-off narrative generator, or exploratory research script?**
   - **YES** $\rightarrow$ Write in **Python** first; rewrite in Go only if it becomes a pipeline bottleneck.
5. **Is memory a hard constraint (many concurrent background agents / CI parallel jobs)?**
   - **YES** $\rightarrow$ Use **Go** (or **Rust** for heavy terminal TUIs/simulators). Resident daemons must stay < 20 MB.

---

## 3. Tool Mapping & Benchmarks

| Capability | Go Implementation | Python Baseline | Speedup | Peak RSS |
|---|---|---|---|---|
| **Rulebook Synchronization** | `ashfall-dev sync-agents` | `sync-agent-rulebooks.py` | **12x** (<10ms vs 120ms) | **8.0 MB** (-42%) |
| **Data Catalog Validation** | `ashfall-dev validate-json` | `json-schema-policy-gate.py` | **3.4x** (100ms vs 340ms) | **24 MB** |
| **Repository File Indexing** | `ashfall-dev index` | Walk scripts | **4.8x** (381ms for 22k files) | **13.2 MB** |
| **Changed-Test Selection** | `ashfall-dev select-tests` | `agent-fast-verify.py` | **5.6x** (80ms for 3.3k files) | **11.4 MB** |
| **Save Store Contract Scanner** | `ashfall-dev scan-saves` | `generate-save-store-matrix.py` | **3.4x** (350ms for 255 stores)| **14.0 MB** |
| **Asset Manifest Builder** | `ashfall-dev build-manifest` | `production_manifest.py` | **3x** (540ms for 6.4k assets) | **20.1 MB** |
| **Agent Orchestrator Daemon** | `ashfall-dev agent-core` | None (Heavy Python daemons) | **Instant** | **0.48 MB heap** |
| **LLM Proxy & Router** | `ashfall-dev llm-proxy` | Ad-hoc Python proxies | **High concurrency** | **< 10 MB** |
