using Xunit;

public class TodoRepositoryTests
{
    public TodoRepositoryTests()
    {
        // Reset static state before each test
        typeof(TodoRepository)
            .GetField("_todos", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, new System.Collections.Generic.List<TodoItem>());
        typeof(TodoRepository)
            .GetField("_nextId", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, 1);
    }
    [Fact]
    public void Add_AddsTodoItem()
    {
        var repo = new TodoRepository();
        var item = new TodoItem { Title = "Test", IsCompleted = false };
        var added = repo.Add(item);
        Assert.NotEqual(0, added.Id);
        Assert.Equal("Test", added.Title);
        Assert.False(added.IsCompleted);
    }

    [Fact]
    public void GetAll_ReturnsAllItems()
    {
        var repo = new TodoRepository();
        repo.Add(new TodoItem { Title = "A", IsCompleted = false });
        repo.Add(new TodoItem { Title = "B", IsCompleted = true });
        var all = repo.GetAll();
        Assert.Equal(2, System.Linq.Enumerable.Count(all));
    }

    [Fact]
    public void Update_UpdatesItem()
    {
        var repo = new TodoRepository();
        var item = repo.Add(new TodoItem { Title = "A", IsCompleted = false });
        var updated = new TodoItem { Title = "B", IsCompleted = true };
        var result = repo.Update(item.Id, updated);
        Assert.True(result);
        var fetched = repo.Get(item.Id);
        Assert.Equal("B", fetched.Title);
        Assert.True(fetched.IsCompleted);
    }

    [Fact]
    public void Delete_RemovesItem()
    {
        var repo = new TodoRepository();
        var item = repo.Add(new TodoItem { Title = "A", IsCompleted = false });
        var result = repo.Delete(item.Id);
        Assert.True(result);
        var fetched = repo.Get(item.Id);
        Assert.Null(fetched);
    }
}
