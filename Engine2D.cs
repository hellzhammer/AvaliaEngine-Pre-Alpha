using Engine_lib.Core;
using Engine_lib.UI_Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

namespace Engine_lib
{
	public class Engine2D : EngineCore
	{
		protected static SceneModel currentScene { get; set; }
        public static Engine2D current { get; private set; }

		public static SpriteFont Game_Font { get; set; }
		protected GUIManager GUI_MANAGER { get; set; }
		protected Camera2D camera { get; set; }
		protected SpriteBatch spriteBatch { get; set; }
		protected CalendarSystem calendar { get; set; }

		public void SetCalendar(CalendarSystem cs)
		{
			this.calendar = cs;
		}

        protected Engine2D()
		{
			this.GUI_MANAGER = new GUIManager(this);
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
		}

        /// <summary>
        /// update the current scene
        /// </summary>
        /// <param name="newscene"></param>
        public static void NewScene(SceneModel newscene)
        {
			if (OnSceneClose != null)
			{
				OnSceneClose.Invoke();
			}

            currentScene = null;
            currentScene = newscene;
        }

		/// <summary>
		/// what happens when the scene changes.
		/// </summary>
		public static Action OnSceneClose { get; set; }

        protected override void Initialize()
        {
			Game_Font = Content.Load<SpriteFont>("Game_Font");

			base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
			if (calendar != null)
			{
				calendar.Update(gameTime);
			}
            if (camera != null)
            {
                Camera2D.main_camera.Update();
            }
        }
    }
}
