using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Engine_lib.Core
{
    public class EntityManager : GameComponent
    {
        public Dictionary<string, GameObject2D> Entities { get; set; }

        public List<string> to_draw { get; set; }

        public EntityManager(Game game) : base(game)
        {
            to_draw = new List<string>();
            Entities = new Dictionary<string, GameObject2D>();
            game.Components.Add(this);
        }

        bool isrunning = false;
        public override void Update(GameTime gt) 
        {
            if (!isrunning)
            {
                isrunning = true;
                to_draw.Clear(); // reset the drawables list

                List<string> toRemove = new List<string>();

                foreach (var item in Entities)
                {
                    if (item.Value == null)
                    {
                        Debug.WriteLine($"Entity with ID: {item.Key} is null and will be removed from entity manager.");
                        toRemove.Add(item.Key);
                        continue;
                    }

                    item.Value.SetInRenderView(Camera2D.Is_In_Render_View_BoundsCheck(item.Value.Position));

                    if (item.Value.in_render_view)
                    {
                        Input._OnMouseOver(item.Value);
                        to_draw.Add(item.Key); // only draw if in render view
                    }
                }

                for (int i = 0; i < toRemove.Count; i++)
                {
                    Entities.Remove(toRemove[i]);
                }

                isrunning = false;
            }

            base.Update(gt);
        }

        public void AddEntity(GameObject2D new_entity)
        {
            if (Entities.ContainsKey(new_entity.id))
            {
                Debug.WriteLine($"Entity already exists! NAME: {new_entity.object_name} -- ID: {new_entity.id}");
                return;
            }

            if (Entities.Count > 0 && Entities.Values.Contains(new_entity))
            {
                Debug.WriteLine($"Entity already exists! NAME: {new_entity.object_name} -- ID: {new_entity.id}");
                return;
            }

            string nID = string.Empty;
            for (int i = 0; i < 1000000; i++)
            {
                nID = $"{new_entity.id}_{i}";
                if (!Entities.ContainsKey(nID))
                {
                    new_entity.id = nID;
                    break;
                }
            }

            this.Entities.Add(nID, new_entity);
            Debug.WriteLine($"Adding: {new_entity.object_name} with ID: {new_entity.id} to entity manager!");
        }

        public void RemoveEntity(string id)
        {
            if (Entities.ContainsKey(id))
            {
                Entities.Remove(id);
                Debug.WriteLine($"Removing entity with ID: {id} from entity manager!");
                return;
            }
            Debug.WriteLine($"No entity with id: {id} could be found to remove!");
        }

        public GameObject2D FindObjectByID(string id)
        {
            if (Entities.TryGetValue(id, out GameObject2D obj))
                return obj;

            Debug.WriteLine($"No entity with id: {id} could be found!");
            return null;
        }

        public GameObject2D FindObjectByName(string name)
        {
            foreach (var kvp in Entities)
            {
                if (kvp.Value.id == name)
                    return kvp.Value;
            }

            Debug.WriteLine($"No entity with name: {name} could be found!");
            return null;
        }

        public List<GameObject2D> FindObjectsByName(string name)
        {
            List<GameObject2D> results = new List<GameObject2D>();

            foreach (var kvp in Entities)
            {
                if (kvp.Value.id == name)
                    results.Add(kvp.Value);
            }

            if (results.Count == 0)
                Debug.WriteLine($"No entities with name: {name} could be found!");

            return results;
        }

        public T FindObjectByType<T>() where T : GameObject2D
        {
            foreach (var kvp in Entities)
            {
                if (kvp.Value is T obj)
                    return obj;
            }

            Debug.WriteLine($"No entity of type {typeof(T).Name} could be found!");
            return null;
        }

        public List<T> FindObjectsByType<T>() where T : GameObject2D
        {
            List<T> results = new List<T>();

            foreach (var kvp in Entities)
            {
                if (kvp.Value is T obj)
                    results.Add(obj);
            }

            if (results.Count == 0)
                Debug.WriteLine($"No entities of type {typeof(T).Name} could be found!");

            return results;
        }
    }
}