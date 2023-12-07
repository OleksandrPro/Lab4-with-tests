using SFML.Graphics;

namespace Lab4
{
    public abstract class GameObject
    {
        public bool IsActive {  get; protected set; }
        public FloatRect Collider { get; set; }
        public static FloatRect DefaultCollider { get; private set; }
        public GameObject() 
        {
            DefaultCollider = new FloatRect();
            IsActive = false;
        }
        public void SetActive(bool b)
        {
            IsActive = b;
        }
    }
}
