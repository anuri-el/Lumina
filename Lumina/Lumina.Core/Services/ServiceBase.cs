using Lumina.Core.Interfaces;
using System.Linq.Expressions;

namespace Lumina.Core.Services
{
    public abstract class ServiceBase<TModel> : IService<TModel>
        where TModel : class
    {
        protected readonly IRepository<TModel> _repository;

        protected ServiceBase(IRepository<TModel> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
            => await _repository.GetAllAsync();

        public virtual async Task<TModel?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public virtual async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
            => await _repository.FindAsync(predicate);

        public virtual async Task<TModel> AddAsync(TModel model)
            => await _repository.AddAsync(model);

        public virtual async Task<TModel> UpdateAsync(TModel model)
            => await _repository.UpdateAsync(model);

        public virtual async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}
