# Development Workflow Guide

## Project Manager's Feature Development Framework

### Time Allocation (Per Feature)

Based on your N-layer architecture, here's the recommended time allocation for a typical feature:

| Phase | Time % | Duration (Small Feature) | Duration (Medium Feature) | Duration (Large Feature) |
|-------|--------|-------------------------|--------------------------|-------------------------|
| **Planning & Design** | 15% | 1-2 hours | 3-4 hours | 1 day |
| **Backend Development** | 35% | 3-4 hours | 1-2 days | 2-3 days |
| **Frontend Development** | 30% | 2-3 hours | 1-2 days | 2-3 days |
| **Testing & Bug Fixes** | 15% | 1-2 hours | 3-4 hours | 1 day |
| **Documentation & Review** | 5% | 30 min - 1 hour | 1-2 hours | 2-3 hours |

**Small Feature Example**: Add a new field to existing entity, update UI
**Medium Feature Example**: New CRUD operations for new entity
**Large Feature Example**: Complex feature with multiple entities, file handling, complex business logic

---

## Standard Feature Development Workflow

### Phase 1: Planning & Design (Day 0 - Before coding)

**Time**: 15% of total feature time

**Tasks**:
1. **Define Requirements**
   - What problem does this solve?
   - Who will use this feature? (User, Dispatcher, Admin)
   - What are the acceptance criteria?

2. **Design Data Model**
   - What entities are needed/modified?
   - What relationships exist?
   - What DTOs are required?

3. **Design API Endpoints**
   - List all endpoints (GET, POST, PUT, DELETE)
   - Define request/response shapes
   - Identify authorization requirements

4. **Design UI Components**
   - Sketch page layouts
   - Identify reusable components
   - Plan routing changes

5. **Create Task Breakdown**
   - Break feature into small, testable tasks
   - Identify dependencies
   - Estimate each task

**Deliverable**: Written design document or checklist

---

### Phase 2: Backend Development (Layer by Layer)

**Time**: 35% of total feature time

Follow the **Bottom-Up** approach (Models → DAL → BLL → API):

#### Step 1: Models Layer (2-3 hours for medium feature)

**Location**: `src/DispatcherApp.Models/`

1. **Create/Update Entities** (`Entities/`)
   ```csharp
   // Example: src/DispatcherApp.Models/Entities/YourEntity.cs
   public class YourEntity
   {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; }
       // Navigation properties
   }
   ```

2. **Create DTOs** (`DTOs/YourFeature/`)
   - Request DTOs (for POST/PUT)
   - Response DTOs (for GET)
   - Keep them focused and specific

3. **Add Constants** (if needed) (`Constants/`)

4. **Add Custom Exceptions** (if needed) (`Exceptions/`)

**Checkpoint**: Build Models project - `dotnet build src/DispatcherApp.Models/DispatcherApp.Models.csproj`

#### Step 2: DAL Layer (2-4 hours for medium feature)

**Location**: `src/DispatcherApp.DAL/`

1. **Update DbContext** (`Data/AppDbContext.cs`)
   ```csharp
   public DbSet<YourEntity> YourEntities { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       // Configure entity
       modelBuilder.Entity<YourEntity>(entity =>
       {
           entity.HasKey(e => e.Id);
           entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
           // Configure relationships
       });
   }
   ```

2. **Create Migration**
   ```bash
   dotnet ef migrations add Add_YourFeature --project src/DispatcherApp.DAL --startup-project src/DispatcherApp.API
   ```

3. **Review Migration** - Check generated SQL in `Migrations/`

4. **Update Database**
   ```bash
   dotnet ef database update --project src/DispatcherApp.DAL --startup-project src/DispatcherApp.API
   ```

5. **Add Seed Data** (if needed) (`Seed/`)

**Checkpoint**: Verify database schema in SQL Server

#### Step 3: BLL Layer (3-5 hours for medium feature)

**Location**: `src/DispatcherApp.BLL/`

1. **Create Service Interface** (`Interfaces/IYourFeatureService.cs`)
   ```csharp
   public interface IYourFeatureService
   {
       Task<List<YourResponse>> GetListAsync();
       Task<YourResponse> GetByIdAsync(int id);
       Task<int> CreateAsync(YourRequest request);
       Task UpdateAsync(int id, YourRequest request);
       Task DeleteAsync(int id);
   }
   ```

2. **Implement Service** (`Services/YourFeatureService.cs`)
   - Inject `AppDbContext` and `IUserContextService`
   - Use Entity Framework directly (no repository pattern)
   - Apply business logic and validation
   - Use AutoMapper for DTO mapping

3. **Create AutoMapper Profile** (`Mapping/MappingProfile.cs`)
   ```csharp
   CreateMap<YourEntity, YourResponse>();
   CreateMap<YourRequest, YourEntity>();
   ```

