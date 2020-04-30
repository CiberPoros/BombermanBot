using Bomberman.Api;
using Bomberman.Logic.Handlers.Facade;
using System.Linq;

namespace Bomberman.Logic
{
    internal class Utils
    {
        internal static bool IsActiveBomb(Element element) =>
            element == Element.BOMB_BOMBERMAN ||
            element == Element.BOMB_TIMER_1 ||
            element == Element.BOMB_TIMER_2 ||
            element == Element.BOMB_TIMER_3 ||
            element == Element.BOMB_TIMER_4 ||
            element == Element.BOMB_TIMER_5 ||
            element == Element.OTHER_BOMB_BOMBERMAN;

        internal static Move GetMoveByMask(int mask) =>
            (Move)(mask & (int)(Move.Down | Move.Left | Move.Right | Move.Stop | Move.Up));

        internal static bool IsBarrier(Element element) =>
            element != Element.Space;

        internal static bool IsStaticUnit(Board condition, Point point) =>
            (condition.GetAt(point) == Element.OTHER_BOMBERMAN && HandlersFacade.AfkPlayers.Contains(point)) ||
            condition.GetAt(point) == Element.DESTROYABLE_WALL;

        internal static string MoveMaskToString(int moveMask)
        {
            string result = "";
            if (((Move)moveMask & Move.ActBefore) > 0)
                result += "Act ";

            foreach (var dir in new Move[] { Move.Down, Move.Left, Move.Right, Move.Up, Move.Stop })
                if (((Move)moveMask & dir) > 0)
                    result += $" { dir } ";

            if (((Move)moveMask & Move.ActAfter) > 0)
                result += "Act";

            return result.Trim();
        }
    }
}
