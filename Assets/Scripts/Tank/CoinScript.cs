using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

	private shop m_shop;
    private void Awake()
    {
		m_shop = GameObject.Find("Shop").GetComponent<shop>();
    }
    void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.GetComponent<TankMovement>().m_PlayerNumber == 1)
		{

			m_shop.m_cash2.scorevalue += 10;
			Destroy(gameObject);
			
		}

		if (other.gameObject.GetComponent<TankMovement>().m_PlayerNumber == 2)
		{

			//m_shop.m_score.CmdAddCash(10);
			m_shop.m_score.scorevalue += 10;
			Destroy(gameObject);
			
		}
	}
}
