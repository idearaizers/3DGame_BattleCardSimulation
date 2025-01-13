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

            foreach (var labFieldCharacterModel in labFieldCharacterModels)
            {
                var baseFieldCharacter = Instantiate(BaseFieldCharacterPrefab, transform);
                BaseFieldCharacters.Add(baseFieldCharacter);

                baseFieldCharacter.Initialize(MainTalkController, MainQuestController, MainCamera);
                baseFieldCharacter.Setup(labFieldCharacterModel);
            }
        }
    }
}
