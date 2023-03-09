using System.Collections.Generic;
using Salvage.TOC.Classes;
using UnityEngine;
using UnityEngine.Events;

namespace Salvage.TOC.Components
{
    public class TriggerEnterEvent : MonoBehaviour
    {
        private TriggerEnterEventArgs triggerEnterEventArgs;

        #region SerializeFields

        [SerializeField] private Collider triggerCollider;
        [SerializeField] private UnityEvent<TriggerEnterEventArgs> triggerEnter;
        [SerializeField] private List<string> tagsToCheck;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            triggerEnterEventArgs = new TriggerEnterEventArgs(triggerCollider);
        }

        private void Reset()
        {
            triggerCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (triggerEnter == null || !collider.HasAnyTag(tagsToCheck))
            {
                return;
            }

            triggerEnterEventArgs.Collider = collider;
            triggerEnter.Invoke(triggerEnterEventArgs);
        }

        #endregion

    }
}