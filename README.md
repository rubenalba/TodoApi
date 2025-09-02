# 📝 TodoApi

A **RESTful API** for managing tasks (To-Do list) built with **.NET 8**.  
It follows a clean layered architecture (**Controller → Service → Repository**) with **Entity Framework Core** and includes **unit and integration tests**.
---

## 🚀 Tech Stack
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- xUnit, Moq (unit tests)
- EFCore.InMemory (integration tests)

---

## Features

-   **Task CRUD**: Create, read, update, and delete tasks.\
-   **DTO Mapping**: Uses AutoMapper to map between domain models and
    DTOs.\
-   **Repository Pattern**: Clear separation of concerns with
    repositories handling data access.\
-   **JWT Authentication**: API endpoints are secured using JSON Web
    Tokens. Each user can only access their own tasks.\
-   **Unit Testing**: Full coverage for controllers and services with
    xUnit and Moq.

## 📂 Project Structure
- **Domain** → Entities and DTOs
- **Application** → Interfaces and business logic (Services)
- **Infrastructure** → Data access layer (Repositories with EF Core)
- **API** → Controllers and API configuration
- **Tests** → Unit and integration tests


## Project Structure

    TodoApi.sln
    ├── Api                 → Entry point (controllers, DI setup, authentication config)
    ├── Application         → Services, interfaces, DTOs, business logic
    ├── Domain              → Entities, models, core business rules
    ├── Infrastructure      → EF Core, repositories, DbContext
    ├── Tests
    │   ├── UnitTests       → Unit tests for controllers and services
    │   └── IntegrationTests→ Integration tests (planned/ongoing)


## ⚙️ Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/rubenalba/TodoApi.git
   cd TodoApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Create the database:
   ```bash
   dotnet ef database update
   ```

4. Run the project:
   ```bash
   dotnet run --project Api
   ```

👉 Swagger UI available at:
```
https://localhost:5001/swagger
```

---

## 🧪 Testing

Run all tests (unit + integration):
```bash
dotnet test
```

Coverage:
- **TaskControllerTest** ✅ 100%
- **TaskServiceTest** ✅ 100%
- **IntegrationTests (Repository)** 🚧 in progress

---

## 📡 Main Endpoints

### Get all tasks for a user
```http
GET /api/tasks
```

### Get a task by ID
```http
GET /api/tasks/{id}
```

### Create a new task
```http
POST /api/tasks
Content-Type: application/json

{
  "title": "Finish project",
  "description": "Complete TodoApi with tests",
  "dueDate": "2025-09-15T00:00:00Z"
}
```

### Update a task
```http
PUT /api/tasks/{id}
Content-Type: application/json

{
  "title": "Updated task title",
  "isCompleted": true
}
```

### Delete a task
```http
DELETE /api/tasks/{id}
```

---

## 📜 License
This project is licensed under the MIT License.