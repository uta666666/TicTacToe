using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public enum GameStatus
    {
        /// <summary>
        /// 決着
        /// </summary>
        Settlement = 2,
        /// <summary>
        /// リーチあり
        /// </summary>
        Reach = 1,
        /// <summary>
        /// 何もなし
        /// </summary>
        None = 0
    }

    public interface ICheckGameStatusResult
    {
        GameStatus Status { get; }
    }

    public class SettlementResult : ICheckGameStatusResult
    {
        public GameStatus Status { get; } = GameStatus.Settlement;

        public List<Point> SettlementCells { get; set; }
    }

    public class ReachResult : ICheckGameStatusResult
    {
        public GameStatus Status { get; } = GameStatus.Reach;

        public List<Point> ReachCells { get; set; }
    }

    public class NothingResult : ICheckGameStatusResult
    {
        public GameStatus Status { get; } = GameStatus.None;
    }



    public static class CellsExtension
    {
        private static List<List<Point>> RowVictoryPattern = new List<List<Point>>()
        {
            new List<Point>()
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0)
            },
            new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1)
            },
            new List<Point>()
            {
                new Point(0, 2),
                new Point(1, 2),
                new Point(2, 2)
            }
        };

        private static List<List<Point>> ColVictoryPattern = new List<List<Point>>()
        {
            new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2)
            },
            new List<Point>()
            {
                new Point(1, 0),
                new Point(1, 1),
                new Point(1, 2)
            },
            new List<Point>()
            {
                new Point(2, 0),
                new Point(2, 1),
                new Point(2, 2)
            }
        };

        private static List<List<Point>> DiagVictoryPatten = new List<List<Point>>()
        {
            new List<Point>()
            {
                new Point(0, 0),
                new Point(1, 1),
                new Point(2, 2)
            },
            new List<Point>()
            {
                new Point(2, 0),
                new Point(1, 1),
                new Point(0, 2)
            }
        };


        public static ICheckGameStatusResult GetGameStatus(this Cell[,] cells, CellType type)
        {
            List<ICheckGameStatusResult> resultList = new List<ICheckGameStatusResult>();

            var rowResult = GetGameStatus(cells, type, RowVictoryPattern);
            if (rowResult.Status == GameStatus.Settlement)
            {
                return rowResult;
            }
            resultList.Add(rowResult);

            var colResult = GetGameStatus(cells, type, ColVictoryPattern);
            if (colResult.Status == GameStatus.Settlement)
            {
                return colResult;
            }
            resultList.Add(colResult);

            var diagResult = GetGameStatus(cells, type, DiagVictoryPatten);
            if (diagResult.Status == GameStatus.Settlement)
            {
                return diagResult;
            }
            resultList.Add(diagResult);

            var result = resultList.Aggregate((result, next) => (int)result.Status > (int)next.Status ? result : next);
            if (resultList.Any(r => r.Status == GameStatus.Reach))
            {
                var reachCells = resultList.Where(r => r.Status == GameStatus.Reach).SelectMany(r => (r as ReachResult).ReachCells).ToList();
                return new ReachResult()
                {
                    ReachCells = reachCells
                };
            }

            return new NothingResult();
        }

        private static ICheckGameStatusResult GetGameStatus(Cell[,] cells, CellType type, List<List<Point>> pattern)
        {
            List<Point> reachCells = new List<Point>();
            foreach (var row in pattern)
            {
                List<Point> hitCells = new List<Point>();
                List<Point> emptyCells = new List<Point>();
                foreach (var cell in row)
                {
                    if (cells[(int)cell.Y, (int)cell.X].Type == type)
                    {
                        hitCells.Add(cell);
                    }
                    else if (cells[(int)cell.Y, (int)cell.X].Type == CellType.None)
                    {
                        emptyCells.Add(cell);
                    }
                }
                if (hitCells.Count == 3)
                {
                    return new SettlementResult()
                    {
                        SettlementCells = hitCells
                    };
                }
                if (hitCells.Count == 2 && emptyCells.Count == 1)
                {
                    reachCells.AddRange(emptyCells);
                }
            }
            if (reachCells.Any())
            {
                return new ReachResult()
                {
                    ReachCells = reachCells
                };
            }
            else
            {
                return new NothingResult();
            }
        }


        public static Point? Select(this IEnumerable<Point> cells, ICellSelector selector)
        {
            if (!cells.Any())
            {
                return null;
            }
            return selector.Select(cells);
        }
    }
}
