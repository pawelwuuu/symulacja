using Market.visitors;

namespace Market.entities;

public interface IEntity
{
    void Accept(IVisitor visitor);
}