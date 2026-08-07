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
        public List<MatchModel> FindAllTilesToBeDestroyed(List<List<Tile>> newBoard)
        {
            List<MatchModel> matchesMade = new();
            foreach (IMatchPatternStrategy matchPatternStrategy in _matchPatternStrategies)
            {
                matchesMade.AddRange(matchPatternStrategy.FindMatches(newBoard));
            }

            return matchesMade;
        }
    }
}
