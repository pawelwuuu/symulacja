using Market.visitors;

namespace Market.entities;

public interface IVisitable
{ 
    void Accept(IVisitor visitor);
}