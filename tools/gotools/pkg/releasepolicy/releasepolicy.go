// Package releasepolicy verifies that the release workflow
// (.github/workflows/release.yml) actually runs every gate the CI gate
// manifest (docs/ci/CI_GATE_MANIFEST.json) marks release_required, that it
// runs them as a blocking step (no continue-on-error, no conditional
// suppression, no --skip-full), and that the export job cannot upload a
// build artifact that was never produced.
//
// This is a structural policy check, not a gate runner: it never executes
// the release gate, the exporters, or any test. It only parses the workflow
// YAML, the release-gate.sh script text, and the manifest JSON, and reports
// where they disagree.
package releasepolicy

import (
	"encoding/json"
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"regexp"
	"sort"
	"strings"
	"time"

	"github.com/goccy/go-yaml"
)

// SchemaVersion identifies the compact JSON report format produced by this
// package. Bump it if the Report shape changes in a way a consumer would
// need to know about.
const SchemaVersion = "1.0.0"

// CanonicalReleaseGateInvocation is the exact, unmodified command the
// release workflow's gate job must run as a blocking step. Any extra flag
// (most importantly --skip-full) or wrapper (continue-on-error, if:) defeats
// the purpose of a release gate that must never pass silently.
const CanonicalReleaseGateInvocation = "bash scripts/ci/release-gate.sh"

const (
	KindMissingRequiredGate = "missing_required_gate"
	KindNonblockingStep     = "nonblocking_step"
	KindWeakenedCriticality = "weakened_criticality"
	KindOrphanedReleaseID   = "orphaned_release_id"
	KindExportOrphan        = "export_orphan"
)

// ---------------------------------------------------------------------------
// Manifest (docs/ci/CI_GATE_MANIFEST.json) — only the fields this check uses.
// ---------------------------------------------------------------------------

// ManifestGate mirrors the subset of a CI_GATE_MANIFEST.json gate entry this
// policy check needs. ReleaseRequired defaults to false when the manifest
// predates that field, so this check is forward-compatible with a manifest
// that has not yet had release_required added to any gate.
type ManifestGate struct {
	GateID          string `json:"gate_id"`
	Command         string `json:"command"`
	Classification  string `json:"classification"`
	Critical        *bool  `json:"critical"`
	ReleaseRequired bool   `json:"release_required"`
}

// Manifest is the top-level shape of docs/ci/CI_GATE_MANIFEST.json.
type Manifest struct {
	Gates []ManifestGate `json:"gates"`
}

// LoadManifest reads and parses a CI gate manifest JSON file.
func LoadManifest(path string) (*Manifest, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, fmt.Errorf("read manifest %s: %w", path, err)
	}
	var m Manifest
	if err := json.Unmarshal(data, &m); err != nil {
		return nil, fmt.Errorf("parse manifest %s: %w", path, err)
	}
	return &m, nil
}

// ---------------------------------------------------------------------------
// Workflow (.github/workflows/release.yml) — only the fields this check uses.
// ---------------------------------------------------------------------------

// WorkflowStep mirrors the subset of a GitHub Actions step this check needs.
type WorkflowStep struct {
	Name            string         `yaml:"name"`
	Uses            string         `yaml:"uses"`
	Run             string         `yaml:"run"`
	If              string         `yaml:"if"`
	ContinueOnError any            `yaml:"continue-on-error"`
	With            map[string]any `yaml:"with"`
}

// WorkflowJob mirrors the subset of a GitHub Actions job this check needs.
type WorkflowJob struct {
	Needs           any            `yaml:"needs"`
	If              string         `yaml:"if"`
	ContinueOnError any            `yaml:"continue-on-error"`
	Steps           []WorkflowStep `yaml:"steps"`
}

// Workflow is the top-level shape of a GitHub Actions workflow file, reduced
// to the "jobs" map. Other top-level keys (name, on, ...) are intentionally
// unparsed; GitHub Actions' bare "on:" key is a YAML 1.1 boolean literal and
// this check has no use for it.
type Workflow struct {
	Jobs map[string]WorkflowJob `yaml:"jobs"`
}