4. **Register Service** (`DependencyInjection.cs`)
   ```csharp
   builder.Services.AddScoped<IYourFeatureService, YourFeatureService>();
   ```

**Checkpoint**: Build BLL project - `dotnet build src/DispatcherApp.BLL/DispatcherApp.BLL.csproj`

#### Step 4: API Layer (2-3 hours for medium feature)

**Location**: `src/DispatcherApp.API/`

1. **Create Controller** (`Controllers/YourFeatureController.cs`)
   ```csharp
   [Authorize]
   [Authorize(Roles = "Dispatcher,Administrator")] // Adjust as needed
   [Route("api/[controller]")]
   [ApiController]
   public class YourFeatureController(IYourFeatureService service) : ControllerBase
   {
       private readonly IYourFeatureService _service = service;

       [HttpGet]
       public async Task<ActionResult<List<YourResponse>>> GetAll()
       {
           var result = await _service.GetListAsync();
           return Ok(result);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<YourResponse>> GetById(int id)
       {
           var result = await _service.GetByIdAsync(id);
           return Ok(result);
       }

       [HttpPost]
       public async Task<ActionResult<int>> Create([FromBody] YourRequest request)
       {
           var id = await _service.CreateAsync(request);
           return CreatedAtAction(nameof(GetById), new { id }, id);
       }

       [HttpPut("{id}")]
       public async Task<IActionResult> Update(int id, [FromBody] YourRequest request)
       {
           await _service.UpdateAsync(id, request);
           return NoContent();
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> Delete(int id)
       {
           await _service.DeleteAsync(id);
           return NoContent();
       }
   }
   ```

2. **Test API with Swagger**
   - Run API: `dotnet run --project src/DispatcherApp.API/DispatcherApp.API.csproj`
   - Navigate to `https://localhost:<port>/api`
   - Test each endpoint manually

**Checkpoint**: All backend endpoints working via Swagger

---

### Phase 3: Frontend Development

**Time**: 30% of total feature time

**Location**: `src/frontend-app/src/`

#### Step 1: API Client Setup (1 hour)

1. **Regenerate NSwag Client** (if needed)
   ```bash
   # Build API project - NSwag should auto-generate
   dotnet build src/DispatcherApp.API/DispatcherApp.API.csproj
   ```

2. **Create API Service** (`api/`)
   - Use axios with the generated types
   - Add error handling

#### Step 2: Custom Hooks (2-3 hours)

**Location**: `src/frontend-app/src/components/hooks/`

```typescript
// useYourFeature.ts
import { useState, useEffect } from 'react';
import axios from 'axios';
import type { YourResponse, YourRequest } from '../api/generatedTypes';

export const useYourFeature = () => {
    const [items, setItems] = useState<YourResponse[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchItems = async () => {
        setLoading(true);
        try {
            const response = await axios.get<YourResponse[]>('/api/yourfeature');
            setItems(response.data);
        } catch (err) {
            setError('Failed to fetch items');
        } finally {
            setLoading(false);
        }
    };

    const createItem = async (request: YourRequest) => {
        // Implementation
    };

    useEffect(() => {
        fetchItems();
    }, []);

    return { items, loading, error, fetchItems, createItem };
};
```

#### Step 3: UI Components (3-5 hours)

1. **Reusable Components** (`components/ui/`)
   - Forms, inputs, buttons, cards
   - Keep them generic and reusable

2. **Page Components** (`components/dashboard/pages/`)
   ```typescript
   // YourFeature.tsx
   import { useYourFeature } from '../../hooks/useYourFeature';

   export default function YourFeature() {
       const { items, loading, error } = useYourFeature();

       if (loading) return <div>Loading...</div>;
       if (error) return <div>Error: {error}</div>;

       return (
           <div>
               {/* Your UI */}
           </div>
       );
   }
   ```

3. **Add Route** (`routes.tsx`)
   ```typescript
   {
       path: "your-feature",
       element: <YourFeature />,
   }
   ```

4. **Update Navigation** (`config/navigation.ts`)
   ```typescript
   { path: "/dashboard/your-feature", label: "Your Feature" }
   ```

**Checkpoint**: Feature accessible and functional in browser

---

### Phase 4: Testing & Quality Assurance

**Time**: 15% of total feature time

#### Backend Testing (2-3 hours)

1. **Create Unit Tests** (`test/DispatcherApp.Tests/`)
   ```csharp
   public class YourFeatureServiceTests
   {
       [Fact]
       public async Task GetListAsync_ReturnsItems()
       {
           // Arrange
           // Act
           // Assert
       }
   }
   ```

