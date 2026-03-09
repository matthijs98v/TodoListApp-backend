# Todo list app

The backend of a todolist app. In the backend you could create and share a todolist, It is also posible to add members to your list with read and write access as well.

This Project uses the backend
- [Vue frontend](https://github.com/matthijs98v/TodoListApp-frontend)

Here is a live demo [DEMO](https://todo.matthijsverheijen.com).

## Technical detials 
This app is build in c# and dotnet 10. It uses the dotnet Entity framework for the database and migrations. It also uses signal r for the realtime todolist

## Setup on local in development
### For starting the project
```sh
docker compose up -d
```
```sh
cd .\src\TodoApp.Api\
```
```sh
dotnet watch run
```
### For the tests
```sh
dotnet test
```

## Ef core usage
### Add migrations
```sh
dotnet ef migrations add InitialCreate --output-dir Infrastructure/Data/Migrations
```
### Remove migrations
```sh
dotnet ef migrations remove
```

### Apply to the database
```sh
dotnet ef database update
```