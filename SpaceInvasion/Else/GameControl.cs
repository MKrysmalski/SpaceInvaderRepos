using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;

namespace SpaceInvasion
{
    class GameControl:INotifyPropertyChanged
    {
        private DispatcherTimer Timer { get; set; }
        public Canvas GameDisplay { get; set; }
        public StreamWriter Writer { get; set; }
        public StreamReader Reader { get; set; }
        public GameLists GameLists { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        string game_Score = "Score: 0";
        string game_Highscore="Highscore: 0";
        double highscore = 0;
        readonly string DataPath = @"Highscores\TopHighscore.txt";
        double playerAndWallSkinSetter;
        string game_Level="Level: 1";
        string game_player_HP = "Lifes: 3";
        double game_Difficulty;
        string difficulty_Text = "easy";
        public string Game_Score
        {
            get { return game_Score; }
            set
            {
                game_Score = value;
                OnPropertyChanged("Game_Score");
            }
        }
        public string Difficulty_Text
        {
            get { return difficulty_Text; }
            set
            {
                difficulty_Text = value;
                OnPropertyChanged("Difficulty_Text");
            }
        }
        public double Game_Difficulty
        {
            get { return game_Difficulty; }
            set
            {
                
                game_Difficulty = value;
                OnPropertyChanged("Game_Difficulty");
            }
        }
        public double PlayerAndWallSkinSetter
        {
            get { return playerAndWallSkinSetter; }
            set
            {
                playerAndWallSkinSetter = value;
                OnPropertyChanged("PlayerAndWallSkinSetter");
            }
        }
        public string Game_Highscore
        {
            get { return game_Highscore; }
            set
            {
                game_Highscore = value;
                OnPropertyChanged("Game_Highscore");
            }
        }
        public string Game_Level
        {
            get { return game_Level; }
            set
            {
                game_Level = value;
                OnPropertyChanged("Game_Level");
            }
        }
        public string Game_PlayerHP
        {
            get { return game_player_HP; }
            set
            {
                game_player_HP = value;
                OnPropertyChanged("Game_PlayerHP");
            }
        }
        public GameControl(Canvas GameDisplay)
        {
            this.Timer = new DispatcherTimer();
            this.GameDisplay = GameDisplay;
            this.GameLists = new GameLists(GameDisplay,0);
            Timer.Tick += Timer_Tick;
            

        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        public void StartGame()
        {
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            Timer.Start();

            Reader = new StreamReader(DataPath);
            highscore = Convert.ToDouble(Reader.ReadToEnd());
            Reader.Close();//The Highscore is loaded from a textfile into the Game
            game_Highscore = string.Format("Highscore: {0}", highscore);
            OnPropertyChanged("Game_Highscore");

            GameLists.difficultyIndex = game_Difficulty;//Level difficulty is updated
            GameLists.SetLevel(playerAndWallSkinSetter);//Level is generated
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            GameLists.CleanTemporaryGameLists(); //lists are cleaned up
            GameLists.DrawGameObjects(); //all current gameobjects are drawn in their current position
            GameLists.NewEnemies(playerAndWallSkinSetter); //once the level has been completed, new opponents are generated
            GameLists.GameObjectMovementAndShoot(); //movement and shoots of the gameobjects
            GameLists.CheckGameCollision(); //discovers collisions between projectiles and other gameobjects

            game_Score = string.Format("Score: {0}", GameLists.score);
            OnPropertyChanged("Game_Score");//returns the current score
            game_Level = string.Format("Level: {0}", GameLists.ActualLevel());
            OnPropertyChanged("Game_Level");//returns the current level
            game_Highscore = string.Format("Highscore: {0}", highscore);
            OnPropertyChanged("Game_Highscore");//returns the current highscore
            game_player_HP = string.Format("Lifes: {0}", GameLists.ActualPLayerHP());
            OnPropertyChanged("Game_PlayerHP");//returns the current player healthpoints
            if (GameLists.IsGameOver())
            {
                Timer.Stop();
                if (highscore < GameLists.score)//checks if the current highscore is smaller than the actual score
                {
                    highscore = GameLists.score;
                    Writer = new StreamWriter(DataPath);
                    Writer.Write(Convert.ToString(highscore));//saves the new highscore
                    Writer.Close();
                    game_Highscore = string.Format("Highscore: {0}", highscore);
                    OnPropertyChanged("Game_Highscore");//returns the new highscore
                }
            }
        }
        public bool IsGameOver()
        {
           return GameLists.IsGameOver();//gives a bool value depending on whether the game is lost
        }
        public void ClearGame()
        {
            GameLists.ClearGame();//Removes all Gameobjects from the Gamedisplay and resets the Gamevalues
        }
    }
}
