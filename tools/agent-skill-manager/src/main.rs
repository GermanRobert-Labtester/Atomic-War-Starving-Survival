use std::{
    fs,
    io::stdout,
    path::{Path, PathBuf},
    time::Duration,
};

use anyhow::Result;
use crossterm::{
    event::{self, Event, KeyCode, KeyEventKind, KeyModifiers},
    execute,
    terminal::{disable_raw_mode, enable_raw_mode, EnterAlternateScreen, LeaveAlternateScreen},
};
use ratatui::{
    backend::CrosstermBackend,
    layout::{Alignment, Constraint, Direction, Layout},
    style::{Color, Modifier, Style},
    text::{Line, Span},
    widgets::{
        Block, BorderType, Borders, List, ListItem, ListState, Paragraph, Wrap,
    },
    Frame, Terminal,
};

#[derive(Debug, Clone)]
pub struct SkillInfo {
    pub name: String,
    pub is_active: bool,
    pub path: PathBuf,
    pub description: String,
    pub is_symlink: bool,
}

#[derive(Debug, Clone)]
pub struct AgentHarness {
    pub id: String,
    pub name: String,
    pub candidate_paths: Vec<PathBuf>,
    pub resolved_path: Option<PathBuf>,
    pub is_installed: bool,
    pub skills: Vec<SkillInfo>,
}

impl AgentHarness {
    pub fn new(id: &str, name: &str, candidate_paths: Vec<PathBuf>) -> Self {
        let mut harness = Self {
            id: id.to_string(),
            name: name.to_string(),
            candidate_paths,
            resolved_path: None,
            is_installed: false,
            skills: Vec::new(),
        };
        harness.refresh();
        harness
    }

    pub fn refresh(&mut self) {
        self.skills.clear();
        self.resolved_path = None;
        self.is_installed = false;

        let paths = self.candidate_paths.clone();
        for p in &paths {
            if p.exists() {
                self.is_installed = true;
                self.resolved_path = Some(p.clone());
                self.load_skills_from_dir(p);
                break;
            }
        }

        // If parent directory exists, consider the agent installed even if skills folder isn't populated yet
        if !self.is_installed {
            for p in &paths {
                if let Some(parent) = p.parent() {
                    if parent.exists() {
                        self.is_installed = true;
                        self.resolved_path = Some(p.clone());
                        break;
                    }
                }
            }
        }
    }

    fn load_skills_from_dir(&mut self, dir: &Path) {
        if let Ok(entries) = fs::read_dir(dir) {
            let mut items = Vec::new();
            for entry in entries.flatten() {
                let path = entry.path();
                let file_name = path.file_name().unwrap_or_default().to_string_lossy().to_string();

                if file_name.starts_with('.') && file_name != ".disabled" {
                    continue;
                }

                let is_symlink = path.is_symlink();
                let is_dir = path.is_dir();
                let is_file = path.is_file();

                if !is_dir && !is_file && !is_symlink {
                    continue;
                }

                let is_disabled = file_name.ends_with(".disabled")
                    || file_name.ends_with(".off")
                    || file_name.ends_with(".bak");

                let display_name = if is_disabled {
                    file_name
                        .trim_end_matches(".disabled")
                        .trim_end_matches(".off")
                        .trim_end_matches(".bak")
                        .to_string()
                } else {
                    file_name.clone()
                };

                let desc = extract_skill_description(&path);

                items.push(SkillInfo {
                    name: display_name,
                    is_active: !is_disabled,
                    path: path.clone(),
                    description: desc,
                    is_symlink,
                });
            }

            items.sort_by(|a, b| a.name.to_lowercase().cmp(&b.name.to_lowercase()));
            self.skills = items;
        }
    }

    pub fn toggle_skill(&mut self, index: usize, target_state: Option<bool>) -> Result<bool> {
        if index >= self.skills.len() {
            return Ok(false);
        }

        let skill = &mut self.skills[index];
        let desired = target_state.unwrap_or(!skill.is_active);

        if desired == skill.is_active {
            return Ok(false); // No change needed
        }

        let old_path = &skill.path;
        let parent = match old_path.parent() {
            Some(p) => p,
            None => return Ok(false),
        };

        let new_path = if desired {
            // Enable: remove .disabled suffix
            parent.join(&skill.name)
        } else {
            // Disable: append .disabled suffix
            parent.join(format!("{}.disabled", skill.name))
        };

        if old_path.exists() {
            fs::rename(old_path, &new_path)?;
            skill.path = new_path;
            skill.is_active = desired;
            Ok(true)
        } else {
            Ok(false)
        }
    }

