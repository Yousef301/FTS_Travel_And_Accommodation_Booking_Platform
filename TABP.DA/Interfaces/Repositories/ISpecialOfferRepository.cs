using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ISpecialOfferRepository
{
    Task<IEnumerable<SpecialOffer>> GetAsync();
    Task<SpecialOffer?> GetByIdAsync(Guid id);
    Task<SpecialOffer> CreateAsync(SpecialOffer specialOffer);
    Task DeleteAsync(SpecialOffer specialOffer);
    Task UpdateAsync(SpecialOffer specialOffer);
    Task<bool> ExistsAsync(Expression<Func<SpecialOffer, bool>> predicate);
}