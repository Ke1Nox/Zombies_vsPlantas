using System.Collections.Generic;

[System.Serializable]
public class ContenedorRanking
{
    public List<PuntajeJugador> ranking;

    public ContenedorRanking(List<PuntajeJugador> ranking)
    {
        this.ranking = ranking;
    }
}
