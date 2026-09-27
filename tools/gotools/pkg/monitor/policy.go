package monitor

import (
	"encoding/json"
	"fmt"
	"os"
)

// SizePolicy is the "size" section of docs/ci/MONITORING_POLICY.json.
type SizePolicy struct {
	NewMarkdownMaxBytes int64    `json:"new_markdown_max_bytes"`
	PRGrowthFailBytes   int64    `json:"pr_growth_fail_bytes"`
	PRGrowthWarnBytes   int64    `json:"pr_growth_warn_bytes"`
	MarkdownAllowlist   []string `json:"markdown_allowlist"`
}

// KnownCompilePath is one explicit-allowlist entry for a tracked .cs path
// that is intentionally uncompiled, or intentionally compiled only by the
// test project outside its normal test root.
type KnownCompilePath struct {
	Path           string   `json:"path"`
	Classification string   `json:"classification"`
	Severity       Severity `json:"severity"`
	Reason         string   `json:"reason"`
}

// CompilePolicy is the "compile" section of docs/ci/MONITORING_POLICY.json.
type CompilePolicy struct {
	ProductionProjects []string           `json:"production_projects"`
	TestProjects       []string           `json:"test_projects"`
	TestRootPrefix     string             `json:"test_root_prefix"`
	KnownPaths         []KnownCompilePath `json:"known_paths"`
}

// Policy is the full docs/ci/MONITORING_POLICY.json document.
type Policy struct {
	SchemaVersion string        `json:"schema_version"`
	Size          SizePolicy    `json:"size"`
	Compile       CompilePolicy `json:"compile"`
}

// LoadPolicy reads and parses the monitoring policy file. A missing or
// malformed policy file is a hard configuration error (fail closed): both
// monitors need it to know their thresholds and allowlists, so silently
// falling back to defaults would hide the exact rules being enforced.
func LoadPolicy(path string) (*Policy, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return nil, fmt.Errorf("read monitoring policy %s: %w", path, err)
	}
	var p Policy
	if err := json.Unmarshal(data, &p); err != nil {
		return nil, fmt.Errorf("parse monitoring policy %s: %w", path, err)
	}
	if p.Size.NewMarkdownMaxBytes <= 0 || p.Size.PRGrowthFailBytes <= 0 {
		return nil, fmt.Errorf("monitoring policy %s: size thresholds must be positive", path)
	}
	if len(p.Compile.ProductionProjects) == 0 || len(p.Compile.TestProjects) == 0 {
		return nil, fmt.Errorf("monitoring policy %s: compile.production_projects and compile.test_projects must be non-empty", path)
	}
	return &p, nil
}

// KnownPath looks up an exact-path allowlist entry by repo-relative,
// forward-slash path.
func (c *CompilePolicy) KnownPath(path string) (KnownCompilePath, bool) {
	for _, k := range c.KnownPaths {
		if k.Path == path {
			return k, true
		}
	}
	return KnownCompilePath{}, false
}
