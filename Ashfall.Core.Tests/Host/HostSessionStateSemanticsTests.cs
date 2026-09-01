// SPDX-License-Identifier: MIT
// ASHFALL CI Test Suite: StatefulSession State-Change, Dirty, and Flush Semantics (Task 108).
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Host
{
    public sealed class HostSessionStateSemanticsTests : IDisposable
    {
        private readonly string _testTempDir;

        public HostSessionStateSemanticsTests()
        {
            _testTempDir = Path.Combine(Path.GetTempPath(), "ashfall_semantics_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testTempDir);
            SaveEnvelopeHelper.ResetAtomicWriteCounter();
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_testTempDir))
                    Directory.Delete(_testTempDir, true);
            }
            catch { }
        }

        private class TestSession : StatefulSessionBase
        {
            public int CustomValue { get; private set; }

            public ActionResult MutateValue(int delta)
            {
                if (delta <= 0)
                {
                    return HandleActionResult(ActionResult.Blocked("invalid_delta", "test.invalid_delta"));
                }
                CustomValue += delta;
                return HandleActionResult(ActionResult.Success("test.mutated"));
            }

            public void PerformNoOp()
            {
                // No mutation, does not call RaiseStateChanged
            }

            public void PerformDirectMutation()
            {
                CustomValue += 1;
                RaiseStateChanged();
            }
        }

        private sealed class TestDomainHostSession : StatefulSessionBase
        {
            public ApprenticeshipSystem System { get; }
            public string LastEvent { get; private set; } = string.Empty;

            public TestDomainHostSession(ApprenticeshipSystem system)
            {
                System = system;
                System.OnApprenticeshipChanged += () => RaiseStateChanged();
            }

            public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f)
            {
                var res = System.StartPair(mentorId, apprenticeId, targetSkillId, targetXp);
                if (res.IsSuccess)
                {
                    LastEvent = $"Assigned {apprenticeId} under mentor {mentorId}";
                }
                return res;
            }
        }

        private sealed class TestAirlockHostSession : StatefulSessionBase
        {
            public AirlockSecuritySystem System { get; }
            public string LastEvent { get; private set; } = string.Empty;

            public TestAirlockHostSession(AirlockSecuritySystem system)
            {
                System = system;
                System.OnSecurityChanged += () => RaiseStateChanged();
            }

            public ActionResult ResolveIncident(VisitorDecision decision)
            {
                var res = System.ResolveIncident(decision);
                if (res.IsSuccess)
                {
                    LastEvent = $"Resolved {decision}";
                }
                return res;
            }
        }

        [Fact]
        public void InitialSession_IsClean_StateVersionZero_SaveCountZero()
        {
            using var session = new TestSession();
            Assert.False(session.IsDirty);
            Assert.Equal(0, session.StateVersion);
            Assert.Equal(0, session.SaveCount);
        }

        [Fact]
        public void SuccessfulAction_IncrementsStateVersion_MarksDirty_FiresEvents()
        {
            using var session = new TestSession();
            bool stateChangedFired = false;
            long observedVersion = -1;

            session.StateChanged += () => stateChangedFired = true;
            session.StateVersionChanged += v => observedVersion = v;

            var result = session.MutateValue(5);

            Assert.True(result.IsSuccess);
            Assert.True(session.IsDirty);
            Assert.Equal(1, session.StateVersion);
            Assert.Equal(1, observedVersion);
            Assert.True(stateChangedFired);
            Assert.Equal(5, session.CustomValue);
        }

        [Fact]
        public void RejectedAction_DoesNotIncrementStateVersion_DoesNotMarkDirty_DoesNotFireEvents()
        {
            using var session = new TestSession();
            bool stateChangedFired = false;
            long observedVersion = -1;

            session.StateChanged += () => stateChangedFired = true;
            session.StateVersionChanged += v => observedVersion = v;

            var result = session.MutateValue(-10); // Rejected: delta <= 0

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.False(session.IsDirty);
            Assert.Equal(0, session.StateVersion);
            Assert.Equal(-1, observedVersion);
            Assert.False(stateChangedFired);
            Assert.Equal(0, session.CustomValue);
        }

        [Fact]
        public void PresentationRefresh_DoesNotModifyDirty_Or_StateVersion()
        {
            using var session = new TestSession();
            bool presentationFired = false;
            bool stateChangedFired = false;

            session.PresentationRefreshRequested += () => presentationFired = true;
            session.StateChanged += () => stateChangedFired = true;

            session.RequestPresentationRefresh();

            Assert.True(presentationFired);
            Assert.False(stateChangedFired);
            Assert.False(session.IsDirty);
            Assert.Equal(0, session.StateVersion);
        }

        [Fact]
        public void CoalescedMutations_FlushOnce_IncrementsSaveCountOnce()
        {
            using var session = new TestSession();

            // Perform 5 successive mutations in a burst
            for (int i = 0; i < 5; i++)
            {
                session.PerformDirectMutation();
            }

            Assert.True(session.IsDirty);
            Assert.Equal(5, session.StateVersion);
            Assert.Equal(0, session.SaveCount);

            // Simulation step ends -> single Save() flush
            session.Save();

            Assert.False(session.IsDirty);
            Assert.Equal(1, session.SaveCount);
            Assert.Equal(5, session.StateVersion);

            // Second Save() with clean state is a no-op
            session.Save();
            Assert.Equal(1, session.SaveCount);
        }

        [Fact]
        public void ClearDirty_LeavesVersionIntact_ClearsDirty()
        {
            using var session = new TestSession();
            session.PerformDirectMutation();
            session.PerformDirectMutation();

            Assert.True(session.IsDirty);
            Assert.Equal(2, session.StateVersion);

            session.ClearDirty();

            Assert.False(session.IsDirty);
            Assert.Equal(2, session.StateVersion);
        }

        [Fact]
        public void SaveStore_FailedOrNoOpActions_ProduceZeroWrites()
        {
            SaveEnvelopeHelper.ResetAtomicWriteCounter();
            Assert.Equal(0, SaveEnvelopeHelper.TotalAtomicWrites);

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var log = NullLog.Instance;
            string savePath = Path.Combine(_testTempDir, "test_session_save.json");

            var store = new SaveStore<ApprenticeshipState>(
                "test_session_save.json",
                files,
                json,
                log,
                () => _testTempDir,
                "TestStore");

            // Null state save produces 0 writes and returns false
            bool nullSaved = store.TrySave(null!);
            Assert.False(nullSaved);
            Assert.Equal(0, store.WriteCount);
            Assert.Equal(0, SaveEnvelopeHelper.TotalAtomicWrites);

            // Real save writes once atomically
            var state = new ApprenticeshipState();
            bool ok = store.TrySave(state);
            Assert.True(ok);
            Assert.Equal(1, store.WriteCount);
            Assert.Equal(1, SaveEnvelopeHelper.TotalAtomicWrites);
            Assert.True(files.FileExists(savePath));
        }

        [Fact]
        public void DomainSession_Apprenticeship_RejectedPair_EmitsZeroChanges()
        {
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var system = new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations, NullLog.Instance);
            using var host = new TestDomainHostSession(system);

            bool stateChanged = false;
            host.StateChanged += () => stateChanged = true;

            // Mentor has 0 skill XP -> Rejected with mentor_unqualified
            var res = host.StartPair("unskilled_dweller", "apprentice_dweller", "skill_medical");

            Assert.False(res.IsSuccess);
            Assert.Equal("mentor_unqualified", res.FailureCode);
            Assert.False(host.IsDirty);
            Assert.Equal(0, host.StateVersion);
            Assert.False(stateChanged);
        }

        [Fact]
        public void DomainSession_AirlockSecurity_NoIncident_Resolve_EmitsZeroChanges()
        {
            var system = new AirlockSecuritySystem(new SeededRng(42), NullLog.Instance);
            using var host = new TestAirlockHostSession(system);

            bool stateChanged = false;
            host.StateChanged += () => stateChanged = true;

            // No visitor incident active -> ResolveIncident rejected with no_incident
            var res = host.ResolveIncident(VisitorDecision.Admit);

            Assert.False(res.IsSuccess);
            Assert.Equal("no_incident", res.FailureCode);
            Assert.False(host.IsDirty);
            Assert.Equal(0, host.StateVersion);
            Assert.False(stateChanged);
        }
    }
}
