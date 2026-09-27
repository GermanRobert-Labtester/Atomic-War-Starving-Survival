package monitor

import (
	"encoding/json"
	"os"
	"path/filepath"
)

// ReportSchemaVersion is the schema_version stamped on every monitor report.
// Bump it (and document the change) if the report shape changes in a way
// that breaks existing consumers.
const ReportSchemaVersion = "1.0"

// Severity classifies a Violation. Only SeverityError causes a non-zero
// process exit; SeverityWarning and SeverityInfo are always surfaced in the
// report but do not fail the gate.
type Severity string

const (
	SeverityError   Severity = "error"
	SeverityWarning Severity = "warning"
	SeverityInfo    Severity = "info"
)

// Violation is one finding. Confidence is a 0.0-1.0 estimate of how certain
// the monitor is that this finding is a real, actionable problem (1.0 for
// deterministic checks like byte-threshold comparisons; lower for anything
// derived from best-effort classification).
type Violation struct {
	Severity   Severity `json:"severity"`
	Confidence float64  `json:"confidence"`
	Category   string   `json:"category"`
	Path       string   `json:"path,omitempty"`
	Message    string   `json:"message"`
}

// Report is the common envelope for both monitor-size and monitor-compile.
// It is intentionally compact: no timestamps, no giant embedded logs.
type Report struct {
	SchemaVersion string      `json:"schema_version"`
	Monitor       string      `json:"monitor"`
	CommitSHA     string      `json:"commit_sha"`
	Passed        bool        `json:"passed"`
	Metrics       interface{} `json:"metrics"`
	Violations    []Violation `json:"violations"`
}

// NewReport builds a Report and derives Passed from the absence of any
// SeverityError violation.
func NewReport(monitorName, commitSHA string, metrics interface{}, violations []Violation) *Report {
	if violations == nil {
		violations = []Violation{}
	}
	passed := true
	for _, v := range violations {
		if v.Severity == SeverityError {
			passed = false
			break
		}
	}
	return &Report{
		SchemaVersion: ReportSchemaVersion,
		Monitor:       monitorName,
		CommitSHA:     commitSHA,
		Passed:        passed,
		Metrics:       metrics,
		Violations:    violations,
	}
}

// WriteJSON writes the report to outPath as indented JSON, creating parent
// directories as needed. Callers must write the report even when the
// monitor is about to exit non-zero — a policy violation is not a reason to
// withhold the evidence.
func (r *Report) WriteJSON(outPath string) error {
	data, err := json.MarshalIndent(r, "", "  ")
	if err != nil {
		return err
	}
	if dir := filepath.Dir(outPath); dir != "" && dir != "." {
		if err := os.MkdirAll(dir, 0o755); err != nil {
			return err
		}
	}
	return os.WriteFile(outPath, data, 0o644)
}

// HasError reports whether the report contains at least one SeverityError
// violation (used by the CLI to decide the process exit code).
func (r *Report) HasError() bool {
	return !r.Passed
}
