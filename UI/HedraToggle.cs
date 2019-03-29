using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HedraLibrary.UI {

    [Serializable] public class ToggleEvent : UnityEvent<HedraToggle> { }
    [Serializable] public class ToggleStateEvent : UnityEvent<bool> { }
    public class HedraToggle : MonoBehaviour {

        [SerializeField] protected bool set;
        [SerializeField] protected bool interactable;
        [SerializeField] protected HedraToggleGroup group;
        
        [SerializeField] public ToggleStateEvent OnValueChanged = new ToggleStateEvent();
        [SerializeField] public ToggleEvent OnSet = new ToggleEvent();
        [SerializeField] public ToggleEvent OnUnset = new ToggleEvent();
        [SerializeField] public ToggleEvent OnHoverEnter = new ToggleEvent();
        [SerializeField] public ToggleEvent OnHoverExit = new ToggleEvent();
        [SerializeField] public ToggleEvent OnEnabled = new ToggleEvent();
        [SerializeField] public ToggleEvent OnDisabled = new ToggleEvent();
        
        public bool IsSet {
            get { return set; }
            protected set {
                if (!interactable) {
                    return;
                }

                if (value) {
                    Set();

                    if (Group != null) {
                        Group.NotifyToggleSelected(this);
                    }
                } else {
                    Unset();

                    if (Group != null) {
                        Group.NotifyToggleDeselected(this);
                    }
                }
                controlSet = value;
            }
        }

        public bool IsInteractable {
            get { return interactable; }
            protected set {
                if (value) {
                    Enable();
                } else {
                    Disable();
                }
                controlInteractable = value;
            }
        }

        public HedraToggleGroup Group {
            get { return group; }
            set {
                group.Remove(this);
                group = value;
                group.Register(this);
                controlGroup = value;
            }
        }

        HedraToggleGroup controlGroup;
        bool controlSet;
        bool controlInteractable;

        void Awake() {
            Init();
            AssignMouseEvents();
        }

        void Init() {
            controlGroup = group;
            if (group != null) {
                Group = group;
            }
        }

        void Start() {
            SetUp();
        }

        void SetUp() {
            IsSet = set;
            IsInteractable = interactable;
        }

        void Update() {
            ControlInspector();
        }

        #region Control
        void ControlInspector() {
            if (controlInteractable != interactable) {
                IsInteractable = interactable;
            }

            if (controlGroup != Group) {
                Group = group;
                return; // Group sets on or off.
            }

            if (controlSet == set) {
                return;
            }

            if (!IsInteractable) {
                set = controlSet;
            }

            IsSet = set;
        }
        
        public void Set() {
            if (!interactable) {
                return;
            }

            set = true;
            controlSet = set;

            OnValueChanged.Invoke(set);
            OnSet.Invoke(this);
        }

        public void Unset() {
            if (!interactable) {
                return;
            }

            set = false;
            controlSet = set;

            OnValueChanged.Invoke(set);
            OnUnset.Invoke(this);
        }

        public void HoverEnter() {
            OnHoverEnter.Invoke(this);
        }

        public void HoverExit() {
            OnHoverExit.Invoke(this);
        }

        public void Enable() {
            interactable = true;
            controlInteractable = interactable;

            OnEnabled.Invoke(this);
        }

        public void Disable() {
            interactable = false;
            controlInteractable = interactable;

            OnDisabled.Invoke(this);
        }
        #endregion

        #region Mouse Control 
        void AssignMouseEvents() {
            Hedra.AssignEvent(this.gameObject, EventTriggerType.PointerEnter, (data) => { OnMouseEnter(); });
            Hedra.AssignEvent(this.gameObject, EventTriggerType.PointerExit, (data) => { OnMouseExit(); });
            Hedra.AssignEvent(this.gameObject, EventTriggerType.PointerClick, (data) => { OnMouseClick(); });
        }

        void OnMouseEnter() {
            HoverEnter();
        }

        void OnMouseExit() {
            HoverExit();
        }

        void OnMouseClick() {
            IsSet = !IsSet;
        }
        #endregion
    }
}