namespace Gazeus.DesafioMatch3.Models
{
    public enum MatchBonusType
    {
        None = 0, 
        HorizontalLineClear = 100, 
        VerticalLineClear = 200, 
        Explosion = 300,
        ClearAllTilesOfSameColor = 400,
        ClearRandomTiles = 500
    }
}
