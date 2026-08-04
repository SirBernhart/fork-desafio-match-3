using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public class LineMatchPatternStrategy : IMatchPatternStrategy
    {
        public int Priority => 1;
        
        public List<List<bool>> FindMatches(List<List<Tile>> newBoard, List<List<bool>> matchedTiles)
        {
            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    FindMatchesInLine(x, y, newBoard, matchedTiles);
                    FindMatchesInColumn(x, y, newBoard, matchedTiles);
                }
            }

            return matchedTiles;
        }

        private void FindMatchesInLine(int x, int y, List<List<Tile>> newBoard, List<List<bool>> matchedTiles)
        {
            if (CanMatchLine(x, y, newBoard))
            {
                matchedTiles[y][x] = true;
                matchedTiles[y][x - 1] = true;
                matchedTiles[y][x - 2] = true;
                if (x >= 3 &&
                    newBoard[y][x - 2].Type == newBoard[y][x - 3].Type)
                {
                    if (x >= 4 &&
                        newBoard[y][x - 3].Type == newBoard[y][x - 4].Type)
                    {
                        if (x >= 5 &&
                            newBoard[y][x - 4].Type == newBoard[y][x - 5].Type) // Could match 6
                        {
                            SetToDestroyAllTilesOfType(newBoard[y][x].Type, newBoard, matchedTiles);
                        }
                        else // Could match 5
                        {
                            SetExplosion(x-2, y, newBoard, matchedTiles);
                        }
                    }
                    else // Could match 4
                    {
                        SetWholeLineToBeCleared(matchedTiles[y]);
                    }
                }
            }
        }
        
        private void SetWholeLineToBeCleared(List<bool> lineToBeCleared)
        {
            for(int x = 0; x < lineToBeCleared.Count; x++)
            {
                lineToBeCleared[x] = true;
            }
        }
        
        public static bool CanMatchLine(int x, int y, List<List<Tile>> newBoard)
        {
            return x > 1 &&
                   newBoard[y][x].Type == newBoard[y][x - 1].Type &&
                   newBoard[y][x - 1].Type == newBoard[y][x - 2].Type;
        }

        public static bool CanMatchColumn(int x, int y, List<List<Tile>> newBoard)
        {
            return y > 1 &&
                   newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                   newBoard[y - 1][x].Type == newBoard[y - 2][x].Type;
        }
        
        private void FindMatchesInColumn(int x, int y, List<List<Tile>> newBoard, List<List<bool>> matchedTiles)
        {
            if (CanMatchColumn(x, y, newBoard))
            {
                matchedTiles[y][x] = true;
                matchedTiles[y - 1][x] = true;
                matchedTiles[y - 2][x] = true;
                if (y >= 3 &&
                    newBoard[y - 2][x].Type == newBoard[y - 3][x].Type)
                {
                    if (y >= 4 &&
                        newBoard[y - 3][x].Type == newBoard[y - 4][x].Type)
                    {
                        if (y >= 5 &&
                            newBoard[y - 4][x].Type == newBoard[y - 5][x].Type)
                        {
                            SetToDestroyAllTilesOfType(newBoard[y][x].Type, newBoard, matchedTiles);
                        }
                        else
                        {
                            SetExplosion(x, y-2, newBoard, matchedTiles);
                        }
                    }
                    else
                    {
                        SetWholeColumnToBeCleared(x, matchedTiles);
                    }
                }
            }
        }
        
        private void SetWholeColumnToBeCleared(int columnToBeClearedNumber, List<List<bool>> matchedTiles)
        {
            for (int y = 0; y < matchedTiles.Count; y++)
            {
                matchedTiles[y][columnToBeClearedNumber] = true;
            }
        }

        /// <summary>
        /// Sets tiles to be destroyed in a diamond shape, with the middle tile of the 5-matched line as it's center
        /// </summary>
        /// <param name="xCenter">center's x coordinate</param>
        /// <param name="yCenter">center's y coordinate</param>
        private void SetExplosion(int xCenter, int yCenter, List<List<Tile>> newBoard, List<List<bool>> matchedTiles)
        {
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
                    matchedTiles[currY][currXToLeft] = true;
                }

                if (currXToRight < newBoard[currY].Count)
                {
                    matchedTiles[currY][currXToRight] = true;
                }
            }
        }

        private void SetToDestroyAllTilesOfType(int type, List<List<Tile>> newBoard, List<List<bool>> matchedTiles)
        {
            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (newBoard[y][x].Type == type && !matchedTiles[y][x])
                    {
                        matchedTiles[y][x] = true;
                    }
                }
            }
        }

    }
}
