using System;
using System.Collections.Generic;
using System.Text;

namespace TrexRunner.Graphics
{
    //DAI DIEN CHO 1 FRAME CU THE TRONG 1 HOAT ANH, CHUA: SPRITE VÀ TIMESTAMP
    public class SpriteAnimationFrame
    {
        private Sprite _sprite;

        public Sprite Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "The sprite cannot be null.");

                _sprite = value;

            }
        }
        // luu tru thoi diem bat dau cua frame
        public float TimeStamp { get; }

        public SpriteAnimationFrame(Sprite sprite, float timeStamp)
        {
            Sprite = sprite;
            TimeStamp = timeStamp;
        }

    }
}