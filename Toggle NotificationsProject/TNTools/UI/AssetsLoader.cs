using SpaceWarp.API.Assets;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    internal class AssetsLoader
    {
        internal static Texture2D LoadIcon(string path)
        {
            Texture2D imageTexture = AssetManager.GetAsset<Texture2D>("togglenotifications/images/" + path + ".png");

            if (imageTexture == null)
            {
                Debug.LogError("Failed to load image texture from path: " + path);
                Debug.Log("Full resource path: " + Application.dataPath + "/" + path);
                Debug.Log("Expected resource type: Texture2D");
                Debug.Log("Full path: " + Application.dataPath + "/togglenotifications/images/" + path + ".png");
            }
            return imageTexture;
        }
    }
}