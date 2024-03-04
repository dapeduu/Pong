using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

public class Enemy: Entity
{
    public float Speed;
    public enum MoveState { UP, DOWN, STOPPED };
    public MoveState State;

    // Ai stuff
    public Vector2? Prediction;
    public double ReactionTime;
    public double LastTimePredicted;
    public int Error;

    public Enemy(Vector2 position, float speed) : base(position)
    {
        Speed = speed;
        State = MoveState.STOPPED;
        ReactionTime = 0.5;
    }

    public void Update(GameTime gameTime, Ball ball)
    {
        if (ball.Direction.X > 0)
        {
            State = MoveState.STOPPED;
            return;
        }

        switch (State)
        {
            case MoveState.UP:
                Position.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            case MoveState.DOWN:
                Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            case MoveState.STOPPED:
                break;
        }

        Position.Y = MathHelper.Clamp(Position.Y, WorldValues.minBoundaries.Y + Texture.Height / 2, WorldValues.maxBoundaries.Y - Texture.Height / 2);

        var currentTime = gameTime.TotalGameTime.TotalSeconds;


        // Only repredicts if the reaction time has passed
        if (Prediction.HasValue &&
            currentTime - LastTimePredicted < ReactionTime)
        {
            return;
        }

        var startLine = new Vector2(Position.X, -10000);
        var endLine = new Vector2(Position.X, 10000);

        var ballStartLine = ball.Position;
        var ballEndLine = ball.Position + ball.Direction * 10000;

        var pointOfIntersection = FindIntersection(startLine, endLine, ballStartLine, ballEndLine);

        Prediction = pointOfIntersection.HasValue ? pointOfIntersection.Value : null;
     

        if (Prediction.HasValue)
        {
            LastTimePredicted = (float)gameTime.TotalGameTime.TotalSeconds;
            Prediction = new Vector2(Prediction.Value.X,
                                    Prediction.Value.Y + new Random().Next(-Error, Error));

            if (Prediction.Value.Y >= Position.Y - Texture.Height / 2 && Prediction.Value.Y <= Position.Y + Texture.Height / 2)
            {
                State = MoveState.STOPPED;
            }
            else if (Prediction.Value.Y < Position.Y)
            {
                State = MoveState.UP;
            }
            else if (Prediction.Value.Y > Position.Y)
            {
                State = MoveState.DOWN;
            }
        }

    }

    // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
    private static Vector2? FindIntersection(Vector2 aLineStart, Vector2 aLineEnd, Vector2 bLineStart, Vector2 bLineEnd)
    {
        var denominator = (aLineStart.X - aLineEnd.X) * (bLineStart.Y - bLineEnd.Y) - (aLineStart.Y - aLineEnd.Y) * (bLineStart.X - bLineEnd.X);

        // Check if the lines are parallel
        if (denominator == 0)
        {
            return null;
        }

        var t = ((aLineStart.X - bLineStart.X) * (bLineStart.Y - bLineEnd.Y) - (aLineStart.Y - bLineStart.Y) * (bLineStart.X - bLineEnd.X)) / denominator;
        var u = -((aLineStart.X - aLineEnd.X) * (aLineStart.Y - bLineStart.Y) - (aLineStart.Y - aLineEnd.Y) * (aLineStart.X - bLineStart.X)) / denominator;

        // Check if the intersection point lies within the line segments
        if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
        {
            return new Vector2(aLineStart.X + t * (aLineEnd.X - aLineStart.X), aLineStart.Y + t * (aLineEnd.Y - aLineStart.Y));
        }

        return null;
    }
}
