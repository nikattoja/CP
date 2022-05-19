using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace TPW.Dane
{
    public abstract class DaneAPI : IObserver<int>, IObservable<int>
    {
        public static DaneAPI CreateDataBall()
        {
            return new DaneBall();
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);

        public abstract void CreateBalls(int howMany);
        public abstract List<Ball> GetBallsList();

        public abstract int GetBallsCount();

        public abstract void SetBallSpeed(int id, Vector2 velocity);
        public abstract IDisposable Subscribe(IObserver<int> observer);

        private class DaneBall : DaneAPI
        {
            private BallsList ballsList;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IList<IObserver<int>> observers;
            private Barrier barrier;
            public DaneBall()
            {
                this.ballsList = new BallsList();
                observers = new List<IObserver<int>>();

            }

            public override void SetBallSpeed(int id, Vector2 velocity)
            {
                ballsList.GetBall(id).Velocity = velocity;
            }

            public override List<Ball> GetBallsList()
            {
                return ballsList.GetBallsList();
            }
            public override int GetBallsCount()
            {
                return ballsList.GetBallsList().Count;
            }

            public override void CreateBalls(int howMany)
            {
                barrier = new Barrier(howMany);
                ballsList.AddBalls(howMany);
                foreach (var ball in ballsList.GetBallsList())
                {
                    Subscribe(ball);

                 
                }
            }

            public override void OnCompleted()
            {
                Unsubscribe();
            }

            public override void OnError(Exception error)
            {
                throw error;
            }

            public override void OnNext(int value)
            {
                barrier.SignalAndWait();

                foreach (var observer in observers)
                {
                    observer.OnNext(value);
                }
            }
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }
            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            public override IDisposable Subscribe(IObserver<int> observer)
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
        }
        }
    }

