package monitor

import (
	"bytes"
	"encoding/json"
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"sort"
	"strings"
)

// CompileOptions configures RunCompileMonitor.
type CompileOptions struct {
	Root       string // absolute repository root
	PolicyPath string // path to docs/ci/MONITORING_POLICY.json
}

// CompileMetrics is the "metrics" payload for the monitor-compile report.
type CompileMetrics struct {
	TrackedCSFiles               int      `json:"tracked_cs_files"`
	ProductionCount              int      `json:"production_count"`
	TestCount                    int      `json:"test_count"`
	StandaloneToolCount          int      `json:"standalone_tool_count"`
	AcceptedDebtCount            int      `json:"accepted_debt_count"`
	NewUnclassifiedCount         int      `json:"new_unclassified_count"`
	MissingWorkingTreeCount      int      `json:"missing_working_tree_count"`
	ProjectsEvaluated            []string `json:"projects_evaluated"`
	StandaloneProjectsDiscovered []string `json:"standalone_projects_discovered"`
}

type msbuildCompileItem struct {
	FullPath string `json:"FullPath"`
}

type msbuildGetItemResult struct {
	Items struct {
		Compile []msbuildCompileItem `json:"Compile"`
	} `json:"Items"`
}

// GetCompileItems runs `dotnet msbuild <csprojRelPath> -getItem:Compile -nologo`
// against the project at root/csprojRelPath and returns the evaluated
// Compile item FullPaths converted to root-relative, forward-slash paths.
// This is the actual MSBuild-evaluated compile set (globs, explicit
// Includes/Removes, Link items, and Exclude patterns all already applied),
// not a re-implementation of MSBuild glob semantics.
func GetCompileItems(root, csprojRelPath string) ([]string, error) {
	absProj := filepath.Join(root, csprojRelPath)
	if _, err := os.Stat(absProj); err != nil {
		return nil, fmt.Errorf("project %s not found at %s: %w", csprojRelPath, absProj, err)
	}

	cmd := exec.Command("dotnet", "msbuild", absProj, "-getItem:Compile", "-nologo")
	cmd.Dir = root
	var stdout, stderr bytes.Buffer
	cmd.Stdout = &stdout
	cmd.Stderr = &stderr
	if err := cmd.Run(); err != nil {
		return nil, fmt.Errorf("dotnet msbuild %s -getItem:Compile: %w: %s", csprojRelPath, err, strings.TrimSpace(stderr.String()))
	}

	var result msbuildGetItemResult
	if err := json.Unmarshal(stdout.Bytes(), &result); err != nil {
		return nil, fmt.Errorf("parse msbuild -getItem:Compile JSON for %s: %w", csprojRelPath, err)
	}

	var out []string
	for _, item := range result.Items.Compile {
		if item.FullPath == "" {
			continue
		}
		rel, relErr := filepath.Rel(root, item.FullPath)
		if relErr != nil {
			continue
		}
		rel = filepath.ToSlash(rel)
		if strings.HasPrefix(rel, "../") {
			// Outside the repo root (e.g. an SDK-provided generated file);
			// it cannot be a git-tracked source path, so it is out of scope.
			continue
		}
		out = append(out, rel)
	}
	return out, nil
}

// discoverStandaloneProjects finds every .csproj under root that is not in
// the exclude set (the known production/test projects), skipping bin/obj
// and VCS directories. This picks up standalone dev-tool projects such as
// tools/ui-preview.csproj without needing to hardcode their paths.
func discoverStandaloneProjects(root string, exclude map[string]bool) ([]string, error) {
	var found []string
	err := filepath.WalkDir(root, func(p string, d os.DirEntry, err error) error {
		if err != nil {
			return nil
		}
		if d.IsDir() {
			switch d.Name() {
			case "bin", "obj", ".git", "node_modules", ".godot":
				return filepath.SkipDir
			}
			return nil
		}
		if !strings.HasSuffix(d.Name(), ".csproj") {
			return nil
		}
		rel, relErr := filepath.Rel(root, p)
		if relErr != nil {
			return nil
		}
		rel = filepath.ToSlash(rel)
		if exclude[rel] {
			return nil
		}
		found = append(found, rel)
		return nil
	})
	if err != nil {
		return nil, fmt.Errorf("walk %s for standalone csproj files: %w", root, err)
	}
	sort.Strings(found)
	return found, nil
}

