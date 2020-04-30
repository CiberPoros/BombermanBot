using Bomberman.Api;
using Bomberman.Logic.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Bomberman.Logic.Handlers
{
    internal class FreeDirectionsHandler : AbstractHandler<FreeDirectionsHandler>
    {
        private Dictionary<Move, bool> _isFreeDirection;

        internal IReadOnlyDictionary<Move, bool> IsFreeDirection => _isFreeDirection;

        private FreeDirectionsHandler() { }

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            Point bomberman = condition.GetBomberman();

            foreach (Move direction in _isFreeDirection.Keys.ToArray())
                if (Utils.IsBarrier(condition.GetAt(bomberman.Shift(direction))))
                    _isFreeDirection[direction] = false;
        }

        protected override void Reset()
        {
            _isFreeDirection[Move.Down]  = true;
            _isFreeDirection[Move.Up]    = true;
            _isFreeDirection[Move.Right] = true;
            _isFreeDirection[Move.Left]  = true;
            _isFreeDirection[Move.Stop]  = true;
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _isFreeDirection = new Dictionary<Move, bool>()
            {
                { Move.Left,  true },
                { Move.Right, true },
                { Move.Down,  true },
                { Move.Up,    true },
                { Move.Stop,  true },
            };
        }
    }
}
