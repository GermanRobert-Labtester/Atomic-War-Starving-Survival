package monitor

import (
	"os"
	"path/filepath"
	"strings"
	"testing"
)

// TestListTreeBlobs_UsesCommittedBlobSizeNotWorkingTreeSize proves
// ListTreeBlobs reports the size git recorded for the committed blob, not
// whatever happens to be on disk in the working tree right now. This is the
// exact property that makes it safe to treat a Git LFS pointer file as its
// raw (small) git blob byte count instead of hydrating and reading the real
// (potentially huge) LFS payload: the size monitor only ever asks git's
// object store, never the filesystem.
func TestListTreeBlobs_UsesCommittedBlobSizeNotWorkingTreeSize(t *testing.T) {
	dir := initTestRepo(t)
	pointerContent := []byte("version https://git-lfs.github.com/spec/v1\noid sha256:deadbeefdeadbeef\nsize 999999999\n")
	writeFile(t, dir, "pointer.bin", pointerContent)
	sha := commitAll(t, dir, "add lfs-style pointer file")

	// Simulate a smudged/hydrated working-tree file without committing it -
	// this must NOT change what ListTreeBlobs reports for the committed sha.
	hydrated := make([]byte, 5000)
	if err := os.WriteFile(filepath.Join(dir, "pointer.bin"), hydrated, 0o644); err != nil {
		t.Fatalf("simulate hydrated working tree file: %v", err)
	}

	entries, err := ListTreeBlobs(dir, sha)
	if err != nil {
		t.Fatalf("ListTreeBlobs: %v", err)
	}

	var found *TreeEntry
	for i := range entries {
		if entries[i].Path == "pointer.bin" {
			found = &entries[i]
			break
		}
	}
	if found == nil {
		t.Fatalf("pointer.bin not found in tree entries: %+v", entries)
	}
	if found.Size != int64(len(pointerContent)) {
		t.Errorf("expected committed pointer blob size %d, got %d (working-tree size leaked in)", len(pointerContent), found.Size)
	}
}

func TestResolveCommit_UnknownRefFails(t *testing.T) {
	dir := initTestRepo(t)
	commitAll(t, dir, "seed commit")

	if _, err := ResolveCommit(dir, "definitely-not-a-real-ref"); err == nil {
		t.Fatalf("expected error resolving an unknown ref, got nil")
	}

	if _, err := ResolveCommit(dir, ""); err == nil {
		t.Fatalf("expected error resolving an empty ref, got nil")
	}
}

func TestListTrackedFiles_FiltersByPathspec(t *testing.T) {
	dir := initTestRepo(t)
	writeFile(t, dir, "a.cs", []byte("class A {}"))
	writeFile(t, dir, "sub/b.cs", []byte("class B {}"))
	writeFile(t, dir, "readme.md", []byte("# hi"))
	commitAll(t, dir, "seed files")

	files, err := ListTrackedFiles(dir, "*.cs")
	if err != nil {
		t.Fatalf("ListTrackedFiles: %v", err)
	}
	if len(files) != 2 {
		t.Fatalf("expected 2 tracked .cs files, got %d: %v", len(files), files)
	}
	joined := strings.Join(files, ",")
	if !strings.Contains(joined, "a.cs") || !strings.Contains(joined, "sub/b.cs") {
		t.Errorf("unexpected tracked file set: %v", files)
	}
}
