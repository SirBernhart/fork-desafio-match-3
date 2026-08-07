using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public static class ExplosionMatchBonusController
    {
        /// <summary>
        /// Sets tiles to be destroyed in a diamond shape, with the middle tile of the 5-matched line as it's center
        /// </summary>
        /// <param name="xCenter">center's x coordinate</param>
        /// <param name="yCenter">center's y coordinate</param>
        public static List<Vector2Int> GetTileCoordinates(int xCenter, int yCenter, List<List<Tile>> newBoard)
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
    }
}
