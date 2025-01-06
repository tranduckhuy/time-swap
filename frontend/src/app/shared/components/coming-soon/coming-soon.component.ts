import { Component, DestroyRef, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { PreLoaderComponent } from "../pre-loader/pre-loader.component";

@Component({
  selector: 'app-coming-soon',
  standalone: true,
  imports: [RouterLink, PreLoaderComponent],
  templateUrl: './coming-soon.component.html',
  styleUrl: './coming-soon.component.css'
})
export class ComingSoonComponent {
  isLoading = signal<boolean>(true);

  private destroyRef = inject(DestroyRef);

  ngOnInit(): void {
    const timeOutId = setTimeout(() => this.isLoading.set(false), 1000);
    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }
}
