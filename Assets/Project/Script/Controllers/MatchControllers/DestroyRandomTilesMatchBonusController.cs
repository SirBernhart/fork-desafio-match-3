using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public static class DestroyRandomTilesMatchBonusController 
    {
        public static List<Vector2Int> GetTileCoordinates(int amountToDestroy, List<List<Tile>> board, List<Vector2Int> tilesToIgnore)
        {
            List<Vector2Int> selectedTiles = new ();

            int tilesSelected = 0;
            while (tilesSelected < amountToDestroy)
            {
                int y = Random.Range(0, board.Count);
                int x = Random.Range(0, board[0].Count);

                int j = 0;
                for (; j < tilesToIgnore.Count; j++)
                {
                    Vector2Int tileToIgnore = tilesToIgnore[j];
                    if (tileToIgnore.x == x && tileToIgnore.y == y)
                    {
                        break;
                    }
                }

                if (j < tilesToIgnore.Count)
                {
                    continue;
                }
                
                selectedTiles.Add(new (x, y));
                tilesSelected++;
            }

            return selectedTiles;
        }        
    }
}
