namespace AdventureKit.UI.TransitionScreens
{
    public class CinematicAnimatedTextScreen : CinematicScreen
    {        
        public override void Skip()
        {
            base.Skip();
            m_animator.Play("BS_ShowContentState", 0, 1f);
        }
    }
}

