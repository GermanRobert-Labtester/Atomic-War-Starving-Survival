package proxy

import (
	"context"
	"fmt"
	"io"
	"log"
	"net/http"
	"net/http/httputil"
	"net/url"
	"strings"
	"sync"
	"time"
)

type RouteTarget struct {
	Prefix     string `json:"prefix"`      // e.g. "/v1/chat/completions" or "/anthropic"
	BackendURL string `json:"backend_url"` // e.g. "https://api.openai.com" or "http://localhost:11434"
	AuthHeader string `json:"auth_header,omitempty"`
}

type LLMProxyServer struct {
	ListenAddr string
	Routes     []RouteTarget
	server     *http.Server
	mu         sync.RWMutex
}

func NewLLMProxyServer(listenAddr string, routes []RouteTarget) *LLMProxyServer {
	if listenAddr == "" {
		listenAddr = "127.0.0.1:8088"
	}
	return &LLMProxyServer{
		ListenAddr: listenAddr,
		Routes:     routes,
	}
}

func (s *LLMProxyServer) Start() error {
	mux := http.NewServeMux()
	mux.HandleFunc("/healthz", func(w http.ResponseWriter, r *http.Request) {
		w.WriteHeader(http.StatusOK)
		w.Write([]byte(`{"status":"ok"}`))
	})

	mux.HandleFunc("/", s.handleProxy)

	s.server = &http.Server{
		Addr:         s.ListenAddr,
		Handler:      mux,
		ReadTimeout:  120 * time.Second,
		WriteTimeout: 120 * time.Second,
	}

	log.Printf("[LLM-Proxy] Listening on http://%s with %d configured routes", s.ListenAddr, len(s.Routes))
	return s.server.ListenAndServe()
}

func (s *LLMProxyServer) Shutdown(ctx context.Context) error {
	if s.server != nil {
		return s.server.Shutdown(ctx)
	}
	return nil
}

func (s *LLMProxyServer) handleProxy(w http.ResponseWriter, r *http.Request) {
	start := time.Now()
	s.mu.RLock()
	routes := s.Routes
	s.mu.RUnlock()

	var matched *RouteTarget
	for i := range routes {
		if strings.HasPrefix(r.URL.Path, routes[i].Prefix) {
			matched = &routes[i]
			break
		}
	}

	if matched == nil {
		http.Error(w, fmt.Sprintf(`{"error":"No LLM route matched path %s"}`, r.URL.Path), http.StatusBadGateway)
		return
	}

	targetURL, err := url.Parse(matched.BackendURL)
	if err != nil {
		http.Error(w, fmt.Sprintf(`{"error":"Invalid backend URL: %v"}`, err), http.StatusInternalServerError)
		return
	}

	proxy := httputil.NewSingleHostReverseProxy(targetURL)
	originalDirector := proxy.Director
	proxy.Director = func(req *http.Request) {
		originalDirector(req)
		req.Host = targetURL.Host
		if matched.AuthHeader != "" {
			req.Header.Set("Authorization", matched.AuthHeader)
		}
	}

	proxy.ErrorHandler = func(rw http.ResponseWriter, req *http.Request, e error) {
		log.Printf("[LLM-Proxy] Error routing %s -> %s: %v", req.URL.Path, matched.BackendURL, e)
		rw.WriteHeader(http.StatusBadGateway)
		rw.Write([]byte(fmt.Sprintf(`{"error":"Backend failure: %v"}`, e)))
	}

	proxy.ServeHTTP(w, r)
	log.Printf("[LLM-Proxy] %s %s -> %s (Elapsed: %v)", r.Method, r.URL.Path, matched.BackendURL, time.Since(start))
}

func MockLLMResponse(w http.ResponseWriter, model string, reply string) {
	w.Header().Set("Content-Type", "application/json")
	body := fmt.Sprintf(`{
  "id": "chatcmpl-mock",
  "object": "chat.completion",
  "created": %d,
  "model": "%s",
  "choices": [{
    "index": 0,
    "message": {
      "role": "assistant",
      "content": %q
    },
    "finish_reason": "stop"
  }],
  "usage": {
    "prompt_tokens": 10,
    "completion_tokens": 15,
    "total_tokens": 25
  }
}`, time.Now().Unix(), model, reply)
	io.WriteString(w, body)
}
