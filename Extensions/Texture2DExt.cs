using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrexRunner.Extensions
{
    public static class Texture2DExt
    {
        //chuyen doi mau sac cua texture2d bang cach dao nguoc mau cua tung pixel
        public static Texture2D InvertColors(this Texture2D texture, Color? excludeColor = null)
        {
            if (texture is null)
                throw new ArgumentNullException(nameof(texture));

            Texture2D result = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);

            //chua du lieu pixel cua texture
            Color[] pixelData = new Color[texture.Width * texture.Height];

            texture.GetData(pixelData);


            Color[] invertedPixelData = pixelData.Select(p => excludeColor.HasValue && p == excludeColor ? p : new Color(255 - p.R, 255 - p.G, 255 - p.B, p.A)).ToArray();

            result.SetData(invertedPixelData);

            return result;

        }

    }
}