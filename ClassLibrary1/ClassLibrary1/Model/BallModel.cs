using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using TPW.Presentation.Model;

namespace TPW.Presentation.Model
{
    
    public class BallModel : IBall
    {


        public event PropertyChangedEventHandler PropertyChanged;

        public BallModel(double y, double X)
        {
           
            Y = y;
            this.X = X;
        }

        private double top;

        public double Y
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

        public double X
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

        public void Move(double positionX, double positionY)
        {
           X = positionX;
           Y = positionY;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    }

