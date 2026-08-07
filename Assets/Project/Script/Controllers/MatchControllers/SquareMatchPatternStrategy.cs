using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public class SquareMatchPatternStrategy : IMatchPatternStrategy
    {
        public List<MatchModel> FindMatches(List<List<Tile>> newBoard)
        {
            List<MatchModel> matchesMade = new ();
            for (int y = 0; y < newBoard.Count-1; y++)
            {
                for (int x = 0; x < newBoard[y].Count-1; x++)
                {
                    Tile currentTile = newBoard[y][x];
                    if (currentTile.Type == newBoard[y][x + 1].Type
                        && currentTile.Type == newBoard[y + 1][x].Type
                        && currentTile.Type == newBoard[y + 1][x + 1].Type)
                    {
                        var match = new MatchModel
                        {
                            MatchedTiles = new List<Vector2Int>()
                            {
                                new(x, y),
                                new(x, y + 1),
                                new(x + 1, y),
                                new(x + 1, y + 1)
                            },
                            CenterPosition = new Vector2Int(x, y),
                            MatchBonusType = MatchBonusType.ClearRandomTiles
                        };

                        match.MatchBonusTiles = DestroyRandomTilesMatchBonusController.GetTileCoordinates(3, newBoard, match.MatchedTiles);
                        match.MatchedTiles.AddRange(match.MatchBonusTiles);
                        matchesMade.Add(match);
                    }
                }
            }
            
            return matchesMade;
        }
    }
}
