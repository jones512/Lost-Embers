using AdventureKit.Kernel.Context;

namespace AdventureKit.Kernel
{
    public interface iSceneBootstrap
    {

        void Init(BaseContext context);


        void InitFromKernel();
    }

}
