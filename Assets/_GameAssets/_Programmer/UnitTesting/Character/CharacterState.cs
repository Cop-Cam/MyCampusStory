namespace MyCampusStory.StatePatternTest
{
    public abstract class CharacterState
    {
        public abstract void EnterState(Character character);
        public abstract void UpdateState(Character character);
        public abstract void ExitState(Character character);
    }
}
