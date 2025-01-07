export interface UserModel {
  id: string;
  fullName: string;
  email: string;
  phoneNumber?: string;
  fullLocation?: string;
  avatarUrl: string;
  description?: string;
  balance: number;
  subscriptionPlan: number;
  subscriptionExpiryDate?: string;
  role: string[]; 
}