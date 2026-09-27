package releasepolicy

import (
	"strings"
	"testing"
)

func boolPtr(b bool) *bool { return &b }

// goodManifest matches the shape landed alongside this check: fast gates
// that already run unconditionally, plus three full-tier gates marked
// release_required + critical. export_parity's command carries the
// "--export-parity-selftest" flag, matching the real manifest — it is
// satisfied by the export job, not by release-gate.sh.
func goodManifest() *Manifest {
	return &Manifest{Gates: []ManifestGate{
		{GateID: "whitespace_hygiene", Classification: "fast", Critical: boolPtr(true), Command: "bash scripts/ci/no-whitespace-churn.sh"},
		{GateID: "build_core_tests", Classification: "fast", Critical: boolPtr(true), Command: "dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo"},
		{GateID: "build_godot_host", Classification: "fast", Critical: boolPtr(true), Command: "dotnet build Ashfall.csproj --nologo"},
		{GateID: "test_core_suite", Classification: "full", Critical: boolPtr(true), ReleaseRequired: true, Command: "dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo"},
		{GateID: "save_support_window", Classification: "full", Critical: boolPtr(true), ReleaseRequired: true, Command: "dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveSupportWindowTests --nologo"},
		{GateID: "export_parity", Classification: "full", Critical: boolPtr(true), ReleaseRequired: true, Command: "bash scripts/ci/run-godot-bounded.sh --path . -- --export-parity-selftest"},
	}}
}

// goodScript mirrors the real scripts/ci/release-gate.sh: fast tier, then
// only the two full-tier gates that release-gate.sh names explicitly.
// export_parity is deliberately absent — it is satisfied by the export job.
const goodScript = `#!/usr/bin/env bash
set -euo pipefail
bash scripts/ci/verify-fast.sh
python3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window --report-json build/reports/release-full-results.json
`

// goodExportScript mirrors the real scripts/ci/export-build.sh: it runs the
// packaged parity selftest inline, satisfying export_parity.
const goodExportScript = `#!/usr/bin/env bash
set -euo pipefail
dotnet build Ashfall.csproj
scripts/ci/godot-export-linux.sh
"${GODOT_RUNNER[@]}" --path . -- --export-parity-selftest --parity-target "$DIR/builds/linux"
`

func goodWorkflow() *Workflow {
	return &Workflow{Jobs: map[string]WorkflowJob{
		"release-gate": {
			Steps: []WorkflowStep{
				{Name: "checkout", Uses: "actions/checkout@v4"},
				{Name: "Run canonical release gate", Run: "bash scripts/ci/release-gate.sh"},
			},
		},
		"export-release": {
			Needs: "release-gate",
			Steps: []WorkflowStep{
				{Name: "checkout", Uses: "actions/checkout@v4"},
				{Name: "linux export", Run: "bash scripts/ci/export-build.sh"},
				{Name: "windows export", Run: "mkdir -p builds/windows\nbash scripts/ci/run-godot-bounded.sh --path . --export-release \"Windows Desktop\" builds/windows/ashfall.exe\n"},
				{
					Name: "upload linux", Uses: "actions/upload-artifact@v4",
					With: map[string]any{"name": "release-Linux", "path": "builds/linux", "if-no-files-found": "error"},
				},
				{
					Name: "upload windows", Uses: "actions/upload-artifact@v4",
					With: map[string]any{"name": "release-Windows", "path": "builds/windows", "if-no-files-found": "error"},
				},
			},
		},
	}}
}

func evalGood(wf *Workflow, script string) Report {
	return Evaluate(goodManifest(), wf, script, goodExportScript, "release-gate", "export-release")
}

func hasKind(vs []Violation, kind string) bool {
	for _, v := range vs {
		if v.Kind == kind {
			return true
		}
	}
	return false
}

func findByKindAndGate(vs []Violation, kind, gateID string) *Violation {
	for i := range vs {
		if vs[i].Kind == kind && vs[i].GateID == gateID {
			return &vs[i]
		}
	}
	return nil
}

