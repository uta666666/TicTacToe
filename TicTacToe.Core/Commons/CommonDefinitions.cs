using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Core.Commons
{
    public enum CellType
    {
        /// <summary>
        /// ○
        /// </summary>
        Circle,
        /// <summary>
        /// ✕
        /// </summary>
        Cross,
        /// <summary>
        /// 空
        /// </summary>
        None
    }
    public enum GameStatusByUser
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

    public enum GameStatus
    {
        Win_Circle,
        Win_Cross,
        None
    }

    public enum SettledPattern
    {
        CrossLeft,
        CrossRight,
        VerticalLeft,
        VerticalCenter,
        VerticalRight,
        HorizontalTop,
        HorizontalMiddle,
        HorizontalBottom,
        None
    }
}
