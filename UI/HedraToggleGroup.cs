using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HedraLibrary.UI {

    [Serializable] public class ToggleGroupEvent : UnityEvent<HedraToggle> { }
    public class HedraToggleGroup : MonoBehaviour {

        public enum ToggleGroupBehaviour {
            RadioButton, 
            Checkbox
        }

        public ToggleGroupBehaviour behaviour;

        List<HedraToggle> toggles = new List<HedraToggle>();
        List<HedraToggle> setToggles = new List<HedraToggle>();
        
        public void Register(HedraToggle toggle) {
            if (toggles.Contains(toggle)) {
                return;
            }
            toggles.Add(toggle);

            if (!toggle.IsSet) {
                return;
            }

            if (behaviour == ToggleGroupBehaviour.RadioButton && setToggles.Count > 0) {
                toggle.Unset();
            }

            if (toggle.IsSet) {
                setToggles.Add(toggle);
            }
        }

        public void Remove(HedraToggle toggle) {
            toggles.Remove(toggle);
        }

        public void NotifyToggleSelected(HedraToggle toggle) {
            if (behaviour == ToggleGroupBehaviour.RadioButton) {
                UnsetTogglesExcept(toggle);
                setToggles.Clear();
            }
            setToggles.Add(toggle);
        }
        
        public void NotifyToggleDeselected(HedraToggle toggle) {
            if (behaviour == ToggleGroupBehaviour.RadioButton && setToggles.Count <= 1) {
                toggle.Set();
                return;
            }
            setToggles.Remove(toggle);
        }
        
        #region Control
        public void EnableGroup() {
            for (int i = 0; i < toggles.Count; i++) {
                toggles[i].Enable();
            }
        }

        public void DisableGroup() {
            for (int i = 0; i < toggles.Count; i++) {
                toggles[i].Disable();
            }
        }
                
        public void Set(int index) {
            if (index < 0 || index >= toggles.Count) {
                return;
            }

            HedraToggle target = toggles[index];
            if (!target.IsSet) {
                target.Set();
            }

            if (behaviour == ToggleGroupBehaviour.RadioButton) {
                UnsetTogglesExcept(target);
                setToggles.Clear();
            }
            setToggles.Add(target);
        }

        public void Unset(int index) {
            if (index < 0 || index >= toggles.Count) {
                return;
            }

            if (behaviour == ToggleGroupBehaviour.RadioButton) {
                return;
            }

            HedraToggle target = toggles[index];
            if (!target.IsSet) {
                return;
            }

            target.Unset();
            setToggles.Remove(target);            
        }

        public void SetAll() {
            if (behaviour == ToggleGroupBehaviour.RadioButton) {
                return;
            }

            for (int i = 0; i < toggles.Count; i++) {
                toggles[i].Set();
            }
        }

        public void UnsetAll() {
            if (behaviour == ToggleGroupBehaviour.RadioButton) {
                return;
            }

            for (int i = 0; i < toggles.Count; i++) {
                toggles[i].Unset();
            }
        }

        protected void UnsetTogglesExcept(HedraToggle exception) {
            for (int i = 0; i < toggles.Count; i++) {
                if (toggles[i] != exception) {
                    toggles[i].Unset();
                }
            }
        }
        #endregion
    }

}