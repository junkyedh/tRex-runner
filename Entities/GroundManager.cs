using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    //QUAN LY VA DIEU KHIEN CAC DOI TUONG DUOI MAT DAT
    public class GroundManager : IGameEntity
    {
        private const float GROUND_TILE_POS_Y = 119;

        private const int SPRITE_WIDTH = 600;
        private const int SPRITE_HEIGHT = 14;

        private const int SPRITE_POS_X = 2;
        private const int SPRITE_POS_Y = 54;

        //text2d chua sprite sheet cua tro choi
        private Texture2D _spriteSheet;
        //quan ly cac doi tuong trong tro choi
        private readonly EntityManager _entityManager;
        //danh sach cac doi tuong GoundTile
        private readonly List<GroundTile> _groundTiles;

        //Sprite cho dat phang va dat go ghe
        private Sprite _regularSprite;
        private Sprite _bumpySprite;

        //Tham chieu den doi tuong Trex de lay thong tin ve toc do
        private Trex _trex;

        private Random _random;

        public int DrawOrder { get; set; }

        public GroundManager(Texture2D spriteSheet, EntityManager entityManager, Trex trex)
        {
            _spriteSheet = spriteSheet;
            _groundTiles = new List<GroundTile>();
            _entityManager = entityManager;

            _regularSprite = new Sprite(spriteSheet, SPRITE_POS_X, SPRITE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
            _bumpySprite = new Sprite(spriteSheet, SPRITE_POS_X + SPRITE_WIDTH, SPRITE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);

            _trex = trex;
            _random = new Random();

        }
        //Duoc ve thong qua cac doi tuong 'GroundTile'
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
        
        //Cap nhat vi tri cac doi tuong dat, kiem tra va loai bo cac doi tuong 
            //da di qua man hinh
        public void Update(GameTime gameTime)
        {
            if (_groundTiles.Any())
            {
                float maxPosX = _groundTiles.Max(g => g.PositionX);

                if (maxPosX < 0)
                    SpawnTile(maxPosX);
            }

            List<GroundTile> tilesToRemove = new List<GroundTile>();

            foreach (GroundTile gt in _groundTiles)
            {
                gt.PositionX -= _trex.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (gt.PositionX < -SPRITE_WIDTH)
                {
                    _entityManager.RemoveEntity(gt);
                    tilesToRemove.Add(gt);

                }

            }

            foreach (GroundTile gt in tilesToRemove)
                _groundTiles.Remove(gt);

        }

        //Khoi tao ds cac doi tuong dat, loai bo cac doi tuong cu
        public void Initialize()
        {
            _groundTiles.Clear();

            foreach (GroundTile gt in _entityManager.GetEntitiesOfType<GroundTile>())
            {
                _entityManager.RemoveEntity(gt);
            }

            GroundTile groundTile = CreateRegularTile(0);
            _groundTiles.Add(groundTile);

            _entityManager.AddEntity(groundTile);

        }

        private GroundTile CreateRegularTile(float positionX)
        {
            GroundTile groundTile = new GroundTile(positionX, GROUND_TILE_POS_Y, _regularSprite);

            return groundTile;

        }

        private GroundTile CreateBumpyTile(float positionX)
        {
            GroundTile groundTile = new GroundTile(positionX, GROUND_TILE_POS_Y, _bumpySprite);

            return groundTile;

        }

        //Tao va them doi tuong dat moi vao danh sach
        private void SpawnTile(float maxPosX)
        {
            double randomNumber = _random.NextDouble();

            float posX = maxPosX + SPRITE_WIDTH;

            GroundTile groundTile;

            if (randomNumber > 0.5)
                groundTile = CreateBumpyTile(posX);
            else
                groundTile = CreateRegularTile(posX);

            _entityManager.AddEntity(groundTile);
            _groundTiles.Add(groundTile);

        }

    }
}