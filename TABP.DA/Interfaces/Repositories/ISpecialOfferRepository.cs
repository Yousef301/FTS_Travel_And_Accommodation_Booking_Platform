using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ISpecialOfferRepository
{
    Task<IEnumerable<SpecialOffer>> GetAsync();
    Task<IEnumerable<SpecialOffer>> GetExpiredOffersAsync();
    Task<IEnumerable<SpecialOffer>> GetRoomOffersAsync(Guid roomId);
    Task<SpecialOffer?> GetByRoomIdAndOfferIdAsync(Guid id, Guid roomId);
    Task<SpecialOffer> CreateAsync(SpecialOffer specialOffer);
    Task DeleteAsync(Guid id, Guid roomId);
    Task UpdateAsync(SpecialOffer specialOffer);
    Task<bool> ExistsAsync(Expression<Func<SpecialOffer, bool>> predicate);
}