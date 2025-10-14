# 🚀 Northwind DB Excercises with MudBlazor

Welcome to this Blazor Web App! This project demonstrates how to create a modern Blazor WebAssembly app with a responsive UI powered by **MudBlazor** and connected to a **SQL Server** database (Northwind DB).

## 🛠️ Features

- **Blazor WebAssembly**: A client-side web app framework powered by .NET.
- **MudBlazor**: A Material Design component library for Blazor.
- **Northwind DB**: A popular demo database for learning SQL and database management.
- **SQL Server**: The back-end database that holds the Northwind data.

## 📦 Prerequisites

Before running this project, you need the following tools and software:

- [Visual Studio 2022](https://visualstudio.microsoft.com/), [Rider](https://www.jetbrains.com/rider/), [VSCode](https://code.visualstudio.com/) (with **Blazor WebAssembly** and **SQL Server** components)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [MudBlazor](https://mudblazor.com/) library installed via NuGet
- The [Northwind Database](https://github.com/microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs) installed in SQL Server

## 📝 Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/hco556/Northwind.Odata.Templates.git
cd NorthwindExercises
```
### 2. Set Up Northwind Database

Download Northwind DB: [click here](https://github.com/microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs)

Restore Northwind DB in SQL Server: Use the instnwnd.sql script file and execute it on your SQL Server instance.
Update the appsettings.Development.json file with your SQL Server connection string.
```json
{
  "ConnectionStrings": {
    "NorthwindConnection": "Server=your_server;Database=Northwind;User Id=your_username;Password=your_password;Integrated Security=True;Trust Server Certificate=True;Trusted_Connection=True;MultipleActiveResultSets=true;"
  }
}
```
### 3. Install MudBlazor or MudBlazor Template

Install MudBlazor using NuGet:
```bash
dotnet add package MudBlazor
```

or Install MudBlazor Templates (Ready to use) using Terminal:

The MudBlazor Templates are based on the Microsoft Web App template but have been modified to include MudBlazor components. 

Open a terminal and install them using this command:
```bash
dotnet new install MudBlazor.Templates
```

### 4. Run the Application

Run the Blazor WebAssembly app using Visual Studio/Visual Studio Code or Rider or the following command:
```bash
dotnet run
```

Visit http://localhost:5000 **(your port must be different)** in your browser and explore the application!

## 📊 Project Structure

- Pages: Contains all the Blazor pages (UI components).
- Data: Data models and services (e.g., NorthwindService.cs for DB interaction).
- wwwroot: Static files like CSS, JS, and images.
- Shared: Common UI components shared across pages.
- MudBlazor Components: Custom MudBlazor components and styling.

## 🔧 Technologies Used

- Blazor WebAssembly: Build interactive web apps using C#.
- MudBlazor: A Material Design library for Blazor apps.
- SQL Server: A relational database management system.
- Northwind Database: A sample database for understanding business operations.

## 💻 Contribution

- Fork the repository.
- Create a new branch: git checkout -b feature/your-feature.
- Commit your changes: git commit -m 'Add new feature'.
- Push to your branch: git push origin feature/your-feature.
- Create a pull request.

## ⚙️ Troubleshooting

- SQL Server connection issues: Double-check your connection string and make sure SQL Server is running.
- MudBlazor components not loading: Ensure you’ve installed the correct MudBlazor package and added it to _Imports.razor.

##  🧾 License

- This project is licensed under the MIT License - see the LICENSE.md file for details.

🎉 Enjoy Building Your Blazor App!
Feel free to modify, customize, and build upon this template for your own Blazor web projects. Happy coding! 😊


# T4 templates & Kiota api client generation
## Project Northwind.Odata.Api:
This project also includes T4 templates for code generation and Kiota for generating API clients.
- T4 Templates: Used for generating repetitive code structures, reducing boilerplate code, and improving maintainability.
- Kiota: A tool for generating API clients from OpenAPI descriptions, making it easier to interact with RESTful APIs.
- Usage: You can find the T4 templates in the 'Templates' folder and Kiota configurations in the 'Kiota' folder. Follow the documentation for each tool to customize and generate code as needed.
- Benefits: Using T4 templates and Kiota can significantly speed up development time, ensure consistency in code, and simplify API interactions.
- Getting Started: To get started with T4 templates, you can refer to the official [T4 documentation](https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates). For Kiota, check out the [Kiota GitHub repository](

## Northwind.Odata.Api.Client
- kiota client generation commands:
- https://learn.microsoft.com/en-us/openapi/kiota/quickstarts/dotnet
- Create Console App Northwind.Odata.Api.Client
- install kiota bundle:Microsoft.Kiota.Bundle
- Install-Package Microsoft.Kiota.Bundle
- Install the Kiota CLI tool globally:
- dotnet tool install --global Microsoft.OpenApi.Kiota
- Generate the client code using the Kiota CLI:
- kiota generate -l CSharp -c NorthwindClient -n Northwind.Odata.Api.Client -d https://localhost:5000/odata/openapi.json -o ./Northwind.Odata.Api.Client

# To Do's:
## Use Relection in T4 templates:
- example: https://blog.jermdavis.dev/posts/2023/migrating-t4-to-source-generators#top

- ## Add Validation using T4 templates