// LoadWorkflow reads and parses a GitHub Actions workflow YAML file.
func LoadWorkflow(path string) (*Workflow, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, fmt.Errorf("read workflow %s: %w", path, err)
	}
	var w Workflow
	if err := yaml.Unmarshal(data, &w); err != nil {
		return nil, fmt.Errorf("parse workflow %s: %w", path, err)
	}
	return &w, nil
}

// ---------------------------------------------------------------------------
// Report shape.
// ---------------------------------------------------------------------------

// Violation is one concrete policy failure found while evaluating the
// workflow, script, and manifest together.
type Violation struct {
	Kind        string `json:"kind"`
	GateID      string `json:"gate_id,omitempty"`
	Location    string `json:"location"`
	Detail      string `json:"detail"`
	Remediation string `json:"remediation"`
}

// Evidence is a small, named fact used to reach the verdict — kept compact
// (IDs joined by comma) rather than dumping full gate objects.
type Evidence struct {
	Description string `json:"description"`
	Detail      string `json:"detail"`
}

// Metrics summarizes the sets this check compared.
type Metrics struct {
	RequiredGateCount     int `json:"required_gate_count"`
	FastTierGateCount     int `json:"fast_tier_gate_count"`
	ExplicitFullGateCount int `json:"explicit_full_gate_count"`
	WorkflowExecutedCount int `json:"workflow_executed_count"`
	ViolationCount        int `json:"violation_count"`
}

// Report is the compact, versioned JSON artifact this check writes via --out.
type Report struct {
	SchemaVersion    string      `json:"schema_version"`
	Commit           string      `json:"commit"`
	GeneratedAt      string      `json:"generated_at"`
	WorkflowPath     string      `json:"workflow_path"`
	ScriptPath       string      `json:"script_path"`
	ExportScriptPath string      `json:"export_script_path"`
	ManifestPath     string      `json:"manifest_path"`
	Pass             bool        `json:"pass"`
	Metrics          Metrics     `json:"metrics"`
	Violations       []Violation `json:"violations"`
	Evidence         []Evidence  `json:"evidence"`
	Remediation      []string    `json:"remediation"`
}

// WriteJSON marshals the report as indented JSON to path, creating parent
// directories as needed.
func (r Report) WriteJSON(path string) error {
	data, err := json.MarshalIndent(r, "", "  ")
	if err != nil {
		return err
	}
	if dir := filepath.Dir(path); dir != "" && dir != "." {
		if err := os.MkdirAll(dir, 0o755); err != nil {
			return err
		}
	}
	return os.WriteFile(path, append(data, '\n'), 0o644)
}

// ---------------------------------------------------------------------------
// Evaluate — the pure check. No file IO, no process execution, so tests can
// drive it directly with in-memory fixtures.
// ---------------------------------------------------------------------------

