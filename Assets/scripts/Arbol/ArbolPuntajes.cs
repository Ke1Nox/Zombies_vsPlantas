using System.Collections.Generic;

[System.Serializable]
public class NodoPuntaje
{
    public PuntajeJugador dato;
    public NodoPuntaje izquierda;
    public NodoPuntaje derecha;

    public NodoPuntaje(PuntajeJugador dato)
    {
        this.dato = dato;
        izquierda = null;
        derecha = null;
    }
}

public class ArbolPuntajes
{
    public NodoPuntaje raiz;

    public void Insertar(PuntajeJugador dato)
    {
        raiz = InsertarRecursivo(raiz, dato);
    }

    private NodoPuntaje InsertarRecursivo(NodoPuntaje nodo, PuntajeJugador dato)
    {
        if (nodo == null)
            return new NodoPuntaje(dato);

        if (dato.puntaje < nodo.dato.puntaje)
            nodo.izquierda = InsertarRecursivo(nodo.izquierda, dato);
        else
            nodo.derecha = InsertarRecursivo(nodo.derecha, dato);

        return nodo;
    }

    public PuntajeJugador ObtenerMaximo()
    {
        NodoPuntaje actual = raiz;
        while (actual != null && actual.derecha != null)
        {
            actual = actual.derecha;
        }
        return actual != null ? actual.dato : null;
    }

    public List<PuntajeJugador> ObtenerTop(int cantidad)
    {
        List<PuntajeJugador> lista = new List<PuntajeJugador>();
        InOrderDesc(raiz, lista, cantidad);
        return lista;
    }

    private void InOrderDesc(NodoPuntaje nodo, List<PuntajeJugador> lista, int limite)
    {
        if (nodo == null || lista.Count >= limite)
            return;

        InOrderDesc(nodo.derecha, lista, limite);
        if (lista.Count < limite)
            lista.Add(nodo.dato);
        InOrderDesc(nodo.izquierda, lista, limite);
    }
}
