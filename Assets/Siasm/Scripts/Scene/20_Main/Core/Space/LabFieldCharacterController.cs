namespace Siasm
{
    /// <summary>
    /// 全てのラボ職員の生成と管理を行う
    /// NOTE: 通行人も含まれているので管理が複雑になるのであれば分ける
    /// </summary>
    public sealed class LabFieldCharacterController : BaseFieldCharacterController
    {
        public void Setup(LabFieldCharacterModel[] labFieldCharacterModels)
        {
            base.Setup();

            // モデルクラスを基に生成
            foreach (var labFieldCharacterModel in labFieldCharacterModels)
            {
                // 生成と格納
                var baseFieldCharacter = Instantiate(BaseFieldCharacterPrefab, transform);
                BaseFieldCharacters.Add(baseFieldCharacter);

                // 初期化
                baseFieldCharacter.Initialize(MainTalkController, MainQuestController, MainCamera);
                baseFieldCharacter.Setup(labFieldCharacterModel);
            }
        }
    }
}
