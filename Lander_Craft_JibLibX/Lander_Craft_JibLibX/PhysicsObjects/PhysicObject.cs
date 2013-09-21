using System;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lander_Craft_JibLibX.PhysicsObjects
{

    /// <summary>
    /// Helps to combine the physics with the graphics.
    /// </summary>
    public abstract class PhysicObject : GameObject
    {

        public Body Body;
        protected CollisionSkin Collision;

        //protected Model model;
        protected Vector3 color;

        protected Vector3 scale = Vector3.One;

        public Body PhysicsBody{get { return Body; }}
        public CollisionSkin PhysicsSkin{ get { return Collision; }}

        protected static Random random = new Random();

        public PhysicObject(Game game,Model model) : base(game, model)
        {
           
            color = new Vector3(random.Next(255), random.Next(255), random.Next(255));
            color /= 255.0f;
        }

        public PhysicObject(Game game)
            : base(game, null)
        {
            Model = null;
            color = new Vector3(random.Next(255), random.Next(255), random.Next(255));
            color /= 255.0f;
        }

        protected Vector3 SetMass(float mass)
        {
            PrimitiveProperties primitiveProperties =
                new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Density, mass);

            float junk; Vector3 com; Matrix it, itCoM;

            Collision.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
            Body.BodyInertia = itCoM;
            Body.Mass = junk;

            return com;
        }
        Matrix[] boneTransforms = null;
        int boneCount = 0;

        public abstract void ApplyEffects(BasicEffect effect);

        public override void Update(GameTime gameTime)
        {
            Position = Body.Position;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Model != null)
            {
                //Console.WriteLine("Phy DRAW");

                if (boneTransforms == null || boneCount != Model.Bones.Count)
                {
                    boneTransforms = new Matrix[Model.Bones.Count];
                    boneCount = Model.Bones.Count;
                }

                Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
                
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {

                        // the body has an orientation but also the primitives in the collision skin
                        // owned by the body can be rotated!
                        if(Body.CollisionSkin != null)
                            effect.World = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Body.CollisionSkin.GetPrimitiveLocal(0).Transform.Orientation * Body.Orientation *  Matrix.CreateTranslation(Body.Position);
                        else
                            effect.World = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Body.Orientation  * Matrix.CreateTranslation(Body.Position);
                        
                        effect.View = Camera.View();

                        effect.Projection = Camera.Projection();

                        ApplyEffects(effect);

                        //if (!this.PhysicsBody.IsActive)
                        //    effect.Alpha = 0.4f;
                        //else
                        //    effect.Alpha = 1.0f;


                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                    }

                    mesh.Draw();
                }

            }

            //if (this.Game.DebugDrawer.Enabled)
            //{

            //    wf = collision.GetLocalSkinWireframe();

            //    // if the collision skin was also added to the body
            //    // we have to transform the skin wireframe to the body space
            //    if (body.CollisionSkin != null)
            //    {
            //        body.TransformWireframe(wf);
            //    }

            //    ((JiggleGame)this.Game).DebugDrawer.DrawShape(wf);
            //}


           // base.Draw(gameTime);
        }

        VertexPositionColor[] wf;

    }
}
