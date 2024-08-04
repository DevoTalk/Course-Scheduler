### README

# University Course Scheduling App

This application is developed using C# and ASP.NET Core to help manage university course schedules. The app allows users to define courses, their prerequisites, and instructors along with their available times. Based on this data, the app generates a weekly schedule.

## Features

- Define courses and their prerequisites.
- Manage instructor information and their availability.
- Generate weekly course schedules automatically.
- User-friendly interface for easy management of data.

## Requirements

- .NET Core SDK 6.0 or higher
- Visual Studio 2022 (optional but recommended for development)

## Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/university-course-scheduling-app.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd university-course-scheduling-app
    ```

3. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

4. **Update database connection string:**

    Open `appsettings.json` and update the `ConnectionStrings` section with your database information.

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
    }
    ```

5. **Apply migrations and update the database:**

    ```bash
    dotnet ef database update
    ```

6. **Run the application:**

    ```bash
    dotnet run
    ```

    The application should now be running at `https://localhost:5001` or `http://localhost:5000`.

## Usage

1. **Define Courses and Prerequisites:**
    - Navigate to the "Courses" section.
    - Add new courses and specify their prerequisites if any.

2. **Manage Instructors and Availability:**
    - Navigate to the "Instructors" section.
    - Add new instructors and specify their available times.

3. **Generate Weekly Schedule:**
    - Go to the "Generate Schedule" section.
    - Click on "Generate" to create a weekly schedule based on the provided data.

## Contributing

If you would like to contribute to the project, please follow these steps:

1. Fork the repository.
2. Create a new feature branch.
3. Make your changes and commit them.
4. Push your branch and create a pull request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.

## Contact

If you have any questions or feedback, please feel free to reach out at [devotalk@gmail.com](mailto:devotalk@gmail.com).

---

Please let me know if you need any additional information or if there are specific details you'd like to include.
