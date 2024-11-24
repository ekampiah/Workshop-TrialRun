# Todo Application Documentation

## Overview
The Simple Todo Application is designed using the MVC (Model-View-Controller) architecture. This pattern helps to separate concerns, making the code more maintainable and scalable. The application allows users to manage their todo items with basic CRUD (Create, Read, Update, Delete) operations.

## Model
The Model represents the application's data and business logic. It manages the todo items and handles data operations such as adding, editing, and deleting items.

### Item
- **id**: Guid - Unique identifier for each todo item.
- **title**: String - Title of the todo item.
- **description**: String - Description of the todo item.
- **isCompleted**: Boolean - Status of the todo item (completed or not).

## Views
The View is responsible for displaying the todo items to the user. It listens to the Controller for updates and renders the user interface accordingly.

### TodoList
- **renderTodoList(todoItems: List<TodoItem>)**: Renders the list of todo items.
- **renderTodoItem(todoItem: TodoItem)**: Renders a single todo item.

### TodoForm
- **renderAddTodoForm()**: Renders the form for adding a new todo item.
- **renderEditTodoForm(todoItem: TodoItem)**: Renders the form for editing an existing todo item.

## API(Controller) Methods
The Controller acts as an intermediary between the Model and the View. It handles user input, updates the Model, and selects the View to display data.

- **GetItemList()**: Retrieves list of Todo items
- **AddItem(title: String, description: String)**: Adds a new todo item.
- **GetItem(id: guid)**: Retrieves a todo item by its ID.
- **UpdateItem(id: guid, title: String, description: String, isCompleted: Boolean)**: Updates a todo item.
- **DeleteItem(id: guid)**: Deletes a todo item.

## Conclusion
The Simple Todo Application leverages the MVC architecture to maintain a clear separation of concerns, making the codebase easier to understand and manage. By documenting each component—Model, View, and Controller—we ensure that the application is well-structured and maintainable. This documentation can serve as a guide for developers working on the application or for future enhancements.
