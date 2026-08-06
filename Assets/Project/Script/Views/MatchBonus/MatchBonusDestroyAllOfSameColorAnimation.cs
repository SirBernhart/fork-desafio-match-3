using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class MatchBonusDestroyAllOfSameColorAnimation : MonoBehaviour
    {
        [SerializeField] private float _fadeOutStart = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.2f;
        [SerializeField] private Image _image;
        [SerializeField] private  float angle1 = 90f;
        [SerializeField] private  float angle2 = -90f;
        
        private bool isAngle1 = true;
        
        private void Awake()
        {
            Sequence sequence = DOTween.Sequence();
            StartCoroutine(Animate());
            sequence.Insert(_fadeOutStart, DOTween.ToAlpha(() => _image.color, x => _image.color = x, 0f, _fadeOutDuration));
        }

        private IEnumerator Animate()
        {
            _image.rectTransform.localRotation = Quaternion.Euler(0, 0, angle1);
            
            while (true)
            {
                if (isAngle1)
                {
                    _image.rectTransform.localRotation = Quaternion.Euler(0, 0, angle2);
                }
                else
                {
                    _image.rectTransform.localRotation = Quaternion.Euler(0, 0, angle1);
                }
                isAngle1 = !isAngle1;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
