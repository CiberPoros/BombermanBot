using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers.Facade
{
    internal static class HandlersFacade
    {
        /// <summary>
        /// Свободные направления для следующего тика
        /// </summary>
        internal static IReadOnlyDictionary<Move, bool> IsFreeDirection => FreeDirectionsHandler.Instance.IsFreeDirection;

        /// <summary>
        /// Список бомб, поставленных мной
        /// </summary>
        internal static IReadOnlyCollection<Point> MyBombs => MyBombsHandler.Instance.MyBombs;
        /// <summary>
        /// Проверка, моя ли бомба
        /// </summary>
        /// <param name="bomb"></param>
        /// <returns></returns>
        internal static bool IsMyBomb(Point bomb) => MyBombsHandler.Instance.IsMyBomb(bomb);

        /// <summary>
        /// Определяет, возможна ли коллизия для заданного направления в следующий тик
        /// </summary>
        internal static IReadOnlyDictionary<Move, bool> CanMakeCollision => CollisionHandler.Instance.CanMakeCollision;

        /// <summary>
        /// Список игроков, которые, предполагаемо, стоят АФК
        /// </summary>
        internal static IReadOnlyCollection<Point> AfkPlayers => AfkPlayersHandler.Instance.AfkPlayers;

        /// <summary>
        /// Значение, означающее, что в данной точне не предвидится взрыв
        /// </summary>
        internal static int WillNotExploded => BlastHandler.Instance.WillNotExploded;
        /// <summary>
        /// Возвращает количество тиков до взрыва в точке
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal static int GetTimeToBoom(Point point) => BlastHandler.Instance.GetTimeToBoom(point);
        /// <summary>
        /// Определяет, будет ли эта клетка взорвана именно моим взрывом
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal static bool IsMyBoom(Point point) => BlastHandler.Instance.IsMyBoom(point);

        /// <summary>
        /// Возвращает вес клетки
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal static long GetWeightOfPoint(Point point) => PointsWeightHandler.Instance.GetWeightOfPoint(point);

        /// <summary>
        /// Возвращает веса для направлений
        /// </summary>
        internal static IReadOnlyDictionary<Move, long> WeightOfDirection => DirectionsWeightHandler.Instance.WeightOfDirection;

        /// <summary>
        /// Возвращает маски всех возможных ходов, с учетом того, что персонаж может туда ходить и может выжить после этого 
        /// </summary>
        internal static IReadOnlyCollection<int> MovesMasks => MovesHandler.Instance.MovesMasks;
    }
}
