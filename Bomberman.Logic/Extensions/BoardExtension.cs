using Bomberman.Api;
using System.Collections.Generic;
using System.Linq;

namespace Bomberman.Logic.Extensions
{
    internal static class BoardExtension
    {
        internal static IReadOnlyCollection<Point> GetAliveOtherBombermans(this Board board) =>
            board.Get(Element.OTHER_BOMBERMAN)
                .Concat(board.Get(Element.OTHER_BOMB_BOMBERMAN))
                .ToList();
    }
}
