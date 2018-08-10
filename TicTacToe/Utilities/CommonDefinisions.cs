using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Utilities
{
    public enum GridItem
    {
        Circle,
        Cross
    }

    public static class GridItemExtension
    {
        public static string ToString(this GridItem item)
        {
            switch (item)
            {
                case GridItem.Circle:
                    return "○";
                case GridItem.Cross:
                    return "✕";
                default:
                    return string.Empty;
            }
        }
    }
}
