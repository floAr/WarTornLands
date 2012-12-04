using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw
{
    /// <summary>
    /// Class for a frame
    /// </summary>
     struct Frame
    {
        /// <summary>
        /// The frame source
        /// </summary>
       public Rectangle FrameSource;
       /// <summary>
       /// The duration_ms
       /// </summary>
       public float Duration_ms;
    }
    /// <summary>
    /// Class to hold a Animation, whcih is a set of Frames
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// The standard frame 
        /// duration
        /// </summary>
        public  const float StandardDuration = 250;
        private List<Frame> _frames;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        private float _counter;
        private int _currentFrame;

        private AnimatedDrawer _parent;
        public AnimatedDrawer Parent { get { return _parent; } set { _parent = value; } }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is looping.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is looping; otherwise, <c>false</c>.
        /// </value>
        public bool IsLooping { get;  set; }
        private bool _backwards;

        /// <summary>
        /// Gets a value indicating whether this instance is repeating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is repeating; otherwise, <c>false</c>.
        /// </value>
        public bool IsRepeating { get;  set; }
        private Animation _foolowUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        public Animation(string name)
        {
            _frames = new List<Frame>();
            this.Name = name;
            IsLooping = false;
            IsRepeating = true;

        }

        /// <summary>
        /// Initializes a new non repeating instance of the <see cref="Animation" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <param name="followUp">The follow up animation.</param>
        public Animation(string name,Animation followUp)
        {
            _frames = new List<Frame>();
            this.Name = name;
            _foolowUp = followUp;
            IsRepeating = false;

        }

        /// <summary>
        /// Adds a frame.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public void AddFrame(Rectangle frame)
        {
            _frames.Add(new Frame() { Duration_ms = StandardDuration, FrameSource = frame });
        }

        /// <summary>
        /// Adds a frame with a certain duration.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="duration">The duration.</param>
        public void AddFrame(Rectangle frame,float duration)
        {
            _frames.Add(new Frame() { Duration_ms = duration, FrameSource = frame });
        }

        /// <summary>
        /// Adds the frame at.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="position">The position.</param>
        public void AddFrameAt(Rectangle frame,int position)
        {
            _frames.Insert(position, new Frame() { Duration_ms = StandardDuration, FrameSource = frame });
        }

        /// <summary>
        /// Adds the frame with a certain duration at.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="position">The position.</param>
        public void AddFrameAt(Rectangle frame, float duration,int position)
        {
            _frames.Insert(position,new Frame() { Duration_ms = duration, FrameSource = frame });
        }

        public Rectangle CurrentFrame { get { return _frames[_currentFrame].FrameSource; } }

        public void AddOffset(float offsetMS)
        {
            _counter += offsetMS;
        }
        /// <summary>
        /// Updates the animation with the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            //Add time to counter
            _counter += gameTime.ElapsedGameTime.Milliseconds;
            //if counter extends frame duration
            if (_counter >= _frames[_currentFrame].Duration_ms)
            {
                //subtract frames duraction
                _counter -= _frames[_currentFrame].Duration_ms;
                //if we are not playing backwards..
                if (!_backwards)
                    //step one frame foreward..
                    _currentFrame++;
                else
                    //or backwards
                    _currentFrame--;
                //if we are past the last frame..
                if (_currentFrame >= _frames.Count)
                {
                    //.. and if we are not repeating the animation
                    if (!IsRepeating)
                        //..start plaing the follow up
                        if (_foolowUp != null)
                            _parent.SetCurrentAnimation(_foolowUp.Name);
                        else
                            this.HasEnded = true;
                    //if we are looping..
                    if (IsLooping)
                    {
                        //..toggle direction...
                        _backwards = !_backwards;
                        //..and set frame to the one before the last
                        _currentFrame = _frames.Count - 2;
                    }
                    else
                        //if not set the frame to a new frame
                        _currentFrame = 0;
                }                
            }
        }



        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _counter = 0;
            _currentFrame = 0;
        }

        public bool HasEnded { get; set; }
    }
}
