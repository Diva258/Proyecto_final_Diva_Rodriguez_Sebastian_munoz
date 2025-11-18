using UnityEngine;

public class AnimatorBoolTrigger : MonoBehaviour
{
    public Animator animator;
    public string parameterName = "Hover";
    public void SetBoolTrue()  { animator.SetBool(parameterName, true); }
    public void SetBoolFalse() { animator.SetBool(parameterName, false); }
}
