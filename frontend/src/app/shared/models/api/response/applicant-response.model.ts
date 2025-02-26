import { ApplicantModel } from '../../entities/applicant.model';
import { UserModel } from '../../entities/user.model';

export interface ApplicantResponseModel {
  count: number;
  pageIndex: number;
  pageSize: number;
  data: ApplicantModel[];
}
