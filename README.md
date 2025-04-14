# Ordering System

A simple ordering system built with ASP.NET Core Web API for the backend and HTML, CSS, JavaScript for the frontend.

## Features

### Customer:
- Register a new account or log in.
- View available products (added by the admin only).
- Place new orders.
- View all personal orders.
- Delete existing orders.

**Important:** If a customer deletes 3 or more orders on the same day they were created, they will be banned from placing new orders for 6 hours.  
This feature is implemented using a **middleware** in the backend.

### Admin:
- There is only one admin.
- The admin can add and update products (through the API only).
- has two sections
    #### Poducts
      - Create New Product
      - Edit Existing Product 
      - Delete Product 
      - View All Customers Who Added This Product to Their Orders

    #### Customers
      - View All Orders Placed by a Specific Customer

## Technology Stack

- Frontend: HTML, CSS, JavaScript
- Backend: ASP.NET Core Web API (.NET 6)
- Database: SQL Server
- Hosting:
  - Frontend: GitHub Pages
  - API: IIS (only over HTTPS)

## How to Run

### Backend (API):
- Open the `API` folder in Visual Studio.
- Apply migrations and update the database.
- Run the project on IIS with HTTPS enabled.

### Frontend:
- Located inside the `client/` folder.
- Hosted on GitHub Pages at:
  [https://ahmedelsaadany10.github.io/Ordering-System/](https://ahmedelsaadany10.github.io/Ordering-System/)

## Author

Ahmed El Saadany  
https://github.com/AhmedElsaadany10


