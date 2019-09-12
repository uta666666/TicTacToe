using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using TicTacToe.Core.Commons;
using System.Threading.Tasks;

namespace TicTacToe.Core.Models
{
    /// <summary>
    /// MinMax法でセルを選択する
    /// </summary>
    public class MinMaxFunctionSelector : ICellSelector
    {
        public MinMaxFunctionSelector(Board board, CellType type)
        {
            _board = board;
            _selfType = type;
            if (_selfType == CellType.Circle)
            {
                _nonSelfType = CellType.Cross;
            }
            else if (_selfType == CellType.Cross)
            {
                _nonSelfType = CellType.Circle;
            }
            else
            {
                _nonSelfType = CellType.None;
            }

            _evaluationValues = new Dictionary<Point, double>();
        }

        private Board _board;
        private CellType _selfType;
        private CellType _nonSelfType;

        private Dictionary<Point, double> _evaluationValues;


        public async Task<Point?> SelectAsync(IEnumerable<Point> cells)
        {
            return await Task.Run(() =>
            {
                _evaluationValues.Clear();
                var score = MinMax(_board.Clone(), true, 0);
                return score.Key;
            });
        }

        private bool Evaluate(Board board, int depth, out double score)
        {
            if (board.GetWinner() == _selfType)
            //if (board.CheckGameStatus(_selfType).Status == GameStatusByUser.Settlement)
            {
                score = 10 - depth;
                return true;
            }
            if (board.GetWinner() == _nonSelfType)
            //if (board.CheckGameStatus(_nonSelfType).Status == GameStatusByUser.Settlement)
            {
                score = -10 + depth;
                return true;
            }

            if (board.GetEmptyCells().Any())
            {
                score = 0;
                return false;
            }
            else
            {
                //Random r = new Random();
                //score = r.Next(0, 9) * 0.1;
                score = 0;
                return true;
            }
        }

        private KeyValuePair<Point, double> MinMax(Board board, bool isMyTurn, int depth)
        {
            double evaluationValue;
            if (Evaluate(board, depth, out evaluationValue))
            {
                return new KeyValuePair<Point, double>(new Point(0, 0), evaluationValue);
            }

            double bestScore = isMyTurn ? int.MinValue : int.MaxValue;
            var cellType = isMyTurn ? _selfType : _nonSelfType;

            Point bestCell;
            var emptyCells = board.GetEmptyCells();
            foreach (var cell in emptyCells)
            {
                board.SetCellType((int)cell.Y, (int)cell.X, cellType);
                double tempScore = MinMax(board, !isMyTurn, depth + 1).Value;
                if (isMyTurn)
                {
                    if (tempScore > bestScore)
                    {
                        bestScore = tempScore;
                        bestCell = cell;
                    }
                }
                else
                {
                    if (tempScore < bestScore)
                    {
                        bestScore = tempScore;
                        bestCell = cell;
                    }
                }
                board.Undo((int)cell.Y, (int)cell.X);

                if (depth == 0)
                {
                    _evaluationValues.Add(cell, tempScore);
                }
            }
            if (depth == 0)
            {
                return _evaluationValues.Where(e => e.Value == bestScore).OrderBy(e => Guid.NewGuid()).First();
            }
            else
            {
                return new KeyValuePair<Point, double>(bestCell, bestScore);
            }
        }
    }
}
