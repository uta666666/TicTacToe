using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToe.Core.Models
{
    public class NegaAlphaBetaFunctionSelector : ICellSelector
    {
        public Task<Point?> SelectAsync(IEnumerable<Point> cells)
        {
            throw new NotImplementedException();
        }
    }
}
