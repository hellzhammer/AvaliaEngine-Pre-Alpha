using Engine_lib;
using Engine_lib.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public enum MouseButton
{
	Left, Right, Middle
}
public static class Input
{
    public static Vector2 MousePosition { get; private set; }
	public static Vector2 MouseWorldPosition { get; private set; }
    public static bool Allow_Keyboard = true;
	private static KeyboardState key_board_state { get; set; }
	private static MouseState mouse_state { get; set; }

	private static KeyboardState last_key_board_state { get; set; }
	private static MouseState last_mouse_state { get; set; }

	public static MouseState Get_Mouse_State()
	{
		return mouse_state;
	}

	public static KeyboardState Get_Keyboard_State()
	{
		return key_board_state;
	}

	public static void Update()
	{
        // update the mouse position
        var mstate = Input.Get_Mouse_State();
        MousePosition = new Vector2(mstate.X, mstate.Y);
        MouseWorldPosition = Camera2D.ScreenToWorldSpace(MousePosition, Camera2D.main_camera.GetViewMatrix());

        last_mouse_state = mouse_state;
		last_key_board_state = key_board_state;
		key_board_state = Keyboard.GetState();
		mouse_state = Mouse.GetState();
	}

	public static bool KeyUp(Keys key)
	{
		return last_key_board_state.IsKeyDown(key) && key_board_state.IsKeyUp(key);
	}

	public static bool LeftMouseUp(MouseButton mouse)
	{
		bool rtn = false;
		if (mouse == MouseButton.Left)
		{
			var last_left = last_mouse_state.LeftButton;
			var curr_left = mouse_state.LeftButton;
			if (last_left == ButtonState.Pressed && curr_left != ButtonState.Pressed)
			{
				rtn = true;
			}
		}

		return rtn;
	}

	public static bool RightMouseUp(MouseButton mouse)
	{
		bool rtn = false;
		if (mouse == MouseButton.Right)
		{
			var last_right = last_mouse_state.RightButton;
			var curr_right = mouse_state.RightButton;
			if (last_right == ButtonState.Pressed && curr_right != ButtonState.Pressed)
			{
				rtn = true;
			}
		}

		return rtn;
	}

	public static void _OnMouseOver(GameObject2D val)
	{
		if (MousePosition == Vector2.Zero)
			return;

		var Mouse_Rect = new Rectangle(MouseWorldPosition.ToPoint(), new Point(8, 8));

		var obj_rect = new Rectangle(
			new Vector2(
				val.Position.X, 
				val.Position.Y
				).ToPoint(), 
			new Point(
				(int)(TextureManager.Texture_Dictionary[val.texture_name].Width * val.scale), 
				(int)(TextureManager.Texture_Dictionary[val.texture_name].Height * val.scale)
				)
			);

		if (obj_rect.Intersects(Mouse_Rect) && !val.is_mouse_over)
		{
			val.SetMouseOver(true);
			if (val.OnMouseOver != null)
			{
				val.OnMouseOver.Invoke();
			}
		}
		else if (!obj_rect.Intersects(Mouse_Rect) && val.is_mouse_over)
		{
			val.SetMouseOver(false);
			if (val.OnMouseExit != null)
			{
				val.OnMouseExit.Invoke();
			}
		}

		if (obj_rect.Intersects(Mouse_Rect) && val.is_mouse_over && Input.LeftMouseDown(MouseButton.Left))
		{
			if (val.OnLeftClick != null)
			{
				val.OnLeftClick.Invoke();
			}
		}
		else if (obj_rect.Intersects(Mouse_Rect) && val.is_mouse_over && Input.RightMouseDown(MouseButton.Right))
		{
			if (val.OnRightClick != null)
			{
				val.OnRightClick.Invoke();
			}
		}
	}

	public static bool KeyDown(Keys key)
	{
		return !last_key_board_state.IsKeyDown(key) && key_board_state.IsKeyDown(key);
	}

	public static bool LeftMouseDown(MouseButton mouse)
	{
		bool rtn = false;
		if (mouse == MouseButton.Left)
		{
			var last_left = last_mouse_state.LeftButton;
			var curr_left = mouse_state.LeftButton;
			if (last_left != ButtonState.Pressed && curr_left == ButtonState.Pressed)
			{
				rtn = true;
			}
		}

		return rtn;
	}

	public static bool RightMouseDown(MouseButton mouse)
	{
		bool rtn = false;
		if (mouse == MouseButton.Right)
		{
			var last_right = last_mouse_state.RightButton;
			var curr_right = mouse_state.RightButton;
			if (last_right != ButtonState.Pressed && curr_right == ButtonState.Pressed)
			{
				rtn = true;
			}
		}

		return rtn;
	}

	public static bool KeyHold(Keys key)
	{
		return key_board_state.IsKeyDown(key) && last_key_board_state.IsKeyDown(key);
	}

	public static bool MouseHold(MouseButton mouse)
	{
		bool rtn = false;
		if (mouse == MouseButton.Left)
		{
			var last_left = last_mouse_state.LeftButton;
			var curr_left = mouse_state.LeftButton;
			if (last_left == ButtonState.Pressed && curr_left == ButtonState.Pressed)
			{
				rtn = true;
			}
		}
		else if (mouse == MouseButton.Right)
		{
			var last_right = last_mouse_state.RightButton;
			var curr_right = mouse_state.RightButton;
			if (last_right == ButtonState.Pressed && curr_right == ButtonState.Pressed)
			{
				rtn = true;
			}
		}

		return rtn;
	}
}
