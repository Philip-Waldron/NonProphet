using System;
using DG.Tweening;
using UnityEngine;
using Transition = NonProphet.Scripts.XR.XRInputController.InputEvents.InputEvent.Transition;

namespace NonProphet.Scripts.XR
{
    public class Cursor : MonoBehaviour
    {
        private enum CursorState { Default, Grab, Select }
        
        [SerializeField] private Sprite defaultState, grabState, selectState;
        [SerializeField, Range(0f, 1f)] private float defaultOffset, grabOffset, selectOffset, duration;

        private float offset, range;
        private CursorState cursorState;
        private XRInputController.Check check;
        
        private Vector3[] positions;

        private SpriteRenderer CursorSpriteRenderer => GetComponentInChildren<SpriteRenderer>();
        private LineRenderer CursorLineRenderer => GetComponentInChildren<LineRenderer>();
        
        private static XRInputController XRInputController => Reference.XRInputController();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handedness"></param>
        /// <param name="distance"></param>
        /// <param name="scale"></param>
        public void CreateCursor(XRInputController.Check handedness, float distance, float scale)
        {
            check = handedness;
            range = distance;
            CursorLineRenderer.useWorldSpace = false;
            CursorLineRenderer.startWidth *= scale;
            CursorLineRenderer.endWidth *= scale;
        }
        private void Update()
        {
            CursorPosition();
            LineRendererState();
            
            switch (cursorState)
            {
                case CursorState.Default when XRInputController.InputEvent(XRInputController.Event.Grab).State(check, Transition.Down):
                    SetCursorState(CursorState.Grab);
                    break;
                case CursorState.Default when XRInputController.InputEvent(XRInputController.Event.Select).State(check, Transition.Down):
                    SetCursorState(CursorState.Select);
                    break;
                case CursorState.Grab when XRInputController.InputEvent(XRInputController.Event.Grab).State(check, Transition.Up):
                    SetCursorState(CursorState.Default);
                    break;
                case CursorState.Select when XRInputController.InputEvent(XRInputController.Event.Select).State(check, Transition.Up):
                    SetCursorState(CursorState.Default);
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CursorPosition()
        {
            Transform cursorTransform = transform;
            if (XRInputController.Hover(check, Vector3.down, out RaycastHit hit, range))
            {
                cursorTransform.position = hit.point;
                cursorTransform.up = hit.normal;
            }
            else
            {
                cursorTransform.position = Vector3.zero;
                cursorTransform.up = Vector3.zero;
            }
        }
        private void LineRendererState()
        {
            positions = new[] {Vector3.zero, new Vector3(0, offset, 0)};
            CursorLineRenderer.SetPositions(positions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void SetCursorState(CursorState state)
        {
            switch (state)
            {
                case CursorState.Default:
                    cursorState = CursorState.Default;
                    DOTween.To(() => offset, x => offset = x, defaultOffset, duration);
                    CursorSpriteRenderer.sprite = defaultState;
                    return;
                case CursorState.Grab:
                    cursorState = CursorState.Grab;
                    CursorSpriteRenderer.sprite = grabState;
                    DOTween.To(() => offset, x => offset = x, grabOffset, duration);
                    return;
                case CursorState.Select:
                    cursorState = CursorState.Select;
                    CursorSpriteRenderer.sprite = selectState;
                    DOTween.To(() => offset, x => offset = x, selectOffset, duration);
                    break;
                default:
                    return;
            }
        }
    }
}
