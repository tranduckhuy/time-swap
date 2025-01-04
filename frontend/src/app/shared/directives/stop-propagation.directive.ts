import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appStopPropagation]',
  standalone: true
})
export class StopPropagationDirective {
  @HostListener('click', ['$event'])
  onClick(event: Event): boolean {
    event.stopPropagation();
    return false;
  }

  @HostListener('keydown', ['$event'])
  onKeydown(event: KeyboardEvent): boolean {
    if (event.key === ' ') {
      event.stopPropagation(); // Chặn space nếu muốn
    }
    return true;
  }
}
