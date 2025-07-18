﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    //DAI DIEN CHO DOI TUONG BAY
    public class FlyingDino : Obstacle
    {
        private const int TEXTURE_COORDS_X = 134;
        private const int TEXTURE_COORDS_Y = 0;

        private const int SPRITE_WIDTH = 46;
        private const int SPRITE_HEIGHT = 42;

        private const float ANIMATION_FRAME_LENGTH = 0.2f;

        private const int VERTICAL_COLLISION_INSET = 10;
        private const int HORIZONTAL_COLLISION_INSET = 6;

        private const float SPEED_PPS = 80f;

        private SpriteAnimation _animation;

        //tham chieu den doi tuong Trex de kiem tra xem no con sóng hay khong
        private Trex _trex;

        //Rectangle duoc tao ra voi thong so la vi tri va kich thuoc cua Flying Dino
        public override Rectangle CollisionBox
        {
            get
            {
                //Tao ra hinh chu nhat co goc trai tren la vi tri cua Flying Dino va co kich thuoc la chieu rong + cao cua sprite
                Rectangle collisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), SPRITE_WIDTH, SPRITE_HEIGHT);
                //Phuong thuc 'Inflate' dieu chinh kich thuoc cua hinh chu nhat. No giam chieu rong va chieu cao cua hop va cham theo cac gia tri tuong ung
                // => dieu chinh chinh xac hon ve vung can xet va cham
                collisionBox.Inflate(-HORIZONTAL_COLLISION_INSET, -VERTICAL_COLLISION_INSET);
                return collisionBox;
            }
        }

        public FlyingDino(Trex trex, Vector2 position, Texture2D spriteSheet) : base(trex, position)
        {
            //Khoi tao 2 sprite cho hoat anh
            Sprite spriteA = new Sprite(spriteSheet, TEXTURE_COORDS_X, TEXTURE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
            Sprite spriteB = new Sprite(spriteSheet, TEXTURE_COORDS_X + SPRITE_WIDTH, TEXTURE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);

            //Gan tham chieu Trex
            _trex = trex;

            //Khoi tao SpriteAnimation
            _animation = new SpriteAnimation();
            _animation.AddFrame(spriteA, 0);
            _animation.AddFrame(spriteB, ANIMATION_FRAME_LENGTH);
            _animation.AddFrame(spriteA, ANIMATION_FRAME_LENGTH * 2);
            _animation.ShouldLoop = true;
            _animation.Play();

        }

        //Ve Flying Dino bang cach goi phuong thuc Draw của SpriteAnimation
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animation.Draw(spriteBatch, Position);
        }

        public override void Update(GameTime gameTime)
        {
            //Cap nhat tinh trang song cua Trex
            base.Update(gameTime);

            if (_trex.IsAlive)
            {
                _animation.Update(gameTime);
                //Di chuyen Flying Dino qua phai theo huong nguoc lai cua toc do cua no
                Position = new Vector2(Position.X - SPEED_PPS * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }
        }

    }
}