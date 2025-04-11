using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class StopNRepeat : MonoBehaviour
{
    public GameObject GameoverPopup;
    public void StopGame()
    {

        GameoverPopup.SetActive(true);
        StartCoroutine(NewGame());
    }

    IEnumerator NewGame()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
