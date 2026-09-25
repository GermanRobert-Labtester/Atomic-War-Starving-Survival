package orchestrator

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"runtime"
	"sync"
	"time"

	"ashfall/gotools/pkg/runner"
)

type AgentTaskStatus string

const (
	StatusQueued  AgentTaskStatus = "queued"
	StatusRunning AgentTaskStatus = "running"
	StatusDone    AgentTaskStatus = "done"
	StatusFailed  AgentTaskStatus = "failed"
)

type AgentTaskRecord struct {
	ID          string          `json:"id"`
	AgentID     string          `json:"agent_id"`
	Command     string          `json:"command"`
	Status      AgentTaskStatus `json:"status"`
	CreatedAt   time.Time       `json:"created_at"`
	CompletedAt *time.Time      `json:"completed_at,omitempty"`
	DurationMs  int64           `json:"duration_ms"`
	PeakRSSKb   int64           `json:"peak_rss_kb"`
	Output      string          `json:"output,omitempty"`
	Error       string          `json:"error,omitempty"`
}

type AgentCoreServer struct {
	ListenAddr string
	repoRoot   string
	mu         sync.RWMutex
	tasks      map[string]*AgentTaskRecord
	server     *http.Server
	pool       *runner.TaskRunnerPool
}

func NewAgentCoreServer(addr string, repoRoot string) *AgentCoreServer {
	if addr == "" {
		addr = "127.0.0.1:8082"
	}
	return &AgentCoreServer{
		ListenAddr: addr,
		repoRoot:   repoRoot,
		tasks:      make(map[string]*AgentTaskRecord),
		pool:       runner.NewTaskRunnerPool(4),
	}
}

func (s *AgentCoreServer) Start() error {
	mux := http.NewServeMux()

	mux.HandleFunc("/healthz", func(w http.ResponseWriter, r *http.Request) {
		var m runtime.MemStats
		runtime.ReadMemStats(&m)
		w.Header().Set("Content-Type", "application/json")
		fmt.Fprintf(w, `{"status":"ok","allocated_mb":%.2f,"goroutines":%d}`,
			float64(m.Alloc)/(1024*1024), runtime.NumGoroutine())
	})

	mux.HandleFunc("/agent/task", s.handleCreateTask)
	mux.HandleFunc("/agent/tasks", s.handleListTasks)

	s.server = &http.Server{
		Addr:         s.ListenAddr,
		Handler:      mux,
		ReadTimeout:  60 * time.Second,
		WriteTimeout: 60 * time.Second,
	}

	log.Printf("[Game-Agent-Core] Orchestrator listening on http://%s (Resident memory: < 20 MB)", s.ListenAddr)
	return s.server.ListenAndServe()
}

func (s *AgentCoreServer) Shutdown(ctx context.Context) error {
	if s.server != nil {
		return s.server.Shutdown(ctx)
	}
	return nil
}

func (s *AgentCoreServer) handleCreateTask(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, `{"error":"Method not allowed"}`, http.StatusMethodNotAllowed)
		return
	}

	var req struct {
		AgentID string   `json:"agent_id"`
		Command string   `json:"command"`
		Args    []string `json:"args"`
		Timeout int      `json:"timeout_seconds"`
	}

	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, fmt.Sprintf(`{"error":"Invalid request: %v"}`, err), http.StatusBadRequest)
		return
	}

	taskID := fmt.Sprintf("task-%d", time.Now().UnixNano())
	rec := &AgentTaskRecord{
		ID:        taskID,
		AgentID:   req.AgentID,
		Command:   req.Command,
		Status:    StatusRunning,
		CreatedAt: time.Now().UTC(),
	}

	s.mu.Lock()
	s.tasks[taskID] = rec
	s.mu.Unlock()

	go func() {
		timeout := time.Duration(req.Timeout) * time.Second
		if timeout <= 0 {
			timeout = 180 * time.Second
		}

		res := s.pool.RunTasks([]runner.Task{
			{
				ID:         taskID,
				Command:    req.Command,
				Args:       req.Args,
				WorkingDir: s.repoRoot,
				Timeout:    timeout,
			},
		})

		now := time.Now().UTC()
		s.mu.Lock()
		rec.CompletedAt = &now
		rec.DurationMs = res[0].Duration.Milliseconds()
		rec.PeakRSSKb = res[0].PeakRSSKb
		rec.Output = res[0].Output
		rec.Error = res[0].Error
		if res[0].ExitCode == 0 {
			rec.Status = StatusDone
		} else {
			rec.Status = StatusFailed
		}
		s.mu.Unlock()
	}()

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusAccepted)
	_ = json.NewEncoder(w).Encode(rec)
}

func (s *AgentCoreServer) handleListTasks(w http.ResponseWriter, r *http.Request) {
	s.mu.RLock()
	var list []*AgentTaskRecord
	for _, t := range s.tasks {
		list = append(list, t)
	}
	s.mu.RUnlock()

	w.Header().Set("Content-Type", "application/json")
	_ = json.NewEncoder(w).Encode(list)
}
