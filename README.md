
# Educational Web Application

## Overview

This is an educational web application designed for administrative users to manage and enroll users across departments, trainees, courses, and instructors. The application provides CRUD operations and enrollment functionalities for managing educational content and participants.

## Features

- **Admin CRUD Operations**: Manage Departments, Trainees, Courses, and Instructors.
- **Enrollment Management**: Handle trainee enrollment in different courses.
- **User Authentication and Authorization**: Secure login and access controls using ASP.NET Core Identity.
- **Responsive UI**: Built with Bootstrap for a mobile-friendly design.
- **Asynchronous Operations**: AJAX-based updates for smooth user interactions.

## Technologies Used

- **Backend**: ASP.NET Core MVC
- **Identity Management**: ASP.NET Core Identity
- **Database**: MS SQL Server, managed via Entity Framework Core
- **Frontend**: Bootstrap, AJAX
- **Design Patterns**:
  - **Dependency Injection**
  - **Repository Pattern** (handling all service-related functionality)

## Project Structure

- **Controllers**: Define the logic for each entity and handle CRUD operations.
- **Models**: Represent the data structure for Departments, Trainees, Courses, and Instructors.
- **View Models**: Facilitate data transfer between the models and views.
- **Views**: Razor views, styled with Bootstrap, provide the user interface.
- **Repositories**: Interfaces and implementations for data access and business logic.

## Getting Started

### Prerequisites

- .NET 6 SDK or higher
- MS SQL Server
- Visual Studio 2022 or Visual Studio Code

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/morg-ma/Educational-Web-Application.git
   cd Educational-Web-Application
   ```

2. **Restore the database**:
   - Open SQL Server Management Studio (SSMS).
   - Connect to your SQL Server instance.
   - Right-click on **Databases** and select **Restore Database...**.
   - Choose **Device** and select the uploaded database backup file.
   - Ensure the database name matches the connection string in `appsettings.json`.

3. **Configure the connection string**:
   - Update the connection string in `appsettings.json` to match your SQL Server instance and database name.

4. **Run the application**:
   ```bash
   dotnet run
   ```

### Usage

- **Admin Login**: Use predefined admin credentials or register a new account.
- **Department Management**: Create, update, and delete departments.
- **Trainee Enrollment**: Enroll trainees in courses and manage enrollments.
- **Course Management**: Add and manage course details and assign instructors.

## Contribution

Feel free to submit pull requests or report issues if you would like to contribute to the project.
