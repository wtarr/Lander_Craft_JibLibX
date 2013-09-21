﻿#region Using Statements

using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
using JigLibX.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Lander_Craft_JibLibX.PhysicsObjects
{
    public class HeightmapObject : PhysicObject
    {
        public HeightmapObject(Game game, Model model,Vector2 shift)
            : base(game, model)
        {
            Body = new Body(); // just a dummy. The PhysicObject uses its position to get the draw pos
            Collision = new CollisionSkin(null);

            HeightMapInfo heightMapInfo = model.Tag as HeightMapInfo;
			Array2D field = new Array2D(heightMapInfo.heights.GetLength(0), heightMapInfo.heights.GetLength(1));

			for (int x = 0; x < heightMapInfo.heights.GetLength(0); x++)
            {
				for (int z = 0; z < heightMapInfo.heights.GetLength(1); z++)
                {
                    field.SetAt(x,z,heightMapInfo.heights[x,z]);  
                }
            }

            // move the body. The body (because its not connected to the collision
            // skin) is just a dummy. But the base class shoudl know where to
            // draw the model.
            Body.MoveTo(new Vector3(shift.X,0,shift.Y), Matrix.Identity);

			Collision.AddPrimitive(new Heightmap(field, shift.X, shift.Y, heightMapInfo.terrainScale, heightMapInfo.terrainScale), new MaterialProperties(0.7f, 0.7f, 0.6f));

            PhysicsSystem.CurrentPhysicsSystem.CollisionSystem.AddCollisionSkin(Collision);
        }

        public override void ApplyEffects(BasicEffect effect)
        {
            effect.PreferPerPixelLighting = true;
        }

    }
}