2. **Run Tests**
   ```bash
   dotnet test
   ```

3. **Manual API Testing**
   - Test all endpoints via Swagger
   - Test authorization scenarios
   - Test error cases

#### Frontend Testing (1-2 hours)

1. **Manual Browser Testing**
   - Test all user interactions
   - Test different screen sizes
   - Test error states

2. **Cross-browser Check** (Chrome, Edge)

3. **ESLint Check**
   ```bash
   npm run lint
   ```

---

### Phase 5: Documentation & Code Review

**Time**: 5% of total feature time

1. **Update CLAUDE.md** (if architecture changed)

2. **Update progress.md** (track what you built)

3. **Code Review Checklist**:
   - [ ] All endpoints have proper authorization
   - [ ] Error handling implemented
   - [ ] Input validation in place
   - [ ] No hardcoded values
   - [ ] AutoMapper profiles configured
   - [ ] TODOs removed or tracked
   - [ ] Frontend has loading/error states
   - [ ] Responsive design works
   - [ ] TypeScript types used properly

4. **Git Commit**
   ```bash
   git add .
   git commit -m "feat: add your feature with full CRUD operations"
   git push
   ```

---

## Daily Workflow

### Morning (Start of Development)
1. Pull latest changes: `git pull`
2. Review today's tasks
3. Start backend API if needed
4. Start frontend dev server if needed

### During Development
1. Commit frequently (after each layer/component)
2. Test incrementally (don't wait until the end)
3. Keep notes in `docs/progress.md`

### End of Day
1. Complete current task or reach a checkpoint
2. Commit all work
3. Update progress documentation
4. Plan tomorrow's tasks

---

## Priority Order for Incomplete Features

Based on your current codebase analysis:

### High Priority (Complete These First)
1. **Assignment CRUD** - Controllers have empty stubs (POST, PUT, DELETE)
2. **Tutorial CRUD** - Missing CREATE, UPDATE, DELETE endpoints
3. **Refresh Token Storage** - TODO in TokenService.cs
4. **User Management** - UserController is mostly empty

### Medium Priority
1. **File Management** - Enhance beyond tutorial files
2. **Profile Management** - Frontend exists but backend incomplete
3. **Settings Page** - Frontend exists but no backend

### Low Priority (Polish)
1. **Admin Panel** - Role management, user oversight
2. **Email Service** - Replace DummyEmailSender with real service
3. **Error Boundaries** - More comprehensive error handling

---

## Quick Reference: Command Cheat Sheet

```bash
# Backend Development
dotnet build                                                      # Build entire solution
dotnet run --project src/DispatcherApp.API/DispatcherApp.API.csproj  # Run API
dotnet test                                                       # Run tests
dotnet ef migrations add MigrationName --project src/DispatcherApp.DAL --startup-project src/DispatcherApp.API
dotnet ef database update --project src/DispatcherApp.DAL --startup-project src/DispatcherApp.API

# Frontend Development (from src/frontend-app/)
npm install                                                       # Install dependencies
npm run dev                                                       # Start dev server
npm run build                                                     # Build for production
npm run lint                                                      # Lint code
```

---

## Tips for Solo Development

1. **Don't over-engineer**: You're using N-layer which is appropriate. Avoid adding more complexity.

2. **Test as you go**: Don't leave testing for the end. Test each layer as you complete it.

3. **Document why, not what**: Code shows what. Comments/docs should explain why you made decisions.

4. **Consistent naming**: Follow C# conventions for backend, TypeScript conventions for frontend.

5. **One feature at a time**: Complete features fully before starting new ones.

6. **Use git branches**: Create feature branches, merge to main when complete.

7. **Take breaks**: After 2 hours of coding, take a 15-minute break to maintain focus.

---

## Example: Building "Assignment Create" Feature

**Total Time**: ~4-6 hours (Medium Feature)

### Planning (1 hour)
- Requirements: Dispatchers can create assignments for users
- Data: Title, description, due date, assigned users
- Endpoint: POST /api/assignment
- UI: Form in assignments page

### Backend (2-3 hours)
1. Models: `CreateAssignmentRequest` DTO (20 min)
2. DAL: Verify schema, add seed data (30 min)
3. BLL: `CreateAssignmentAsync` in AssignmentService (60 min)
4. API: Implement POST in AssignmentController (30 min)
5. Test via Swagger (20 min)

### Frontend (1-2 hours)
1. Update useAssignments hook with create function (30 min)
2. Create AssignmentForm component (45 min)
3. Integrate into Assignments page (30 min)
4. Test in browser (15 min)

### Testing & Docs (30 min)
1. Unit test for service (15 min)
2. Update progress.md (5 min)
3. Git commit (10 min)

**Total**: ~5 hours actual work time
