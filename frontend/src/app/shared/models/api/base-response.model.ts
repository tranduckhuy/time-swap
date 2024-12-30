export interface BaseResponseModel<T = null | undefined> {
  statusCode: string;
  data?: T;
  message: string;
}