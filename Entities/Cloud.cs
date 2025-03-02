using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    //DAI DIEN CHO DOI TUONG MAY 
    public class Cloud : SkyObject
    {
        private const int TEXTURE_COORDS_X = 87;
        private const int TEXTURE_COORDS_Y = 0;

        private const int SPRITE_WIDTH = 46;
        private const int SPRITE_HEIGHT = 17;


        private Sprite _sprite;

        //override thuoc tinh Speed tu lop co so SkyObject: Toc do cua may phụ thuoc vao
        // toc do cua doi luong Trex, nhung duoc giam xuong 0.5 lan
        public override float Speed => _trex.Speed * 0.5f;

        //khoi tao
        public Cloud(Texture2D spriteSheet, Trex trex, Vector2 position) : base(trex, position)
        {
            _sprite = new Sprite(spriteSheet, TEXTURE_COORDS_X, TEXTURE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }

        //override tu lop co so
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, Position);
        }
    }
}