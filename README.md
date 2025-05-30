# Event Management System

A comprehensive event management system built with Azure Functions (C#) backend and Vue.js frontend. This system allows users to browse events, view event details, and register for events.

## 📋 Features

### Backend (Azure Functions)
- **CRUD Operations**: Create, read, update, and delete events
- **Event Filtering**: Filter events by date and location
- **RESTful API**: Clean REST API endpoints
- **Event Model**: Events include name, location, date, and start time

### Frontend (Vue.js)
- **Event Listing**: Browse all events with filtering capabilities
- **Event Details**: Detailed view of individual events
- **Event Registration**: User-friendly registration form
- **Responsive Design**: Works on desktop and mobile devices

### User Registration
When registering for events, users provide:
- Name
- Email address
- Pronouns
- Opt-in preference for future communications

## 🏗️ Architecture

```
├── backend/                 # Azure Functions C# backend
│   └── EventManagement/    # Main Azure Functions project
├── frontend/               # Vue.js frontend
│   └── event-management/   # Main Vue.js application
├── .github/
│   ├── workflows/          # CI/CD pipelines
│   ├── dependabot.yml     # Dependency management
│   └── SECURITY.md        # Security policy
└── docs/                  # Documentation
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js 20.x
- Azure Functions Core Tools (for local development)

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend/EventManagement
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run locally:
   ```bash
   func start
   ```

The backend API will be available at `http://localhost:7071`

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend/event-management
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Run development server:
   ```bash
   npm run dev
   ```

The frontend will be available at `http://localhost:5173`

## 🔧 API Endpoints

### Events
- `GET /api/events` - Get all events (supports filtering)
  - Query parameters: `date`, `location`
- `GET /api/events/{id}` - Get specific event
- `POST /api/events` - Create new event
- `PUT /api/events/{id}` - Update event
- `DELETE /api/events/{id}` - Delete event

### Example Event Object
```json
{
  "id": 1,
  "name": "Tech Conference 2024",
  "location": "San Francisco, CA",
  "date": "2024-06-15T00:00:00Z",
  "startTime": "09:00:00"
}
```

## 🐳 Docker Support

Both frontend and backend can be containerized:

### Backend Container
```bash
cd backend/EventManagement
docker build -t event-management-backend .
```

### Frontend Container
```bash
cd frontend/event-management
docker build -t event-management-frontend .
```

## 🔄 CI/CD

The project includes automated CI/CD pipelines:

- **Backend CI**: Builds, tests, and creates container images for the Azure Functions backend
- **Frontend CI**: Builds, lints, type-checks, and creates container images for the Vue.js frontend
- **Dependabot**: Automatically updates dependencies daily
- **Security Scanning**: Automated security checks

## 🛡️ Security

We take security seriously. Please review our [Security Policy](.github/SECURITY.md) for:
- Reporting vulnerabilities
- Security best practices
- Supported versions

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Ensure tests pass
5. Submit a pull request

### Code Standards
- Follow existing code style and patterns
- Use dependency injection where appropriate
- Write tests for new functionality
- Document public APIs

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🏷️ Versioning

We use [Semantic Versioning](https://semver.org/) for version management.

## 📞 Support

For support and questions:
- Create an issue in this repository
- Review existing documentation
- Check the security policy for security-related concerns