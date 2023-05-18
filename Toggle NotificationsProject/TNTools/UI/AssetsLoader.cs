using UnityEngine;
using SpaceWarp.API.Assets;

namespace ToggleNotifications.TNTools.UI;

public class AssetsLoader
{
    public static Texture2D LoadIcon(string path)
    {
        Texture2D imageTexture = AssetManager.GetAsset<Texture2D>($"togglenotifications/images/{path}.png");

        //   Check if the texture is null
        if (imageTexture == null)
        {
            // Print an Error message to the Console
            Debug.LogError("Failed to load image texture from path: " + path);

            // Print the full path of the resource
            Debug.Log("Full resource path: " + Application.dataPath + "/" + path);

            // Print the type of resource that was expected
            Debug.Log("Expected resource type: Texture2D");
        }

        return imageTexture;
    }
}