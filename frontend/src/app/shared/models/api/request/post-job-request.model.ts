export interface PostJobRequestModel {
  title: string;
  fee: number;
  responsibilities: string;
  description: string;
  industryId: number;
  categoryId: number;
  cityId: string;
  wardId: string;
  startDate: Date;
  dueDate: Date;
}