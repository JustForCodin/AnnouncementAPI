# Announcement Web API

A robust and scalable Web API for managing announcements, built with .NET 8 and PostgreSQL. This project demonstrates a clean architecture approach, separating concerns into Domain, Application, Infrastructure, and API layers for maintainability and testability.

---

## Features

* **Add New Announcement:** Create and persist new announcements.
* **Edit Announcement:** Update the title and description of existing announcements.
* **Delete Announcement:** Remove an announcement from the system.
* **View All Announcements:** Retrieve a list of all announcements, ordered by the most recent.
* **View Announcement Details:** Get a single announcement by its ID, which also includes a list of the **top 3 most similar announcements**.
    * *Similarity is determined by a simple algorithm that counts shared words in the title and description.*

---

## Technology Stack

* **Backend:** C#, .NET 8
* **Framework:** ASP.NET Core Web API
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core 8
* **API Documentation:** Swagger / OpenAPI

---

## Project Structure

This solution follows the principles of Clean Architecture to ensure a separation of concerns:

* **`Announcements.Domain`**: The core of the application, containing the `Announcement` entity. It has no dependencies on other layers.
* **`Announcements.Application`**: Contains the application logic, services, DTOs, and interfaces. It orchestrates the flow of data.
* **`Announcements.Infrastructure`**: Handles external concerns, primarily the database interaction via Entity Framework Core and the `AnnouncementDbContext`.
* **`Announcements.Api`**: The presentation layer that exposes the functionality via HTTP endpoints.

---

## Setup and Installation

Follow these steps to get the project running locally using the .NET CLI.

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [PostgreSQL](https://www.postgresql.org/download/)
* [Git](https://git-scm.com/downloads/)
* (Optional) A code editor like [Visual Studio Code](https://code.visualstudio.com/).

### 1. Clone the Repository

```bash
git clone <your-repository-url>
cd <repository-folder>
```

### 2. Configure the Database Connection
1. Open the Announcements.Api/appsettings.Development.json file.

2. Update the DefaultConnection string with your local PostgreSQL credentials:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=dbname;Username=your_pg_user;Password=your_pg_password"
}
```

### 3. Install EF Core Tools
If you haven't already, install the Entity Framework Core CLI tool globally.

```bash
dotnet tool install --global dotnet-ef
```

### 4. Create and Apply Migrations
These commands will create the database and the necessary tables based on the AnnouncementDbContext.

```bash
# Navigate to the API project directory
cd Announcements.Api

# Create the initial migration
dotnet ef migrations add InitialCreate --project ../Announcements.Infrastructure

# Apply the migration to the database
dotnet ef database update
```

### 5. Run the Application
Now you can run the API server.

```bash
# From the Announcements.Api directory
dotnet run
```

he application will start, and you can access the Swagger UI for interactive testing at `http://localhost:<port>/swagger`

| Method   | Endpoint | Description                                                        |
| :------- | :------- | :----------------------------------------------------------------- |
| `GET`    | `/`      | Retrieves a list of all announcements.                             |
| `GET`    | `/{id}`  | Retrieves a specific announcement by its ID, including similar ones. |
| `POST`   | `/`      | Creates a new announcement. (Body: `title`, `description`)         |
| `PUT`    | `/{id}`  | Updates an existing announcement. (Body: `title`, `description`)   |
| `DELETE` | `/{id}`  | Deletes a specific announcement by its ID.                         |