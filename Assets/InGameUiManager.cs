using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class InGameUiManager : MonoBehaviour
{
    public TMP_Text m_pointText;
    public TMP_Text m_jumpText;

    public RectTransform m_leveText;
    private int m_score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int _points)
    {
        m_score += _points;
        m_pointText.SetText(m_score+" points");
    }
    public void UpdateJump(int _lives)
    {
        
        m_jumpText.SetText(_lives+" JUMPS");
    }

    public void DisplayLevelText(int _level)
    {
        m_leveText.GetComponent<TMP_Text>().SetText("Level "+_level);
        var sequence = DOTween.Sequence();
        sequence.Join(m_leveText.DOScale(Vector3.one, .8f));
        sequence.Join(m_leveText.GetComponent<CanvasGroup>().DOFade(1, .5f));
        sequence.AppendInterval(.5f);
        sequence.Append(m_leveText.GetComponent<CanvasGroup>().DOFade(0, .3f));
        sequence.OnComplete(ResetLevelText);
    }

    private void ResetLevelText()
    {
        m_leveText.localScale = Vector3.zero;
        
    }
}
