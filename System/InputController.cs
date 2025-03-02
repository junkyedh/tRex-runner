using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TrexRunner.Entities;

namespace TrexRunner.System
{
    //KIEM SOAT INPUT DE DIEU KHIEN HANH DONG CUA NHAN VAT
    public class InputController
    {
        
        private bool _isBlocked;
        private Trex _trex;

        private KeyboardState _previousKeyboardState;

        public InputController(Trex trex)
        {
            _trex = trex;
        }

        public void ProcessControls(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();

            //kiem tra xem input co bi chan khong
            if (!_isBlocked)
            {
                bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);
                //phim nhay co duoc nhan xuong ? va truoc do co dang duoc giu hay khong
                if (!wasJumpKeyPressed && isJumpKeyPressed)
                {

                    if (_trex.State != TrexState.Jumping) //khong o trang thai nhay
                        _trex.BeginJump(); //bat dau hanh dong nhay

                }
                //neu dang o trang thay nhay va phim nhay khong con duoc giu
                else if (_trex.State == TrexState.Jumping && !isJumpKeyPressed)
                {
                    //huy bo hanh dong nhay
                    _trex.CancelJump();

                }
                //Neu phim xuong duoc nhan
                else if (keyboardState.IsKeyDown(Keys.Down))
                {

                    // kiem tra trang thai
                    if (_trex.State == TrexState.Jumping || _trex.State == TrexState.Falling)
                        _trex.Drop(); //thuc hien roi
                    else
                        _trex.Duck(); //thuc hien nhay goi

                }
                //neu dang trang thai ngoi & phim xuong khong duoc giu
                else if (_trex.State == TrexState.Ducking && !keyboardState.IsKeyDown(Keys.Down))
                {
                    //dung day
                    _trex.GetUp();

                }

            }

            _previousKeyboardState = keyboardState;

            _isBlocked = false;

        }

        // Chan dau vao tu ban phim
        public void BlockInputTemporarily()
        {
            _isBlocked = true;
        }

    }
}