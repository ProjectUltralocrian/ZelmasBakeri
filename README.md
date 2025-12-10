# ZelmasBakeri

ZelmasBakeri is a full-stack .NET application for managing bakery orders. The solution models a bakery with products, orders, and possibility for customer feedback. The goal is to support practical production planning based on incoming orders and real-world constraints.

The project is both a useful tool in a real domain and a playground for exploring domain modelling, architecture, and running .NET applications in Azure.

---

## Technology overview

- **Language & runtime**
  - C#, .NET 9

- **Frontend**
  - Blazor (`ZelmasBakeriApp`)

- **Backend / domain logic / API**
  - ASP.NET Core (`ZelmasBakeriBackend`)
  - Domain models and business logic implemented in C# classes
  - Exposes functionality to the Blazor app and other potential clients

- **Database**
  - SQL Server
  - Hosted as **Azure SQL**
  - Dapper for data access (`ZelmasBakeriDB`)

- **Cloud & hosting**
  - Deployed as an **Azure Web App**
  - Connection strings and secrets handled via Azure configuration (App Settings / Key Vault, depending on environment)
  - Separate configuration for local development and production

- **Version control & build**
  - Git (GitHub)

---

## Architecture

The intent is to keep a clear separation between:

- **UI** (`ZelmasBakeriApp`)
- **Backend/domain logic** (`ZelmasBakeriBackend`)
- **Database/persistence** (`ZelmasBakeriDB`)
