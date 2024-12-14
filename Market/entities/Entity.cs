using Market.visitors;

namespace Market.entities;

public abstract class Entity : IVisitable
{
    public Guid Guid = Guid.NewGuid();
    protected int Turn;
    public void NextTurn() => Turn++;
    public abstract void Accept(IVisitor visitor);
}