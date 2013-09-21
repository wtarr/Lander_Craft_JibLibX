using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Lander_Craft_JibLibX
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameObject : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Model Model;
        public Vector3 Position;
        public float Rotation;
        protected Vector3 Scale;
        protected Vector3 Forward;
        protected Vector3 Up;
        protected Vector3 Right;

        public GameObject(Game game, Model model)
            : base(game)
        {
            // TODO: Construct any child components here
            this.Model = model;
            game.Components.Add(this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Model != null)
            {
                Console.WriteLine("drawing");

                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[Model.Bones.Count];
                Model.CopyAbsoluteBoneTransformsTo(transforms);

                // Draw the model. A model can have multiple meshes, so loop.
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    // This is where the mesh orientation is set, as well 
                    // as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index] *
                            Matrix.CreateRotationY(Rotation)
                            * Matrix.CreateTranslation(Position);
                        //effect.View = Matrix.CreateLookAt(Camera.Position,
                        //    Vector3.Zero, Vector3.Up);
                        effect.View = Camera.View();

                        effect.Projection = Camera.Projection();
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }
            
            base.Draw(gameTime);
        }
    }
}
