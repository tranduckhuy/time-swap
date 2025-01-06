export interface JobListRequestModel {
  search: string;
  minFee: number;
  maxFee: number;
  industryId: number;
  categoryId: number;
  cityId: string;
  wardId: string;
  postedDate: string;
  pageIndex?: number;
  pageSize?: number;
  isActive?: boolean;
}