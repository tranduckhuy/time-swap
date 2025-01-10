import { WardModel } from "./location.model";
import { CategoryModel } from "./category.model";
import { IndustryModel } from "./industry.model";

export interface JobPostModel {
  id: string;
  userId: string;
  ownerAvatarUrl: string;
  ownerName: string;
  ownerEmail: string;
  ownerLocation: string;
  title: string;
  description: string;
  responsibilities: string;
  responsibilitiesList: string[];
  fee: number;
  startDate: Date;
  dueDate: Date;
  assignedTo?: string;
  isOwnerComplete?: boolean;
  isAssigneeComplete?: boolean;
  createdAt: Date;
  modifiedAt?: Date;
  industry: IndustryModel;
  category: CategoryModel;
  ward: WardModel;
}