// Evaluate compares a parsed manifest against a parsed workflow, the raw
// release-gate.sh script text, and the raw export-build.sh script text, and
// returns every policy violation found. gateJobName/exportJobName are the
// job keys expected in the workflow ("release-gate" / "export-release" in
// the shipped release.yml).
//
// A release_required gate counts as executed by the workflow if either:
//   - it is fast-tier (verify-fast.sh always runs it), or
//   - release-gate.sh names it explicitly via `run-gates.py --gate ...`, or
//   - its manifest command carries a `--foo-selftest` flag that also
//     appears in the export job's steps or in exportScript. This is how
//     export_parity is satisfied today: the Linux export job runs the exact
//     selftest through scripts/ci/export-build.sh, not through
//     release-gate.sh, and that split is intentional (parity needs the
//     freshly exported package, not the repository tree).
func Evaluate(m *Manifest, wf *Workflow, script, exportScript, gateJobName, exportJobName string) Report {
	rep := Report{SchemaVersion: SchemaVersion, Violations: []Violation{}, Remediation: []string{}}

	gateByID := map[string]ManifestGate{}
	var fastIDs []string
	var requiredIDs []string
	for _, g := range m.Gates {
		gateByID[g.GateID] = g
		if g.Classification == "fast" {
			fastIDs = append(fastIDs, g.GateID)
		}
		if g.ReleaseRequired {
			requiredIDs = append(requiredIDs, g.GateID)
		}
	}
	sort.Strings(fastIDs)
	sort.Strings(requiredIDs)
	fastSet := toSet(fastIDs)

	// release_required gates must stay critical:true — a release_required
	// gate that is not critical could fail without failing run-gates.py.
	for _, id := range requiredIDs {
		g := gateByID[id]
		if g.Critical == nil || !*g.Critical {
			rep.Violations = append(rep.Violations, Violation{
				Kind:        KindWeakenedCriticality,
				GateID:      id,
				Location:    "docs/ci/CI_GATE_MANIFEST.json",
				Detail:      fmt.Sprintf("gate %q is release_required but critical is not true", id),
				Remediation: fmt.Sprintf("set \"critical\": true for gate_id %q in docs/ci/CI_GATE_MANIFEST.json", id),
			})
		}
	}

	// The gate IDs release-gate.sh names explicitly via run-gates.py --gate.
	explicitIDs := parseExplicitGateIDs(stripShellComments(script))
	explicitSet := toSet(explicitIDs)
	for _, id := range explicitIDs {
		if _, ok := gateByID[id]; !ok {
			rep.Violations = append(rep.Violations, Violation{
				Kind:        KindOrphanedReleaseID,
				GateID:      id,
				Location:    "scripts/ci/release-gate.sh",
				Detail:      fmt.Sprintf("release-gate.sh invokes gate id %q, which does not exist in docs/ci/CI_GATE_MANIFEST.json", id),
				Remediation: fmt.Sprintf("remove the stale gate id %q from the --gate list in scripts/ci/release-gate.sh, or restore it to the manifest", id),
			})
		}
	}

	if !fastInvocationRe.MatchString(stripShellComments(script)) {
		rep.Violations = append(rep.Violations, Violation{
			Kind:        KindMissingRequiredGate,
			GateID:      "fast_tier",
			Location:    "scripts/ci/release-gate.sh",
			Detail:      "release-gate.sh does not invoke scripts/ci/verify-fast.sh; the fast tier would not run on tag push",
			Remediation: "call `bash scripts/ci/verify-fast.sh` unconditionally from release-gate.sh before the full-tier gates",
		})
	}

	// A gate whose command carries a `--foo-selftest` flag also counts as
	// executed if the export job (or the export script it calls) invokes
	// that exact flag — this is how export_parity is satisfied today,
	// against the freshly exported package rather than the repo tree.
	exportSurface := exportJobSurface(wf, exportJobName) + "\n" + stripShellComments(exportScript)
	var exportSatisfiedIDs []string
	exportSatisfiedSet := map[string]bool{}
	for _, g := range m.Gates {
		flag := selftestFlagRe.FindString(g.Command)
		if flag != "" && strings.Contains(exportSurface, flag) {
			exportSatisfiedSet[g.GateID] = true
			exportSatisfiedIDs = append(exportSatisfiedIDs, g.GateID)
		}
	}
	sort.Strings(exportSatisfiedIDs)

	// release_required ⊆ workflow_executed, where workflow_executed is the
	// fast tier (always runs) union the gates release-gate.sh names
	// explicitly union the gates the export job satisfies directly. A
	// release_required gate that is in none of these is a gate the release
	// workflow can pass without ever having run.
	workflowExecuted := unionSet(unionSet(fastSet, explicitSet), exportSatisfiedSet)
	for _, id := range requiredIDs {
		if !workflowExecuted[id] {
			rep.Violations = append(rep.Violations, Violation{
				Kind:        KindMissingRequiredGate,
				GateID:      id,
				Location:    "scripts/ci/release-gate.sh",
				Detail:      fmt.Sprintf("gate %q is release_required but is neither fast-tier, explicitly invoked by release-gate.sh, nor satisfied by the export job", id),
				Remediation: fmt.Sprintf("add %q to the run-gates.py --gate list in scripts/ci/release-gate.sh, or wire its selftest flag into the export job", id),
			})
		}
	}

	rep.Violations = append(rep.Violations, checkGateJobStep(wf, gateJobName)...)
	rep.Violations = append(rep.Violations, checkExportJob(wf, gateJobName, exportJobName)...)

	rep.Metrics = Metrics{
		RequiredGateCount:     len(requiredIDs),
		FastTierGateCount:     len(fastIDs),
		ExplicitFullGateCount: len(explicitIDs),
		WorkflowExecutedCount: len(workflowExecuted),
		ViolationCount:        len(rep.Violations),
	}
	rep.Pass = len(rep.Violations) == 0
	rep.Evidence = []Evidence{
		{Description: "fast_tier_gate_ids", Detail: strings.Join(fastIDs, ",")},
		{Description: "release_required_gate_ids", Detail: strings.Join(requiredIDs, ",")},
		{Description: "explicit_full_tier_gate_ids", Detail: strings.Join(explicitIDs, ",")},
		{Description: "export_job_satisfied_gate_ids", Detail: strings.Join(exportSatisfiedIDs, ",")},
	}
	rep.Remediation = collectRemediation(rep.Violations)
	return rep
}

