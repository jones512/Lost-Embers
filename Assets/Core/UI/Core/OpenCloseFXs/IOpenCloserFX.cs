namespace AdventureKit.UI.Core.OpenCloseFX
{
    public interface IOpenCloserFX
    {
        void DoOpenFX(System.Action onOpenFXFinished);
        void DoCloseFX(System.Action onCloseFXFinished);
    }
}
