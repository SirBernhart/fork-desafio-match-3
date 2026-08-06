using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class MatchBonusExplosionAnimation : MonoBehaviour
    {
        [SerializeField] private float _endScale = 3f;
        [SerializeField] private float _scaleUpduration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.2f;
        
        private void Awake()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(_endScale, _scaleUpduration).SetEase(Ease.OutExpo));
            Image image = transform.GetComponent<Image>();
            sequence.Insert(_scaleUpduration/2f, DOTween.ToAlpha(() => image.color, x => image.color = x, 0f, _fadeOutDuration));
        }
    }
}
