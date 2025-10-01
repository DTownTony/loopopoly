using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TokenUnlockHandler : MonoBehaviour
{
    [SerializeField] private GearDatabase _gearDatabase;
    [SerializeField] private GlobalEvents _globalEvents;
    
    public void UnlockTokens(int amount)
    {
        StartCoroutine(UnlockTokenSequence(amount));
    }

    private IEnumerator UnlockTokenSequence(int amount)
    {
        var data = GlobalManagers.Instance.GameProfile.GameData;
        var remainingTokens = amount;
        while (remainingTokens > 0)
        {
            var unlocks = new List<Unlock>();
            //todo: get locked items
            
            //get all locked events that player has not unlocked
            var lockedEvents = _globalEvents.GetLockedEvents(data.UnlockedEvents);
            foreach (var lockedEvent in lockedEvents)
                unlocks.Add(new Unlock(lockedEvent.Id, UnlockType.Event));
            
            //get all gears that player has not unlocked
            var lockedGears = _gearDatabase.GetLockedGear(data.UnlockedGear);
            foreach (var lockedGear in lockedGears)
                unlocks.Add(new Unlock(lockedGear.Id, UnlockType.Gear));

            if (unlocks.Count <= 0)
            {
                //todo: give loop gems
                continue;
            }
            
            var randomUnlockIndex = Random.Range(0, unlocks.Count);
            var unlock = unlocks[randomUnlockIndex];

            switch (unlock.UnlockType)
            {
                case UnlockType.Gear:
                    data.UnlockedGear.Add(unlock.Id);
                    break;
                case UnlockType.Event:
                    data.UnlockedEvents.Add(unlock.Id);
                    break;
            }
            
            remainingTokens--;
            
            //todo: fancy animation
            
            yield return new WaitForSeconds(1f);
        }
    }

    private struct Unlock
    {
        public string Id;
        public UnlockType UnlockType;

        public Unlock(string id, UnlockType type)
        {
            Id = id;
            UnlockType = type;
        }
    }

    private enum UnlockType
    {
        Gear,
        Event
    }
}