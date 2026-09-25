package checkplan

import (
	"bufio"
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
)

const ApprovedStatusMarker = "STATUS: APPROVED BY USER"

// IsCodeFile determines whether a changed path requires an approved plan.
func IsCodeFile(path string) bool {
	norm := filepath.ToSlash(path)

	// Ignore plans, state files, docs, git metadata, and CI configs
	if strings.HasPrefix(norm, ".ai/") ||
		strings.HasPrefix(norm, "docs/") ||
		strings.HasPrefix(norm, ".github/") ||
		strings.HasPrefix(norm, ".git/") ||
		strings.HasSuffix(norm, ".md") ||
		strings.HasSuffix(norm, ".txt") ||
		strings.HasSuffix(norm, ".png") ||
		strings.HasSuffix(norm, ".jpg") ||
		strings.HasSuffix(norm, ".svg") ||
		strings.HasSuffix(norm, ".import") ||
		strings.HasSuffix(norm, ".gitignore") ||
		strings.HasSuffix(norm, ".gitattributes") {
		return false
	}

	// Code and authoritative game data
	ext := strings.ToLower(filepath.Ext(norm))
	switch ext {
	case ".cs", ".gd", ".py", ".go", ".tscn", ".godot", ".shader", ".json":
		return true
	}

	return false
}

func isApprovedPlan(path string, content string) bool {
	if strings.EqualFold(filepath.Base(path), "template.md") {
		return false
	}
	scanner := bufio.NewScanner(strings.NewReader(content))
	for scanner.Scan() {
		line := strings.TrimSpace(scanner.Text())
		line = strings.Trim(line, "*_`# ")
		if strings.EqualFold(line, "STATUS: APPROVED BY USER") {
			return true
		}
	}
	return false
}

// CheckApprovedPlan verifies that if any code files have changed, an approved plan exists in .ai/plans/ or .ai/plan.md.
func CheckApprovedPlan(repoRoot string, stagedOnly bool, compareRef string, explicitFiles []string) (bool, string, error) {
	var files []string

	if len(explicitFiles) > 0 {
		files = explicitFiles
	} else if stagedOnly {
		cmd := exec.Command("git", "diff", "--cached", "--name-only")
		cmd.Dir = repoRoot
		out, err := cmd.Output()
		if err != nil {
			return false, "", fmt.Errorf("git diff --cached failed: %w", err)
		}
		scanner := bufio.NewScanner(strings.NewReader(string(out)))
		for scanner.Scan() {
			t := strings.TrimSpace(scanner.Text())
			if t != "" {
				files = append(files, t)
			}
		}
	} else if compareRef != "" {
		cmd := exec.Command("git", "diff", "--name-only", compareRef)
		cmd.Dir = repoRoot
		out, err := cmd.Output()
		if err != nil {
			return false, "", fmt.Errorf("git diff failed: %w", err)
		}
		scanner := bufio.NewScanner(strings.NewReader(string(out)))
		for scanner.Scan() {
			t := strings.TrimSpace(scanner.Text())
			if t != "" {
				files = append(files, t)
			}
		}
	} else {
		cmd := exec.Command("git", "status", "--porcelain")
		cmd.Dir = repoRoot
		out, err := cmd.Output()
		if err != nil {
			return false, "", fmt.Errorf("git status failed: %w", err)
		}
		scanner := bufio.NewScanner(strings.NewReader(string(out)))
		for scanner.Scan() {
			line := strings.TrimSpace(scanner.Text())
			if len(line) >= 4 {
				path := strings.TrimSpace(line[3:])
				if idx := strings.Index(path, "->"); idx != -1 {
					path = strings.TrimSpace(path[idx+2:])
				}
				path = strings.Trim(path, "\"")
				if path != "" {
					files = append(files, path)
				}
			}
		}
	}

	// Filter for code changes
	var codeFiles []string
	for _, f := range files {
		if IsCodeFile(f) {
			codeFiles = append(codeFiles, f)
		}
	}

	if len(codeFiles) == 0 {
		return true, "No code files changed; plan check skipped.", nil
	}

	// Search for approved plans
	approvedPlans := []string{}

	// Check .ai/plans/ directory
	plansDir := filepath.Join(repoRoot, ".ai", "plans")
	if entries, err := os.ReadDir(plansDir); err == nil {
		for _, e := range entries {
			if !e.IsDir() && strings.HasSuffix(e.Name(), ".md") && !strings.EqualFold(e.Name(), "template.md") {
				planPath := filepath.Join(plansDir, e.Name())
				content, err := os.ReadFile(planPath)
				if err == nil && isApprovedPlan(planPath, string(content)) {
					approvedPlans = append(approvedPlans, filepath.Join(".ai", "plans", e.Name()))
				}
			}
		}
	}

	// Check .ai/plan.md
	rootPlan := filepath.Join(repoRoot, ".ai", "plan.md")
	if content, err := os.ReadFile(rootPlan); err == nil {
		if isApprovedPlan(rootPlan, string(content)) {
			approvedPlans = append(approvedPlans, ".ai/plan.md")
		}
	}

	if len(approvedPlans) == 0 {
		msg := fmt.Sprintf("No approved plan found for changed files (%d code file(s) changed: %s). Create/update a plan in .ai/plans/ and set STATUS: APPROVED BY USER.",
			len(codeFiles), strings.Join(codeFiles, ", "))
		return false, msg, nil
	}

	return true, fmt.Sprintf("Approved plan(s) verified: %s", strings.Join(approvedPlans, ", ")), nil
}
