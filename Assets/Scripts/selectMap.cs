using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class selectMap : MonoBehaviour {

	public void ChangeScene (int scene) {
        SceneManager.LoadScene(scene);
    }
}