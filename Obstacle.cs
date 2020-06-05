using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    class Obstacle
    {
        // fields
        protected Vector2 position;
        protected int radius;
        public static List<Obstacle> obstacles = new List<Obstacle>();
        protected Vector2 hitPos; // hit box


        // properties
        public Vector2 Postition
        {
            get { return position; }
        }
        public int Radius
        {
            get { return radius; }
        }

        // Constructors
        public Obstacle(Vector2 newPos)
        {
            position = newPos;
        }

        //Methods 
        public void Update()
        {

        }

        // collision testing between obtacles and all other sprites
        // static so all other classes can use it
        public static bool didCollide(Vector2 otherPos, int otherRad) 
        {
            foreach (Obstacle ob in Obstacle.obstacles)
            {
                int sum = ob.Radius + otherRad;
                if (Vector2.Distance(otherPos, ob.hitPos) < sum)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Tree : Obstacle
    {
        // constructor
        public Tree(Vector2 newPos) : base(newPos)
        {
            radius = 40; // hit box
            hitPos = new Vector2(position.X + 64, position.Y + 175); // hit box - position.Y higher means hit box moves down to stump
        }
    }
    class Bush : Obstacle
    {
        // constructor
        public Bush(Vector2 newPos) : base(newPos)
        {
            radius = 32; // hit box
            hitPos = new Vector2(position.X + 56, position.Y + 57); // hit box
        }
    }
}
