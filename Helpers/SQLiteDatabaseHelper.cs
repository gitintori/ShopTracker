using ShopTracker.Models;
using SQLite;

namespace ShopTracker.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Product>().Wait();
        }

        public Task<int> Insert(Product p)
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Product>> Update(Product p)
        {
            string sql = "UPDATE Product SET Description=?, Quantity=?, Price=?, Category=? WHERE Id=?";
            return _conn.QueryAsync<Product>(
            sql, p.Description, p.Quantity, p.Price, p.Category, p.Id
            );
        }
        public Task<int> Delete(int id)
        {
            return _conn.Table<Product>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Product>> GetAll()
        {
            return _conn.Table<Product>().ToListAsync();
        }
        public Task<List<Product>> Search(string q)
        {
            string sql = "SELECT * FROM Product WHERE description LIKE '%" + q + "%' ";
            return _conn.QueryAsync<Product>(sql);
        }

        public Task<List<Product>> FilterByCategory(string category)
        {
            if (string.IsNullOrEmpty(category) || category == "ALL")
            {
                return GetAll();
            }

            return _conn.Table<Product>()
                       .Where(p => p.Category == category)
                       .ToListAsync();
        }

    }
}