    pub fn set_all_skills(&mut self, enable: bool) -> Result<usize> {
        let mut count = 0;
        for i in 0..self.skills.len() {
            if self.skills[i].is_active != enable {
                if self.toggle_skill(i, Some(enable))? {
                    count += 1;
                }
            }
        }
        Ok(count)
    }
}

fn extract_skill_description(path: &Path) -> String {
    let skill_file = if path.is_dir() {
        path.join("SKILL.md")
    } else {
        path.to_path_buf()
    };

    if let Ok(content) = fs::read_to_string(&skill_file) {
        // Try YAML frontmatter first
        if content.starts_with("---") {
            let mut in_yaml = false;
            let mut desc_lines = Vec::new();
            let mut capturing_desc = false;

            for line in content.lines() {
                if line.trim() == "---" {
                    if in_yaml {
                        break;
                    } else {
                        in_yaml = true;
                        continue;
                    }
                }

                if in_yaml {
                    if line.starts_with("description:") {
                        capturing_desc = true;
                        let desc_part = line.trim_start_matches("description:").trim();
                        let clean = desc_part.trim_matches('"').trim_matches('\'').trim_matches('>');
                        if !clean.is_empty() {
                            desc_lines.push(clean.to_string());
                        }
                    } else if capturing_desc {
                        if line.starts_with("  ") || line.starts_with('\t') {
                            desc_lines.push(line.trim().to_string());
                        } else if line.contains(':') {
                            capturing_desc = false;
                        }
                    }
                }
            }

            if !desc_lines.is_empty() {
                return desc_lines.join(" ");
            }
        }

        // Fallback: search for first non-heading paragraph
        for line in content.lines() {
            let trimmed = line.trim();
            if !trimmed.is_empty()
                && !trimmed.starts_with('#')
                && !trimmed.starts_with("---")
                && !trimmed.starts_with("```")
                && !trimmed.starts_with("name:")
            {
                return trimmed.chars().take(220).collect();
            }
        }
    }

    "No description provided in SKILL.md".to_string()
}

#[derive(PartialEq, Eq, Clone, Copy)]
pub enum FocusPane {
    Agents,
    Skills,
    Filter,
}

pub struct App {
    pub harnesses: Vec<AgentHarness>,
    pub agent_state: ListState,
    pub skill_state: ListState,
    pub focus: FocusPane,
    pub filter: String,
    pub status_message: String,
    pub status_is_err: bool,
}

