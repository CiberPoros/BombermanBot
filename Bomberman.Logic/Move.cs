using System;

namespace Bomberman.Logic
{
    [Flags]
    internal enum Move
    {
        Up          = 1,
        Down        = 2,
        Left        = 4,
        Right       = 8,
        Stop        = 16,
        ActBefore   = 32,
        ActAfter    = 64,
    }
}
