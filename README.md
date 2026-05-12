# Healthcare RCM Platform

> **ASP.NET Core · Angular · SQL Server · OAuth 2.0 · SSRS · Serilog**

Full-stack Healthcare Revenue Cycle Management (RCM) platform — owns the complete API layer covering claims submission, eligibility validation, denial tracking, and payment reconciliation in a single traceable transaction flow.

**Outcome:** ~20% faster claims processing | 60% reduction in active dataset | Audit findings resolved

---

## 🏗️ Architecture

```
Angular UI (Frontend)
  - Reactive forms for claims submission
  - Route guards for role-based navigation
  - Reusable table components for high-volume billing data
        │
        ▼
ASP.NET Core Web API
  - Claims submission endpoint
  - Eligibility validation service
  - Denial tracking & appeals workflow
  - Payment reconciliation
  - OAuth 2.0 + RBAC middleware (Billing / Reviewer / Admin)
  - Serilog structured logging + exception middleware
        │
        ▼
SQL Server
  - Stored procedures with set-based JOIN logic
  - Composite indexes on high-frequency queries
  - Read-optimized views for SSRS reporting (NOLOCK)
  - Claims archive table (records > 12 months)
```

---

## ✨ Key Features

- **Full RCM flow** — claims submission → eligibility → denial → payment reconciliation
- **OAuth 2.0 RBAC** — Billing, Reviewer, Admin roles enforced at middleware level
- **~20% faster processing** — rewrote nested SELECT loops with set-based JOIN + composite indexes
- **60% DB size reduction** — archiving pipeline moved 8M+ rows to separate archive table
- **Zero table locking** — SSRS reports migrated to read-optimized views with NOLOCK hints
- **Structured logging** — Serilog exception middleware cut incident MTTD from hours to minutes
- **Audit compliant** — resolved internal audit finding of excessive data access

---

## 📊 Impact Metrics

| Metric | Before | After |
|--------|--------|-------|
| Claims processing time | Baseline | ~20% faster |
| Active table size | 8M+ rows | 60% reduction |
| Month-end table locking | Frequent | Eliminated |
| Incident diagnosis time | Hours | Minutes |
| Data access audit finding | Open | Resolved |

---

## 🗂️ Project Structure

```
healthcare-rcm-platform/
├── src/
│   ├── Controllers/
│   │   ├── ClaimsController.cs
│   │   ├── EligibilityController.cs
│   │   ├── DenialController.cs
│   │   └── PaymentController.cs
│   ├── Services/
│   │   ├── ClaimsService.cs
│   │   ├── EligibilityService.cs
│   │   └── ArchiveService.cs
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   └── RbacAuthorizationMiddleware.cs
│   ├── Models/
│   │   ├── Claim.cs
│   │   ├── EligibilityRequest.cs
│   │   └── DenialRecord.cs
│   └── Program.cs
├── frontend/                    # Angular application
│   ├── src/app/
│   │   ├── claims/
│   │   ├── eligibility/
│   │   └── shared/
│   └── package.json
├── database/
│   ├── stored-procedures/
│   ├── indexes/
│   ├── views/                   # SSRS read-optimized views
│   └── archive/                 # Archive pipeline scripts
└── README.md
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular v8–v13, TypeScript |
| Backend | ASP.NET Core Web API (C#) |
| Auth | OAuth 2.0, JWT, RBAC |
| ORM/Data | ADO.NET, Stored Procedures |
| Database | MS SQL Server |
| Reporting | SSRS (read-optimized views) |
| Logging | Serilog |
| API Docs | Swagger / OpenAPI |

---

*Built as the core backend platform at Newport Med India (May 2020 – Feb 2024).*
