using UnityEngine;
using TMPro;

public class CapturaNombreJugador : MonoBehaviour
{
    public TMP_InputField inputNombre;

    public void GuardarNombre()
    {
        string nombre = inputNombre.text.Trim();

        if (string.IsNullOrEmpty(nombre))
            nombre = "Anonimo";

        PlayerPrefs.SetString("nombreJugador", nombre);
        PlayerPrefs.Save();

        Debug.Log("Nombre guardado: " + nombre); //  Confirmar en consola
    }

}

