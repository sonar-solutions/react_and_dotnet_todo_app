
import userEvent from '@testing-library/user-event';

import { render, screen } from '@testing-library/react';
import App from './App';

test('can add, toggle, and delete a todo', async () => {
  // Mock fetch for GET, POST, PUT, DELETE
  const todos = [
    { id: 1, title: 'Test Todo', isCompleted: false }
  ];
  global.fetch = jest.fn()
    .mockImplementationOnce(() => Promise.resolve({ json: () => Promise.resolve([]) })) // initial GET
    .mockImplementationOnce(() => Promise.resolve({})) // POST
    .mockImplementationOnce(() => Promise.resolve({ json: () => Promise.resolve(todos) })) // GET after add
    .mockImplementationOnce(() => Promise.resolve({})) // PUT
    .mockImplementationOnce(() => Promise.resolve({ json: () => Promise.resolve([{ ...todos[0], isCompleted: true }]) })) // GET after toggle
    .mockImplementationOnce(() => Promise.resolve({})) // DELETE
    .mockImplementationOnce(() => Promise.resolve({ json: () => Promise.resolve([]) })); // GET after delete

  render(<App />);
  const input = screen.getByPlaceholderText(/add a new todo/i);
  const addBtn = screen.getByText(/add/i);
  await userEvent.type(input, 'Test Todo');
  await userEvent.click(addBtn);

  // After add
  expect(await screen.findByText('Test Todo')).toBeInTheDocument();

  // Toggle
  const todoBtn = screen.getByText('Test Todo');
  await userEvent.click(todoBtn);
  expect(todoBtn).toHaveStyle('text-decoration: line-through');

  // Delete
  const deleteBtn = screen.getByText('Delete');
  await userEvent.click(deleteBtn);
  expect(screen.queryByText('Test Todo')).not.toBeInTheDocument();
});
