using Market.entities;

namespace Market.visitors;

public interface IVisitor
{
    void VisitSeller(Seller seller);
    void VisitBuyer(Buyer buyer);
}