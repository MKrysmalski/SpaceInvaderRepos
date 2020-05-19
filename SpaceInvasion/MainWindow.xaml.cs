using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

namespace SpaceInvasion
{
    public partial class MainWindow : Window
    {
        GameControl Gamecontoller { get; set; }
        DispatcherTimer BoardUpdater { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.Gamecontoller = new GameControl(GameDisplay);
            this.BoardUpdater = new DispatcherTimer();
            BoardUpdater.Interval = new TimeSpan(0, 0, 0, 0, 100);
            BoardUpdater.Tick += BoardUpdater_Tick;
            BoardUpdater.Start();
            DataContext = Gamecontoller;
        }
        private void BoardUpdater_Tick(object sender, EventArgs e)
        {
            if (Gamecontoller.IsGameOver())
            {
                //BoardUpdater.Stop();
                Retry.Visibility = Visibility.Visible;
                Exit.Visibility = Visibility.Visible;
            }
            switch (DifficultySlider.Value)
            {
                case 0:
                    TextBlockForDifficultyEasy.Visibility = Visibility.Visible;
                    TextBlockForDifficultyMedium.Visibility = Visibility.Hidden;
                    TextBlockForDifficultyHard.Visibility = Visibility.Hidden; break;
                case 1:
                    TextBlockForDifficultyEasy.Visibility = Visibility.Hidden;
                    TextBlockForDifficultyMedium.Visibility = Visibility.Visible;
                    TextBlockForDifficultyHard.Visibility = Visibility.Hidden; break;
                case 2:
                    TextBlockForDifficultyEasy.Visibility = Visibility.Hidden;
                    TextBlockForDifficultyMedium.Visibility = Visibility.Hidden;
                    TextBlockForDifficultyHard.Visibility = Visibility.Visible; break;
            }
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            Gamecontoller.ClearGame();
            StartSetup.Width = 0;
            StartSetup.Visibility = Visibility.Hidden;
            HPText.Visibility = Visibility.Visible;
            LevelText.Visibility = Visibility.Visible;
            Gamecontoller.StartGame();
        }
        private void ShipSkin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (ShipSkin.Value)
            {
                case 0: ShipDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_red.PNG", UriKind.Relative)));
                        WallDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallRed.PNG", UriKind.Relative))); break;
                case 1: ShipDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_green.PNG", UriKind.Relative)));
                        WallDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallGreen.PNG", UriKind.Relative))); break;
                case 2: ShipDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_blue.PNG", UriKind.Relative)));
                        WallDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallBlue.PNG", UriKind.Relative))); break;
                case 3: ShipDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_orange.PNG", UriKind.Relative)));
                        WallDesign.Background = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallYellow.PNG", UriKind.Relative))); break;
            }
        }
        private void Retry_Click(object sender, RoutedEventArgs e)//restart the game with the same difficulty and skin
        {
            Gamecontoller.ClearGame();
            Retry.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            Gamecontoller.StartGame();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)//back to the main menu
        {
            Gamecontoller.ClearGame();
            StartSetup.Width = 200;
            Retry.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            StartSetup.Visibility = Visibility.Visible;
            HPText.Visibility = Visibility.Hidden;
            LevelText.Visibility = Visibility.Hidden;
        }
        private void ExitGameButton_Click(object sender, RoutedEventArgs e)//close the application
        {
            Application.Current.Shutdown();
        }
        private void CreditsGameButton_Click(object sender, RoutedEventArgs e)//show credits
        {
            MessageBox.Show("Space Invasion by Michael Krysmalski\n" +"\n"+
                            "Goal:\n"+"The goal is to defeat wave after wave of descending aliens \n" +
                            "with a horizontally moving laser to earn as many points as possible\n"+ "\n" +
                            "Control:\n" + "Move left = A\n" + "Move right = D\n" + "Shoot = Space\n" + "\n" + "Good luck and have fun!\n");
        }
    }
}