func TestEvaluate_Positive_NoViolations(t *testing.T) {
	rep := evalGood(goodWorkflow(), goodScript)
	if !rep.Pass {
		t.Fatalf("expected Pass=true, got violations: %+v", rep.Violations)
	}
	if rep.Metrics.ViolationCount != 0 {
		t.Fatalf("expected 0 violations, got %d", rep.Metrics.ViolationCount)
	}
	if rep.Metrics.RequiredGateCount != 3 {
		t.Fatalf("expected 3 required gates, got %d", rep.Metrics.RequiredGateCount)
	}
}

func TestEvaluate_ExportJobSatisfiesSelftestGate(t *testing.T) {
	// export_parity is release_required, absent from release-gate.sh's
	// --gate list, and only reachable through the export job's packaged
	// parity selftest. It must not be reported missing.
	rep := evalGood(goodWorkflow(), goodScript)
	if v := findByKindAndGate(rep.Violations, KindMissingRequiredGate, "export_parity"); v != nil {
		t.Errorf("export_parity should be satisfied by the export job, got violation: %+v", v)
	}
	found := false
	for _, e := range rep.Evidence {
		if e.Description == "export_job_satisfied_gate_ids" {
			found = strings.Contains(e.Detail, "export_parity")
		}
	}
	if !found {
		t.Errorf("expected export_parity in export_job_satisfied_gate_ids evidence, got %+v", rep.Evidence)
	}
}

func TestEvaluate_MissingRequiredGate_NotSatisfiedByExportJobEither(t *testing.T) {
	// Same as the export-satisfaction case, but neither the export job nor
	// export-build.sh mentions the selftest flag at all — export_parity must
	// be reported missing, proving the export-job path does not blanket-pass
	// every release_required gate.
	rep := Evaluate(goodManifest(), goodWorkflow(), goodScript, "" /* no export script content */, "release-gate", "export-release")
	if v := findByKindAndGate(rep.Violations, KindMissingRequiredGate, "export_parity"); v == nil {
		t.Errorf("expected export_parity missing when no export surface mentions its selftest flag, got %+v", rep.Violations)
	}
}

func TestEvaluate_MissingRequiredGate(t *testing.T) {
	script := `#!/usr/bin/env bash
bash scripts/ci/verify-fast.sh
python3 scripts/ci/run-gates.py --gate test_core_suite --report-json build/reports/release-full-results.json
`
	rep := Evaluate(goodManifest(), goodWorkflow(), script, "" /* export_parity not satisfied here either */, "release-gate", "export-release")
	if rep.Pass {
		t.Fatalf("expected failure, got pass")
	}
	for _, id := range []string{"save_support_window", "export_parity"} {
		if v := findByKindAndGate(rep.Violations, KindMissingRequiredGate, id); v == nil {
			t.Errorf("expected missing_required_gate violation for %q, got %+v", id, rep.Violations)
		}
	}
	// test_core_suite was named explicitly, so it must NOT be reported missing.
	if v := findByKindAndGate(rep.Violations, KindMissingRequiredGate, "test_core_suite"); v != nil {
		t.Errorf("test_core_suite should not be missing, it was explicitly invoked: %+v", v)
	}
}

func TestEvaluate_MissingRequiredGate_FastTierNotInvoked(t *testing.T) {
	script := `#!/usr/bin/env bash
python3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window --report-json build/reports/release-full-results.json
`
	rep := evalGood(goodWorkflow(), script)
	if v := findByKindAndGate(rep.Violations, KindMissingRequiredGate, "fast_tier"); v == nil {
		t.Errorf("expected a missing_required_gate violation for fast_tier when verify-fast.sh is not invoked, got %+v", rep.Violations)
	}
}

