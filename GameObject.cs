namespace Engine_lib
{
    public abstract class GameObject
    {
        protected double next_update = 0;
        public float scale { get; set; }
        public bool is_mouse_over { get; protected set; }
        public string id { get; protected set; }
        public string object_name { get; set; }

        public float rotation { get; set; }

        public Action OnMouseOver { get; set; }
        public Action OnMouseExit { get; set; }
        public Action OnLeftClick { get; set; }
        public Action OnRightClick { get; set; }

        public bool In_Render_View { get; protected set; }

        public void SetMouseOver(bool change)
        {
            this.is_mouse_over = change;
        }
    }
}
