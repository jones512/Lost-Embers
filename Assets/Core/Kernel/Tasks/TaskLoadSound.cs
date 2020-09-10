using UnityEngine;
using System.Collections;

using AdventureKit.Utils;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskLoadSound : Task.Task
    {
        [SerializeField]
        private AudioClip m_SoundClip;

        [SerializeField]
        private bool m_Loop = true;

        protected override void DoStart()
        {
            SoundManager SoundManager = SoundManager.Instance;

            SoundManager.PlayMusic(m_SoundClip, m_Loop);
            //if (SoundManager.SoundEnabled)
            //{
            //    //if (!SoundManager.AudioSourceMuted && !SoundManager.IsCurrentMusicChanelPlaying())
            //        SoundManager.PlayMusic(m_SoundClip, m_Loop);
            //}
                
            Complete("TaskLoadSound::Complete");
        }
    }
}
