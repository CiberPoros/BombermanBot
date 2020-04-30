using Bomberman.Api;
using Bomberman.Logic.Extensions;
using Bomberman.Logic.Handlers.Facade;
using System.Collections.Generic;

namespace Bomberman.Logic.Handlers
{
    internal class DirectionsWeightHandler : AbstractHandler<DirectionsWeightHandler>
    {
        private Dictionary<Move, long> _weightOfDirection;

        public IReadOnlyDictionary<Move, long> WeightOfDirection => _weightOfDirection;

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            CalcWeights(condition);
        }

        private void CalcWeights(Board condition)
        {
            Point bomberman = condition.GetBomberman();

            foreach (var direction in new Move[] { Move.Down, Move.Left, Move.Right, Move.Up, Move.Stop })
                _weightOfDirection[direction] = GetWeightByBfs(condition, bomberman.Shift(direction));
        }

        long GetWeightByBfs(Board condition, Point startPoint)
        {
            Dictionary<Point, int> distance = new Dictionary<Point, int>();
            long weight = 0;

            Queue<Point> q = new Queue<Point>();
            q.Enqueue(startPoint);
            distance.Add(startPoint, 1);

            while (q.Count > 0)
            {
                Point point = q.Dequeue();
                if (HandlersFacade.GetTimeToBoom(point) == HandlersFacade.WillNotExploded)
                    weight += HandlersFacade.GetWeightOfPoint(point) / (Parameters.DirectionsWeightReducer * distance[point]);

                if (distance[point] >= Parameters.DeepForRecalcDirectionsWeight)
                    continue;

                if ((condition.GetAt(point) == Element.DESTROYABLE_WALL || Utils.IsActiveBomb(condition.GetAt(point))) &&
                        distance[point] < HandlersFacade.GetTimeToBoom(point)) // Иначе уничтожаемая стена (или бомба) будет взорвана к этому тику
                    continue;

                foreach (var direction in new Move[] { Move.Down, Move.Left, Move.Right, Move.Up })
                {
                    Point next = point.Shift(direction);
                    Element element = condition.GetAt(next);

                    if (element == Element.WALL)
                        continue;

                    if (!distance.ContainsKey(next))
                    {
                        q.Enqueue(next);
                        distance.Add(next, distance[point] + 1);
                    }
                }
            }

            return weight;
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _weightOfDirection = new Dictionary<Move, long>
            {
                { Move.Down,  0 },
                { Move.Up,    0 },
                { Move.Right, 0 },
                { Move.Left,  0 },
                { Move.Stop,  0 },
            };
        }

        protected override void Reset()
        {
            _weightOfDirection[Move.Down]  = 0;
            _weightOfDirection[Move.Up]    = 0;
            _weightOfDirection[Move.Right] = 0;
            _weightOfDirection[Move.Left]  = 0;
            _weightOfDirection[Move.Stop]  = 0;
        }
    }
}
