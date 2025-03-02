using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TrexRunner.Entities
{
    //QUAN LY VIEC XUAT HIEN VA CAP NHAT CAC DOI TUONG VAT CAN
    public class ObstacleManager : IGameEntity
    {
        // mang chua cac vi tri y cho Flying Dino
        private static readonly int[] FLYING_DINO_Y_POSITIONS = new int[] { 90, 62, 24 };

        //khoang cach toi thieu de bat dau xuat hien chuong ngai vat
        private const float MIN_SPAWN_DISTANCE = 10;

        // khoang cach toi thieu va toi da giua cac chuong ngai vat
        private const int MIN_OBSTACLE_DISTANCE = 6;
        private const int MAX_OBSTACLE_DISTANCE = 28;

        // do bien doi cua khoang cach giua cac cnv dua tren toc do cua Trex
        private const int OBSTACLE_DISTANCE_SPEED_TOLERANCE = 5;

        // vi tri y cho cac xr lon nho
        private const int LARGE_CACTUS_POS_Y = 80;
        private const int SMALL_CACTUS_POS_Y = 94;

        //thu tu ve
        private const int OBSTACLE_DRAW_ORDER = 12;

        // vi tri x de loai bo chuong ngai vat khi no vuot qua man hinh
        private const int OBSTACLE_DESPAWN_POS_X = -200;

        //diem toi thieu de bat dau xuat hien Flying Dino
        private const int FLYING_DINO_SPAWN_SCORE_MIN = 150;

        private double _lastSpawnScore = -1;        // luu tru diem cuoi cung khi mot chuong ngai vat duoc xuat hien
        private double _currentTargetDistance;      // khoang cach giua cac cnv

        private readonly EntityManager _entityManager; //quan ly cac thuc the
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreBoard;        //theo doi diem va hien thi diem

        private readonly Random _random;

        private Texture2D _spriteSheet;     //texture2D chua cac hinh anh cua cnv

        //Kiem tra 'ObstacleManager' co duoc kich hoat hay khong
        public bool IsEnabled { get; set; }

        //Xac dinh xam co the xuat hien cnv hay khong dua tren dieu kien kich hoat va diem
        public bool CanSpawnObstacles => IsEnabled && _scoreBoard.Score >= MIN_SPAWN_DISTANCE;

        public int DrawOrder => 0;

        public ObstacleManager(EntityManager entityManager, Trex trex, ScoreBoard scoreBoard, Texture2D spriteSheet)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreBoard = scoreBoard;
            _random = new Random();
            _spriteSheet = spriteSheet;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        //Cap nhat vi tri va kiem tra dieu kien de xuat hien cnv
        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;

            if (CanSpawnObstacles &&
                (_lastSpawnScore <= 0 || (_scoreBoard.Score - _lastSpawnScore >= _currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble()
                    * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE) + MIN_OBSTACLE_DISTANCE;

                _currentTargetDistance += (_trex.Speed - Trex.START_SPEED) / (Trex.MAX_SPEED - Trex.START_SPEED) * OBSTACLE_DISTANCE_SPEED_TOLERANCE;

                _lastSpawnScore = _scoreBoard.Score;

                SpawnRandomObstacle();
            }

            foreach (Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                if (obstacle.Position.X < OBSTACLE_DESPAWN_POS_X)
                    _entityManager.RemoveEntity(obstacle);

            }

        }

        //Tao ngau nhien cnv và them vao EntityManager
        private void SpawnRandomObstacle()
        {

            Obstacle obstacle = null;

            int cactusGroupSpawnRate = 75;
            int flyingDinoSpawnRate = _scoreBoard.Score >= FLYING_DINO_SPAWN_SCORE_MIN ? 25 : 0;

            int rng = _random.Next(0, cactusGroupSpawnRate + flyingDinoSpawnRate + 1);

            if (rng <= cactusGroupSpawnRate)
            {
                CactusGroup.GroupSize randomGroupSize = (CactusGroup.GroupSize)_random.Next((int)CactusGroup.GroupSize.Small, (int)CactusGroup.GroupSize.Large + 1);

                bool isLarge = _random.NextDouble() > 0.5f;

                float posY = isLarge ? LARGE_CACTUS_POS_Y : SMALL_CACTUS_POS_Y;

                obstacle = new CactusGroup(_spriteSheet, isLarge, randomGroupSize, _trex, new Vector2(TRexRunnerGame.WINDOW_WIDTH, posY));

            }
            else
            {
                int verticalPosIndex = _random.Next(0, FLYING_DINO_Y_POSITIONS.Length);
                float posY = FLYING_DINO_Y_POSITIONS[verticalPosIndex];

                obstacle = new FlyingDino(_trex, new Vector2(TRexRunnerGame.WINDOW_WIDTH, posY), _spriteSheet);
            }

            obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;

            _entityManager.AddEntity(obstacle);

        }

        //Loai bo tat ca cac cnv va dat lai cac thuoc tinh
        public void Reset()
        {

            foreach (Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                _entityManager.RemoveEntity(obstacle);
            }

            _currentTargetDistance = 0;
            _lastSpawnScore = -1;

        }

    }
}