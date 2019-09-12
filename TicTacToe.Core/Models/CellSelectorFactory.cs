using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public static class CellSelectorFactory
    {
        public static ICellSelector GetSelector(Board board, CellType type)
        {
            var selector = ConfigurationManager.AppSettings.Get("CellSelector");
            return (ICellSelector)Activator.CreateInstance(Type.GetType(selector), board, type);
        }
    }
}