// exportJobSurface concatenates the `run:` text of every step in the export
// job, so a selftest flag search covers inline workflow commands too (e.g.
// the Windows export step), not just the scripts it shells out to.
func exportJobSurface(wf *Workflow, exportJobName string) string {
	job := wf.Jobs[exportJobName]
	var b strings.Builder
	for _, s := range job.Steps {
		b.WriteString(stripShellComments(s.Run))
		b.WriteString("\n")
	}
	return b.String()
}

// checkGateJobStep asserts the gate job invokes the canonical release-gate.sh
// as a single, unconditional, non-continue-on-error blocking step.
func checkGateJobStep(wf *Workflow, gateJobName string) []Violation {
	job, ok := wf.Jobs[gateJobName]
	if !ok {
		return []Violation{{
			Kind:        KindNonblockingStep,
			Location:    fmt.Sprintf("jobs.%s", gateJobName),
			Detail:      fmt.Sprintf("workflow has no job named %q", gateJobName),
			Remediation: fmt.Sprintf("add a %q job that runs `%s` as a blocking step", gateJobName, CanonicalReleaseGateInvocation),
		}}
	}

	var out []Violation
	if issue := blockingCondition(job.If, job.ContinueOnError); issue != "" {
		out = append(out, Violation{
			Kind:        KindNonblockingStep,
			Location:    fmt.Sprintf("jobs.%s", gateJobName),
			Detail:      "release gate job " + issue,
			Remediation: "run the release gate job unconditionally and without continue-on-error",
		})
	}
	found := false
	for i, step := range job.Steps {
		line := canonicalCommandLine(step.Run)
		if line == "" {
			continue
		}
		found = true
		if issue := stepBlockingIssue(line, step.If, step.ContinueOnError); issue != "" {
			out = append(out, Violation{
				Kind:        KindNonblockingStep,
				Location:    fmt.Sprintf("jobs.%s.steps[%d]", gateJobName, i),
				Detail:      issue,
				Remediation: fmt.Sprintf("invoke exactly `%s` with no if:, continue-on-error, or extra flags", CanonicalReleaseGateInvocation),
			})
		}
	}
	if !found {
		out = append(out, Violation{
			Kind:        KindNonblockingStep,
			Location:    fmt.Sprintf("jobs.%s", gateJobName),
			Detail:      fmt.Sprintf("no step in job %q invokes `%s`", gateJobName, CanonicalReleaseGateInvocation),
			Remediation: fmt.Sprintf("add a run step invoking exactly `%s`", CanonicalReleaseGateInvocation),
		})
	}
	return out
}

