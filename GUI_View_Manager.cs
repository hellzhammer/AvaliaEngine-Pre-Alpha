using Engine_lib;
using Engine_lib.UI_Framework.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class GUI_View_Manager : GameComponent
{
	public static GUI_View_Manager current { get; private set; }
	
	public Dictionary<string, View> GUI { get; private set; }

	public GUI_View_Manager(Game game) : base(game)
	{
		GUI = new Dictionary<string, View>();
		Game.Components.Add(this);
		current = this;
	}
	
	float timer = 0.12f;
	public override void Update(GameTime gt)
	{
		if (timer <= 0)
		{
            foreach (var widget in GUI)
            {
                widget.Value.Update(gt);
            }

			timer = 0.12f;
			Debug.WriteLine("Updating!!!!");
        }
		else
		{
			timer -= (float)gt.ElapsedGameTime.TotalSeconds;
        }
	}

	public void Draw(SpriteBatch batch)
	{
		var vMat = Camera2D.main_camera.GetViewMatrix();
		foreach (var widget in GUI)
		{
			if (widget.Value.Is_Active)
			{
				widget.Value.Draw(batch, vMat);
			}
		}
	}

    public void Remove_GUI(string id)
	{
		if (GUI.ContainsKey(id))
		{
			GUI.Remove(id);
		}
		else
		{
			Debug.WriteLine("No element was foudn with that ID!");
		}
	}

	public void Add_GUI(string id, View view)
	{
		if (!GUI.ContainsKey(id))
		{
            GUI.Add(id, view);
        }
		else
		{
			Debug.WriteLine("An element with that ID already exists!");
		}
	}
}