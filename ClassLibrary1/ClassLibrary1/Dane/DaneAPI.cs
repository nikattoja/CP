namespace TPW.Dane
{
    public abstract class DaneAPI
    {
        public static DaneAPI CreateDataBall()
        {
            return new DaneBall();
        }

        private class DaneBall : DaneAPI
        {
            public DaneBall()
            {

            }
        }
    }
}