// checkExportJob asserts the export job depends on the gate job, runs the
// Linux export/parity script and the Windows export before either upload,
// and uploads both artifacts with if-no-files-found: error so a missing
// build fails the workflow instead of merely warning (the "false green"
// this check exists to prevent).
func checkExportJob(wf *Workflow, gateJobName, exportJobName string) []Violation {
	job, ok := wf.Jobs[exportJobName]
	if !ok {
		return []Violation{{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s", exportJobName),
			Detail:      fmt.Sprintf("workflow has no job named %q", exportJobName),
			Remediation: fmt.Sprintf("add a %q job that needs %q and exports both Linux and Windows builds", exportJobName, gateJobName),
		}}
	}

	var out []Violation
	if issue := blockingCondition(job.If, job.ContinueOnError); issue != "" {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s", exportJobName),
			Detail:      "export job " + issue,
			Remediation: "run the export job unconditionally after the release gate succeeds",
		})
	}

	if !containsString(needsList(job.Needs), gateJobName) {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s.needs", exportJobName),
			Detail:      fmt.Sprintf("job %q does not `needs: %s`; a failed release gate would not block the export", exportJobName, gateJobName),
			Remediation: fmt.Sprintf("add `needs: %s` to job %q", gateJobName, exportJobName),
		})
	}

	linuxExportIdx := findStepIndex(job.Steps, func(s WorkflowStep) bool {
		return strings.Contains(stripShellComments(s.Run), "export-build.sh")
	})
	winExportIdx := findStepIndex(job.Steps, func(s WorkflowStep) bool {
		run := stripShellComments(s.Run)
		return strings.Contains(run, "--export-release") && strings.Contains(run, "Windows Desktop")
	})
	linuxUploadIdx, linuxUploadStrict := findUploadStep(job.Steps, "builds/linux")
	winUploadIdx, winUploadStrict := findUploadStep(job.Steps, "builds/windows")

	if linuxExportIdx < 0 {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s", exportJobName),
			Detail:      "no step invokes scripts/ci/export-build.sh (Linux export + packaged parity)",
			Remediation: "add a run step invoking `bash scripts/ci/export-build.sh`",
		})
	}
	if winExportIdx < 0 {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s", exportJobName),
			Detail:      `no step exports the Windows Desktop build (--export-release "Windows Desktop")`,
			Remediation: `add a run step invoking scripts/ci/run-godot-bounded.sh --export-release "Windows Desktop" builds/windows/ashfall.exe`,
		})
	}
	if linuxExportIdx >= 0 && winExportIdx >= 0 && winExportIdx < linuxExportIdx {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s", exportJobName),
			Detail:      "the Windows export step runs before the Linux export/parity step",
			Remediation: "move the Windows export step after the export-build.sh step",
		})
	}
	for _, idx := range []int{linuxExportIdx, winExportIdx, linuxUploadIdx, winUploadIdx} {
		if idx < 0 {
			continue
		}
		step := job.Steps[idx]
		if issue := blockingCondition(step.If, step.ContinueOnError); issue != "" {
			out = append(out, Violation{
				Kind:        KindExportOrphan,
				Location:    fmt.Sprintf("jobs.%s.steps[%d]", exportJobName, idx),
				Detail:      "export or artifact step " + issue,
				Remediation: "remove the conditional or continue-on-error from the export and artifact steps",
			})
		}
	}

	out = append(out, checkUploadStep(exportJobName, "builds/linux", linuxExportIdx, linuxUploadIdx, linuxUploadStrict)...)
	out = append(out, checkUploadStep(exportJobName, "builds/windows", winExportIdx, winUploadIdx, winUploadStrict)...)

	return out
}

func checkUploadStep(jobName, artifactPath string, exportIdx, uploadIdx int, strict bool) []Violation {
	var out []Violation
	if uploadIdx < 0 {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s", jobName),
			Detail:      fmt.Sprintf("no upload-artifact step uploads %s", artifactPath),
			Remediation: fmt.Sprintf("add an actions/upload-artifact step with path: %s and if-no-files-found: error", artifactPath),
		})
		return out
	}
	if !strict {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s.steps[%d]", jobName, uploadIdx),
			Detail:      fmt.Sprintf("%s upload does not set if-no-files-found: error, so a missing artifact would only warn (false green)", artifactPath),
			Remediation: fmt.Sprintf("set `if-no-files-found: error` on the %s upload-artifact step", artifactPath),
		})
	}
	if exportIdx >= 0 && uploadIdx < exportIdx {
		out = append(out, Violation{
			Kind:        KindExportOrphan,
			Location:    fmt.Sprintf("jobs.%s.steps[%d]", jobName, uploadIdx),
			Detail:      fmt.Sprintf("%s is uploaded before the export step that produces it", artifactPath),
			Remediation: fmt.Sprintf("move the %s upload step after the export step that produces it", artifactPath),
		})
	}
	return out
}

