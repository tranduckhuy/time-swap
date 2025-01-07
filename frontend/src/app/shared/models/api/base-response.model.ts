export interface BaseResponseModel<T = null | undefined> {
  statusCode: string | number;
  data?: T;
  message?: string;
}