using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Models
{
    public class MatchModel
    {
        public int Priority { get; set; }
        public List<Vector2Int> MatchedTiles { get; set; }
        public MatchShape Shape { get; set; }
        public Vector2Int CenterPosition { get; set; }
        public MatchBonusType MatchBonusType { get; set; }
        /// <summary>
        /// Optional field, only used by some match bonuses
        /// </summary>
        public List<Vector2Int> MatchBonusTiles { get; set; }
    }
}
