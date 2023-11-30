using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VR
{
    public class HandAnimationController : MonoBehaviour
    {
        public const string GRIP_ANIMATION = "Grip";

        [SerializeField] private InputActionProperty triggerAnimationAction;
        [SerializeField] private Animator handAnimator;

        private float _gripValue;

        void Update()
        {
            _gripValue = triggerAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat(GRIP_ANIMATION, _gripValue);
        }
    }
}