using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace CommonAPI {
    /// <summary>
    /// Utility methods for dealing with textures.
    /// </summary>
    public static class TextureUtility {
        /// <summary>
        /// Loads a sprite from a PNG/JPG image.
        /// </summary>
        public static Sprite LoadSprite(string path) {
            var texture = new Texture2D(1, 1);
            var imageData = File.ReadAllBytes(path);
            texture.LoadImage(imageData);
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        }
    }
}
