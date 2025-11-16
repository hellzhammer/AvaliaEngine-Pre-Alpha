using Engine_lib;
using Engine_lib.UI_Framework.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ViewManager : GameComponent
{
	public static ViewManager current { get; private set; }
	
	private List<View> view_stack { get; set; }
	public int StackCount => view_stack.Count;

    public ViewManager(Game game) : base(game)
	{
		view_stack = new List<View>();

		game.Components.Add(this);
		current = this;
	}
	
	public override void Update(GameTime gt)
	{
        if (view_stack != null && view_stack.Count > 0)
			view_stack[0].Update(gt);

        base.Update(gt);
	}

	public virtual void Draw(SpriteBatch batch)
	{
		var vMat = Camera2D.main_camera.GetViewMatrix();
		if(view_stack != null && view_stack.Count > 0)
			view_stack[0].Draw(batch);
    }

    /// <summary>
	/// adds a view to the top of the stack.
	/// </summary>
    public virtual void PushView(View view)
	{
		if (view_stack != null && view_stack.Count > 0)
		{
            if (!view_stack.Contains(view))
            {
                view_stack.Insert(0, view);
            }
            else
            {
                Engine2D.current.Log("An element with that ID already exists!");
            }
        }
		else
		{
			if (view_stack == null)
			{
				view_stack = new List<View>();
				view_stack.Add(view);
            }
			else
			{
				view_stack.Add(view);
            }
		}
    }

	public virtual void PopView()
	{
		if (view_stack != null && view_stack.Count > 0)
		{
            this.view_stack.RemoveAt(0);
        }
    }

	/// <summary>
	/// clears the current ui view
	/// </summary>
	public virtual void ClearStack()
	{
		view_stack = new List<View>();
    }
}