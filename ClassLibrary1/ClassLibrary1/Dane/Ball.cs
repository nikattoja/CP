using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Dane
{

    public class Ball : IObservable<int>
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int id;
        private Task BallTask;
        private Stopwatch Timer = new Stopwatch();
        internal readonly IList<IObserver<int>> observers;
        public Logger logger;

        public Ball(int id)
        {
            this.id = id;
            this.Position = GetRandomPointInsideBoard();
            var rng = new Random();
            var x = (float)(rng.NextDouble() * 0.03) * 1;
            var y = (float)(rng.NextDouble() * 0.03) * 1;
            var result = new Vector2(x, y);
            Velocity = result;
            observers = new List<IObserver<int>>();

        }

        private Vector2 GetRandomPointInsideBoard()
        {
            var rng = new Random();
            var x = rng.Next(40, (int)(650 - 40));
            var y = rng.Next(40, (int)(400 - 40));

            return new Vector2(x, y);
        }
        public void StartMoving()
        {
            this.BallTask = new Task(Simulate);
            BallTask.Start();
        }
        public void Simulate()
        {
            while (true)
            {
     
               Timer.Restart();
               Timer.Start();
                Position = GetNextPosition(Timer.ElapsedMilliseconds);
               BallLog();

                foreach (var observer in observers.ToList())
                {

                    if (observer != null)
                    {
                        observer.OnNext(id);
                    }
                }
               Timer.Stop();

            }
        }

        public void BallLog()
        {
            logger.log(this);
        }
        private Vector2 GetNextPosition(long timeInMs)
        {
            Vector2 newPosition;

            if (timeInMs > 0)
            {
                newPosition = Position + Velocity * timeInMs;
            }
            else
            {
                newPosition = Position + Velocity ;
            }
            return newPosition;
        }


    #region provider

    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
        return new Unsubscriber(observers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private IList<IObserver<int>> _observers;
        private IObserver<int> _observer;

        public Unsubscriber
        (IList<IObserver<int>> observers, IObserver<int> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    #endregion
}
}
