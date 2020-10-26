using UnityEngine;

namespace NonProphet.Scripts.Utilities
{
    public static class TransformExpanded
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Vector3 ScaleFactor(float factor)
        {
            return new Vector3(factor, factor, factor);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static void ScaleFactor(this Transform target,float factor)
        {
            target.localScale =  new Vector3(factor, factor, factor);
        }
    }
}
