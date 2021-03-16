using System;
using _Project.Scripts.ScriptableObjects.Achievements;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class UIAchievementDisplay : MonoBehaviour
    {
        public bool m_testAnimation;
        public TMP_Text m_titleText;
        public TMP_Text m_descriptionText;
        public RectTransform m_notification;
        public bool IsCurrentlyDisplayingAchievementNotification { get; private set; } 

        public void DisplayAchievementAlert(Achievement _achievement)
        {
            IsCurrentlyDisplayingAchievementNotification = true;
            print("Start display: " + _achievement.m_name);
            SetText(_achievement.m_name, _achievement.m_description);
            StartAnimation();

        }

        private void CompleteAchievementDisplay()
        {
            print("CompleteAchievementDisplay");
            IsCurrentlyDisplayingAchievementNotification = false;
        }

        private void Update()
        {
            if (m_testAnimation)
            {
                m_testAnimation = false;
                SetText("Test Title","test description");
                StartAnimation();
            }
        }

        private void SetText(string _title, string _description)
        {
            m_titleText.SetText(_title);
            m_descriptionText.SetText(_description);
        }
        private void StartAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(m_notification.DOLocalMoveY(-50, 1));
            sequence.AppendInterval(2);
            sequence.Append(m_notification.DOLocalMoveY(300, 1));
            sequence.AppendInterval(2);
            sequence.OnComplete(CompleteAchievementDisplay);
        }
    }
}