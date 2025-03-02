using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace TrexRunner.Graphics
{
    //QUAN LY VA CHOI CAC DOAN HOAT ANH CUA SPRITE
    public class SpriteAnimation
    {
        //danh sach cac SpriteAnimationFrame: dai dien cho cac frame trong hoat anh
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();

        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index);

            }

        }

        //so luong frame trong hoat anh
        public int FrameCount => _frames.Count;

        //frame hien tai cua hoat anh dua tren thoi diem cua trinh phat
        public SpriteAnimationFrame CurrentFrame
        {

            get
            {
                return _frames
                    .Where(f => f.TimeStamp <= PlaybackProgress) 
                    .OrderBy(f => f.TimeStamp)
                    .LastOrDefault(); 

            }

        }

        //Tong thoi gian cua hoat anh: lay thoi diem cuoi cung cua frame
        public float Duration
        {

            get
            {

                if (!_frames.Any())
                    return 0;

                return _frames.Max(f => f.TimeStamp);

            }

        }

        //Trang thai hien tai cua trinh phat
        public bool IsPlaying { get; private set; }
        //tien trinh phat cua hoat anh, do bang time
        public float PlaybackProgress { get; private set; }
        //xac dinh xem hoat anh co nen lap lai hay khong
        public bool ShouldLoop { get; set; } = true;

        //Them mot frame moi voi mot sprite và thoi diem cu the trong danh sach frame
        public void AddFrame(Sprite sprite, float timeStamp)
        {

            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);

            _frames.Add(frame);

        }
        //cap nhat trang thai cua hoat anh dua tren time troi qua
        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {

                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //kiem tra da het hoat anh chua
                //da het
                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                        PlaybackProgress -= Duration; //quay lai ban dau và tiep tuc phat
                    else
                        Stop();
                }

            }

        }
        //ve frame hien tai len man hinh
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {

            SpriteAnimationFrame frame = CurrentFrame;

            if (frame != null)
                frame.Sprite.Draw(spriteBatch, position);

        }
        //bat dau phat hoat anh
        public void Play()
        {

            IsPlaying = true;

        }
        // dung phat anh va dat lai thoi gian phat = 0 
        public void Stop()
        {

            IsPlaying = false;
            PlaybackProgress = 0;

        }
        //lay frame tai 1 vi tri cu the trong danh sach frames
        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "A frame with index " + index + " does not exist in this animation.");

            return _frames[index];

        }
        //dung phat va xoa all frames
        public void Clear()
        {

            Stop();
            _frames.Clear();

        }
        //Tao mot hoat anh tu mot texture voi cac tham so
        public static SpriteAnimation CreateSimpleAnimation(Texture2D texture, Point startPos, int width, int height, Point offset, int frameCount, float frameLength)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            SpriteAnimation anim = new SpriteAnimation();

            for (int i = 0; i < frameCount; i++)
            {
                //thoi diem bat dau cua moi frame = framelength * so thu tu cua frame trong day
                Sprite sprite = new Sprite(texture, startPos.X + i * offset.X, startPos.Y + i * offset.Y, width, height);
                anim.AddFrame(sprite, frameLength * i);
                
                //neu la frame cuoi cung: them 1 frame nua voi thoi diem bat dau la .... de dam bao toan bo thoi luong cua hoat anh duoc bao gom
                if (i == frameCount - 1)
                    anim.AddFrame(sprite, frameLength * (i + 1));

            }

            return anim;

        }

    }
}