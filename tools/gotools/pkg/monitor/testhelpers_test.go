package monitor

import (
	"encoding/json"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
	"testing"
)

// initTestRepo creates a throwaway git repository under a path segment that
// contains a space (mirroring this project's own "Atomic War" repo root) so
// every test in this package exercises path normalization for spaces, not
// just a single dedicated case. The repo and its identity are local to the
// temp directory and are discarded with it; they never touch the real
// project's git config.
func initTestRepo(t *testing.T) string {
	t.Helper()
	dir := filepath.Join(t.TempDir(), "repo with spaces")
	if err := os.MkdirAll(dir, 0o755); err != nil {
		t.Fatalf("mkdir %s: %v", dir, err)
	}
	runGitT(t, dir, "init", "-q")
	runGitT(t, dir, "config", "user.email", "monitor-test@example.invalid")
	runGitT(t, dir, "config", "user.name", "Monitor Test")
	return dir
}

func runGitT(t *testing.T, dir string, args ...string) string {
	t.Helper()
	cmd := exec.Command("git", args...)
	cmd.Dir = dir
	out, err := cmd.CombinedOutput()
	if err != nil {
		t.Fatalf("git %v (dir=%s) failed: %v\n%s", args, dir, err, out)
	}
	return strings.TrimSpace(string(out))
}

func writeFile(t *testing.T, dir, rel string, content []byte) {
	t.Helper()
	full := filepath.Join(dir, rel)
	if err := os.MkdirAll(filepath.Dir(full), 0o755); err != nil {
		t.Fatalf("mkdir %s: %v", filepath.Dir(full), err)
	}
	if err := os.WriteFile(full, content, 0o644); err != nil {
		t.Fatalf("write %s: %v", full, err)
	}
}

func commitAll(t *testing.T, dir, message string) string {
	t.Helper()
	runGitT(t, dir, "add", "-A")
	runGitT(t, dir, "commit", "-q", "--allow-empty", "-m", message)
	return runGitT(t, dir, "rev-parse", "HEAD")
}

// writePolicy marshals p to disk and returns its absolute path.
func writePolicy(t *testing.T, dir string, p Policy) string {
	t.Helper()
	data, err := json.MarshalIndent(p, "", "  ")
	if err != nil {
		t.Fatalf("marshal policy: %v", err)
	}
	path := filepath.Join(dir, "policy.json")
	if err := os.WriteFile(path, data, 0o644); err != nil {
		t.Fatalf("write policy: %v", err)
	}
	return path
}

// violationsByCategory indexes violations for convenient lookup in
// assertions without depending on slice order.
func violationsByPath(vs []Violation) map[string]Violation {
	m := make(map[string]Violation, len(vs))
	for _, v := range vs {
		if v.Path != "" {
			m[v.Path] = v
		}
	}
	return m
}
