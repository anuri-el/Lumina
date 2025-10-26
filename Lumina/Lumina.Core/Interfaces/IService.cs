using System.Linq.Expressions;

namespace Lumina.Core.Interfaces
{
    public interface IService<TModel>
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);
        Task<TModel> AddAsync(TModel model);
        Task<TModel> UpdateAsync(TModel model);
        Task DeleteAsync(int id);
    }
}
