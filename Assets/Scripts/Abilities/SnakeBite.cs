using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBite : MonoBehaviour
{
    public void ActivateAbility(Entity target) {
        StatusEffect poison = GetComponent<Poison>();
    }
}
