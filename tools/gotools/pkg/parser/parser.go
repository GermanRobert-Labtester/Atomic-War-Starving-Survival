package parser

import (
	"bufio"
	"encoding/json"
	"fmt"
	"regexp"
	"strconv"
	"strings"
)

type FailureTaxonomy string

const (
	TaxonomyPass           FailureTaxonomy = "pass"
	TaxonomyAssertFail     FailureTaxonomy = "assert-fail"
	TaxonomyBuildBreak     FailureTaxonomy = "build-break"
	TaxonomyTimeout        FailureTaxonomy = "timeout"
	TaxonomyInfrastructure FailureTaxonomy = "infrastructure"
	TaxonomyQuarantined    FailureTaxonomy = "quarantined"
	TaxonomyUnknown        FailureTaxonomy = "unknown-failure"
)

type TestCaseFailure struct {
	Name    string `json:"name"`
	Message string `json:"message"`
	Details string `json:"details,omitempty"`
}

type ParsedResult struct {
	Runner       string          `json:"runner"` // "xunit", "pytest", "generic"
	Taxonomy     FailureTaxonomy `json:"taxonomy"`
	Passed       int             `json:"passed"`
	Failed       int             `json:"failed"`
	Skipped      int             `json:"skipped"`
	Total        int             `json:"total"`
	DurationStr  string          `json:"duration_str"`
	Failures     []TestCaseFailure `json:"failures,omitempty"`
	RawSummary   string          `json:"raw_summary"`
}

var (
	// dotnet test regexes
	// Passed!  - Failed:     0, Passed:    32, Skipped:     0, Total:    32, Duration: 49 ms - Ashfall.Core.Tests.dll (net9.0)
	reDotnetSummary = regexp.MustCompile(`(?:Passed!|Failed!)\s+-\s+Failed:\s+(\d+),\s+Passed:\s+(\d+),\s+Skipped:\s+(\d+),\s+Total:\s+(\d+)(?:,\s+Duration:\s+([^-\n]+))?`)
	// Failed [TestName]
	reDotnetFailedTest = regexp.MustCompile(`\[FAIL\]\s+(.+)`)

	// pytest regexes
	// ===== 5 passed, 1 failed, 1 skipped in 0.94s =====
	rePytestSummary = regexp.MustCompile(`=+\s+(?:(\d+)\s+passed)?(?:,\s*)?(?:(\d+)\s+failed)?(?:,\s*)?(?:(\d+)\s+skipped)?.*in\s+([0-9.]+s)\s+=+`)
	rePytestFailedTest = regexp.MustCompile(`FAILED\s+([^ -]+)`)
)

func ParseTestOutput(output string, exitCode int) *ParsedResult {
	res := &ParsedResult{
		Runner:   "generic",
		Taxonomy: TaxonomyPass,
	}

	if exitCode == 124 {
		res.Taxonomy = TaxonomyTimeout
		res.RawSummary = "Execution timed out (exit code 124)"
		return res
	}

	if strings.Contains(output, "error CS") || strings.Contains(output, "MSBUILD : error") {
		res.Taxonomy = TaxonomyBuildBreak
		res.RawSummary = "Build compilation broken"
		return res
	}

	// 1. Check dotnet test pattern
	if m := reDotnetSummary.FindStringSubmatch(output); m != nil {
		res.Runner = "dotnet/xunit"
		res.Failed, _ = strconv.Atoi(m[1])
		res.Passed, _ = strconv.Atoi(m[2])
		res.Skipped, _ = strconv.Atoi(m[3])
		res.Total, _ = strconv.Atoi(m[4])
		if len(m) > 5 {
			res.DurationStr = strings.TrimSpace(m[5])
		}
		res.RawSummary = m[0]
	} else if mPy := rePytestSummary.FindStringSubmatch(output); mPy != nil {
		res.Runner = "pytest"
		if mPy[1] != "" {
			res.Passed, _ = strconv.Atoi(mPy[1])
		}
		if mPy[2] != "" {
			res.Failed, _ = strconv.Atoi(mPy[2])
		}
		if mPy[3] != "" {
			res.Skipped, _ = strconv.Atoi(mPy[3])
		}
		res.Total = res.Passed + res.Failed + res.Skipped
		res.DurationStr = mPy[4]
		res.RawSummary = mPy[0]
	}

	// Scan for failure lines
	scanner := bufio.NewScanner(strings.NewReader(output))
	for scanner.Scan() {
		line := scanner.Text()
		if fMatch := reDotnetFailedTest.FindStringSubmatch(line); fMatch != nil {
			res.Failures = append(res.Failures, TestCaseFailure{
				Name:    strings.TrimSpace(fMatch[1]),
				Message: line,
			})
		} else if fPy := rePytestFailedTest.FindStringSubmatch(line); fPy != nil {
			res.Failures = append(res.Failures, TestCaseFailure{
				Name:    strings.TrimSpace(fPy[1]),
				Message: line,
			})
		}
	}

	if res.Failed > 0 || exitCode != 0 {
		if res.Failed > 0 {
			res.Taxonomy = TaxonomyAssertFail
		} else if exitCode != 0 && res.Taxonomy == TaxonomyPass {
			res.Taxonomy = TaxonomyUnknown
		}
	}

	return res
}

func (r *ParsedResult) MarkdownReport() string {
	var sb strings.Builder
	sb.WriteString("### Test Execution Result\n\n")
	sb.WriteString(fmt.Sprintf("- **Runner:** `%s`\n", r.Runner))
	sb.WriteString(fmt.Sprintf("- **Status / Taxonomy:** `%s`\n", r.Taxonomy))
	sb.WriteString(fmt.Sprintf("- **Summary:** Passed: %d, Failed: %d, Skipped: %d, Total: %d (Duration: %s)\n",
		r.Passed, r.Failed, r.Skipped, r.Total, r.DurationStr))

	if len(r.Failures) > 0 {
		sb.WriteString("\n#### Failures:\n")
		for _, f := range r.Failures {
			sb.WriteString(fmt.Sprintf("- ❌ `%s`: %s\n", f.Name, f.Message))
		}
	}
	return sb.String()
}

func (r *ParsedResult) ToJSON() ([]byte, error) {
	return json.MarshalIndent(r, "", "  ")
}
