using System;
using Ashfall.Bridge;

namespace UnityEngine.SceneManagement
{
    public struct Scene
    {
        public string name => "Main";
        public bool isLoaded => true;
        public int buildIndex => 0;
    }

    public enum LoadSceneMode { Single, Additive }

    public static class SceneManager
    {
        // Correctly inert: Godot owns the scene tree, and from Unity code's point of view there is
        // exactly one always-loaded scene.
        public static Scene GetActiveScene() => new Scene();

        // Returning quietly would tell the caller a scene transition happened. Godot scene changes
        // go through GetTree().ChangeSceneToFile; route to that rather than reviving these.
        public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) =>
            BridgeGap.Semantic("SceneManager.LoadScene(string)", "No scene transition would occur, but the caller would continue as though it had.");
        public static void LoadScene(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single) =>
            BridgeGap.Semantic("SceneManager.LoadScene(int)", "No scene transition would occur, but the caller would continue as though it had.");
    }
}
