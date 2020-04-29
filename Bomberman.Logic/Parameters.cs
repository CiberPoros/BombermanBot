using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;

namespace Bomberman.Logic
{
    public static class Parameters
    {
        /// <summary>
        /// Радиус взрыва
        /// </summary>
        public const int BoomRadius = 3;

        /// <summary>
        /// Количество тиков, после которого игрок считается стоящим АФК
        /// </summary>
        public const int AfkIdentifyTicks = 5;

        /// <summary>
        /// Коэффициент, влияющий на то, насколько возможность коллизии будет уменьшать вес возможного хода
        /// </summary>
        public const int CollisionReducerWeight = 3;

        /// <summary>
        /// Количество тиков, на которые будут просчитываться ходы других игроков
        /// </summary>
        public const int DeepForRecalcWeightsUnits = 3;

        /// <summary>
        /// Количество тиков, на которые будут просчитываться ходы возможные ходы моего персонажа
        /// </summary>
        public const int DeepForRecalcDirectionsWeight = 7;

        /// <summary>
        /// Количество тиков, на которые будут просчитываться ходы персонажа и проверяться на смерть с учетом поставленных ранее бомб
        /// </summary>
        public const int DeepForCheckDeadlyMoves = 5;

        /// <summary>
        /// Коэффициент, на который будет уменьшаться рассчет веса клетки при увеличении глубины рекурсии во время обхода для юнитов с весом
        /// </summary>
        public const int UnitsWeightReducer = 3;

        /// <summary>
        /// Коэффициент, на который будет уменьшаться рассчет веса клетки при увеличении глубины рекурсии во время обхода для юнитов с весом
        /// </summary>
        public const int DirectionsWeightReducer = 3;

        /// <summary>
        /// Вес вражеского юнита (не обязательно его очки за убийство!)
        /// </summary>
        public const long BombermanWeight = 7000000000000;

        /// <summary>
        /// Вес мясника (не обязательно его очки за убийство!)
        /// </summary>
        public const long MeatChopperWeight = 2500000000000;

        /// <summary>
        /// Вес уничтожаемой стены (не обязательно её очки за разрушение!)
        /// </summary>
        public const long WallWeight = 200000000000;

        /// <summary>
        /// Вес смерти
        /// </summary>
        public const int DeathWeight = 600; // TODO: will be used?

        /// <summary>
        /// Бомбермен будет ставить бомбы только до движения
        /// </summary>
        public const bool OnlyActBeforeMode = true;

        /// <summary>
        /// Будет проверять на наличие игроков, стоящих афк
        /// </summary>
        public const bool CheckAfkPlayers = true;

        internal static long GetWeightOfElement(Element element)
        {
            switch (element)
            {
                case Element.OTHER_BOMBERMAN:       return BombermanWeight;
                case Element.OTHER_BOMB_BOMBERMAN:  return BombermanWeight;
                case Element.MEAT_CHOPPER:          return MeatChopperWeight;
                case Element.DESTROYABLE_WALL:      return WallWeight;
                default:                            throw new ArgumentException("Invalid element type.", nameof(element));
            }
        }
    }
}
