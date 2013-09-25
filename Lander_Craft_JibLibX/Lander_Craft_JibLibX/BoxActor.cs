using System;
using System.Collections.Generic;
using System.Linq;
using JigLibX.Math;
using JigLibX.Physics;
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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BoxActor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Vector3 Position { get; private set; }
        public Vector3 Scale { get; private set; }
        private Model _landerModel;
        public Body Body { get; private set; }
        public CollisionSkin Skin { get; private set; }
        
        public BoxActor(Game game, Vector3 position, Vector3 scale)
            : base(game)
        {
            // TODO: Construct any child components here
            this.Position = position;
            this.Scale = scale;

            Body = new Body();
            Skin = new CollisionSkin(Body);

            Body.CollisionSkin = Skin;

            Box box = new Box(Vector3.Zero, Matrix.Identity, scale);
            Skin.AddPrimitive(box, new MaterialProperties(0.8f, 0.8f, 0.7f)); // Elasticity, StaticRoughness, Dynamic Roughness

            Vector3 com = SetMass(1.0f);

            Body.MoveTo(position, Matrix.Identity);

            Skin.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            Body.EnableBody();

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

        protected override void LoadContent()
        {
            _landerModel = Game.Content.Load<Model>("box");
            base.LoadContent();
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
            LanderGame game = (LanderGame)this.Game;

            Matrix[] tranforms = new Matrix[_landerModel.Bones.Count];
            _landerModel.CopyAbsoluteBoneTransformsTo(tranforms);

            Matrix worldMatrix = getWorldMatrix();

            foreach (ModelMesh mesh in _landerModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = tranforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = Camera.View();
                    effect.Projection = Camera.CameraProjection;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }

        private Vector3 SetMass(float mass)
        {
            PrimitiveProperties primitiveProperties = new PrimitiveProperties(
                PrimitiveProperties.MassDistributionEnum.Solid,
                PrimitiveProperties.MassTypeEnum.Mass, mass);

            float junk;
            Vector3 com;
            Matrix it;
            Matrix itCom;

            Skin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCom);

            Body.BodyInertia = itCom;
            Body.Mass = junk;

            return com;

        }

        private Matrix getWorldMatrix()
        {
            return Matrix.CreateScale(Scale) * Skin.GetPrimitiveLocal(0).Transform.Orientation * Body.OldOrientation *
                   Matrix.CreateTranslation(Body.Position);
        }
    }
}
