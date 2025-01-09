import { Component, Input, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-empty-job',
  standalone: true,
  imports: [],
  templateUrl: './empty-job.component.html',
  styleUrls: ['./empty-job.component.css'], 
})
export class EmptyJobComponent {
  private router = inject(Router);

  @Input() title!: string;
  @Input() desc!: string;
  @Input() btn!: { msg: string; link: string };

  returnToShop(): void {
    this.router.navigate([this.btn.link]);
  }
}
