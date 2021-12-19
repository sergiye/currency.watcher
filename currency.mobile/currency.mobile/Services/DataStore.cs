using currency.mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace currency.mobile.Services {

  public interface IDataStore<T> {
    Task<bool> AddItemAsync(T item);
    //Task<bool> UpdateItemAsync(T item);
    //Task<bool> DeleteItemAsync(string id);
    Task<T> GetItemAsync(string id);
    Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
  }

  public class DataStore : IDataStore<Item> {
    readonly List<Item> items;

    public DataStore() {
      items = new List<Item>()
      {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
    }

    public async Task<bool> AddItemAsync(Item item) {
      items.Add(item);

      return await Task.FromResult(true);
    }

    public async Task<bool> UpdateItemAsync(Item item) {
      var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
      items.Remove(oldItem);
      items.Add(item);

      return await Task.FromResult(true);
    }

    public async Task<bool> DeleteItemAsync(string id) {
      var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
      items.Remove(oldItem);

      return await Task.FromResult(true);
    }

    public async Task<Item> GetItemAsync(string id) {
      return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
    }

    public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false) {
      return await Task.FromResult(items);
    }
  }
}