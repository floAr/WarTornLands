using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.Systems.DrawSystem;

namespace WarTornLands.Infrastructure.Systems.SkyLight
{
    public class Lightmanager:IDrawProvider
    {

        private static Lightmanager _instance = new Lightmanager();
        public static Lightmanager Instance { get { return _instance; } }

        private static List<IDrawExecuter> _groundLights = new List<IDrawExecuter>();
        private static List<IDrawExecuter> _upperLights = new List<IDrawExecuter>(); 
        
        //Daylight
        private static Color _skyColor = Color.White;
        private static bool _dynamicLightCycle;
        private static List<Color> _dayGradient;
        private static int _dayLength;
        private static bool _pingPong;
        private static float _counter;
        private static float _fractal;
        private static int _currentColor;
        //endDaylight

        public void SetStaticColor(Color c)
        {
            _skyColor = c;
            _dynamicLightCycle = false;
        }

        public void SetDayCycle(List<Color> gradient, int durationMS,bool pingPong=false)
        {
            _dayLength = durationMS;
            _dayGradient = gradient;
            _pingPong=pingPong;
            _fractal =(_dayLength / _dayGradient.Count);
            _counter = 0;
            _currentColor = 0;
            _skyColor = _dayGradient[_currentColor];
            _dynamicLightCycle = true;
        }

        public void AddLight(IDrawExecuter light, bool isUpperLight = false)
        {
            if (isUpperLight)
                _upperLights.Add(light);
            else
                _groundLights.Add(light);

        }

        public  void Draw(GameTime gameTime)
        {
            if (_dynamicLightCycle)
            {
                _counter += gameTime.ElapsedGameTime.Milliseconds;
                if (_counter >= _fractal)
                {
                    if (!_pingPong)
                    {
                        _currentColor++;
                        if (_currentColor >= _dayGradient.Count - 1)
                            _currentColor = 0;
                    }
                    else
                        throw new NotImplementedException();
                    _counter = 0;
                }
                float a = _counter / _fractal;
                _skyColor = Color.Lerp(_dayGradient[_currentColor], _dayGradient[_currentColor + 1], a);
            }

            Game1.Instance.SpriteBatch.Draw(Game1.Instance.Content.Load<Texture2D>("sprite/light"), new Rectangle(-20, -20, Game1.Instance.GraphicsDevice.DisplayMode.Width + 40, Game1.Instance.GraphicsDevice.DisplayMode.Height + 20), _skyColor);
            foreach (Entities.Entity e in _groundLights)
            {
                e.Draw(gameTime);
            }
            foreach (Entities.Entity e in _upperLights)
            {
                e.Draw(gameTime);
            }
        }

       
    }
}
