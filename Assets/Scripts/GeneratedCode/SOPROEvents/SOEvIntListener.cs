using System;
using UnityEngine;
namespace SOPRO
{
    /// <summary>
    /// Listener for Scriptable Object event
    /// </summary>
    [Serializable]
    public class SOEvIntListener : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Event to register with.")]
        private SOEvInt Event;
        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")]
        private UnEvInt Actions;

        /// <summary>
        /// Adds listener to event
        /// </summary>
        protected void RegisterToEvent()
        {
            Event.AddListener(this);
        }
        /// <summary>
        /// Removes listener from event
        /// </summary>
        protected void UnregisterToEvent()
        {
            Event.RemoveListener(this);
        }
				protected virtual void Awake()
        {
            Event.AddListener(this);
        }
		protected virtual void OnDestroy()
        {
            Event.RemoveListener(this);
        }
		        /// <summary>
        /// Invokes unity event
        /// </summary>
        internal void OnEventRaised(int Value0)
        {
            Actions.Invoke(Value0);
        }
    }
}
