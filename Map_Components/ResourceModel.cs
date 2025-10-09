/*using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine_lib.Map_Components
{
    public class ResourceModel : GameObject2D
    {
        public readonly ResourceType resource_type;
        public float CurrentAmount { get; protected set; }
        private float time_to_regen = 100;

        public ResourceModel(string ID, string obj_name, string textureName, Vector2 position, ResourceType resource_type) : base(ID, obj_name, textureName, position)
        {
            this.resource_type = resource_type;
            if (resource_type == ResourceType.Stone)
            {
                this.CurrentAmount = 25;
            }
            else if (resource_type == ResourceType.Wood)
            {
                this.CurrentAmount = 100;
            }
            else if (resource_type == ResourceType.Food)
            {
                this.CurrentAmount = 5;
            }
            this.Collision_Rect = new Rectangle(this.Position.ToPoint(), new Point(32, 32));
        }

        public override void Update(GameTime gt)
        {
            if (CurrentAmount > 0)
            {
                base.Update(gt);
            }
            else if (CurrentAmount <= 0 && time_to_regen > 0)
            {
                time_to_regen -= (float)gt.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (resource_type == ResourceType.Stone)
                {
                    this.CurrentAmount = 25;
                }
                else if (resource_type == ResourceType.Wood)
                {
                    this.CurrentAmount = 100;
                }
                else if (resource_type == ResourceType.Food)
                {
                    this.CurrentAmount = 5;
                }
                time_to_regen = 100;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (CurrentAmount > 0)
            {
                base.Draw(batch);
            }
        }

        public void ConsumeResource(float dmg)
        {
            if (dmg < 0)
            {
                dmg = 0;
            }

            this.CurrentAmount -= dmg;
            if (CurrentAmount < 0)
            {
                CurrentAmount = 0;
            }
            if (this.CurrentAmount > 1000)
            {
                this.CurrentAmount = 1000;
            }
        }

        public enum ResourceType
        {
            Wood,
            Stone,
            Food
        }
    }
}
*/