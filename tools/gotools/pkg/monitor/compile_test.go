package monitor

import (
	"os"
	"path/filepath"
	"strings"
	"testing"
)

const fixtureProdCsproj = `<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="prod/**/*.cs" />
  </ItemGroup>
</Project>
`

const fixtureTestsCsproj = `<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="../legacy/Legacy.cs" Link="Legacy.cs" />
  </ItemGroup>
</Project>
`

const fixtureToolCsproj = `<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Tool.cs" />
  </ItemGroup>
</Project>
`

// buildCompileFixture lays out one production project, one test project
// (SDK default compile items, plus one explicit Compile Include for a file
// outside its own tree - mirroring Ashfall.Core.Tests.csproj's real
// NoiseDisciplineSystem.cs Link), one standalone tool project discoverable
// without being named in policy, and one genuinely uncompiled stray file.
func buildCompileFixture(t *testing.T) string {
	t.Helper()
	dir := initTestRepo(t)

	writeFile(t, dir, "Prod.csproj", []byte(fixtureProdCsproj))
	writeFile(t, dir, "prod/Widget.cs", []byte("namespace Fixture.Prod { public class Widget {} }\n"))

	writeFile(t, dir, "Tests/Tests.csproj", []byte(fixtureTestsCsproj))
	writeFile(t, dir, "Tests/WidgetTests.cs", []byte("namespace Fixture.Tests { public class WidgetTests {} }\n"))

	// Compiled only by Tests.csproj, outside the Tests/ root: the
	// NoiseDisciplineSystem.cs-shaped debt case.
	writeFile(t, dir, "legacy/Legacy.cs", []byte("namespace Fixture.Legacy { public class Legacy {} }\n"))

	// Not compiled by anything: the BunkerSocialSystems.cs-shaped case.
	writeFile(t, dir, "stray/Stray.cs", []byte("namespace Fixture.Stray { public class Stray {} }\n"))

	writeFile(t, dir, "tools/Tool.csproj", []byte(fixtureToolCsproj))
	writeFile(t, dir, "tools/Tool.cs", []byte("namespace Fixture.Tool { public class Tool {} }\n"))

	commitAll(t, dir, "fixture: initial compile-set layout")
	return dir
}

func compilePolicyFixture(knownPaths []KnownCompilePath) Policy {
	return Policy{
		SchemaVersion: "1.0",
		Size: SizePolicy{
			NewMarkdownMaxBytes: 1 << 20,
			PRGrowthFailBytes:   1 << 30,
			PRGrowthWarnBytes:   1 << 20,
		},
		Compile: CompilePolicy{
			ProductionProjects: []string{"Prod.csproj"},
			TestProjects:       []string{"Tests/Tests.csproj"},
			TestRootPrefix:     "Tests/",
			KnownPaths:         knownPaths,
		},
	}
}

