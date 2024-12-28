import { Component, input } from '@angular/core';

@Component({
  selector: 'app-button-icon',
  standalone: true,
  imports: [],
  templateUrl: './button-with-icon.component.html',
  styleUrl: './button-with-icon.component.css'
})
export class ButtonWithIconComponent {
  title = input.required<string>();
  icon = input.required<string>();
}