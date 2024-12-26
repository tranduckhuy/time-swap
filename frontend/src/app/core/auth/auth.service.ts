import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { REFRESH_TOKEN_KEY, TOKEN_KEY } from '../../shared/constants/auth-constants';

import { AuthRequestModel } from '../../shared/models/request/auth-request.model';
import { AuthResponseModel } from '../../shared/models/response/auth-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient);

  LOGIN_API_URL = 'accounts/login';

  signin(authReq: AuthRequestModel): Observable<AuthResponseModel> {
    const body = JSON.stringify(authReq);
    return this.httpClient.post<AuthResponseModel>(`${environment.apiBaseUrl}/${this.LOGIN_API_URL}`, body, {
      headers: {
        'Content-Type' : 'application/json'
      }
    });
  }

  isLoggedIn() {
    return this.getToken() != null ? true : false;
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
