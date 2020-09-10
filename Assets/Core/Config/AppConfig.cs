namespace AdventureKit.Config
{
    public class AppConfig
    {
        public const string APP_NAME = "2 Weeks Challenge";

        public const float VERSION = 010f;
        public const string APP_VERSION = "0.1.0";

        public static float LatestVersion { get; set; }
        public static bool IsNewVersionRequired
        {
            get { return AppConfig.VERSION < AppConfig.LatestVersion; }
        }


        public const float CHECK_COMPLETED_TASK_INTERVAL = 0.016f;


        public const string MAIN_SCENE = "MainScene";
        public const string MAIN_MENU_SCENE = "MainMenuScene";
        public const string WORLD_SELECTION_SCENE = "WorldSelectionScene";
        public const string WORLD_HUD_SCENE = "WorldHudScene";
        public const string FIRST_LEVEL_SCENE = "Level_1";


        const string KEY_IS_MUSIC_ENABLED = "IsMusicEnabled";

        const string KEY_IS_SOUND_ENABLED = "IsSoundEnabled";

        public const float MUSIC_FADE_DURATION = 1.0f;

        public const float MUSIC_VOLUME = 0.2f;

        public const float FX_VOLUME = 0.3f;
    }

}
