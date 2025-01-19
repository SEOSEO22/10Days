using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHungerModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerState stat = character.GetComponent<PlayerState>();
        if (stat != null)
        {
            stat.IncreaseHungerStat(val);
            GameManager.Instance.SetGaugeText();
        }
    }
}
