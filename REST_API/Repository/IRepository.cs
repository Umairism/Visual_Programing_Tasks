namespace REST_API.Repository
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Add new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Update existing entity
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Delete entity by ID
        /// </summary>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Check if entity exists
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Save changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}
