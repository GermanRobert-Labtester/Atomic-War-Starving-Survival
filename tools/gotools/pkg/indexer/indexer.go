package indexer

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"path/filepath"
	"strings"
	"sync"
	"time"

	"github.com/fsnotify/fsnotify"
	"golang.org/x/sync/errgroup"
)

type FileRecord struct {
	Path     string    `json:"path"`
	Size     int64     `json:"size"`
	ModTime  time.Time `json:"mod_time"`
	SHA256   string    `json:"sha256,omitempty"`
	Category string    `json:"category"`
}

type IndexReport struct {
	RootPath     string         `json:"root_path"`
	IndexedAt    time.Time      `json:"indexed_at"`
	TotalFiles   int            `json:"total_files"`
	TotalBytes   int64          `json:"total_bytes"`
	DurationMs   int64          `json:"duration_ms"`
	CategoryStat map[string]int `json:"category_counts"`
	Files        []FileRecord   `json:"files,omitempty"`
}

var defaultIgnoredDirs = map[string]bool{
	".git":          true,
	".godot":        true,
	"bin":           true,
	"obj":           true,
	"node_modules":  true,
	"__pycache__":   true,
	".pytest_cache": true,
	".vscode":       false,
}

func categorizePath(path string) string {
	ext := strings.ToLower(filepath.Ext(path))
	switch ext {
	case ".cs":
		if strings.Contains(path, "Tests") {
			return "csharp_test"
		}
		return "csharp_source"
	case ".json":
		return "json_data"
	case ".yaml", ".yml":
		return "yaml_config"
	case ".py":
		return "python_script"
	case ".sh":
		return "shell_script"
	case ".png", ".jpg", ".jpeg", ".webp", ".svg":
		return "image_asset"
	case ".wav", ".ogg", ".mp3":
		return "audio_asset"
	case ".tscn", ".gd":
		return "godot_resource"
	case ".md":
		return "documentation"
	default:
		return "other"
	}
}

func IndexRepository(root string, computeHashes bool, includeFileList bool) (*IndexReport, error) {
	start := time.Now()
	cleanRoot, err := filepath.Abs(root)
	if err != nil {
		return nil, err
	}

	report := &IndexReport{
		RootPath:     cleanRoot,
		IndexedAt:    time.Now().UTC(),
		CategoryStat: make(map[string]int),
	}

	var mu sync.Mutex
	var files []FileRecord

	type task struct {
		absPath string
		relPath string
		size    int64
		modTime time.Time
	}

	tasks := make(chan task, 1024)
	var g errgroup.Group

	numWorkers := 8
	for i := 0; i < numWorkers; i++ {
		g.Go(func() error {
			for t := range tasks {
				rec := FileRecord{
					Path:     t.relPath,
					Size:     t.size,
					ModTime:  t.modTime,
					Category: categorizePath(t.relPath),
				}

				if computeHashes {
					hash, err := computeSHA256(t.absPath)
					if err == nil {
						rec.SHA256 = hash
					}
				}

				mu.Lock()
				report.TotalFiles++
				report.TotalBytes += t.size
				report.CategoryStat[rec.Category]++
				if includeFileList {
					files = append(files, rec)
				}
				mu.Unlock()
			}
			return nil
		})
	}

	err = filepath.WalkDir(cleanRoot, func(p string, d os.DirEntry, err error) error {
		if err != nil {
			return nil
		}
		if d.IsDir() {
			name := d.Name()
			if defaultIgnoredDirs[name] {
				return filepath.SkipDir
			}
			return nil
		}

		info, err := d.Info()
		if err != nil {
			return nil
		}

		rel, err := filepath.Rel(cleanRoot, p)
		if err != nil {
			rel = p
		}

		tasks <- task{
			absPath: p,
			relPath: filepath.ToSlash(rel),
			size:    info.Size(),
			modTime: info.ModTime(),
		}
		return nil
	})

	close(tasks)
	_ = g.Wait()

	if err != nil {
		return nil, err
	}

	report.DurationMs = time.Since(start).Milliseconds()
	if includeFileList {
		report.Files = files
	}
	return report, nil
}

func computeSHA256(path string) (string, error) {
	f, err := os.Open(path)
	if err != nil {
		return "", err
	}
	defer f.Close()

	h := sha256.New()
	if _, err := io.Copy(h, f); err != nil {
		return "", err
	}
	return hex.EncodeToString(h.Sum(nil)), nil
}

