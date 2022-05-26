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

        public abstract void StopSimulation();
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
                Subscribe(daneAPI);

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

                    var tmpBallList = daneAPI.GetBallsList();


                    Monitor.Enter(tmpBallList);
                    try
                    {
                        Collisions collisions = new Collisions(tmpBallList[value-1].Position, tmpBallList[value-1].Velocity, 0);
                        
                        for(int i = 0; i < tmpBallList.Count; i++)
                        {

                            if (value != i)
                            {
                                if (collisions.IsCollision(tmpBallList[i].Position+tmpBallList[i].Velocity, 38, true))
                                {
                                    if (collisions.IsCollision(tmpBallList[i].Position, 38, true))
                                    {
   
                                    Vector2[] VelocityTab = collisions.ImpulseSpeed(tmpBallList[value-1].Velocity, tmpBallList[i].Velocity);
                                    daneAPI.SetBallSpeed(value, VelocityTab[0]);
                                    daneAPI.SetBallSpeed(i+1, VelocityTab[1]);
                                }
                                }
                            }
                        }
                    if (tmpBallList[value-1].Position.X+tmpBallList[value-1].Velocity.X< 0 || tmpBallList[value-1].Position.X+tmpBallList[value-1].Velocity.X > BoardSize.X - 40)
                    {
                        var velocityx = tmpBallList[value-1].Velocity.X * -1;
                        Vector2 speed = new Vector2(velocityx, tmpBallList[value-1].Velocity.Y);
                        daneAPI.SetBallSpeed(value, speed);
                    }

                    if (tmpBallList[value-1].Position.Y+tmpBallList[value-1].Velocity.Y< 0 || tmpBallList[value-1].Position.Y+tmpBallList[value-1].Velocity.Y> BoardSize.Y - 40)
                    {
                        var velocityY = tmpBallList[value-1].Velocity.Y * -1;
                        Vector2 speed = new Vector2(tmpBallList[value-1].Velocity.X, velocityY);
                        daneAPI.SetBallSpeed(value, speed);
                    }
                    BallChanged?.Invoke(this, new OnPositionChangeEventArgs {ballId = value });
                    }
                    catch (SynchronizationLockException exception)
                    {
                        throw new Exception("Checking collision synchronization lock not working", exception);
                    }
                    finally
                    {
                        Monitor.Exit(tmpBallList);
                    }
                
            }
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }

            public override void StopSimulation()
            {
                throw new NotImplementedException();
            }
        }
    }
}