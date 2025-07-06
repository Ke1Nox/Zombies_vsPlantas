[System.Serializable]
public class NodoPuntaje
{
    public int puntaje;
    public NodoPuntaje izquierda;
    public NodoPuntaje derecha;

    public NodoPuntaje(int puntaje)
    {
        this.puntaje = puntaje;
        izquierda = null;
        derecha = null;
    }
}

public class ArbolPuntajes
{
    public NodoPuntaje raiz;

    public void Insertar(int puntaje)
    {
        raiz = InsertarRecursivo(raiz, puntaje);
    }

    private NodoPuntaje InsertarRecursivo(NodoPuntaje nodo, int puntaje)
    {
        if (nodo == null)
            return new NodoPuntaje(puntaje);

        if (puntaje < nodo.puntaje)
            nodo.izquierda = InsertarRecursivo(nodo.izquierda, puntaje);
        else
            nodo.derecha = InsertarRecursivo(nodo.derecha, puntaje);

        return nodo;
    }

    public int ObtenerMaximo()
    {
        NodoPuntaje actual = raiz;
        while (actual != null && actual.derecha != null)
        {
            actual = actual.derecha;
        }
        return actual != null ? actual.puntaje : 0;
    }
}
