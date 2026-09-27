// Package monitor implements the repository-size/growth monitor
// (monitor-size) and the compile-set / test-only-production-source monitor
// (monitor-compile) for ASHFALL. Both monitors are read-only: they inspect
// git plumbing and MSBuild item evaluation and never mutate the worktree.
package monitor

import (
	"bytes"
	"fmt"
	"os/exec"
	"path/filepath"
	"strconv"
	"strings"
)

// runGit executes git in dir and returns trimmed stdout. stderr is folded
// into the returned error so callers get an actionable message instead of a
// bare exit-status failure.
func runGit(dir string, args ...string) (string, error) {
	cmd := exec.Command("git", args...)
	cmd.Dir = dir
	var stdout, stderr bytes.Buffer
	cmd.Stdout = &stdout
	cmd.Stderr = &stderr
	if err := cmd.Run(); err != nil {
		return "", fmt.Errorf("git %s: %w: %s", strings.Join(args, " "), err, strings.TrimSpace(stderr.String()))
	}
	return strings.TrimSpace(stdout.String()), nil
}

// ResolveCommit resolves ref to a full commit SHA. It fails (non-nil error)
// for any ref git cannot resolve to a commit, including a missing/unknown
// base ref — callers must treat that as fail-closed, not as "no base to
// compare against".
func ResolveCommit(root, ref string) (string, error) {
	if strings.TrimSpace(ref) == "" {
		return "", fmt.Errorf("empty git ref")
	}
	sha, err := runGit(root, "rev-parse", "--verify", ref+"^{commit}")
	if err != nil {
		return "", fmt.Errorf("resolve ref %q: %w", ref, err)
	}
	return sha, nil
}

// TreeEntry is one blob recorded in a git tree at a given ref.
type TreeEntry struct {
	Path string
	Size int64
}

// ListTreeBlobs returns every blob (file) reachable from ref with its raw
// git object size, as reported by `git ls-tree -r -z -l`. This is
// deliberately plumbing-only: for a Git LFS pointer file the blob IS the
// small pointer text, so the reported size is the pointer's byte count, not
// the real large-file payload. That is the intended behavior — the monitor
// must never hydrate or read LFS payload content to compute size.
func ListTreeBlobs(root, ref string) ([]TreeEntry, error) {
	out, err := runGit(root, "ls-tree", "-r", "-z", "-l", ref)
	if err != nil {
		return nil, fmt.Errorf("list tree at %q: %w", ref, err)
	}

	var entries []TreeEntry
	records := strings.Split(out, "\x00")
	for _, rec := range records {
		if rec == "" {
			continue
		}
		tabIdx := strings.IndexByte(rec, '\t')
		if tabIdx < 0 {
			continue
		}
		meta := rec[:tabIdx]
		path := rec[tabIdx+1:]
		fields := strings.Fields(meta)
		if len(fields) < 4 {
			continue
		}
		// fields: mode type sha size (size is "-" for non-blob entries, e.g.
		// submodule commits; trees are not emitted at all with -r).
		typ := fields[1]
		sizeStr := fields[3]
		if typ != "blob" {
			continue
		}
		size, convErr := strconv.ParseInt(sizeStr, 10, 64)
		if convErr != nil {
			continue
		}
		entries = append(entries, TreeEntry{Path: path, Size: size})
	}
	return entries, nil
}

// ListTrackedFiles returns git-tracked paths under root matching the given
// pathspec (e.g. "*.cs"), relative to root, using forward slashes.
func ListTrackedFiles(root string, pathspec string) ([]string, error) {
	cmd := exec.Command("git", "ls-files", "-z", "--", pathspec)
	cmd.Dir = root
	var stdout, stderr bytes.Buffer
	cmd.Stdout = &stdout
	cmd.Stderr = &stderr
	if err := cmd.Run(); err != nil {
		return nil, fmt.Errorf("git ls-files -- %s: %w: %s", pathspec, err, strings.TrimSpace(stderr.String()))
	}
	var files []string
	for _, p := range strings.Split(stdout.String(), "\x00") {
		if p == "" {
			continue
		}
		files = append(files, filepath.ToSlash(p))
	}
	return files, nil
}

// HeadCommit resolves HEAD to a full commit SHA.
func HeadCommit(root string) (string, error) {
	return ResolveCommit(root, "HEAD")
}
