using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;
public class Player: Entity
{
    public float Speed;

    public Player(Vector2 position, float speed): base(position)
    {
        Speed = speed;
    }

    public void Update(GameTime gameTime, KeyboardState kstate)
    {
        if (kstate.IsKeyDown(Keys.Up))
        {
            Position.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (kstate.IsKeyDown(Keys.Down))
        {
            Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        Position.Y = MathHelper.Clamp(Position.Y, WorldValues.minBoundaries.Y + Texture.Height / 2, WorldValues.maxBoundaries.Y - Texture.Height / 2);
    }
}
