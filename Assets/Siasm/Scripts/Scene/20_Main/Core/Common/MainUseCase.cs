using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Siasm
{
    public class MainUseCase : BaseUseCase
    {
        private readonly MainRepository mainRepository;
        private readonly SaveManager saveManager;

        public SaveDataCache LoadedSaveDataCache => saveManager.LoadedSaveDataCache;

        [Inject]
        public MainUseCase(MainRepository mainRepository, SaveManager saveManager, MemoryDatabase memoryDatabase)
            : base(memoryDatabase)
        {
            this.mainRepository = mainRepository;
            this.saveManager = saveManager;
        }

        public void AddDate()
        {
            mainRepository.AddDate();
        }

        /// <summary>
        /// ステージのモデルクラスの作成
        /// </summary>
        /// <returns></returns>
        public MainStageModel CreateMainStageModel()
        {
            var mainStageModelFactory = new MainStageModelFactory();
            return mainStageModelFactory.CreateMainStageModel(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// 自動販売機のモデルクラスの作成
        /// </summary>
        /// <returns></returns>
        public VendingMachineModel[] CreateVendingMachineModels()
        {
            var vendingMachineModelFactory = new VendingMachineModelFactory();
            return vendingMachineModelFactory.CreateVendingMachineModels(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// 収容Boxのモデルクラスの作成
        /// </summary>
        /// <returns></returns>
        public CreatureBoxModel[] CreateCreatureBoxModels()
        {
            var creatureBoxModelFactory = new CreatureBoxModelFactory();
            return creatureBoxModelFactory.CreateCreatureBoxModels(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// メインクエストのモデルクラスの作成
        /// </summary>
        /// <returns></returns>
        public BaseMainQuestModel CreateBaseMainQuestModel()
        {
            var mainQuestModelFactory = new MainQuestModelFactory();
            return mainQuestModelFactory.CreateBaseMainQuestModel(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// 操作チュートリアルのモデルクラスの作成
        /// 解放しているがクリアしていないチュートリアルの中からIdが一番低いものを取得する
        /// </summary>
        /// <returns></returns>
        public MainOperationTutorialModel CreateMainOperationTutorialModel()
        {
            var mainOperationTutorialModelFactory = new MainOperationTutorialModelFactory();
            return mainOperationTutorialModelFactory.CreateMainOperationTutorialModel(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// ラボ職員のモデルクラスの生成
        /// </summary>
        /// <returns></returns>
        public LabFieldCharacterModel[] CreateLabFieldCharacterModels()
        {
            var labFieldCharacterModelFactory = new LabFieldCharacterModelFactory();
            return labFieldCharacterModelFactory.CreateLabFieldCharacterModels(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// クリシェミナのモデルクラスの生成
        /// </summary>
        /// <returns></returns>
        public CreatureFieldCharacterModel[] CreateCreatureFieldCharacterModels()
        {
            var creatureFieldCharacterModelFactory = new CreatureFieldCharacterModelFactory();
            return creatureFieldCharacterModelFactory.CreateCreatureFieldCharacterModels(saveManager.LoadedSaveDataCache);
        }

        /// <summary>
        /// 指定のメインクエストをクリアに変える
        /// </summary>
        /// <param name="questId"></param>
        public void SetIsClearOfMainQuest(BaseMainQuestModel baseMainQuestModel)
        {
            mainRepository.SetIsClearOfMainQuest(baseMainQuestModel);
        }

        /// <summary>
        /// 指定のクエストを進行状態にする
        /// </summary>
        /// <param name="nextQuestId"></param>
        public void SetProgressOfMainQuest(int nextQuestId)
        {
            mainRepository.SetProgressOfMainQuest(nextQuestId);
        }

        /// <summary>
        /// 会話したかどうかを保持
        /// ここでは会話したかどうかだけを保持して、必要なら2回目に別のセリフを話すなどで使用する
        /// </summary>
        /// <param name="labCharacterId"></param>
        public void SetTalk(int labCharacterId)
        {
            Debug.Log($"ラボ職員と会話した <color=yellow>labCharacterId => {labCharacterId}</color>");
        }

        /// <summary>
        /// 会話モデルを生成
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public async UniTask<BaseTalkModel[]> CreateBaseTalkModels(int characterId, int detialIndex = 0)
        {
            var talkModelFactory = new TalkJsonModelFactory();
            return await talkModelFactory.CreateBaseTalkModels(characterId, detialIndex);
        }

        /// <summary>
        /// 拾った情報を保持する
        /// </summary>
        /// <param name="pickedIndex"></param>
        public void AddPickedIndex(int pickedIndex)
        {
            mainRepository.AddPickedIndex(pickedIndex);
        }

        /// <summary>
        /// アイテムを付与する
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="number"></param>
        public void AddItem(int itemId, int number)
        {
            mainRepository.AddItem(itemId, number);
        }

        /// <summary>
        /// 指定のアイテムを所持しているのか確認
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool HasItemOfOwn(int itemId)
        {
            var saveDataOwnItem = LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == itemId);
            if (saveDataOwnItem == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 撃破状態に変更
        /// </summary>
        public void DestroyedEnemyOfItemDrop(int creatureId)
        {
            var saveDataCreatureBox = LoadedSaveDataCache.SaveDataCreatureBoxs.FirstOrDefault(x => x.CreatureId == creatureId);
            if (saveDataCreatureBox != null)
            {
                // 撃破状態に変更
                saveDataCreatureBox.IsDestroyed = true;
            }
        }

        /// <summary>
        /// 撃破状態を全てリセットする
        /// </summary>
        public void AllResetDestroyedEnemy()
        {
            foreach (var saveDataCreatureBox in LoadedSaveDataCache.SaveDataCreatureBoxs)
            {
                saveDataCreatureBox.IsDestroyed = false;
            }
        }
    }
}
