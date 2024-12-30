import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';

import { TOAST_ANIMATION, type Toast } from '../../models/utils/toast.model';

import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [NgClass],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
  animations: TOAST_ANIMATION
})
export class ToastComponent implements OnInit {
  toasts: Toast[] = []

  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  ngOnInit(): void {
    const subscription = this.toastService.toast$.subscribe({
      next: (toasts) => this.toasts = toasts
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  removeToast(id: number) {
    this.toastService.remove(id);
  }
}