// ---------------------------------------------------------------------------
// Small parsing/set helpers.
// ---------------------------------------------------------------------------

var gateFlagRe = regexp.MustCompile(`--gate\s+((?:[A-Za-z0-9_]+\s*,\s*)*[A-Za-z0-9_]+)`)
var fastInvocationRe = regexp.MustCompile(`(?m)^\s*(?:if\s+)?bash\s+scripts/ci/verify-fast\.sh(?:\s|;|$)`)

// stripShellComments removes full-line comments from inspected shell/YAML run
// blocks so mentioning a command in documentation cannot satisfy the monitor.
// It intentionally does not attempt to interpret arbitrary shell control flow.
func stripShellComments(script string) string {
	lines := strings.Split(script, "\n")
	var active []string
	for _, line := range lines {
		if strings.HasPrefix(strings.TrimSpace(line), "#") {
			continue
		}
		active = append(active, line)
	}
	return strings.Join(active, "\n")
}

// selftestFlagRe extracts the distinguishing `--foo-selftest` flag from a
// manifest gate's command, e.g. "--export-parity-selftest" out of
// "bash scripts/ci/run-godot-bounded.sh --path . -- --export-parity-selftest".
// Gates without such a flag (dotnet test, python3 script gates, ...) are not
// candidates for export-job satisfaction and FindString returns "".
var selftestFlagRe = regexp.MustCompile(`--[a-zA-Z0-9][a-zA-Z0-9-]*-selftest\b`)

// parseExplicitGateIDs extracts every gate id named in a
// `run-gates.py --gate id1,id2` invocation anywhere in the script text.
func parseExplicitGateIDs(script string) []string {
	matches := gateFlagRe.FindAllStringSubmatch(script, -1)
	seen := map[string]bool{}
	var ids []string
	for _, m := range matches {
		for _, id := range strings.Split(m[1], ",") {
			id = strings.TrimSpace(id)
			if id == "" || seen[id] {
				continue
			}
			seen[id] = true
			ids = append(ids, id)
		}
	}
	sort.Strings(ids)
	return ids
}

// canonicalCommandLine scans a `run:` block for the single non-comment,
// non-blank line that starts with the canonical release-gate invocation, or
// returns "" if no such line exists. Block-scalar `run:` steps are routinely
// annotated with comments and trailing diagnostics; only the executable
// invocation line matters here.
func canonicalCommandLine(run string) string {
	for _, raw := range strings.Split(run, "\n") {
		line := strings.TrimSpace(raw)
		if line == "" || strings.HasPrefix(line, "#") {
			continue
		}
		if strings.HasPrefix(line, "bash scripts/ci/release-gate.sh") {
			return line
		}
	}
	return ""
}

// stepBlockingIssue returns a human-readable reason the step is not a valid
// blocking canonical-release-gate invocation, or "" if it is valid.
func stepBlockingIssue(line, ifCond string, continueOnError any) string {
	if issue := blockingCondition(ifCond, continueOnError); issue != "" {
		return issue
	}
	if strings.Contains(line, "--skip-full") {
		return "step invokes release-gate.sh with --skip-full, which drops the required full-tier gates in CI"
	}
	if line != CanonicalReleaseGateInvocation {
		return fmt.Sprintf("step invokes %q instead of the exact canonical command %q", line, CanonicalReleaseGateInvocation)
	}
	return ""
}

func blockingCondition(ifCond string, continueOnError any) string {
	if strings.TrimSpace(ifCond) != "" {
		return fmt.Sprintf("has a conditional 'if: %s' that can skip the required step", strings.TrimSpace(ifCond))
	}
	if isTruthy(continueOnError) {
		return "sets continue-on-error and can mask a failed gate"
	}
	return ""
}

