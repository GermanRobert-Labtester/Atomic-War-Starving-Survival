package monitor

import (
	"fmt"
	"path/filepath"
	"strings"
)

// SizeOptions configures RunSizeMonitor.
type SizeOptions struct {
	Root       string // absolute repository root
	BaseRef    string // git ref to diff HEAD against (required; fail-closed if unresolvable)
	PolicyPath string // path to docs/ci/MONITORING_POLICY.json
}

// SizeMetrics is the "metrics" payload for the monitor-size report.
type SizeMetrics struct {
	HeadCommit           string `json:"head_commit"`
	BaseRef              string `json:"base_ref"`
	BaseCommit           string `json:"base_commit,omitempty"`
	HeadTrackedFiles     int    `json:"head_tracked_files"`
	BaseTrackedFiles     int    `json:"base_tracked_files,omitempty"`
	HeadTotalBytes       int64  `json:"head_total_bytes"`
	BaseTotalBytes       int64  `json:"base_total_bytes,omitempty"`
	GrowthBytes          int64  `json:"growth_bytes,omitempty"`
	NewMarkdownCount     int    `json:"new_markdown_count"`
	NewMarkdownOverCount int    `json:"new_markdown_over_limit_count"`
	BaseResolved         bool   `json:"base_resolved"`
}

func isMarkdownPath(path string) bool {
	ext := strings.ToLower(filepath.Ext(path))
	return ext == ".md" || ext == ".markdown"
}

func blobMap(entries []TreeEntry) (map[string]int64, int64) {
	m := make(map[string]int64, len(entries))
	var total int64
	for _, e := range entries {
		m[e.Path] = e.Size
		total += e.Size
	}
	return m, total
}

func contains(list []string, target string) bool {
	for _, v := range list {
		if v == target {
			return true
		}
	}
	return false
}

// RunSizeMonitor computes repository tracked-blob growth between BaseRef and
// HEAD and checks it, plus any newly added Markdown files, against
// docs/ci/MONITORING_POLICY.json. It always returns a fully-formed Report;
// callers should write it to disk regardless of whether an error is also
// returned. An error is only returned for infrastructure failures (git
// binary missing, root is not a git repository, policy unreadable) where no
// meaningful report metrics could be computed at all.
func RunSizeMonitor(opts SizeOptions) (*Report, error) {
	policy, err := LoadPolicy(opts.PolicyPath)
	if err != nil {
		return nil, err
	}

	headSHA, err := HeadCommit(opts.Root)
	if err != nil {
		return nil, fmt.Errorf("resolve HEAD: %w", err)
	}

	headEntries, err := ListTreeBlobs(opts.Root, headSHA)
	if err != nil {
		return nil, fmt.Errorf("list HEAD tree: %w", err)
	}
	headMap, headTotal := blobMap(headEntries)

	metrics := &SizeMetrics{
		HeadCommit:       headSHA,
		BaseRef:          opts.BaseRef,
		HeadTrackedFiles: len(headMap),
		HeadTotalBytes:   headTotal,
	}

	var violations []Violation

	baseSHA, baseErr := ResolveCommit(opts.Root, opts.BaseRef)
	if baseErr != nil {
		// Fail closed: no base means no growth comparison is possible, and
		// that absence is itself the failure — never silently pass.
		violations = append(violations, Violation{
			Severity:   SeverityError,
			Confidence: 1.0,
			Category:   "base_ref_unresolved",
			Message:    fmt.Sprintf("base ref %q could not be resolved to a commit (%v); size-growth comparison requires a valid --base and fails closed when it is missing", opts.BaseRef, baseErr),
		})
		return NewReport("monitor-size", headSHA, metrics, violations), nil
	}

	metrics.BaseResolved = true
	metrics.BaseCommit = baseSHA

	baseEntries, err := ListTreeBlobs(opts.Root, baseSHA)
	if err != nil {
		return nil, fmt.Errorf("list base tree at %s: %w", baseSHA, err)
	}
	baseMap, baseTotal := blobMap(baseEntries)
	metrics.BaseTrackedFiles = len(baseMap)
	metrics.BaseTotalBytes = baseTotal

	growth := headTotal - baseTotal
	metrics.GrowthBytes = growth

	if growth > policy.Size.PRGrowthFailBytes {
		violations = append(violations, Violation{
			Severity:   SeverityError,
			Confidence: 1.0,
			Category:   "growth_exceeds_fail_threshold",
			Message: fmt.Sprintf("tracked-blob growth %d bytes exceeds fail threshold %d bytes (base %s -> HEAD %s)",
				growth, policy.Size.PRGrowthFailBytes, shortSHA(baseSHA), shortSHA(headSHA)),
		})
	} else if growth > policy.Size.PRGrowthWarnBytes {
		violations = append(violations, Violation{
			Severity:   SeverityWarning,
			Confidence: 1.0,
			Category:   "growth_exceeds_warn_threshold",
			Message: fmt.Sprintf("tracked-blob growth %d bytes exceeds warn threshold %d bytes (base %s -> HEAD %s)",
				growth, policy.Size.PRGrowthWarnBytes, shortSHA(baseSHA), shortSHA(headSHA)),
		})
	}

	// New Markdown files only: a file already present in base (even if it
	// grew) is not in scope for this check per policy — absolute legacy
	// bloat is tracked separately and is explicitly not gated here.
	for path, size := range headMap {
		if !isMarkdownPath(path) {
			continue
		}
		if _, existedInBase := baseMap[path]; existedInBase {
			continue
		}
		metrics.NewMarkdownCount++
		if size <= policy.Size.NewMarkdownMaxBytes {
			continue
		}
		if contains(policy.Size.MarkdownAllowlist, path) {
			violations = append(violations, Violation{
				Severity:   SeverityInfo,
				Confidence: 1.0,
				Category:   "new_markdown_allowlisted",
				Path:       path,
				Message:    fmt.Sprintf("new markdown file is %d bytes (over %d byte cap) but is explicitly allowlisted in policy", size, policy.Size.NewMarkdownMaxBytes),
			})
			continue
		}
		metrics.NewMarkdownOverCount++
		violations = append(violations, Violation{
			Severity:   SeverityError,
			Confidence: 1.0,
			Category:   "new_markdown_oversize",
			Path:       path,
			Message:    fmt.Sprintf("new markdown file is %d bytes, exceeding the %d byte cap for newly added Markdown; add an explicit allowlist entry in docs/ci/MONITORING_POLICY.json if this is intentional", size, policy.Size.NewMarkdownMaxBytes),
		})
	}

	return NewReport("monitor-size", headSHA, metrics, violations), nil
}

func shortSHA(sha string) string {
	if len(sha) > 10 {
		return sha[:10]
	}
	return sha
}
