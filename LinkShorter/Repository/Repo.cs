using LinkShorter.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Repository;

public class Repo<T> : IRepo<T> where T : class
{
    private LinkShorterContext _linkShorterContext;
    private readonly DbSet<T> _data;
    public Repo(LinkShorterContext linkShorterContext)
    {
        _linkShorterContext = linkShorterContext;
         _data = _linkShorterContext.Set<T>();
    }
    private bool DeleteOperation(T t)
    {
        try
        {
            if (t == null) return false;
            _data.Remove(t);
            _linkShorterContext.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public bool DeleteItem(int id)
    {
        try
        {
            if (id <= 0)
                return false;
            var item = _data.Find(id);
            return DeleteOperation(item);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteItem(T t)
    {
        return DeleteOperation(t);
    }
    //public virtual IQueryable<T> Table => _data;
    public IQueryable<T> GetAllItems()
    {
        return _data;
    }

    public T GetItemById(int id)
    {
        return _data.Find(id);
    }

    public void InsertItem(T t)
    {
        _data.Add(t);
        _linkShorterContext.SaveChanges();
    }

    public bool UpdateItem(int id)
    {
        throw new NotImplementedException();
    }

    public bool UpdateItem(T t)
    {
        throw new NotImplementedException();
    }
}
