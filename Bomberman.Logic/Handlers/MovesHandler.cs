using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bomberman.Logic.Extensions;
using System.Threading.Tasks;
using Bomberman.Logic.Handlers.Facade;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    internal class MovesHandler : AbstractHandler<MovesHandler>
    {
        private List<int> _movesMasks;

        internal IReadOnlyCollection<int> MovesMasks => _movesMasks;

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            ICollection<int> movesMasks = GetMovesMasks(GetDirectionsForShift(condition), GetDirectionsForPlant(condition));

            _movesMasks = (from mask in movesMasks
                           where IsNonDeadlyMove(condition, mask)
                           select mask)
                          .ToList();
        }

        private bool IsNonDeadlyMove(Board condition, int moveMask)
        {
            IReadOnlyDictionary<Point, int> newPointsAffectedByBlast = GetNewPointsAffectedByBlast(condition, moveMask, out Point? bomb);

            Point bomberman = condition.GetBomberman();
            Move direction = Utils.GetMoveByMask(moveMask);
            Console.WriteLine($"mm: { moveMask }");

            bool isNotDeadly = false;
            CheckIsDeadly(condition.GetBomberman().Shift(direction));
            return isNotDeadly;

            void CheckIsDeadly(Point point, int deep = 1)
            {
                if (deep >= Parameters.DeepForCheckDeadlyMoves)
                {
                    isNotDeadly = true;
                    return;
                }

                if (HandlersFacade.GetTimeToBoom(point) == deep ||
                    (newPointsAffectedByBlast.ContainsKey(point) && newPointsAffectedByBlast[point] == deep)) // Именно на этом тике будет взрыв!!
                    return;

                foreach (var dir in new Move[] { Move.Down, Move.Left, Move.Right, Move.Stop, Move.Up })
                {
                    Point next = point.Shift(dir);
                    if (condition.GetAt(next) == Element.WALL ||
                        (Parameters.CheckAfkPlayers && HandlersFacade.AfkPlayers.Contains(next)) ||
                        (condition.GetAt(next) == Element.DESTROYABLE_WALL && HandlersFacade.GetTimeToBoom(next) >= deep) ||
                        (Utils.IsActiveBomb(condition.GetAt(next)) && HandlersFacade.GetTimeToBoom(next) >= deep) ||
                        ((bomb != null) && next == bomb && newPointsAffectedByBlast[next] >= deep))
                        continue;

                    CheckIsDeadly(next, deep + 1);

                    if (isNotDeadly)
                        return;
                }
            }
        }

        private IReadOnlyDictionary<Point, int> GetNewPointsAffectedByBlast(Board condition, int moveMask, out Point? bomb)
        {
            Point? bombPoint = null;

            if ((moveMask & (int)Move.ActAfter) > 0)
                bombPoint = condition.GetBomberman().Shift(Utils.GetMoveByMask(moveMask));
            else if ((moveMask & (int)Move.ActBefore) > 0)
                bombPoint = condition.GetBomberman();

            Dictionary<Point, int> result = new Dictionary<Point, int>();
            bomb = bombPoint;
            if (bombPoint == null)
                return result;

            int timeToBoom = Math.Min(HandlersFacade.GetTimeToBoom((Point)bombPoint), 4);
            result.Add((Point)bombPoint, timeToBoom);

            foreach (var dir in new Move[] { Move.Down, Move.Left, Move.Right, Move.Up })
            {
                Point p = (Point)bombPoint;
                for (int i = 0; i < Parameters.BlastRadius; i++)
                {
                    p = p.Shift(dir);
                    result.Add(p, timeToBoom);

                    if (condition.GetAt(p) == Element.WALL ||
                        condition.GetAt(p) == Element.DESTROYABLE_WALL)
                        break;
                }
            }

            return result;
        }

        private ICollection<int> GetMovesMasks(ICollection<Move> directionsForShift, ICollection<Move> directionsForPlant) =>
            (from dirFS in directionsForShift
            select (int)dirFS)
            .ToList()
            .Concat
            (from dirFS in directionsForShift
            where directionsForPlant.Contains(dirFS) && !Parameters.OnlyActBeforeMode
            select (int)(dirFS | Move.ActAfter))
            .ToList()
            .Concat
            (from dirFS in directionsForShift
            where directionsForPlant.Contains(Move.Stop)
            select (int)(dirFS | Move.ActBefore))
            .ToList();

        private ICollection<Move> GetDirectionsForShift(Board condition)
        {
            Point bomberman = condition.GetBomberman();

            return (from dir in new Move[] { Move.Down, Move.Left, Move.Right, Move.Stop, Move.Up }
                    where HandlersFacade.IsFreeDirection[dir]
                    select dir)
                   .ToList();
        }

        private ICollection<Move> GetDirectionsForPlant(Board condition)
        {
            Point bomberman = condition.GetBomberman();

            return (from dir in new Move[] { Move.Down, Move.Left, Move.Right, Move.Stop, Move.Up }
                    where (HandlersFacade.GetTimeToBoom(bomberman.Shift(dir)) == HandlersFacade.WillNotExploded ||
                           HandlersFacade.IsMyBoom(bomberman.Shift(dir)) &&
                           !Utils.IsActiveBomb(condition.GetAt(bomberman.Shift(dir))))
                    select dir)
                   .ToList();
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _movesMasks = new List<int>();
        }

        protected override void Reset()
        {
            _movesMasks.Clear();
        }
    }
}
