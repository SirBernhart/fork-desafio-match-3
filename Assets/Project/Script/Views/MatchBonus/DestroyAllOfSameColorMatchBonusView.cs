using System.Collections;
using DG.Tweening;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views.MatchBonus
{
    public class DestroyAllOfSameColorMatchBonusView : MatchBonusView
    {
        [SerializeField] private float _fadeOutStart = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.2f;
        [SerializeField] private Image _vfxImage;
        [SerializeField] private  float angle1 = 90f;
        [SerializeField] private  float angle2 = -90f;
        
        private bool isAngle1 = true;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        
        public override void SetupAndPlay(MatchModel matchModel, TileSpotView[][] tileSpotViews)
        {
            base.SetupAndPlay(matchModel, tileSpotViews);
            Sequence sequence = DOTween.Sequence();
            StartCoroutine(Animate());
            sequence.Insert(_fadeOutStart, DOTween.ToAlpha(() => _vfxImage.color, x => _vfxImage.color = x, 0f, _fadeOutDuration));
        }

        private IEnumerator Animate()
        {
            _vfxImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle1);
            
            while (true)
            {
                if (isAngle1)
                {
                    _vfxImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle2);
                }
                else
                {
                    _vfxImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle1);
                }
                isAngle1 = !isAngle1;
                yield return _waitForSeconds;
            }
        }
    }
}
