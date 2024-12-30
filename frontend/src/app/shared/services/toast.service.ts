import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';

import type { Toast } from '../models/utils/toast.model';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toasts: Toast[] = [];
  private toastSubject = new BehaviorSubject<Toast[]>([]);
  toast$ = this.toastSubject.asObservable();
  
  private show(title: string, message: string, type: 'success' | 'error' | 'warn' | 'info', icon: 'fa fa-check-circle' | 'fa fa-exclamation-circle' | 'fa fa-info-circle') {
    const id = Date.now();
    const toast: Toast = {id, title, message, type, icon};
    
    this.toasts.push(toast);
    this.toastSubject.next(this.toasts);
    setTimeout(() => this.remove(id), 5000);

    return id;
  }

  remove(id: number) {
    this.toasts = this.toasts.filter(t => t.id !== id);
    this.toastSubject.next(this.toasts);
  }

  success(title: string, message: string): number {
    return this.show(title, message, 'success', 'fa fa-check-circle');
  }

  error(title: string, message: string): number {
    return this.show(title, message, 'error', 'fa fa-exclamation-circle');
  }

  warn(title: string, message: string): number {
    return this.show(title, message, 'warn', 'fa fa-exclamation-circle');
  }

  info(title: string, message: string): number {
    return this.show(title, message, 'info', 'fa fa-info-circle');
  }
}