func TestEvaluate_NonblockingStep(t *testing.T) {
	cases := []struct {
		name  string
		steps []WorkflowStep
	}{
		{
			name: "continue-on-error",
			steps: []WorkflowStep{
				{Name: "Run canonical release gate", Run: "bash scripts/ci/release-gate.sh", ContinueOnError: true},
			},
		},
		{
			name: "conditional if",
			steps: []WorkflowStep{
				{Name: "Run canonical release gate", Run: "bash scripts/ci/release-gate.sh", If: "success()"},
			},
		},
		{
			name: "skip-full flag",
			steps: []WorkflowStep{
				{Name: "Run canonical release gate", Run: "bash scripts/ci/release-gate.sh --skip-full"},
			},
		},
		{
			name:  "not invoked at all",
			steps: []WorkflowStep{{Name: "unrelated", Run: "echo hi"}},
		},
	}

	for _, tc := range cases {
		t.Run(tc.name, func(t *testing.T) {
			wf := &Workflow{Jobs: map[string]WorkflowJob{
				"release-gate":   {Steps: tc.steps},
				"export-release": goodWorkflow().Jobs["export-release"],
			}}
			rep := evalGood(wf, goodScript)
			if !hasKind(rep.Violations, KindNonblockingStep) {
				t.Errorf("case %q: expected a nonblocking_step violation, got %+v", tc.name, rep.Violations)
			}
		})
	}
}

func TestEvaluate_RequiredJobsAndExportStepsCannotBeSkipped(t *testing.T) {
	cases := []struct {
		name   string
		change func(*Workflow)
		kind   string
	}{
		{"gate job conditional", func(w *Workflow) {
			j := w.Jobs["release-gate"]
			j.If = "false"
			w.Jobs["release-gate"] = j
		}, KindNonblockingStep},
		{"export job conditional", func(w *Workflow) {
			j := w.Jobs["export-release"]
			j.If = "false"
			w.Jobs["export-release"] = j
		}, KindExportOrphan},
		{"linux export conditional", func(w *Workflow) {
			j := w.Jobs["export-release"]
			j.Steps[1].If = "false"
			w.Jobs["export-release"] = j
		}, KindExportOrphan},
		{"windows export continue-on-error", func(w *Workflow) {
			j := w.Jobs["export-release"]
			j.Steps[2].ContinueOnError = true
			w.Jobs["export-release"] = j
		}, KindExportOrphan},
		{"artifact upload conditional", func(w *Workflow) {
			j := w.Jobs["export-release"]
			j.Steps[3].If = "false"
			w.Jobs["export-release"] = j
		}, KindExportOrphan},
	}
	for _, tc := range cases {
		t.Run(tc.name, func(t *testing.T) {
			wf := goodWorkflow()
			tc.change(wf)
			if rep := evalGood(wf, goodScript); !hasKind(rep.Violations, tc.kind) {
				t.Fatalf("expected %s violation, got %+v", tc.kind, rep.Violations)
			}
		})
	}
}

func TestEvaluate_CommentedCommandsDoNotProveReleaseParity(t *testing.T) {
	script := `#!/usr/bin/env bash
# bash scripts/ci/verify-fast.sh
# python3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window
echo "no gates run"
`
	rep := Evaluate(goodManifest(), goodWorkflow(), script, "# --export-parity-selftest", "release-gate", "export-release")
	for _, id := range []string{"fast_tier", "test_core_suite", "save_support_window", "export_parity"} {
		if v := findByKindAndGate(rep.Violations, KindMissingRequiredGate, id); v == nil {
			t.Errorf("commented command must not satisfy %s: %+v", id, rep.Violations)
		}
	}
}

func TestEvaluate_GateStepWithCommentsIsRecognized(t *testing.T) {
	// A blocking canonical invocation preceded by comments/blank lines in a
	// `run: |` block scalar must still be recognized — this is the check the
	// naive "grep the whole run: string for equality" approach would fail.
	steps := []WorkflowStep{
		{
			Name: "Run canonical release gate",
			Run:  "# one canonical, blocking command\n\nbash scripts/ci/release-gate.sh\n",
		},
	}
	wf := &Workflow{Jobs: map[string]WorkflowJob{
		"release-gate":   {Steps: steps},
		"export-release": goodWorkflow().Jobs["export-release"],
	}}
	rep := evalGood(wf, goodScript)
	if hasKind(rep.Violations, KindNonblockingStep) {
		t.Errorf("expected no nonblocking_step violation for a commented-but-canonical invocation, got %+v", rep.Violations)
	}
}

