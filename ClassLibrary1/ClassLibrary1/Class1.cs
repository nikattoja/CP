namespace ClassLibrary1
{
    public class Class1
    {
        private double a, b;
        public double Sum()
        {
            return a + b;
        }

        public double Multiply()
        {
            return a * b;
        }

        public Class1(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
    }
}