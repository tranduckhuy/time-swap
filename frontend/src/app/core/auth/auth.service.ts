import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { REFRESH_TOKEN_KEY, TOKEN_KEY } from '../../shared/constants/auth-constants';

import { LoginRequestModel } from '../../shared/models/request/login-request.model';
import { LoginResponseModel } from '../../shared/models/response/login-response.model';
import { RegisterRequestModel } from '../../shared/models/request/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient);

  LOGIN_API_URL = 'accounts/login';
  REGISTER_API_URL = 'accounts/register';

  signin(loginReq: LoginRequestModel): Observable<LoginResponseModel> {
    const body = JSON.stringify(loginReq);
    return this.httpClient.post<LoginResponseModel>(`${environment.apiBaseUrl}/${this.LOGIN_API_URL}`, body, {
      headers: {
        'Content-Type' : 'application/json'
      }
    });
  }

  register(registerReq: RegisterRequestModel) {
    const body = JSON.stringify(registerReq);
    return this.httpClient.post(`${environment.apiBaseUrl}/${this.REGISTER_API_URL}`, body, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  saveToken(token: string, refreshToken: string) {
    localStorage.setItem(TOKEN_KEY, token);  
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);  
  }

  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
  }

  getClaims(){
   return JSON.parse(window.atob(this.getToken()!.split('.')[1]))
  }
}
