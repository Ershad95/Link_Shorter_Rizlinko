namespace LinkShorter.Repository;

public interface IRepo<T> where T : class
{
    public T GetItemById(int id);

    public IQueryable<T> GetAllItems();

    public bool DeleteItem(int id);
    public bool UpdateItem(int id);

    public bool DeleteItem(T t);
    public bool UpdateItem(T t);


    public void InsertItem(T t);


}
