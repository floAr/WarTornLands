using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WarTornLands.Counter
{
    public enum INCREMENT { HOURS, MINUTES, SECONDS, MILLISECONDS }

    public class CounterManager
    {
        public event EventHandler<BangEventArgs> Bang;
        internal event EventHandler<CounterEventArgs> Start;
        internal event EventHandler<CounterEventArgs> Cancel;
        private List<Counter> COUNTER;
        private int mCounterAutoname;

        public CounterManager()
        {
            if (COUNTER == null)
                COUNTER = new List<Counter>();

            AddCounter("counter_0");
        }

        public void AddCounter()
        {
            string T_id = "counter_";
            T_id += (++mCounterAutoname).ToString();
            COUNTER.Add(new Counter(T_id));
            COUNTER[COUNTER.Count - 1].Initialize(this);
            COUNTER[COUNTER.Count - 1].Bang += new EventHandler<BangEventArgs>(PassOnBang);
        }

        public void AddCounter(string _id)
        {
            COUNTER.Add(new Counter(_id));
            COUNTER[COUNTER.Count - 1].Initialize(this);
            COUNTER[COUNTER.Count - 1].Bang += new EventHandler<BangEventArgs>(PassOnBang);
        }

        public void AddCounter(string _id, int _default)
        {
            COUNTER.Add(new Counter(_default, _id));
            COUNTER[COUNTER.Count - 1].Initialize(this);
            COUNTER[COUNTER.Count - 1].Bang += new EventHandler<BangEventArgs>(PassOnBang);
        }

        public void Update(GameTime _GT)
        {
            foreach (Counter c in COUNTER)
                c.Update(_GT);
        }

        private void CheckCounterAvailabillity(string _id)
        {
            foreach (Counter c in COUNTER)
            {
                if (c.GetID().Equals(_id))
                    return;
            }

            throw new CounterIDNotRecognisedException(_id);
        }

        public void StartCounter(float _amount, INCREMENT _inc)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(null, new CounterEventArgs(_amount, "counter_0", true, _inc));
        }
        public void StartCounter(String _id)
        {
            CheckCounterAvailabillity(_id);

            if (Start != null)
                Start(this, new CounterEventArgs(_id, true));
        }
        public void StartCounter(String _id, bool _refresh)
        {
            CheckCounterAvailabillity(_id);

            if (Start != null)
                Start(this, new CounterEventArgs(_id, _refresh));
        }
        public void StartCounter(int _amount)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, "counter_0", true));
        }
        public void StartCounter(float _amount, string _id, INCREMENT _inc)
        {
            CheckCounterAvailabillity(_id);

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, _id, true, _inc));
        }
        public void StartCounter(int _amount, string _id)
        {
            CheckCounterAvailabillity(_id);

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, _id, true));
        }
        public void StartCounter(float _amount, bool _refresh, INCREMENT _inc)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, "counter_0", _refresh, _inc));
        }
        public void StartCounter(int _amount, bool _refresh)
        {
            CheckCounterAvailabillity("counter_0");

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, "counter_0", _refresh));
        }
        public void StartCounter(float _amount, bool _refresh, string _id, INCREMENT _inc)
        {
            CheckCounterAvailabillity(_id);

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, _id, _refresh, _inc));
        }
        public void StartCounter(int _amount, bool _refresh, string _id)
        {
            CheckCounterAvailabillity(_id);

            if (Start != null)
                Start(this, new CounterEventArgs(_amount, _id, _refresh));
        }

        public float GetPercentage(string _id)
        {
            CheckCounterAvailabillity(_id);

            foreach(Counter c in COUNTER)
            {
                if (c.GetID().Equals(_id))
                    return c.GetPercentage();
            }

            throw new CounterIDNotRecognisedException(_id);
        }
        public float GetPercentage()
        {
            CheckCounterAvailabillity("counter_0");

            return COUNTER[0].GetPercentage();

            throw new CounterIDNotRecognisedException("counter_0");
        }

        public void CancelCounter()
        {
            Cancel(null, new CounterEventArgs("counter:0"));
        }
        public void CancelCounter(string _id)
        {
            Cancel(null, new CounterEventArgs(_id));
        }

        private void PassOnBang(object _sender, BangEventArgs _e)
        {
            if (Bang != null)
                Bang(null, _e);
        }
    }

    class Counter
    {
        private int  mDefault;                                                                          // Can be set to a default value the term gets resets to after bang
        private int  mTerm;                                                                             // The term after which the counter calls the bang
        private int  mElapsedTime;                                                                      // The ellapsed time since the counter got started
        private bool mActive;                                                                           // Bool whether the counter is currently counting or not
        private string mID;
        private BangEventArgs mBangTag;

        public event EventHandler<BangEventArgs> Bang;                                                  // Event that gets called when the counter bangs


        // Summary:
        //     Initializes a counter without a default term and a specified ID
        //     (term is to be set every time a countdown is requested)
        //
        public Counter(string _id)
        {
            mDefault        = -1;
            mTerm           = mDefault;
            mElapsedTime    = 0;
            mActive         = false;
            mID             = _id;
            mBangTag        = new BangEventArgs(mID);
        }
        // Summary:
        //     Initializes a counter with a default term
        //
        // Parameters:
        //   _default:
        //     Default value that the counter gets resets to after bang
        //
        public Counter(int _default, string _id)
        {
            mDefault        = _default;
            mTerm           = mDefault;
            mElapsedTime    = 0;
            mActive         = false;
            mID             = _id;
            mBangTag        = new BangEventArgs(mID);
        }


        public void Initialize(CounterManager _proprietor)
        {
            _proprietor.Start += new EventHandler<CounterEventArgs>(OnStartCounter);
            _proprietor.Cancel += new EventHandler<CounterEventArgs>(OnCancel);
        }

        public string GetID()
        {
            return mID;
        }

        public float GetPercentage()
        {
            return (float)mElapsedTime / (float)mTerm;
        }

        public void Update(GameTime _GT)
        {
            if (mActive)
            {
                mElapsedTime += _GT.ElapsedGameTime.Milliseconds;

                if (mElapsedTime >= mTerm
                    && Bang != null)
                {
                    Bang(this, mBangTag);
                    mActive = false;
                    mElapsedTime = 0;
                    mTerm = mDefault;
                }
            }
        }

        public void OnCancel(object _sender, CounterEventArgs _e)
        {
            if (_e.mID.Equals(this.mID) &&
               (!this.mActive || _e.mRefresh))
            {
                mElapsedTime = 0;
                mTerm = mDefault;
                mActive = false;
            }
        }

        private void OnStartCounter(object _sender, CounterEventArgs _e)
        {
            if (_e.mID.Equals(this.mID) &&
                (!this.mActive || _e.mRefresh))
            {
                if (mDefault == -1)
                {
                    if (_e.mTerm != -1)
                        this.mTerm = _e.mTerm;
                    else
                        throw new CounterCalledWithoutTermException(_e.mID);
                }
                else if (_e.mTerm != -1) throw new CounterHasDefaultException(_e.mID);
                

                mElapsedTime = 0;
                mActive = true;
            }
        }
    }

    class CounterHasDefaultException : Exception
    {
        public CounterHasDefaultException() : base("A counter that has been initialized with a default term cannot be started with a desired term.") { }

        public CounterHasDefaultException(string _id) : base("The counter with ID " + _id + " has been initialized with a default term and thus cannot be started with a desired term.") { }
    }
    class CounterIDNotRecognisedException : Exception
    {
        public CounterIDNotRecognisedException(string _id) : base("There is no counter specified with ID " + _id) {}
    }
    class CounterCalledWithoutTermException : Exception
    {
        public CounterCalledWithoutTermException(string _id) : base("The counter " + _id + " has no default and got colled without a term.") { }
    }

    class CounterEventArgs : EventArgs
    {
        private const int C_hour = 3600000;
        private const int C_minute = 60000;
        private const int C_seconds = 1000;

        internal string mID;
        internal int    mTerm;
        internal bool   mRefresh;

        public CounterEventArgs(float _amount, string _id, bool _refresh, INCREMENT _inc)
        {
            mID = _id;
            mRefresh = _refresh;

            switch (_inc)
            {
                case INCREMENT.HOURS:
                    mTerm = (int)(_amount * C_hour);
                    break;
                case INCREMENT.MINUTES:
                    mTerm = (int)(_amount * C_minute);
                    break;
                case INCREMENT.SECONDS:
                    mTerm = (int)(_amount * C_seconds);
                    break;
                case INCREMENT.MILLISECONDS:
                    mTerm = (int)(_amount);
                    break;
                default:
                    mTerm = -1;
                    break;
            }
        }

        public CounterEventArgs(int _amount, string _id, bool _refresh)
        {
            mID = _id;
            mTerm = _amount;
            mRefresh = _refresh;
        }

        public CounterEventArgs(string _id, bool _refresh)
        {
            mID = _id;
            mTerm = -1;
            mRefresh = _refresh;
        }

        public CounterEventArgs(string _id)
        {
            mID = _id;
        }
    }

    public class BangEventArgs : EventArgs
    {
        internal string ID;

        public BangEventArgs(string _id)
        {
            ID = _id;
        }
    }
}
