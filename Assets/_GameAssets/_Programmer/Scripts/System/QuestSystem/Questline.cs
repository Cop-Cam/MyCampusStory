// //----------------------------------------------------------------------
// // Author   : "Ananta Miyoru Wijaya"
// //----------------------------------------------------------------------

// using System.Collections.Generic;
// using UnityEngine;

// using MyCampusStory.DesignPatterns;

// namespace MyCampusStory.QuestSystem
// {
//     /// <summary>
//     /// Class for Questline initialization and runtime
//     /// </summary>
//     public class Questline
//     {
//         public delegate void QuestlineIsCompletedDelegate();
//         public event QuestlineIsCompletedDelegate OnQuestlineIsCompleted;

//         public QuestlineSO QuestlineData { get; private set; }
//         public bool IsQuestlineCompleted { get; private set; } = false;

//         public Quest CurrentActiveQuestInQuestline { get; private set; }
//         private int _currentActiveQuestIndex;


//         //Constructor
//         public Questline(QuestlineSO questlineSO)
//         {
//             QuestlineData = questlineSO;

//             _currentActiveQuestIndex = 0;
//             InitCurrentActiveQuestInQuestline();
//         }

//         private void InitCurrentActiveQuestInQuestline()
//         {
//             CurrentActiveQuestInQuestline = new Quest(QuestlineData.QuestsInQuestline[_currentActiveQuestIndex]);
            
//             CurrentActiveQuestInQuestline.OnQuestIsCompleted += EvaluateCurrentActiveQuestInQuestline;
//         }

//         private void EvaluateCurrentActiveQuestInQuestline()
//         {
//             if(!CurrentActiveQuestInQuestline.IsQuestCompleted) 
//                 return;

//             CurrentActiveQuestInQuestline.OnQuestIsCompleted -= EvaluateCurrentActiveQuestInQuestline;

//             if(_currentActiveQuestIndex < QuestlineData.QuestsInQuestline.Length - 1)
//             {
//                 _currentActiveQuestIndex++;
//                 InitCurrentActiveQuestInQuestline();
//             }
//             else
//             {
//                 IsQuestlineCompleted = true;
//                 OnQuestlineIsCompleted?.Invoke();
//             }
//         }
//     }
    
//     /// <summary>
//     /// Class for Questline data only or template purpose
//     /// </summary>
    
//     [CreateAssetMenu(fileName = "Name_QuestlineSO", menuName = "ScriptableObjects/QuestlineSO")]
//     public class QuestlineSO : ScriptableObject
//     {
//         public string QuestlineId;
//         public string QuestlineName;
//         public QuestSO[] QuestsInQuestline;
//     }
// }