# Performance Comparison: IEnumerable vs IQueryable Filtering

## Overview

This document compares two approaches to filtering student data in an ASP.NET Core Web API:
- **IEnumerable**: Loads all data into memory first, then filters using LINQ to Objects
- **IQueryable**: Builds an expression tree that gets translated to SQL and executed at the database level

## API Endpoints

### Base URL
```
GET /api/Student/filtered
```

### Query Parameters
- `minAge` (optional): Minimum age filter
- `maxAge` (optional): Maximum age filter  
- `address` (optional): Address substring search (case-insensitive)
- `method` (optional): `"ienumerable"` or `"iqueryable"` (default: `"iqueryable"`)

### Specific Endpoints
- `GET /api/Student/filtered?method=iqueryable` - Uses IQueryable (database-level filtering)
- `GET /api/Student/filtered?method=ienumerable` - Uses IEnumerable (in-memory filtering)
- `GET /api/Student/filtered` - Unified endpoint (defaults to IQueryable)

## Implementation Details

### Student Entity
```csharp
public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
}
```

### Age Calculation
Age is computed from `DateOfBirth`:
```csharp
private static int CalculateAge(DateTime dateOfBirth)
{
    var today = DateTime.Today;
    var age = today.Year - dateOfBirth.Year;
    if (dateOfBirth.Date > today.AddYears(-age)) age--;
    return age;
}
```

### Response DTO
Only required fields are returned:
```csharp
public class StudentResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
}
```

## Test Results (10 records in database)

| Test Case | Method | Execution Time (ms) | Record Count | Notes |
|-----------|--------|-------------------|--------------|-------|
| Get all students | IQueryable | 863 ms | 10 | First query includes EF Core context initialization overhead |
| Get all students | IEnumerable | 60 ms | 10 | Loads all 10 records and filters in memory |
| Filter: address='st' | IQueryable | 80 ms | 5 | Case-insensitive, matches "St" in street names and "st" in city names |
| Filter: address='st' | IEnumerable | 1 ms | 5 | Very fast for small in-memory collection |
| Filter: minAge=25 | IQueryable | 5 ms | 8 | Database-level age calculation via date range |
| Filter: minAge=25 | IEnumerable | 1 ms | 8 | In-memory filtering |
| Combined: age 20-30 + address='st' | IQueryable | 5 ms | 4 | Multiple filters combined |
| Combined: age 20-30 + address='st' | IEnumerable | 1 ms | 4 | In-memory filtering |

## Analysis

### When IEnumerable is Faster (Small Datasets)
With only 10 records, IEnumerable outperforms IQueryable for simple "get all" operations (60ms vs 863ms). This is because:
1. **IQueryable overhead**: EF Core must translate the query to SQL, create parameters, and execute the command
2. **Database round-trip**: Even with local PostgreSQL, there's network/connection overhead
3. **Materialization**: IQueryable needs to materialize entities and map to DTOs
4. **Cold start**: The first IQueryable query includes context initialization

For tiny datasets, loading everything into memory and filtering there is more efficient.

### When IQueryable is Better (Large Datasets)
With larger datasets (thousands+ records), IQueryable becomes superior because:
1. **Reduced data transfer**: Only matching records are sent over the network
2. **Database optimization**: SQL Server/PostgreSQL uses indexes and query optimization
3. **Memory efficiency**: Application doesn't need to hold entire table in memory
4. **Scalability**: Performance remains consistent as data grows

### Example: 1,000,000 records
- **IEnumerable**: Loads 1M records into memory → OOM or very slow
- **IQueryable**: Retrieves only matching records (e.g., 1000) → fast and efficient

## Key Differences

| Aspect | IEnumerable | IQueryable |
|--------|-------------|------------|
| **Execution** | In-memory, after data retrieval | Database-level, before data retrieval |
| **Data Transfer** | Entire table transferred | Only filtered records transferred |
| **Performance (Small Data)** | Faster (less overhead) | Slower (SQL translation overhead) |
| **Performance (Large Data)** | Slower (loads everything) | Faster (filters at source) |
| **Memory Usage** | High (entire table) | Low (only results) |
| **Case Sensitivity** | Configurable via StringComparison | Database-dependent (made case-insensitive via ToLower()) |
| **Query Translation** | LINQ to Objects | LINQ to SQL/Entity Framework |

## Recommendations

### Use IEnumerable when:
- Dataset is small (< 1000 records) and fits comfortably in memory
- You need complex in-memory operations that are difficult to translate to SQL
- You're working with already materialized collections
- Simplicity is more important than optimization

### Use IQueryable when:
- Dataset is large (thousands+ records)
- You need pagination (Skip/Take)
- You want to leverage database indexes
- Network bandwidth is a concern
- You need consistent performance at scale

## Best Practice: Defer Execution

Both approaches use deferred execution, but the key difference is **where** the execution happens:
- **IEnumerable**: Execution happens in the application's memory space
- **IQueryable**: Execution happens in the database engine

## Code Location

- **Controller**: [`Controllers/StudentController.cs`](Controllers/StudentController.cs)
- **IEnumerable method**: `GetFilteredStudentsIEnumerable()` (line ~120)
- **IQueryable method**: `GetFilteredStudentsIQueryable()` (line ~150)
- **Unified endpoint**: `GetFilteredStudents()` (line ~200)

## Testing

### Using Swagger UI (with JWT Authentication)

1. Navigate to: `http://localhost:5150/swagger`
2. Click the **"Authorize"** button (top right)
3. Enter: `Bearer YOUR_JWT_TOKEN` (without quotes)
   - Example: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
4. Click "Authorize" → "Close"
5. Now you can call protected endpoints like `/api/Course`

**How to get a JWT token:**
```bash
# Register (if needed)
curl -X POST http://localhost:5150/api/Auth/register-admin \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin123!","firstName":"Admin","lastName":"User","phone":"123-456-7890"}'

# Login
curl -X POST http://localhost:5150/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin123!"}'
```
Response: `{"success":true,"token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."}`

### Using VS Code REST Client
Use the provided [`sem2week1.http`](sem2week1.http) file.

### Using curl
```bash
# IQueryable (public - no auth needed)
curl "http://localhost:5150/api/Student/filtered?minAge=25&method=iqueryable"

# IEnumerable (public - no auth needed)
curl "http://localhost:5150/api/Student/filtered?minAge=25&method=ienumerable"

# Protected Course endpoint (requires auth)
TOKEN="YOUR_TOKEN_HERE"
curl -H "Authorization: Bearer $TOKEN" "http://localhost:5150/api/Course"
```

## Conclusion

For this specific application with only 10 test records, **IEnumerable appears faster** due to minimal overhead. However, in production with thousands of student records, **IQueryable would be the clear winner** for performance, scalability, and resource efficiency.

The implementation demonstrates both approaches, allowing you to choose based on your actual data volume and requirements.
