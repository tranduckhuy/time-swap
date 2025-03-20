export interface ApplicantsRequestModel {
  search: string;
  industryId: number;
  categoryId: number;
  cityId: string;
  wardId: string;
  pageIndex?: number;
  pageSize?: number;
  isActive?: boolean;
}
