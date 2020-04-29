using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;
using Bomberman.Logic.Handlers.Facade;
using Bomberman.Logic.Extensions;
using System.Linq;

namespace Bomberman.Logic.Handlers
{
    internal class PointsWeightHandler : AbstractHandler<PointsWeightHandler>
    {
        private long[,] _pointsWeight;

        internal long GetWeightOfPoint(Point point) => _pointsWeight[point.X, point.Y];

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            CalcAllWeights(condition);
        }

        private void CalcAllWeights(Board condition)
        {
            for (int i = 0; i < MapSize; i++)
                for (int j = 0; j < MapSize; j++)
                    CalcWeightForPoint(condition, new Point(i, j));
        }

        private void CalcWeightForPoint(Board condition, Point point)
        {
            Element element = condition.GetAt(point);

            if (element == Element.OTHER_BOMBERMAN ||
                element == Element.OTHER_BOMB_BOMBERMAN ||
                element == Element.MEAT_CHOPPER ||
                element == Element.DESTROYABLE_WALL)
            {
                long weight = Parameters.GetWeightOfElement(element);

                if (Utils.IsStaticUnit(condition, point))
                    _pointsWeight[point.X, point.Y] += weight;
                else
                    DfsWithoutCheckUsed(condition, point, weight);
            }    
        }

        private void DfsWithoutCheckUsed(Board condition, Point point, long weight, int deep = 0)
        {
            _pointsWeight[point.X, point.Y] += weight;

            if (deep >= Parameters.DeepForRecalcWeightsUnits)
                return;

            List<Point> nextPoints = new List<Point>(5);
            foreach (var direction in new Move[] { Move.Down, Move.Left, Move.Right, Move.Up, Move.Stop })
            {
                Point next = point.Shift(direction);
                Element element = condition.GetAt(next);

                if (element == Element.WALL)
                    continue;

                if ((element == Element.DESTROYABLE_WALL || Utils.IsActiveBomb(element)) &&
                    deep < HandlersFacade.GetTimeToBoom(point)) // Иначе уничтожаемая стена (или бомба) будет взорвана к этому тику
                    continue;

                nextPoints.Add(next);
            }

            foreach (var next in nextPoints)
                DfsWithoutCheckUsed(condition, next, (weight / Parameters.UnitsWeightReducer) / nextPoints.Count, deep + 1);
        }

        protected override void Reset()
        {
            for (int i = 0; i < MapSize; i++)
                for (int j = 0; j < MapSize; j++)
                    _pointsWeight[i, j] = 0;
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _pointsWeight = new long[size, size];
        }
    }
}
