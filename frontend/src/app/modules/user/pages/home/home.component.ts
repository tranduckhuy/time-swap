import { Component } from '@angular/core';

import { ToastComponent } from "../../../../shared/components/toast/toast.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ToastComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
}
