using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrexRunner.Entities
{
    //LOP TRUU TUONG, DAI DIEN CHO CAC DOI TUONG XUAT HIEN TREN TROI
    public abstract class SkyObject : IGameEntity
    {
        //tuong tac voi doi tuong trex trong lop con
        protected readonly Trex _trex;

        //quy dinh thu tu ve cua doi tuong: thu tu thap => ve truoc
        public int DrawOrder { get; set; }
        
        //thuoc tinh truu tuong dai dien cho toc do. Cac lop con phai trien khai
        public abstract float Speed { get; }

        public Vector2 Position { get; set; }

        //khoi tao: Nhan 1 doi tuong Trex va mot vi tri => luu vao cac bien tuong ung
        protected SkyObject(Trex trex, Vector2 position)
        {
            _trex = trex;
            Position = position;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        //Cap nhat vi tri doi tuong dua vao toc do và ElapsedTime.
        //Neu Trex con song => vi tri duoc cap nhat
        //Phuong thuc nay co the duoc ghi de boi cac lop con
        public virtual void Update(GameTime gameTime)
        {
            if (_trex.IsAlive)
                Position = new Vector2(Position.X - Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);

        }

    }
}