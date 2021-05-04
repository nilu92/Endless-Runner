using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EndlessRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        Rect playerHitBox;
        Rect groundHitBox;
        Rect obstacleHitBox;

        bool jumping;

        int force = 20;
        int speed = 5;

        Random random = new Random();

        bool gameOver;

        double spriteIndex = 0;

        ImageBrush playerSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();
        ImageBrush BackgroundSprite = new ImageBrush();

        int[] obstaclePosition = { 320, 310, 300, 305, 325 };
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            BackgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif"));

            Background.Fill = BackgroundSprite;
            Background2.Fill = BackgroundSprite;
            StartGame();

        }

        private void GameEngine(object sender, EventArgs e)
        {
            Canvas.SetLeft(Background, Canvas.GetLeft(Background) - 3);
            Canvas.SetLeft(Background2, Canvas.GetLeft(Background2) - 3);

            if (Canvas.GetLeft(Background) < -1262)
            {
                Canvas.SetLeft(Background, Canvas.GetLeft(Background2) + Background2.Width);
            }
            if (Canvas.GetLeft(Background2) < -1262)
            {
                Canvas.SetLeft(Background2, Canvas.GetLeft(Background) + Background.Width);
            }

            Canvas.SetTop(Player, Canvas.GetTop(Player) + speed);
            Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) - 12);

            ScoreText.Content = "Score " + score;

            playerHitBox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width - 15, Player.Height);
            obstacleHitBox = new Rect(Canvas.GetLeft(Obstacle), Canvas.GetTop(Obstacle), Obstacle.Width, Obstacle.Height);
            groundHitBox = new Rect(Canvas.GetLeft(Ground), Canvas.GetTop(Ground), Ground.Width, Ground.Height);

            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                speed = 0;

                Canvas.SetTop(Player, Canvas.GetTop(Ground) - Player.Height);

                jumping = false;

                spriteIndex += .5;
                if (spriteIndex > 8)
                {
                    spriteIndex = 1;
                }

                RunSprite(spriteIndex);
            }

            if(jumping == true)
            {
                speed = -9;
                force -= 1;
            }
            else
            {
                speed = 12;
            }

            if(force < 0)
            {
                jumping = false;
            }

            if(Canvas.GetLeft(Obstacle) < -50)
            {
                Canvas.SetLeft(Obstacle, 950);

                Canvas.SetTop(Obstacle, obstaclePosition[random.Next(0, obstaclePosition.Length)]);

                score += 1;
            }

            if (playerHitBox.IntersectsWith(obstacleHitBox))
            {
                gameOver = true;
                gameTimer.Stop();
            }
            if(gameOver == true)
            {
                Obstacle.Stroke = Brushes.Black;
                Obstacle.StrokeThickness = 1;

                Player.Stroke = Brushes.Red;
                Player.StrokeThickness = 1;

                if(score <= 2)
                {
                    ScoreText.Content = "Final Score : " + score + " Sucky much? Enter to try again";
                }else if (score >= 5)
                {
                    ScoreText.Content = "Final Score : " + score + " You much suck? Enter to try again";
                }
                
            }
            else
            {
                Player.StrokeThickness = 0;
                Obstacle.StrokeThickness = 0;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && jumping == false && Canvas.GetTop(Player) > 260)
            {
                jumping = true;
                force = 15;
                speed = -12;

                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && gameOver == true)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            Canvas.SetLeft(Background, 0);
            Canvas.SetLeft(Background2, 1262);

            Canvas.SetLeft(Player, 110);
            Canvas.SetTop(Player, 140);

            Canvas.SetLeft(Obstacle, 950);
            Canvas.SetTop(Obstacle, 310);

            RunSprite(1);

            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/obstacle.png"));
            Obstacle.Fill = obstacleSprite;

            jumping = false;
            gameOver = false;
            score = 0;

            ScoreText.Content = "Score " + score;

            gameTimer.Start();
        }

        private void RunSprite(double v)
        {
            switch (v)
            {

                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_01.gif"));
                    break;


                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
                    break;

                case 3:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_03.gif"));
                    break;

                case 4:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_04.gif"));
                    break;

                case 5:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_05.gif"));
                    break;

                case 6:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_06.gif"));
                    break;

                case 7:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_07.gif"));
                    break;

                case 8:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_08.gif"));
                    break;

               
            }
            Player.Fill = playerSprite;

        }
    }
}
