using System.Collections;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Views.MatchBonus
{
    public class MatchBonusView : MonoBehaviour
    {
        [SerializeField] private float _vfxDuration = 1;
        protected MatchModel MatchModel;
        protected TileSpotView[][] TileSpotViews;

        public virtual void SetupAndPlay(MatchModel matchModel, TileSpotView[][] tileSpotViews)
        {
            MatchModel = matchModel;
            TileSpotViews = tileSpotViews;
            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(_vfxDuration);
            Destroy(gameObject);
        }
    }
}
