namespace DevSpot.Repositories
{
    public interface IRepository<T>where T : class
    {
        Task<IEnumerable<T>> GetAllAsinc();
        Task<T> GetByIdAsync(int Id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);   


    }
}
