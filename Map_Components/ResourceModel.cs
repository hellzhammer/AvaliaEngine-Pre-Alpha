using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine_lib.Map_Components
{
    public class ResourceModel : GameObject2D
    {
        public readonly ResourceType resource_type;
        public float CurrentAmount { get; protected set; }
        private float time_to_regen = 10;

        public ResourceModel(string ID, string obj_name, string textureName, Vector2 position, ResourceType resource_type) : base(ID, obj_name, textureName, position)
        {
            this.resource_type = resource_type;
            this.CurrentAmount = 1000;
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
                CurrentAmount = 1000;
                time_to_regen = 10;
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
