import { ApplicantModel } from "../../entities/applicant.model";

export interface ApplicantResponseModel {
  count: number;
  pageIndex: number;
  pageSize: number;
  data: ApplicantModel[];
}