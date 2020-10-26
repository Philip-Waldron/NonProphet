using System;
using UnityEngine;
using Transition = NonProphet.Scripts.XR.XRInputController.InputEvents.InputEvent.Transition;

namespace NonProphet.Scripts.XR
{
    public class Locomotion : MonoBehaviour
    {
        private bool position = true, rotation = false;
        private static XRInputController XRInputController => Reference.XRInputController();

        private GameObject rotationParent, displacementParent;
        private XRInputController.Check activeHand, inactiveHand;
        private bool locomotion;
        private Vector3 startHandPosition, startHandRotation, startPlayerPosition, startPlayerRotation;

        private void Awake()
        {
            rotationParent = new GameObject { name = "[Rotation Parent]" };
            displacementParent = new GameObject { name = "[Displacement Parent]" };
        }

        private void Update()
        {
            rotationParent.transform.position = XRInputController.NormalisedPosition(activeHand, true);
                
            if (XRInputController.InputEvent(XRInputController.Event.Grab).State(XRInputController.NonDominantHand(), Transition.Down))
            {
                LocomotionStart(XRInputController.NonDominantHand());
                return;
            }
            if (XRInputController.InputEvent(XRInputController.Event.Grab).State(XRInputController.DominantHand(), Transition.Down))
            {
                LocomotionStart(XRInputController.DominantHand());
                return;
            }
            if (XRInputController.InputEvent(XRInputController.Event.Grab).State(activeHand, Transition.Up) && XRInputController.Grab(inactiveHand) && locomotion)
            {
                LocomotionStart(inactiveHand);
                return;
            }
            if (XRInputController.InputEvent(XRInputController.Event.Grab).State(activeHand, Transition.Up) && locomotion)
            {
                LocomotionEnd();
            }
            
            if (!locomotion) return;
            transform.position = position ? DisplacedPosition() : startPlayerPosition;
            rotationParent.transform.eulerAngles = rotation ? DisplacedRotation() : startPlayerRotation;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LocomotionStart(XRInputController.Check hand)
        {
            if (!locomotion)
            {
                displacementParent.transform.position = XRInputController.NormalisedPosition(hand);
                transform.SetParent(displacementParent.transform);
            }
            
            locomotion = true;
            activeHand = hand;
            inactiveHand = hand == XRInputController.Check.Left
                ? XRInputController.Check.Right
                : XRInputController.Check.Left;

            startHandPosition = XRInputController.NormalisedPosition(activeHand, local: true);
            startHandRotation = XRInputController.Rotation(activeHand, local: true);
            
            startPlayerPosition = transform.position;
            startPlayerRotation = rotationParent.transform.eulerAngles;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LocomotionEnd()
        {
            locomotion = false;
            transform.SetParent(null);
        }
        /// <summary>
        /// Calculates the absolute normalised distance travelled by the active hand 
        /// </summary>
        /// <returns></returns>
        private Vector3 Displacement()
        {
            return (XRInputController.NormalisedPosition(activeHand, local: true) - startHandPosition) * XRInputController.ScaleFactor();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 DisplacedPosition()
        {
            return startPlayerPosition - Displacement();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private float Alignment()
        {
            return XRInputController.Rotation(activeHand, local: true).y - startHandRotation.y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Vector3 DisplacedRotation()
        {
            return new Vector3(0, startPlayerRotation.y - Alignment(), 0);
        }
    }
}
