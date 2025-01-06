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
  
  /**
   * Creates and displays a new toast notification.
   * 
   * @param title - The toast notification title.
   * @param message - The toast notification message.
   * @param type - The type of notification ('success' | 'error' | 'warn' | 'info').
   * @param icon - The Font Awesome icon class to display.
   * @returns The unique ID of the created toast.
   * @private
   */
  private show(title: string, message: string, type: 'success' | 'error' | 'warn' | 'info', icon: 'fa fa-check-circle' | 'fa fa-exclamation-circle' | 'fa fa-info-circle') {
    const id = Date.now();
    const toast: Toast = {id, title, message, type, icon};
    
    this.toasts.push(toast);
    this.toastSubject.next(this.toasts);
    setTimeout(() => this.remove(id), 6000);

    return id;
  }

  /**
   * Removes a toast notification by its ID.
   * 
   * @param id - The unique ID of the toast to remove.
   */
  remove(id: number) {
    this.toasts = this.toasts.filter(t => t.id !== id);
    this.toastSubject.next(this.toasts);
  }

  /**
   * Displays a success toast notification.
   * 
   * @param title - The toast notification title.
   * @param message - The toast notification message.
   * @returns The unique ID of the created toast.
   */
  success(title: string, message: string): number {
    return this.show(title, message, 'success', 'fa fa-check-circle');
  }

  /**
   * Displays a error toast notification.
   * 
   * @param title - The toast notification title.
   * @param message - The toast notification message.
   * @returns The unique ID of the created toast.
   */
  error(title: string, message: string): number {
    return this.show(title, message, 'error', 'fa fa-exclamation-circle');
  }

  /**
   * Displays a warning toast notification.
   * 
   * @param title - The toast notification title.
   * @param message - The toast notification message.
   * @returns The unique ID of the created toast.
   */
  warn(title: string, message: string): number {
    return this.show(title, message, 'warn', 'fa fa-exclamation-circle');
  }

  /**
   * Displays a information toast notification.
   * 
   * @param title - The toast notification title.
   * @param message - The toast notification message.
   * @returns The unique ID of the created toast.
   */
  info(title: string, message: string): number {
    return this.show(title, message, 'info', 'fa fa-info-circle');
  }
}
