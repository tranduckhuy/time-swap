export interface PaymentRequestModel {
  amount: number;
  paymentContent: string;
  paymentMethodId: number;
}

export interface VnPayReturnRequestModel {
  vnp_Amount: number;
  vnp_BankCode: string;
  vnp_BankTranNo: string;
  vnp_CardType: string;
  vnp_OrderInfo: string;
  vnp_PayDate: string;
  vnp_ResponseCode: string;
  vnp_TmnCode: string;
  vnp_TransactionNo: string;
  vnp_TransactionStatus: string;
  vnp_TxnRef: string;
  vnp_SecureHash: string;
}

export interface PayOsReturnRequestModel {
  status: string;
  code: string;
  id: string;
  orderCode: string;
  cancel: boolean;
}

export interface SubscriptionPlanRequestModel {
  subscriptionPlan: number;
}
