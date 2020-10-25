using System;
using System.Collections;
using UnityEngine;

public class AnimateTransform : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public enum CoordinateSpace
    {
        Local,
        World
    }

    public Transform Target;
    public AnimationCurve AnimationCurve;
    public Axis _Axis;
    public CoordinateSpace _CoordinateSpace;
    public bool AlongRotation;

    private Coroutine _coroutine;

    public void PlayAnimation()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Animate(_Axis));
    }

    private IEnumerator Animate(Axis axis)
    {
        float animationTime = AnimationCurve.keys[0].time;
        float previousAnimationValue = 0;

        while (animationTime < AnimationCurve.keys[AnimationCurve.length - 1].time)
        {
            animationTime += Time.deltaTime;
            float animationValue = AnimationCurve.Evaluate(animationTime);
            float positionModifier = animationValue - previousAnimationValue;
            previousAnimationValue = animationValue;

            switch (axis)
            {
                case Axis.X:
                    if (_CoordinateSpace == CoordinateSpace.Local)
                    {
                        if (AlongRotation)
                        {
                            Target.localPosition += Target.forward * positionModifier;
                        }
                        else
                        {
                            Target.localPosition += Vector3.forward * positionModifier;
                        }
                    }
                    else
                    {
                        if (AlongRotation)
                        {
                            Target.position += Target.forward * positionModifier;
                        }
                        else
                        {
                            Target.position += Vector3.forward * positionModifier;
                        }
                    }
                    break;
                case Axis.Y:
                    if (_CoordinateSpace == CoordinateSpace.Local)
                    {
                        if (AlongRotation)
                        {
                            Target.localPosition += Target.up * positionModifier;
                        }
                        else
                        {
                            Target.localPosition += Vector3.up * positionModifier;
                        }
                    }
                    else
                    {
                        if (AlongRotation)
                        {
                            Target.position += Target.up * positionModifier;
                        }
                        else
                        {
                            Target.position += Vector3.up * positionModifier;
                        }
                    }
                    break;
                case Axis.Z:
                    if (_CoordinateSpace == CoordinateSpace.Local)
                    {
                        if (AlongRotation)
                        {
                            Target.localPosition += Target.right * positionModifier;
                        }
                        else
                        {
                            Target.localPosition += Vector3.right * positionModifier;
                        }
                    }
                    else
                    {
                        if (AlongRotation)
                        {
                            Target.position += Target.right * positionModifier;
                        }
                        else
                        {
                            Target.position += Vector3.right * positionModifier;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }

            yield return null;
        }
    }
}
