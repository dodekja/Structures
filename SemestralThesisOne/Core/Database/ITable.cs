using System.ComponentModel;
using System.Windows.Automation.Peers;

namespace SemestralThesisOne.Core.Database;

public interface ITable<T>
{
    public void Add(T data);
}