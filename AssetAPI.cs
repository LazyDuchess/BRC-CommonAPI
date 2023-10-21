using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace CommonAPI
{
    /// <summary>
    /// Provides methods to more easily load assets from the game's asset bundles.
    /// </summary>
    public static class AssetAPI
    {
        private static Dictionary<ShaderNames, Shader> CachedShaders = new Dictionary<ShaderNames, Shader>();
        public enum ShaderNames
        {
            AmbientCharacter,
            AmbientEnvironment,
            AmbientEnvironmentCutout,
            AmbientEnvironmentTransparent
        }

        /// <summary>
        /// Returns a shader from the game.
        /// </summary>
        public static Shader GetShader(ShaderNames shaderName)
        {
            if (CachedShaders.TryGetValue(shaderName, out Shader result))
            {
                if (result != null)
                    return result;
            }
            var assets = Core.Instance.Assets;
            switch(shaderName)
            {
                case ShaderNames.AmbientCharacter:
                    var shellMat = assets.LoadAssetFromBundle<Material>("common_assets", "shell");
                    var shellShader = shellMat.shader;
                    CacheShader(shaderName, shellShader);
                    return shellShader;

                case ShaderNames.AmbientEnvironment:
                    var poleMat = assets.LoadAssetFromBundle<Material>("common_assets", "SkateboardScrewPoleMat");
                    var poleShader = poleMat.shader;
                    CacheShader(shaderName, poleShader);
                    return poleShader;

                case ShaderNames.AmbientEnvironmentCutout:
                    var preludeMat = assets.LoadAssetFromBundle<Material>("common_assets", "Prelude_PropsAtlasMat");
                    var preludeShader = preludeMat.shader;
                    CacheShader(shaderName, preludeShader);
                    return preludeShader;

                case ShaderNames.AmbientEnvironmentTransparent:
                    var discMat = assets.LoadAssetFromBundle<Material>("common_assets", "MusicCollectableMiniDiscTransperantMat");
                    var discShader = discMat.shader;
                    CacheShader(shaderName, discShader);
                    return discShader;
            }
            throw new ArgumentOutOfRangeException("shaderName", "Shader name is out of range!");
        }
        private static void CacheShader(ShaderNames shaderName, Shader shader)
        {
            CachedShaders[shaderName] = shader;
        }
    }
}
