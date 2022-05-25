using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using TPW.Presentation.Model;

namespace TPW.Presentation.Model
{
    
    public class BallModel : IBall
    {

        public int Diameter { get;}
        public event PropertyChangedEventHandler PropertyChanged;

        public BallModel(double top, double left, int radius)
        {
           
            Top = top;
            Left = left;
            Diameter = radius * 2;
        }

        private double top;

        public double Top
        {
            get { return top; }
            set 
            {
                if (top == value)
                    return;
                top = value;
                RaisePropertyChanged();
            }
        }

        private double left;

        public double Left
        {
            get { return left; }
            set
            {
                if (left == value)
                    return;
                left = value;
                RaisePropertyChanged();
            }
        }

        public void Move(double poitionX, double positionY)
        {
           Left = poitionX;
           Top = positionY;
            System.Diagnostics.Trace.WriteLine("top " + Top + " Left " + Left);
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    }

