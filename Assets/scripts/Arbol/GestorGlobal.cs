using UnityEngine;
public static class GestorGlobal
{
    public static int VidaTorre
    {
        get
        {
            if (GameManagerEndless.Instance != null)
                return GameManagerEndless.Instance.vidaTorre;
            if (GameManager.Instance != null)
                return GameManager.Instance.vidaTorre;
            return 0;
        }
    }

    public static float Puntos
    {
        get
        {
            if (GameManagerEndless.Instance != null)
                return GameManagerEndless.Instance.puntuacionActual;
            if (GameManager.Instance != null)
                return GameManager.Instance.puntos;
            return 0;
        }
    }

    public static TorreScript Torre
    {
        get
        {
            if (GameManagerEndless.Instance != null)
                return GameManagerEndless.Instance.torreScript;
            if (GameManager.Instance != null)
                return GameManager.Instance.torreScript;
            return null;
        }
    }

    public static void SumarPuntos(float puntos)
    {
        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.SumarPuntos((int)puntos);
        else if (GameManager.Instance != null)
            GameManager.Instance.SumarPuntos(puntos);
    }

    public static void SumarMonedas(int cantidad)
    {
        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.SumarMonedas(cantidad);
        else if (GameManager.Instance != null)
            GameManager.Instance.SumarMonedas(cantidad);
    }

    public static bool GastarMonedas(int cantidad)
    {
        if (GameManagerEndless.Instance != null)
            return GameManagerEndless.Instance.GastarMonedas(cantidad);
        else if (GameManager.Instance != null)
            return GameManager.Instance.GastarMonedas(cantidad);
        return false;
    }

    public static void EnemigoDerrotado()
    {
        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.EnemigoDerrotado();
    }

    public static void DamageTower(int daño)
    {
        if (GameManagerEndless.Instance != null)
            GameManagerEndless.Instance.DamageTower(daño);
        else if (GameManager.Instance != null)
            GameManager.Instance.DamageTower(daño);
    }
}



