package monitor

import (
	"strings"
	"testing"
)

// sizePolicyFixture builds a Policy with the given size thresholds. The
// compile section is populated with harmless placeholder values only to
// satisfy LoadPolicy's validation; RunSizeMonitor never reads it.
func sizePolicyFixture(markdownMax, growthFail, growthWarn int64, allowlist []string) Policy {
	return Policy{
		SchemaVersion: "1.0",
		Size: SizePolicy{
			NewMarkdownMaxBytes: markdownMax,
			PRGrowthFailBytes:   growthFail,
			PRGrowthWarnBytes:   growthWarn,
			MarkdownAllowlist:   allowlist,
		},
		Compile: CompilePolicy{
			ProductionProjects: []string{"placeholder.csproj"},
			TestProjects:       []string{"placeholder.tests.csproj"},
			TestRootPrefix:     "Tests/",
		},
	}
}

func TestRunSizeMonitor_PassWithinThresholds(t *testing.T) {
	dir := initTestRepo(t)
	writeFile(t, dir, "src/a.txt", []byte("baseline content"))
	baseSHA := commitAll(t, dir, "base commit")

	writeFile(t, dir, "src/b.txt", []byte("small addition"))
	commitAll(t, dir, "head commit: small addition")

	policyPath := writePolicy(t, dir, sizePolicyFixture(1<<20, 1<<20, 1<<10, nil))

	report, err := RunSizeMonitor(SizeOptions{Root: dir, BaseRef: baseSHA, PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunSizeMonitor: %v", err)
	}
	if !report.Passed {
		t.Fatalf("expected pass, got violations: %+v", report.Violations)
	}
	if report.Violations == nil {
		t.Fatal("expected an empty violations array, not JSON null")
	}
	metrics := report.Metrics.(*SizeMetrics)
	if !metrics.BaseResolved {
		t.Fatalf("expected base to resolve")
	}
	if metrics.GrowthBytes != int64(len("small addition")) {
		t.Errorf("expected growth %d, got %d", len("small addition"), metrics.GrowthBytes)
	}
}

func TestRunSizeMonitor_NewMarkdownOversizeFailsUnlessAllowlisted(t *testing.T) {
	dir := initTestRepo(t)
	writeFile(t, dir, "src/a.txt", []byte("baseline"))
	baseSHA := commitAll(t, dir, "base commit")

	bigMD := strings.Repeat("x", 2048) // exceeds the 1024-byte cap used below
	writeFile(t, dir, "docs/NEW_BIG.md", []byte(bigMD))
	commitAll(t, dir, "head commit: oversize new markdown")

	t.Run("fails_without_allowlist", func(t *testing.T) {
		policyPath := writePolicy(t, dir, sizePolicyFixture(1024, 1<<30, 1<<20, nil))
		report, err := RunSizeMonitor(SizeOptions{Root: dir, BaseRef: baseSHA, PolicyPath: policyPath})
		if err != nil {
			t.Fatalf("RunSizeMonitor: %v", err)
		}
		if report.Passed {
			t.Fatalf("expected failure for oversize new markdown, got passed=true")
		}
		by := violationsByPath(report.Violations)
		v, ok := by["docs/NEW_BIG.md"]
		if !ok {
			t.Fatalf("expected a violation for docs/NEW_BIG.md, got %+v", report.Violations)
		}
		if v.Severity != SeverityError || v.Category != "new_markdown_oversize" {
			t.Errorf("unexpected violation shape: %+v", v)
		}
	})

	t.Run("passes_when_explicitly_allowlisted", func(t *testing.T) {
		policyPath := writePolicy(t, dir, sizePolicyFixture(1024, 1<<30, 1<<20, []string{"docs/NEW_BIG.md"}))
		report, err := RunSizeMonitor(SizeOptions{Root: dir, BaseRef: baseSHA, PolicyPath: policyPath})
		if err != nil {
			t.Fatalf("RunSizeMonitor: %v", err)
		}
		if !report.Passed {
			t.Fatalf("expected pass once allowlisted, got violations: %+v", report.Violations)
		}
		by := violationsByPath(report.Violations)
		v, ok := by["docs/NEW_BIG.md"]
		if !ok {
			t.Fatalf("expected the allowlisted file to still be surfaced, not silently dropped")
		}
		if v.Severity != SeverityInfo || v.Category != "new_markdown_allowlisted" {
			t.Errorf("unexpected violation shape for allowlisted file: %+v", v)
		}
	})
}

// TestRunSizeMonitor_ExistingMarkdownNotFlaggedRegression guards against a
// regression where the new-markdown cap starts applying to every Markdown
// file in HEAD instead of only ones added since base. The repo's hundreds of
// pre-existing >2MiB plan/audit docs must never trip this check.
func TestRunSizeMonitor_ExistingMarkdownNotFlaggedRegression(t *testing.T) {
	dir := initTestRepo(t)
	bigMD := strings.Repeat("y", 2048)
	writeFile(t, dir, "docs/ALREADY_HERE.md", []byte(bigMD))
	writeFile(t, dir, "src/a.txt", []byte("baseline"))
	baseSHA := commitAll(t, dir, "base commit already has an oversize markdown file")

	// Head only touches an unrelated file; the oversize markdown is
	// untouched and already existed in base.
	writeFile(t, dir, "src/a.txt", []byte("baseline modified slightly"))
	commitAll(t, dir, "head commit: unrelated change")

	policyPath := writePolicy(t, dir, sizePolicyFixture(1024, 1<<30, 1<<20, nil))
	report, err := RunSizeMonitor(SizeOptions{Root: dir, BaseRef: baseSHA, PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunSizeMonitor: %v", err)
	}
	if !report.Passed {
		t.Fatalf("expected pass: pre-existing oversize markdown must not be flagged, got: %+v", report.Violations)
	}
	metrics := report.Metrics.(*SizeMetrics)
	if metrics.NewMarkdownCount != 0 {
		t.Errorf("expected 0 new markdown files, got %d", metrics.NewMarkdownCount)
	}
	if _, flagged := violationsByPath(report.Violations)["docs/ALREADY_HERE.md"]; flagged {
		t.Errorf("pre-existing markdown file must not appear in violations")
	}
}

func TestRunSizeMonitor_GrowthExceedsFailThreshold(t *testing.T) {
	dir := initTestRepo(t)
	writeFile(t, dir, "src/a.txt", []byte("baseline"))
	baseSHA := commitAll(t, dir, "base commit")

	writeFile(t, dir, "assets/big.bin", []byte(strings.Repeat("z", 4096)))
	commitAll(t, dir, "head commit: large binary addition")

	policyPath := writePolicy(t, dir, sizePolicyFixture(1<<20, 2048, 512, nil))
	report, err := RunSizeMonitor(SizeOptions{Root: dir, BaseRef: baseSHA, PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunSizeMonitor: %v", err)
	}
	if report.Passed {
		t.Fatalf("expected failure: growth exceeds fail threshold")
	}
	found := false
	for _, v := range report.Violations {
		if v.Category == "growth_exceeds_fail_threshold" && v.Severity == SeverityError {
			found = true
		}
	}
	if !found {
		t.Errorf("expected a growth_exceeds_fail_threshold violation, got: %+v", report.Violations)
	}
}

func TestRunSizeMonitor_BaseRefUnresolvedFailsClosed(t *testing.T) {
	dir := initTestRepo(t)
	writeFile(t, dir, "src/a.txt", []byte("baseline"))
	commitAll(t, dir, "only commit")

	policyPath := writePolicy(t, dir, sizePolicyFixture(1<<20, 1<<30, 1<<20, nil))
	report, err := RunSizeMonitor(SizeOptions{Root: dir, BaseRef: "no-such-ref-anywhere", PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunSizeMonitor should return a report, not a hard error, for a missing base ref: %v", err)
	}
	if report.Passed {
		t.Fatalf("expected fail-closed behavior when base ref cannot be resolved")
	}
	metrics := report.Metrics.(*SizeMetrics)
	if metrics.BaseResolved {
		t.Errorf("expected BaseResolved=false")
	}
	if metrics.HeadTotalBytes == 0 {
		t.Errorf("expected HEAD metrics to still be populated even though base failed to resolve")
	}
	foundErr := false
	for _, v := range report.Violations {
		if v.Category == "base_ref_unresolved" && v.Severity == SeverityError {
			foundErr = true
		}
	}
	if !foundErr {
		t.Errorf("expected a base_ref_unresolved error violation, got: %+v", report.Violations)
	}
}
