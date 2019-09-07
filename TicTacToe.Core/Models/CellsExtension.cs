using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using TicTacToe.Core.Commons;
using System.Threading.Tasks;

namespace TicTacToe.Core.Models
{
    public class SettlementResult : ICheckGameStatusResult
    {
        public GameStatusByUser Status { get; } = GameStatusByUser.Settlement;

        public List<Point> SettlementCells { get; set; }

        public SettledPattern SettlementPattern { get; set; }
    }

    public class ReachResult : ICheckGameStatusResult
    {
        public GameStatusByUser Status { get; } = GameStatusByUser.Reach;

        public List<Point> ReachCells { get; set; }
    }

    public class NothingResult : ICheckGameStatusResult
    {
        public GameStatusByUser Status { get; } = GameStatusByUser.None;
    }



    public static class CellsExtension
    {

        private static List<KeyValuePair<SettledPattern, List<Point>>> RowVictoryPattern = new List<KeyValuePair<SettledPattern, List<Point>>>()
        {
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.HorizontalTop, new List<Point>()
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0)
            }),
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.HorizontalMiddle, new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1)
            }),
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.HorizontalBottom, new List<Point>()
            {
                new Point(0, 2),
                new Point(1, 2),
                new Point(2, 2)
            })
        };

        private static List<KeyValuePair<SettledPattern, List<Point>>> ColVictoryPattern = new List<KeyValuePair<SettledPattern, List<Point>>>()
        {
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.VerticalLeft, new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2)
            }),
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.VerticalCenter, new List<Point>()
            {
                new Point(1, 0),
                new Point(1, 1),
                new Point(1, 2)
            }),
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.VerticalRight, new List<Point>()
            {
                new Point(2, 0),
                new Point(2, 1),
                new Point(2, 2)
            })
        };

        private static List<KeyValuePair<SettledPattern, List<Point>>> DiagVictoryPatten = new List<KeyValuePair<SettledPattern, List<Point>>>()
        {
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.CrossLeft, new List<Point>()
            {
                new Point(0, 0),
                new Point(1, 1),
                new Point(2, 2)
            }),
            new KeyValuePair<SettledPattern, List<Point>>(SettledPattern.CrossRight, new List<Point>()
            {
                new Point(2, 0),
                new Point(1, 1),
                new Point(0, 2)
            })
        };




        /// <summary>
        /// ゲームの勝者を返す
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public static void GetGameStatus(this Cell[,] cells, out GameStatus result, out SettledPattern pattern)
        {
            GetGameStatus(cells, RowVictoryPattern, out result, out pattern);
            if (result != GameStatus.None)
            {
                return;
            }

            GetGameStatus(cells, ColVictoryPattern, out result, out pattern);
            if (result != GameStatus.None)
            {
                return;
            }

            GetGameStatus(cells, DiagVictoryPatten, out result, out pattern);
            if (result != GameStatus.None)
            {
                return;
            }

            result = GameStatus.None;
            pattern = SettledPattern.None;
        }

        private static void GetGameStatus(Cell[,] cells, List<KeyValuePair<SettledPattern, List<Point>>> checkPattern, out GameStatus status, out SettledPattern pattern)
        {
            foreach (var row in checkPattern)
            {
                List<Point> hitCells = new List<Point>();
                List<Point> emptyCells = new List<Point>();

                if (row.Value.Select(c => cells[(int)c.Y, (int)c.X]).All(c => c.Type == CellType.Circle))
                {
                    status = GameStatus.Win_Circle;
                    pattern = row.Key;
                    return;
                }
                if (row.Value.Select(c => cells[(int)c.Y, (int)c.X]).All(c => c.Type == CellType.Cross))
                {
                    status = GameStatus.Win_Cross;
                    pattern = row.Key;
                    return;
                }
            }
            status = GameStatus.None;
            pattern = SettledPattern.None;
        }




        /// <summary>
        /// 指定されたマークのゲームの状態を返す
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICheckGameStatusResult GetGameStatus(this Cell[,] cells, CellType type)
        {
            List<ICheckGameStatusResult> resultList = new List<ICheckGameStatusResult>();

            var rowResult = GetGameStatus(cells, type, RowVictoryPattern);
            if (rowResult.Status == GameStatusByUser.Settlement)
            {
                return rowResult;
            }
            resultList.Add(rowResult);

            var colResult = GetGameStatus(cells, type, ColVictoryPattern);
            if (colResult.Status == GameStatusByUser.Settlement)
            {
                return colResult;
            }
            resultList.Add(colResult);

            var diagResult = GetGameStatus(cells, type, DiagVictoryPatten);
            if (diagResult.Status == GameStatusByUser.Settlement)
            {
                return diagResult;
            }
            resultList.Add(diagResult);

            var result = resultList.Aggregate((result, next) => (int)result.Status > (int)next.Status ? result : next);
            if (resultList.Any(r => r.Status == GameStatusByUser.Reach))
            {
                var reachCells = resultList.Where(r => r.Status == GameStatusByUser.Reach).SelectMany(r => (r as ReachResult).ReachCells).ToList();
                return new ReachResult()
                {
                    ReachCells = reachCells
                };
            }

            return new NothingResult();
        }

        private static ICheckGameStatusResult GetGameStatus(Cell[,] cells, CellType type, List<KeyValuePair<SettledPattern, List<Point>>> pattern)
        {
            List<Point> reachCells = new List<Point>();
            foreach (var row in pattern)
            {
                List<Point> hitCells = new List<Point>();
                List<Point> emptyCells = new List<Point>();
                foreach (var cell in row.Value)
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
                        SettlementCells = hitCells,
                        SettlementPattern = row.Key
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


        /// <summary>
        /// セルを選択する
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static async Task<Point?> SelectAsync(this IEnumerable<Point> cells, ICellSelector selector)
        {
            if (!cells.Any())
            {
                return null;
            }
            return await selector.SelectAsync(cells);
        }
    }
}
