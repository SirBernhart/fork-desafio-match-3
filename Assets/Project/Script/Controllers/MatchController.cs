using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class MatchController : MonoBehaviour
    {
        /// <summary>
        /// Sets all the coordinates that have tiles to be destroyed as true. This includes normal
        /// matching and bonus effects (line-clearer, explosions etc)
        /// </summary>
        /// <param name="newBoard">"The board to have the coordinates marked"</param>
        /// <returns></returns>
        public static List<List<bool>> FindAllTilesToBeDestroyed(List<List<Tile>> newBoard)
        {
            List<List<bool>> matchedTiles = new();
            for (int y = 0; y < newBoard.Count; y++)
            {
                matchedTiles.Add(new List<bool>(newBoard[y].Count));
                for (int x = 0; x < newBoard.Count; x++)
                {
                    matchedTiles[y].Add(false);
                }
            }

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    FindMatchesInLine(x, y);
                    FindMatchesInColumn(x, y);
                }
            }

            return matchedTiles;

            void FindMatchesInLine(int x, int y)
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
                            // Destroy all of same color
                        }
                        else
                        {
                            SetWholeLineToBeCleared(matchedTiles[y]);
                        }
                    }
                }
                
                void SetWholeLineToBeCleared(List<bool> lineToBeCleared)
                {
                    for(int x = 0; x < lineToBeCleared.Count; x++)
                    {
                        lineToBeCleared[x] = true;
                    }
                }
            }

            void FindMatchesInColumn(int x, int y)
            {
                if (CanMatchColumn(x, y, newBoard))
                {
                    matchedTiles[y][x] = true;
                    matchedTiles[y - 1][x] = true;
                    matchedTiles[y - 2][x] = true;
                    if (y >= 3 &&
                        newBoard[y - 2][x].Type == newBoard[y - 3][x].Type)
                    {
                        if (x >= 4 &&
                            newBoard[y - 3][x].Type == newBoard[y - 4][x].Type)
                        {
                            // Destroy all of same color
                        }
                        else
                        {
                            SetWholeColumnToBeCleared(x);
                        }
                    }
                }
                void SetWholeColumnToBeCleared(int columnToBeClearedNumber)
                {
                    for (int y = 0; y < matchedTiles.Count; y++)
                    {
                        matchedTiles[y][columnToBeClearedNumber] = true;
                    }
                }
            }

            void SetToDestroyAllTilesOfType(int type)
            {
                for (int y = 0; y < newBoard.Count; y++)
                {
                    for (int x = 0; x < newBoard[y].Count; x++)
                    {
                        if (newBoard[y][x].Type == type)
                        {
                            matchedTiles[y][x] = true;
                        }
                    }
                }
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
        
        /*private readonly List<IMatchPattern> _patterns;
        private readonly List<IMatchEffect> _effects;
    
        public MatchController()
        {
            _patterns = new List<IMatchPattern>
            {
                new TShapePattern(),     // Priority 4
                new SquarePattern(),     // Priority 3
                new LShapePattern(),     // Priority 2
                new StraightLinePattern() // Priority 1
            }.OrderByDescending(p => p.Priority).ToList();
        
            _effects = new List<IMatchEffect>
            {
                new SimpleDestroyEffect(),
                new RowClearEffect(),
                new ColumnClearEffect(),
                new BombEffect(),
                // etc.
            };
        }
    
        public void ProcessMatches(List<Tile> selectedTiles)
        {
            MatchData matchData = null;
        
            // Find matching pattern (Strategy)
            foreach (var pattern in _patterns)
            {
                if (pattern.Matches(selectedTiles, out matchData))
                    break;
            }
        
            if (matchData == null) return;
        
            // Execute matching effects (Command)
            foreach (var effect in _effects)
            {
                if (effect.CanExecute(matchData))
                {
                    effect.Execute(matchData);
                }
            }
        }*/
    }
}
