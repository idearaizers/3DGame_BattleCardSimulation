using System;

namespace Siasm
{
    [Serializable]
    public class GraphicSetting
    {
        public int ResolutionSelectedIndex = 5;     // 解像度
        public int DisplaySelectedIndex = 0;        // ディスプレイ
        public int TextureSelectedQualityIndex = 0; // テクスチャ品質
    }
}
