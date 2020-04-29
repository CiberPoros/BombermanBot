using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;
using System.Linq;
using Bomberman.Logic.Extensions;
using Bomberman.Logic.Handlers.Facade;

namespace Bomberman.Logic.Handlers
{
    internal class BlastHandler : AbstractHandler<BlastHandler>
    {
        private int[,] _timeToBoom;
        private bool[,] _isMyBoom;

        internal int WillNotExploded => int.MaxValue;

        private BlastHandler() { }

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            HandleAllBombs(condition);
        }

        internal int GetTimeToBoom(Point point) => _timeToBoom[point.X, point.Y];

        internal bool IsMyBoom(Point point) => _isMyBoom[point.X, point.Y];

        private void HandleAllBombs(Board condition)
        {
            ICollection<Point> bombs = condition.GetBombs();
            Dictionary<Point, bool> used = bombs.ToDictionary(x => x, x => false);

            foreach (var bomb in bombs)
            {
                if (used[bomb])
                    continue;

                int timeToBoom = WillNotExploded;
                HashSet<Point> affectedPoints = new HashSet<Point>();
                bool isMyBoom = false;
                timeToBoom = HandleBomb(condition, bomb, affectedPoints, used, timeToBoom, ref isMyBoom);

                foreach (var p in affectedPoints)
                {
                    if (timeToBoom < _timeToBoom[p.X, p.Y])
                    {
                        _timeToBoom[p.X, p.Y] = timeToBoom;
                        _isMyBoom[p.X, p.Y] = isMyBoom;
                    }
                    else if (timeToBoom == _timeToBoom[p.X, p.Y])
                    {
                        _isMyBoom[p.X, p.Y] = isMyBoom || _isMyBoom[p.X, p.Y]; // mb synchronous boom with another player
                    }
                }
            }
        }

        private int HandleBomb(Board condition, Point bomb, HashSet<Point> affectedPoints,
                Dictionary<Point, bool> used, int timeToBoom, ref bool isMyBoom)
        {
            used[bomb] = true;
            affectedPoints.Add(bomb);

            int ttb = GetTimeToBoom(condition, bomb);
            if (ttb < timeToBoom)
            {
                timeToBoom = ttb;
                isMyBoom = HandlersFacade.MyBombs.Contains(bomb);
            }
            else if (ttb == timeToBoom)
            {
                isMyBoom = isMyBoom || HandlersFacade.MyBombs.Contains(bomb); // mb synchronous boom with another player
            }

            foreach (Move direction in new Move[] { Move.Left, Move.Right, Move.Up, Move.Down })
                checkDirection(direction, ref isMyBoom);

            void checkDirection(Move direction, ref bool isMB)
            {
                Point p = bomb;
                for (int i = 0; i < Parameters.BoomRadius; i++)
                {
                    p = p.Shift(direction);
                    affectedPoints.Add(p);

                    if (Utils.IsActiveBomb(condition.GetAt(p)))
                        if (!used[p])
                            timeToBoom = Math.Min(timeToBoom, HandleBomb(condition, p, affectedPoints, used, timeToBoom, ref isMB));

                    if (condition.GetAt(p) == Element.WALL ||
                        condition.GetAt(p) == Element.DESTROYABLE_WALL)
                        break;
                }
            }

            return timeToBoom;
        }

        private int GetTimeToBoom(Board board, Point bombPoint)
        {
            switch (board.GetAt(bombPoint))
            {
                case Element.BOMB_TIMER_1:          return 1;
                case Element.BOMB_TIMER_2:          return 2;
                case Element.BOMB_TIMER_3:          return 3;
                case Element.BOMB_TIMER_4:          return 4;
                case Element.BOMB_TIMER_5:          return 5;
                case Element.BOMB_BOMBERMAN:        return 4; // TODO: calc that
                case Element.OTHER_BOMB_BOMBERMAN:  return 4; // TODO: calc that
                default:                            throw new ArgumentException("Invalid bomb type");
            }
        }

        protected override void Reset()
        {
            for (int i = 0; i < MapSize; i++)
                for (int j = 0; j < MapSize; j++)
                {
                    _timeToBoom[i, j] = WillNotExploded;
                    _isMyBoom[i, j] = false;
                }
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _timeToBoom = new int[size, size];
            _isMyBoom = new bool[size, size];
        }
    }
}
