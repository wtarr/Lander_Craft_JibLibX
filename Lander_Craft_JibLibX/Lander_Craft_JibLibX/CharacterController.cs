using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JigLibX.Physics;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Utils;
using Microsoft.Xna.Framework;

namespace Lander_Craft_JibLibX
{
    public class CharacterController : Controller
    {
        public Body Body { get; set; }
        public bool boost = false,
            fore = false,
            aft = false,
            port = false,
            starboard = false;


        public CharacterController()
        {
            
        }

        public void Initialize(Body body)
        {
            EnableController();
            Body = body;
        }

        public override void UpdateController(float dt)
        {
            //Body.ClearForces();
            if (boost)
            {
                Body.AddBodyForce(new Vector3(0 , 50000, 0));
                boost = false;
            }

            if (port)
            {
                Body.AddBodyForce(new Vector3(0, 0, -5000));
                port = false;
            }

            if (starboard)
            {
                Body.AddBodyForce(new Vector3(0, 0, 5000));
                starboard = false;
            }

            if (fore)
            {
                Body.AddBodyForce(new Vector3(5000, 0, 0));
                fore = false;
            }

            if (aft)
            {
                Body.AddBodyForce(new Vector3(-5000, 0, 0));
                aft = false;
            }
                
        }
    }
}
