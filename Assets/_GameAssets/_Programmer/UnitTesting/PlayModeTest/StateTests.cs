using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using MyCampusStory.StatePatternTest;

public class StateTests
{
    private GameObject _roamerObject;
    private RoamerCharacter _roamerCharacter;

    private GameObject _buildingObject;
    private TestBuilding _building;

    // Custom test building to track interaction behavior
    private class TestBuilding : Building
    {
        public bool Interacted { get; private set; }
        public bool StoppedInteraction { get; private set; }

        public override void OnInteract(GameObject obj) => Interacted = true;
        public override void OnStopInteract(GameObject obj) => StoppedInteraction = true;
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create test building and register it
        _buildingObject = new GameObject("TestBuilding");
        _building = _buildingObject.AddComponent<TestBuilding>();
        _buildingObject.transform.position = Vector3.zero;

        // Create test character
        _roamerObject = new GameObject("RoamerCharacter");
        _roamerCharacter = _roamerObject.AddComponent<RoamerCharacter>();

        // Ensure initialization (calls Start)
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(_roamerObject);
        Object.Destroy(_buildingObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator B04_StateTransitions()
    {
        // Confirm initial state
        Assert.IsInstanceOf<RoamerCharacter.RoamerIdleState>(_roamerCharacter.GetCurrentState());

        // Manually trigger transition from Idle → Walk
        var idleState = new RoamerCharacter.RoamerIdleState();
        idleState.EnterState(_roamerCharacter);
        _roamerCharacter.SwitchState(new RoamerCharacter.RoamerWalkState());

        Assert.IsInstanceOf<RoamerCharacter.RoamerWalkState>(_roamerCharacter.GetCurrentState());

        // Simulate walk transition to interact
        _roamerCharacter.CurrentBuildingToMove = _building;
        var walkState = new RoamerCharacter.RoamerWalkState();
        walkState.EnterState(_roamerCharacter);
        _roamerCharacter.SwitchState(new RoamerCharacter.RoamerInteractState());

        Assert.IsInstanceOf<RoamerCharacter.RoamerInteractState>(_roamerCharacter.GetCurrentState());

        yield break;
    }

    [UnityTest]
    public IEnumerator B05_BehaviorChangesWithState()
    {
        // Simulate Idle behavior (nothing to assert here — no side effects)
        var idleState = new RoamerCharacter.RoamerIdleState();
        idleState.EnterState(_roamerCharacter);
        idleState.UpdateState(_roamerCharacter);

        // Switch to Walk state and simulate entering it
        _roamerCharacter.CurrentBuildingToMove = _building;
        var walkState = new RoamerCharacter.RoamerWalkState();
        walkState.EnterState(_roamerCharacter);
        walkState.UpdateState(_roamerCharacter); // Should prepare building to move
        Assert.IsFalse(_building.Interacted, "Should not interact yet during walk");

        // Switch to Interact state and simulate interaction
        var interactState = new RoamerCharacter.RoamerInteractState();
        interactState.EnterState(_roamerCharacter);
        interactState.UpdateState(_roamerCharacter);

        // Simulate end of interaction
        _building.OnStopInteract(_roamerCharacter.gameObject);

        yield break;
    }
}
