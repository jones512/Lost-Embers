using UnityEngine;
using System.Collections;

namespace AdventureKit.Kernel.Loading
{

    public interface iLoadingEventsHandler
    {

        void OnLoadingCloseAnimFinished();
        void OnLoadingOpenAnimFinished();

        float GetProgress();

    }

}