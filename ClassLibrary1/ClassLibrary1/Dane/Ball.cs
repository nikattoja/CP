using System;
using System.Collections.Generic;
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
        internal readonly IList<IObserver<int>> observers;

        public Ball(int id)
        {
            this.id = id;
            this.Position = GetRandomPointInsideBoard();
            var rng = new Random();
            var x = (float)(rng.NextDouble() - 0.5) * 1;
            var y = (float)(rng.NextDouble() - 0.5) * 1;
            var result = new Vector2(x, y);
            observers = new List<IObserver<int>>();
        this.Velocity = result;
        }

        private Vector2 GetRandomPointInsideBoard()
        {
            var rng = new Random();
            var x = rng.Next(40, (int)(650 - 40));
            var y = rng.Next(40, (int)(400 - 40));

            return new Vector2(x, y);
        }
        public void Simulate()
        {
            while (true)
            {


                foreach (var observer in observers)
                {
                    if (observer != null)
                    {
                        observer.OnNext(id);
                    }
                }
                System.Threading.Thread.Sleep(1);
            }
        }
        private Vector2 GetNextPosition()
        {

            Vector2 newPosition = Position + Velocity;
            Vector2 newPosition2 = Velocity;

            if (newPosition.X < 0 || newPosition.X > 650 - 40)
            {
                newPosition2.X = -newPosition2.X;
            }

            if (newPosition.Y < 0 || newPosition.Y > 400 - 40)
            {
                newPosition2.Y = -newPosition2.Y;
            }
            Velocity = newPosition2;

            return Position + newPosition2;
        }

        public override string ToString()
        {
            return $"({Position.X}, {Position.Y})";
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