impl App {
    pub fn new() -> Self {
        let home = dirs::home_dir().unwrap_or_else(|| PathBuf::from("/home/robertsrff"));
        let workspace = std::env::current_dir().unwrap_or_else(|_| home.join("Music/Atomic_War_Straving_Survival/Atomic War"));

        let mut harnesses = vec![
            AgentHarness::new(
                "antigravity",
                "Antigravity (Gemini CLI)",
                vec![
                    home.join(".gemini/config/skills"),
                    home.join(".gemini/antigravity-cli/skills"),
                ],
            ),
            AgentHarness::new(
                "pi",
                "Pi Coding Agent",
                vec![
                    home.join(".pi/agent/skills"),
                    home.join(".pi/skills"),
                ],
            ),
            AgentHarness::new(
                "goose",
                "Goose Agent",
                vec![
                    home.join(".config/goose/skills"),
                    home.join(".local/share/goose/skills"),
                ],
            ),
            AgentHarness::new(
                "workspace",
                "Workspace Repo (.agents)",
                vec![
                    workspace.join(".agents/skills"),
                    home.join(".agents/skills"),
                ],
            ),
            AgentHarness::new(
                "claude",
                "Claude Code",
                vec![
                    home.join(".claude/skills"),
                    home.join(".config/claude/skills"),
                ],
            ),
            AgentHarness::new(
                "cline",
                "Cline Agent",
                vec![home.join(".cline/skills")],
            ),
            AgentHarness::new(
                "cursor",
                "Cursor AI",
                vec![
                    home.join(".cursor/skills"),
                    home.join(".cursorrules"),
                ],
            ),
            AgentHarness::new(
                "roo",
                "Roo Code / Roo Cline",
                vec![home.join(".roo/skills")],
            ),
            AgentHarness::new(
                "aider",
                "Aider / Aider Desk",
                vec![
                    home.join(".aider-desk/skills"),
                    home.join(".aider/skills"),
                ],
            ),
            AgentHarness::new(
                "openhands",
                "OpenHands Agent",
                vec![home.join(".openhands/skills")],
            ),
            AgentHarness::new(
                "continue",
                "Continue Dev",
                vec![home.join(".continue/skills")],
            ),
            AgentHarness::new(
                "codex",
                "Codex Agent",
                vec![home.join(".codex/skills")],
            ),
            AgentHarness::new(
                "crush",
                "Crush Agent",
                vec![home.join(".crush/skills")],
            ),
            AgentHarness::new(
                "mimocode",
                "Mimocode",
                vec![home.join(".mimocode/skills")],
            ),
            AgentHarness::new(
                "qwen",
                "Qwen Code",
                vec![home.join(".qwen/skills")],
            ),
            AgentHarness::new(
                "vibe",
                "Vibe Agent",
                vec![home.join(".vibe/skills")],
            ),
            AgentHarness::new(
                "trae",
                "Trae / Trae-CN",
                vec![
                    home.join(".trae/skills"),
                    home.join(".trae-cn/skills"),
                ],
            ),
            AgentHarness::new(
                "kilo",
                "Kilo / Kilocode",
                vec![
                    home.join(".kilo/skills"),
                    home.join(".kilocode/skills"),
                ],
            ),
            AgentHarness::new(
                "kiro",
                "Kiro Agent",
                vec![home.join(".kiro/skills")],
            ),
            AgentHarness::new(
                "kode",
                "Kode Agent",
                vec![home.join(".kode/skills")],
            ),
            AgentHarness::new(
                "neovate",
                "Neovate Agent",
                vec![home.join(".neovate/skills")],
            ),
            AgentHarness::new(
                "pochi",
                "Pochi Agent",
                vec![home.join(".pochi/skills")],
            ),
            AgentHarness::new(
                "adal",
                "Adal Agent",
                vec![home.join(".adal/skills")],
            ),
            AgentHarness::new(
                "commandcode",
                "CommandCode",
                vec![home.join(".commandcode/skills")],
            ),
            AgentHarness::new(
                "codebuddy",
                "CodeBuddy",
                vec![home.join(".codebuddy/skills")],
            ),
            AgentHarness::new(
                "factory",
                "Factory AI",
                vec![home.join(".factory/skills")],
            ),
            AgentHarness::new(
                "junie",
                "Junie Agent",
                vec![home.join(".junie/skills")],
            ),
            AgentHarness::new(
                "iflow",
                "iFlow Agent",
                vec![home.join(".iflow/skills")],
            ),
            AgentHarness::new(
                "mcpjam",
                "MCPJam Agent",
                vec![home.join(".mcpjam/skills")],
            ),
            AgentHarness::new(
                "mux",
                "Mux Agent",
                vec![home.join(".mux/skills")],
            ),
            AgentHarness::new(
                "qoder",
                "Qoder / Qoder-CN",
                vec![
                    home.join(".qoder/skills"),
                    home.join(".qoder-cn/skills"),
                    home.join(".qodersec/skills"),
                ],
            ),
            AgentHarness::new(
                "zencoder",
                "Zencoder",
                vec![home.join(".zencoder/skills")],
            ),
            AgentHarness::new(
                "zcode",
                "ZCode Agent",
                vec![home.join(".zcode/skills")],
            ),
            AgentHarness::new(
                "minimax",
                "MiniMax Agent",
                vec![home.join(".minimax/skills")],
            ),
            AgentHarness::new(
                "bailian",
                "Bailian Agent",
                vec![home.join(".bailian/skills")],
            ),
            AgentHarness::new(
                "bob",
                "Bob Agent",
                vec![home.join(".bob/skills")],
            ),
            AgentHarness::new(
                "devin",
                "Devin Agent",
                vec![home.join(".devin/skills")],
            ),
            AgentHarness::new(
                "openclaw",
                "OpenClaw",
                vec![home.join(".openclaw/skills")],
            ),
            AgentHarness::new(
                "plandex",
                "Plandex",
                vec![home.join(".plandex-home-v2/skills")],
            ),
        ];

        // Sort so installed agents appear first, then alphabetically
        harnesses.sort_by(|a, b| {
            b.is_installed.cmp(&a.is_installed).then_with(|| a.name.cmp(&b.name))
        });

        let mut agent_state = ListState::default();
        if !harnesses.is_empty() {
            agent_state.select(Some(0));
        }

        let mut skill_state = ListState::default();
        if !harnesses.is_empty() && !harnesses[0].skills.is_empty() {
            skill_state.select(Some(0));
        }

        Self {
            harnesses,
            agent_state,
            skill_state,
            focus: FocusPane::Agents,
            filter: String::new(),
            status_message: "Use [Left Arrow] to DISABLE, [Right Arrow] to ENABLE. [Tab] to switch panels.".to_string(),
            status_is_err: false,
        }
    }

