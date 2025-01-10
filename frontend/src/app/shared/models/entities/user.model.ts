export interface UserModel {
  id: string;
  fullName: string;
  email: string;
  phoneNumber?: string;
  fullLocation?: string;
  avatarUrl: string;
  description?: string;
  balance: number ;
  educationHistory: string[];
  majorCategory: string;
  majorIndustry: string;
  subscriptionPlan: number;
  subscriptionExpiryDate?: string;
  role: string[]; 
}
export interface UserUpdateModel {
  firstName?: string,
  lastName?: string,
  phoneNumber?: string,
  description?: string,
  avatarUrl?: string,
  cityId?: string,
  wardId?: string,
  educationHistory?: [
    string
  ],
  majorCategoryId?: number,
  majorIndustryId?: number
}