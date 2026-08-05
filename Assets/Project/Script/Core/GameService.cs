using System.Collections.Generic;
using Gazeus.DesafioMatch3.Controllers;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core
{
    public class GameService
    {
        private MatchController _matchController;
        private List<List<Tile>> _boardTiles;
        private List<int> _tilesTypes;
        private int _tileCount;

        public List<List<Tile>> StartGame(int boardWidth, int boardHeight, MatchController matchController)
        {
            _tilesTypes = new List<int> { 0, 1, 2, 3 };
            _boardTiles = CreateBoard(boardWidth, boardHeight, _tilesTypes);
            _matchController = matchController;

            return _boardTiles;
        }

        public List<BoardSequence> SwapTile(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            List<BoardSequence> boardSequences = new();
            List<MatchModel> matchesMade = _matchController.FindAllTilesToBeDestroyed(newBoard);

            while (matchesMade.Count > 0)
            {
                List<Vector2Int> allMatchedPositions = new();
                HashSet<int> affectedColumns = new();
                foreach (MatchModel match in matchesMade)
                {
                    foreach (Vector2Int matchedTile in match.MatchedTiles)
                    {
                        // Already cleared by an earlier, overlapping match this pass - skip it
                        if (newBoard[matchedTile.y][matchedTile.x].Id == -1)
                        {
                            continue;
                        }
                        newBoard[matchedTile.y][matchedTile.x] = new Tile { Id = -1, Type = -1 };
                        allMatchedPositions.Add(matchedTile);
                        affectedColumns.Add(matchedTile.x);
                    }
                }

                // Dropping the tiles: compact each affected column downward in ONE pass.
                // This is correct regardless of how many matches touched the column or
                // what order they were found in (unlike shifting once per matched tile,
                // which can overwrite/lose tiles when a column has 2+ matches).
                Dictionary<int, MovedTileInfo> movedTiles = new();
                List<MovedTileInfo> movedTilesList = new();
                int boardHeight = newBoard.Count;
                foreach (int x in affectedColumns)
                {
                    int writeRow = boardHeight - 1;
                    for (int readRow = boardHeight - 1; readRow >= 0; readRow--)
                    {
                        Tile tile = newBoard[readRow][x];
                        if (tile.Type == -1)
                        {
                            continue;
                        }

                        if (readRow != writeRow)
                        {
                            newBoard[writeRow][x] = tile;
                            newBoard[readRow][x] = new Tile { Id = -1, Type = -1 };

                            MovedTileInfo movedTileInfo = new()
                            {
                                From = new Vector2Int(x, readRow),
                                To = new Vector2Int(x, writeRow)
                            };
                            movedTiles.Add(tile.Id, movedTileInfo);
                            movedTilesList.Add(movedTileInfo);
                        }

                        writeRow--;
                    }
                }

                // Filling the board
                List<AddedTileInfo> addedTiles = new();
                for (int y = newBoard.Count - 1; y > -1; y--)
                {
                    for (int x = newBoard[y].Count - 1; x > -1; x--)
                    {
                        if (newBoard[y][x].Type == -1)
                        {
                            int tileType = Random.Range(0, _tilesTypes.Count);
                            Tile tile = newBoard[y][x];
                            tile.Id = _tileCount++;
                            tile.Type = _tilesTypes[tileType];
                            addedTiles.Add(new AddedTileInfo
                            {
                                Position = new Vector2Int(x, y),
                                Type = tile.Type
                            });
                        }
                    }
                }

                BoardSequence sequence = new()
                {
                    MatchedPosition = allMatchedPositions,
                    MovedTiles = movedTilesList,
                    AddedTiles = addedTiles,
                    MatchModels = matchesMade
                };
                boardSequences.Add(sequence);

                matchesMade = _matchController.FindAllTilesToBeDestroyed(newBoard);
            }

            _boardTiles = newBoard;

            return boardSequences;
        }

        private static List<List<Tile>> CopyBoard(List<List<Tile>> boardToCopy)
        {
            List<List<Tile>> newBoard = new(boardToCopy.Count);
            for (int y = 0; y < boardToCopy.Count; y++)
            {
                newBoard.Add(new List<Tile>(boardToCopy[y].Count));
                for (int x = 0; x < boardToCopy[y].Count; x++)
                {
                    Tile tile = boardToCopy[y][x];
                    newBoard[y].Add(new Tile { Id = tile.Id, Type = tile.Type });
                }
            }

            return newBoard;
        }

        private List<List<Tile>> CreateBoard(int width, int height, List<int> tileTypes)
        {
            List<List<Tile>> board = new(height);
            _tileCount = 0;
            for (int y = 0; y < height; y++)
            {
                board.Add(new List<Tile>(width));
                for (int x = 0; x < width; x++)
                {
                    board[y].Add(new Tile { Id = -1, Type = -1 });
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<int> noMatchTypes = new(tileTypes.Count);
                    for (int i = 0; i < tileTypes.Count; i++)
                    {
                        noMatchTypes.Add(_tilesTypes[i]);
                    }

                    if (x > 1 &&
                        board[y][x - 1].Type == board[y][x - 2].Type)
                    {
                        noMatchTypes.Remove(board[y][x - 1].Type);
                    }

                    if (y > 1 &&
                        board[y - 1][x].Type == board[y - 2][x].Type)
                    {
                        noMatchTypes.Remove(board[y - 1][x].Type);
                    }

                    board[y][x].Id = _tileCount++;
                    board[y][x].Type = noMatchTypes[Random.Range(0, noMatchTypes.Count)];
                }
            }

            return board;
        }
    }
}
