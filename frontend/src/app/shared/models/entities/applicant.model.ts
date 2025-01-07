export interface ApplicantModel {
  // id: string;
  userId: string;
  jobPostId: string;
  fullName: string;
  avatarUrl: number;
  email: Date;
  appliedAt?: Date;
}