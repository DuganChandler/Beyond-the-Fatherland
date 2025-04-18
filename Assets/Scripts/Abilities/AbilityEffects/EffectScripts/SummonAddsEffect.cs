using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Summon Adds Effect", menuName = "Abilities/Effects/SummonAdds")]
public class SummonAddsEffect : AbilityEffectBase{
    [SerializeField] private List<Character> enemies;
    [SerializeField] private int maxSummon;

    public override IEnumerator ApplyToBattle(AbilityContext context) {
        int numSummoned = 0;
        foreach (Character enemy in enemies) {
            if (context.battleActions.TrySummonEnemy(enemy)) {
                numSummoned++;
                if (numSummoned >= maxSummon) {
                    break;
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }
}