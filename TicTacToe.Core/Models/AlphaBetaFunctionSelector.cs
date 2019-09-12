using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    /// <summary>
    /// alpha-beta法
    /// </summary>
    public class AlphaBetaFunctionSelector : ICellSelector
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="board"></param>
        /// <param name="type"></param>
        public AlphaBetaFunctionSelector(Board board, CellType type)
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


        /// <summary>
        /// セルを選択する
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public async Task<Point?> SelectAsync(IEnumerable<Point> cells)
        {
            return await Task.Run(() =>
            {
                var score = AlphaBeta(_board.Clone(), true, 0, int.MinValue, int.MaxValue);
                return score.Key;
            });
        }

        /// <summary>
        /// 評価関数
        /// </summary>
        /// <param name="board"></param>
        /// <param name="depth"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private bool Evaluate(Board board, int depth, out double score)
        {
            if (board.GetWinner() == _selfType)
            {
                //勝った時
                score = 10 - depth;
                return true;
            }
            if (board.GetWinner() == _nonSelfType)
            {
                //負けた時
                score = -10 + depth;
                return true;
            }

            if (board.GetEmptyCells().Any())
            {
                //継続
                score = 0;
                return false;
            }
            else
            {
                //引き分け
                score = 0;// depth / 10d;
                return true;
            }
        }

        /// <summary>
        /// alpha-beta法で探索
        /// </summary>
        /// <param name="board"></param>
        /// <param name="isMyTurn"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private KeyValuePair<Point, double> AlphaBeta(Board board, bool isMyTurn, int depth, double alpha, double beta)
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
                double tempScore;
                try
                {
                    tempScore = AlphaBeta(board, !isMyTurn, depth + 1, alpha, beta).Value;
                    if (isMyTurn)
                    {
                        if (depth != 0 && tempScore >= beta)
                        {
                            return new KeyValuePair<Point, double>(cell, tempScore);
                        }
                        if (tempScore > bestScore)
                        {
                            bestScore = tempScore;
                            bestCell = cell;
                            alpha = (alpha > bestScore) ? alpha : bestScore;
                        }
                    }
                    else
                    {
                        if (depth != 0 && tempScore <= alpha)
                        {
                            return new KeyValuePair<Point, double>(cell, tempScore);
                        }
                        if (tempScore < bestScore)
                        {
                            bestScore = tempScore;
                            bestCell = cell;
                            beta = (beta < bestScore) ? beta : bestScore;
                        }
                    }
                }
                finally
                {
                    board.Undo((int)cell.Y, (int)cell.X);
                }

                //alpha-beta法の場合、枝刈りがあるので、貯められない

            }
            return new KeyValuePair<Point, double>(bestCell, bestScore);
        }
    }
}
