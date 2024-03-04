using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong;

public enum Winner
{
    Player,
    Enemy,
    None
}

public class Game1 : Game
{
    public Player player;
    public Enemy enemy;
    public Ball ball;
    public Texture2D board;
    public Texture2D scoreBar;
    public SpriteFont font;
    public int playerPoints, enemyPoints;
    public bool isPaused;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        isPaused = false;
        playerPoints = 0;
        enemyPoints = 0;
    }

    protected override void Initialize()
    {
        // System changes
        const int scoreBarHeight = 47;
        const int boardHeight = 455;
        const int boardWidth = 802;

        _graphics.PreferredBackBufferWidth = boardWidth;
        _graphics.PreferredBackBufferHeight = scoreBarHeight + boardHeight;
        _graphics.ApplyChanges();

        // Initialization

        WorldValues.maxBoundaries = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        WorldValues.minBoundaries = new Vector2(0, scoreBarHeight);

        player = new Player(
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2),
            500f
        );

        enemy = new Enemy(
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2),
            500f
            );

        ball = new Ball(
            new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
            400f
            );

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        player.Texture = Content.Load<Texture2D>("assets/arts/Player");
        player.Position.X = _graphics.PreferredBackBufferWidth - player.Texture.Width;

        enemy.Texture = Content.Load<Texture2D>("assets/arts/Computer");
        enemy.Position.X = enemy.Texture.Width;

        ball.Texture = Content.Load<Texture2D>("assets/arts/Ball");

        string[] songs = {"music/pongblipf4", "music/ponblipg5", "music/pongblipa3", "music/pongblipa4"};
        foreach (string s in songs)
        {
            var song = Content.Load<SoundEffect>(s);
            ball.Songs.Add(song);
        }

        board = Content.Load<Texture2D>("assets/arts/Board");
        scoreBar = Content.Load<Texture2D>("assets/arts/ScoreBar");

        font = Content.Load<SpriteFont>("File");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.P))
        {
            isPaused = false;
        }

        if (kstate.IsKeyDown(Keys.R))
        {
            playerPoints = 0;
            enemyPoints = 0;
            ResetPositionsAndPause();
        }

        if (!isPaused)
        {
            player.Update(gameTime, kstate);
            enemy.Update(gameTime, ball);
            ball.Update(gameTime, player, enemy);

            switch (ball.GetWinner())
            {
                case Winner.Player:
                    playerPoints += 1;
                    ball.Direction = DirectionsHelper.GetRandomDirection(false);
                    enemy.ReactionTime -= 0.2;
                    ResetPositionsAndPause();
                    break;
                case Winner.Enemy:
                    enemyPoints += 1;
                    ball.Direction = DirectionsHelper.GetRandomDirection(true);
                    enemy.ReactionTime += 0.1;
                    ResetPositionsAndPause();
                    break;
                case Winner.None: break;
            }
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        DrawBoard(_spriteBatch);
        DrawPlayerScoreBar(_spriteBatch);
        DrawEnemyScoreBar(_spriteBatch);
        player.Draw(_spriteBatch);
        enemy.Draw(_spriteBatch);
        ball.Draw(_spriteBatch);
        DrawText(_spriteBatch, 
                new Vector2(_graphics.PreferredBackBufferWidth / 2 + 100, 
                            scoreBar.Height / 2 - 12), 
                playerPoints.ToString());
        DrawText(_spriteBatch,
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - 112, 
                            scoreBar.Height / 2 - 12),
                enemyPoints.ToString());
        DrawText(_spriteBatch,
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - 6,
                            scoreBar.Height / 2 - 12),
                            ((int)gameTime.TotalGameTime.TotalSeconds).ToString());


        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawBoard(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            board,
            new Vector2(0, scoreBar.Height),
            null,
            Color.White,
            0,
            Vector2.Zero,
            new Vector2(1),
            SpriteEffects.None,
            0f
         );
    }

    private void DrawPlayerScoreBar(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            scoreBar,
            new Vector2(_graphics.PreferredBackBufferWidth, 0),
            null,
            Color.White,
            0,
            new Vector2(scoreBar.Width, 0),
            new Vector2(1),
            SpriteEffects.FlipHorizontally,
            0f
         );
    }

    private void DrawEnemyScoreBar(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            scoreBar,
            Vector2.Zero,
            null,
            Color.White,
            0,
            Vector2.Zero,
            new Vector2(1),
            SpriteEffects.None,
            0f
         );
    }

    private void ResetPositionsAndPause()
    {
        player.Position = new Vector2(_graphics.PreferredBackBufferWidth - player.Texture.Width, _graphics.PreferredBackBufferHeight / 2);
        enemy.Position = new Vector2(enemy.Texture.Width, _graphics.PreferredBackBufferHeight / 2);
        ball.Position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        ball.Speed = 400f;
        isPaused = true;
    }

    private void DrawText(SpriteBatch spriteBatch, Vector2 position, string text)
    {
        var color = Color.White;
        spriteBatch.DrawString(
            font, 
            text, 
            position, 
            color, 
            0,
            new Vector2(0, 0), 
            1,
            SpriteEffects.None, 
            1
       );
    }
}

