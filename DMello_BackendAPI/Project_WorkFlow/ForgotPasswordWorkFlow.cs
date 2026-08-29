//             ** How Forgot Password Works 
  
//Step 1: User requests password reset (/forgot-password)

//Backend checks _userRepo.GetByEmailAsync(email).

//If user exists, generate a unique random token (e.g., GUID).

//Save token and expiry time in DB using _userRepo.SaveResetTokenAsync(...).

//Send an email to the user with a link: [https://myfrontend.com/reset-password?token=XYZ&email=abc@gmail.com](https://myfrontend.com/reset-password?token=XYZ&email=abc@gmail.com).

//Step 2: User clicks email link (Frontend Angular)

//Angular opens ResetPasswordComponent and reads token and email from URL parameters.

//User enters a new password.

//Step 3: User submits new password (/reset-password)

//Backend verifies the token matches and is not expired.

//Update password hash in DB using _userRepo.UpdatePasswordAsync(...).