// RunCompileMonitor evaluates the actual MSBuild Compile-item sets for the
// production and test projects named in docs/ci/MONITORING_POLICY.json,
// discovers any other standalone .csproj files, intersects all of that
// against git-tracked .cs files, and classifies every tracked path. A
// tracked .cs file that is not compiled by a production project, not
// compiled by a test project under its normal test root, not compiled by a
// discovered standalone project, and not covered by an explicit
// known_paths allowlist entry is a NEW violation and fails the gate — see
// docs/ci/MONITORING_POLICY.json for the current exact-path exceptions and
// their written reasons.
func RunCompileMonitor(opts CompileOptions) (*Report, error) {
	policy, err := LoadPolicy(opts.PolicyPath)
	if err != nil {
		return nil, err
	}

	headSHA, err := HeadCommit(opts.Root)
	if err != nil {
		return nil, fmt.Errorf("resolve HEAD: %w", err)
	}

	var evaluated []string

	prodSet := map[string]bool{}
	for _, proj := range policy.Compile.ProductionProjects {
		items, err := GetCompileItems(opts.Root, proj)
		if err != nil {
			return nil, fmt.Errorf("evaluate production project %s: %w", proj, err)
		}
		evaluated = append(evaluated, proj)
		for _, it := range items {
			prodSet[it] = true
		}
	}

	testSet := map[string]bool{}
	for _, proj := range policy.Compile.TestProjects {
		items, err := GetCompileItems(opts.Root, proj)
		if err != nil {
			return nil, fmt.Errorf("evaluate test project %s: %w", proj, err)
		}
		evaluated = append(evaluated, proj)
		for _, it := range items {
			testSet[it] = true
		}
	}

	exclude := map[string]bool{}
	for _, p := range policy.Compile.ProductionProjects {
		exclude[filepath.ToSlash(p)] = true
	}
	for _, p := range policy.Compile.TestProjects {
		exclude[filepath.ToSlash(p)] = true
	}

	standaloneProjects, err := discoverStandaloneProjects(opts.Root, exclude)
	if err != nil {
		return nil, err
	}

	standaloneCompiled := map[string][]string{}
	for _, proj := range standaloneProjects {
		items, err := GetCompileItems(opts.Root, proj)
		if err != nil {
			return nil, fmt.Errorf("evaluate standalone project %s: %w", proj, err)
		}
		for _, it := range items {
			standaloneCompiled[it] = append(standaloneCompiled[it], proj)
		}
	}

	trackedCS, err := ListTrackedFiles(opts.Root, "*.cs")
	if err != nil {
		return nil, fmt.Errorf("list tracked .cs files: %w", err)
	}

	metrics := &CompileMetrics{
		TrackedCSFiles:               len(trackedCS),
		ProjectsEvaluated:            evaluated,
		StandaloneProjectsDiscovered: standaloneProjects,
	}

	var violations []Violation
	seenKnown := map[string]bool{}

	for _, path := range trackedCS {
		if _, err := os.Stat(filepath.Join(opts.Root, filepath.FromSlash(path))); err != nil {
			if !os.IsNotExist(err) {
				return nil, fmt.Errorf("stat tracked source %s: %w", path, err)
			}
			metrics.MissingWorkingTreeCount++
			violations = append(violations, Violation{
				Severity:   SeverityError,
				Confidence: 1.0,
				Category:   "tracked_source_missing_from_worktree",
				Path:       path,
				Message:    "tracked C# source is missing from the working tree while still present in the index; the evaluated compile set cannot classify it reliably. Restore or commit the deletion, then rerun on a coherent checkout",
			})
			continue
		}
		inProd := prodSet[path]
		inTest := testSet[path]

		switch {
		case inProd && inTest:
			metrics.ProductionCount++
			violations = append(violations, Violation{
				Severity:   SeverityInfo,
				Confidence: 0.8,
				Category:   "shared_production_and_test",
				Path:       path,
				Message:    "compiled by both a production project and a test project Compile item; verify this is intentional",
			})

		case inProd:
			metrics.ProductionCount++

		case inTest:
			if strings.HasPrefix(path, policy.Compile.TestRootPrefix) {
				metrics.TestCount++
				continue
			}
			// Compiled only by the test project, and outside its normal
			// test root: production-like source with no production owner.
			if known, ok := policy.Compile.KnownPath(path); ok {
				seenKnown[path] = true
				metrics.AcceptedDebtCount++
				violations = append(violations, Violation{
					Severity:   known.Severity,
					Confidence: 1.0,
					Category:   known.Classification,
					Path:       path,
					Message:    known.Reason,
				})
				continue
			}
			metrics.NewUnclassifiedCount++
			violations = append(violations, Violation{
				Severity:   SeverityError,
				Confidence: 1.0,
				Category:   "new_test_only_production_source",
				Path:       path,
				Message: fmt.Sprintf(
					"newly compiled only by a test project (%s) outside its normal test root (%s); this is production-like source with no production owner. Add an explicit docs/ci/MONITORING_POLICY.json compile.known_paths entry with a written reason if this is intentional accepted debt, or give it a production compile owner.",
					strings.Join(policy.Compile.TestProjects, ", "), policy.Compile.TestRootPrefix),
			})

		default:
			if projs, ok := standaloneCompiled[path]; ok {
				metrics.StandaloneToolCount++
				violations = append(violations, Violation{
					Severity:   SeverityInfo,
					Confidence: 1.0,
					Category:   "standalone_tool_compiled",
					Path:       path,
					Message:    fmt.Sprintf("compiled by standalone project(s): %s", strings.Join(projs, ", ")),
				})
				continue
			}
			if known, ok := policy.Compile.KnownPath(path); ok {
				seenKnown[path] = true
				metrics.AcceptedDebtCount++
				violations = append(violations, Violation{
					Severity:   known.Severity,
					Confidence: 1.0,
					Category:   known.Classification,
					Path:       path,
					Message:    known.Reason,
				})
				continue
			}
			metrics.NewUnclassifiedCount++
			violations = append(violations, Violation{
				Severity:   SeverityError,
				Confidence: 1.0,
				Category:   "unclassified_uncompiled_source",
				Path:       path,
				Message:    "tracked .cs file is not compiled by any production, test, or discovered standalone project, and has no docs/ci/MONITORING_POLICY.json compile.known_paths entry explaining why. Add a compile item or an explicit allowlist entry with a written reason.",
			})
		}
	}

	// Hygiene: an allowlist entry that no longer matches any tracked file we
	// actually classified this way means the file moved, was deleted, or is
	// now compiled normally. Flag it so the policy file does not silently
	// drift from reality.
	for _, k := range policy.Compile.KnownPaths {
		if seenKnown[k.Path] {
			continue
		}
		violations = append(violations, Violation{
			Severity:   SeverityWarning,
			Confidence: 0.9,
			Category:   "stale_policy_allowlist_entry",
			Path:       k.Path,
			Message:    "docs/ci/MONITORING_POLICY.json compile.known_paths entry no longer matches a tracked file classified in this bucket (removed, renamed, or now compiled normally); remove or update the entry",
		})
	}

	return NewReport("monitor-compile", headSHA, metrics, violations), nil
}
