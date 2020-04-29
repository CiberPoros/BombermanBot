using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    internal abstract class AbstractHandler<T> : IHandler
    {
        private static readonly T _instance = (T)Activator.CreateInstance(typeof(T), true);

        internal static T Instance => _instance;

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
