import { animate, keyframes, style, transition, trigger } from "@angular/animations";

export interface Toast {
  id: number;
  title: string;
  message: string;
  type: 'success' | 'error' | 'warn' | 'info';
  icon: 'fa fa-check-circle' | 'fa fa-exclamation-circle' | 'fa fa-info-circle';
}

export const TOAST_ANIMATION = [
  trigger('toastAnimation', [
    transition(':enter', [
      animate(
        '300ms linear',
        keyframes([
          style({ opacity: 0, transform: 'translateX(calc(100% + 32px))', offset: 0 }),
          style({ opacity: 1, transform: 'translateX(0)', offset: 1 })
        ])
      )
    ]),
    transition(':leave', [
      animate(
        '300ms linear',
        keyframes([
          style({ opacity: 1, transform: 'translateX(0)', offset: 0 }),
          style({ opacity: 0, transform: 'translateX(calc(100% + 32px))', offset: 1 })
        ])
      )
    ])
  ])
];