using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;
public abstract class Entity
{
    public Vector2 Position;
    public Texture2D Texture;

    protected Entity(Vector2 position)
    {
        Position = position;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Position,
            null,
            Color.White,
            0f,
            new Vector2(Texture.Width / 2, Texture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f);
    }
}

