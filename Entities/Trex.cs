using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public class Trex : IGameEntity, ICollidable
    {
        //Dinh nghia cac thong so cua hang va thuoc tinh

        private const float RUN_ANIMATION_FRAME_LENGTH = 0.1f;

        private const float MIN_JUMP_HEIGHT = 40f;

        private const float GRAVITY = 1600f;
        private const float JUMP_START_VELOCITY = -480f;

        private const float CANCEL_JUMP_VELOCITY = -100f;

        private const int TREX_IDLE_BACKGROUND_SPRITE_POS_X = 40;
        private const int TREX_IDLE_BACKGROUND_SPRITE_POS_Y = 0;

        public const int TREX_DEFAULT_SPRITE_POS_X = 848;
        public const int TREX_DEFAULT_SPRITE_POS_Y = 0;
        public const int TREX_DEFAULT_SPRITE_WIDTH = 44;
        public const int TREX_DEFAULT_SPRITE_HEIGHT = 52;

        private const float BLINK_ANIMATION_RANDOM_MIN = 2f;
        private const float BLINK_ANIMATION_RANDOM_MAX = 10f;
        private const float BLINK_ANIMATION_EYE_CLOSE_TIME = 0.5f;

        private const int TREX_RUNNING_SPRITE_ONE_POS_X = TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_WIDTH * 2;
        private const int TREX_RUNNING_SPRITE_ONE_POS_Y = 0;

        private const int TREX_DUCKING_SPRITE_WIDTH = 59;

        private const int TREX_DUCKING_SPRITE_ONE_POS_X = TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_WIDTH * 6;
        private const int TREX_DUCKING_SPRITE_ONE_POS_Y = 0;

        private const int TREX_DEAD_SPRITE_POS_X = 1068;
        private const int TREX_DEAD_SPRITE_POS_Y = 0;

        private const float DROP_VELOCITY = 600f;

        public const float START_SPEED = 280f;
        public const float MAX_SPEED = 900f;

        private const float ACCELERATION_PPS_PER_SECOND = 3f;

        private const int COLLISION_BOX_INSET = 3;
        private const int DUCK_COLLISION_REDUCTION = 20;

        private Sprite _idleBackgroundSprite;
        private Sprite _idleSprite;
        private Sprite _idleBlinkSprite;
        private Sprite _deadSprite;

        private SoundEffect _jumpSound;

        private SpriteAnimation _blinkAnimation;
        private SpriteAnimation _runAnimation;
        private SpriteAnimation _duckAnimation;

        private Random _random;

        private float _verticalVelocity;
        private float _startPosY;
        private float _dropVelocity;

        //Su kien duoc kich hoat khi nhay hoan tat
        public event EventHandler JumpComplete;
        //Su kien duoc kich hoat khi nhan vat chet
        public event EventHandler Died;

        //Trang thai hien tai cua nhan vat (dung yen, nhay, chay, ngoi)
        public TrexState State { get; private set; }

        public Vector2 Position { get; set; }

        public bool IsAlive { get; private set; }

        public float Speed { get; private set; }

        public int DrawOrder { get; set; }

        //Xac dinh khoang cach va cham
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle(
                    (int)Math.Round(Position.X),
                    (int)Math.Round(Position.Y),
                    TREX_DEFAULT_SPRITE_WIDTH,
                    TREX_DEFAULT_SPRITE_HEIGHT
                );
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);

                if (State == TrexState.Ducking)
                {
                    box.Y += DUCK_COLLISION_REDUCTION;
                    box.Height -= DUCK_COLLISION_REDUCTION;

                }

                return box;
            }
        }

        //Khoi tao doi tuong Trex
        public Trex(Texture2D spriteSheet, Vector2 position, SoundEffect jumpSound)
        {

            Position = position;
            _idleBackgroundSprite = new Sprite(spriteSheet, TREX_IDLE_BACKGROUND_SPRITE_POS_X, TREX_IDLE_BACKGROUND_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            State = TrexState.Idle;

            _jumpSound = jumpSound;

            _random = new Random();

            _idleSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);
            _idleBlinkSprite = new Sprite(spriteSheet, TREX_DEFAULT_SPRITE_POS_X + TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);

            _blinkAnimation = new SpriteAnimation();
            CreateBlinkAnimation();

            _blinkAnimation.Play();

            _startPosY = position.Y;

            _runAnimation = new SpriteAnimation();
            _runAnimation.AddFrame(new Sprite(spriteSheet, TREX_RUNNING_SPRITE_ONE_POS_X, TREX_RUNNING_SPRITE_ONE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT), 0);
            _runAnimation.AddFrame(new Sprite(spriteSheet, TREX_RUNNING_SPRITE_ONE_POS_X + TREX_DEFAULT_SPRITE_WIDTH, TREX_RUNNING_SPRITE_ONE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT), RUN_ANIMATION_FRAME_LENGTH);
            _runAnimation.AddFrame(_runAnimation[0].Sprite, RUN_ANIMATION_FRAME_LENGTH * 2);
            _runAnimation.Play();

            _duckAnimation = new SpriteAnimation();
            _duckAnimation.AddFrame(new Sprite(spriteSheet, TREX_DUCKING_SPRITE_ONE_POS_X, TREX_DUCKING_SPRITE_ONE_POS_Y, TREX_DUCKING_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT), 0);
            _duckAnimation.AddFrame(new Sprite(spriteSheet, TREX_DUCKING_SPRITE_ONE_POS_X + TREX_DUCKING_SPRITE_WIDTH, TREX_DUCKING_SPRITE_ONE_POS_Y, TREX_DUCKING_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT), RUN_ANIMATION_FRAME_LENGTH);
            _duckAnimation.AddFrame(_duckAnimation[0].Sprite, RUN_ANIMATION_FRAME_LENGTH * 2);
            _duckAnimation.Play();

            _deadSprite = new Sprite(spriteSheet, TREX_DEAD_SPRITE_POS_X, TREX_DEAD_SPRITE_POS_Y, TREX_DEFAULT_SPRITE_WIDTH, TREX_DEFAULT_SPRITE_HEIGHT);

            IsAlive = true;
        }

        //Khoi tao trang thai ban dau cua nhan vat
        public void Initialize()
        {
            Speed = START_SPEED;
            //Speed = MAX_SPEED;
            State = TrexState.Running;
            IsAlive = true;
            Position = new Vector2(Position.X, _startPosY);

        }

        //Ve nhan vat dua vao trang thai hien tai
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (IsAlive)
            {
                // trang thai Idle (dung yen)
                if (State == TrexState.Idle)
                {

                    _idleBackgroundSprite.Draw(spriteBatch, Position);
                    _blinkAnimation.Draw(spriteBatch, Position);

                }
                else if (State == TrexState.Jumping || State == TrexState.Falling)
                {

                    _idleSprite.Draw(spriteBatch, Position);

                }
                else if (State == TrexState.Running)
                {

                    _runAnimation.Draw(spriteBatch, Position);

                }
                else if (State == TrexState.Ducking)
                {

                    _duckAnimation.Draw(spriteBatch, Position);

                }

            }
            else
            {
                _deadSprite.Draw(spriteBatch, Position);
            }

        }

        //Cap nhat trang thai va vi tri han vat dua vao thoi gian troi qua
        public void Update(GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {
                //trang thai Idle (dung yen)
                if (!_blinkAnimation.IsPlaying)
                {
                    //kiem tra neu khong nhap mat => tao va chay animation nhap mat
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();
                }

                _blinkAnimation.Update(gameTime);

            }
            //Neu dang o trang thai nhay hoac roi => cap nhat vi tri dua tren thoi gian troi qua, toc do doc, toc do roi
            else if (State == TrexState.Jumping || State == TrexState.Falling)
            {

                Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds + _dropVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                _verticalVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // neu toc do doc khong am => trang thai Falling
                if (_verticalVelocity >= 0)
                    State = TrexState.Falling;

                if (Position.Y >= _startPosY)
                {

                    Position = new Vector2(Position.X, _startPosY);
                    _verticalVelocity = 0;
                    State = TrexState.Running;

                    OnJumpComplete();

                }


            }
            //Neu nhan vat dang chay => cap nhat animation chay
            else if (State == TrexState.Running)
            {

                _runAnimation.Update(gameTime);

            }
            else if (State == TrexState.Ducking)
            {

                _duckAnimation.Update(gameTime);

            }

            //Neu nhan vat khong o trang thai dung yen => gia tang toc do dua tren gia toc va thoi gian troi qua
            if (State != TrexState.Idle)
                Speed += ACCELERATION_PPS_PER_SECOND * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Kiem tra neu toc do vuot qua gia han toi da => giu ơ gioi han toi da
            if (Speed > MAX_SPEED)
                Speed = MAX_SPEED;

            //dat toc do roi ve 0 => khong co toc do roi khi khong o trang thai nhay hoac roi
            _dropVelocity = 0;

        }
        // Tao animation cho viec nhay mat
        private void CreateBlinkAnimation()
        {
            _blinkAnimation.Clear();
            _blinkAnimation.ShouldLoop = false;

            double blinkTimeStamp = BLINK_ANIMATION_RANDOM_MIN + _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN);

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlinkSprite, (float)blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp + BLINK_ANIMATION_EYE_CLOSE_TIME);

        }
        //Thuc hien hanh dong nhay
        public bool BeginJump()
        {

            if (State == TrexState.Jumping || State == TrexState.Falling)
                return false;

            _jumpSound.Play();

            State = TrexState.Jumping;

            _verticalVelocity = JUMP_START_VELOCITY;

            return true;

        }
        // Huy nhay
        public bool CancelJump()
        {

            if (State != TrexState.Jumping || (_startPosY - Position.Y) < MIN_JUMP_HEIGHT)
                return false;

            _verticalVelocity = _verticalVelocity < CANCEL_JUMP_VELOCITY ? CANCEL_JUMP_VELOCITY : 0;

            return true;

        }

        //Ngoi
        public bool Duck()
        {
            if (State == TrexState.Jumping || State == TrexState.Falling)
                return false;

            State = TrexState.Ducking;

            return true;

        }

        // Dung day
        public bool GetUp()
        {
            if (State != TrexState.Ducking)
                return false;

            State = TrexState.Running;

            return true;

        }

        //Roi xuong
        public bool Drop()
        {
            if (State != TrexState.Falling && State != TrexState.Jumping)
                return false;

            State = TrexState.Falling;

            _dropVelocity = DROP_VELOCITY;

            return true;
        }

        //Kich hoat su kien JumpComplete
        protected virtual void OnJumpComplete()
        {
            EventHandler handler = JumpComplete;
            handler?.Invoke(this, EventArgs.Empty);

        }

        //Kich hoat su kien Đie
        protected virtual void OnDied()
        {
            EventHandler handler = Died;
            handler?.Invoke(this, EventArgs.Empty);
        }

        //Dat trang thai nhan vat la da chet va kich hoat su kien 'Died'
        public bool Die()
        {
            if (!IsAlive)
                return false;

            State = TrexState.Idle;
            Speed = 0;

            IsAlive = false;

            OnDied();

            return true;
        }

    }
}