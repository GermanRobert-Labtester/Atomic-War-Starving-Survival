using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ClassroomState
    {
        public string moduleId = "shelter_module_classroom";
        public int maxChildren = 3;
        public bool requiresTeacher = true;
        public string teacherId = "";
        public List<string> enrolledChildIds = new List<string>();
    }

    public class ShelterModule_Classroom
    {
        public event Action<string> OnChildEnrolled;
        public event Action<string> OnNoiseHalted;
        public event Action<string, string> OnStatIncreased;

        private readonly ClassroomState _state;

        public ShelterModule_Classroom()
        {
            _state = new ClassroomState();
        }

        public void AssignTeacher(string teacherId)
        {
            _state.teacherId = teacherId;
        }

        public bool Enroll(string childId, string teacherId)
        {
            if (string.IsNullOrEmpty(teacherId) && _state.requiresTeacher)
                return false;
            if (_state.enrolledChildIds.Count >= _state.maxChildren)
                return false;
            if (_state.enrolledChildIds.Contains(childId))
                return false;

            if (!string.IsNullOrEmpty(teacherId))
                _state.teacherId = teacherId;

            _state.enrolledChildIds.Add(childId);
            OnChildEnrolled?.Invoke(childId);
            return true;
        }

        public void TickDay()
        {
            if (_state.requiresTeacher && string.IsNullOrEmpty(_state.teacherId))
                return;

            for (int i = 0; i < _state.enrolledChildIds.Count; i++)
            {
                string childId = _state.enrolledChildIds[i];

                OnStatIncreased?.Invoke(childId, "intelligence");
                OnStatIncreased?.Invoke(childId, "engineering");
                OnNoiseHalted?.Invoke(childId);
            }
        }

        public bool IsEnrolled(string childId)
        {
            return _state.enrolledChildIds.Contains(childId);
        }

        public ClassroomState CaptureState() => _state;

        public void RestoreState(ClassroomState state)
        {
            _state.moduleId = state.moduleId;
            _state.maxChildren = state.maxChildren;
            _state.requiresTeacher = state.requiresTeacher;
            _state.teacherId = state.teacherId;
            _state.enrolledChildIds = new List<string>(state.enrolledChildIds);
        }
    }
}
