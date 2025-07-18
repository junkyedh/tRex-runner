﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    //VE MOON
    public class Moon : SkyObject
    {

        private const int RIGHTMOST_SPRITE_COORDS_X = 624;
        private const int RIGHTMOST_SPRITE_COORDS_Y = 2;

        private const int SPRITE_WIDTH = 20;
        private const int SPRITE_HEIGHT = 40;

        private const int SPRITE_COUNT = 7;
        
        //tham chieu den interface 'IDayNightCycle'
        private readonly IDayNightCycle _dayNightCycle;
        private Sprite _sprite;

        //override tu lop cha 'SkyObject': toc do di chuyen cua mat trang dua tren toc do cua trex
        public override float Speed => _trex.Speed * 0.1f;

        //Khoi tao
        public Moon(IDayNightCycle dayNightCycle, Texture2D spriteSheet, Trex trex, Vector2 position) : base(trex, position)
        {
            _dayNightCycle = dayNightCycle;
            _sprite = new Sprite(spriteSheet, RIGHTMOST_SPRITE_COORDS_X, RIGHTMOST_SPRITE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }

        //Override tu lop cha SkyObject
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            UpdateSprite();

            //kiem tra neu la dem thi ve trang
            if (_dayNightCycle.IsNight)
                _sprite.Draw(spriteBatch, Position);
        }

        private void UpdateSprite()
        {
            int spriteIndex = _dayNightCycle.NightCount % SPRITE_COUNT;

            int spriteWidth = SPRITE_WIDTH;
            int spriteHeight = SPRITE_HEIGHT;

            if (spriteIndex == 3)
                spriteWidth *= 2;

            if (spriteIndex >= 3)
                spriteIndex++;

            _sprite.Height = spriteHeight;
            _sprite.Width = spriteWidth;

            _sprite.X = RIGHTMOST_SPRITE_COORDS_X - spriteIndex * SPRITE_WIDTH;
            _sprite.Y = RIGHTMOST_SPRITE_COORDS_Y;

        }

    }
}