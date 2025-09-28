using System.Collections;
using UnityEngine;

public class TokenUnlockHandler : MonoBehaviour
{
    public void UnlockTokens(int amount)
    {
        StartCoroutine(UnlockTokenSequence(amount));
    }

    private IEnumerator UnlockTokenSequence(int amount)
    {
        var remainingTokens = amount;
        while (remainingTokens > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTokens--;
        }
    }
}