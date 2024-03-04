namespace Pong;

using System;
using Microsoft.Xna.Framework;

public class DirectionsHelper
{
    public static Vector2 GetRandomDirection(bool right)
    {
        double x, y;
        var random = new Random();

        x = right ? 1 : -1;
        y = Math.Clamp(random.NextDouble() * 2 - 1, -0.7, 0.7);

        return new Vector2((float)x, (float)y);
    }
}