func isTruthy(v any) bool {
	switch t := v.(type) {
	case bool:
		return t
	case string:
		s := strings.TrimSpace(strings.ToLower(t))
		return s != "" && s != "false"
	default:
		return false
	}
}

func needsList(v any) []string {
	switch t := v.(type) {
	case string:
		return []string{t}
	case []any:
		out := make([]string, 0, len(t))
		for _, e := range t {
			if s, ok := e.(string); ok {
				out = append(out, s)
			}
		}
		return out
	default:
		return nil
	}
}

func containsString(list []string, target string) bool {
	for _, s := range list {
		if s == target {
			return true
		}
	}
	return false
}

func findStepIndex(steps []WorkflowStep, pred func(WorkflowStep) bool) int {
	for i, s := range steps {
		if pred(s) {
			return i
		}
	}
	return -1
}

// findUploadStep finds an actions/upload-artifact step whose `with.path`
// contains pathSubstr, and reports whether it strictly fails on a missing
// artifact (if-no-files-found: error).
func findUploadStep(steps []WorkflowStep, pathSubstr string) (int, bool) {
	for i, s := range steps {
		if !strings.HasPrefix(s.Uses, "actions/upload-artifact") {
			continue
		}
		p, _ := s.With["path"].(string)
		if strings.Contains(p, pathSubstr) {
			mode, _ := s.With["if-no-files-found"].(string)
			return i, mode == "error"
		}
	}
	return -1, false
}

func toSet(ids []string) map[string]bool {
	set := make(map[string]bool, len(ids))
	for _, id := range ids {
		set[id] = true
	}
	return set
}

func unionSet(a, b map[string]bool) map[string]bool {
	out := make(map[string]bool, len(a)+len(b))
	for k := range a {
		out[k] = true
	}
	for k := range b {
		out[k] = true
	}
	return out
}

func collectRemediation(violations []Violation) []string {
	seen := map[string]bool{}
	out := []string{}
	for _, v := range violations {
		if !seen[v.Remediation] {
			seen[v.Remediation] = true
			out = append(out, v.Remediation)
		}
	}
	return out
}

// ---------------------------------------------------------------------------
// Run — file-IO entry point used by cmd/releasepolicy.
// ---------------------------------------------------------------------------

// Run loads the manifest, workflow, release-gate script, and export script
// from disk (paths relative to root) and evaluates them. It never executes
// the release gate, exporters, or any test — it only reads these four files.
// A missing exportScriptRel is non-fatal (empty content is evaluated as-is)
// since export-job satisfaction is an additive check, not this tool's core
// invariant.
func Run(root, workflowRel, scriptRel, exportScriptRel, manifestRel string) (Report, error) {
	m, err := LoadManifest(filepath.Join(root, manifestRel))
	if err != nil {
		return Report{}, err
	}
	wf, err := LoadWorkflow(filepath.Join(root, workflowRel))
	if err != nil {
		return Report{}, err
	}
	scriptPath := filepath.Join(root, scriptRel)
	scriptBytes, err := os.ReadFile(scriptPath)
	if err != nil {
		return Report{}, fmt.Errorf("read script %s: %w", scriptPath, err)
	}
	exportScriptPath := filepath.Join(root, exportScriptRel)
	exportScriptBytes, _ := os.ReadFile(exportScriptPath) // best-effort; see doc comment

	rep := Evaluate(m, wf, string(scriptBytes), string(exportScriptBytes), "release-gate", "export-release")
	rep.WorkflowPath = workflowRel
	rep.ScriptPath = scriptRel
	rep.ExportScriptPath = exportScriptRel
	rep.ManifestPath = manifestRel
	rep.GeneratedAt = time.Now().UTC().Format(time.RFC3339)
	rep.Commit = commitHash(root)
	return rep, nil
}

func commitHash(root string) string {
	cmd := exec.Command("git", "rev-parse", "HEAD")
	if abs, err := filepath.Abs(root); err == nil {
		cmd.Dir = abs
	}
	out, err := cmd.Output()
	if err != nil {
		return "unknown"
	}
	return strings.TrimSpace(string(out))
}