func TestEvaluate_WeakenedCriticality(t *testing.T) {
	m := goodManifest()
	for i := range m.Gates {
		if m.Gates[i].GateID == "export_parity" {
			m.Gates[i].Critical = nil // main agent forgot to set critical:true
		}
	}
	rep := Evaluate(m, goodWorkflow(), goodScript, goodExportScript, "release-gate", "export-release")
	if v := findByKindAndGate(rep.Violations, KindWeakenedCriticality, "export_parity"); v == nil {
		t.Errorf("expected weakened_criticality violation for export_parity, got %+v", rep.Violations)
	}
}

func TestEvaluate_OrphanedReleaseID(t *testing.T) {
	script := `#!/usr/bin/env bash
bash scripts/ci/verify-fast.sh
python3 scripts/ci/run-gates.py --gate test_core_suite,save_support_window,ghost_gate --report-json build/reports/release-full-results.json
`
	rep := evalGood(goodWorkflow(), script)
	if v := findByKindAndGate(rep.Violations, KindOrphanedReleaseID, "ghost_gate"); v == nil {
		t.Errorf("expected orphaned_release_id violation for ghost_gate, got %+v", rep.Violations)
	}
}

func TestEvaluate_ExportOrphan(t *testing.T) {
	cases := []struct {
		name string
		job  WorkflowJob
	}{
		{
			name: "missing needs dependency",
			job: WorkflowJob{
				Steps: goodWorkflow().Jobs["export-release"].Steps,
			},
		},
		{
			name: "missing windows export",
			job: WorkflowJob{
				Needs: "release-gate",
				Steps: []WorkflowStep{
					{Name: "linux export", Run: "bash scripts/ci/export-build.sh"},
					{Name: "upload linux", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/linux", "if-no-files-found": "error"}},
					{Name: "upload windows", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/windows", "if-no-files-found": "error"}},
				},
			},
		},
		{
			name: "upload without if-no-files-found error",
			job: WorkflowJob{
				Needs: "release-gate",
				Steps: []WorkflowStep{
					{Name: "linux export", Run: "bash scripts/ci/export-build.sh"},
					{Name: "windows export", Run: "bash scripts/ci/run-godot-bounded.sh --path . --export-release \"Windows Desktop\" builds/windows/ashfall.exe"},
					{Name: "upload linux", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/linux"}},
					{Name: "upload windows", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/windows"}},
				},
			},
		},
		{
			name: "windows export runs before linux export",
			job: WorkflowJob{
				Needs: "release-gate",
				Steps: []WorkflowStep{
					{Name: "windows export", Run: "bash scripts/ci/run-godot-bounded.sh --path . --export-release \"Windows Desktop\" builds/windows/ashfall.exe"},
					{Name: "linux export", Run: "bash scripts/ci/export-build.sh"},
					{Name: "upload linux", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/linux", "if-no-files-found": "error"}},
					{Name: "upload windows", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/windows", "if-no-files-found": "error"}},
				},
			},
		},
		{
			name: "upload happens before its export step",
			job: WorkflowJob{
				Needs: "release-gate",
				Steps: []WorkflowStep{
					{Name: "upload linux", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/linux", "if-no-files-found": "error"}},
					{Name: "linux export", Run: "bash scripts/ci/export-build.sh"},
					{Name: "windows export", Run: "bash scripts/ci/run-godot-bounded.sh --path . --export-release \"Windows Desktop\" builds/windows/ashfall.exe"},
					{Name: "upload windows", Uses: "actions/upload-artifact@v4", With: map[string]any{"path": "builds/windows", "if-no-files-found": "error"}},
				},
			},
		},
	}

	for _, tc := range cases {
		t.Run(tc.name, func(t *testing.T) {
			wf := &Workflow{Jobs: map[string]WorkflowJob{
				"release-gate":   goodWorkflow().Jobs["release-gate"],
				"export-release": tc.job,
			}}
			rep := evalGood(wf, goodScript)
			if !hasKind(rep.Violations, KindExportOrphan) {
				t.Errorf("case %q: expected an export_orphan violation, got %+v", tc.name, rep.Violations)
			}
		})
	}
}

// TestEvaluate_ExportOrphan_FalsePositive proves the export-job check does
// not fire on legitimate stylistic variation: unrelated steps interspersed,
// multi-line diagnostics after the real command, and a `with:` block whose
// keys are declared in a different order than the shipped release.yml.
func TestEvaluate_ExportOrphan_FalsePositive(t *testing.T) {
	job := WorkflowJob{
		Needs: []any{"release-gate"},
		Steps: []WorkflowStep{
			{Name: "checkout", Uses: "actions/checkout@v4", With: map[string]any{"lfs": true}},
			{Name: "Setup .NET SDKs", Uses: "actions/setup-dotnet@v4"},
			{Name: "Setup Godot 4.7.1 mono", Uses: "chickensoft-games/setup-godot@v2"},
			{
				Name: "Run export-build.sh (Linux export + packaged parity)",
				Run:  "bash scripts/ci/export-build.sh\n",
			},
			{
				Name: "Export Windows Desktop",
				Run: "mkdir -p builds/windows\n" +
					"bash scripts/ci/run-godot-bounded.sh --path . --export-release \"Windows Desktop\" builds/windows/ashfall.exe\n" +
					"ls -lh builds/windows/\n" +
					"test -f builds/windows/ashfall.exe\n",
			},
			{
				Name: "Upload Linux build",
				Uses: "actions/upload-artifact@v4",
				With: map[string]any{
					"name":              "release-Linux-v1.2.3",
					"if-no-files-found": "error",
					"path":              "builds/linux",
				},
			},
			{
				Name: "Upload Windows build",
				Uses: "actions/upload-artifact@v4",
				With: map[string]any{
					"if-no-files-found": "error",
					"path":              "builds/windows",
					"name":              "release-Windows-v1.2.3",
				},
			},
		},
	}
	wf := &Workflow{Jobs: map[string]WorkflowJob{
		"release-gate":   goodWorkflow().Jobs["release-gate"],
		"export-release": job,
	}}
	rep := evalGood(wf, goodScript)
	if hasKind(rep.Violations, KindExportOrphan) {
		t.Errorf("expected no export_orphan false positive on stylistic variation, got %+v", rep.Violations)
	}
}

func TestParseExplicitGateIDs(t *testing.T) {
	script := "python3 scripts/ci/run-gates.py --gate  a,b, c  --report-json out.json\nsome other line --gate d\n"
	ids := parseExplicitGateIDs(script)
	got := strings.Join(ids, ",")
	want := "a,b,c,d"
	if got != want {
		t.Fatalf("parseExplicitGateIDs = %q, want %q", got, want)
	}
}

func TestCanonicalCommandLine(t *testing.T) {
	tests := []struct {
		run  string
		want string
	}{
		{"bash scripts/ci/release-gate.sh", "bash scripts/ci/release-gate.sh"},
		{"# comment\n\nbash scripts/ci/release-gate.sh\n", "bash scripts/ci/release-gate.sh"},
		{"bash scripts/ci/release-gate.sh --skip-full", "bash scripts/ci/release-gate.sh --skip-full"},
		{"echo hi", ""},
	}
	for _, tt := range tests {
		if got := canonicalCommandLine(tt.run); got != tt.want {
			t.Errorf("canonicalCommandLine(%q) = %q, want %q", tt.run, got, tt.want)
		}
	}
}
