using System;
using System.Collections.Generic;
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
    public static class Camera
    {
        public static Vector3 Position { get; set; }
        public static Vector3 Target { get; set; }
        public static Vector3 Up { get; set; }
        public static float AspectRatio;
        public static float NearPlane;
        public static float FarPlane;
        //public static Matrix CameraView;
        public static Matrix CameraProjection;

        public static Matrix View()
        {
           return Matrix.CreateLookAt(Position, Target, Up);
        }

        public static Matrix Projection()
        {
            return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), AspectRatio, NearPlane, FarPlane);
        }
    }
}
