using System;
using System.Collections.Generic;

using AdventureKit.ScreensManagement.BaseClaseAndUtils;

namespace AdventureKit.UI.Core.Screens
{
    public class ScreenManagerInitializedEventArgs : EventArgs
    {
        private List<ScreenBase> mListScreens;

        public ScreenManagerInitializedEventArgs(List<ScreenBase> listScreens)
        {
            mListScreens = listScreens;
        }

        public List<ScreenBase> listScreens 
        {
            get 
                {
                return mListScreens;
            }
        }
    }
}
