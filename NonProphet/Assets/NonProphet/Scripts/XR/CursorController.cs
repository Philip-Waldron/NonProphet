using System;
using UnityEngine;

namespace NonProphet.Scripts.XR
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private GameObject cursorPrefab;
        [SerializeField, Range(1, 250)] private float distance = 100f;

        private Cursor leftCursor, rightCursor;
        
        private float ScaleFactor => transform.localScale.x;

        private void Awake()
        {
            leftCursor = CreateCursor(XRInputController.Check.Left);
            rightCursor = CreateCursor(XRInputController.Check.Right);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        private Cursor CreateCursor(XRInputController.Check check)
        {
            Cursor cursor = Instantiate(cursorPrefab).GetComponent<Cursor>();
            cursor.transform.localScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
            cursor.name = $"[Cursor / {check.ToString()}]";
            cursor.transform.SetParent(transform);
            cursor.CreateCursor(check, distance, ScaleFactor);
            return cursor;
        }
    }
}
