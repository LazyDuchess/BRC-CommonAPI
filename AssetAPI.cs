using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace CommonAPI {
    /// <summary>
    /// Provides methods to more easily load assets from the game's asset bundles.
    /// </summary>
    public static class AssetAPI {
        private static readonly Dictionary<ShaderNames, Shader> CachedShaders = new();
        private static readonly Dictionary<MaterialNames, Material> CachedMaterials = new();
        public enum ShaderNames {
            AmbientCharacter,
            AmbientEnvironment,
            AmbientEnvironmentCutout,
            AmbientEnvironmentTransparent,
            AmbientEnvironmentGlass
        }

        public enum MaterialNames {
            ToonWaterPyramid,
            OasisWater
        }

        /// <summary>
        /// Returns the graffiti art from the game.
        /// </summary>
        public static GraffitiArtInfo GetGraffitiArtInfo() {
            var assets = Core.Instance.Assets;
            var grafArtInfo = assets.LoadAssetFromBundle<GraffitiArtInfo>("graffiti", "GraffitiArtInfo");
            return grafArtInfo;
        }

        public static Material GetMaterial(MaterialNames materialName) {
            if (CachedMaterials.TryGetValue(materialName, out var result)) {
                if (result != null) return result;
            }
            var assets = Core.Instance.Assets;
            switch (materialName) {
                case MaterialNames.ToonWaterPyramid:
                    var pyramidMat = assets.LoadAssetFromBundle<Material>("common_assets", "ToonWater_Pyramid");
                    CacheMaterial(materialName, pyramidMat);
                    return pyramidMat;

                case MaterialNames.OasisWater:
                    var oasisMat = assets.LoadAssetFromBundle<Material>("common_assets", "OasisWater");
                    CacheMaterial(materialName, oasisMat);
                    return oasisMat;
            }
            throw new ArgumentOutOfRangeException("materialName", "Material name is out of range!");
        }

        /// <summary>
        /// Returns a shader from the game.
        /// </summary>
        public static Shader GetShader(ShaderNames shaderName) {
            if (CachedShaders.TryGetValue(shaderName, out var result)) {
                if (result != null)
                    return result;
            }
            var assets = Core.Instance.Assets;
            switch (shaderName) {
                case ShaderNames.AmbientEnvironmentGlass:
                    var glassMat = assets.LoadAssetFromBundle<Material>("common_assets", "glass");
                    var glassShader = glassMat.shader;
                    CacheShader(shaderName, glassShader);
                    return glassShader;

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
        private static void CacheShader(ShaderNames shaderName, Shader shader) {
            CachedShaders[shaderName] = shader;
        }

        private static void CacheMaterial(MaterialNames materialName, Material material) {
            CachedMaterials[materialName] = material;
        }
    }
}
