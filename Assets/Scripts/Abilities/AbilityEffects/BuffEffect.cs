using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Effect", menuName = "Abilities/Effects/Buff")]
public class AbilityBuffEffect : AbilityEffectBase {
    [SerializeField] private int abilityBuffAmount;
    [SerializeField] private int duration;
    [SerializeField] private string stat;

    // public override IEnumerator ApplyToCharacter(AbilityContext) {
    //     // if(initailRound == 0){
    //     //     // initailRound = battleSystem.CurrentRound();
    //     //     switch(stat){
    //     //         case "Strength":
    //     //             user.strengthBuffs += user.Stats.Strength * (1/abilityBuffAmount);
    //     //             break;
    //     //         case "Magic":
    //     //             user.magicBuffs += user.Stats.Magic * (1/abilityBuffAmount);
    //     //             break;
    //     //         case "Defense":
    //     //             user.defenseBuffs += user.Stats.Defense * (1/abilityBuffAmount);
    //     //             break;
    //     //     }
    //     // }
    //     // if(battleSystem.CurrentRound() - initailRound > duration){
    //     //     switch(stat){
    //     //         case "Strength":
    //     //             user.strengthBuffs -= user.Stats.Strength * (1/abilityBuffAmount);
    //     //             break;
    //     //         case "Magic":
    //     //             user.magicBuffs -= user.Stats.Magic * (1/abilityBuffAmount);
    //     //             break;
    //     //         case "Defense":
    //     //             user.defenseBuffs -= user.Stats.Defense * (1/abilityBuffAmount);
    //     //             break;
    //     //     }
    //     // }
    //     // return new AbilityEffectInfo(Color.green, $"");
    //     yield return null;
    // }


}
