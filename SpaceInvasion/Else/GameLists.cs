using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvasion
{
    class GameLists
    {
        private List<EnemyProjectile> EnemyProjectiles { get; set; }
        private List<EnemyProjectile> TemporaryEnemyProjectiles { get; set; }
        private List<PlayerProjectile> PlayerProjectiles { get; set; }
        private List<PlayerProjectile> TemporaryPlayerProjectiles { get; set;}
        private List<Invader> Invaders { get; set; }
        private List<Invader> TemporaryInvaders { get; set; }
        private List<Ufo> Ufos { get; set; }
        private List<Ufo> TemporaryUfos { get; set; }
        private List<Wall> Walls { get; set; }
        private List<Wall> TemporaryWalls { get; set; }
        public double difficultyIndex = 1;
        private int PlayerReloadTimer = 0;
        private int UfoReloadTimer = 0;
        public double score;
        private int levelcounter=1;
        private Random Rnd { get; set; }
        public Player Player { get; set; }
        private Canvas GameDisplay { get; set; }
        public GameLists(Canvas gameDisplay,double difficultyIndex)
        {
            this.EnemyProjectiles = new List<EnemyProjectile>();
            this.TemporaryEnemyProjectiles = new List<EnemyProjectile>();
            this.PlayerProjectiles = new List<PlayerProjectile>();
            this.TemporaryPlayerProjectiles = new List<PlayerProjectile>();
            this.Invaders = new List<Invader>();
            this.TemporaryInvaders = new List<Invader>();
            this.Ufos = new List<Ufo>();
            this.TemporaryUfos = new List<Ufo>();
            this.Walls = new List<Wall>();
            this.TemporaryWalls = new List<Wall>();
            this.difficultyIndex = difficultyIndex;
            this.Player = new Player(gameDisplay, 10);
            this.GameDisplay = gameDisplay;
            this.Rnd = new Random();
        }
        public void CheckGameCollision()//discovers collisions between all projectiles and other gameobjects
        {
            CheckEnemyProjectileCollision();
            CheckplayerProjectileCollisions();
        }
        public void SetLevel(double PlayerAndWallSkin)//first Level is generated
        {
            SetPlayer(PlayerAndWallSkin);
            SetWalls(PlayerAndWallSkin);
            SetInvaderRows();
        }
        public void GameObjectMovementAndShoot()//movement and shoots of the gameobjects
        {
            Player.Movement();
            UfoDraw();//random UFO spawn
            EnemyMovement();//UFO and Invader movement
            EnemyShoot();
            PlayerShoot();
        }
        public void SetPlayer(double PlayerAndWallSkin)
        {

            switch (PlayerAndWallSkin)//instanciates a new Image for the player
            {
                case 0: Player.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_red.PNG", UriKind.Relative))); break;
                case 1: Player.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_green.PNG", UriKind.Relative))); break;
                case 2: Player.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_blue.PNG", UriKind.Relative))); break;
                case 3: Player.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_orange.PNG", UriKind.Relative))); break;
            }
        }
        public void DrawGameObjects()//draw all Gameobjects into the gamedisplay
        {
            foreach (Invader invader in Invaders)
            {
                invader.Draw();
            }
            foreach (Wall wall in Walls)
            {
                wall.Draw();
            }
            foreach (Ufo ufo in Ufos)
            {
                ufo.Draw();
            }
            foreach (PlayerProjectile playerprojectile in PlayerProjectiles)
            {
                playerprojectile.Draw();
                playerprojectile.Shoot();
            }
            foreach (EnemyProjectile Projectile in EnemyProjectiles)
            {
                Projectile.Draw();
                Projectile.Shoot();
            }
            Player.Draw();
        }
        public void NewEnemies(double PlayerAndWallSkin)//once the level has been completed, new opponents and walls are generated
        {
            if (Invaders.Count == 0)
            {
                SetInvaderRows();
                Walls.Clear();
                SetWalls(PlayerAndWallSkin);
                levelcounter++;
                score = score + 50 + 50 * difficultyIndex;//points for the completed level
            }
        }
        public bool IsGameOver()//returns the value true if the player loses all healthpoints or the invaders get too close
        {
            foreach (Invader i in Invaders)
            {
                if (i.Y > 500 || Player.HealthPoints == 0)
                    return true;

            }
            return false;
        }
        public void CleanTemporaryGameLists()//gameobject lists are cleaned up
        {
            GameDisplay.Children.Clear();
            //lists are updated
            EnemyProjectiles = EnemyProjectiles.Except(TemporaryEnemyProjectiles).ToList();
            PlayerProjectiles = PlayerProjectiles.Except(TemporaryPlayerProjectiles).ToList();
            Ufos = Ufos.Except(TemporaryUfos).ToList();
            Walls = Walls.Except(TemporaryWalls).ToList();
            Invaders = Invaders.Except(TemporaryInvaders).ToList();
            //temporary lists are cleared
            TemporaryWalls.Clear();
            TemporaryPlayerProjectiles.Clear();
            TemporaryInvaders.Clear();
            TemporaryEnemyProjectiles.Clear();
            TemporaryUfos.Clear();
        }
        public void ClearGame()//Removes all Gameobjects from the Gamedisplay and resets the Gamevalues
        {
            Player.HealthPoints = 3;
            score = 0;
            levelcounter = 1;
          
            PlayerProjectiles.Clear();
            EnemyProjectiles.Clear();
            Invaders.Clear();
            Ufos.Clear();
            Walls.Clear();
            GameDisplay.Children.Clear();
            
        }
        public double ActualLevel()//returns the actual value of the levelcounter
        {
            return levelcounter;
        }
        public double ActualPLayerHP()//returns the actual value of player healthpoints
        {
            return Player.HealthPoints;
        }
        private void CheckplayerProjectileCollisions()//discovers collisions between player projectiles and other gameobjects
        {
            foreach (PlayerProjectile PlayerProjectile in PlayerProjectiles)
            {
                foreach (Wall wall in Walls)
                {
                if (wall.IsHit(PlayerProjectile.X, PlayerProjectile.Y))
                    {
                        TemporaryWalls.Add(wall);//removes wall
                        TemporaryPlayerProjectiles.Add(PlayerProjectile);//removes player projectile
                    }
                }
                foreach (Invader invader in Invaders)
                {
                    if (invader.IsHit(PlayerProjectile.X, PlayerProjectile.Y))
                    {
                        TemporaryInvaders.Add(invader);//removes invader
                        TemporaryPlayerProjectiles.Add(PlayerProjectile);//removes player projectile
                        score = score + 5 + 15 * difficultyIndex;//score for a destroyed invader
                    }
                }
                foreach (Ufo ufo in Ufos)
                {
                    if (ufo.IsHit(PlayerProjectile.X, PlayerProjectile.Y))
                    {
                        score += 50;
                        TemporaryUfos.Add(ufo);//removes UFO
                        TemporaryPlayerProjectiles.Add(PlayerProjectile);//removes player projectile
                    }
                }
                if (PlayerProjectile.Y < 0)
                    TemporaryPlayerProjectiles.Add(PlayerProjectile);//removes player projectile
            }
        }
        private void CheckEnemyProjectileCollision()//discovers collisions between enemy projectiles and other gameobjects
        {
            foreach (EnemyProjectile EnemyProjectile in EnemyProjectiles)
            {
                foreach (Wall wall in Walls)
                {
                    if (wall.IsHit(EnemyProjectile.X, EnemyProjectile.Y + 10))
                    {
                        TemporaryEnemyProjectiles.Add(EnemyProjectile);//removes enemy projectile
                        TemporaryWalls.Add(wall);//removes wall
                    }
                }
                if (Player.IsHit(EnemyProjectile.X, EnemyProjectile.Y))
                {
                    Player.HealthPoints--;//removes one player life
                    TemporaryEnemyProjectiles.Add(EnemyProjectile);//removes enemy projectile
                }
                if (EnemyProjectile.Y > GameDisplay.ActualHeight)
                {
                    TemporaryEnemyProjectiles.Add(EnemyProjectile);//removes enemy projectile
                }
            }
        }
        private void SetInvaderRows()//generate Invaders
        {
            int randomInt = Rnd.Next(0, 4);//random invader skin
            //generates five rows of aliens
            for (int j = 0; j < 5; j++)
            {
                //generates 15 aliens per row
                for (int i = 0; i < 15; i++)
                {
                    //new object of the type Invader
                    Invader OneInvader = new Invader(GameDisplay, 5 + difficultyIndex * 2.1)
                    {
                        X = i * 35 + 5,
                        Y = (j * 35 + 5) + 50
                    };
                    Invaders.Add(OneInvader);
                    switch (randomInt)//instanciates a new Image foreach Invader
                    {
                        case 0:switch (j)
                        {
                             case 0: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyRed1.PNG", UriKind.Relative))); break;
                             case 1: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyRed2.PNG", UriKind.Relative))); break;
                             case 2: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyRed3.PNG", UriKind.Relative))); break;
                             case 3: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyRed4.PNG", UriKind.Relative))); break;
                             case 4: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyRed5.PNG", UriKind.Relative))); break;
                        }break;
                        case 1:switch (j)
                        {
                             case 0: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlue1.PNG", UriKind.Relative))); break;
                             case 1: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlue2.PNG", UriKind.Relative))); break;
                             case 2: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlue3.PNG", UriKind.Relative))); break;
                             case 3: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlue4.PNG", UriKind.Relative))); break;
                             case 4: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlue5.PNG", UriKind.Relative))); break;
                        }break;
                        case 2:switch (j)
                        {
                            case 0: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlack1.PNG", UriKind.Relative))); break;
                            case 1: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlack2.PNG", UriKind.Relative))); break;
                            case 2: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlack3.PNG", UriKind.Relative))); break;
                            case 3: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlack4.PNG", UriKind.Relative))); break;
                            case 4: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyBlack5.PNG", UriKind.Relative))); break;
                        }break;
                        case 3:switch (j)
                        {
                            case 0: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyGreen1.PNG", UriKind.Relative))); break;
                            case 1: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyGreen2.PNG", UriKind.Relative))); break;
                            case 2: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyGreen3.PNG", UriKind.Relative))); break;
                            case 3: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyGreen4.PNG", UriKind.Relative))); break;
                            case 4: OneInvader.reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyGreen5.PNG", UriKind.Relative))); break;
                        }break;
                    }
                }
            }
        }
        private void SetWalls(double PlayerAndWallSkin)//generate Walls
        {
            //set three rows of walls
            for (int row = 0; row < 3; row++)
            {
                //set six walls per row
                for (int wallPerRow = 0; wallPerRow < 6; wallPerRow++)
                {
                    //set five wallBlocks
                    for (int wallStack = 0; wallStack < 5; wallStack++)
                    {
                        //new object of the type Wall
                        Wall w = new Wall(GameDisplay)
                        {
                            X = wallPerRow * (20 + 1) + (wallStack * 730 / 5) + 15,
                            Y = row * 20 + 550
                        };
                        Walls.Add(w);
                        switch (PlayerAndWallSkin)//instanciates a new Image foreach Wall
                        {
                         case 0: w.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallRed.PNG", UriKind.Relative))); break;
                         case 1: w.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallGreen.PNG", UriKind.Relative))); break;
                         case 2: w.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallBlue.PNG", UriKind.Relative))); break;
                         case 3: w.Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallYellow.PNG", UriKind.Relative))); break;
                        }
                    }
                }
            }
        }
        private void EnemyShoot()//let every enemy randomly or over a duration fire a projectile
        {
            EnemyProjectile enemyProjectile;
            foreach (Invader enemy in Invaders)
            {
                if (Rnd.Next(0, 800) == 1)
                {
                    enemyProjectile = new EnemyProjectile(GameDisplay, enemy, 7);//new object of the type EnemyProjectile
                    EnemyProjectiles.Add(enemyProjectile);
                }
            }
            if (UfoReloadTimer < 100)
                UfoReloadTimer++;
            if (UfoReloadTimer > 90)
            {
                foreach (Ufo enemy in Ufos)
                {
                    enemyProjectile = new EnemyProjectile(GameDisplay, enemy, 7);//new object of the type EnemyProjectile
                    EnemyProjectiles.Add(enemyProjectile);
                }
                UfoReloadTimer -= 16;
            }
        }
        private void PlayerShoot()//let the player shoot player projectiles
        {
            PlayerProjectile pp;
            if (PlayerReloadTimer <=10)
            {
                PlayerReloadTimer++;
            }
            if (Keyboard.IsKeyDown(Key.Space) && PlayerReloadTimer >= 10)//if space is pressed the player shoots
            {
                PlayerReloadTimer -= 10;
                pp = new PlayerProjectile(GameDisplay, Player, 15);//new object of the type PlayerProjectile
                PlayerProjectiles.Add(pp);
            }
        }
        private void UfoDraw()//randomly generates an UFO
        {
            if (Rnd.Next(0, 800) == 1)
                Ufos.Add(new Ufo(GameDisplay, 5));//new object of the type Ufo
        }
        private void EnemyMovement()//UFO and Invader movement
        {
            double maxrightinvader = 0;//gamedisplay left side
            double maxileftinvader = 700;//gamedisplay right side
            int index = 0;
            foreach (Invader invader in Invaders)
            {

                if (invader.X > maxrightinvader)
                    maxrightinvader = invader.X;
                if (invader.X < maxileftinvader)
                    maxileftinvader = invader.X;
                if (maxrightinvader > GameDisplay.ActualWidth - 30)
                {
                    index = 1;//Index=1 reverse the movement of the invaders, so they move left
                }
                if (maxileftinvader < 0)
                {
                    index = 2;//Index=2 reverse the movement of the invaders, so they move right
                }
            }
            
            foreach (Invader invader in Invaders)//Invader movement
            {
                invader.Movement(index);
            }
            foreach (Ufo ufo in Ufos)//UFO movement
            {
                ufo.Movement();
            }
        }
    }
}
