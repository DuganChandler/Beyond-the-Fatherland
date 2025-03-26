using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Items/Effects/HealHP")]
public class HealEffect : ItemEffectBase {
    [SerializeField] private int healAmount;

    public override void ApplyEffect(Character user, Character target) {
        target.IncreaseHP(healAmount);
    }

}
