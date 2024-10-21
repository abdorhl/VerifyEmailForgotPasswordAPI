# VerifyEmailForgotPasswordAPI

This project provides a basic API for verifying email addresses and handling password reset functionality. It's built with ASP.NET Core, enabling users to sign up, verify their email addresses, and reset their passwords if they forget them.

## Features

- **User Registration**: Allows users to create accounts with an email verification process.
- **Email Verification**: After signing up, users receive a verification email to confirm their address.
- **Forgot Password**: Users can request a password reset if they forget their password, receiving an email with reset instructions.
- **Reset Password**: Users can reset their password using a token provided in the password reset email.

## Technologies Used

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**: For database management.
- **SQL Server**: The database for storing user information.
- **Identity**: For managing user authentication and authorization.


## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/abdorhl/VerifyEmailForgotPasswordAPI.git
   ```

2. Navigate to the project directory:

   ```bash
   cd VerifyEmailForgotPasswordAPI
   ```

3. Restore NuGet packages:

   ```bash
   dotnet restore
   ```

4. Set up your database by applying migrations:

   ```bash
   dotnet ef database update
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

## Usage

- **Register a new user**: 
  Send a `POST` request to `/api/auth/register` with user details.
  
- **Verify email**: 
  After registration, an email is sent with a verification link. The user needs to click the link to verify their account.

- **Forgot password**: 
  Send a `POST` request to `/api/auth/forgot-password` with the registered email.

- **Reset password**: 
  Use the link sent in the password reset email to reset the user's password.

## API Endpoints

- `POST /api/User/register`: Register a new user.
- `POST /api/User/LOGIN`: login a user.
- `POST /api/User/verify`: Verify the user's email.
- `POST /api/User/forgot-password`: Initiate a password reset.
- `POST /api/User/reset-password`: Reset the user's password.

## Contributing

Feel free to contribute by creating issues or submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
