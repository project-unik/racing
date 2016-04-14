using UnityEditor;

public class BuildScript
{

    public static void PerformBuild()
    {
        string[] scenes = {"Assets/Scenes/Menu.unity"};
        BuildPipeline.BuildPlayer(scenes, "Build/server.out", BuildTarget.StandaloneLinux, BuildOptions.EnableHeadlessMode);
    }
}