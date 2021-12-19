using System;
using System.Threading.Tasks;

namespace currency {

  public interface ICurrencyProvider<T> where T: class {

    bool IsEmpty(int index);
    T GetLastItem(int index);
    T GetByDate(int index, DateTime date);
    event EventHandler<int> OnDataChanged;
    Task Refresh(int index);
  }
}