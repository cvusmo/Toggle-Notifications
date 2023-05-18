using SpaceWarp.API.Assets;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class AssetsLoader
    {
        public static Texture2D LoadIcon(string path)
        {
            Texture2D imageTexture = AssetManager.GetAsset<Texture2D>($"assets/images/{path}.png");

            // Check if the texture is null
            if (imageTexture == null)
            {
                Debug.LogError("Failed to load image texture from path: " + path);
                Debug.Log("Full resource path: " + Application.dataPath + "/" + path);
                Debug.Log("Expected resource type: Texture2D");
            }

            return imageTexture;
        }
    }
}
