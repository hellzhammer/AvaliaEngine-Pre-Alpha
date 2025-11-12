using Engine_lib.Core;
using Engine_lib.UI_Framework;
using Engine_lib.UI_Framework.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Engine_lib
{
	/// <summary>
	/// this component adds on to the core components and adds the 
	/// scenemanager, global instance, and more. also handles keyboard input 
	/// for text box and input use cases.
	/// </summary>
	public class Engine2D : EngineCore
	{
        public static SpriteFont Game_Font { get; set; }
        public static Engine2D current { get; private set; }

        public EntityManager entityManager { get; protected set; } // game component
        public TextureManager textureManager { get; protected set; } // reg class        
		protected ViewManager GUI_MANAGER { get; set; } // game component

		protected Camera2D main_camera { get; set; }	// reg class

        protected Engine2D()
		{
			random = new Random(); // to remove -- not needed internally. Not for now anyways. 
			// this is required to get keyboard info for text gui elements.
			this.Window.TextInput += (s, a) => {
				if (TextWidget.active_text_input != null)
				{
					Regex r = new Regex("^[a-zA-Z0-9]*$");
					if (r.IsMatch(a.Character.ToString()) && !char.IsPunctuation(a.Character))
					{
						if (!TextWidget.active_text_input.numeric_input)
                            TextWidget.active_text_input.Content += a.Character;

						else
							if (TextWidget.active_text_input.numeric_input && char.IsNumber(a.Character))
                            TextWidget.active_text_input.Content += a.Character;
					}
					else if (!r.IsMatch(a.Character.ToString()) && !char.IsPunctuation(a.Character))
					{
						if (a.Key == Keys.Back)
						{
							var _s = TextWidget.active_text_input.Content;
							string n_string = string.Empty;
							for (int i = 0; i < _s.Length - 1; i++)
								n_string += _s[i];

                            TextWidget.active_text_input.Content = n_string;
						}
						else if (a.Key == Keys.Space)
                            TextWidget.active_text_input.Content += " ";

						else if (a.Key == Keys.Enter)
                            TextWidget.active_text_input = null;
					}
					else if (!r.IsMatch(a.Character.ToString()) && char.IsPunctuation(a.Character))
                        TextWidget.active_text_input.Content += a.Character;
				}
			};

			graphics = new GraphicsDeviceManager(this);
			current = this;

            entityManager = new EntityManager(this);
            this.GUI_MANAGER = new ViewManager(this);
        }

        protected override void Initialize()
        {
            Game_Font = Content.Load<SpriteFont>("Game_Font");

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera2D.main_camera.GetViewMatrix());

            // draw the actual scene first, then follow with the gui
            if (SceneManager.currentScene != null)
            {
                SceneManager.Draw(spriteBatch);
            }

			// draw gui on top of scene view
            if (GUI_MANAGER != null)
			{
                GUI_MANAGER.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (main_camera != null)
            {
                Camera2D.main_camera.Update();
            }

			if (SceneManager.currentScene != null)
			{
				SceneManager.Update(gameTime);
			}

			base.Update(gameTime);
        }

		public virtual void QuitGame()
		{
			this.Exit();
		}

		public static void PopView()
		{
			ViewManager.current.PopView();
        }

		public static void PushView(View view)
		{
			ViewManager.current.PushView(view);
        }

        public static void PopAndPushNewView(View new_view)
        {
			if (ViewManager.current.StackCount > 0)
			{
                ViewManager.current.PopView();
            }
            ViewManager.current.PushView(new_view);
        }

        public virtual void Log(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}