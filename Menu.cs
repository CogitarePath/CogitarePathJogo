using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void AcaoBotao(GameObject button)
    {
        string Tagbutton = button.tag;

        if (Tagbutton == "NovoJogo")
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("AreaResiddencial");
            Debug.Log("Novo Jogo");
        }
        else if (Tagbutton == "ContinuarJogo")
        {
            SceneManager.LoadScene("NomeDaCenaDoJogo");
            Debug.Log("Continuar Jogo");
        }
        else if (Tagbutton == "Sair")
        {
            Application.Quit();
            Debug.Log("Sair da aplicação");
        }
    }
}
