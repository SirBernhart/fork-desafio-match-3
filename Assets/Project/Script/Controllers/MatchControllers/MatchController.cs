using System.Collections.Generic;
using System.Linq;
using Gazeus.DesafioMatch3.Controllers.MatchControllers;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class MatchController : MonoBehaviour
    {
        private List<IMatchPatternStrategy> _matchPatternStrategies;
        //private readonly List<IMatchEffect> _effects;
        
        private void Awake()
        {
            _matchPatternStrategies = new List<IMatchPatternStrategy>
            {
                // TODO: Add other strategies
                new LineMatchPatternStrategy()
            }.OrderByDescending(p => p.Priority).ToList();
        }

        /// <summary>
        /// Sets all the coordinates that have tiles to be destroyed as true. This includes normal
        /// matching and bonus effects (line-clearer, explosions etc)
        /// </summary>
        /// <param name="newBoard">"The board to have the coordinates marked"</param>
        /// <returns></returns>
        public List<List<bool>> FindAllTilesToBeDestroyed(List<List<Tile>> newBoard)
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

            foreach (IMatchPatternStrategy matchPatternStrategy in _matchPatternStrategies)
            {
                matchPatternStrategy.FindMatches(newBoard, matchedTiles);
            }

            return matchedTiles;
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
