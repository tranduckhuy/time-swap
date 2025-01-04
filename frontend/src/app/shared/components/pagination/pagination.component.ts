import { Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  currentPage = input<number>(1); 
  totalJobs = input<number>(1); 
  limit = input<number>(1); 
  pageChange = output<number>();
  
  totalPages = computed<number>(() => Math.round(Math.ceil(this.totalJobs() / this.limit())))

  // * Get visible page numbers (limit to 3)
  get visiblePages(): number[] {
    const pages: number[] = [];

    if (this.totalJobs() === 0) {
      pages.push(1);
    } else {
      const start = Math.max(1, this.currentPage() - 1);
      const end = Math.min(this.totalPages(), this.currentPage() + 1);
  
      for (let i = start; i <= end; i++) {
        pages.push(i);
      }
    }
    return pages;
  }

  // * Emit the new page number when clicked
  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages() && this.currentPage() !== page) {
      this.pageChange.emit(page);
    }
  }
}
