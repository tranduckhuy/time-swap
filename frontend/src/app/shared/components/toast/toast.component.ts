import { Component, inject } from '@angular/core';
import { NgClass } from '@angular/common';

import { TOAST_ANIMATION } from '../../models/utils/toast.model';

import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [NgClass],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
  animations: TOAST_ANIMATION
})
export class ToastComponent {
  private toastService = inject(ToastService);

  toasts = this.toastService.toasts;

  removeToast(id: number) {
    this.toastService.remove(id);
  }
}
