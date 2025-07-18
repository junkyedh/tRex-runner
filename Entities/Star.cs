﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    //DAI DIEN CHO DOI TUONG SAO
    public class Star : SkyObject
    {

        private const int INITIAL_FRAME_TEXTURE_COORDS_X = 644;
        private const int INITIAL_FRAME_TEXTURE_COORDS_Y = 2;

        private const int SPRITE_WIDTH = 9;
        private const int SPRITE_HEIGHT = 9;

        private const float ANIMATION_FRAME_LENGTH = 0.4f;

        private SpriteAnimation _animation;
        private IDayNightCycle _dayNightCycle;

        // toc do di chuyen cua sao la mot phan nho cua toc do cua trex
        public override float Speed => _trex.Speed * 0.2f;

        //Khoi tao
        public Star(IDayNightCycle dayNightCycle, Texture2D spriteSheet, Trex trex, Vector2 position) : base(trex, position)
        {
            _dayNightCycle = dayNightCycle;

            _animation = SpriteAnimation.CreateSimpleAnimation(
                spriteSheet,
                new Point(INITIAL_FRAME_TEXTURE_COORDS_X, INITIAL_FRAME_TEXTURE_COORDS_Y),
                SPRITE_WIDTH,
                SPRITE_HEIGHT,
                new Point(0, SPRITE_HEIGHT),
                3,
                ANIMATION_FRAME_LENGTH
            );

            _animation.ShouldLoop = true;
            _animation.Play();

        }

        //Cap nhat animation neu nhan vat chinh con song
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_trex.IsAlive)
                _animation.Update(gameTime);
        }

        //Chi ve sao khi dang dem
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_dayNightCycle.IsNight)
                _animation.Draw(spriteBatch, Position);
        }
    }
}