    pub fn selected_agent_index(&self) -> usize {
        self.agent_state.selected().unwrap_or(0)
    }

    pub fn selected_agent(&self) -> Option<&AgentHarness> {
        self.harnesses.get(self.selected_agent_index())
    }

    pub fn selected_agent_mut(&mut self) -> Option<&mut AgentHarness> {
        let idx = self.selected_agent_index();
        self.harnesses.get_mut(idx)
    }

    pub fn filtered_skills(&self) -> Vec<(usize, SkillInfo)> {
        if let Some(agent) = self.selected_agent() {
            agent
                .skills
                .iter()
                .enumerate()
                .filter(|(_, s)| {
                    if self.filter.is_empty() {
                        true
                    } else {
                        s.name.to_lowercase().contains(&self.filter.to_lowercase())
                            || s.description.to_lowercase().contains(&self.filter.to_lowercase())
                    }
                })
                .map(|(i, s)| (i, s.clone()))
                .collect()
        } else {
            Vec::new()
        }
    }

    pub fn next_agent(&mut self) {
        if self.harnesses.is_empty() {
            return;
        }
        let current = self.selected_agent_index();
        let next = if current + 1 >= self.harnesses.len() {
            0
        } else {
            current + 1
        };
        self.agent_state.select(Some(next));
        self.skill_state.select(Some(0));
    }

    pub fn prev_agent(&mut self) {
        if self.harnesses.is_empty() {
            return;
        }
        let current = self.selected_agent_index();
        let prev = if current == 0 {
            self.harnesses.len() - 1
        } else {
            current - 1
        };
        self.agent_state.select(Some(prev));
        self.skill_state.select(Some(0));
    }

    pub fn next_skill(&mut self) {
        let count = self.filtered_skills().len();
        if count == 0 {
            return;
        }
        let current = self.skill_state.selected().unwrap_or(0);
        let next = if current + 1 >= count { 0 } else { current + 1 };
        self.skill_state.select(Some(next));
    }

    pub fn prev_skill(&mut self) {
        let count = self.filtered_skills().len();
        if count == 0 {
            return;
        }
        let current = self.skill_state.selected().unwrap_or(0);
        let prev = if current == 0 { count - 1 } else { current - 1 };
        self.skill_state.select(Some(prev));
    }

    pub fn toggle_current_skill(&mut self, force_state: Option<bool>) {
        let filtered = self.filtered_skills();
        let sel = self.skill_state.selected().unwrap_or(0);

        if let Some((real_idx, _)) = filtered.get(sel).cloned() {
            let agent_name = self.selected_agent().map(|a| a.name.clone()).unwrap_or_default();
            if let Some(agent) = self.selected_agent_mut() {
                let skill_name = agent.skills[real_idx].name.clone();
                match agent.toggle_skill(real_idx, force_state) {
                    Ok(true) => {
                        let new_state = agent.skills[real_idx].is_active;
                        let state_str = if new_state { "ENABLED [✔ ACTIVE]" } else { "DISABLED [✖ OFF]" };
                        self.status_message = format!("{} -> {}: {}", agent_name, skill_name, state_str);
                        self.status_is_err = false;
                    }
                    Ok(false) => {
                        let current_state = if agent.skills[real_idx].is_active { "already ENABLED" } else { "already DISABLED" };
                        self.status_message = format!("Skill '{}' is {}", skill_name, current_state);
                        self.status_is_err = false;
                    }
                    Err(e) => {
                        self.status_message = format!("Failed to toggle skill: {}", e);
                        self.status_is_err = true;
                    }
                }
            }
        }
    }

