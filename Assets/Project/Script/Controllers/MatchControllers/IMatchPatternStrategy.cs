using System.Collections.Generic;
using Gazeus.DesafioMatch3.Models;

namespace Gazeus.DesafioMatch3.Controllers.MatchControllers
{
    public interface IMatchPatternStrategy
    {
        List<MatchModel> FindMatches(List<List<Tile>> newBoard);
    }
}
