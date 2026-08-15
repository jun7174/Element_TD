// Assets/Scripts/Stages/StageManager.cs

using UnityEngine;

namespace ElementTD
{
    // 선택된 스테이지 데이터를 읽어서 스테이지 프리팹을 배치하고,
    // 원소 타일 생성과 웨이브 진행을 시작한다.
    public class StageManager : MonoBehaviour
    {
        [SerializeField]
        private StageDataSO _stageData;

        [SerializeField]
        private StageElementTileGenerator _elementTileGenerator;

        [SerializeField]
        private WaveSpawner _waveSpawner;

        [SerializeField]
        private TowerSelectionController _selectionController;

        private void Start()
        {
            LoadStage();
        }

        private void LoadStage()
        {
            GameObject stageInstance = Instantiate(_stageData.StagePrefab);
            StageReferences stageReferences = stageInstance.GetComponent<StageReferences>();

            ElementTileQuery.CurrentOverlayTilemap = stageReferences.ElementOverlayTilemap;
            _selectionController.SetBuildableAreaTilemap(stageReferences.BuildableAreaTilemap);

            _elementTileGenerator.Generate(stageReferences, _stageData.ElementTileRatio);

            EconomyManager.Initialize(_stageData.StartingGold);
            PlayerHealth.ResetHealth();
            GameTimer.ResetTimer();
            Debug.Log("시작 골드 " + EconomyManager.CurrentGold);

            _waveSpawner.StartStage(_stageData, stageReferences);
        }
    }
}