    pub fn set_all_for_selected(&mut self, enable: bool) {
        let agent_name = self.selected_agent().map(|a| a.name.clone()).unwrap_or_default();
        if let Some(agent) = self.selected_agent_mut() {
            match agent.set_all_skills(enable) {
                Ok(count) => {
                    let action = if enable { "Enabled" } else { "Disabled" };
                    self.status_message = format!("{} {} skills for {}", action, count, agent_name);
                    self.status_is_err = false;
                }
                Err(e) => {
                    self.status_message = format!("Error updating skills: {}", e);
                    self.status_is_err = true;
                }
            }
        }
    }

    pub fn refresh_all(&mut self) {
        for h in &mut self.harnesses {
            h.refresh();
        }
        self.status_message = "Refreshed all AI agent skill manifests.".to_string();
        self.status_is_err = false;
    }
}

fn main() -> Result<()> {
    enable_raw_mode()?;
    let mut stdout = stdout();
    execute!(stdout, EnterAlternateScreen)?;
    let backend = CrosstermBackend::new(stdout);
    let mut terminal = Terminal::new(backend)?;

    let mut app = App::new();
    let res = run_app(&mut terminal, &mut app);

    disable_raw_mode()?;
    execute!(terminal.backend_mut(), LeaveAlternateScreen)?;
    terminal.show_cursor()?;

    if let Err(err) = res {
        eprintln!("Application Error: {:#?}", err);
    }

    Ok(())
}

fn run_app<B: ratatui::backend::Backend>(terminal: &mut Terminal<B>, app: &mut App) -> Result<()> {
    loop {
        terminal.draw(|f| ui(f, app))?;

        if event::poll(Duration::from_millis(100))? {
            if let Event::Key(key) = event::read()? {
                if key.kind != KeyEventKind::Press {
                    continue;
                }

                if app.focus == FocusPane::Filter {
                    match key.code {
                        KeyCode::Esc | KeyCode::Enter => {
                            app.focus = FocusPane::Skills;
                        }
                        KeyCode::Backspace => {
                            app.filter.pop();
                            app.skill_state.select(Some(0));
                        }
                        KeyCode::Char(c) => {
                            app.filter.push(c);
                            app.skill_state.select(Some(0));
                        }
                        _ => {}
                    }
                    continue;
                }

                match key.code {
                    KeyCode::Char('q') | KeyCode::Esc => return Ok(()),
                    KeyCode::Char('r') | KeyCode::F(5) => app.refresh_all(),
                    KeyCode::Char('/') | KeyCode::Char('s') => {
                        app.focus = FocusPane::Filter;
                        app.filter.clear();
                    }

                    // Panel Switching
                    KeyCode::Tab => {
                        app.focus = match app.focus {
                            FocusPane::Agents => FocusPane::Skills,
                            FocusPane::Skills => FocusPane::Agents,
                            FocusPane::Filter => FocusPane::Skills,
                        };
                    }
                    KeyCode::BackTab => {
                        app.focus = match app.focus {
                            FocusPane::Agents => FocusPane::Skills,
                            FocusPane::Skills => FocusPane::Agents,
                            FocusPane::Filter => FocusPane::Agents,
                        };
                    }

                    // Navigation
                    KeyCode::Up | KeyCode::Char('k') => match app.focus {
                        FocusPane::Agents => app.prev_agent(),
                        FocusPane::Skills => app.prev_skill(),
                        _ => {}
                    },
                    KeyCode::Down | KeyCode::Char('j') => match app.focus {
                        FocusPane::Agents => app.next_agent(),
                        FocusPane::Skills => app.next_skill(),
                        _ => {}
                    },

                    // Toggling & Directional controls
                    KeyCode::Right | KeyCode::Char('l') => match app.focus {
                        FocusPane::Agents => {
                            app.focus = FocusPane::Skills;
                            app.skill_state.select(Some(0));
                        }
                        FocusPane::Skills => {
                            // Right Arrow = TOGGLE ON / ENABLE
                            app.toggle_current_skill(Some(true));
                        }
                        _ => {}
                    },
                    KeyCode::Left | KeyCode::Char('h') => match app.focus {
                        FocusPane::Skills => {
                            // Left Arrow = TOGGLE OFF / DISABLE
                            app.toggle_current_skill(Some(false));
                        }
                        FocusPane::Agents => {
                            // Already on agents panel
                        }
                        _ => {}
                    },
                    KeyCode::Char(' ') | KeyCode::Enter => {
                        if app.focus == FocusPane::Skills {
                            app.toggle_current_skill(None);
                        } else {
                            app.focus = FocusPane::Skills;
                            app.skill_state.select(Some(0));
                        }
                    }

                    // Bulk Actions
                    KeyCode::Char('a') | KeyCode::Char('A') => {
                        if key.modifiers.contains(KeyModifiers::CONTROL) || key.code == KeyCode::Char('a') || key.code == KeyCode::Char('A') {
                            app.set_all_for_selected(true);
                        }
                    }
                    KeyCode::Char('d') | KeyCode::Char('D') => {
                        app.set_all_for_selected(false);
                    }

                    _ => {}
                }
            }
        }
    }
}

