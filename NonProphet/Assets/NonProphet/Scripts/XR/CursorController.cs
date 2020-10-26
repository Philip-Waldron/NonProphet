using System;
using NonProphet.Scripts.Utilities;
using UnityEngine;

namespace NonProphet.Scripts.XR
{
    public class CursorController : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private GameObject cursorPrefab;

        internal const float Range = 100f;
        private Cursor leftCursor, rightCursor;

        private void Start()
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
            cursor.name = $"[Cursor / {check.ToString()}]";
            cursor.transform.SetParent(transform);
            cursor.CreateCursor(check);
            return cursor;
        }
    }
}
