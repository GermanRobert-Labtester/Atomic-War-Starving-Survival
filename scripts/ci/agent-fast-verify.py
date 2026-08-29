#!/usr/bin/env python3
"""
agent-fast-verify.py — Domain-Aware Fast Verification Runner for AI Agents & Developers

Enables fast, targeted pre-flight verification on specific subsystems (2–5 seconds)
with machine-readable JSON output and built-in Rule 7 timeout discipline.

Usage:
  python3 scripts/ci/agent-fast-verify.py --domain persistence
  python3 scripts/ci/agent-fast-verify.py --domain data
  python3 scripts/ci/agent-fast-verify.py --domain ui
  python3 scripts/ci/agent-fast-verify.py --domain expansion
  python3 scripts/ci/agent-fast-verify.py --domain audio
  python3 scripts/ci/agent-fast-verify.py --domain schema
  python3 scripts/ci/agent-fast-verify.py --domain smoke
  python3 scripts/ci/agent-fast-verify.py --domain core
  python3 scripts/ci/agent-fast-verify.py --domain docs
  python3 scripts/ci/agent-fast-verify.py --domain fast
  python3 scripts/ci/agent-fast-verify.py --domain all
  python3 scripts/ci/agent-fast-verify.py --list-domains
"""

import sys
import time
import json
import pathlib
import subprocess
import argparse

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent

# High-level domain targets mapped to concrete verification steps
DOMAIN_GATES = {
    "persistence": [
        {"name": "SaveStoreChecksumSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--save-store-checksum-selftest"]},
        {"name": "SaveLoadUiFailureSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--save-load-ui-failure-selftest"]},
        {"name": "SaveStoreCoverageTests", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "FullyQualifiedName~SaveStore"]},
        {"name": "CampaignEnvelopeFuzzTests", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "CampaignEnvelopeFuzzTests"]},
    ],
    "data": [
        {"name": "DataIntegritySelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--data-integrity-selftest"]},
        {"name": "ContentUtilizationSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--content-utilization-selftest"]},
        {"name": "CatalogRegistryDrift", "cmd": ["python3", "scripts/ci/generate-catalog-registry.py", "--check"]},
    ],
    "ui": [
        {"name": "SceneBindingSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--scene-binding-selftest"]},
        {"name": "SceneLint", "cmd": ["python3", "scripts/ci/scene-lint.py"]},
        {"name": "UiAccessibilitySelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--ui-accessibility-selftest"]},
        {"name": "UiPanelCatalogDrift", "cmd": ["python3", "scripts/ci/generate-ui-panel-catalog.py", "--check"]},
        {"name": "UiPanelContractTests", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "UiPanelContractTests"]},
    ],
    "expansion": [
        {"name": "ExpansionsCompletenessSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--expansions-selftest"]},
        {"name": "DutyRosterSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--duty-roster-selftest"]},
        {"name": "VerdictSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--verdict-selftest"]},
        {"name": "BlackFlotillaSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--black-flotilla-selftest"]},
        {"name": "SilentFoundrySelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--silent-foundry-selftest"]},
        {"name": "ExpansionsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-expansions-catalog.py", "--check"]},
    ],
    "audio": [
        {"name": "AudioCueIntegrity", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "AudioCueIntegrityTests"]},
        {"name": "AudioCatalogDrift", "cmd": ["python3", "scripts/ci/generate-audio-catalog.py", "--check"]},
        {"name": "AudioEventTests", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "AudioEventIntegrationTests"]},
    ],
    "schema": [
        {"name": "JsonSchemaPolicyGate", "cmd": ["python3", "scripts/ci/json-schema-policy-gate.py"]},
        {"name": "PersistentFilenameGate", "cmd": ["python3", "scripts/ci/persistent-filename-gate.py"]},
        {"name": "CatalogRegistryDrift", "cmd": ["python3", "scripts/ci/generate-catalog-registry.py", "--check"]},
        {"name": "AgentSkillsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-agent-skills-catalog.py", "--check"]},
    ],
    "smoke": [
        {"name": "SevenDaySmokeSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--7-day-smoke-selftest"]},
        {"name": "PlayableShellSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--playable-shell-selftest"]},
        {"name": "Day1OnboardingSelfTest", "cmd": ["godot", "--headless", "--path", ".", "--", "--day1-selftest"]},
    ],
    "core": [
        {"name": "DotnetBuild", "cmd": ["dotnet", "build", "Ashfall.csproj"]},
        {"name": "CoreTestsSuite", "cmd": ["dotnet", "test", "Ashfall.Core.Tests"]},
        {"name": "CoreSystemsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-core-systems-catalog.py", "--check"]},
    ],
    "docs": [
        {"name": "DocsIndexDrift", "cmd": ["python3", "scripts/ci/generate-docs-index.py", "--check"]},
        {"name": "DocLinkPortability", "cmd": ["python3", "scripts/ci/normalize-doc-links.py", "--check"]},
        {"name": "AgentRuleIntegrity", "cmd": ["dotnet", "test", "Ashfall.Core.Tests", "--filter", "AgentRuleIntegrityTests"]},
        {"name": "AgentRulebooksSync", "cmd": ["python3", "scripts/ci/sync-agent-rulebooks.py", "--check"]},
        {"name": "AgentSkillsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-agent-skills-catalog.py", "--check"]},
        {"name": "AudioCatalogDrift", "cmd": ["python3", "scripts/ci/generate-audio-catalog.py", "--check"]},
    ],
    "fast": [
        {"name": "SceneLint", "cmd": ["python3", "scripts/ci/scene-lint.py"]},
        {"name": "DocsIndexDrift", "cmd": ["python3", "scripts/ci/generate-docs-index.py", "--check"]},
        {"name": "DocLinkPortability", "cmd": ["python3", "scripts/ci/normalize-doc-links.py", "--check"]},
        {"name": "AgentRulebooksSync", "cmd": ["python3", "scripts/ci/sync-agent-rulebooks.py", "--check"]},
        {"name": "CoreSystemsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-core-systems-catalog.py", "--check"]},
        {"name": "CatalogRegistryDrift", "cmd": ["python3", "scripts/ci/generate-catalog-registry.py", "--check"]},
        {"name": "UiPanelCatalogDrift", "cmd": ["python3", "scripts/ci/generate-ui-panel-catalog.py", "--check"]},
        {"name": "ExpansionsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-expansions-catalog.py", "--check"]},
        {"name": "AudioCatalogDrift", "cmd": ["python3", "scripts/ci/generate-audio-catalog.py", "--check"]},
        {"name": "AgentSkillsCatalogDrift", "cmd": ["python3", "scripts/ci/generate-agent-skills-catalog.py", "--check"]},
    ]
}

