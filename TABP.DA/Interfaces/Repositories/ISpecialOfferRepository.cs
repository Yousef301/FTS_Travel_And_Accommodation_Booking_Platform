using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ISpecialOfferRepository
{
    Task<SpecialOffer?> GetByIdAsync(Guid id);
    Task<IEnumerable<SpecialOffer>> GetExpiredOffersAsync();
    Task<IEnumerable<SpecialOffer>> GetRoomOffersAsync(Guid roomId);
    Task<SpecialOffer?> GetByRoomIdAndOfferIdAsync(Guid id, Guid roomId);
    Task<SpecialOffer> CreateAsync(SpecialOffer specialOffer);
    void Delete(SpecialOffer specialOffer);
    void Update(SpecialOffer specialOffer);
    Task<bool> ExistsAsync(Expression<Func<SpecialOffer, bool>> predicate);
}