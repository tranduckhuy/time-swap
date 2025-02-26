export interface ResetPasswordRequestModel {
  email: string;
  token: string;
  password: string;
  confirmPassword: string;
}
