using Bomberman.Api;
using System;

namespace Bomberman.Logic.Handlers
{
    internal abstract class AbstractHandler<T> : IHandler
    {
        internal static T Instance { get; } = (T)Activator.CreateInstance(typeof(T), true);

        protected AbstractHandler() { }

        protected int MapSize { get; private set; }

        protected abstract void Reset();

        internal virtual void Init(int size) => MapSize = size;

        internal virtual void HandleCondition(Board condition)
        {
            Reset();
        }

        void IHandler.Init(int size)
        {
            Init(size);
        }

        void IHandler.HandleCondition(Board condition)
        {
            HandleCondition(condition);
        }
    }
}
