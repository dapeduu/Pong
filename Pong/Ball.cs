using System.Diagnostics;
using Microsoft.Xna.Framework;
namespace Pong;

public class Ball: Entity
{
    public Vector2 Direction;
    public float Speed;

    public Ball(Vector2 position, float speed) : base(position)
    {
        Speed = speed;
        Direction = DirectionsHelper.GetRandomDirection(false);
    }

    public void Update(GameTime gameTime, Player player, Enemy enemy)
    {
        Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        HandleWallsCollision();
        HandleCollisionWithEntity(enemy);
        HandleCollisionWithEntity(player);
    }

    public Winner GetWinner() 
    {
        if (Position.X - Texture.Width / 2 < WorldValues.minBoundaries.X)
        {
            return Winner.Player;
        } 
        else if (Position.X + Texture.Width / 2 > WorldValues.maxBoundaries.X)
        {
            return Winner.Enemy;
        }
        else
        {
            return Winner.None;
        };
    }

    private void HandleWallsCollision()
    {
        var ballTopLeft = Position - new Vector2(Texture.Width / 2, Texture.Height / 2);
        

        if (ballTopLeft.Y < WorldValues.minBoundaries.Y || ballTopLeft.Y + Texture.Height > WorldValues.maxBoundaries.Y)
        {
            Direction.Y *= -1;
        }

        if (ballTopLeft.X < WorldValues.minBoundaries.X || ballTopLeft.X + Texture.Width > WorldValues.maxBoundaries.X)
        {
            Direction.X *= -1;
        }
    }

    // Maps the given x to its corresponding value in the range
    // output_start + ((output_end - output_start) * (input - input_start)) / (input_end - input_start)
    // https://stackoverflow.com/questions/5731863/mapping-a-numeric-range-onto-another
    private float MapRange(float x, float xMin, float xMax, float yMin, float yMax)
    {

        return yMin + ((yMax - yMin) * (x - xMin)) / (xMax - xMin) ;
    }

    // If the ball collided with the entity
    // Add speed and change direction based on where it collided
    private void HandleCollisionWithEntity(Entity entity)
    {
        var ballTopLeftPoint = Position - new Vector2(Texture.Width / 2, Texture.Height / 2);
        var ballBottomRight = Position + new Vector2(Texture.Width / 2, Texture.Height / 2);

        var entityTopLeftPoint = entity.Position - new Vector2(entity.Texture.Width / 2, entity.Texture.Height / 2);
        var entityBottomRight = entity.Position + new Vector2(entity.Texture.Width / 2, entity.Texture.Height / 2);
        
        if (ballTopLeftPoint.X < entityTopLeftPoint.X + entity.Texture.Width &&
            ballTopLeftPoint.X + Texture.Width > entityTopLeftPoint.X &&
            ballTopLeftPoint.Y < entityTopLeftPoint.Y + entity.Texture.Height &&
            ballTopLeftPoint.Y + Texture.Height > entityTopLeftPoint.Y)
        {
            Direction.X *= -1;
            Direction.Y = MapRange(Position.Y, entityTopLeftPoint.Y, entityBottomRight.Y, -1f, 1f);
            Speed += 2;
        }
    }

}


