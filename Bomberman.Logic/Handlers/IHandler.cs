using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    internal interface IHandler
    {
        void Init(int size);

        void HandleCondition(Board condition);
    }
}
