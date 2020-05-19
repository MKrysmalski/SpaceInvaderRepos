using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceInvasion
{
    class GameObject 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double V { get; set; }
        public GameObject(double x, double y, double v) : this(x, y)
        {
            V = v;
        }
        public GameObject(double x, double y)
        {
            X = x;
            Y = y;
        }
        public virtual void Draw()
        {
            //empty
        }
        public virtual void Draw(Player p)
        {
            //empty
        }
        
    }
}