func (r *IndexReport) ToJSON(indent bool) ([]byte, error) {
	if indent {
		return json.MarshalIndent(r, "", "  ")
	}
	return json.Marshal(r)
}

func (r *IndexReport) Summary() string {
	var sb strings.Builder
	sb.WriteString(fmt.Sprintf("Repository Index Summary (%s)\n", r.RootPath))
	sb.WriteString(fmt.Sprintf("Indexed in %d ms | Total Files: %d | Total Size: %.2f MB\n",
		r.DurationMs, r.TotalFiles, float64(r.TotalBytes)/(1024*1024)))
	sb.WriteString("File Distribution:\n")
	for cat, count := range r.CategoryStat {
		sb.WriteString(fmt.Sprintf("  - %-18s: %d\n", cat, count))
	}
	return sb.String()
}

// Resident Indexer Server & File Watcher (keeps low RAM resident index for AI agents)
type ResidentIndexer struct {
	mu           sync.RWMutex
	root         string
	latestReport *IndexReport
	recentEvents []string
	watcher      *fsnotify.Watcher
}

func NewResidentIndexer(root string) (*ResidentIndexer, error) {
	abs, err := filepath.Abs(root)
	if err != nil {
		return nil, err
	}
	rep, err := IndexRepository(abs, false, true)
	if err != nil {
		return nil, err
	}

	watcher, err := fsnotify.NewWatcher()
	if err != nil {
		return nil, err
	}

	idx := &ResidentIndexer{
		root:         abs,
		latestReport: rep,
		watcher:      watcher,
	}

	return idx, nil
}

func (idx *ResidentIndexer) StartWatcher() error {
	// Add root and key subdirectories to watcher
	dirs := []string{"src", "Assets", "Ashfall.Core", "Ashfall.Core.Tests", "tools", "scripts"}
	for _, d := range dirs {
		p := filepath.Join(idx.root, d)
		if _, err := os.Stat(p); err == nil {
			_ = idx.watcher.Add(p)
		}
	}
	_ = idx.watcher.Add(idx.root)

	go func() {
		for {
			select {
			case event, ok := <-idx.watcher.Events:
				if !ok {
					return
				}
				idx.mu.Lock()
				msg := fmt.Sprintf("[%s] %s: %s", time.Now().Format("15:04:05"), event.Op, event.Name)
				idx.recentEvents = append(idx.recentEvents, msg)
				if len(idx.recentEvents) > 100 {
					idx.recentEvents = idx.recentEvents[1:]
				}
				idx.mu.Unlock()
			case err, ok := <-idx.watcher.Errors:
				if !ok {
					return
				}
				log.Printf("[resident-indexer watcher error]: %v", err)
			}
		}
	}()
	return nil
}

func (idx *ResidentIndexer) StartHTTP(addr string) error {
	mux := http.NewServeMux()

	mux.HandleFunc("/api/index", func(w http.ResponseWriter, r *http.Request) {
		idx.mu.RLock()
		data, _ := json.Marshal(idx.latestReport)
		idx.mu.RUnlock()
		w.Header().Set("Content-Type", "application/json")
		w.Write(data)
	})

	mux.HandleFunc("/api/search", func(w http.ResponseWriter, r *http.Request) {
		q := strings.ToLower(r.URL.Query().Get("q"))
		var matches []FileRecord
		idx.mu.RLock()
		for _, f := range idx.latestReport.Files {
			if strings.Contains(strings.ToLower(f.Path), q) {
				matches = append(matches, f)
			}
		}
		idx.mu.RUnlock()

		w.Header().Set("Content-Type", "application/json")
		_ = json.NewEncoder(w).Encode(matches)
	})

	mux.HandleFunc("/api/changes", func(w http.ResponseWriter, r *http.Request) {
		idx.mu.RLock()
		events := append([]string(nil), idx.recentEvents...)
		idx.mu.RUnlock()
		w.Header().Set("Content-Type", "application/json")
		_ = json.NewEncoder(w).Encode(events)
	})

	log.Printf("[Resident-Indexer] API listening at http://%s (Index cached: %d files)", addr, idx.latestReport.TotalFiles)
	return http.ListenAndServe(addr, mux)
}
