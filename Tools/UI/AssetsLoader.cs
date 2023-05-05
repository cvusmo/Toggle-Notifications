using UnityEngine;
using SpaceWarp.API.Assets;

namespace ToggleNotifications.UI
{
    public class AssetsLoader
    {
        // BEPEXVersion
        public static Texture2D loadIcon(string path)
        {
            // hardcoded per mod
            string full_path = $"ToggleNotifications/assets/images/icon.png";
            var imageTexture = AssetManager.GetAsset<Texture2D>(full_path);

            // Check if the texture is null
            if (imageTexture == null)
            {
                // Print an error message to the Console
                Debug.LogError("Failed to load image texture from path: " + full_path);

                // Print the full path of the resource
                Debug.Log("Full resource path: " + Application.dataPath + "/" + full_path);

                // Print the type of resource that was expected
                Debug.Log("Expected resource type: Texture2D");
            }

            return imageTexture;
        }
    }
}
