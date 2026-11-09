# Heritage Reptile Supply — H60 Assignment 3

## Projects

| Project | Description |
| --- | --- |
| `THH60Services` | ASP.NET Core ReST API for products, categories, and customers |
| `THH60A03` | Store application (Identity, products, categories, and customers) |
| `THH60Customer` | Customer storefront (login, catalog browsing, and search) |

## Requirements

- .NET 10 SDK
- SQL Server with SQL Server Management Studio (SSMS)
- Visual Studio 2022

## Database configuration

All three projects use the connection string named `Assignment3DB`. Update it in:

- `THH60A03/appsettings.json`
- `THH60Services/appsettings.json`
- `THH60Customer/appsettings.json`

Example connection string (use your SSMS connection details):

```text
Server=localhost;Database=H60Assignment3DB_TH;User Id=sa;Password=YourPassword;TrustServerCertificate=True
```

## Services URL

`THH60A03` and `THH60Customer` call the ReST API at `http://localhost:7777/`. Change `ServiceSettings:BaseUrl` in their `appsettings.json` files if needed.

## Database setup

Use SSMS to create or connect to the `H60Assignment3DB_TH` database. Start `THH60Services` and `THH60A03` once from Visual Studio; they apply the EF Core migrations at startup, and the Store seeds the roles and test accounts. Start the Store before using the Customer app.

## Running

Start all three projects from Visual Studio using multiple startup projects.

| Application | URL |
| --- | --- |
| Services API / Swagger | `http://localhost:7777/swagger` |
| Store | `http://localhost:5555` |
| Customer storefront | `http://localhost:9999` |

## Test accounts

| Role | Email | Password |
| --- | --- | --- |
| Manager | `manager@heritagereptiles.ca` | `Manager123!` |
| Clerk | `clerk@heritagereptiles.ca` | `Clerk123!` |
| Customer (generic) | `customer@heritagereptiles.ca` | `Customer123!` |
| Thomas (linked customer) | `thomas@heritagereptiles.ca` | `Customer123!` |
| Alice (linked customer) | `alice@heritagereptiles.ca` | `Customer123!` |
| Bob (linked customer) | `bob@heritagereptiles.ca` | `Customer123!` |

Thomas, Alice, and Bob are seeded Customer rows linked to their Identity accounts.
`customer@` is a standalone customer login for testing the storefront.

- Store login: `http://localhost:5555/Identity/Account/Login`
- Customer login: `http://localhost:9999/Identity/Account/Login`

## Roles

- **Manager** — all Store screens, including product buy and sell prices.
- **Clerk** — all Store screens except changing buy and sell prices.
- **Customer** — Customer storefront only; Store screens return Access Denied.

## Features

- Products and categories are served by `THH60Services` through repositories and consumed by the Store and Customer apps over HTTP.
- Store customer management includes list, create, edit, and delete.
- Creating a customer in the Store creates the Identity login on the same screen and links it to the Customer row (Final Five option).
- The Customer app never displays buy prices; it shows sell prices only.
- The Customer catalog supports search and category filtering.
