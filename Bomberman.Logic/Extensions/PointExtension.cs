using Bomberman.Api;
using System;

namespace Bomberman.Logic.Extensions
{
	internal static class PointExtension
    {
		internal static Point Shift(this Point p, Move direction)
		{
			switch (direction)
			{
				case Move.Down:  return p.ShiftBottom();
				case Move.Up:	 return p.ShiftTop();
				case Move.Left:  return p.ShiftLeft();
				case Move.Right: return p.ShiftRight();
				case Move.Stop:	 return p;
				default:		 throw new ArgumentException();
			}
		}
	}
}
