using System.Collections.Generic;
using DG.Tweening;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views.MatchBonus
{
    public class DestroyRandomTilesMatchBonusView : MatchBonusView
    {
        [SerializeField] private float _endScale = 1.1f;
        [SerializeField] private float _scaleUpduration = 0.5f;
        [SerializeField] private float _fadeOutStartTime = 0.25f;
        [SerializeField] private float _fadeOutDuration = 0.2f;
        [SerializeField] private Image _explosionImage;
        [SerializeField] private GameObject _tileDestroyerVfxPrefab;

        private List<GameObject> _vfxInstances = new();
        
        public override void SetupAndPlay(MatchModel matchModel, TileSpotView[][] tileSpotViews)
        {
            base.SetupAndPlay(matchModel, tileSpotViews);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_explosionImage.transform.DOScale(_endScale, _scaleUpduration).SetEase(Ease.OutExpo));
            sequence.Insert(_fadeOutStartTime, DOTween.ToAlpha(() => _explosionImage.color, x => _explosionImage.color = x, 0f, _fadeOutDuration));
            
            foreach (Vector2Int bonusTile in MatchModel.MatchBonusTiles)
            {
                _vfxInstances.Add(Instantiate(_tileDestroyerVfxPrefab, TileSpotViews[bonusTile.y][bonusTile.x].transform));
            }
        }

        private void OnDestroy()
        {
            foreach (GameObject vfxInstance in _vfxInstances)
            {
                Destroy(vfxInstance);
            }
        }
    }
}