def run_gate(gate_info, timeout_sec=180):
    name = gate_info["name"]
    cmd = gate_info["cmd"]
    start = time.time()
    try:
        res = subprocess.run(cmd, cwd=REPO_ROOT, capture_output=True, text=True, timeout=timeout_sec)
        elapsed = time.time() - start
        return {
            "name": name,
            "cmd": " ".join(cmd),
            "exit_code": res.returncode,
            "status": "PASS" if res.returncode == 0 else "FAIL",
            "duration_sec": round(elapsed, 2),
            "stdout": res.stdout.strip(),
            "stderr": res.stderr.strip()
        }
    except subprocess.TimeoutExpired:
        elapsed = time.time() - start
        return {
            "name": name,
            "cmd": " ".join(cmd),
            "exit_code": -1,
            "status": "TIMEOUT",
            "duration_sec": round(elapsed, 2),
            "stdout": "",
            "stderr": f"Gate timed out after {timeout_sec}s"
        }
    except Exception as ex:
        elapsed = time.time() - start
        return {
            "name": name,
            "cmd": " ".join(cmd),
            "exit_code": -1,
            "status": "ERROR",
            "duration_sec": round(elapsed, 2),
            "stdout": "",
            "stderr": str(ex)
        }

def list_domains():
    print("=============================================================================")
    print("  ASHFALL FAST-VERIFY DOMAIN REGISTRY")
    print("=============================================================================")
    print(f"{'Domain':<16} {'Gates':<8} {'Typical Est.':<14} {'Description'}")
    print("-----------------------------------------------------------------------------")
    desc_map = {
        "persistence": "Save stores, campaign envelope fuzzing & checksums",
        "data": "Data authority catalogs, utilization & registry drift",
        "ui": "Scene unique-name contracts, UI accessibility & scene lint",
        "expansion": "Expansions 01-11 completeness & domain self-tests",
        "audio": "Audio cue catalog integrity & audio event tests",
        "schema": "JSON schema policy, catalog & skill registry drift",
        "smoke": "7-Day deterministic replay & playable shell smoke",
        "core": "Host compilation & xUnit Core test suite",
        "docs": "Documentation index, links & agent rulebooks",
        "fast": "Lightweight drift & linter pre-flight suite",
        "all": "Full union of all domain verification gates",
    }
    for d, gates in DOMAIN_GATES.items():
        cnt = len(gates)
        est = "~2-5s" if cnt < 5 else "~10-25s"
        desc = desc_map.get(d, "Domain verification gates")
        print(f"{d:<16} {cnt:<8} {est:<14} {desc}")
    print(f"{'all':<16} {'~30':<8} {'~30-50s':<14} {'Full union of all domain verification gates'}")
    print("=============================================================================\n")

