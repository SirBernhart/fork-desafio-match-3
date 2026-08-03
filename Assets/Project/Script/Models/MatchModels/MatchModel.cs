using System.Collections.Generic;
using Gazeus.DesafioMatch3.Controllers.MatchControllers;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Models
{
    public class MatchModel
    {
        public List<Tile> MatchedTiles { get; set; }
        public MatchShape Shape { get; set; }
        public Vector2Int CenterPosition { get; set; }
        public IMatchPatternStrategy SourcePatternStrategy { get; set; }
    }
}
