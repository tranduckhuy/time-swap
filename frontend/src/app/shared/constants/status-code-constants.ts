// * =========================== SUCCESS =========================== */
// ? Common Code
export const SUCCESS_CODE = 1000;

// ? Auth Code
export const REGISTER_CONFIRM_SUCCESS_CODE = 1001;
export const LOGIN_SUCCESS_CODE = 1002;
export const FORGOT_PASSWORD_SUCCESS_CODE = 1003;
export const RESET_PASSWORD_SUCCESS_CODE = 1004;

// * =========================== ERROR =========================== */
// ? Common Code
export const ERROR_CODE = 2000;

// ? Auth Code
export const EMAIL_EXIST_CODE = 2002;
export const REGISTER_FAILED_CODE = 2003;
export const USER_NOT_EXIST_CODE = 2004;
export const NOT_CONFIRM_CODE = 2005;
export const INVALID_CREDENTIAL_CODE = 2006;
export const INVALID_TOKEN = 2007;
export const TOKEN_EXPIRED_CODE = 2008;

// ? Jobs Code
export const USER_NOT_ENOUGH_BALANCE = 2023;
export const DUE_DATE_START_FAILED = 2024;
export const DUE_DATE_CURRENT_FAILED = 2025;
export const FEE_GREATER_THAN_FIFTY = 2026;

// ? Payment Code
export const PAYMENT_TIMEOUT = 2042;
export const CANCEL_TRANSACTION = 2045;
export const NOT_ENOUGH_BALANCE = 2046;
export const UNDEFINED_ERROR = 2050;
