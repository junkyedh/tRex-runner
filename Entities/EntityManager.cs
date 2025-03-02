using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace TrexRunner.Entities
{
    //QUAN LY VA DIEU KHIEN CAC DOI TUONG
    public class EntityManager
    {
        //danh sach chua tat ca cac thuc the trong tro choi
        private readonly List<IGameEntity> _entities = new List<IGameEntity>();

        //danh sach chua cac thục the can them vao danh sach chinh
        private readonly List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
        //danh sach chua cac thuc the can loai khoi khoi danh sach chinh
        private readonly List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();

        //Thuoc tinh chi doc cung cap mot ban sao chi doc cua danh sach '_entities'
        public IEnumerable<IGameEntity> Entities => new ReadOnlyCollection<IGameEntity>(_entities);

        //Cap nhat cac doi tuong trong danh sach '_entities': 
            // Cac doi tuong moi duoc them vao, doi tuong can loai bo se duoc xu ly
        public void Update(GameTime gameTime)
        {

            foreach (IGameEntity entity in _entities)
            {

                if (_entitiesToRemove.Contains(entity))
                    continue;

                entity.Update(gameTime);

            }

            foreach (IGameEntity entity in _entitiesToAdd)
            {
                _entities.Add(entity);
            }

            foreach (IGameEntity entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }

            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();

        }
        //Ve doi tuong theo thu tu duoc xac dinh boi DrawOrder
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (IGameEntity entity in _entities.OrderBy(e => e.DrawOrder))
            {

                entity.Draw(spriteBatch, gameTime);

            }

        }
        // Them doi tuong moi vao danh sach '_entitiesToAdd'
        public void AddEntity(IGameEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Null cannot be added as an entity.");

            _entitiesToAdd.Add(entity);

        }
        //Danh dau doi tuong de loai bo khoi danh sach '_entitiesToRemove'
        public void RemoveEntity(IGameEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Null is not a valid entity.");

            _entitiesToRemove.Add(entity);

        }

        //Danh dau tat ca doi tuong tron '_entities' de loai bo
        public void Clear()
        {

            _entitiesToRemove.AddRange(_entities);

        }

        //Tra ve mot IEnumerable chua cac doi tuong kieu T tu dsach '_entities'
        public IEnumerable<T> GetEntitiesOfType<T>() where T : IGameEntity
        {
            return _entities.OfType<T>();
        }

    }
}