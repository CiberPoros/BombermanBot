using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;
using Bomberman.Logic.Extensions;
using System.Linq;

namespace Bomberman.Logic.Handlers
{
    internal class CollisionHandler : AbstractHandler<CollisionHandler>
    {
        private Dictionary<Move, bool> _canMakeCollision;
        internal IReadOnlyDictionary<Move, bool> CanMakeCollision => _canMakeCollision;

        protected CollisionHandler() { }

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            Point bomberman = condition.GetBomberman();
            foreach (var move in _canMakeCollision.Keys.ToArray())
                _canMakeCollision[move] = CanMakeCollisionPoint(condition, bomberman.Shift(move));
        }

        private bool CanMakeCollisionPoint(Board condition, Point p)
        {
            foreach (var direction in new Move[] { Move.Down, Move.Left, Move.Right, Move.Up, Move.Stop, })
            {
                Element element = condition.GetAt(p.Shift(direction));
                if (element == Element.MEAT_CHOPPER ||
                    element == Element.OTHER_BOMB_BOMBERMAN ||
                    element == Element.OTHER_BOMBERMAN)
                    return true;
            }
            return false;
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _canMakeCollision = new Dictionary<Move, bool>()
            {
                { Move.Left,  false },
                { Move.Right, false },
                { Move.Down,  false },
                { Move.Up,    false },
                { Move.Stop,  false },
            };
        }

        protected override void Reset()
        {
            _canMakeCollision[Move.Down]  = false;
            _canMakeCollision[Move.Up]    = false;
            _canMakeCollision[Move.Right] = false;
            _canMakeCollision[Move.Left]  = false;
            _canMakeCollision[Move.Stop]  = false;
        }
    }
}
