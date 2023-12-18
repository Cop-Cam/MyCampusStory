using UnityEngine;
using TMPro;

namespace MyCampusStory.StatSystem
{
    [System.Serializable]
    public class Stat
    {
        private const int _statUpperLimit = 100;
        private const int _statLowerLimit = 0;
        
        [SerializeField] private int _value;
        public int Value
        {
            get { return _value; }
            private set { _value = value; }
        }
    
        [SerializeField] private TextMeshProUGUI _valueText;
        public TextMeshProUGUI ValueText
        {
            get { return _valueText; }
            private set { _valueText = value; }
        }

        /// <summary>
        /// This method replaces the current value with a specified value.
        /// </summary>
        public void ReplaceWithValue(int newValue)
        {
            if(newValue > _statUpperLimit)
            {
                _value = _statUpperLimit;
            }
            else if(newValue < _statLowerLimit)
            {
                _value = _statLowerLimit;
            }
            else
            {
                _value = newValue;
            }

            _valueText.text = _value.ToString();
        }

        /// <summary>
        /// This method adds a specified value to the current value.
        /// </summary>
        public void AddToValue(int newValue)
        {

            if(_value + newValue > _statUpperLimit)
            {
                _value = _statUpperLimit;
            }
            else if(_value + newValue < _statLowerLimit)
            {
                _value = _statLowerLimit;
            }
            else
            {
                _value += newValue;
            }

            _valueText.text = _value.ToString();
        }
    }

}
