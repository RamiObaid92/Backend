# Copilot Instructions – WebAPI Backend Best Practices

These instructions serve as guidance for both developers and GitHub Copilot to maintain consistent, secure, and clean code in the WebAPI backend.

---

## 1. Layered Architecture

1. **Data Layer (Repositories, EF Core)**
   - Keep EF Core `DbContext` and entity classes separated from business logic.
   - Repositories encapsulate all database queries.

2. **Business Layer (Services)**
   - Implement core logic, domain rules, validations.
   - Use DTO mapping to convert between entities and DTOs.

3. **API Layer (Controllers)**
   - Remain as thin as possible – just handle HTTP requests/responses.
   - Defer all logic to Services; only coordinate calls and return results.

**Instruction:** 
- Avoid placing **business logic** or **database queries** inside **Controllers**.  
- Keep **Entities** and **DTOs** separated.  
- Use **Dependency Injection** for Repositories and Services.

---

## 2. Security Considerations

1. **Authentication & Authorization**
   - Implement **ASP.NET Core Identity** or **JWT bearer tokens** (or both) for auth.
   - Protect routes with `[Authorize]`.  
   - Restrict dangerous endpoints to roles, e.g. `[Authorize(Roles="Admin")]`.

2. **API Keys & Secrets**
   - Store secrets and connection strings in **user-secrets** or environment variables.
   - Never commit secrets or API keys to source control.

3. **CORS**
   - Configure **CORS** policies to allow only trusted origins. Unless in Development phase for testing.
   - If using cookies (httpOnly) for auth tokens, also set `SameSite` and `Secure`.

4. **Input Validation & ModelState**
   - Validate incoming data at multiple levels:
     - **ModelState** (using `[ApiController]`).
     - **Business logic** checks in Services (unique constraints, etc.).
     - **Repository** checks for final failsafe (e.g., null-checks).

**Instruction:**  
- Always use `[ValidateAntiForgeryToken]` or XSRF tokens if using cookies.  
- Use `[FromBody]` DTOs and confirm `ModelState.IsValid` in each controller action.  
- Implement strong password policies if using Identity.

---

## 3. Naming & Conventions

1. **Controllers**
   - Suffix with `Controller`, e.g. `ProjectController`.
   - Route examples:
     - `[Route("api/[controller]")]`
     - `[HttpGet("{id}")]`, `[HttpPost]`, etc.

2. **Entities & DTOs**
   - Entities: `ProjectEntity`, `ClientEntity`, `UserEntity`.
   - DTOs: `ProjectDto`, `CreateProjectDto`, `UpdateProjectDto`.
   - Keep naming consistent and singular.

3. **Repository & Service**
   - Interfaces: `IProjectRepository`, `IProjectService`.
   - Implementations: `ProjectRepository`, `ProjectService`.

**Instruction:**  
- Use **PascalCase** for class and method names.  
- Avoid ambiguous names, keep them descriptive.

---

## 4. Logging & Error Handling

1. **Exception Handling**
   - Use **global error handling middleware** or filters to catch unhandled exceptions.
   - Return appropriate HTTP status codes (`400`, `404`, `500`, etc.).

**Instruction:**  
- Keep errors consistent so frontends can handle them properly.

---

## 5. Performance & Caching

1. **Caching**
   - Use in-memory or distributed cache for read-heavy data.
   - Mark common GET endpoints with `[ResponseCache]` or custom caching logic.

2. **Pagination & Filtering**
   - Implement paging for large result sets to avoid returning huge payloads.
   - Provide query parameters for filtering/sorting where applicable.

3. **Async/Await**
   - Use async/await for all database/network I/O to avoid blocking threads.

**Instruction:**  
- Avoid returning giant lists in a single request if expecting thousands of records.  
- Provide `pageNumber`, `pageSize`, `sort`, etc. in the query where it makes sense.

---

## 6. DTO Mapping & Factories

1. **AutoMapper** (optional but recommended)
   - If you have many DTOs, consider using AutoMapper for cleaner mapping code.

2. **Factory Methods**
   - For smaller or simpler use cases, create manual mapping methods in the Service layer.

**Instruction:**  
- Keep **DTO-to-Entity** and **Entity-to-DTO** mapping separate from the core logic.  
- Don’t mix mapping code directly into your controllers or repositories.

---

## 7. Entity Relationships & Normalization

1. **Follow Normal Forms (1NF, 2NF, 3NF)**
   - Separate address info, status fields, etc. into their own tables if it’s distinct data.

2. **Navigation Properties**
   - Use `ICollection<T>` for 1:N relationships, and a single reference for the N:1 side.
   - Name foreign keys consistently: e.g. `public Guid ClientId { get; set; }`.

3. **Lazy vs Eager Loading**
   - Prefer **eager loading** (`.Include()`) or **explicit loading** for clarity.
   - If using **lazy loading**, mark navigation properties as `virtual` and enable proxies.

**Instruction:**  
- Keep relationships clean and explicit.  
- Avoid giant "god classes" – break out specialized entities as needed.

---

## 8. Deployment & Environment Setup

1. **appsettings.json**
   - Use this for local configuration, not for production secrets.
   - Have a `appsettings.Development.json` for dev overrides.

2. **Production Config**
   - Use environment variables or a secure secrets manager.
   - Consider Docker for consistent deployment.

**Instruction:**  
- Never check in production secrets.  

---

## 9. Testing

1. **Postman or Automated API Tests**
   - Validate endpoints, especially error handling and authentication flows.

**Instruction:**  
- Aim for coverage on critical logic in Services and Repositories.  
- Keep tests well-organized, e.g. a `Tests` project.

---

## 10. Final Words

- Keep controllers lean.
- Handle validation everywhere: ModelState, Services, Repos.
- Leverage Identity (or JWT) for auth.
- Practice consistent naming and separation of concerns.

**Remember:** A clean, secure, and maintainable WebAPI is built on good architecture, safe handling of user data, and a readiness to iterate and improve.
