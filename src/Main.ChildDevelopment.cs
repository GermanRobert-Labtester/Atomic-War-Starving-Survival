// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChildDevelopmentHostSession? _childDevelopment;
        private bool _childDevelopmentDirty;

        public ChildDevelopmentHostSession EnsureChildDevelopment()
        {
            if (_childDevelopment != null) return _childDevelopment;
            SetupChildDevelopment();
            return _childDevelopment!;
        }

        private void SetupChildDevelopment()
        {
            if (_childDevelopment != null) return;

            string dataDir = CatalogPath.ResolveDataDir();
            _childDevelopment = ChildDevelopmentHostSession.Create(dataDir);

            _childDevelopment.StateChanged += () =>
            {
                _childDevelopmentDirty = true;
            };

            // Synchronize canonical children from GenerationalSystem if available
            SetupGenerational();
            if (_generational != null)
            {
                var genState = _generational.CaptureState();
                if (genState?.children != null)
                {
                    foreach (var c in genState.children)
                    {
                        var profile = _childDevelopment.RegisterChild(
                            c.survivorId,
                            c.survivorId,
                            c.birthDay,
                            null,
                            c.assignedGuardianId ?? string.Empty);

                        profile.EducationScore = Math.Clamp(c.educationXp, 0f, 100f);
                        profile.Stage = ChildDevelopmentSystem.ResolveCanonicalStage(c.birthDay, _simDay, c.adulthoodProcessed);
                    }
                }
            }
        }

        public void ResetChildDevelopment()
        {
            _childDevelopment = null;
            _childDevelopmentDirty = false;
        }

        public ChildProfile RegisterChildDevelopment(
            string childId,
            string name,
            int birthDay,
            IEnumerable<string>? parentIds = null,
            string caregiverId = "")
        {
            var session = EnsureChildDevelopment();
            var profile = session.RegisterChild(childId, name, birthDay, parentIds, caregiverId);

            _journal?.TryAddRawEntry(
                "child_registered",
                $"Child {name} registered in shelter nursery (Caregiver: {(string.IsNullOrEmpty(caregiverId) ? "unassigned" : caregiverId)}).",
                null!,
                _simDay);

            return profile;
        }

        public bool RecordChildEducation(string childId, float amount = 5f)
        {
            return EnsureChildDevelopment().RecordEducation(childId, amount);
        }

        public bool AssignChildCaregiver(string childId, string caregiverId)
        {
            return EnsureChildDevelopment().AssignCaregiver(childId, caregiverId);
        }

        public ChildDevelopmentCensus GetChildDevelopmentCensus()
        {
            return EnsureChildDevelopment().GetCensus();
        }
    }
}
