# Travel and Accommodation Booking Platform 

The **Travel and Accommodation Booking Platform** is a robust ASP.NET Core-based API that facilitates the booking of accommodation services. It provides endpoints for users to search, book, and manage accommodations. This app is designed to be scalable, secure, and easy to integrate with other services.

## Key Features
- **User Management**: Secure login and registration with JWT-based authentication.
  
- **Admin Management**: Admin users can manage cities, hotels, and rooms.
  
- **Search for Hotels**: Users can search for available hotels based on the city, hotel name and more.
  
- **Booking Management**: Users can create, view, checkout, and cancel bookings.
  
- **Email Notifications**: Automated email notifications are sent to users after they complete a booking, including an invoice. Additionally, notifications are sent to inform users if the payment process was successful or failed.
  
- **Image Management**: Images for cities, hotels, and rooms are stored in an S3 bucket. Admins have the ability to add, remove, or update these images via the API, ensuring the platform always has up-to-date visuals.
  
- **Trending Cities**: Frequently searched cities are cached using Redis for fast retrieval.
  
- **Secure Payments**: Stripe is integrated to handle secure payments during the booking process.
  
- **Logging**: All logs are managed by Serilog and stored in AWS CloudWatch for monitoring.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/downloads/) (for caching)
- [AWS CLI](https://aws.amazon.com/cli/) (for S3 and CloudWatch integration)
- [ngrok](https://ngrok.com/download) (A Tunneling Service for Local Development)
- [Postman](https://www.postman.com/) (for testing the API)

### Installation Steps
1. Clone the repository:
   ```bash
    git clone https://github.com/Yousef301/fts_travel_and_accommodation_booking_platform.git
    cd fts_final_project
    ```
   
2. Restore dependencies:
    ```bash
    dotnet restore
    ```

3. Set up the databases:
    - Update the connection strings in `appsettings.json` to point to your SQL Server and Redis instances.
    - Apply migrations to create the database schema:
      ```bash
      dotnet ef database update
      ```

4. Run the project:
    ```bash
    dotnet run
    ```
    
## Usage

Below are some of the available controllers and their endpoints:

| **Controller**            | **Endpoint**                                                  | **HTTP Method** | **Description**                                                                                      |
|-------------------------  |---------------------------------------------------------------|-----------------|------------------------------------------------------------------------------------------------------|
| **AuthController**        | `/api/v{v:apiVersion}/auth/login`                             | `POST`          | Authenticates a user and returns a JWT.                                                              |
|                           | `/api/v{v:apiVersion}/auth/register`                          | `POST`          | Registers a new user.                                                                                |
| **BookingsController**    | `/api/v{v:apiVersion}/user/bookings`                          | `GET`           | Retrieves a list of bookings for the currently authenticated user.                                   |
|                           | `/api/v{v:apiVersion}/user/bookings/{id}`                     | `GET`           | Retrieves a specific booking by its ID for the currently authenticated user.                         |
|                           | `/api/v{v:apiVersion}/user/bookings`                          | `POST`          | Create a booking.                                                                                    |
|                           | `/api/v{v:apiVersion}/user/bookings/{id}/cancel`              | `PATCH`         | Cancels a specific booking by its ID for the currently authenticated user.                           |
|                           | `/api/v{v:apiVersion}/user/bookings/{id}/checkout`            | `POST`          | Initiates the checkout process for a specific booking by its ID for the currently authenticated user |
| **HotelsController**      | `/api/v{v:apiVersion}/hotels`                                 | `GET `          | Retrieves a list of hotels for administrative purposes based on filter parameters.                   |
|                           | `/api/v{v:apiVersion}/hotels`                                 | `POST`          | Creates a new hotel.                                                                                 |
|                           | `/api/v{v:apiVersion}/hotels/search`                          | `GET`           | Searches for hotels based on user-specified criteria.                                                |
|                           | `/api/v{v:apiVersion}/hotels/featured-deals`                  | `GET`           | Retrieves a list of hotels that has featured deals.                                                  |
|                           | `/api/v{v:apiVersion}/hotels`                                 | `DELETE`        | Deletes a hotel by its ID.                                                                           |
|                           | `/api/v{v:apiVersion}/hotels`                                 | `PATCH`         | Updates a hotel by its ID using a JSON Patch document.                                               |
| **HotelImagesController** | `/api/v{v:apiVersion}/hotels/{hotelId}/images`                | `GET `          | Retrieves images for a specific hotel.                                                               |
|                           | `/api/v{v:apiVersion}/hotels/{hotelId}/images/{id}`           | `GET`           | Retrieves a specific image by its unique identifier.                                                 |
|                           | `/api/v{v:apiVersion}/hotels/{hotelId}/images`                | `POST`          | Uploads images for a specific hotel.                                                                 |
|                           | `/api/v{v:apiVersion}/hotels/{hotelId}/images/thumbnail`      | `GET`           | Retrieves the thumbnail image for a specific hotel.                                                  |
|                           | `/api/v{v:apiVersion}/hotels/{hotelId}/images/thumbnail`      | `POST`          | Uploads a thumbnail image for a specific hotel.                                                      |
|                           | `/api/v{v:apiVersion}/hotels/{hotelId}/images/{id}`           | `DELETE`        | Deletes a specific image for a hotel.                                                                |
| **InvoicesController**    | `/api/v{v:apiVersion}/user/bookings/{bookingId:guid}/invoice` | `GET`           | Retrieves the invoice for a specific booking as a PDF file.                                          |
| **PaymentsController**    | `/api/v{v:apiVersion}/user/payments`                          | `GET`           | Retrieves a list of payments for the currently authenticated user.                                   |
|                           | `/api/v{v:apiVersion}/user/payments/webhook`                  | `POST`          | Handles Stripe webhook events.                                                                       |
| **UsersController**       | `/api/v{v:apiVersion}/user/recently-visited`                  | `GET`           | Retrieves admin dashboard data.                                                                      |

And the list goes on...

## Architecture
A 3-tier architecture used to organize the application into three distinct layers:

1. Presentation Layer
    - **Purpose**: Manages user interactions and displays information.
    - **Components**: ASP.NET Core MVC Controllers, API Endpoints, and Models.
  
3. Application Layer
    - **Purpose**: Handles business logic and application workflows.
    - **Components**: Services, MediatR for commands and queries.
  
2. Data Layer
    - **Purpose**: Manages data access and persistence.
    - **Components**: Entity Framework Core for ORM, Repositories, and Unit of Work for transactions.

## API Versioning
For managing and maintaining API compatibility, we use the Asp.Versioning.Http and Asp.Versioning.Mvc.ApiExplorer libraries with a focus on URL-based versioning.

   - **Asp.Versioning.Http**: Implements API versioning through URLs, allowing you to specify different versions directly in the API path (e.g., /api/v1/resource).
   - **Asp.Versioning.Mvc.ApiExplorer**: Works with ASP.NET Core's API Explorer to accurately reflect and document these URL-based versions.

This approach simplifies the process of evolving APIs while ensuring clear version management and up-to-date documentation.

## External Services
   - **Stripe**: Used for handling payments securely.
   - **Redis**: Used to cache frequently searched cities for faster retrieval.
   - **AWS S3**: Used to store images related to cities, hotels, and rooms.
   - **AWS CloudWatch**: Used to log application activities for monitoring and debugging.
   - **AWS Secrets Manager**: Used to store connection strings and secret keys.

## Configurations
The application settings are managed through an external service called 'Secrets Manager,' which is an AWS service.
    
## Documentation
For detailed API documentation, you can access the Swagger UI after running the project:
   
   - **Swagger UI**: `https://localhost:7133/swagger`

