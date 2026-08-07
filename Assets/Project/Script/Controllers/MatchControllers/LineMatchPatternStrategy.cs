using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public class LineMatchPatternStrategy : IMatchPatternStrategy
    {
        public List<MatchModel> FindMatches(List<List<Tile>> newBoard)
        {
            List<MatchModel> matches = new ();
            // Check all horizontal lines
            for (int y = 0; y < newBoard.Count; y++)
            {
                matches.AddRange(FindMatchesInLine(y, true, newBoard));
            }
            
            // Check all vertical lines
            for (int x = 0; x < newBoard[0].Count; x++)
            {
                matches.AddRange(FindMatchesInLine(x, false, newBoard));
            }

            return matches;
        }

        private List<MatchModel> FindMatchesInLine(int fixedCoordinateValue, bool isHorizontalLine, List<List<Tile>> newBoard)
        {
            List<MatchModel> matchModels = new();

            int matchSequenceSize = 1;
            for (int i = 0; LineHasNotEnded(i); i++)
            {
                // Match is still increasing
                if (!MatchSequenceHasEnded(i))
                {
                    matchSequenceSize++;
                    continue;
                }
                // Match sequence ended, but doesn't meet the minimum requirement to break tiles
                if (matchSequenceSize < 3)
                {
                    matchSequenceSize = 1;
                    continue;
                }

                MatchShape matchShape;
                Vector2Int centerPosition;
                int sequenceMiddleIndex = i - Mathf.CeilToInt(matchSequenceSize / 2f);
                if (isHorizontalLine)
                {
                    matchShape = MatchShape.HorizontalLine;
                    centerPosition = new Vector2Int(sequenceMiddleIndex, fixedCoordinateValue);
                }
                else
                {
                    matchShape = MatchShape.VerticalLine;
                    centerPosition = new Vector2Int(fixedCoordinateValue, sequenceMiddleIndex);
                }
                
                MatchModel matchModel = new()
                {
                    Shape = matchShape,
                    CenterPosition = centerPosition
                };

                List<Vector2Int> matchedTileCoordinates;

                switch (matchSequenceSize)
                {
                    case 3:
                        int matchSequenceSizeZeroBased = matchSequenceSize - 1;
                        int startingIndex = i - matchSequenceSizeZeroBased;
                        matchedTileCoordinates = SetLineMatch(matchSequenceSize, startingIndex, fixedCoordinateValue, isHorizontalLine);
                        matchModel.MatchBonusType = MatchBonusType.None;
                        break;
                    case 4:
                        int lineSize = isHorizontalLine 
                            ? newBoard[fixedCoordinateValue].Count 
                            : newBoard.Count;
                        matchedTileCoordinates = SetLineMatch(lineSize, 0, fixedCoordinateValue, isHorizontalLine);
                        matchModel.MatchBonusType = isHorizontalLine ? MatchBonusType.HorizontalLineClear : MatchBonusType.VerticalLineClear;
                        break;
                    case 5:
                        matchedTileCoordinates = ExplosionMatchBonusController.GetTileCoordinates(matchModel.CenterPosition.x, matchModel.CenterPosition.y, newBoard);
                        matchModel.MatchBonusType = MatchBonusType.Explosion;
                        break;
                    default: // 6 or more
                        int tileType = isHorizontalLine 
                            ? newBoard[fixedCoordinateValue][i].Type 
                            : newBoard[i][fixedCoordinateValue].Type;
                        matchedTileCoordinates = DestroyAllTilesOfTypeMatchBonusController.GetTileCoordinates(tileType, newBoard);
                        matchModel.MatchBonusType = MatchBonusType.ClearAllTilesOfSameColor;
                        break;
                }
                matchModel.MatchedTiles = matchedTileCoordinates;
                
                matchModels.Add(matchModel);
                
                matchSequenceSize = 1;
            }
            
            return matchModels;

            bool LineHasNotEnded(int currIndex)
            {
                return isHorizontalLine 
                    ? currIndex < newBoard[fixedCoordinateValue].Count
                    : currIndex < newBoard.Count;
            }

            bool MatchSequenceHasEnded(int currIndex)
            {
                int currTileType;
                int nextTileType;
                if (isHorizontalLine)
                {
                    if (currIndex >= newBoard[fixedCoordinateValue].Count - 1)
                    {
                        return true;
                    }
                    currTileType = newBoard[fixedCoordinateValue][currIndex].Type;
                    nextTileType = newBoard[fixedCoordinateValue][currIndex + 1].Type;
                }
                else
                {
                    if (currIndex >= newBoard.Count - 1)
                    {
                        return true;
                    }
                    
                    currTileType = newBoard[currIndex][fixedCoordinateValue].Type;
                    nextTileType = newBoard[currIndex + 1][fixedCoordinateValue].Type;
                }

                return currTileType != nextTileType;
            }
        }

        private List<Vector2Int> SetLineMatch(int matchSize, int startingIndex, int fixedCoordinateValue, bool isHorizontalLine)
        {
            List<Vector2Int> matchedTiles = new List<Vector2Int>();
            int lastIndex = startingIndex + matchSize;
            for (int movingCoordinateValue = startingIndex;
                 movingCoordinateValue < lastIndex;
                 movingCoordinateValue++)
            {
                if (isHorizontalLine)
                {
                    matchedTiles.Add(new Vector2Int(movingCoordinateValue, fixedCoordinateValue));
                }
                else
                {
                    matchedTiles.Add(new Vector2Int(fixedCoordinateValue, movingCoordinateValue));
                }
            }
            
            return matchedTiles;
        }

        

        
    }
}
