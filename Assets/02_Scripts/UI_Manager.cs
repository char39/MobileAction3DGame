using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnClickButton()
    {
        Player.instance.PlayerSkillOn();
    }
    public void OnClickButtonDashAttack()
    {
        Player.instance.PlayerDashAttackOn();
    }
    
}
