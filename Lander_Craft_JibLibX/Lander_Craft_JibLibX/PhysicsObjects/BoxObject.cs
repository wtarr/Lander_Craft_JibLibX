#region Using Statements

using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Lander_Craft_JibLibX.PhysicsObjects
{

    public class BoxObject : PhysicObject
    {
        private Game game;


        public BoxObject(Game game,Model model,Vector3 sideLengths, Matrix orientation, Vector3 position) : base(game,model)
        {
            this.game = game;
            Body = new Body();
            Collision = new CollisionSkin(Body);

            Collision.AddPrimitive(new Box(- 0.5f * sideLengths, orientation, sideLengths), new MaterialProperties(0.8f, 0.8f, 0.7f));
            Body.CollisionSkin = this.Collision;
            Vector3 com = SetMass(1.0f);
            Body.MoveTo(position, Matrix.Identity);
            Collision.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            Body.EnableBody();
            this.scale = sideLengths;
        }
        
        public override void ApplyEffects(BasicEffect effect)
        {
            effect.DiffuseColor = color;
        }
    }
}


