using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPW.Dane;

namespace TPW.Logika
{

   
    public abstract class LogicApi : IObserver<int>, IObservable<int>
    {

        public class OnPositionChangeEventArgs : EventArgs
        {
            public int ballId { get; set; }
        }
        public abstract IDisposable Subscribe(IObserver<int> observer);

	public event EventHandler<OnPositionChangeEventArgs> PositionChange;
	public abstract void AddBalls(int howMany);


	public static LogicApi CreateBallsLogic(Vector2 boardSize, DaneAPI data = default(DaneAPI))
	{
		return new BallsLogic( boardSize, data == null ? DaneAPI.CreateDataBall() : data);
	}

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);
        public abstract Vector2 getBallPosition(int index);

        internal class BallsLogic : LogicApi, IObservable<int>
        {
            private readonly DaneAPI daneAPI;
            public static readonly int BallRadius = 40;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IObservable<EventPattern<OnPositionChangeEventArgs>> eventObservable = null;
            public event EventHandler<OnPositionChangeEventArgs> BallChanged;



            public BallsLogic(Vector2 boardSize,DaneAPI daneAPI)
            {
                eventObservable = Observable.FromEventPattern<OnPositionChangeEventArgs>(this, "BallChanged");
                this.daneAPI = daneAPI;
                BoardSize = boardSize;

            }
            public override Vector2 getBallPosition(int index)
            {
                return this.daneAPI.GetPositionBall(index);
            }

            public Vector2 BoardSize { get; }


            public List<Ball> GetBallsList()
            {
                return daneAPI.GetBallsList();
            }
     
            public override void AddBalls(int howMany)
            {
                daneAPI.CreateBalls(howMany);
            }

            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.ballId), ex => observer.OnError(ex), () => observer.OnCompleted());
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
                System.Diagnostics.Trace.WriteLine("XDDD");
                
                    System.Diagnostics.Trace.WriteLine("XD1DD");
                    var tmpBallList = daneAPI.GetBallsList();

                    System.Diagnostics.Trace.WriteLine("XDDD2");
                    Monitor.Enter(_lock);
                    try
                    {
                        Collisions collisions = new Collisions(tmpBallList[value].Position, tmpBallList[value].Velocity, 40);
                        
                        for(int i = 1; i < tmpBallList.Count+1; i++)
                        {
                            System.Diagnostics.Trace.WriteLine("XDDD3");
                            Vector2[] VelocityTab = collisions.ImpulseSpeed(tmpBallList[value].Velocity, tmpBallList[i].Velocity);
                            daneAPI.SetBallSpeed(value, VelocityTab[0]);
                            daneAPI.SetBallSpeed(i, VelocityTab[1]);
                            if (value != i)
                            {
                                if (collisions.IsCollision(tmpBallList[i].Position+tmpBallList[i].Velocity, 40, true))
                                {
                                    if (collisions.IsCollision(tmpBallList[i].Position, 40, true))
                                    {
                                    
                                    }
                                }
                            }
                        }
                       BallChanged?.Invoke(this, new OnPositionChangeEventArgs {ballId = value });
                    }
                    catch (SynchronizationLockException exception)
                    {
                        throw new Exception("Checking collision synchronization lock not working", exception);
                    }
                    finally
                    {
                        Monitor.Exit(_lock);
                    }
                
            }
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }
        }
    }
}