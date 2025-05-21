//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using MyCampusStory.StandaloneManager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for displaying quests
    /// </summary>
    public class QuestUIManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _activeAnimParam = "ISOPEN";
        [SerializeField] private TextMeshProUGUI _questNameText;
        [SerializeField] private TextMeshProUGUI _questDescriptionText;
        [SerializeField] private ObjectiveUI _objectiveUIPrefab;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void OnEnable()
        {
            LevelManager.Instance.QuestManager.OnQuestStarted += RefreshDisplay;
        }

        private void OnDisable()
        {
            LevelManager.Instance.QuestManager.OnQuestStarted -= RefreshDisplay;
        }

        public void OpenQuestUI()
        {
            RefreshDisplay();

            _animator.SetBool("ISOPEN", true);
        }

        public void CloseQuestUI()
        {
            _animator.SetBool("ISOPEN", false);
        }

        private void RefreshDisplay()
        {
            if(LevelManager.Instance.QuestManager.CurrentActiveQuest == null)
            {
                _questNameText.text = "???";
                _questDescriptionText.text = "???";
            }
            else
            {
                Quest currentActiveQuest = LevelManager.Instance.QuestManager.CurrentActiveQuest;
                _questNameText.text = currentActiveQuest.QuestData.QuestName;
                _questDescriptionText.text = currentActiveQuest.QuestData.QuestDescription;
            }
        }
    }
}