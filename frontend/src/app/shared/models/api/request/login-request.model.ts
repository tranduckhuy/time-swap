export interface LoginRequestModel {
  email: string;
  password: string;
}

export interface RefreshRequestModel {
  accessToken: string;
  refreshToken: string;
}