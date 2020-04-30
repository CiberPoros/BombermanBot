using Bomberman.Api;
using Bomberman.Logic.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Bomberman.Logic.Handlers
{
    internal class AfkPlayersHandler : AbstractHandler<AfkPlayersHandler>
    {
        private Dictionary<Point, int> _prevBombermansPointAndStayTime;
        private List<Point> _afkPlayers;

        internal IReadOnlyCollection<Point> AfkPlayers => _afkPlayers;

        internal override void HandleCondition(Board condition)
        {
            base.HandleCondition(condition);

            var players = condition.GetAliveOtherBombermans();
            _prevBombermansPointAndStayTime = (from kvp in _prevBombermansPointAndStayTime
                                               where players.Contains(kvp.Key)
                                               select kvp)
                                               .ToDictionary(kvp => kvp.Key, kvp => kvp.Value + 1);

            foreach (var p in players)
                if (!_prevBombermansPointAndStayTime.ContainsKey(p))
                    _prevBombermansPointAndStayTime.Add(p, 1);

            _afkPlayers = (from kvp in _prevBombermansPointAndStayTime
                           where kvp.Value >= Parameters.AfkIdentifyTicks
                           select kvp.Key)
                          .ToList();
        }

        internal override void Init(int size)
        {
            base.Init(size);

            _prevBombermansPointAndStayTime = new Dictionary<Point, int>();
            _afkPlayers = new List<Point>();
        }

        protected override void Reset()
        {
            _afkPlayers.Clear();
        }
    }
}
