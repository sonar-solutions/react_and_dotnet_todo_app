
import React, { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState('');

  const fetchTodos = async () => {
    const res = await fetch('/api/todos');
    setTodos(await res.json());
  };

  useEffect(() => {
    fetchTodos();
  }, []);

  const addTodo = async (e) => {
    e.preventDefault();
    if (!newTodo.trim()) return;
    await fetch('/api/todos', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title: newTodo, isCompleted: false })
    });
    setNewTodo('');
    fetchTodos();
  };

  const toggleTodo = async (todo) => {
    await fetch(`/api/todos/${todo.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ ...todo, isCompleted: !todo.isCompleted })
    });
    fetchTodos();
  };

  const deleteTodo = async (id) => {
    await fetch(`/api/todos/${id}`, { method: 'DELETE' });
    fetchTodos();
  };

  return (
    <div className="App">
      <h1>TODO List</h1>
      <form onSubmit={addTodo}>
        <input
          value={newTodo}
          onChange={e => setNewTodo(e.target.value)}
          placeholder="Add a new todo"
        />
        <button type="submit">Add</button>
      </form>
      <ul>
        {todos.map(todo => (
          <li key={todo.id}>
            <button
              type="button"
              style={{ textDecoration: todo.isCompleted ? 'line-through' : 'none', cursor: 'pointer', background: 'none', border: 'none', padding: 0, font: 'inherit' }}
              onClick={() => toggleTodo(todo)}
              aria-pressed={todo.isCompleted}
            >
              {todo.title}
            </button>
            <button onClick={() => deleteTodo(todo.id)} style={{ marginLeft: 8 }}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
