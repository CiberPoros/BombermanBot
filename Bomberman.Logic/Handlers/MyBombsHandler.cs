using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;
using System.Linq;

namespace Bomberman.Logic.Handlers
{
    internal class MyBombsHandler : AbstractHandler<MyBombsHandler>
    {
        private List<Point> _myBombs;

        internal IReadOnlyCollection<Point> MyBombs => _myBombs;

        protected MyBombsHandler() { }

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            _myBombs = (from bomb in _myBombs
                        where Utils.IsActiveBomb(condition.GetAt(bomb))
                        select (bomb))
                       .ToList();
        }

        internal bool IsMyBomb(Point bomb) => _myBombs.Contains(bomb);

        private void AddBomb(Point bomb)
        {
            if (!_myBombs.Contains(bomb))
                _myBombs.Add(bomb);
        }

        protected override void Reset() { }

        internal override void Init(int size)
        {
            base.Init(size);

            _myBombs = new List<Point>();
            Solver.OnPlantBomb += AddBomb;
        }
    }
}
