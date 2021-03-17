using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Mono;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    public GameEvent m_startEvent;
    public StringGameEvent m_loadSceneEvent;

    public GameObject m_gameLogo;
    
    public UIButton m_startButton;
    public UIButton m_resetButton;
    
    [SerializeField]
    private string m_sceneToLoad;
    [SerializeField]
    private string m_sceneToLoad2;

    // Start is called before the first frame update
    void Start()
    {
        m_resetButton.gameObject.SetActive(false);
        #if UNITY_EDITOR
        m_resetButton.gameObject.SetActive(true);
        #endif
        m_startButton.m_clickEvent.AddListener(Close);
        m_resetButton.m_clickEvent.AddListener(ResetPlayerPrefs);
    }

    private void ResetPlayerPrefs()
    {
        DataSaveLoadUtils.SaveAchievementData("");
        DataSaveLoadUtils.SaveGameData("");
    }

    private void Close()
    {
        
        m_startButton.gameObject.SetActive(false);
        m_resetButton.gameObject.SetActive(false);
        m_loadSceneEvent.Raise(m_sceneToLoad);
        var sequence = DOTween.Sequence();
        sequence.Join(m_gameLogo.GetComponent<RectTransform>().DOScale(Vector3.zero, 1).SetEase(Ease.InElastic));
        //sequence.Join(m_gameLogo.GetComponent<Image>()
        sequence.OnComplete(StartGame);
    }

    public void StartGame()
    {
        m_startEvent.Raise();
        m_loadSceneEvent.Raise(m_sceneToLoad2);
    }

}