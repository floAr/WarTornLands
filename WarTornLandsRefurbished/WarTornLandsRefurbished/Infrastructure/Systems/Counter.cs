using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WarTornLands.Counter
{
    public enum Increment { HOURS, MINUTES, SECONDS, MILLISECONDS }

    public class CounterManager
    {
        internal const int C_hour = 3600000;
        internal const int C_minute = 60000;
        internal const int C_seconds = 1000;

        public event EventHandler<BangEventArgs> Bang;
        public event EventHandler<BangEventArgs> Step;
        internal event EventHandler<CounterEventArgs> Start;
        internal event EventHandler<CounterEventArgs> Cancel;
        private List<Counter> _counters;
        private int _counterAutoname;

        public CounterManager()
        {
            if (_counters == null)
                _counters = new List<Counter>();

            AddCounter("counter_0");
        }

        public void AddCounter()
        {
            string id = "counter_";
            id += (++_counterAutoname).ToString();

            AddCounter(id);
        }

        public void AddCounter(string id)
        {
            AddCounter(id, Counter.DEFAULT);
        }

        public void AddCounter(string id, int defaultTerm, bool stepCalled = false)
        {
            CheckCounterUse(id);

            if (!stepCalled)
            {
                _counters.Add(new Counter(id, defaultTerm));
                _counters[_counters.Count - 1].Initialize(this);
                _counters[_counters.Count - 1].Bang += new EventHandler<BangEventArgs>(PassOnBang);
            }
            else 
            {
                _counters.Add(new Counter(id, defaultTerm, stepCalled));
                _counters[_counters.Count - 1].Initialize(this);
                _counters[_counters.Count - 1].Bang += new EventHandler<BangEventArgs>(PassOnBang);
                _counters[_counters.Count - 1].Step += new EventHandler<BangEventArgs>(PassOnStep);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Counter c in _counters)
                c.Update(gameTime);
        }

        private void CheckCounterAvailabillity(string id)
        {
            foreach (Counter c in _counters)
            {
                if (c.ID.Equals(id))
                    return;
            }

            throw new CounterIDNotRecognisedException(id);
        }
        private void CheckCounterUse(string id)
        {
            foreach (Counter c in _counters)
            {
                if (c.ID.Equals(id))
                    throw new CounterIDAlreadyInUseException(id);
            }
        }

        /// <summary>
        /// Unfinished method, do not use.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="term">The term.</param>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <param name="loop">if set to <c>true</c> [loop].</param>
        /// <returns></returns>
        public bool StartCounter(String id, int term = -1, bool refresh = true, bool loop = false)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(id, refresh));

            return true;
        }
        public void StartCounter(float term, Increment increment)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(null, new CounterEventArgs(term, "counter_0", true, increment));
        }
        public void StartCounter(String id)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(id, true));
        }
        public void StartCounter(String id, bool refresh)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(id, refresh));
        }
        public void StartCounter(int term)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(this, new CounterEventArgs(term, "counter_0", true));
        }
        public void StartCounter(float term, string id, Increment inc)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(term, id, true, inc));
        }
        public void StartCounter(int term, string id)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(term, id, true));
        }
        public void StartCounter(float term, bool refresh, Increment inc)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(this, new CounterEventArgs(term, "counter_0", refresh, inc));
        }
        public void StartCounter(int term, bool refresh)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(this, new CounterEventArgs(term, "counter_0", refresh));
        }
        public void StartCounter(float term, bool refresh, string id, Increment inc)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(term, id, refresh, inc));
        }
        public void StartCounter(int term, bool refresh, string id)
        {
            CheckCounterAvailabillity(id);

            if (Start != null)
                Start(this, new CounterEventArgs(term, id, refresh));
        }

        public float GetPercentage(string id)
        {
            CheckCounterAvailabillity(id);

            foreach(Counter c in _counters)
            {
                if (c.ID.Equals(id))
                    return c.GetPercentage();
            }

            throw new CounterIDNotRecognisedException(id);
        }
        public float GetPercentage()
        {
            CheckCounterAvailabillity("counter_0");

            return _counters[0].GetPercentage();

            throw new CounterIDNotRecognisedException("counter_0");
        }

        public void CancelCounter()
        {
            Cancel(null, new CounterEventArgs("counter:0"));
        }
        public void CancelCounter(string id)
        {
            Cancel(null, new CounterEventArgs(id));
        }

        private void PassOnBang(object sender, BangEventArgs e)
        {
            if (Bang != null)
                Bang(null, e);
        }
        private void PassOnStep(object sender, BangEventArgs e)
        {
            if (Step != null)
                Step(null, e);
        }
    }

    internal class Counter
    {
        internal const int DEFAULT = -1;

        private int  _default;                                                                          // Can be set to a default value the term gets resets to after bang
        private int  _term;                                                                             // The term after which the counter calls the bang
        private int  _elapsedTime;                                                                      // The ellapsed time since the counter got started
        private bool _active;                                                                           // Bool whether the counter is currently counting or not
        private string _id;
        private BangEventArgs _bangTag;
        private bool _loop;
        private bool _stepCalling;

        public event EventHandler<BangEventArgs> Bang;                                                  // Event that gets called when the counter bangs
        public event EventHandler<BangEventArgs> Step;                                                  // Event that fires every time the counter is updated while active (must be enabled)

        public Counter(string id, bool stepCalled = false)
            : this(id, DEFAULT, stepCalled)
        {
        }

        public Counter(string id, int defaultTerm)
            : this(id, defaultTerm, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter" /> class.
        /// </summary>
        /// <param name="defaultTerm">The default term.</param>
        /// <param name="id">The ID.</param>
        public Counter(string id, int defaultTerm, bool stepCalled)
        {
            _default        = defaultTerm;
            _term           = _default;
            _elapsedTime    = 0;
            _active         = false;
            _id             = id;
            _bangTag        = new BangEventArgs(_id);
            _stepCalling     = stepCalled;
        }

        public void Initialize(CounterManager _proprietor)
        {
            _proprietor.Start += new EventHandler<CounterEventArgs>(OnStartCounter);
            _proprietor.Cancel += new EventHandler<CounterEventArgs>(OnCancel);
        }

        public string ID
        {
            get { return _id; }
        }

        public float GetPercentage()
        {
            return (float)_elapsedTime / (float)_term;
        }

        public void Update(GameTime gameTime)
        {
            if (_active)
            {
                _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_elapsedTime >= _term
                    && Bang != null)
                {
                    _elapsedTime = 1;
                    Bang(this, _bangTag);
                    _term = _default;
                    _elapsedTime = 0;

                    if (!_loop)
                        _active = false;
                }

                if (_stepCalling)
                {
                    if (Step != null)
                        Step(null, new BangEventArgs(this.ID));
                }
            }
        }

        public void OnCancel(object sender, CounterEventArgs e)
        {
            if (e.ID.Equals(this._id) &&
               (!this._active || e.Refresh))
            {
                _elapsedTime = 0;
                _term = _default;
                _active = false;
            }
        }

        private void OnStartCounter(object sender, CounterEventArgs e)
        {
            if (e.ID.Equals(this._id) &&
                (!this._active || e.Refresh))
            {
                if (_default == -1)
                {
                    if (e.Term != -1)
                        this._term = e.Term;
                    else
                        throw new CounterCalledWithoutTermException(e.ID);
                }
                else if (e.Term != -1) throw new CounterHasDefaultException(e.ID);
                

                _elapsedTime = 0;
                _active = true;
            }
        }
    }

    class CounterHasDefaultException : Exception
    {
        public CounterHasDefaultException() : base("A counter that has been initialized with a default term cannot be started with a desired term.") { }

        public CounterHasDefaultException(string id) : base("The counter with ID " + id + " has been initialized with a default term and thus cannot be started with a desired term.") { }
    }
    class CounterIDNotRecognisedException : Exception
    {
        public CounterIDNotRecognisedException(string id) : base("There is no counter specified with ID " + id) {}
    }
    class CounterCalledWithoutTermException : Exception
    {
        public CounterCalledWithoutTermException(string id) : base("The counter " + id + " has no default and got colled without a term.") { }
    }
    class CounterIDAlreadyInUseException : Exception
    {
        public CounterIDAlreadyInUseException(string id) : base("The counter ID " + id + " is already in use.") { }
    }

    internal class CounterEventArgs : EventArgs
    {
        internal string ID;
        internal int    Term;
        internal bool   Refresh;
        internal bool   Loop;

        public CounterEventArgs(float term, string id, bool refresh, Increment inc)
        {
            ID = id;
            Refresh = refresh;

            switch (inc)
            {
                case Increment.HOURS:
                    Term = (int)(term * CounterManager.C_hour);
                    break;
                case Increment.MINUTES:
                    Term = (int)(term * CounterManager.C_minute);
                    break;
                case Increment.SECONDS:
                    Term = (int)(term * CounterManager.C_seconds);
                    break;
                case Increment.MILLISECONDS:
                    Term = (int)(term);
                    break;
                default:
                    Term = -1;
                    break;
            }
        }

        public CounterEventArgs(int term, string id, bool refresh)
        {
            ID = id;
            Term = term;
            Refresh = refresh;
        }

        public CounterEventArgs(string id, bool refresh)
        {
            ID = id;
            Term = -1;
            Refresh = refresh;
        }

        public CounterEventArgs(string id)
        {
            ID = id;
        }
    }

    public class BangEventArgs : EventArgs
    {
        internal string ID;

        public BangEventArgs(string id)
        {
            ID = id;
        }

        public bool IsDesiredCounter(string id)
        {
            return ID == id;
        }
    }
}
