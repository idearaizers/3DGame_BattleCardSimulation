using VContainer;

namespace Siasm
{
    public sealed class TitleUseCase : BaseUseCase
    {
        [Inject]
        public TitleUseCase(MemoryDatabase memoryDatabase)
            : base(memoryDatabase)
        {
            // 
        }
    }
}
