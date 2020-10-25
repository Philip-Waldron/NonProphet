using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace NonProphet.Scripts.XR
{
    public class XRInputController : MonoBehaviour
    {
        [SerializeField] private Check dominantHand = Check.Right;
        [SerializeField] private Transform headTransform, leftTransform, rightTransform;
        [SerializeField] private XRController leftXRController, rightXRController;

        private InputDevice LeftInputDevice => leftXRController.inputDevice;
        private InputDevice RightInputDevice => rightXRController.inputDevice;

        [HideInInspector] public InputEvents grabEvents, selectEvents;
        
        [Serializable] public class InputEvents
        {
            [Serializable] public class InputEvent
            {
                public Check check;
                public enum Transition
                {
                    Down, Up
                }
                private bool buttonDown, buttonUp;
                public bool current, previous;
                /// <summary>
                /// 
                /// </summary>
                /// <param name="handedness"></param>
                public void Set(Check handedness)
                {
                    check = handedness;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="state"></param>
                public void CheckState(bool state)
                {
                    current = state;
                    buttonDown = current && !previous;
                    buttonUp = !current && previous;
                    previous = current;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="transition"></param>
                /// <returns></returns>
                public bool State(Transition transition)
                {
                    switch (transition)
                    {
                        case Transition.Down:
                            return buttonDown;
                        case Transition.Up:
                            return buttonUp;
                        default:
                            return false;
                    }
                }
            } 
            
            public InputEvent left, right;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="leftState"></param>
            /// <param name="rightState"></param>
            public void SetState(bool leftState, bool rightState)
            {
                left.CheckState(leftState);
                right.CheckState(rightState);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="check"></param>
            /// <param name="transition"></param>
            /// <returns></returns>
            public bool State(Check check, InputEvent.Transition transition)
            {
                switch (check)
                {
                    case Check.Left:
                        return left.State(transition);
                    case Check.Right:
                        return right.State(transition);
                    case Check.Head:
                        return false;
                    default:
                        return false;
                }
            }
        }

        private void Update()
        {
            grabEvents.SetState(Grab(Check.Left), Grab(Check.Right));
            selectEvents.SetState(Select(Check.Left), Select(Check.Right));
        }

        #region Private Accessors

        public enum Check
        {
            Left, 
            Right, 
            Head
        }
        public enum Event
        {
            Grab, 
            Select
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Check DominantHand()
        {
            return dominantHand;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Check NonDominantHand()
        {
            return dominantHand == Check.Right ? Check.Left : Check.Right;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public InputEvents InputEvent(Event eventType)
        {
            switch (eventType)
            {
                case Event.Grab:
                    return grabEvents;
                case Event.Select:
                    return selectEvents;
                default:
                    return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Transform LeftTransform()
        {
            return leftTransform;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Transform RightTransform()
        {
            return rightTransform;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 LeftPosition(bool local = false)
        {
            return local ? LeftTransform().localPosition : LeftTransform().position;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 RightPosition(bool local = false)
        {
            return local ? RightTransform().localPosition : RightTransform().position;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 LeftRotation(bool local = false)
        {
            return local ? LeftTransform().localEulerAngles : LeftTransform().eulerAngles;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 RightRotation(bool local = false)
        {
            return local ? RightTransform().localEulerAngles : RightTransform().eulerAngles;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float ControllerDistance()
        {
            return Vector3.Distance(RightPosition(), LeftPosition());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Transform CameraTransform()
        {
            return headTransform;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 HeadPosition(bool local = false)
        {
            return local ? headTransform.localPosition : headTransform.position;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 HeadRotation(bool local = false)
        {
            return local ? headTransform.localEulerAngles : headTransform.eulerAngles;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private static bool Grab(InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.gripButton, out bool state);
            return state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private static bool Select(InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.triggerButton, out bool state);
            return state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private bool Primary(InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.primaryButton, out bool state);
            return state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private static bool Menu(InputDevice device)
        {
            device.TryGetFeatureValue(CommonUsages.menuButton, out bool state);
            return state;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 LeftForwardVector()
        {
            return LeftTransform().TransformVector(Vector3.forward);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 RightForwardVector()
        {
            return RightTransform().TransformVector(Vector3.forward);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 CameraForwardVector()
        {
            return headTransform.forward;
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public Transform Transform(Check check)
        {
            switch (check)
            {
                case Check.Left:
                    return LeftTransform();
                case Check.Right:
                    return RightTransform();
                case Check.Head:
                    return CameraTransform();
                default:
                    return CameraTransform();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool Grab(Check check)
        {
            switch (check)
            {
                case Check.Left:
                    return Grab(LeftInputDevice);
                case Check.Right:
                    return Grab(RightInputDevice);
                case Check.Head:
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool Select(Check check)
        {
            switch (check)
            {
                case Check.Left:
                    return Select(LeftInputDevice);
                case Check.Right:
                    return Select(RightInputDevice);
                case Check.Head:
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool Menu(Check check)
        {
            switch (check)
            {
                case Check.Left:
                    return Menu(LeftInputDevice);
                case Check.Right:
                    return Menu(RightInputDevice);
                case Check.Head:
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool Primary(Check check)
        {
            switch (check)
            {
                case Check.Left:
                    return Primary(LeftInputDevice);
                case Check.Right:
                    return Primary(RightInputDevice);
                case Check.Head:
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public Vector3 Forward(Check check)
        {
            switch (check)
            {
                case Check.Left:
                    return LeftForwardVector();
                case Check.Right:
                    return RightForwardVector();
                case Check.Head:
                    return CameraForwardVector();
                default:
                    return Vector3.zero;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public Vector3 Position(Check check, bool local = false)
        {
            switch (check)
            {
                case Check.Left:
                    return LeftPosition(local);
                case Check.Right:
                    return RightPosition(local);
                case Check.Head:
                    return HeadPosition(local);
                default:
                    return Vector3.zero;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public Vector3 Rotation(Check check, bool local = false)
        {
            switch (check)
            {
                case Check.Left:
                    return LeftRotation(local);
                case Check.Right:
                    return RightRotation(local);
                case Check.Head:
                    return HeadRotation(local);
                default:
                    return Vector3.zero;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public Vector3 NormalisedPosition(Check check, bool local = false)
        {
            switch (check)
            {
                case Check.Left:
                    return new Vector3(LeftPosition(local).x, 0, LeftPosition(local).z);
                case Check.Right:
                    return new Vector3(RightPosition(local).x, 0, RightPosition(local).z);
                case Check.Head:
                    return new Vector3(HeadPosition(local).x, 0, HeadPosition(local).z);
                default:
                    return Vector3.zero;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <param name="direction"></param>
        /// <param name="raycastHit"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public bool Hover(Check check, Vector3 direction, out RaycastHit raycastHit, float distance = 1f)
        {
            bool valid = false;
            if (Physics.Raycast(Position(check), direction, out RaycastHit hit, distance))
            {
                raycastHit = hit;
                valid = true;
            }
            raycastHit = valid ? hit : new RaycastHit();
            return valid;
        }
    }
}