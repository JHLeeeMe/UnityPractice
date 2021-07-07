using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other) {
        RubyController ruby = other.GetComponent<RubyController>();
        if (ruby == null) { return; }

        ruby.ChangeHealth(-1);
    }
}
