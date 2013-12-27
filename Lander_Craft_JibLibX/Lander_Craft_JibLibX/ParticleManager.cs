﻿/* This code is taken from
 * "XNA 4 3D Game Developement by Example"
 * by Kurt Jaegers 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lander_Craft_JibLibX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lander_Craft_JibLibX
{

    public static class ParticleManager
    {
        #region Fields
        private static GraphicsDevice graphicsDevice;
        private static List<Particle> particles = new List<Particle>();
        private static Effect particleEffect;
        private static Texture2D particleTexture;
        private static Random rand = new Random();
        #endregion

        #region MyRegion

        public static void Initialize(
            GraphicsDevice device,
            Effect effect,
            Texture2D texture)
        {
            graphicsDevice = device;
            particleEffect = effect;
            particleTexture = texture;
            Particle.GraphicsDevice = device;
            for (int x = 0; x < 300; x++)
            {
                particles.Add(new Particle());
            }
        }
        #endregion

        #region Particle Creation
        public static void AddParticle(
            Vector3 position,
            Vector3 velocity,
            float duration,
            float scale)
        {
            for (int x = 0; x < particles.Count; x++)
            {
                if (!particles[x].IsActive)
                {
                    particles[x].Activate(position, velocity, duration, scale);
                    return;
                }
            }
        }
        #endregion

        #region Update
        public static void Update(GameTime gameTime)
        {
            foreach (var particle in particles)
            {
                if (particle.IsActive)
                    particle.Update(gameTime);
            }
        }
        #endregion

        #region Draw

        public static void Draw()
        {
            graphicsDevice.BlendState = BlendState.Additive;

            particleEffect.CurrentTechnique =
                particleEffect.Techniques["ParticleTechnique"];

            particleEffect.Parameters["particleTexture"].SetValue(
                particleTexture);

            particleEffect.Parameters["View"].SetValue(Camera.View());

            particleEffect.Parameters["Projection"].SetValue(Camera.Projection());

            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (Particle particle in particles)
            {
                if (particle.IsActive)
                    particle.Draw(particleEffect);
            }
            graphicsDevice.RasterizerState =
                RasterizerState.CullCounterClockwise;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
        #endregion

        #region Helper Methods
        public static void MakeExplosion(Vector3 position, int particleCount)
        {
            for (int i = 0; i < particleCount; i++)
            {
                float duration = (float)(rand.Next(0, 20)) / 10f + 2;
                float x = ((float)rand.NextDouble() - 0.5f) * 1.5f;
                float y = ((float)rand.Next(1, 100)) / 10f;
                float z = ((float)rand.NextDouble() - 0.5f) * 1.5f;
                float s = (float)rand.NextDouble() + 3.0f;

                Vector3 direction = Vector3.Normalize(
                    new Vector3(x, y, z)) *
                                    (((float)rand.NextDouble() * 3f) + 6f);

                AddParticle(
                    new Vector3(position.X, position.Y-5, position.Z),
                    direction,
                    duration, s);
            }
        }
        #endregion

    }
}
