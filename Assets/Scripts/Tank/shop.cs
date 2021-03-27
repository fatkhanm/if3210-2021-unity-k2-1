using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class shop : MonoBehaviour
{
    int cash_1;
    int cash_2;
    public Button Buy1;
    public Button Buy2;
    public Button Buy3;
    public Button Buy4;
    public bool is_buy1;
    public bool is_buy2;
    public bool is_buy3;
    public bool is_buy4;

    public cash2 m_cash2;
    public score m_score;
    // Start is called before the first frame update
    void Start()
    {
        cash_1 = m_score.scorevalue;
        cash_2 = m_cash2.scorevalue;
    }
    
    // Update is called once per frame
    void Update()
    {
        cash_1 = m_score.scorevalue;
        cash_2 = m_cash2.scorevalue;

        if (cash_1 >= 50 && is_buy1 == false)
            Buy1.interactable = true;
            
        else
            Buy1.interactable = false;

        if (cash_1 >= 100 && is_buy1 == true && is_buy2 == false)
            Buy2.interactable = true;

        else
            Buy2.interactable = false;

        if (cash_2 >= 50 && is_buy3 == false)
            Buy3.interactable = true;

        else
            Buy3.interactable = false;

        if (cash_2 >= 100 && is_buy3 == true && is_buy4 == false)
            Buy4.interactable = true;

        else
            Buy4.interactable = false;
    }

    public void Buy_1()
    {
        m_score.CmdAddCash(-50);
        TankShooting.shopvalue2 += 1;
        is_buy1 = true;
    }

    public void Buy_2()
    {
        m_score.CmdAddCash(-100);
        TankShooting.shopvalue2 += 2;
        is_buy2 = true;
    }

    public void Buy_3()
    {
        m_cash2.CmdAddCash(-50);
        TankShooting.shopvalue1 += 1;
        is_buy3 = true;
    }

    public void Buy_4()
    {
        m_cash2.CmdAddCash(-100);
        TankShooting.shopvalue1 += 2;
        is_buy4 = true;
    }
}
