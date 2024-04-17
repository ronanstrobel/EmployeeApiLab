# EmployeeApi - Laboratory Application with .NET 8 and C# 12

## Description

This application is a laboratory project created with the aim of testing and exploring various libraries and technologies in the .NET ecosystem. It was developed using .NET 8 and C# 12 and serves as a playground to experiment with the following libraries:

- TestContainers
- FastEndpoints
- RepoDb

The main idea is to provide an environment where we can test and better understand the operation of these libraries, their characteristics, and how they can be integrated into real projects.

## Technologies Used

- **.NET 8**: Framework used as the base for developing the application.
- **C# 12**: Programming language used to write the application's logic.

## Requirements

To run this application, you'll need the following prerequisites installed and running:

- **.NET SDK 8**: .NET 8 SDK to compile and run the project.
- **Docker**: Container platform used to run the Docker containers necessary for testing with TestContainers and databases.

## Tested Libraries

### TestContainers

[TestContainers](https://www.testcontainers.org/) is a library that simplifies running Docker containers for testing. With it, you can start, manage, and stop Docker containers directly in tests, providing an isolated and controlled environment for integration testing.

#### Tested Features:

- Start and stop Docker containers for integration testing.
- Connect to databases in Docker containers for integration testing with RepoDb.

### FastEndpoints

[FastEndpoints](https://github.com/FastEndpoints/FastEndpoints) is a library that assists in creating HTTP endpoints quickly and simply, allowing for the creation of APIs with less code and greater productivity.

#### Tested Features:

- Create HTTP endpoints for CRUD (Create, Read, Update, Delete) using FastEndpoints.
- Integrate FastEndpoints with RepoDb for database operations.

### RepoDb

[RepoDb](https://repodb.net/) is a lightweight and fast ORM (Object-Relational Mapping) library for .NET. It offers a simple and efficient way to perform database operations without the need to write SQL queries manually.

#### Tested Features:

- Perform CRUD (Create, Read, Update, Delete) operations with the database.
- Test integration with databases in Docker containers using TestContainers.

## How to Run

1. **Prerequisites:**
   - Make sure **.NET SDK 8** and **Docker** are installed and running on your machine.

2. **Clone the repository:**
    ```
    git clone https://github.com/ronanstrobel/EmployeeApiLab.git
    ```

3. **Navigate to the project directory:**
    ```
    cd EmployeeApiLab
    ```

4. **Restore dependencies:**
    ```
    dotnet restore
    ```

5. **Run postgreSql docker container**
    ```
    $ docker run -p 5432:5432 -e POSTGRES_PASSWORD=123456 -d postgres
    ```

6. **Run the application:**
    ```
    dotnet run src/CrudEmployee.Api 
    ```
---

We hope this documentation helps you better understand the purpose and technologies used in this laboratory project! If you have any questions or suggestions, feel free to reach out.
