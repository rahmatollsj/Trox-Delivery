﻿
namespace Harmony
{
    public static partial class Finder
    {
        private static SceneBundleLoader findableSceneBundleLoader = null;

        public static SceneBundleLoader SceneBundleLoader
        {
            get
            {
                if (!findableSceneBundleLoader)
                {
                    findableSceneBundleLoader = FindWithTag<SceneBundleLoader>(Tags.MainController);
                }

                return findableSceneBundleLoader;
            }
        }
    }
}