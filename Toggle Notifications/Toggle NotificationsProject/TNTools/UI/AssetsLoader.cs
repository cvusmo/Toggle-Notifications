using SpaceWarp.API.Assets;
using UnityEngine;

namespace ToggleNotifications.UI

{
    public class AssetsLoader
    {
        public static Texture2D loadIcon(string path)
        {
            Texture2D asset = AssetManager.GetAsset<Texture2D>("flight_plan/images/" + path + ".png");
            if (!((System.Object)asset == (System.Object)null))
                return asset;
            Debug.LogError((object)("Failed to load image texture from path: " + path));
            Debug.Log((object)("Full resource path: " + Application.dataPath + "/" + path));
            Debug.Log((object)"Expected resource type: Texture2D");
            return asset;
        }
    }
}
