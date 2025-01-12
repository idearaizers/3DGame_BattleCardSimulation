using UnityEngine;

namespace Siasm
{
    public class VendingMachineController : MonoBehaviour
    {
        // [SerializeField]
        // private BaseFieldCharacter baseFieldCharacterPrefab;

        [SerializeField]
        private VendingMachine[] vendingMachines;

        public void Initialize()
        {
            // 
        }

        /// <summary>
        /// モデルクラスを基にセットアップ予定
        /// </summary>
        public void Setup(VendingMachineModel[] vendingMachineModels)
        {
            // 
            foreach (var vendingMachine in vendingMachines)
            {
                vendingMachine.Initialize();
            }

            // 
            // NOTE: モデルクラスを基に生成に変える
            // baseFieldCharacters = new List<BaseFieldCharacter>();

            // // モデルクラスを基に生成
            // foreach (var labFieldCharacterModel in labFieldCharacterModels)
            // {
            //     // 生成と格納
            //     var baseFieldCharacter = Instantiate(BaseFieldCharacterPrefab, transform);
            //     baseFieldCharacters.Add(baseFieldCharacter);

            //     // 初期化
            //     var labFieldCharacter = baseFieldCharacter as LabFieldCharacter;
            //     labFieldCharacter.Initialize(labFieldCharacterModel, MainTalkController, MainQuestController);
            // }

        }
    }
}
