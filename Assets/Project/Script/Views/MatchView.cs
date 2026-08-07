using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.Views.MatchBonus;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class MatchView : MonoBehaviour
    {
        [SerializeField] private List<MatchBonusModel> _matchBonusModels;

        public Tween AnimateTileDestruction(List<MatchModel> matchModels, GameObject[][] tiles, TileSpotView[][] tileSpots)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (MatchModel matchModel in matchModels)
            {
                Vector2 matchCenterTile = matchModel.CenterPosition;
                Transform effectCenterTileSpotTransform = tileSpots[(int)matchCenterTile.y][(int)matchCenterTile.x].transform;
                
                for (int i = 0; i < matchModel.MatchedTiles.Count; i++)
                {
                    Vector2Int position = matchModel.MatchedTiles[i];
                    GameObject tile = tiles[position.y][position.x];
                    if (tile)
                    {
                        sequence.Join(tile.transform.DOScale(0.5f, 0.1f).SetEase(Ease.OutCubic)
                            .OnComplete(() => tile.transform.DOScale(1.3f, 0.1f).SetEase(Ease.OutCubic)
                                .OnComplete(() =>
                                {
                                    Destroy(tile);
                                })));
                    }
                    
                    tiles[position.y][position.x] = null;
                }

                if (matchModel.MatchBonusType == MatchBonusType.None)
                {
                    continue;
                }

                MatchBonusModel bonusModel = _matchBonusModels.FirstOrDefault(bonusModel => bonusModel.BonusType == matchModel.MatchBonusType);
                if (bonusModel == null)
                {
                    Debug.LogError($"Missing match bonus model: {matchModel.MatchBonusType}");
                    continue;
                }

                sequence.PrependCallback(() =>
                {
                    MatchBonusView matchBonusView = Instantiate(bonusModel.MatchBonusViewPrefab, effectCenterTileSpotTransform);
                    matchBonusView.SetupAndPlay(matchModel, tileSpots);
                });
            }

            sequence.Append(DOVirtual.DelayedCall(0.2f, () => { }));
            return sequence;
        }
    }
}
