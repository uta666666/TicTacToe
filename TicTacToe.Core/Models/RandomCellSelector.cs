using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToe.Core.Models
{
    public class RandomCellSelector : ICellSelector
    {
        public async Task<Point?> SelectAsync(IEnumerable<Point> cells)
        {
            return await Task.Run<Point?>(() =>
            {
                if (!cells.Any())
                {
                    return null;
                }
                var r = new Random();
                var randomIndex = r.Next(0, cells.Count() - 1);
                return cells.Select((data, index) => new { Data = data, Index = index }).First(c => c.Index == randomIndex).Data;
            });
        }
    }
}
