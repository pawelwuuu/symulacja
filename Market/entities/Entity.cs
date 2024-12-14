namespace Market.entities;

public abstract class Entity
{
    protected Guid Guid = Guid.NewGuid();
    protected int Turn;
    public void NextTurn() => Turn++;
}