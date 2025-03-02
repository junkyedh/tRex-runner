using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    //DAI DIEN CHO CAC NHOM CACTUS, KE THUA TU CLASS OBSTACLE
    public class CactusGroup : Obstacle
    {
        //dinh nghi kich thuoc
        public enum GroupSize
        {
            Small = 0,
            Medium = 1,
            Large = 2
        }

        private const int SMALL_CACTUS_SPRITE_HEIGHT = 36;
        private const int SMALL_CACTUS_SPRITE_WIDTH = 17;

        private const int SMALL_CACTUS_TEXTURE_POS_X = 228;
        private const int SMALL_CACTUS_TEXTURE_POS_Y = 0;

        private const int LARGE_CACTUS_TEXTURE_POS_X = 332;
        private const int LARGE_CACTUS_TEXTURE_POS_Y = 0;

        private const int LARGE_CACTUS_SPRITE_HEIGHT = 51;
        private const int LARGE_CACTUS_SPRITE_WIDTH = 25;
        
        //khoang cach va cham
        private const int COLLISION_BOX_INSET = 5;

        //Doc va tra ve 
        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Sprite.Width, Sprite.Height);
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return box;
            }
        }

        //xac dinh nhom xr lon ?
        public bool IsLarge { get; }

        //size cua nhom xr
        public GroupSize Size { get; }

        //dai dien cho hinh anh cua nhom xr
        public Sprite Sprite { get; private set; }

        //khoi tao
        public CactusGroup(Texture2D spriteSheet, bool isLarge, GroupSize size, Trex trex, Vector2 position) : base(trex, position)
        {
            IsLarge = isLarge;
            Size = size;
            Sprite = GenerateSprite(spriteSheet);
        }
        
        //khoi tao Sprite dua tren thong so cu the
        private Sprite GenerateSprite(Texture2D spriteSheet)
        {

            Sprite sprite = null;

            int spriteWidth = 0;
            int spriteHeight = 0;
            int posX = 0;
            int posY = 0;

            if (!IsLarge) // Create small cactus group
            {
                spriteWidth = SMALL_CACTUS_SPRITE_WIDTH;
                spriteHeight = SMALL_CACTUS_SPRITE_HEIGHT;

                posX = SMALL_CACTUS_TEXTURE_POS_X;
                posY = SMALL_CACTUS_TEXTURE_POS_Y;
            }
            else // Create large cactus group
            {
                spriteWidth = LARGE_CACTUS_SPRITE_WIDTH;
                spriteHeight = LARGE_CACTUS_SPRITE_HEIGHT;

                posX = LARGE_CACTUS_TEXTURE_POS_X;
                posY = LARGE_CACTUS_TEXTURE_POS_Y;
            }

            int offsetX = 0;
            int width = spriteWidth;

            //dua vao gia tri cua Size => vi tri offset va chieu rong cua sprite
            if (Size == GroupSize.Small)
            {
                offsetX = 0;
                width = spriteWidth;
            }
            else if (Size == GroupSize.Medium)
            {
                offsetX = 1;
                width = spriteWidth * 2;
            }
            else
            {
                offsetX = 3;
                width = spriteWidth * 3;
            }

            //tao doi tuong moi, dua vao cac thong so da xac dinh
            sprite = new Sprite
            (
                spriteSheet,
                posX + offsetX * spriteWidth,
                posY,
                width,
                spriteHeight
            );

            return sprite;

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, Position);
        }

    }
}