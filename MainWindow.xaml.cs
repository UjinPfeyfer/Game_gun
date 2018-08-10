using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        bool ifHit = false;
        Path pa = new Path();
        int hitCount = 0;
        bool hit = false;
        int a = 3;
        int b = 3;
        Grid bulArea;
        Grid labArea;
        Storyboard storb;
        TranslateTransform trans = new TranslateTransform();
        Ellipse Wheel = new Ellipse();
        Line lineGun = new Line();
        Gun gun;
        Bullet bul;
        public MainWindow()
        {
            gun = new Gun(0, 1);
            double x = 90;
            double y = 160;
            InitializeComponent();
            bulArea = new Grid();
            labArea = new Grid();
            Label labV = new Label();
            labArea.Children.Add(labV);
            labV.Content = "Скорость:";
            labV.Margin = new Thickness(-15, -30, 0, 0);
            bulArea.Width = 100;
            bulArea.Children.Add(Wheel);
            bulArea.Children.Add(labArea);
           // bulArea.Background = Brushes.Red;               //Цвет поля снаряда
            bulArea.Margin = new Thickness(0, 0, 0, 0);
            GameField.Children.Remove(labelbul);
            labArea.Children.Add(labelbul);
            //labArea.Background = Brushes.Green;           //Цвет поля надписей
            labelbul.DataContext = bul;
            labArea.Margin = new Thickness(20, 0, 0, 0);
            labArea.Visibility = Visibility.Hidden;
            btn.Width = this.Width;
            btn.Height = this.Height;
            btn.Opacity = 0;
            labelbul.HorizontalContentAlignment = HorizontalAlignment.Right;
            Wheel.Width = 10;
            Wheel.Height = 10;
            Wheel.Fill=Brushes.Black;
            Wheel.Margin = new Thickness(-95, 0, 0, 0);
            Wheel.Visibility = Visibility.Hidden;
            Wheel.Stroke = Brushes.Black;
            Wheel.DataContext = gun;
            Wheel.MouseEnter += ellipse1_MouseEnter;
            Wheel.MouseLeave += ellipse1_MouseLeave;
            Wheel.SetBinding(Canvas.TopProperty, new Binding("Y"));
            Wheel.SetBinding(Canvas.LeftProperty, new Binding("X"));
            GameField.Children.Add(bulArea);
            Canvas.SetLeft(bulArea, x + 10);
            Canvas.SetTop(bulArea, y);
            GameField.DataContext = gun;
            GameField.Children.Add(pa);

        }
        private void Explode(object sender, EventArgs e)
        {
            pa.Visibility = Visibility.Visible;
            double x = bul.lastX();
            double y = bul.lastY();
            DoubleAnimation expanim = new DoubleAnimation();
            PathFigure expl = new PathFigure();
            expl.StartPoint = (new Point(x,y+10));
            expl.Segments.Add(new LineSegment(new Point(x - 20, y - 20), true));
            expl.Segments.Add(new LineSegment(new Point(x - 10, y - 15), true));
            expl.Segments.Add(new LineSegment(new Point(x - 18, y - 40), true));
            expl.Segments.Add(new LineSegment(new Point(x , y - 16), true));
            expl.Segments.Add(new LineSegment(new Point(x +18, y - 40), true));
            expl.Segments.Add(new LineSegment(new Point(x + 10, y - 15), true));
            expl.Segments.Add(new LineSegment(new Point(x + 20, y - 20), true));
            expl.Segments.Add(new LineSegment(new Point(x, y+10), true));
            PathGeometry pa2 = new PathGeometry();
            pa2.Figures.Add(expl);
            pa.Data = pa2;
            pa.Stroke = Brushes.Red;
            if (hit)
                GameMessage.Content = "Есть попадание!";
            else
                GameMessage.Content = "Мимо";
            HitCount.Content = hitCount;
            GameMessage.Visibility = Visibility.Visible;
            DoubleAnimation dop = new DoubleAnimation();
            dop.From = 1;
            dop.To = 0;
            dop.Duration = TimeSpan.FromSeconds(2);
            expanim.Duration=TimeSpan.FromSeconds(2);
            expanim.From = 120;
            expanim.To = 50;
            expanim.Completed += new EventHandler( Expanim_Completed);
            GameMessage.BeginAnimation(Canvas.TopProperty,expanim);
            GameMessage.BeginAnimation(Label.OpacityProperty, dop);
            Wheel.Visibility = Visibility.Hidden;
        }

        private void Expanim_Completed(object sender, EventArgs e)
        {
            pa.Visibility = Visibility.Hidden;
            a = b;
            if (b == 0)
            {
                newgame.Visibility = Visibility.Visible;
                MenuField.Visibility = Visibility.Visible;
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 1;
                da.Duration = TimeSpan.FromSeconds(2);
                MenuField.BeginAnimation(Grid.OpacityProperty, da);
                Score2.Content = hitCount;
                Score1.Visibility = Visibility.Visible;
                Score2.Visibility = Visibility.Visible;
            }
            ifHit = false;
        }

        private PathGeometry Parabola()
        {
            hit = false;
            PathGeometry pathG = new PathGeometry();
            PathFigure path2 = new PathFigure();
            path2.StartPoint = new Point(bul.X, bul.Y);
            double t = bul.t * 100;
            for (int i = 0; i <= t; i++)
            {
                Point a = new Point(bul.getX((double)i / 100), bul.getY((double)i / 100));
                path2.Segments.Add(new LineSegment(a, false));
                if (Path1.Data.FillContains(a, 10, ToleranceType.Absolute))
                {
                    bul.t = bul.t/ t*i;
                    pathG.Figures.Add(path2);
                    return pathG;
                }
                if (Path2.Data.FillContains(a, 10, ToleranceType.Absolute))
                    hit=true;
            }
            if (hit) hitCount++;

            //path2.Segments.Add(new BezierSegment(new Point(bul.getX(3), bul.getY(3)), new Point(bul.getX(3.0 / 2), bul.getY(3.0 / 2)), new Point(bul.lastX(), bul.lastY()), false));
            pathG.Figures.Add(path2);
            return pathG;
        }
        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    if (double.Parse(TB1.Content as string) > 0)
        //        gun.Angle = double.Parse(TB1.Content as string);
        //    else
        //        gun.Angle = -double.Parse(TB1.Content as string);
        //    rec.LayoutTransform = new RotateTransform(90 - double.Parse(TB1.Content as string));

        //}
        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    powBar.Value = double.Parse(TB2.Content as string);


        //}
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //TB2.Content = powBar.Value.ToString();
            ((Slider)sender).SelectionEnd = e.NewValue;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (b != 0&&!ifHit)
            {
                powBar.Value += 1;
                Point p = Mouse.GetPosition(GameField);
                if (p.X > 100 && p.Y < 200)
                    gun.Angle = Math.Acos((p.X - 100) / Math.Sqrt((p.X - 100) * (p.X - 100) + (p.Y - 200) * (p.Y - 200))) * 180 / Math.PI;
                // TB1.Content = Math.Round(gun.Angle, 1);
                rec.LayoutTransform = new RotateTransform(90 - gun.Angle);
            }
        }
        private void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (b != 0)
                if (gun.Power > 20)
                {
                    ifHit = true;
                    b--;
                    bul = new Bullet(100 + 30 * Math.Cos(gun.Angle / 180 * Math.PI), 200 - 30 * Math.Sin(gun.Angle / 180 * Math.PI), Math.Sin(gun.Angle / 180 * Math.PI) * gun.Power, Math.Cos(gun.Angle / 180 * Math.PI) * gun.Power);
                    //bul.tFinished += new Bullet.tHendler(explode);
                    Path pathhh = new Path();
                    pathhh.Stroke = Brushes.Red;
                    PathGeometry pathG2 = Parabola();                   
                        Wheel.Visibility = Visibility.Visible;
                        DoubleAnimationUsingPath dx = new DoubleAnimationUsingPath();
                        dx.Duration = TimeSpan.FromSeconds(bul.t / 2);
                        dx.PathGeometry = pathG2;
                        dx.Source = PathAnimationSource.X;
                        Storyboard.SetTargetProperty(dx, new PropertyPath(Canvas.LeftProperty));
                        DoubleAnimationUsingPath dy = new DoubleAnimationUsingPath();
                        dy.Duration = TimeSpan.FromSeconds(bul.t / 2);
                        dy.PathGeometry = pathG2;
                        dy.Source = PathAnimationSource.Y;
                        Storyboard.SetTargetProperty(dy, new PropertyPath(Canvas.TopProperty));
                        storb = new Storyboard();
                        storb.Children.Add(dx);
                        storb.Children.Add(dy);
                        storb.Completed += new EventHandler(Explode);
                        storb.Begin(bulArea, true);
                        powBar.Value = 0;
                        lab1.Content = b;
                        gun.Angle = 0;
                        rec.LayoutTransform = new RotateTransform(90 - gun.Angle);
                }
                else
                {
                    powBar.Value = 0;
                    GameMessage.Content = "Слишком слабый выстрел!";
                    GameMessage.Visibility = Visibility.Visible;
                    gun.Angle = 0;
                    rec.LayoutTransform = new RotateTransform(90 - gun.Angle);
                }
            a = b;
            
        }
        private void ellipse1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (bul != null)
            {
                double time = storb.GetCurrentTime(bulArea).Value.Seconds + ((double)storb.GetCurrentTime(bulArea).Value.Milliseconds) / 1000;
                if (time<=bul.t/2-0.01)
                labArea.Visibility = Visibility.Visible;
                labelbul.Content = bul.GetSpeed(time);
                storb.Pause(bulArea);
            }
            else labelbul.Content = 0;
        }
        private void ellipse1_MouseLeave(object sender, MouseEventArgs e)
        {
            storb.Resume(bulArea);
            labArea.Visibility = Visibility.Hidden;
        }
        private void btn_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (b != 0&&!ifHit)
            {
                GameMessage.Visibility = Visibility.Hidden;
                Point p = e.GetPosition(this);
                if (p.X > 100 && p.Y < 200)
                    gun.Angle = Math.Acos((p.X - 100) / Math.Sqrt((p.X - 100) * (p.X - 100) + (p.Y - 200) * (p.Y - 200))) * 180 / Math.PI;
                //TB1.Content = Math.Round(gun.Angle, 1).ToString();
                rec.LayoutTransform = new RotateTransform(90 - gun.Angle);
            }
        }

        private void newgame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            newgame.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = TimeSpan.FromSeconds(3);
            da.From = 1;
            da.To = 0;
            da.Completed += new EventHandler(da_Completed);
            MenuField.BeginAnimation(Grid.OpacityProperty, da);
            newgame.Visibility = Visibility.Hidden;
            a = 3;
            b = 3;
            hitCount = 0;
            HitCount.Content = hitCount;
            lab1.Content = a;
        }

        void da_Completed(object sender, EventArgs e)
        {
            MenuField.Visibility = Visibility.Hidden;
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Expanim_Completed(null, null);
        }
    }
    public class Gun : DependencyObject
    {
        double angle;
        double power;
        public double Power
        /*{
            get { return (double)GetValue(Xpower); }
            set { if (value > 0 && value < 100) SetValue(Xpower, value); }
        }*/
        {
            get { return power; }
            set { power = value; }
        }
        // public static readonly DependencyProperty Xpower = DependencyProperty.Register
        //   ("Xpower", typeof(double), typeof(Gun), new PropertyMetadata());
        public Gun(double angle, double power)
        {
            this.angle = angle;
            this.power = power;
        }
        public double Angle
        {
            get { return angle; }
            set { if (value >= 0 && value <= 90)angle = value; }
        }
    }
    //public class TEventArgs:EventArgs
    //{
    //    double time;
    //    public double Time { get { return time; } }
    //    public TEventArgs(double time)
    //    {
    //        this.time=time;
    //    }
    //}
    public class Bullet : DependencyObject
    {
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value);
            //if ((double)GetValue(XProperty) == 200)
            }
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register
            ("X", typeof(double), typeof(Bullet), new PropertyMetadata());
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register
            ("Y", typeof(double), typeof(Bullet), new PropertyMetadata());
        private double g = 9.81;
        private double T;
        private double TReal;
        //public delegate void tHendler(TEventArgs arg);
        //public event tHendler tFinished;
        public double t
        {
            get { return T; }
            set
            {
                if (value < 0.3)
                    T = 0.3;
                else
                    T = value;
                TReal = value;
            }
        }
        //private void OnTFinished(double xpos)
        //{
        //    if (tFinished != null)
        //        tFinished(new TEventArgs(xpos));
        //}
        public Bullet(double x, double y, double speedY, double speedX)
        {
            this.X = x;
            this.Y = y;
            this.speedX = speedX;
            this.speedY = -speedY;
            t = (-this.speedY + Math.Sqrt(speedY * speedY - 2 * g * (Y - 210))) / g;
        }
        public double speedY { get; set; }
        public double speedX { get; set; }

        public double GetSpeedY(double k)
        {double f= Math.Round(-speedY-g*k*2,2);
        return f;
        }
        public double GetSpeed(double k)
        { return Math.Round(Math.Sqrt(GetSpeedY(k) * GetSpeedY(k) + speedX * speedX), 2); }
        public double getX(double k)
        {
            return X + speedX * k;
        }
        public double getY(double k)
        {

            return Y + speedY * k + k * k * g / 2;
        }
        public double lastX()
        {
            return X + speedX * T;
        }
        public double lastY()
        {
            return Y + speedY * T  + T * T * g / 2;
        }

    }
}