def main():
    parser = argparse.ArgumentParser(description="Domain-Aware Fast Verification Runner")
    parser.add_argument("--domain", "-d", choices=list(DOMAIN_GATES.keys()) + ["all"], default="fast",
                        help="Target domain to verify (default: fast)")
    parser.add_argument("--json", action="store_true", help="Output results in JSON format")
    parser.add_argument("--fail-artifact", help="Write failure details to a markdown artifact on failure")
    parser.add_argument("--list-domains", action="store_true", help="List all available verification domains")
    parser.add_argument("--timeout", type=int, default=180, help="Per-gate timeout in seconds (Rule 7: 180s default)")

    args = parser.parse_args()

    if args.list_domains:
        list_domains()
        sys.exit(0)

    if args.domain == "all":
        # Deduplicated ordered list of all gates across all domains
        seen = set()
        gates_to_run = []
        for d in ["persistence", "data", "ui", "expansion", "audio", "schema", "smoke", "core", "docs"]:
            for g in DOMAIN_GATES[d]:
                if g["name"] not in seen:
                    seen.add(g["name"])
                    gates_to_run.append(g)
    else:
        gates_to_run = DOMAIN_GATES[args.domain]

    if not args.json:
        print(f"=== ASHFALL Agent Fast-Verify [{args.domain.upper()}] ===")
        print(f"Running {len(gates_to_run)} verification gates (timeout: {args.timeout}s)...\n")

    results = []
    failed_count = 0

    for g in gates_to_run:
        if not args.json:
            print(f"▶ [{g['name']}] ... ", end="", flush=True)

        res = run_gate(g, timeout_sec=args.timeout)
        results.append(res)

        if res["status"] != "PASS":
            failed_count += 1
            if not args.json:
                print(f"FAILED ({res['duration_sec']}s)")
                err_msg = res["stderr"] or res["stdout"]
                if err_msg:
                    snippet = err_msg.splitlines()[-3:] if len(err_msg.splitlines()) > 3 else err_msg.splitlines()
                    print(f"  Error: {' '.join(snippet)}")
        else:
            if not args.json:
                print(f"PASS ({res['duration_sec']}s)")

    is_success = failed_count == 0

    if args.json:
        output_payload = {
            "domain": args.domain,
            "status": "PASS" if is_success else "FAIL",
            "total": len(gates_to_run),
            "passed": len(gates_to_run) - failed_count,
            "failed": failed_count,
            "results": results
        }
        print(json.dumps(output_payload, indent=2))
    else:
        print(f"\nSummary: {len(gates_to_run) - failed_count}/{len(gates_to_run)} gates passed. Overall: {'PASS' if is_success else 'FAIL'}\n")

    if not is_success and args.fail_artifact:
        fail_path = pathlib.Path(args.fail_artifact)
        fail_path.parent.mkdir(parents=True, exist_ok=True)
        lines = [f"# Fast-Verify [{args.domain.upper()}] Failure Report\n"]
        for r in results:
            if r["status"] != "PASS":
                lines.append(f"## ❌ {r['name']}")
                lines.append(f"- **Command:** `{r['cmd']}`")
                lines.append(f"- **Duration:** {r['duration_sec']}s")
                lines.append(f"- **Exit Code:** {r['exit_code']}\n")
                lines.append("```text")
                lines.append(r["stderr"] or r["stdout"] or "No error output")
                lines.append("```\n")
        fail_path.write_text("\n".join(lines), encoding="utf-8")
        print(f"Wrote failure artifact to {fail_path}")

    sys.exit(0 if is_success else 1)

if __name__ == "__main__":
    main()
