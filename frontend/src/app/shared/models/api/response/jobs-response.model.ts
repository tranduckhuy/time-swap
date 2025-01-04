import { JobPostModel } from "../../entities/job.model";

export interface JobsResponseModel {
  count: number;
  pageIndex: number;
  pageSize: number;
  data: JobPostModel[];
}