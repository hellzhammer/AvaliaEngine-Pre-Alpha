namespace Engine_lib.Core
{
    public interface IEntityObject
    {
        public string id { get; set; }
        public string texture_name { get; set; }
        public string object_name { get; set; }
    }

    public class EntityObject : IEntityObject
    {
        public string id { get; set; }
        public string texture_name { get; set; }
        public string object_name { get; set; }

        protected float X;
        protected float Y; 

        public float rotation { get; set; }
        public float scale { get; set; }       

        public bool mouse_over { get; protected set; }
        public bool in_render_view { get; protected set; }
    }
}
