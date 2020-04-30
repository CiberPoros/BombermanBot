using Bomberman.Api;
using System;
using System.Configuration;

namespace Bomberman.Logic
{
    public static class Parameters
    {
        /// <summary>
        /// Радиус взрыва
        /// </summary>
        public static int BlastRadius => int.Parse(ConfigurationManager.AppSettings.Get("BlastRadius"));

        /// <summary>
        /// Количество тиков, после которого игрок считается стоящим АФК
        /// </summary>
        public static int AfkIdentifyTicks => int.Parse(ConfigurationManager.AppSettings.Get("AfkIdentifyTicks"));

        /// <summary>
        /// Коэффициент, влияющий на то, насколько возможность коллизии будет уменьшать вес возможного хода
        /// </summary>
        public static int CollisionReducerWeight => int.Parse(ConfigurationManager.AppSettings["CollisionReducerWeight"]);

        /// <summary>
        /// Количество тиков, на которые будут просчитываться ходы других игроков
        /// </summary>
        public static int DeepForRecalcWeightsUnits => int.Parse(ConfigurationManager.AppSettings["DeepForRecalcWeightsUnits"]);

        /// <summary>
        /// Количество тиков, на которые будут просчитываться ходы возможные ходы моего персонажа
        /// </summary>
        public static int DeepForRecalcDirectionsWeight => int.Parse(ConfigurationManager.AppSettings["DeepForRecalcDirectionsWeight"]);

        /// <summary>
        /// Количество тиков, на которые будут просчитываться ходы персонажа и проверяться на смерть с учетом поставленных ранее бомб
        /// </summary>
        public static int DeepForCheckDeadlyMoves => int.Parse(ConfigurationManager.AppSettings["DeepForCheckDeadlyMoves"]);

        /// <summary>
        /// Коэффициент, на который будет уменьшаться рассчет веса клетки при увеличении глубины рекурсии во время обхода для юнитов с весом
        /// </summary>
        public static int UnitsWeightReducer => int.Parse(ConfigurationManager.AppSettings["UnitsWeightReducer"]);

        /// <summary>
        /// Коэффициент, на который будет уменьшаться рассчет веса клетки при увеличении глубины рекурсии во время обхода для юнитов с весом
        /// </summary>
        public static int DirectionsWeightReducer => int.Parse(ConfigurationManager.AppSettings["DirectionsWeightReducer"]);

        /// <summary>
        /// Вес вражеского юнита (не обязательно его очки за убийство!)
        /// </summary>
        public static long BombermanWeight => long.Parse(ConfigurationManager.AppSettings["BombermanWeight"]);

        /// <summary>
        /// Вес мясника (не обязательно его очки за убийство!)
        /// </summary>
        public static long MeatChopperWeight => long.Parse(ConfigurationManager.AppSettings["MeatChopperWeight"]);

        /// <summary>
        /// Вес уничтожаемой стены (не обязательно её очки за разрушение!)
        /// </summary>
        public static long WallWeight => long.Parse(ConfigurationManager.AppSettings["WallWeight"]);

        /// <summary>
        /// Вес смерти
        /// </summary>
        public static long DeathWeight => long.Parse(ConfigurationManager.AppSettings["DeathWeight"]);

        /// <summary>
        /// Бомбермен будет ставить бомбы только до движения
        /// </summary>
        public static bool OnlyActBeforeMode => bool.Parse(ConfigurationManager.AppSettings["OnlyActBeforeMode"]);

        /// <summary>
        /// Будет проверять на наличие игроков, стоящих афк
        /// </summary>
        public static bool CheckAfkPlayers => bool.Parse(ConfigurationManager.AppSettings["CheckAfkPlayers"]);

        /// <summary>
        /// Будет проверять на наличие игроков, стоящих афк
        /// </summary>
        public static bool CheckCollision => bool.Parse(ConfigurationManager.AppSettings["CheckCollision"]);

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
