using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public static class DestroyAllTilesOfTypeMatchBonusController
    {
        public static List<Vector2Int> GetTileCoordinates(int type, List<List<Tile>> newBoard)
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
