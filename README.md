
# EF Core Playground

This is a simple .NET console application that demonstrates how to use Entity Framework Core with SQLite. The project includes two entities, `Order` and `OrderItem`, and shows how to query them using joins.

## Table of Contents

- [Requirements](#requirements)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Testing the Query](#testing-the-query)
- [Generating Migrations](#generating-migrations)
- [Contributing](#contributing)
- [License](#license)

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)
- SQLite (for local development)

## Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/ef-core-playground.git
   cd ef-core-playground
   ```

2. **Install EF Core and SQLite packages**:
   This project already includes the necessary dependencies in the `.csproj` file, but if you need to add them manually, run:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

3. **Restore dependencies**:
   After cloning, restore the NuGet packages:
   ```bash
   dotnet restore
   ```

## Running the Application

1. **Build the project**:
   ```bash
   dotnet build
   ```

2. **Run the application**:
   This will create an SQLite database (`orders.db`) in the project directory and run a simple query to join the `Orders` and `OrderItems` tables.
   ```bash
   dotnet run
   ```

   Example output:
   ```
   Order ID: 1, Order Name: Order 1, Item ID: 1, Item Name: Item 1A
   Order ID: 1, Order Name: Order 1, Item ID: 2, Item Name: Item 1B
   Order ID: 2, Order Name: Order 2, Item ID: 3, Item Name: Item 2A
   ```

## Testing the Query

The project demonstrates how to perform a `Join` between the `Order` and `OrderItem` entities. You can modify the LINQ queries in `Program.cs` to test different types of queries, such as filtering, grouping, or projections.

## Generating Migrations

If you need to modify the database schema, you can add new migrations using the following commands:

1. **Add a migration**:
   ```bash
   dotnet ef migrations add <MigrationName>
   ```

2. **Apply the migration**:
   ```bash
   dotnet ef database update
   ```

3. **Remove a migration (if necessary)**:
   ```bash
   dotnet ef migrations remove
   ```

> Ensure you have `Microsoft.EntityFrameworkCore.Tools` installed to run migration commands.

## Contributing

If you would like to contribute to this project, feel free to fork the repository and submit a pull request with your changes. Please make sure your code adheres to the project's style and conventions.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
