using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Logic.Handlers;
using Bomberman.Logic.Handlers.Facade;
using Bomberman.Api;
using Bomberman.Logic.Extensions;
using System.Linq;

namespace Bomberman.Logic
{
    public static class Solver
    {
        private static List<IHandler> _handlers;

        public static event Action<Point> OnPlantBomb;

        public static void Init(int size)
        {
            _handlers = new List<IHandler>()
            {
                FreeDirectionsHandler.Instance,
                MyBombsHandler.Instance,
                CollisionHandler.Instance,
                AfkPlayersHandler.Instance,
                BlastHandler.Instance,
                PointsWeightHandler.Instance,
                DirectionsWeightHandler.Instance,
                MovesHandler.Instance,
            };

            foreach (var handler in _handlers)
                handler.Init(size);
        }

        public static string Solve(Board condition)
        {
            if (_handlers == null)
                Init(condition.BoardSize);

            foreach (var handler in _handlers)
                handler.HandleCondition(condition);

            IReadOnlyCollection<int> movesMasks = HandlersFacade.MovesMasks;

            ICollection<int> masks = (from mask in movesMasks
                                      where (mask & (int)(Move.ActAfter | Move.ActBefore)) > 0
                                      select mask)
                                     .ToList();

            if (masks.Count == 0)
                masks = new List<int>(movesMasks);

            int moveMask = masks.Count > 0 ? masks.First() : (int)Move.Stop;
            foreach (int mask in masks)
            {
                if (HandlersFacade.WeightOfDirection[Utils.GetMoveByMask(mask)] > 
                    HandlersFacade.WeightOfDirection[Utils.GetMoveByMask(moveMask)])
                    moveMask = mask;
            }

            if ((moveMask & (int)(Move.ActAfter | Move.ActBefore)) > 0)
                OnPlantBomb((moveMask & (int)Move.ActBefore) > 0 ? condition.GetBomberman() : 
                    condition.GetBomberman().Shift(Utils.GetMoveByMask(moveMask)));

            foreach (var d in new Move[] { Move.Down, Move.Left, Move.Right, Move.Stop, Move.Up })
                Console.WriteLine($"{ d }: { HandlersFacade.GetTimeToBoom(condition.GetBomberman().Shift(d)) }");
            return Utils.MoveMaskToString(moveMask);
        }

        public static string SolveOld(Board condition)
        {
            if (_handlers == null)
                Init(condition.BoardSize);

            foreach (var handler in _handlers)
                handler.HandleCondition(condition);

            List<Move> directions = (from kvp in HandlersFacade.IsFreeDirection
                                     where kvp.Value
                                     select kvp.Key)
                                    .ToList();

            Point bomberman = condition.GetBomberman();
            directions = (from dir in directions
                          where HandlersFacade.GetTimeToBoom(bomberman.Shift(dir)) == HandlersFacade.WillNotExploded
                          select dir)
                         .ToList();

            Move direction = directions.Count > 0 ? directions.First() : Move.Stop;
            foreach (var dir in directions)
                if (HandlersFacade.WeightOfDirection[dir] > HandlersFacade.WeightOfDirection[direction])
                    direction = dir;

            foreach (var dir in directions)
                Console.WriteLine($"{ HandlersFacade.WeightOfDirection[dir] } --- { dir }");
            return Utils.MoveMaskToString((int)direction);
        }
    }
}
