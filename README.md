# Full-Stack AI Assistant: Natural Language Query for Sales Data

![Demo](images/ai-assistant-demo.gif)

## 🧐 Project Overview

This project demonstrates a full-stack AI-powered assistant that enables natural language querying of product sales data. Built with **C# (.NET 8)** and **Blazor Web App**, it integrates **Azure SQL Database** and **Azure OpenAI (GPT-4o)** to return structured answers with optional chart visualizations.

- 🌐 **Frontend**: Responsive Blazor UI for input/output interactions
- 📦 **Backend**: ASP.NET Core Web App using Entity Framework Core
- ☁️ **Cloud Integration**: Azure OpenAI for language understanding, Azure SQL for persistent storage
- 🧪 **Testing**: Unit testing with `xUnit`, in-memory test database
- 🐳 **DevOps**: Dockerized multi-container development with future CI pipeline support

---

## 🔧 Key Technologies

| Area | Stack |
|------|-------|
| Frontend | Blazor Server (.NET 8), CSS, JS Interop |
| Backend | ASP.NET Core, EF Core, REST APIs |
| AI Integration | Azure OpenAI (gpt-4o-mini), HttpClientFactory |
| Database | Azure SQL, Code-First Migrations |
| DevOps | Docker, `dotnet test`, CI-ready |
| Testing | xUnit, FluentAssertions, EFCore InMemory |
| Extensions (optional) | Python-based profiling, JS chart rendering |

---

## ⚙️ System Architecture

This project follows **clean architecture** principles, separating concerns between components:

### 1. **Blazor UI**
- Input form to query the AI assistant using natural language
- Shows AI response and optionally renders charts via JS (`renderChart()`)

### 2. **API Logic**
- Uses `HttpClientFactory` to connect to Azure OpenAI
- Processes GPT chat responses and determines how to visualize results

### 3. **Database Layer**
- EF Core with code-first migration
- Product model with validation logic and full CRUD operations

### 4. **Testing Layer**
- Includes unit tests for model validation and CRUD flows
- CI-ready structure for integration into GitHub Actions or Azure Pipelines

---

## 🚀 Features

### ✅ Natural Language to SQL via GPT
- Accepts user input such as “Show top-selling products this quarter”
- Forwards the request to GPT-4o using Azure OpenAI
- Outputs structured English responses with optional chart triggers

### ✅ Responsive UI with Blazor
- Built-in loading indicators, response handling
- JS chart rendering for responses containing visualization triggers

### ✅ Full CRUD Support for Product Data
- Create / Read / Update / Delete for product catalog
- Code-first schema with custom validation messages

### ✅ Unit Testing & Docker Dev Setup
- Test coverage for models, DbContext, and UI flows
- Docker Compose configuration for local multi-container dev

---

## 🧪 Example Unit Test Files

| File | Description |
|------|-------------|
| `AppDbContextTests.cs` | Ensures EF Core context is configured correctly |
| `ProductModelTests.cs` | Tests `Product` validation logic |
| `ProductCreateTests.cs` | Tests creation flow for valid/invalid inputs |
| `ProductEditTests.cs` | Ensures updates reflect correctly |
| `ProductDeleteTests.cs` | Validates deletion logic with database state |

---

## 📈 Future Extensions

This project is designed with **extensibility and scalability** in mind:

- Add support for natural language → SQL generation (chain-of-thought prompting)
- Integrate **Python-based AI user profile inference**
- Enhance chart rendering with dynamic JS-based dashboards (e.g., D3.js or Chart.js)
- Deploy to Azure App Service with CI/CD via GitHub Actions

---


## 🧐 Conclusion: What I Learned

- **C#/.NET 8:** Deepened understanding of Blazor, HttpClientFactory, and app architecture
- **Azure OpenAI:** Learned real-world integration of LLMs via secured cloud APIs
- **Clean Architecture:** Built layers that support testing, swapping components, and scaling
- **DevOps Thinking:** Wrote testable code with CI-ready structure and containerized setup

