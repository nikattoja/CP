using System;
using System.Collections.Generic;
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



            public override void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(int value)
            {
                throw new NotImplementedException();
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }
    }
}