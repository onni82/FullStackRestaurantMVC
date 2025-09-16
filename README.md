# FullStackRestaurant MVC

FullStackRestaurant MVC is a sample web application built with ASP.NET Core Razor Pages targeting .NET 8. It demonstrates a full-stack approach for managing restaurant data, including menu items, orders, and customer information.
The API that is being used can be found at [FullStackRestaurant](https://github.com/onni82/FullStackRestaurant)

## Features

- CRUD operations for menu items, orders, and customers
- Responsive UI using Razor Pages
- Entity Framework Core integration for data access
- User authentication and authorization
- Validation and error handling

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or another supported database
- Visual Studio 2022 or later

### Installation

1. Clone the repository:
`git clone https://github.com/onni82/FullStackRestaurantMVC.git`
2. Open the solution in Visual Studio.
3. Update the connection string in `appsettings.json` to match your database configuration.


## Usage

- Access the application at `https://localhost:7085` in HTTPS or `https://localhost:5062` in HTTP (or the port specified in launch settings).
- Use the navigation menu to manage menu items, orders, and customers.
- Authentication is required for administrative actions.
