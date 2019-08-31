using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public class WeightCellSelector : ICellSelector
    {
        public WeightCellSelector(Board board, CellType type)
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
        }

        private Board _board;
        private CellType _selfType;
        private CellType _nonSelfType;


        private Dictionary<Point, int> _weightForFirst = new Dictionary<Point, int>()
         {
             {new Point(0, 0), 5},{new Point(1, 0), 1},{new Point(2, 0), 5},
             {new Point(0, 1), 1},{new Point(1, 1), 3},{new Point(2, 1), 1},
             {new Point(0, 2), 5},{new Point(1, 2), 1},{new Point(2, 2), 5},
         };

        private Dictionary<Point, int> _weightForSecond = new Dictionary<Point, int>()
         {
             {new Point(0, 0), 3},{new Point(1, 0), 1},{new Point(2, 0), 3},
             {new Point(0, 1), 1},{new Point(1, 1), 5},{new Point(2, 1), 1},
             {new Point(0, 2), 3},{new Point(1, 2), 1},{new Point(2, 2), 3},
         };


        public Point? Select(IEnumerable<Point> cells)
        {
            var selfCells = _board.GetCells(_selfType);
            var nonSelfCells = _board.GetCells(_nonSelfType);
            var emptyCells = _board.GetEmptyCells();


            if (_selfType == CellType.Circle)
            {
                return GetCell(cells, _weightForFirst);
            }
            else
            {
                return GetCell(cells, _weightForSecond);
            }
        }

        private Point? GetCell(IEnumerable<Point> cells, Dictionary<Point, int> weight)
        {
            Point targetCellPos = cells.First();
            int targetCellWeight = 0;
            int reachCellCount = 0;

            foreach (var cellGroup in weight.Where(w => cells.Any(c => c.X == w.Key.X && c.Y == w.Key.Y)).GroupBy(w => w.Value).OrderByDescending(g => g.Key))
            {
                foreach (var cell in cellGroup.OrderBy(c => Guid.NewGuid()))
                {
                    var tempBoard = _board.Clone();
                    tempBoard.SetCellType((int)cell.Key.Y, (int)cell.Key.X, _selfType);
                    var result = tempBoard.CheckGameStatus(_selfType);
                    if (result.Status == GameStatus.Reach)
                    {
                        if ((result as ReachResult).ReachCells.Count > reachCellCount)
                        {
                            targetCellPos = cell.Key;
                            targetCellWeight = cell.Value;
                            reachCellCount = (result as ReachResult).ReachCells.Count;
                            continue;
                        }
                        if ((result as ReachResult).ReachCells.Count == reachCellCount &&
                            cell.Value > targetCellWeight)
                        {
                            targetCellPos = cell.Key;
                            targetCellWeight = cell.Value;
                            reachCellCount = (result as ReachResult).ReachCells.Count;
                            continue;
                        }
                    }
                }
            }
            if (reachCellCount == 0)
            {
                var selectableCells = weight.Where(w => cells.Any(c => c.X == w.Key.X && c.Y == w.Key.Y));
                var maxWeight = selectableCells.Max(w => w.Value);
                return selectableCells.Where(w => w.Value == maxWeight).OrderByDescending(w => Guid.NewGuid()).First().Key;
            }
            else
            {
                return targetCellPos;
            }
        }
    }
}
