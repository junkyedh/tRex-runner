using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrexRunner.Entities
{
    //DAI DIEN CHO CAC CAN TRO TRONG GAME
    public abstract class Obstacle : IGameEntity, ICollidable
    {
        //tham chieu den Trex => theo doi tocc do cua trex và kiem tra va cham
        private Trex _trex;

        //Moi lop con cua 'Obstacle' can trien khai thuoc tinh nay de xac dinh vung va cham
        public abstract Rectangle CollisionBox { get; }

        public int DrawOrder { get; set; }

        public Vector2 Position { get; protected set; }

        //Khoi tao
        protected Obstacle(Trex trex, Vector2 position)
        {
            _trex = trex;
            Position = position;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        //Cap nhat vi tri cua vat the dua tren toc do cua Trex
        public virtual void Update(GameTime gameTime)
        {
            float posX = Position.X - _trex.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position = new Vector2(posX, Position.Y);

            //Kiem tra va cham
            CheckCollisions();

        }

        //Kiem tra xem hop va cham cua vat the co chong lap voi hop va cham cua trex khong
        private void CheckCollisions()
        {
            Rectangle obstacleCollisionBox = CollisionBox;
            Rectangle trexCollisionBox = _trex.CollisionBox;

            if (obstacleCollisionBox.Intersects(trexCollisionBox))
            {
                //Neu co thì Die
                _trex.Die();
            }

        }

    }
}