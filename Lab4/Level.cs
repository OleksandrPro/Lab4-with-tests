using System.Collections.Generic;

namespace Lab4
{
    public abstract class Level
    {
        public virtual Player player { get; set; }
        public virtual List<Platform> platforms { get; set; }
        public virtual List<SFML.Graphics.FloatRect> barrier { get; set; }
        public void AddPlatform(int x, int y, int height, int width)
        {
            platforms.Add(new Platform(x, y, height, width));
        }
    }
}
