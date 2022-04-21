using System;

namespace Model
{
    public abstract class BallModelApi
    {
        public abstract int Radius { get; }

        public static BallModelApi CreateApi()
        {
            return new ModelApi();
        }
    }

    internal class ModelApi : BallModelApi
    {
        public override int Radius => 100;
    }
}
