# React and .NET Project

This repository contains a full-stack application with a React frontend and a .NET backend.

## Project Structure

- `frontend/` - React application (JavaScript/TypeScript)
- `backend/` - .NET backend (C#)
- `backendTests/` - Unit tests for the backend
- `architecture/` - Architecture exports and documentation
- `scan.sh` - Shell script for scanning or automation

## Getting Started

### Prerequisites
- Node.js (for frontend)
- .NET SDK (for backend)

### Setup

#### Backend
1. Navigate to the backend directory:
   ```sh
   cd backend
   ```
2. Restore dependencies and run the backend:
   ```sh
   dotnet restore
   dotnet run
   ```

#### Frontend
1. Navigate to the frontend directory:
   ```sh
   cd frontend
   ```
2. Install dependencies and start the frontend:
   ```sh
   npm install
   npm start
   ```

### Running Tests

#### Backend Tests
```sh
cd backendTests
 dotnet test
```

#### Frontend Tests
```sh
cd frontend
npm test
```

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
