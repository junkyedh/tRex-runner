using Microsoft.Xna.Framework;

namespace TrexRunner.Entities
{
    //Class nao muon tham gia vao chu ky ngay dem trong tro choi can 
    // trien khai de thuc hien cac chuc nang duoc dinh nghia trong interface
    public interface IDayNightCycle
    {
        int NightCount { get; }
        bool IsNight { get; }

        Color ClearColor { get; }

    }
}