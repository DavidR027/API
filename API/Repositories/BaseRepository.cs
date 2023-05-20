using API.Contexts;
using API.Contracts;
using API.Models;
using System.Data;

namespace API.Repositories;

public class BaseRepository<T> where T : class
{

    private readonly BookingManagementDbContext _context;

    public BaseRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    /*
     * <summary>
     * Create Entity
     * </summary>
     * <param name="param">Entity object</param>
     * <returns>Entity object</returns>
     */
    public T Create(T param)
    {
        try
        {
            _context.Set<T>().Add(param);
            _context.SaveChanges();
            return param;
        }
        catch
        {
            return null;
        }
    }

    /*
    * <summary>
    * Update Entity
    * </summary>
    * <param name="param">Entity object</param>
    * <returns>true if data updated</returns>
    * <returns>false if data not updated</returns>
    */
    public bool Update(T param)
    {
        try
        {
            _context.Set<T>().Update(param);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }


    /*
    * <summary>
    * Delete Entity
    * </summary>
    * <param name="guid">Entity guid</param>
    * <returns>true if data deleted</returns>
    * <returns>false if data not deleted</returns>
    */
    public bool Delete(Guid guid)
    {
        try
        {
            var param = GetByGuid(guid);
            if (param == null)
            {
                return false;
            }

            _context.Set<T>().Remove(param);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /*
     * <summary>
     * Get all Entity
     * </summary>
     * <returns>List of entities</returns>
     * <returns>Empty list if no data found</returns>
     */
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    /*
     * <summary>
     * Get an entity by guid
     * </summary>
     * <param name="guid">Entity guid</param>
     * <returns>Entity object</returns>
     * <returns>null if no data found</returns>
     */
    public T? GetByGuid(Guid guid)
    {
        return _context.Set<T>().Find(guid);
    }

}

