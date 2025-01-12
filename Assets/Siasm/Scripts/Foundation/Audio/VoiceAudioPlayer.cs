namespace Siasm
{
    public sealed class VoiceAudioPlayer : BaseAudioPlayer
    {
        protected override int AudioSourceNumber => 1;
        protected override bool IsLoop => false;
    }
}