fn ui(f: &mut Frame, app: &mut App) {
    let size = f.area();

    // Base background layout
    let main_chunks = Layout::default()
        .direction(Direction::Vertical)
        .constraints([
            Constraint::Length(3), // Header banner
            Constraint::Min(10),   // Content area
            Constraint::Length(3), // Footer / Hotkeys
        ])
        .split(size);

    // ── Header ──────────────────────────────────────────────────────────
    let installed_count = app.harnesses.iter().filter(|h| h.is_installed).count();
    let total_skills: usize = app.harnesses.iter().map(|h| h.skills.len()).sum();
    let active_skills: usize = app
        .harnesses
        .iter()
        .map(|h| h.skills.iter().filter(|s| s.is_active).count())
        .sum();

    let header_text = vec![
        Line::from(vec![
            Span::styled(" ◈ AI AGENT SKILL MANAGER ", Style::default().fg(Color::Cyan).add_modifier(Modifier::BOLD)),
            Span::styled("│ Universal Multi-Agent Skill Switcher ", Style::default().fg(Color::White)),
            Span::styled(format!("│ Agents: {}/{} ", installed_count, app.harnesses.len()), Style::default().fg(Color::Yellow)),
            Span::styled(format!("│ Total Skills: {} (Active: {}, Disabled: {})", total_skills, active_skills, total_skills - active_skills), Style::default().fg(Color::Green)),
        ]),
    ];

    let header = Paragraph::new(header_text)
        .block(
            Block::default()
                .borders(Borders::ALL)
                .border_type(BorderType::Rounded)
                .border_style(Style::default().fg(Color::Cyan)),
        )
        .alignment(Alignment::Left);
    f.render_widget(header, main_chunks[0]);

    // ── Main Body (Split into Left: Agents, Right: Skills + Inspector) ──
    let body_chunks = Layout::default()
        .direction(Direction::Horizontal)
        .constraints([
            Constraint::Percentage(32), // Left: Agent List
            Constraint::Percentage(68), // Right: Skills & Inspector
        ])
        .split(main_chunks[1]);

    // ── Left: Agent List ────────────────────────────────────────────────
    let agent_items: Vec<ListItem> = app
        .harnesses
        .iter()
        .enumerate()
        .map(|(i, h)| {
            let active_count = h.skills.iter().filter(|s| s.is_active).count();
            let total_count = h.skills.len();

            let status_badge = if h.is_installed {
                if total_count > 0 {
                    Span::styled(format!("● {} active", active_count), Style::default().fg(Color::Green))
                } else {
                    Span::styled("● Installed (0 skills)", Style::default().fg(Color::Cyan))
                }
            } else {
                Span::styled("○ Not Installed", Style::default().fg(Color::DarkGray))
            };

            let name_style = if i == app.selected_agent_index() && app.focus == FocusPane::Agents {
                Style::default().fg(Color::Yellow).add_modifier(Modifier::BOLD)
            } else if h.is_installed {
                Style::default().fg(Color::White)
            } else {
                Style::default().fg(Color::DarkGray)
            };

            let line = Line::from(vec![
                Span::styled(format!("{:<22} ", h.name), name_style),
                status_badge,
            ]);

            ListItem::new(line)
        })
        .collect();

    let agent_border_color = if app.focus == FocusPane::Agents {
        Color::Yellow
    } else {
        Color::DarkGray
    };

    let agent_list = List::new(agent_items)
        .block(
            Block::default()
                .title(" [1] AI Agents & Harnesses ")
                .borders(Borders::ALL)
                .border_type(BorderType::Rounded)
                .border_style(Style::default().fg(agent_border_color)),
        )
        .highlight_style(
            Style::default()
                .bg(Color::Rgb(30, 45, 60))
                .fg(Color::Yellow)
                .add_modifier(Modifier::BOLD),
        )
        .highlight_symbol("▶ ");

    f.render_stateful_widget(agent_list, body_chunks[0], &mut app.agent_state);

    // ── Right: Split into Skills List + Inspector ────────────────────────
    let right_chunks = Layout::default()
        .direction(Direction::Vertical)
        .constraints([
            Constraint::Percentage(62), // Skills List
            Constraint::Percentage(38), // Detail Inspector
        ])
        .split(body_chunks[1]);

    // Extract agent details for display
    let agent_title = app.selected_agent().map(|a| a.name.clone()).unwrap_or_else(|| "No Agent Selected".to_string());
    let is_installed = app.selected_agent().map(|a| a.is_installed).unwrap_or(false);
    let candidate_paths = app.selected_agent().map(|a| a.candidate_paths.clone()).unwrap_or_default();

    let filtered = app.filtered_skills();

    let skill_items: Vec<ListItem> = if !is_installed {
        vec![ListItem::new(Line::from(vec![Span::styled(
            "  This AI agent harness is NOT currently installed on this system.",
            Style::default().fg(Color::DarkGray),
        )]))]
    } else if filtered.is_empty() {
        vec![ListItem::new(Line::from(vec![Span::styled(
            "  No skills discovered in this agent's directory.",
            Style::default().fg(Color::DarkGray),
        )]))]
    } else {
        filtered
            .iter()
            .map(|(_, s)| {
                let (toggle_badge, badge_style) = if s.is_active {
                    (
                        " [✔ ACTIVE]  [● ON ]",
                        Style::default().fg(Color::Green).add_modifier(Modifier::BOLD),
                    )
                } else {
                    (
                        " [✖ OFF]     [ OFF●]",
                        Style::default().fg(Color::Red).add_modifier(Modifier::DIM),
                    )
                };

                let name_style = if s.is_active {
                    Style::default().fg(Color::White).add_modifier(Modifier::BOLD)
                } else {
                    Style::default().fg(Color::DarkGray)
                };

                let symlink_tag = if s.is_symlink {
                    Span::styled(" [symlink]", Style::default().fg(Color::Magenta))
                } else {
                    Span::raw("")
                };

                let line = Line::from(vec![
                    Span::styled(format!("{:<34}", s.name), name_style),
                    symlink_tag,
                    Span::styled(toggle_badge, badge_style),
                ]);

                ListItem::new(line)
            })
            .collect()
    };

    let skill_border_color = if app.focus == FocusPane::Skills {
        Color::Yellow
    } else if app.focus == FocusPane::Filter {
        Color::Cyan
    } else {
        Color::DarkGray
    };

    let skill_title = if app.filter.is_empty() {
        format!(" [2] Skills for {} (Total: {}) ", agent_title, filtered.len())
    } else {
        format!(" [2] Skills for {} [Filter: \"{}\" ({})] ", agent_title, app.filter, filtered.len())
    };

    let skill_list = List::new(skill_items)
        .block(
            Block::default()
                .title(skill_title)
                .borders(Borders::ALL)
                .border_type(BorderType::Rounded)
                .border_style(Style::default().fg(skill_border_color)),
        )
        .highlight_style(
            Style::default()
                .bg(Color::Rgb(25, 40, 55))
                .fg(Color::Yellow)
                .add_modifier(Modifier::BOLD),
        )
        .highlight_symbol("▶ ");

    f.render_stateful_widget(skill_list, right_chunks[0], &mut app.skill_state);

    // Detail Inspector (Bottom Right)
    let selected_skill = {
        let sel = app.skill_state.selected().unwrap_or(0);
        filtered.get(sel).map(|(_, s)| s.clone())
    };

    let inspector_content = if let Some(s) = selected_skill {
        let status_span = if s.is_active {
            Span::styled("ENABLED / ACTIVE (Injected into agent)", Style::default().fg(Color::Green).add_modifier(Modifier::BOLD))
        } else {
            Span::styled("DISABLED / INACTIVE (Excluded from agent)", Style::default().fg(Color::Red).add_modifier(Modifier::BOLD))
        };

        vec![
            Line::from(vec![
                Span::styled("Skill: ", Style::default().fg(Color::Cyan).add_modifier(Modifier::BOLD)),
                Span::styled(s.name.clone(), Style::default().fg(Color::White).add_modifier(Modifier::BOLD)),
                Span::raw("  │  Status: "),
                status_span,
            ]),
            Line::from(vec![
                Span::styled("Path:  ", Style::default().fg(Color::Cyan)),
                Span::styled(s.path.to_string_lossy().to_string(), Style::default().fg(Color::DarkGray)),
            ]),
            Line::from(vec![
                Span::styled("Desc:  ", Style::default().fg(Color::Cyan)),
                Span::styled(s.description.clone(), Style::default().fg(Color::White)),
            ]),
            Line::from(vec![
                Span::styled("Action: ", Style::default().fg(Color::Yellow)),
                Span::styled("Press [RIGHT ARROW] to Enable  │  [LEFT ARROW] to Disable  │  [SPACE] to Toggle", Style::default().fg(Color::Yellow)),
            ]),
        ]
    } else if !is_installed {
        vec![
            Line::from(vec![
                Span::styled("Agent Status: ", Style::default().fg(Color::Cyan).add_modifier(Modifier::BOLD)),
                Span::styled("Not Installed", Style::default().fg(Color::DarkGray)),
            ]),
            Line::from(vec![
                Span::styled("Expected candidate paths: ", Style::default().fg(Color::DarkGray)),
                Span::styled(
                    candidate_paths.iter().map(|p| p.to_string_lossy().to_string()).collect::<Vec<_>>().join(", "),
                    Style::default().fg(Color::DarkGray),
                ),
            ]),
        ]
    } else {
        vec![Line::from(vec![Span::styled(
            "Select a skill to inspect its metadata, path, and toggle its active state.",
            Style::default().fg(Color::DarkGray),
        )])]
    };

    let inspector = Paragraph::new(inspector_content)
        .block(
            Block::default()
                .title(" Skill Inspector & Quick Actions ")
                .borders(Borders::ALL)
                .border_type(BorderType::Rounded)
                .border_style(Style::default().fg(Color::Cyan)),
        )
        .wrap(Wrap { trim: true });

    f.render_widget(inspector, right_chunks[1]);

    // ── Footer / Hotkey Guide ───────────────────────────────────────────
    let status_style = if app.status_is_err {
        Style::default().fg(Color::Red).add_modifier(Modifier::BOLD)
    } else {
        Style::default().fg(Color::Green)
    };

    let footer_text = vec![
        Line::from(vec![
            Span::styled(" [← Left] ", Style::default().fg(Color::Black).bg(Color::Red)),
            Span::raw(" Disable Skill  "),
            Span::styled(" [Right →] ", Style::default().fg(Color::Black).bg(Color::Green)),
            Span::raw(" Enable Skill  "),
            Span::styled(" [Tab] ", Style::default().fg(Color::Black).bg(Color::Yellow)),
            Span::raw(" Switch Pane  "),
            Span::styled(" [A] ", Style::default().fg(Color::Black).bg(Color::Cyan)),
            Span::raw(" Enable All  "),
            Span::styled(" [D] ", Style::default().fg(Color::Black).bg(Color::Magenta)),
            Span::raw(" Disable All  "),
            Span::styled(" [/] ", Style::default().fg(Color::Black).bg(Color::White)),
            Span::raw(" Filter  "),
            Span::styled(" [Q/Esc] ", Style::default().fg(Color::Black).bg(Color::DarkGray)),
            Span::raw(" Quit"),
        ]),
        Line::from(vec![
            Span::styled(" Status: ", Style::default().fg(Color::Cyan)),
            Span::styled(&app.status_message, status_style),
        ]),
    ];

    let footer = Paragraph::new(footer_text)
        .block(
            Block::default()
                .borders(Borders::ALL)
                .border_type(BorderType::Rounded)
                .border_style(Style::default().fg(Color::DarkGray)),
        );
    f.render_widget(footer, main_chunks[2]);
}
