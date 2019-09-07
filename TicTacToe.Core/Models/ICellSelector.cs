using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToe.Core.Models
{
    public interface ICellSelector
    {
        Task<Point?> SelectAsync(IEnumerable<Point> cells);
    }
}
