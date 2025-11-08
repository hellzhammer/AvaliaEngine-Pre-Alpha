using Engine_lib;
using Engine_lib.UI_Framework.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class ViewManager : GameComponent
{
	public static ViewManager current { get; private set; }
	
	private View current_view { get; set; }

	public ViewManager(Game game) : base(game)
	{
		game.Components.Add(this);
		current = this;
	}
	
	public override void Update(GameTime gt)
	{
        if (current_view != null)
            current_view.Update(gt);

        base.Update(gt);
	}

	public void Draw(SpriteBatch batch)
	{
		var vMat = Camera2D.main_camera.GetViewMatrix();
		if(current_view != null)
            current_view.Draw(batch, vMat);
    }

    /// <summary>
	/// adds a view to the top of the stack.
	/// </summary>
    public void NewView(View view)
	{
		if (current_view != view)
		{
            current_view = view;
        }
		else
		{
			Debug.WriteLine("An element with that ID already exists!");
		}
	}

	/// <summary>
	/// clears the current ui view
	/// </summary>
	public void ClearStack()
	{
		current_view = null;
    }
}