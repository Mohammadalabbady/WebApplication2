# Tracker Case Management System

A comprehensive compliance tracking and regulatory case management system built with ASP.NET Core MVC, designed to streamline the management of regulatory findings, compliance cases, and remedial actions.

## 🎯 System Overview

The Tracker Case Management system is designed to support organizations in managing their compliance requirements through:

- **Annual Work Plan Management**: Organize compliance activities around annual plans with multiple legislations
- **Case Tracking**: Comprehensive case management with full audit trails
- **Workflow Automation**: Role-based approval processes for case management
- **Document Management**: Secure file attachments and document tracking
- **Reporting & Analytics**: Comprehensive reporting for compliance monitoring

## 🏗️ Architecture

### Technology Stack
- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server (LocalDB for development)
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Bootstrap 5, jQuery, DataTables
- **Icons**: Font Awesome

### Core Components
- **Models**: Data entities for cases, organizations, legislations, compliance controls
- **Services**: Business logic layer for case management operations
- **Controllers**: HTTP request handling and response management
- **Views**: Razor pages for user interface
- **DbContext**: Entity Framework data access layer

## 🚀 Features

### 1. Case Management
- Create, edit, and delete compliance cases
- Track case status, priority, and progress
- Full audit trail of all case changes
- File attachment support for documentation

### 2. Workflow Management
- Role-based access control (Regulations Admin, Planner, Final User)
- Approval workflows for case creation and modifications
- Status tracking and notifications
- Comment and justification tracking

### 3. Compliance Tracking
- Annual work plan structure
- Legislation and compliance control management
- Finding tracking and resolution
- Remedial plan creation and monitoring

### 4. User Management
- Role-based permissions
- User authentication and authorization
- Department and position tracking
- Activity logging

## 📋 Requirements

### System Requirements
- .NET 8.0 SDK or Runtime
- SQL Server (LocalDB for development)
- Windows 10/11 or compatible OS

### Development Requirements
- Visual Studio 2022 or VS Code
- .NET 8.0 SDK
- SQL Server Management Studio (optional)

## 🛠️ Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd WebApplication2
```

### 2. Database Setup
The system uses Entity Framework Code First approach. The database will be created automatically on first run.

**Connection String** (in `appsettings.json`):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TrackerCaseManagement;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

### 3. Build and Run
```bash
dotnet restore
dotnet build
dotnet run
```

### 4. Initial Setup
On first run, the system will:
- Create the database
- Seed initial data including:
  - User roles (Regulations Admin, Planner, Final User)
  - Admin user account
  - Sample organizations and legislations

## 👥 User Roles & Permissions

### Regulations Admin
- **Full system control**
- Approve/reject case workflows
- Manage all cases and data
- Access to workflow management

### Planner
- **Create and modify cases**
- Add case updates
- Create remedial plans
- Close cases
- Cannot approve workflows

### Final User
- **View-only access**
- Browse cases and reports
- No modification capabilities

## 📊 Data Models

### Core Entities
- **Case**: Main case entity with all compliance details
- **Organization**: Affiliated entities
- **Legislation**: Regulatory frameworks
- **ComplianceControl**: Specific compliance requirements
- **CaseUpdate**: Tracking of case changes
- **RemedialPlan**: Action plans for resolution
- **CaseWorkflow**: Approval workflow management

### Relationships
- Cases belong to Organizations and Legislations
- Cases can have multiple Updates and RemedialPlans
- Workflows track approval processes
- All entities support soft deletion

## 🔄 Workflow Process

### Case Creation Workflow
1. **Planner** creates a new case
2. Case status set to "Open" (pending approval)
3. **Regulations Admin** reviews and approves/rejects
4. Case status updated based on decision
5. Case can proceed to updates, remedial plans, or closure

### Case Update Process
1. **Planner** or **Admin** updates case details
2. Changes tracked in CaseUpdate entity
3. Significant changes trigger workflow if required
4. Full audit trail maintained

## 📁 File Structure

```
WebApplication2/
├── Controllers/          # HTTP request handlers
├── Data/                # Database context and configurations
├── Models/              # Data entities and view models
├── Services/            # Business logic layer
├── Views/               # Razor view templates
│   ├── Cases/          # Case management views
│   ├── Home/           # Home page and navigation
│   └── Shared/         # Layout and common views
├── wwwroot/            # Static files (CSS, JS, images)
└── Program.cs          # Application entry point
```

## 🎨 User Interface

### Design Principles
- **Responsive Design**: Works on desktop and mobile devices
- **Bootstrap 5**: Modern, clean UI framework
- **DataTables**: Enhanced table functionality with sorting and filtering
- **Font Awesome**: Professional iconography
- **Card-based Layout**: Organized information presentation

### Key Views
- **Dashboard**: Overview with statistics and quick actions
- **Case List**: Comprehensive case listing with filters
- **Case Details**: Detailed case information with timeline
- **Forms**: Intuitive input forms for all operations
- **Workflow Management**: Admin interface for approvals

## 🔒 Security Features

- **Authentication**: ASP.NET Core Identity
- **Authorization**: Role-based access control
- **Input Validation**: Server-side and client-side validation
- **File Upload Security**: Type and size restrictions
- **SQL Injection Protection**: Entity Framework parameterized queries

## 📈 Reporting & Analytics

### Available Reports
- Case statistics (total, open, closed, pending)
- Organization-based case distribution
- Legislation compliance tracking
- Workflow status monitoring
- User activity tracking

### Data Export
- DataTables integration for sorting and filtering
- Pagination for large datasets
- Search functionality across all fields

## 🚧 Development & Customization

### Adding New Features
1. **Models**: Add new entities to the Models folder
2. **DbContext**: Update ApplicationDbContext
3. **Services**: Implement business logic in Services layer
4. **Controllers**: Add new actions and endpoints
5. **Views**: Create corresponding Razor views

### Database Migrations
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Configuration
- Database connection strings in `appsettings.json`
- Identity configuration in `Program.cs`
- Service registrations in dependency injection

## 🧪 Testing

### Manual Testing
- Create test cases with sample data
- Test all user roles and permissions
- Verify workflow processes
- Test file uploads and attachments

### Automated Testing
- Unit tests for services (recommended)
- Integration tests for controllers
- Database seeding for test environments

## 📝 API Documentation

The system provides RESTful endpoints for:
- **Cases**: CRUD operations for case management
- **Workflows**: Approval and rejection processes
- **Updates**: Case modification tracking
- **Remedial Plans**: Action plan management

## 🔧 Troubleshooting

### Common Issues
1. **Database Connection**: Verify connection string and SQL Server status
2. **File Uploads**: Check wwwroot/uploads folder permissions
3. **Authentication**: Ensure user roles are properly assigned
4. **Workflow Issues**: Verify case status and workflow state

### Logs
- Application logs in standard .NET logging
- Database queries in Entity Framework logging
- User activity in system audit trails

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Implement changes with proper testing
4. Submit a pull request with detailed description

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📞 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Review system documentation

## 🔮 Future Enhancements

### Planned Features
- **Email Notifications**: Automated workflow notifications
- **Advanced Reporting**: Custom report builder
- **API Integration**: External system connectivity
- **Mobile App**: Native mobile application
- **Advanced Analytics**: Machine learning insights

### Technical Improvements
- **Caching**: Redis integration for performance
- **Microservices**: Service-oriented architecture
- **Cloud Deployment**: Azure/AWS hosting support
- **Real-time Updates**: SignalR integration

---

**Version**: 1.0.0  
**Last Updated**: January 2025  
**Maintainer**: Development Team
