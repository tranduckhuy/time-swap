import { CategoryModel } from './category.model';
import { IndustryModel } from './industry.model';
import { CityModel, WardModel } from './location.model';

export interface UserModel {
  id: string;
  fullName: string;
  email: string;
  phoneNumber?: string;
  city: CityModel;
  ward: WardModel;
  fullLocation?: string;
  avatarUrl: string;
  description?: string;
  balance: number;
  educationHistory: string[];
  majorCategory: CategoryModel;
  majorIndustry: IndustryModel;
  subscriptionPlan: number;
  subscriptionExpiryDate?: string;
  role: string[];
}

export interface UserUpdateModel {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  description?: string;
  avatarUrl?: string;
  cityId?: string;
  wardId?: string;
  educationHistory?: [string];
  majorCategoryId?: number;
  majorIndustryId?: number;
}
