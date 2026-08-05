using System;
using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public class LineMatchPatternStrategy : IMatchPatternStrategy
    {
        public int Priority => 1;
        
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
                
                MatchModel matchModel = new()
                {
                    Shape = isHorizontalLine ? MatchShape.HorizontalLine : MatchShape.VerticalLine
                };
                
                int matchSequenceSizeZeroBased = matchSequenceSize - 1;
                int startingIndex = i - matchSequenceSizeZeroBased;
                List<Vector2Int> matchedTileCoordinates = 
                    SetLineMatch(matchSequenceSize, startingIndex, fixedCoordinateValue, isHorizontalLine);
                matchModel.CenterPosition = matchedTileCoordinates[Mathf.CeilToInt(matchSequenceSize/2)];
                switch (matchSequenceSize)
                {
                    case 3:
                        matchModel.MatchBonusType = MatchBonusType.None;
                        break;
                    case 4:
                        matchedTileCoordinates = SetLineMatch(10, 0, fixedCoordinateValue, isHorizontalLine);
                        matchModel.MatchBonusType = MatchBonusType.Line4;
                        break;
                    case 5:
                        matchedTileCoordinates = SetExplosion(matchModel.CenterPosition.x, matchModel.CenterPosition.y, newBoard);
                        matchModel.MatchBonusType = MatchBonusType.Line5;
                        break;
                    case 6:
                        int tileType = isHorizontalLine 
                            ? newBoard[fixedCoordinateValue][i].Type 
                            : newBoard[i][fixedCoordinateValue].Type;
                        matchedTileCoordinates = SetToDestroyAllTilesOfType(tileType, newBoard);
                        matchModel.MatchBonusType = MatchBonusType.Line6;
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

        /// <summary>
        /// Sets tiles to be destroyed in a diamond shape, with the middle tile of the 5-matched line as it's center
        /// </summary>
        /// <param name="xCenter">center's x coordinate</param>
        /// <param name="yCenter">center's y coordinate</param>
        private List<Vector2Int> SetExplosion(int xCenter, int yCenter, List<List<Tile>> newBoard)
        {
            List<Vector2Int> matchedPositions = new ();
            int maxYDelta = 3;
            for (int yHeightDelta = 0; yHeightDelta < maxYDelta; yHeightDelta++)
            {
                int currYAbove = yCenter - yHeightDelta;
                int currYBelow = yCenter + yHeightDelta;
                int currLineMaxXDelta = maxYDelta - yHeightDelta;
                for (int currXDelta = 0; currXDelta < currLineMaxXDelta; currXDelta++)
                {
                    int currXToLeft = xCenter - currXDelta;
                    int currXToRight = xCenter + currXDelta;
                    if (currYAbove >= 0)
                    {
                        SetTilesInCurrXDelta(currXToLeft, currXToRight, currYAbove);
                    }

                    if (currYBelow < newBoard.Count)
                    {
                        SetTilesInCurrXDelta(currXToLeft, currXToRight, currYBelow);
                    }
                }
            }

            void SetTilesInCurrXDelta(int currXToLeft, int currXToRight, int currY)
            {
                if (currXToLeft >= 0)
                {
                    matchedPositions.Add(new Vector2Int(currXToLeft, currY));
                }

                if (currXToRight < newBoard[currY].Count)
                {
                    matchedPositions.Add(new Vector2Int(currXToRight, currY));
                }
            }

            return matchedPositions;
        }

        private List<Vector2Int> SetToDestroyAllTilesOfType(int type, List<List<Tile>> newBoard)
        {
            var matchedTiles = new List<Vector2Int>();
            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (newBoard[y][x].Type == type)
                    {
                        matchedTiles.Add(new Vector2Int(x, y));
                    }
                }
            }
            return matchedTiles;
        }
    }
}
