using System;
using Salvage.TOC.Classes;
using UnityEngine;

namespace Salvage.TOC.Components
{
    public class Door : MonoBehaviour
    {
        private DateTime lastStateChangeTime;

        #region SerializeFields

        [SerializeField] private Vector3 openLocalRotation;
        [SerializeField] private Vector3 closedLocalRotation;
        [SerializeField] private float openAndCloseTimeSeconds;
        [SerializeField] private DoorState state;

        #endregion

        #region Unity Messages

        private void FixedUpdate()
        {
            switch (state)
            {
                case DoorState.Opening:
                    InterpolateTransform(openLocalRotation, DoorState.Open);
                    break;
                case DoorState.Closing:
                    InterpolateTransform(closedLocalRotation, DoorState.Closed);
                    break;
            }
        }

        #endregion

        #region Private Methods

        private void ChangeState(DoorState targetState)
        {
            lastStateChangeTime = DateTime.Now;
            state = targetState;
        }

        private void InterpolateTransform(Vector3 targetLocalRotation, DoorState stateWhenFinished)
        {
            TimeSpan elapsed = DateTime.Now - lastStateChangeTime;
            float progress = (float)elapsed.TotalSeconds / openAndCloseTimeSeconds;

            if (progress < 1f)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetLocalRotation), progress);
                return;
            }

            transform.localRotation = Quaternion.Euler(targetLocalRotation);
            ChangeState(stateWhenFinished);
        }

        #endregion

        #region Public Methods

        public void OnPlayerEntered(TriggerEnterEventArgs triggerEnterEventArgs)
        {
            switch (state)
            {
                case DoorState.Open:
                    ChangeState(DoorState.Closing);
                    break;
                case DoorState.Closed:
                    ChangeState(DoorState.Opening);
                    break;
            }
        }

        #endregion

        #region Inner Classes

        public enum DoorState
        {
            Open,
            Opening,
            Closed,
            Closing
        }

        #endregion
    }
}