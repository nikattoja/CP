using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TPW.Dane
{

      public class Logger : IDisposable
      {
        BlockingCollection<string> FirstInFirstOut;
        StreamWriter streamWriter;
        string filename = "Logger.log";
        private void endlessLoop()
        {
            try
            {
                foreach (string i in FirstInFirstOut.GetConsumingEnumerable())
                {
                    streamWriter.WriteLine(i);
                }
            }
            finally
            {
                Dispose();
            }

        }

        public Logger()
        {
            FirstInFirstOut = new BlockingCollection<string>();
            streamWriter = new StreamWriter(filename, false);
            Task.Run(endlessLoop);
        }

        public void log(Ball ball) => FirstInFirstOut.Add(DateTime.Now.ToString("HH:mm:ss ")
                    + " ID: " + ball.id 
                    + " Position.X: " 
                    + Math.Round(ball.Position.X, 4) 
                    + " Position.Y: " 
                    + Math.Round(ball.Position.Y, 4) 
                    + " Velocity.X: " + Math.Round(ball.Velocity.X, 4) 
                    + " Velocity.Y: " + Math.Round(ball.Velocity.Y, 4));

        public void Dispose()
        {
            streamWriter.Dispose();
            FirstInFirstOut.Dispose();
        }
    }
}
