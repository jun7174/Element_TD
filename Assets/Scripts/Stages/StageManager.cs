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

        private void Start()
        {
            LoadStage();
        }

        private void LoadStage()
        {
            GameObject stageInstance = Instantiate(_stageData.StagePrefab);
            StageReferences stageReferences = stageInstance.GetComponent<StageReferences>();

            ElementTileQuery.CurrentOverlayTilemap = stageReferences.ElementOverlayTilemap;

            _elementTileGenerator.Generate(stageReferences, _stageData.ElementTileRatio);

            Debug.Log("시작 골드 " + _stageData.StartingGold);

            _waveSpawner.StartStage(_stageData, stageReferences);
        }
    }
}
