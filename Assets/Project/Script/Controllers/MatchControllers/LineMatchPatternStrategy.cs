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
                        SetToDestroyAllTilesOfType(newBoard[y][x].Type, newBoard, matchedTiles);
                    }
                    else
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
                        SetToDestroyAllTilesOfType(newBoard[y][x].Type, newBoard, matchedTiles);
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
