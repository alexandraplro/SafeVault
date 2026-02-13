# SafeVault – Secure Web Application (Activities 1–3)

This repository contains the completed SafeVault project for Activities 1, 2, and 3.  
The project demonstrates secure coding practices, authentication and authorization, SQL injection prevention, XSS mitigation, and debugging using Microsoft Copilot.

---

## 📌 Project Overview

SafeVault is a simple web application that accepts user input through a form and stores it in a database.  
Across Activities 1–3, the application was enhanced to:

- Validate and sanitize user input  
- Prevent SQL injection using parameterized queries  
- Prevent XSS through output encoding and validation  
- Implement authentication and authorization (JWT + RBAC)  
- Debug and resolve security vulnerabilities  
- Add tests for SQL injection and XSS  
- Use Copilot to generate secure code and assist in debugging  

This repository contains the final secure version of the application.

---

## 📁 Project Structure

SafeVault/
│
├── SafeVault.Web/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── RegisterController.cs
│   │   └── SubmitController.cs
│   │
│   ├── Services/
│   │   ├── IUserRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── TokenService.cs
│   │   ├── PasswordHasher.cs
│   │   └── InputValidator.cs
│   │
│   ├── Tests/
│   │   └── TestInputValidation.cs
│   │
│   ├── webform.html
│   ├── database.sql
│   ├── Program.cs
│   ├── appsettings.json
│   └── README.md


---

## 🔐 Security Features Implemented

### ✔ Input Validation
- Strict username validation  
- Email format validation  
- Sanitization to prevent XSS  
- Validation wrapped in try/catch to prevent unsafe values from reaching the database  

### ✔ SQL Injection Prevention
- All SQL queries use **parameterized queries**  
- No string concatenation  
- Repository pattern ensures consistent safe database access  

### ✔ XSS Mitigation
- No raw HTML output  
- JSON responses only  
- Unsafe characters rejected during validation  

### ✔ Authentication & Authorization (RBAC)
- `/auth/register` endpoint  
- `/auth/login` endpoint  
- Password hashing using a secure algorithm  
- JWT generation with:
  - `sub` (username)
  - `role` (admin/user)
  - `iss`, `aud`, `exp` claims  
- Role-based access support  

### ✔ Debugging & Vulnerability Resolution
- Fixed SQL injection vulnerabilities  
- Fixed XSS vulnerabilities  
- Fixed unsafe input handling  
- Fixed missing table creation  
- Improved repository architecture  

---

## 🧪 Testing

The project includes a testing framework with:

- SQL injection test placeholder  
- XSS test placeholder  
- Input validation tests  
- Structure for expanding automated security tests  

These tests demonstrate how Copilot was used to generate and scaffold security testing.

---

## 🗄 Database Schema

The assignment-required schema is included in:

database.sql


```sql
CREATE TABLE Users (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(100),
    Email VARCHAR(100)
);
```

The running application uses SQLite and automatically creates a compatible schema at runtime.

---

### 🚀 How to Run the Application

From the project root:

```
cd SafeVault.Web
dotnet run
```
The app will start at:

```
http://localhost:5078
```

---

### Test Registration:
```
curl -X POST http://localhost:5078/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"MyPass123!","role":"admin"}'
```

---

### Test Login:
```
curl -X POST http://localhost:5078/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"MyPass123!"}'
```

---

### Test Submit (assignment requirement):
```
curl -X POST http://localhost:5078/submit \
  -F "username=test" \
  -F "email=test@example.com"
```





