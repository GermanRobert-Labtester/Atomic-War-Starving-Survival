// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Propaganda;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private PropagandaHostSession _propaganda = null!;
        private PropagandaPanel _propagandaPanel = null!;
        private bool _propagandaDirty;

        public PropagandaHostSession Propaganda => EnsurePropaganda();

        public PropagandaHostSession EnsurePropaganda()
        {
            if (_propaganda != null) return _propaganda;

            var state = PropagandaSaveStore.TryLoad() ?? new PropagandaState();
            var system = new PropagandaSystem(state);

            // Plan 168 — bind the authored campaign templates so template launches
            // resolve from the canonical catalog rather than an empty registry.
            string propagandaCatalogPath = CatalogPath.ResolveCatalog("propaganda_templates.json");
            var propagandaCatalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (propagandaCatalogIo.FileExists(propagandaCatalogPath))
            {
                var propagandaCatalog = System.Text.Json.JsonSerializer.Deserialize<PropagandaTemplateCatalogData>(
                    propagandaCatalogIo.ReadAllText(propagandaCatalogPath),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (propagandaCatalog?.templates != null)
                {
                    system.LoadTemplates(propagandaCatalog.templates);
                }
            }

            _propaganda = new PropagandaHostSession(system);
            _propaganda.StateChanged += OnPropagandaStateChanged;
            return _propaganda;
        }

        private void OnPropagandaStateChanged()
        {
            _propagandaDirty = true;
        }

        public void TickPropaganda(int day)
        {
            EnsurePropaganda();
            _propaganda.TickDay(day);
            _propagandaDirty = true;
        }

        private void SetupPropaganda()
        {
            EnsurePropaganda();
        }

        private void SavePropaganda()
        {
            var session = EnsurePropaganda();
            if (session != null)
            {
                CaptureSection("propaganda_campaigns", PropagandaSaveStore.TryCapturePersisted(session.System.CaptureState()));
                _propagandaDirty = false;
            }
        }

        private void SetupPropagandaPanel()
        {
            if (_propagandaPanel != null && _propagandaPanel.IsInsideTree())
                return;

            EnsurePropaganda();
            _propagandaPanel = new PropagandaPanel();
            _propagandaPanel.Bind(_propaganda);
            _propagandaPanel.OnClose += () => _propagandaPanel.Visible = false;
            _propagandaPanel.Visible = false;
            AddChild(_propagandaPanel);
        }

        public void ShowPropagandaPanel()
        {
            SetupPropagandaPanel();
            _propagandaPanel.Visible = true;
            _propagandaPanel.RefreshView();
        }
    }
}
