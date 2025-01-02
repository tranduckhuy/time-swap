export interface ConfirmRequestModel {
  token: string;
  email: string;
}

export interface ReConfirmRequestModel {
  email: string;
  clientUrl: string;
}