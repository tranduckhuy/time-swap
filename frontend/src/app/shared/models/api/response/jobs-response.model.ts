import { JobPostModel } from '../../entities/job.model';

export interface JobsResponseModel {
  count: number;
  pageIndex: number;
  pageSize: number;
  data: JobPostModel[];
}

export interface JobDetailResponseModel extends JobPostModel {
  isCurrentUserApplied: boolean;
  totalApplicants: number;
  relatedJobPosts: JobPostModel[];
}
