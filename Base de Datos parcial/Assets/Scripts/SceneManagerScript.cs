using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    public Image fade;
    public void CambioEscena(string es)
    {
        fade.CrossFadeAlpha(1, 1, true);
        StartCoroutine(ActivoFade(es));
    }

    IEnumerator ActivoFade(string e)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(e);
    }
}
