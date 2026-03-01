# 🏥 MedLink - Enterprise-Grade Healthcare Solution

MedLink is a sophisticated healthcare management ecosystem built with **.NET 8** and **Clean Architecture**. It provides a robust, scalable backend for connecting patients with medical professionals through intelligent scheduling, geospatial discovery, and secure, event-driven payment processing.

---

## 💎 Advanced Features & Technical Strengths

### 🌍 Geospatial Intelligence
- **Location-Aware Discovery**: Powered by **NetTopologySuite (GIS)**, enabling patients to find doctors based on precise geographic coordinates.
- **Clinic Mapping**: Support for SRID 4326 (WGS84) for spatial data accuracy.

### 💳 Event-Driven Payment Logic
- **Stripe Integration**: Seamless payment processing for consultation fees.
- **Webhook Handling**: Automated, asynchronous confirmation of payments using Stripe Webhooks for maximum reliability and "checkout session" tracking.

### 📅 Intelligent Appointment Ecosystem
- **Smart Scheduling**: Real-time availability management with automated release of unconfirmed slots.
- **Transactional Integrity**: Uses a Unit of Work pattern with explicit **Transaction Management** to ensure data consistency during complex rescheduling workflows.
- **Concurrency Protection**: Implements `DbUpdateConcurrencyException` handling to prevent double-booking.

### 🛡️ Data Resilience & Integrity
- **Soft Deletes**: Global Query Filters ensure that deleted entities (like Doctors) are preserved in the DB but excluded from application logic.
- **Clean Persistence**: Advanced implementation of the **Specification Pattern** to decouple query logic from repositories, ensuring DRY and testable code.

### 🔐 Identity & communication
- **Hybrid Auth**: Secure Microsoft Identity with **Google OAuth 2.0** support.
- **Automated Messaging**: Integrated **Twilio SMS** and **Gmail SMTP** for appointment reminders and account notifications.

---

## 🏗 Architecture Overview

The system follows the **Onion Architecture** pattern to ensure independence from external frameworks:

- **MedLink.Domain**: Pure business logic, Domain Entities, Enums, and Core Interfaces.
- **MedLink.Application**: Services, DTOs, AutoMapper, and the **Specification Pattern** library.
- **MedLink.Infrastructure**: Persistence (EF Core), External APIs (Stripe, Twilio), and Identity services.
- **Medical Team B (API)**: RESTful controllers, global exception handling, and Swagger documentation.

---

## 🛠 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB/Express)
- Visual Studio 2022 or VS Code

### ⚙ Configuration
Update `appsettings.json` in the API project with your API keys:
```json
{
  "ConnectionStrings": { "DefaultConnection": "..." },
  "Stripe": { "SecretKey": "...", "WebhookSecret": "..." },
  "Twilio": { "AccountSID": "...", "AuthToken": "..." },
  "Authentication": { "Google": { "ClientId": "...", "ClientSecret": "..." } }
}
```

### 🚀 Launching
1. **Initialize Database**:
   ```powershell
   dotnet ef database update --project MedLink.Infrastructure --startup-project "Medical Team B"
   ```
2. **Run Application**:
   ```powershell
   dotnet run --project "Medical Team B"
   ```

---

## 📖 Swagger Documentation
Explore and test the API endpoints at:
`https://localhost:XXXX/swagger/index.html`

The API provides detailed endpoints for:
- `Doctor Discovery` (Search by Specialization/City/Name)
- `Appointment Management` (Book/Reschedule/Cancel)
- `Payment History`
- `Patient Dashboards`

---
*Powered by Medical Team B - Quality Healthcare through Clean Code*
