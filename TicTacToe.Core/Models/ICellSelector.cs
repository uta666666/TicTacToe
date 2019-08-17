using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TicTacToe.Core.Models
{
    public interface ICellSelector
    {
        Point? Select(IEnumerable<Point> cells);
    }
}
