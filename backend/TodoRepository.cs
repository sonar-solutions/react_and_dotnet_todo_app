using System.Collections.Generic;
using System.Linq;

public class TodoRepository
{
    private static List<TodoItem> _todos = new List<TodoItem>();
    private static int _nextId = 1;

    public IEnumerable<TodoItem> GetAll() => _todos;
    public TodoItem Get(int id) => _todos.FirstOrDefault(t => t.Id == id);
    public TodoItem Add(TodoItem item)
    {
        item.Id = _nextId++;
        _todos.Add(item);
        return item;
    }
    public bool Update(int id, TodoItem item)
    {
        var todo = Get(id);
        if (todo == null) return false;
        todo.Title = item.Title;
        todo.IsCompleted = item.IsCompleted;
        return true;
    }
    public bool Delete(int id)
    {
        var todo = Get(id);
        if (todo == null) return false;
        _todos.Remove(todo);
        return true;
    }
}
