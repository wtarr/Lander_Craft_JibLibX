using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Lander_Craft_JibLibX.PhysicsObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using JigLibX.Physics;

using JigLibX.Geometry;
using JigLibX.Collision;

namespace Lander_Craft_JibLibX
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LanderGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // Tutorial
        //private float _aspectRatio;
        //private Vector3 modelPosition = Vector3.Zero;
        //private float _modelRotation = 0.0f;
        //private Vector3 _cameraPosition = new Vector3(0.0f, 20.0f, 100.0f);

        //private BoxActor _fallingBox, _immovableBox;

        private BoxObject _landerPhysicsObject, _surfacePhysicsObj;
        private HeightmapObject _map;
        private Model _model;
        private CharacterController _cc;
        private SpriteFont font;

        public LanderGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            InitializePhysics();
            
            Camera.Position = new Vector3(0.0f, 20.0f, 100.0f);
            
            Camera.FarPlane = 10000;
            Camera.NearPlane = 1;

            
        }

        private void InitializePhysics()
        {
            PhysicsSystem world = new PhysicsSystem();
            world.CollisionSystem = new CollisionSystemSAP(); //Sweep and Prune

         
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
           //GameObject
            //go = new GameObject(this, model);

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _model = Content.Load<Model>("rocket");
            _landerPhysicsObject = new BoxObject(this, _model, new Vector3(10, 10, 10), Matrix.Identity, new Vector3(0, 50, 0));
            _cc = new CharacterController();
            _cc.Initialize(_landerPhysicsObject.Body);

            _model = Content.Load<Model>("box");
            _surfacePhysicsObj = new BoxObject(this, _model, new Vector3(20, 20, 20), Matrix.Identity, new Vector3(0, -5, 0));
            _surfacePhysicsObj.Body.Immovable = true;  // Update contructor to handle this !!!!!! put body back to being protected

            Model terrain = Content.Load<Model>("terrain");
            _map = new HeightmapObject(this, terrain, Vector2.Zero );

            Camera.AspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            ParticleManager.Initialize(
                GraphicsDevice,
                Content.Load<Effect>("Particles"),
                Content.Load<Texture2D>("Explosion"));

            font = Content.Load<SpriteFont>("Font");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _cc.boost = true;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _cc.fore = true;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                _cc.aft = true;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                _cc.port = true;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                _cc.starboard = true;

            // TODO: Add your update logic here
            if (_cc.boost)
            {
                ParticleManager.MakeExplosion(_cc.Body.Position , 20);
            }

            ParticleManager.Update(gameTime);


            base.Update(gameTime);

            float timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            PhysicsSystem.CurrentPhysicsSystem.Integrate(timeStep);
            
            Camera.Position = new Vector3(100, 50, 100);
            Camera.Target = _landerPhysicsObject.Position;
            Camera.Up = Vector3.Up;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Boost - Space\nFore RCS - W\nAft RCS -S\nPort RCS - A\nStarboard RCS - D", new Vector2(20, 20), Color.Black);
            spriteBatch.End();
            
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphics.GraphicsDevice.SetRenderTarget(null);

            base.Draw(gameTime);
            
            ParticleManager.Draw();
        }
    }
}
