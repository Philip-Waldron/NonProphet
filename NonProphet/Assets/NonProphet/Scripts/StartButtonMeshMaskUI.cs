using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Not used any more!
public class StartButtonMeshMaskUI : MaskableGraphic
{
    public AnimationCurve AnimationCurve;
    public float SecondMaskModifier = 75;
    public float FinalPositionModifier = 130;
    private float _currentPositionModifier = 0;
    private float _animationTime;
    private Coroutine _coroutine;

    public Mask mask;

    private Vector3 _vector00 = new Vector3(0, 0);
    private Vector3 _vector01 = new Vector3(0, 50);
    private Vector3 _vector10 = new Vector3(50, 0);
    private Vector3 _vector11 = new Vector3(50, 50);

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        vertexHelper.AddUIVertexQuad(new UIVertex[]
        {
            new UIVertex { position = _vector00 + new Vector3(_currentPositionModifier, 0), color = Color.green },
            new UIVertex { position = _vector01 + new Vector3(_currentPositionModifier, 0), color = Color.green },
            new UIVertex { position = _vector11 + new Vector3(_currentPositionModifier, 0), color = Color.green },
            new UIVertex { position = _vector10 + new Vector3(_currentPositionModifier, 0), color = Color.green },
        });

        if (SecondMaskModifier != 0)
        {
            vertexHelper.AddUIVertexQuad(new UIVertex[]
            {
                new UIVertex { position = _vector00 + new Vector3(SecondMaskModifier + _currentPositionModifier, 0), color = Color.green },
                new UIVertex { position = _vector01 + new Vector3(SecondMaskModifier + _currentPositionModifier, 0), color = Color.green },
                new UIVertex { position = _vector11 + new Vector3(SecondMaskModifier + _currentPositionModifier, 0), color = Color.green },
                new UIVertex { position = _vector10 + new Vector3(SecondMaskModifier + _currentPositionModifier, 0), color = Color.green },
            });
        }
    }

    public void PlayAnimation()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(nameof(Animate));
    }

    private IEnumerator Animate()
    {
        //mask.enabled = true;
        //enabled = true;
        _animationTime = 0;
        _currentPositionModifier = 0;

        while (_currentPositionModifier != FinalPositionModifier)
        {
            _animationTime += Time.deltaTime;
            _currentPositionModifier = Mathf.Lerp(0, FinalPositionModifier, AnimationCurve.Evaluate(_animationTime));
            SetVerticesDirty();
            yield return null;
        }

        //mask.enabled = false;
        //enabled = false;
    }
}
