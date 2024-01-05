using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

namespace CommonAPI {
    /// <summary>
    /// Utility methods for dealing with textures.
    /// </summary>
    public static class TextureUtility {
        /// <summary>
        /// Quickly creates a Sprite from a Texture2D.
        /// </summary>
        public static Sprite CreateSpriteFromTexture(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        }

        /// <summary>
        /// Loads a sprite from a PNG/JPG image.
        /// </summary>
        public static Sprite LoadSprite(string path) {
            var texture = new Texture2D(1, 1);
            var imageData = File.ReadAllBytes(path);
            texture.LoadImage(imageData);
            return CreateSpriteFromTexture(texture);
        }
    }
}
