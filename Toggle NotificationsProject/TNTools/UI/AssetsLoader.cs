using UnityEngine;
using SpaceWarp.API.Assets;

namespace ToggleNotifications.TNTools.UI;

public class AssetsLoader
{
    public static Texture2D LoadIcon(string path)
    {
        Texture2D imageTexture = AssetManager.GetAsset<Texture2D>("ToggleNotifications/assets/images/" + path + ".png");

        if (imageTexture == null)
        {
            Debug.LogError("Failed to load image texture from path: " + path);
            Debug.Log("Full resource path: " + Application.dataPath + "/" + path);
            Debug.Log("Expected resource type: Texture2D");
        }

        return imageTexture;
    }
}
