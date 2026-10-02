//! Go `filepath.WalkDir` traversal order.
//!
//! `walkdir` yields entries in raw OS `readdir` order, which is neither sorted
//! nor stable. Go's `filepath.WalkDir` sorts each directory's names and descends
//! immediately, giving a deterministic lexical depth-first order that callers
//! observe (e.g. the order of `scan-saves` results). This module reproduces that
//! order exactly, so walks stay byte-comparable with the Go original.
//!
//! Directory names are compared byte-wise, matching Go's `readDirNames` sort.

use std::ffi::OsStr;
use std::fs;
use std::path::{Path, PathBuf};

/// One visited entry. `is_dir` follows lstat semantics, so a symlink to a
/// directory is not a directory — the same as Go's `DirEntry.IsDir`.
pub struct WalkEntry {
    pub path: PathBuf,
    pub is_dir: bool,
}

/// Walk `root` like Go's `filepath.WalkDir`, calling `descend` with each
/// directory's name: `false` prunes that subtree (`filepath.SkipDir`).
///
/// Unreadable entries are skipped rather than reported; every Go callback in
/// this crate returns `nil` on error, which skips the entry the same way. A
/// missing `root` yields nothing.
pub fn walk_go<F>(root: &Path, mut descend: F) -> Vec<WalkEntry>
where
    F: FnMut(&OsStr) -> bool,
{
    let mut out = Vec::new();
    let meta = match fs::symlink_metadata(root) {
        Ok(m) => m,
        Err(_) => return out,
    };
    let is_dir = meta.is_dir();
    out.push(WalkEntry {
        path: root.to_path_buf(),
        is_dir,
    });
    if is_dir {
        walk_into(root, &mut descend, &mut out);
    }
    out
}

fn walk_into<F>(dir: &Path, descend: &mut F, out: &mut Vec<WalkEntry>)
where
    F: FnMut(&OsStr) -> bool,
{
    let read = match fs::read_dir(dir) {
        Ok(r) => r,
        Err(_) => return,
    };
    let mut entries: Vec<fs::DirEntry> = read.filter_map(|e| e.ok()).collect();
    entries.sort_by_key(|a| a.file_name());

    for entry in entries {
        let name = entry.file_name();
        let is_dir = entry.file_type().map(|t| t.is_dir()).unwrap_or(false);
        if is_dir && !descend(&name) {
            continue;
        }
        let path = entry.path();
        out.push(WalkEntry {
            path: path.clone(),
            is_dir,
        });
        if is_dir {
            walk_into(&path, descend, out);
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    fn tmpdir(tag: &str) -> PathBuf {
        let base = std::env::temp_dir().join(format!(
            "ashfall-rstools-gowalk-{}-{}",
            tag,
            std::process::id()
        ));
        let _ = fs::remove_dir_all(&base);
        fs::create_dir_all(&base).unwrap();
        base
    }

    #[test]
    fn visits_in_lexical_depth_first_order_like_go() {
        let root = tmpdir("order");
        fs::create_dir_all(root.join("src")).unwrap();
        fs::create_dir_all(root.join("assets")).unwrap();
        fs::create_dir_all(root.join("assets").join("ui")).unwrap();
        fs::write(root.join("README.md"), b"x").unwrap();
        fs::write(root.join("src").join("Main.cs"), b"x").unwrap();
        fs::write(root.join("assets").join("ui").join("b.png"), b"x").unwrap();
        fs::write(root.join("assets").join("a.png"), b"x").unwrap();

        let names: Vec<String> = walk_go(&root, |_| true)
            .iter()
            .map(|e| {
                e.path
                    .strip_prefix(&root)
                    .unwrap()
                    .to_string_lossy()
                    .into_owned()
            })
            .collect();
        assert_eq!(
            names,
            vec![
                "",
                "README.md",
                "assets",
                "assets/a.png",
                "assets/ui",
                "assets/ui/b.png",
                "src",
                "src/Main.cs",
            ]
        );
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn descend_false_prunes_the_subtree() {
        let root = tmpdir("prune");
        fs::create_dir_all(root.join("keep")).unwrap();
        fs::create_dir_all(root.join("skip")).unwrap();
        fs::write(root.join("skip").join("hidden.json"), b"{}").unwrap();
        fs::write(root.join("keep").join("kept.json"), b"{}").unwrap();

        let kept: Vec<String> = walk_go(&root, |n| n != "skip")
            .iter()
            .filter(|e| !e.is_dir)
            .map(|e| {
                e.path
                    .strip_prefix(&root)
                    .unwrap()
                    .to_string_lossy()
                    .into_owned()
            })
            .collect();
        assert_eq!(kept, vec!["keep/kept.json"]);
        let _ = fs::remove_dir_all(&root);
    }

    #[test]
    fn missing_root_yields_nothing() {
        let missing = std::env::temp_dir().join("ashfall-rstools-gowalk-absent");
        assert!(walk_go(&missing, |_| true).is_empty());
    }
}