func TestRunCompileMonitor_ClassifiesAllBucketsAndFailsOnUnknownDebt(t *testing.T) {
	dir := buildCompileFixture(t)
	policyPath := writePolicy(t, dir, compilePolicyFixture(nil))

	report, err := RunCompileMonitor(CompileOptions{Root: dir, PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunCompileMonitor: %v", err)
	}

	if report.Passed {
		t.Fatalf("expected failure: stray/Stray.cs and legacy/Legacy.cs are unclassified debt, got passed=true")
	}

	metrics := report.Metrics.(*CompileMetrics)
	if metrics.ProductionCount != 1 {
		t.Errorf("expected production_count=1, got %d", metrics.ProductionCount)
	}
	if metrics.TestCount != 1 {
		t.Errorf("expected test_count=1, got %d", metrics.TestCount)
	}
	if metrics.StandaloneToolCount != 1 {
		t.Errorf("expected standalone_tool_count=1, got %d", metrics.StandaloneToolCount)
	}
	if metrics.NewUnclassifiedCount != 2 {
		t.Errorf("expected new_unclassified_count=2, got %d", metrics.NewUnclassifiedCount)
	}
	if metrics.AcceptedDebtCount != 0 {
		t.Errorf("expected accepted_debt_count=0, got %d", metrics.AcceptedDebtCount)
	}
	if len(metrics.StandaloneProjectsDiscovered) != 1 || metrics.StandaloneProjectsDiscovered[0] != "tools/Tool.csproj" {
		t.Errorf("expected tools/Tool.csproj to be auto-discovered, got %v", metrics.StandaloneProjectsDiscovered)
	}

	by := violationsByPath(report.Violations)

	strayV, ok := by["stray/Stray.cs"]
	if !ok || strayV.Severity != SeverityError || strayV.Category != "unclassified_uncompiled_source" {
		t.Errorf("expected error unclassified_uncompiled_source for stray/Stray.cs, got %+v (ok=%v)", strayV, ok)
	}

	legacyV, ok := by["legacy/Legacy.cs"]
	if !ok || legacyV.Severity != SeverityError || legacyV.Category != "new_test_only_production_source" {
		t.Errorf("expected error new_test_only_production_source for legacy/Legacy.cs, got %+v (ok=%v)", legacyV, ok)
	}
}

func TestRunCompileMonitor_AllowlistedDebtPassesButStaysVisible(t *testing.T) {
	dir := buildCompileFixture(t)
	knownPaths := []KnownCompilePath{
		{
			Path:           "legacy/Legacy.cs",
			Classification: "test_only_production_debt",
			Severity:       SeverityInfo,
			Reason:         "fixture: accepted debt mirroring NoiseDisciplineSystem.cs",
		},
		{
			Path:           "stray/Stray.cs",
			Classification: "intentional_reference_doc",
			Severity:       SeverityInfo,
			Reason:         "fixture: intentionally uncompiled reference source",
		},
		{
			Path:           "ghost/DoesNotExist.cs",
			Classification: "whatever",
			Severity:       SeverityInfo,
			Reason:         "fixture: stale entry that no longer matches any tracked file",
		},
	}
	policyPath := writePolicy(t, dir, compilePolicyFixture(knownPaths))

	report, err := RunCompileMonitor(CompileOptions{Root: dir, PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunCompileMonitor: %v", err)
	}
	if !report.Passed {
		t.Fatalf("expected pass once both debts are explicitly allowlisted, got: %+v", report.Violations)
	}

	metrics := report.Metrics.(*CompileMetrics)
	if metrics.AcceptedDebtCount != 2 {
		t.Errorf("expected accepted_debt_count=2, got %d", metrics.AcceptedDebtCount)
	}
	if metrics.NewUnclassifiedCount != 0 {
		t.Errorf("expected new_unclassified_count=0, got %d", metrics.NewUnclassifiedCount)
	}

	by := violationsByPath(report.Violations)

	// Accepted debt must remain visible in the report, not silently
	// swallowed just because it no longer fails the gate.
	legacyV, ok := by["legacy/Legacy.cs"]
	if !ok || legacyV.Severity != SeverityInfo || legacyV.Category != "test_only_production_debt" {
		t.Errorf("expected visible info test_only_production_debt for legacy/Legacy.cs, got %+v (ok=%v)", legacyV, ok)
	}
	strayV, ok := by["stray/Stray.cs"]
	if !ok || strayV.Severity != SeverityInfo || strayV.Category != "intentional_reference_doc" {
		t.Errorf("expected visible info intentional_reference_doc for stray/Stray.cs, got %+v (ok=%v)", strayV, ok)
	}

	// The stale allowlist entry must produce a non-fatal warning, not be
	// silently ignored.
	ghostV, ok := by["ghost/DoesNotExist.cs"]
	if !ok || ghostV.Severity != SeverityWarning || ghostV.Category != "stale_policy_allowlist_entry" {
		t.Errorf("expected warning stale_policy_allowlist_entry for ghost/DoesNotExist.cs, got %+v (ok=%v)", ghostV, ok)
	}
}

func TestRunCompileMonitor_MissingProjectReturnsError(t *testing.T) {
	dir := buildCompileFixture(t)

	if _, err := GetCompileItems(dir, "DoesNotExist.csproj"); err == nil {
		t.Fatalf("expected GetCompileItems to error on a missing project file")
	} else if !strings.Contains(err.Error(), "not found") {
		t.Errorf("expected a 'not found' error, got: %v", err)
	}

	policy := compilePolicyFixture(nil)
	policy.Compile.ProductionProjects = []string{"DoesNotExist.csproj"}
	policyPath := writePolicy(t, dir, policy)

	if _, err := RunCompileMonitor(CompileOptions{Root: dir, PolicyPath: policyPath}); err == nil {
		t.Fatalf("expected RunCompileMonitor to surface the MSBuild evaluation failure as a hard error")
	}
}

func TestRunCompileMonitor_TrackedSourceDeletedLocallyFailsAsCheckoutMismatch(t *testing.T) {
	dir := buildCompileFixture(t)
	policyPath := writePolicy(t, dir, compilePolicyFixture([]KnownCompilePath{
		{Path: "legacy/Legacy.cs", Classification: "test_only_production_debt", Severity: SeverityInfo, Reason: "fixture: accepted test-only debt"},
		{Path: "stray/Stray.cs", Classification: "intentional_reference_doc", Severity: SeverityInfo, Reason: "fixture: reference source"},
	}))
	if err := os.Remove(filepath.Join(dir, "prod", "Widget.cs")); err != nil {
		t.Fatal(err)
	}

	report, err := RunCompileMonitor(CompileOptions{Root: dir, PolicyPath: policyPath})
	if err != nil {
		t.Fatalf("RunCompileMonitor: %v", err)
	}
	metrics := report.Metrics.(*CompileMetrics)
	if report.Passed || metrics.MissingWorkingTreeCount != 1 || metrics.NewUnclassifiedCount != 0 {
		t.Fatalf("expected fail-closed checkout mismatch without invented compile debt, got metrics=%+v violations=%+v", metrics, report.Violations)
	}
	if v := violationsByPath(report.Violations)["prod/Widget.cs"]; v.Category != "tracked_source_missing_from_worktree" || v.Severity != SeverityError {
		t.Fatalf("expected missing tracked source error, got %+v", v)
	}
}
