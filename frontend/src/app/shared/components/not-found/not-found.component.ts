import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { PreLoaderComponent } from "../pre-loader/pre-loader.component";

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink, PreLoaderComponent],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css'
})
export class NotFoundComponent implements OnInit {
  isLoading = signal<boolean>(true);

  private destroyRef = inject(DestroyRef);

  ngOnInit(): void {
    const timeOutId = setTimeout(() => this.isLoading.set(false), 1000);
    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }
}
