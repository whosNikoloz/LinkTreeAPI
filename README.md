# LinkTreeAPI - API Documentation

Welcome to the official documentation for Your API Name! This guide will help you understand and utilize the various endpoints provided by the API.

## Table of Contents

- [Upload User Link](#upload-user-link)
- [User Links](#user-links)
- [User with Links](#user-with-links)
- [Get Link](#get-link)
- [Edit Link](#edit-link)
- [Delete Link](#delete-link)
- [Clear Link](#clear-link)
- [Users](#users)
- [User Name](#user-name)
- [User Registration](#user-registration)
- [Login with Email](#login-with-email)
- [Login with Username](#login-with-username)
- [Login with Phone Number](#login-with-phone-number)
- [Verify User](#verify-user)
- [Forgot Password](#forgot-password)
- [Reset Password](#reset-password)
- [Change Password](#change-password)
- [Change Username or Number](#change-username-or-number)
- [User Image](#user-image)

## Upload User Link

- Endpoint: `{{baseUrl}}/api/Link/UploadUserLink?user=<string>`
- Description: Uploads a user's link.
- Method: `POST`

## User Links

- Endpoint: `{{baseUrl}}/api/Link/UserLinks?user=<string>`
- Description: Retrieves links for a specific user.
- Method: `GET`

## User with Links

- Endpoint: `{{baseUrl}}/api/Link/UserWithLinks`
- Description: Retrieves users along with their links.
- Method: `GET`

## Get Link

- Endpoint: `{{baseUrl}}/api/Link/GetLink?user=<string>&id=<integer>`
- Description: Retrieves a specific link for a user.
- Method: `GET`

## Edit Link

- Endpoint: `{{baseUrl}}/api/Link/EditLink?username=<string>&linkid=<integer>`
- Description: Edits a specific link for a user.
- Method: `PUT`

## Delete Link

- Endpoint: `{{baseUrl}}/api/Link/DeleteLink?user=<string>&id=<integer>`
- Description: Deletes a specific link for a user.
- Method: `DELETE`

## Clear Link

- Endpoint: `{{baseUrl}}/api/Link/ClearLink?user=<string>`
- Description: Clears all links for a user.
- Method: `DELETE`

## Users

- Endpoint: `{{baseUrl}}/api/User/Users`
- Description: Retrieves a list of users.
- Method: `GET`

## User Name

- Endpoint: `{{baseUrl}}/api/User/UserName`
- Description: Retrieves the username for a user.
- Method: `GET`

## User Registration

- Endpoint: `{{baseUrl}}/api/User/register`
- Description: Registers a new user.
- Method: `POST`

## Login with Email

- Endpoint: `{{baseUrl}}/api/User/loginWithEmail`
- Description: User login using email.
- Method: `POST`

## Login with Username

- Endpoint: `{{baseUrl}}/api/User/loginWithUserName`
- Description: User login using username.
- Method: `POST`

## Login with Phone Number

- Endpoint: `{{baseUrl}}/api/User/loginWithPhoneNumber`
- Description: User login using phone number.
- Method: `POST`

## Verify User

- Endpoint: `{{baseUrl}}/api/User/verify?token=<string>&returnUrl=<string>`
- Description: Verifies a user using a token.
- Method: `GET`

## Forgot Password

- Endpoint: `{{baseUrl}}/api/User/Forgot-password`
- Description: Initiates the forgot password process.
- Method: `POST`

## Reset Password

- Endpoint: `{{baseUrl}}/api/User/Reset-password`
- Description: Resets the user's password.
- Method: `POST`

## Change Password

- Endpoint: `{{baseUrl}}/api/User/Change-password?newpassword=<string>&oldpassword=<string>`
- Description: Changes the user's password.
- Method: `PUT`

## Change Username or Number

- Endpoint: `{{baseUrl}}/api/User/Change-usernameornumber`
- Description: Changes the user's username or phone number.
- Method: `PUT`

## User Image

- Endpoint: `{{baseUrl}}/api/User/userimage`
- Description: Retrieves the user's image.
- Method: `GET`

---

Feel free to refer to this documentation for understanding and utilizing the various endpoints provided by Your API Name. Happy coding!
